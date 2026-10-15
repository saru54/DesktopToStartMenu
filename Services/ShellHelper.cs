using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

namespace DesktopToStartMenu.Services
{
    public static class ShellHelper
    {
        private static readonly Type? WshShellType = Type.GetTypeFromProgID("WScript.Shell");

        /// <summary>
        /// 解析快捷方式的目标路径与工作目录
        /// </summary>
        public static (string targetPath, string workingDir, string arguments) ResolveShortcut(string lnkPath)
        {
            if (WshShellType == null || !File.Exists(lnkPath))
            {
                return (string.Empty, string.Empty, string.Empty);
            }

            try
            {
                dynamic? shell = Activator.CreateInstance(WshShellType);
                if (shell == null) return (string.Empty, string.Empty, string.Empty);

                dynamic shortcut = shell.CreateShortcut(lnkPath);
                string targetPath = shortcut.TargetPath ?? string.Empty;
                string workingDir = shortcut.WorkingDirectory ?? string.Empty;
                string arguments = shortcut.Arguments ?? string.Empty;
                return (targetPath, workingDir, arguments);
            }
            catch
            {
                return (string.Empty, string.Empty, string.Empty);
            }
        }

        /// <summary>
        /// 创建或覆盖快捷方式
        /// </summary>
        public static bool CreateShortcut(string shortcutPath, string targetPath, string workingDir = "", string arguments = "", string description = "")
        {
            if (WshShellType == null) return false;

            try
            {
                string? dir = Path.GetDirectoryName(shortcutPath);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                dynamic? shell = Activator.CreateInstance(WshShellType);
                if (shell == null) return false;

                dynamic shortcut = shell.CreateShortcut(shortcutPath);
                shortcut.TargetPath = targetPath;
                shortcut.WorkingDirectory = string.IsNullOrEmpty(workingDir) && File.Exists(targetPath)
                    ? Path.GetDirectoryName(targetPath) ?? string.Empty
                    : workingDir;

                if (!string.IsNullOrEmpty(arguments))
                {
                    shortcut.Arguments = arguments;
                }

                if (!string.IsNullOrEmpty(description))
                {
                    shortcut.Description = description;
                }

                shortcut.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 清理名称，自动去除 "- 快捷方式"、" - Shortcut" 以及冗余的 .exe 后缀
        /// </summary>
        public static string CleanShortcutName(string rawFileName)
        {
            string name = Path.GetFileNameWithoutExtension(rawFileName);

            // 清理常见的快捷方式后缀，如 " - 快捷方式"、"-快捷方式"、" - Shortcut"
            name = Regex.Replace(name, @"[\s\-_—－]*(快捷方式|Shortcut)$", "", RegexOptions.IgnoreCase).Trim();

            // 如果原本名称带 .exe（如 v2rayN.exe），去除 .exe
            if (name.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
            {
                name = name[..^4].Trim();
            }

            return string.IsNullOrWhiteSpace(name) ? Path.GetFileNameWithoutExtension(rawFileName) : name;
        }

        /// <summary>
        /// 提取关联图标
        /// </summary>
        public static Icon? ExtractIcon(string filePath, string targetPath)
        {
            try
            {
                // 优先从真实目标程序提取高清图标
                if (!string.IsNullOrEmpty(targetPath) && (File.Exists(targetPath) || Directory.Exists(targetPath)))
                {
                    return Icon.ExtractAssociatedIcon(targetPath);
                }

                // 否则从快捷方式本身提取
                if (File.Exists(filePath))
                {
                    return Icon.ExtractAssociatedIcon(filePath);
                }
            }
            catch
            {
                // 忽略非标准或系统保留图标异常
            }

            return null;
        }
    }
}
