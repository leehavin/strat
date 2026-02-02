<div align="center">

# 🚀 Strat

### 企业级多端管理系统解决方案

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)
[![Vue](https://img.shields.io/badge/Vue-3.5-4FC08D?style=flat-square&logo=vue.js)](https://vuejs.org/)
[![TypeScript](https://img.shields.io/badge/TypeScript-5.6-3178C6?style=flat-square&logo=typescript)](https://www.typescriptlang.org/)
[![Avalonia](https://img.shields.io/badge/Avalonia-11.3-8B44AC?style=flat-square)](https://avaloniaui.net/)
[![License](https://img.shields.io/badge/License-Mulan%20PSL%20v2-blue?style=flat-square)](LICENSE)

**简体中文** | [English](README.en.md)

<p>
  <strong>一套代码，全端覆盖</strong><br>
  服务端 · Web管理后台 · 移动端/小程序 · 桌面端
</p>

---

[特性](#-特性) •
[快速开始](#-快速开始) •
[项目架构](#-项目架构) •
[技术栈](#-技术栈) •
[文档](#-文档) •
[贡献](#-贡献)

</div>

## ✨ 特性

<table>
<tr>
<td width="50%">

### 🎯 全栈解决方案
- 🖥️ **服务端** - ASP.NET Core 10 + ABP Framework
- 🌐 **Web端** - Vben Admin 5.x (Vue 3)
- 📱 **移动端** - uni-app 跨平台应用
- 💻 **桌面端** - Avalonia UI 跨平台客户端

</td>
<td width="50%">

### 🏗️ 企业级架构
- 📦 模块化设计，按需加载
- 🔐 完善的 RBAC 权限管理
- 🔄 实时通讯 (SignalR)
- 🌍 多语言国际化支持

</td>
</tr>
<tr>
<td width="50%">

### 🎨 现代化技术栈
- ⚡ Vite 6.0 极速构建
- 🎭 多 UI 框架可选 (Ant Design Vue / Element Plus / Naive UI)
- 📝 TypeScript 全覆盖
- 🧪 完整的测试支持

</td>
<td width="50%">

### 🚀 开箱即用
- 📊 丰富的业务组件
- 🔧 完善的开发工具链
- 📖 详尽的文档说明
- 🐳 Docker 容器化支持

</td>
</tr>
</table>

## 📁 项目结构

```
strat/
├── 📂 server/          # 🖥️  服务端 - ASP.NET Core 10 + ABP Framework
│   ├── src/
│   │   ├── Host/              # 应用入口
│   │   ├── Infrastructure/    # 基础设施层
│   │   └── Modules/           # 业务模块 (Identity/System/Workflow)
│   └── tests/                 # 单元测试
│
├── 📂 vben/            # 🌐  Web管理后台 - Vben Admin 5.x (Monorepo)
│   ├── apps/                  # 主应用 (antd/ele/naive)
│   ├── packages/              # 共享包
│   └── internal/              # 内部工具
│
├── 📂 uni/             # 📱  移动端 - uni-app (Vue 3)
│   └── src/
│       ├── pages/             # 页面组件
│       ├── store/             # 状态管理
│       └── api/               # API 接口
│
└── 📂 wpf/             # 💻  桌面端 - Avalonia UI
    └── src/
        ├── 01.Infrastructure/ # 基础设施层
        ├── 02.Modules/        # 功能模块
        ├── 03.Presentation/   # 表现层
        └── 04.Hosts/          # 应用宿主 (Desktop/Browser)
```

## 🛠️ 技术栈

### 服务端 (Server)

| 技术 | 版本 | 说明 |
|:---|:---:|:---|
| ![.NET](https://img.shields.io/badge/-.NET%2010-512BD4?style=flat-square&logo=dotnet&logoColor=white) | 10.0 | 运行时框架 |
| ![ABP](https://img.shields.io/badge/-ABP%20Framework-EF5350?style=flat-square) | 10.0.1 | 应用框架 |
| ![SqlSugar](https://img.shields.io/badge/-SqlSugar-FF6B35?style=flat-square) | 5.1.4 | ORM 框架 |
| ![JWT](https://img.shields.io/badge/-JWT-000000?style=flat-square&logo=jsonwebtokens) | - | 身份认证 |
| ![Swagger](https://img.shields.io/badge/-Swagger-85EA2D?style=flat-square&logo=swagger&logoColor=black) | - | API 文档 |
| ![SignalR](https://img.shields.io/badge/-SignalR-512BD4?style=flat-square) | - | 实时通讯 |

### Web 管理后台 (Vben)

| 技术 | 版本 | 说明 |
|:---|:---:|:---|
| ![Vue](https://img.shields.io/badge/-Vue%203-4FC08D?style=flat-square&logo=vue.js&logoColor=white) | 3.5 | 前端框架 |
| ![Vben](https://img.shields.io/badge/-Vben%20Admin-646CFF?style=flat-square) | 5.5.1 | 后台框架 |
| ![Vite](https://img.shields.io/badge/-Vite-646CFF?style=flat-square&logo=vite&logoColor=white) | 6.0 | 构建工具 |
| ![TypeScript](https://img.shields.io/badge/-TypeScript-3178C6?style=flat-square&logo=typescript&logoColor=white) | 5.6 | 类型系统 |
| ![Tailwind](https://img.shields.io/badge/-Tailwind%20CSS-06B6D4?style=flat-square&logo=tailwindcss&logoColor=white) | 3.4 | CSS 框架 |
| ![Pinia](https://img.shields.io/badge/-Pinia-F7D336?style=flat-square) | 2.2 | 状态管理 |

### 移动端 (Uni)

| 技术 | 版本 | 说明 |
|:---|:---:|:---|
| ![uni-app](https://img.shields.io/badge/-uni--app-2B9939?style=flat-square) | 3.0 | 跨平台框架 |
| ![Vue](https://img.shields.io/badge/-Vue%203-4FC08D?style=flat-square&logo=vue.js&logoColor=white) | 3.4 | 前端框架 |
| ![Vite](https://img.shields.io/badge/-Vite-646CFF?style=flat-square&logo=vite&logoColor=white) | 5.2 | 构建工具 |
| ![Pinia](https://img.shields.io/badge/-Pinia-F7D336?style=flat-square) | 3.0 | 状态管理 |

### 桌面端 (WPF)

| 技术 | 版本 | 说明 |
|:---|:---:|:---|
| ![.NET](https://img.shields.io/badge/-.NET%208-512BD4?style=flat-square&logo=dotnet&logoColor=white) | 8.0 | 运行时框架 |
| ![Avalonia](https://img.shields.io/badge/-Avalonia%20UI-8B44AC?style=flat-square) | 11.3 | UI 框架 |
| ![Prism](https://img.shields.io/badge/-Prism-FF6B35?style=flat-square) | - | MVVM 框架 |

## 🚀 快速开始

### 环境要求

| 环境 | 版本要求 |
|:---|:---|
| .NET SDK | >= 10.0.102 (Server) / >= 8.0.100 (WPF) |
| Node.js | >= 20.10.0 |
| pnpm | >= 9.12.0 |
| 数据库 | SQL Server / MySQL |

### 一键启动

#### 1️⃣ 启动服务端

```powershell
cd server
dotnet restore
dotnet run --project src/Host/Strat.Host

# ✅ API 运行在 http://localhost:5062
# ✅ Swagger UI: http://localhost:5062/swagger
```

#### 2️⃣ 启动 Web 管理后台

```bash
cd vben
pnpm install
pnpm dev:antd

# ✅ 运行在 http://localhost:5666
```

#### 3️⃣ 启动移动端 (H5)

```bash
cd uni
pnpm install
pnpm dev:h5

# ✅ 运行在 http://localhost:5173
```

#### 4️⃣ 启动桌面端

```powershell
cd wpf
.\build.ps1
dotnet run --project src/04.Hosts/Strat.Desktop/Strat.Desktop.csproj
```

## 📖 详细文档

### 🖥️ Server - 服务端

<details>
<summary><b>点击展开详细说明</b></summary>

#### 项目架构

采用 **DDD (领域驱动设计)** 分层架构，基于 ABP Framework 构建：

```
server/src/
├── Host/                    # 🚪 应用入口层
│   └── Strat.Host/         #    API 宿主程序
│
├── Infrastructure/          # 🏗️ 基础设施层
│   ├── Strat.Infrastructure/#    持久化、缓存、外部服务
│   └── Strat.Shared/       #    共享组件、工具类
│
└── Modules/                 # 📦 业务模块层
    ├── Strat.Identity.*/   #    身份认证模块
    ├── Strat.System.*/     #    系统管理模块
    └── Strat.Workflow.*/   #    工作流模块
```

#### 功能模块

| 模块 | 功能描述 |
|:---|:---|
| **Identity** | 用户管理、角色管理、权限管理、OAuth2 登录 (Gitee/GitHub) |
| **System** | 系统配置、数据字典、接口管理、通知公告、组织架构 |
| **Workflow** | 工作流定义、工作流实例、流程审批 |

#### 核心特性

- ✅ **JWT 认证** - 安全的 Token 认证机制
- ✅ **RBAC 权限** - 基于角色的访问控制
- ✅ **审计日志** - 完整的操作审计追踪
- ✅ **软删除** - 数据安全删除机制
- ✅ **多租户** - 支持 SaaS 多租户架构
- ✅ **实时通讯** - SignalR 实时消息推送

#### 配置说明

编辑 `src/Host/Strat.Host/appsettings.json`：

```json
{
  "ConnectionOptions": {
    "ConnectionString": "Server=localhost;Database=Strat;Trusted_Connection=True;"
  },
  "JwtOptions": {
    "SecretKey": "your-256-bit-secret-key-here",
    "Issuer": "Strat",
    "Audience": "Strat",
    "ExpireMinutes": 1440
  },
  "App": {
    "CorsOrigins": "http://localhost:5173,http://localhost:5666"
  }
}
```

</details>

---

### 🌐 Vben - Web 管理后台

<details>
<summary><b>点击展开详细说明</b></summary>

#### Monorepo 架构

基于 **pnpm workspace** + **Turbo** 的现代化 Monorepo 架构：

```
vben/
├── apps/                    # 📱 主应用
│   ├── web-antd/           #    Ant Design Vue 版本
│   ├── web-ele/            #    Element Plus 版本
│   ├── web-naive/          #    Naive UI 版本
│   └── backend-mock/       #    Mock API 服务器
│
├── packages/               # 📦 共享包
│   ├── @core/              #    核心框架
│   │   ├── base/           #    基础工具
│   │   ├── composables/    #    组合式函数
│   │   ├── preferences/    #    偏好设置
│   │   └── ui-kit/         #    UI 组件库
│   ├── effects/            #    功能模块
│   │   ├── access/         #    权限控制
│   │   ├── layouts/        #    布局组件
│   │   └── request/        #    HTTP 请求
│   ├── locales/            #    国际化
│   └── stores/             #    状态管理
│
└── internal/               # 🔧 内部工具
    ├── lint-configs/       #    代码规范
    ├── tailwind-config/    #    Tailwind 配置
    └── vite-config/        #    Vite 配置
```

#### 多 UI 框架支持

| 应用 | UI 框架 | 特点 |
|:---|:---|:---|
| `web-antd` | Ant Design Vue 4.2 | 企业级设计体系，功能全面 |
| `web-ele` | Element Plus 2.9 | 简洁优雅，上手容易 |
| `web-naive` | Naive UI 2.40 | 现代化设计，性能优秀 |

#### 开发命令

```bash
# 开发
pnpm dev:antd      # Ant Design Vue
pnpm dev:ele       # Element Plus
pnpm dev:naive     # Naive UI

# 构建
pnpm build:antd    # 构建 Ant Design Vue 版本
pnpm build:analyze # 构建并分析包大小

# 代码质量
pnpm lint          # 代码检查
pnpm format        # 代码格式化
pnpm check:type    # 类型检查
pnpm test:unit     # 单元测试
pnpm test:e2e      # E2E 测试
```

</details>

---

### 📱 Uni - 移动端

<details>
<summary><b>点击展开详细说明</b></summary>

#### 跨平台支持

| 平台 | 开发命令 | 构建命令 |
|:---|:---|:---|
| H5 | `pnpm dev:h5` | `pnpm build:h5` |
| 微信小程序 | `pnpm dev:mp-weixin` | `pnpm build:mp-weixin` |
| 支付宝小程序 | `pnpm dev:mp-alipay` | `pnpm build:mp-alipay` |
| 百度小程序 | `pnpm dev:mp-baidu` | `pnpm build:mp-baidu` |
| QQ 小程序 | `pnpm dev:mp-qq` | `pnpm build:mp-qq` |
| 抖音小程序 | `pnpm dev:mp-toutiao` | `pnpm build:mp-toutiao` |

#### 项目结构

```
uni/src/
├── api/             # 🔌 API 接口层
│   └── auth.ts     #    认证相关接口
│
├── pages/           # 📄 页面组件
│   ├── index/      #    首页 (仓储管理入口)
│   ├── login/      #    登录页
│   └── user/       #    用户中心
│
├── store/           # 🗃️ 状态管理
│   ├── user.ts     #    用户状态
│   └── online-user.ts # 在线状态
│
├── utils/           # 🔧 工具函数
│   ├── http.ts     #    HTTP 客户端
│   ├── auth-guard.ts #  路由守卫
│   └── signalr.ts  #    SignalR 连接
│
└── directives/      # 📌 Vue 指令
    └── permission.ts #  权限指令
```

#### 核心功能

- 📦 **仓储管理** - 入库、出库、产品库、库存盘点、库存调拨
- 📊 **统计报表** - 出入库报表、商品统计、仓库统计
- 🔐 **用户认证** - 登录、权限控制、Token 管理
- 📡 **实时同步** - SignalR 在线状态同步

</details>

---

### 💻 WPF - 桌面端

<details>
<summary><b>点击展开详细说明</b></summary>

#### 分层架构

```
wpf/src/
├── 01.Infrastructure/       # 🏗️ 基础设施层
│   ├── Strat.Shared/       #    共享组件 (HTTP、对话框、事件)
│   └── Strat.Infrastructure/#    业务服务实现
│
├── 02.Modules/             # 📦 功能模块
│   ├── Strat.Module.Dashboard/  # 仪表盘模块
│   ├── Strat.Module.Identity/   # 身份认证模块
│   └── Strat.Module.System/     # 系统管理模块
│
├── 03.Presentation/        # 🎨 表现层
│   ├── Strat.UI.Base/      #    主应用 Shell
│   └── Strat.Themes/       #    主题、字体、国际化
│
└── 04.Hosts/               # 🚪 应用宿主
    ├── Strat.Desktop/      #    桌面应用入口
    └── Strat.Browser/      #    浏览器 (WASM) 入口
```

#### 多平台支持

| 平台 | 说明 |
|:---|:---|
| Windows | 原生 Windows 桌面应用 |
| macOS | 原生 macOS 桌面应用 |
| Linux | 原生 Linux 桌面应用 |
| Browser | WebAssembly 浏览器应用 |

#### 多语言支持

支持 **8 种语言**：
- 🇨🇳 简体中文 (zh-CN)
- 🇹🇼 繁体中文 (zh-TW)
- 🇺🇸 English (en-US)
- 🇯🇵 日本語 (ja-JP)
- 🇰🇷 한국어 (ko-KR)
- 🇫🇷 Français (fr-FR)
- 🇩🇪 Deutsch (de-DE)
- 🇪🇸 Español (es-ES)

#### 构建命令

```powershell
# 开发构建
.\build.ps1

# 发布桌面版
.\build.ps1 -Target Desktop -Publish

# 发布浏览器版
.\build.ps1 -Target Browser -Publish

# 发布所有平台
.\build.ps1 -Target All -Publish
```

</details>

## 📋 API 接口

所有客户端通过统一的 RESTful API 与服务端通讯：

### 认证接口

| 方法 | 端点 | 说明 |
|:---|:---|:---|
| `POST` | `/api/v1/auth/login` | 用户登录，获取 Token |
| `GET` | `/api/v1/auth/current-user` | 获取当前用户信息 |
| `GET` | `/api/v1/auth/permissions` | 获取用户权限列表 |
| `GET` | `/api/v1/auth/routers` | 获取用户菜单路由 |
| `PUT` | `/api/v1/auth/update-profile` | 更新用户资料 |

### 认证方式

```http
Authorization: Bearer <your-jwt-token>
```

### 实时通讯

```
SignalR Hub: ws://localhost:5062/signalr-hubs/online-user
```

## 🤝 贡献

欢迎参与项目贡献！请阅读以下指南：

1. **Fork** 本仓库
2. 创建特性分支 (`git checkout -b feature/AmazingFeature`)
3. 提交更改 (`git commit -m 'Add some AmazingFeature'`)
4. 推送到分支 (`git push origin feature/AmazingFeature`)
5. 提交 **Pull Request**

## 📄 许可证

本项目基于 [Mulan PSL v2](LICENSE) 许可证开源。

```
Copyright (c) 2024 Strat
Licensed under Mulan PSL v2.
```

## 🙏 鸣谢

感谢以下开源项目：

- [ABP Framework](https://abp.io/) - 应用程序框架
- [Vben Admin](https://vben.pro/) - Vue 后台管理模板
- [uni-app](https://uniapp.dcloud.net.cn/) - 跨平台开发框架
- [Avalonia UI](https://avaloniaui.net/) - 跨平台 UI 框架

---

<div align="center">

**如果这个项目对你有帮助，请给一个 ⭐ Star 支持一下！**

Made with ❤️ by Strat Team

</div>
