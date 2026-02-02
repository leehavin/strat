<div align="center">

# 🚀 Strat

### Enterprise Multi-Platform Management System

[![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)
[![Vue](https://img.shields.io/badge/Vue-3.5-4FC08D?style=flat-square&logo=vue.js)](https://vuejs.org/)
[![TypeScript](https://img.shields.io/badge/TypeScript-5.6-3178C6?style=flat-square&logo=typescript)](https://www.typescriptlang.org/)
[![Avalonia](https://img.shields.io/badge/Avalonia-11.3-8B44AC?style=flat-square)](https://avaloniaui.net/)
[![License](https://img.shields.io/badge/License-Mulan%20PSL%20v2-blue?style=flat-square)](LICENSE)

[简体中文](README.md) | **English**

<p>
  <strong>One Codebase, All Platforms</strong><br>
  Backend · Web Admin · Mobile/Mini Programs · Desktop
</p>

---

[Features](#-features) •
[Quick Start](#-quick-start) •
[Architecture](#-project-structure) •
[Tech Stack](#️-tech-stack) •
[Documentation](#-documentation) •
[Contributing](#-contributing)

</div>

## ✨ Features

<table>
<tr>
<td width="50%">

### 🎯 Full-Stack Solution
- 🖥️ **Backend** - ASP.NET Core 10 + ABP Framework
- 🌐 **Web** - Vben Admin 5.x (Vue 3)
- 📱 **Mobile** - uni-app Cross-platform
- 💻 **Desktop** - Avalonia UI Cross-platform

</td>
<td width="50%">

### 🏗️ Enterprise Architecture
- 📦 Modular Design, Load on Demand
- 🔐 Complete RBAC Permission Management
- 🔄 Real-time Communication (SignalR)
- 🌍 Multi-language i18n Support

</td>
</tr>
<tr>
<td width="50%">

### 🎨 Modern Tech Stack
- ⚡ Vite 6.0 Lightning Fast Build
- 🎭 Multiple UI Frameworks (Ant Design Vue / Element Plus / Naive UI)
- 📝 Full TypeScript Coverage
- 🧪 Comprehensive Testing Support

</td>
<td width="50%">

### 🚀 Ready to Use
- 📊 Rich Business Components
- 🔧 Complete Development Toolchain
- 📖 Detailed Documentation
- 🐳 Docker Container Support

</td>
</tr>
</table>

## 📁 Project Structure

```
strat/
├── 📂 server/          # 🖥️  Backend - ASP.NET Core 10 + ABP Framework
│   ├── src/
│   │   ├── Host/              # Application Entry
│   │   ├── Infrastructure/    # Infrastructure Layer
│   │   └── Modules/           # Business Modules (Identity/System/Workflow)
│   └── tests/                 # Unit Tests
│
├── 📂 vben/            # 🌐  Web Admin - Vben Admin 5.x (Monorepo)
│   ├── apps/                  # Main Apps (antd/ele/naive)
│   ├── packages/              # Shared Packages
│   └── internal/              # Internal Tools
│
├── 📂 uni/             # 📱  Mobile - uni-app (Vue 3)
│   └── src/
│       ├── pages/             # Page Components
│       ├── store/             # State Management
│       └── api/               # API Layer
│
└── 📂 wpf/             # 💻  Desktop - Avalonia UI
    └── src/
        ├── 01.Infrastructure/ # Infrastructure Layer
        ├── 02.Modules/        # Feature Modules
        ├── 03.Presentation/   # Presentation Layer
        └── 04.Hosts/          # App Hosts (Desktop/Browser)
```

## 🛠️ Tech Stack

### Backend (Server)

| Technology | Version | Description |
|:---|:---:|:---|
| ![.NET](https://img.shields.io/badge/-.NET%2010-512BD4?style=flat-square&logo=dotnet&logoColor=white) | 10.0 | Runtime Framework |
| ![ABP](https://img.shields.io/badge/-ABP%20Framework-EF5350?style=flat-square) | 10.0.1 | Application Framework |
| ![SqlSugar](https://img.shields.io/badge/-SqlSugar-FF6B35?style=flat-square) | 5.1.4 | ORM Framework |
| ![JWT](https://img.shields.io/badge/-JWT-000000?style=flat-square&logo=jsonwebtokens) | - | Authentication |
| ![Swagger](https://img.shields.io/badge/-Swagger-85EA2D?style=flat-square&logo=swagger&logoColor=black) | - | API Documentation |
| ![SignalR](https://img.shields.io/badge/-SignalR-512BD4?style=flat-square) | - | Real-time Communication |

### Web Admin (Vben)

| Technology | Version | Description |
|:---|:---:|:---|
| ![Vue](https://img.shields.io/badge/-Vue%203-4FC08D?style=flat-square&logo=vue.js&logoColor=white) | 3.5 | Frontend Framework |
| ![Vben](https://img.shields.io/badge/-Vben%20Admin-646CFF?style=flat-square) | 5.5.1 | Admin Framework |
| ![Vite](https://img.shields.io/badge/-Vite-646CFF?style=flat-square&logo=vite&logoColor=white) | 6.0 | Build Tool |
| ![TypeScript](https://img.shields.io/badge/-TypeScript-3178C6?style=flat-square&logo=typescript&logoColor=white) | 5.6 | Type System |
| ![Tailwind](https://img.shields.io/badge/-Tailwind%20CSS-06B6D4?style=flat-square&logo=tailwindcss&logoColor=white) | 3.4 | CSS Framework |
| ![Pinia](https://img.shields.io/badge/-Pinia-F7D336?style=flat-square) | 2.2 | State Management |

### Mobile (Uni)

| Technology | Version | Description |
|:---|:---:|:---|
| ![uni-app](https://img.shields.io/badge/-uni--app-2B9939?style=flat-square) | 3.0 | Cross-platform Framework |
| ![Vue](https://img.shields.io/badge/-Vue%203-4FC08D?style=flat-square&logo=vue.js&logoColor=white) | 3.4 | Frontend Framework |
| ![Vite](https://img.shields.io/badge/-Vite-646CFF?style=flat-square&logo=vite&logoColor=white) | 5.2 | Build Tool |
| ![Pinia](https://img.shields.io/badge/-Pinia-F7D336?style=flat-square) | 3.0 | State Management |

### Desktop (WPF)

| Technology | Version | Description |
|:---|:---:|:---|
| ![.NET](https://img.shields.io/badge/-.NET%208-512BD4?style=flat-square&logo=dotnet&logoColor=white) | 8.0 | Runtime Framework |
| ![Avalonia](https://img.shields.io/badge/-Avalonia%20UI-8B44AC?style=flat-square) | 11.3 | UI Framework |
| ![Prism](https://img.shields.io/badge/-Prism-FF6B35?style=flat-square) | - | MVVM Framework |

## 🚀 Quick Start

### Requirements

| Environment | Version |
|:---|:---|
| .NET SDK | >= 10.0.102 (Server) / >= 8.0.100 (WPF) |
| Node.js | >= 20.10.0 |
| pnpm | >= 9.12.0 |
| Database | SQL Server / MySQL |

### One-Click Start

#### 1️⃣ Start Backend

```powershell
cd server
dotnet restore
dotnet run --project src/Host/Strat.Host

# ✅ API running at http://localhost:5062
# ✅ Swagger UI: http://localhost:5062/swagger
```

#### 2️⃣ Start Web Admin

```bash
cd vben
pnpm install
pnpm dev:antd

# ✅ Running at http://localhost:5666
```

#### 3️⃣ Start Mobile (H5)

```bash
cd uni
pnpm install
pnpm dev:h5

# ✅ Running at http://localhost:5173
```

#### 4️⃣ Start Desktop

```powershell
cd wpf
.\build.ps1
dotnet run --project src/04.Hosts/Strat.Desktop/Strat.Desktop.csproj
```

## 📖 Documentation

### 🖥️ Server - Backend

<details>
<summary><b>Click to expand details</b></summary>

#### Project Architecture

Follows **DDD (Domain-Driven Design)** layered architecture, built with ABP Framework:

```
server/src/
├── Host/                    # 🚪 Application Entry Layer
│   └── Strat.Host/         #    API Host
│
├── Infrastructure/          # 🏗️ Infrastructure Layer
│   ├── Strat.Infrastructure/#    Persistence, Cache, External Services
│   └── Strat.Shared/       #    Shared Components, Utilities
│
└── Modules/                 # 📦 Business Module Layer
    ├── Strat.Identity.*/   #    Identity Module
    ├── Strat.System.*/     #    System Management Module
    └── Strat.Workflow.*/   #    Workflow Module
```

#### Feature Modules

| Module | Description |
|:---|:---|
| **Identity** | User Management, Role Management, Permission Management, OAuth2 Login (Gitee/GitHub) |
| **System** | System Configuration, Data Dictionary, API Management, Notifications, Organization Structure |
| **Workflow** | Workflow Definition, Workflow Instances, Process Approval |

#### Core Features

- ✅ **JWT Authentication** - Secure token-based authentication
- ✅ **RBAC Permissions** - Role-based access control
- ✅ **Audit Logging** - Complete operation audit trail
- ✅ **Soft Delete** - Safe data deletion mechanism
- ✅ **Multi-tenancy** - SaaS multi-tenant architecture support
- ✅ **Real-time Communication** - SignalR real-time messaging

#### Configuration

Edit `src/Host/Strat.Host/appsettings.json`:

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

### 🌐 Vben - Web Admin

<details>
<summary><b>Click to expand details</b></summary>

#### Monorepo Architecture

Modern Monorepo architecture based on **pnpm workspace** + **Turbo**:

```
vben/
├── apps/                    # 📱 Main Applications
│   ├── web-antd/           #    Ant Design Vue Version
│   ├── web-ele/            #    Element Plus Version
│   ├── web-naive/          #    Naive UI Version
│   └── backend-mock/       #    Mock API Server
│
├── packages/               # 📦 Shared Packages
│   ├── @core/              #    Core Framework
│   │   ├── base/           #    Base Utilities
│   │   ├── composables/    #    Composables
│   │   ├── preferences/    #    Preferences
│   │   └── ui-kit/         #    UI Components
│   ├── effects/            #    Feature Modules
│   │   ├── access/         #    Access Control
│   │   ├── layouts/        #    Layout Components
│   │   └── request/        #    HTTP Requests
│   ├── locales/            #    Internationalization
│   └── stores/             #    State Management
│
└── internal/               # 🔧 Internal Tools
    ├── lint-configs/       #    Code Standards
    ├── tailwind-config/    #    Tailwind Config
    └── vite-config/        #    Vite Config
```

#### Multiple UI Framework Support

| App | UI Framework | Features |
|:---|:---|:---|
| `web-antd` | Ant Design Vue 4.2 | Enterprise design system, comprehensive features |
| `web-ele` | Element Plus 2.9 | Clean and elegant, easy to learn |
| `web-naive` | Naive UI 2.40 | Modern design, excellent performance |

#### Development Commands

```bash
# Development
pnpm dev:antd      # Ant Design Vue
pnpm dev:ele       # Element Plus
pnpm dev:naive     # Naive UI

# Build
pnpm build:antd    # Build Ant Design Vue version
pnpm build:analyze # Build with bundle analysis

# Code Quality
pnpm lint          # Lint
pnpm format        # Format code
pnpm check:type    # Type check
pnpm test:unit     # Unit tests
pnpm test:e2e      # E2E tests
```

</details>

---

### 📱 Uni - Mobile

<details>
<summary><b>Click to expand details</b></summary>

#### Cross-Platform Support

| Platform | Dev Command | Build Command |
|:---|:---|:---|
| H5 | `pnpm dev:h5` | `pnpm build:h5` |
| WeChat Mini Program | `pnpm dev:mp-weixin` | `pnpm build:mp-weixin` |
| Alipay Mini Program | `pnpm dev:mp-alipay` | `pnpm build:mp-alipay` |
| Baidu Mini Program | `pnpm dev:mp-baidu` | `pnpm build:mp-baidu` |
| QQ Mini Program | `pnpm dev:mp-qq` | `pnpm build:mp-qq` |
| TikTok Mini Program | `pnpm dev:mp-toutiao` | `pnpm build:mp-toutiao` |

#### Project Structure

```
uni/src/
├── api/             # 🔌 API Layer
│   └── auth.ts     #    Authentication APIs
│
├── pages/           # 📄 Page Components
│   ├── index/      #    Home (Warehouse Management Entry)
│   ├── login/      #    Login Page
│   └── user/       #    User Center
│
├── store/           # 🗃️ State Management
│   ├── user.ts     #    User State
│   └── online-user.ts # Online Status
│
├── utils/           # 🔧 Utilities
│   ├── http.ts     #    HTTP Client
│   ├── auth-guard.ts #  Route Guard
│   └── signalr.ts  #    SignalR Connection
│
└── directives/      # 📌 Vue Directives
    └── permission.ts #  Permission Directive
```

#### Core Features

- 📦 **Warehouse Management** - Inbound, Outbound, Product Library, Inventory Check, Stock Transfer
- 📊 **Statistical Reports** - Inbound/Outbound Reports, Product Statistics, Warehouse Statistics
- 🔐 **User Authentication** - Login, Permission Control, Token Management
- 📡 **Real-time Sync** - SignalR Online Status Sync

</details>

---

### 💻 WPF - Desktop

<details>
<summary><b>Click to expand details</b></summary>

#### Layered Architecture

```
wpf/src/
├── 01.Infrastructure/       # 🏗️ Infrastructure Layer
│   ├── Strat.Shared/       #    Shared Components (HTTP, Dialogs, Events)
│   └── Strat.Infrastructure/#    Business Service Implementation
│
├── 02.Modules/             # 📦 Feature Modules
│   ├── Strat.Module.Dashboard/  # Dashboard Module
│   ├── Strat.Module.Identity/   # Identity Module
│   └── Strat.Module.System/     # System Module
│
├── 03.Presentation/        # 🎨 Presentation Layer
│   ├── Strat.UI.Base/      #    Main App Shell
│   └── Strat.Themes/       #    Themes, Fonts, i18n
│
└── 04.Hosts/               # 🚪 Application Hosts
    ├── Strat.Desktop/      #    Desktop App Entry
    └── Strat.Browser/      #    Browser (WASM) Entry
```

#### Multi-Platform Support

| Platform | Description |
|:---|:---|
| Windows | Native Windows Desktop App |
| macOS | Native macOS Desktop App |
| Linux | Native Linux Desktop App |
| Browser | WebAssembly Browser App |

#### Multi-Language Support

Supports **8 languages**:
- 🇨🇳 Simplified Chinese (zh-CN)
- 🇹🇼 Traditional Chinese (zh-TW)
- 🇺🇸 English (en-US)
- 🇯🇵 Japanese (ja-JP)
- 🇰🇷 Korean (ko-KR)
- 🇫🇷 French (fr-FR)
- 🇩🇪 German (de-DE)
- 🇪🇸 Spanish (es-ES)

#### Build Commands

```powershell
# Development Build
.\build.ps1

# Publish Desktop
.\build.ps1 -Target Desktop -Publish

# Publish Browser
.\build.ps1 -Target Browser -Publish

# Publish All Platforms
.\build.ps1 -Target All -Publish
```

</details>

## 📋 API Reference

All clients communicate with the backend through unified RESTful APIs:

### Authentication APIs

| Method | Endpoint | Description |
|:---|:---|:---|
| `POST` | `/api/v1/auth/login` | User login, get Token |
| `GET` | `/api/v1/auth/current-user` | Get current user info |
| `GET` | `/api/v1/auth/permissions` | Get user permission list |
| `GET` | `/api/v1/auth/routers` | Get user menu routes |
| `PUT` | `/api/v1/auth/update-profile` | Update user profile |

### Authentication

```http
Authorization: Bearer <your-jwt-token>
```

### Real-time Communication

```
SignalR Hub: ws://localhost:5062/signalr-hubs/online-user
```

## 🤝 Contributing

Contributions are welcome! Please read the following guide:

1. **Fork** this repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Submit a **Pull Request**

## 📄 License

This project is licensed under [Mulan PSL v2](LICENSE).

```
Copyright (c) 2024 Strat
Licensed under Mulan PSL v2.
```

## 🙏 Acknowledgments

Thanks to the following open source projects:

- [ABP Framework](https://abp.io/) - Application Framework
- [Vben Admin](https://vben.pro/) - Vue Admin Template
- [uni-app](https://uniapp.dcloud.net.cn/) - Cross-platform Development Framework
- [Avalonia UI](https://avaloniaui.net/) - Cross-platform UI Framework

---

<div align="center">

**If this project helps you, please give it a ⭐ Star!**

Made with ❤️ by Strat Team

</div>
