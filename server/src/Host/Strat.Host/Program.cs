using Strat.Host;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseAutofac();
builder.Services.ReplaceConfiguration(builder.Configuration);
builder.Services.AddApplication<StratHostModule>();

var app = builder.Build();
app.InitializeApplication();
app.Run();

