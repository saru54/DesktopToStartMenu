using System;
using System.Collections.Generic;
using System.IO;
using DesktopToStartMenu.Models;

namespace DesktopToStartMenu.Services
{
    public class SyncResult
    {
        public int SuccessCount { get; set; }
        public int FailedCount { get; set; }
        public List<string> ErrorMessages { get; set; } = new();
    }

    public class SyncService
    {
        public SyncResult SyncToStartMenu(IEnumerable<DesktopItem> items, bool cleanNameSuffix = true, string subFolder = "")
        {
            var result = new SyncResult();
            string targetDir = string.IsNullOrWhiteSpace(subFolder)
                ? ScannerService.UserProgramsPath
                : Path.Combine(ScannerService.UserProgramsPath, subFolder.Trim());

            try
            {
                if (!Directory.Exists(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                }
            }
            catch (Exception ex)
            {
                result.FailedCount++;
                result.ErrorMessages.Add($"创建目标目录失败: {ex.Message}");
                return result;
            }

            foreach (var item in items)
            {
                try
                {
                    string finalName = cleanNameSuffix ? item.CleanName : Path.GetFileNameWithoutExtension(item.RawName);
                    // 确保文件名合法
                    foreach (char invalidChar in Path.GetInvalidFileNameChars())
                    {
                        finalName = finalName.Replace(invalidChar, '_');
                    }

                    if (item.ItemType == DesktopItemType.Shortcut)
                    {
                        string destLnk = Path.Combine(targetDir, finalName + ".lnk");
                        var (targetPath, workingDir, arguments) = ShellHelper.ResolveShortcut(item.SourcePath);

                        if (!string.IsNullOrEmpty(targetPath))
                        {
                            bool success = ShellHelper.CreateShortcut(destLnk, targetPath, workingDir, arguments);
                            if (success)
                            {
                                result.SuccessCount++;
                                item.IsAlreadyInStartMenu = true;
                                item.IsSelected = false;
                                continue;
                            }
                        }

                        // 如果 COM 创建失败或无 target，直接复制原 .lnk
                        File.Copy(item.SourcePath, destLnk, overwrite: true);
                        result.SuccessCount++;
                        item.IsAlreadyInStartMenu = true;
                        item.IsSelected = false;
                    }
                    else if (item.ItemType == DesktopItemType.Executable)
                    {
                        string destLnk = Path.Combine(targetDir, finalName + ".lnk");
                        string workingDir = Path.GetDirectoryName(item.SourcePath) ?? string.Empty;
                        bool success = ShellHelper.CreateShortcut(destLnk, item.SourcePath, workingDir);
                        if (success)
                        {
                            result.SuccessCount++;
                            item.IsAlreadyInStartMenu = true;
                            item.IsSelected = false;
                        }
                        else
                        {
                            result.FailedCount++;
                            result.ErrorMessages.Add($"创建快捷方式失败: {item.DisplayName}");
                        }
                    }
                    else if (item.ItemType == DesktopItemType.Url)
                    {
                        string destUrl = Path.Combine(targetDir, finalName + ".url");
                        File.Copy(item.SourcePath, destUrl, overwrite: true);
                        result.SuccessCount++;
                        item.IsAlreadyInStartMenu = true;
                        item.IsSelected = false;
                    }
                }
                catch (Exception ex)
                {
                    result.FailedCount++;
                    result.ErrorMessages.Add($"{item.DisplayName}: {ex.Message}");
                }
            }

            return result;
        }
    }
}
