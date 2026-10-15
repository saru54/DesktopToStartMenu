# DesktopToStartMenu (桌面项目同步至开始菜单工具)

专门用于解决 **“免安装绿色软件（如 v2rayN、HBuilder X 等）只放在桌面，导致 Flow Launcher、Wox 或 Windows 搜索搜不到”** 的实用小工具。

---

## 🌟 核心特性

1. **自动双桌面扫描**：自动同时扫描「当前用户桌面」（支持 `%USERPROFILE%\Desktop` 及用户自定义的重定向桌面目录）与系统「公用桌面」(`C:\Users\Public\Desktop`)。
2. **智能状态比对**：自动检测各项目在开始菜单中的存在状态，精确标记为 **“已存在”** 或 **“未添加”**，并自动提取真实高清图标。
3. **快捷智能勾选**：
   - 默认智能勾选所有「未添加项」；
   - 提供「仅选未添加项」、「全选」、「全不选」、「反选」等一键操作按钮。
4. **名称规范化清理**：自动清理 Windows 生成的 `- 快捷方式`、`- 副本`、`.exe` 冗余后缀，让 Flow Launcher 索引的名字更干净整洁。
5. **支持免环境独立运行**：提供 Self-Contained 单文件版（内嵌运行库，零环境依赖）与超轻量版（180 KB）。
6. **Flow Launcher 友好**：支持一键打开开始菜单目录查看，同步完成后自动提醒唤出 Flow Launcher 输入 `reload` 刷新缓存。

---

## 🚀 下载与启动

直接在 [Releases 最新发布页](https://github.com/saru54/DesktopToStartMenu/releases/tag/v1.0.0) 下载：
* **`DesktopToStartMenu-Standalone-win-x64.exe`**：**完全零依赖独立版（推荐）**，任何新旧 Win10 / Win11 电脑双击即开，无需安装任何 .NET 环境。
* **`DesktopToStartMenu.exe`**：超轻量版（仅 182 KB），需机器具备 .NET 8 桌面运行时。

### 方式 2：开发模式运行
```bash
cd DesktopToStartMenu
dotnet run
```

---

## 📁 目录结构
```
DesktopToStartMenu/
├── publish/
│   └── DesktopToStartMenu.exe      # 单文件便携版可执行程序
├── Models/
│   └── DesktopItem.cs              # 桌面项目数据模型
├── Services/
│   ├── ShellHelper.cs              # Windows Shell COM 与快捷方式读写工具
│   ├── ScannerService.cs           # 桌面与开始菜单扫描比对引擎
│   └── SyncService.cs              # 批量生成/同步快捷方式服务
├── Forms/
│   ├── MainForm.cs                 # 主窗口交互逻辑
│   └── MainForm.Designer.cs        # 界面布局定义
├── 启动工具.bat                     # 便捷双击启动批处理
└── DesktopToStartMenu.csproj       # 工程文件 (.NET 8 WinForms)
```
