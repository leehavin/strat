# Strat Server

基于 ASP.NET Core + ABP Framework 的后端服务。

## 技术栈

- **框架**: ASP.NET Core 10 + ABP Framework 10.0
- **ORM**: SqlSugar
- **认证**: JWT Bearer Token
- **文档**: Swagger/OpenAPI
- **日志**: Serilog
- **ID生成**: 雪花算法 (YitIdHelper)

## 项目结构

```
server/
├── src/
│   ├── Host/                    # 宿主层 - 程序入口
│   │   └── Strat.Host/
│   ├── Infrastructure/          # 基础设施层
│   │   └── Strat.Infrastructure/
│   ├── Modules/                 # 业务模块 (DDD 分层)
│   │   ├── Identity/            # 身份认证模块
│   │   ├── System/              # 系统管理模块
│   │   └── Workflow/            # 工作流模块
│   └── Shared/                  # 共享层
│       └── Strat.Shared/
├── Directory.Build.props        # 全局项目属性
├── Directory.Build.targets      # 全局构建目标
├── global.json                  # SDK 版本锁定
├── NuGet.Config                 # NuGet 源配置
└── Strat.slnx                   # 解决方案文件
```

## 模块说明

每个业务模块采用 DDD 分层：

- `Domain` - 领域实体
- `Application.Contracts` - 接口定义 + DTOs
- `Application` - 业务服务实现

## 快速开始

### 环境要求

- .NET 10 SDK
- SQL Server 或 MySQL

### 配置

1. 修改 `src/Host/Strat.Host/appsettings.json` 中的数据库连接字符串
2. 修改 JWT 密钥配置

### 运行

```bash
# 还原依赖
dotnet restore

# 运行
dotnet run --project src/Host/Strat.Host
```

### 访问

- API: http://localhost:5062
- Swagger: http://localhost:5062/swagger

## 开发规范

- 遵循 `.editorconfig` 中的代码风格规范
- 接口以 `I` 开头
- 私有字段以 `_` 开头
- 使用 file-scoped namespace

## License

MIT
