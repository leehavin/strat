using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using Strat.Host.Filters;
using Strat.Host.Middlewares;
using Strat.Identity.Application;
using Strat.System.Application;
using Strat.Workflow.Application;
using Strat.Infrastructure;
using Strat.Infrastructure.Persistence;
using Strat.Shared.Constants;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.AntiForgery;
using Volo.Abp.Autofac;
using Volo.Abp.Json;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using Volo.Abp.Timing;

namespace Strat.Host;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpSwashbuckleModule),
    typeof(StratInfrastructureModule),
    typeof(StratIdentityApplicationModule),
    typeof(StratSystemApplicationModule),
    typeof(StratWorkflowApplicationModule)
)]
public class StratHostModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        var env = context.Services.GetHostingEnvironment();

        // 配置控制器
        ConfigureControllers(context);

        // 配置认证
        ConfigureAuthentication(context, configuration);

        // 配置 Swagger
        ConfigureSwagger(context);

        // 配置跨域
        ConfigureCors(context, configuration);

        // 配置时区
        Configure<AbpClockOptions>(options => options.Kind = DateTimeKind.Local);

        // 配置 JSON 日期格式
        Configure<AbpJsonOptions>(options => options.OutputDateTimeFormat = "yyyy-MM-dd HH:mm:ss");

        // 配置防伪令牌
        Configure<AbpAntiForgeryOptions>(options => options.AutoValidate = false);

        // 配置 Serilog
        ConfigureSerilog(context);

        // 配置 FluentValidation
        context.Services.AddValidatorsFromAssemblyContaining<StratIdentityApplicationModule>();
    }

    private void ConfigureControllers(ServiceConfigurationContext context)
    {
        context.Services.AddControllers(options =>
        {
            options.Filters.Add<GlobalExceptionFilter>();
            options.Filters.Add<ResultWrapperFilter>();
            options.Filters.Add<ValidationFilter>();
        });

        // 禁用默认的验证错误响应，改用 ValidationFilter
        context.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
        });

        Configure<AbpAspNetCoreMvcOptions>(options =>
        {
            // 企业级写法：批量配置所有 Strat 模块的动态 API 路由
            var applicationAssemblies = new[]
            {
                typeof(StratIdentityApplicationModule).Assembly,
                typeof(StratSystemApplicationModule).Assembly,
                typeof(StratWorkflowApplicationModule).Assembly
            };

            foreach (var assembly in applicationAssemblies)
            {
                options.ConventionalControllers.Create(assembly, setting =>
                {
                    // 将默认的 /api/app 修改为 /api/v1
                    setting.RootPath = "v1";
                });
            }
        });
    }

    private void ConfigureAuthentication(ServiceConfigurationContext context, IConfiguration configuration)
    {
        var secretKey = configuration["JwtOptions:SecretKey"];
        if (string.IsNullOrEmpty(secretKey))
        {
            throw new UserFriendlyException("未配置 JwtOptions:SecretKey，请在 appsettings.json 或环境变量中配置。");
        }

        var key = Encoding.UTF8.GetBytes(secretKey);

        context.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.FromSeconds(10),
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = ctx =>
                    {
                        var accessToken = ctx.Request.Query["access_token"];
                        var path = ctx.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/signalr-hubs"))
                        {
                            ctx.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
    }

    private void ConfigureSwagger(ServiceConfigurationContext context)
    {
        context.Services.AddAbpSwaggerGen(options =>
        {
            options.SwaggerDoc(ApiGroupConst.System, new OpenApiInfo { Title = "系统服务", Version = "v1" });
            options.SwaggerDoc(ApiGroupConst.Workflow, new OpenApiInfo { Title = "工作流服务", Version = "v1" });

            options.CustomSchemaIds(type => type.FullName);
            options.HideAbpEndpoints();

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme.",
                BearerFormat = "JWT",
                Scheme = "bearer",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }

    private void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
    {
        var origins = configuration["App:CorsOrigins"]?
            .Split(",", StringSplitOptions.RemoveEmptyEntries)
            .Select(o => o.TrimEnd('/'))
            .ToArray() ?? [];

        context.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                    .WithOrigins(origins)
                    .WithExposedHeaders("accesstoken")
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });
    }

    private void ConfigureSerilog(ServiceConfigurationContext context)
    {
        var consoleTemplate = "[{Timestamp:HH:mm:ss}] [{Level:u3}] [{TraceId}] {Message:lj}{NewLine}{Exception}";
        var fileTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss}] [{Level:u3}] [{TraceId}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}";

        context.Services.AddSerilog(config =>
        {
            config.MinimumLevel.Override("Microsoft", LogEventLevel.Warning);
            config.Enrich.FromLogContext();
            config.Enrich.WithProperty("TraceId", "");
            config.WriteTo.Console(outputTemplate: consoleTemplate);
            config.WriteTo.File(
                $"{AppContext.BaseDirectory}/logs/log-.txt",
                LogEventLevel.Warning,
                fileTemplate,
                rollingInterval: RollingInterval.Day,
                shared: true);
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint($"/swagger/{ApiGroupConst.System}/swagger.json", "系统服务");
                options.SwaggerEndpoint($"/swagger/{ApiGroupConst.Workflow}/swagger.json", "工作流服务");
            });
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors();
        app.UseTraceId();
        app.UseAuditLog();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseConfiguredEndpoints(options => options.MapControllers().RequireAuthorization());
    }
}

