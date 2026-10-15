using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DesktopToStartMenu.Models;

namespace DesktopToStartMenu.Services
{
    public class ScannerService
    {
        public static readonly string UserProgramsPath = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
        public static readonly string CommonProgramsPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonPrograms);
        public static readonly string UserDesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public static readonly string CommonDesktopPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory);

        /// <summary>
        /// 获取所有已存在的开始菜单程序项（用于去重与状态比对）
        /// </summary>
        public HashSet<string> GetStartMenuSignatures()
        {
            var signatures = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            void ScanDir(string dir)
            {
                if (!Directory.Exists(dir)) return;

                try
                {
                    var files = Directory.GetFiles(dir, "*.lnk", SearchOption.AllDirectories);
                    foreach (var file in files)
                    {
                        // 1. 记录快捷方式名称（纯名称）
                        string cleanName = ShellHelper.CleanShortcutName(Path.GetFileNameWithoutExtension(file));
                        signatures.Add("NAME:" + cleanName);

                        // 2. 记录快捷方式指向的目标真实路径
                        var (targetPath, _, _) = ShellHelper.ResolveShortcut(file);
                        if (!string.IsNullOrWhiteSpace(targetPath))
                        {
                            try
                            {
                                string fullTarget = Path.GetFullPath(targetPath);
                                signatures.Add("TARGET:" + fullTarget);
                            }
                            catch
                            {
                                signatures.Add("TARGET:" + targetPath);
                            }
                        }
                    }
                }
                catch
                {
                    // 权限或访问异常安全跳过
                }
            }

            ScanDir(UserProgramsPath);
            ScanDir(CommonProgramsPath);

            return signatures;
        }

        /// <summary>
        /// 扫描用户桌面和公共桌面上的所有可执行项、快捷方式
        /// </summary>
        public List<DesktopItem> ScanDesktopItems()
        {
            var result = new List<DesktopItem>();
            var startMenuSignatures = GetStartMenuSignatures();
            var processedPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            var desktopDirs = new[] { UserDesktopPath, CommonDesktopPath }
                .Where(d => !string.IsNullOrEmpty(d) && Directory.Exists(d))
                .Distinct(StringComparer.OrdinalIgnoreCase);

            foreach (var desktopDir in desktopDirs)
            {
                try
                {
                    var files = Directory.GetFiles(desktopDir);
                    foreach (var file in files)
                    {
                        string ext = Path.GetExtension(file).ToLowerInvariant();
                        if (ext != ".lnk" && ext != ".url" && ext != ".exe")
                        {
                            continue; // 过滤掉普通文档、txt、zip等，专注可执行程序和快捷方式
                        }

                        // 过滤 desktop.ini 等系统文件
                        string fileName = Path.GetFileName(file);
                        if (fileName.Equals("desktop.ini", StringComparison.OrdinalIgnoreCase))
                        {
                            continue;
                        }

                        if (processedPaths.Contains(file)) continue;
                        processedPaths.Add(file);

                        var item = CreateDesktopItem(file, ext, startMenuSignatures);
                        if (item != null)
                        {
                            result.Add(item);
                        }
                    }
                }
                catch
                {
                    // 忽略单个目录的访问异常
                }
            }

            // 按状态排序：未添加的排在前面，便于用户查阅与操作
            return result.OrderBy(x => x.IsAlreadyInStartMenu ? 1 : 0)
                         .ThenBy(x => x.CleanName)
                         .ToList();
        }

        private DesktopItem CreateDesktopItem(string filePath, string ext, HashSet<string> startMenuSignatures)
        {
            string rawName = Path.GetFileName(filePath);
            string cleanName = ShellHelper.CleanShortcutName(rawName);
            string targetPath = string.Empty;
            DesktopItemType itemType;

            if (ext == ".lnk")
            {
                itemType = DesktopItemType.Shortcut;
                var resolved = ShellHelper.ResolveShortcut(filePath);
                targetPath = resolved.targetPath;
            }
            else if (ext == ".url")
            {
                itemType = DesktopItemType.Url;
                targetPath = filePath;
            }
            else if (ext == ".exe")
            {
                itemType = DesktopItemType.Executable;
                targetPath = filePath;
            }
            else
            {
                itemType = DesktopItemType.Folder;
            }

            // 状态比对：如果名称或目标路径已在开始菜单中，则视为已存在
            bool alreadyInStartMenu = false;
            if (startMenuSignatures.Contains("NAME:" + cleanName))
            {
                alreadyInStartMenu = true;
            }
            else if (!string.IsNullOrEmpty(targetPath))
            {
                try
                {
                    string fullTarget = Path.GetFullPath(targetPath);
                    if (startMenuSignatures.Contains("TARGET:" + fullTarget))
                    {
                        alreadyInStartMenu = true;
                    }
                }
                catch
                {
                    if (startMenuSignatures.Contains("TARGET:" + targetPath))
                    {
                        alreadyInStartMenu = true;
                    }
                }
            }

            var icon = ShellHelper.ExtractIcon(filePath, targetPath);

            return new DesktopItem
            {
                SourcePath = filePath,
                RawName = rawName,
                CleanName = cleanName,
                TargetPath = targetPath,
                ItemType = itemType,
                IsAlreadyInStartMenu = alreadyInStartMenu,
                IsSelected = !alreadyInStartMenu, // 未添加的默认勾选
                Icon = icon
            };
        }
    }
}
