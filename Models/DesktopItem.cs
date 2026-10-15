using System;
using System.Drawing;

namespace DesktopToStartMenu.Models
{
    public enum DesktopItemType
    {
        Shortcut,   // .lnk
        Url,        // .url
        Executable, // .exe
        Folder      // Directory
    }

    public class DesktopItem
    {
        public string SourcePath { get; set; } = string.Empty;
        public string RawName { get; set; } = string.Empty;
        public string CleanName { get; set; } = string.Empty;
        public string TargetPath { get; set; } = string.Empty;
        public DesktopItemType ItemType { get; set; }
        public bool IsAlreadyInStartMenu { get; set; }
        public bool IsSelected { get; set; }
        public Icon? Icon { get; set; }

        public string DisplayName => CleanName;

        public string ItemTypeString => ItemType switch
        {
            DesktopItemType.Shortcut => "快捷方式 (.lnk)",
            DesktopItemType.Url => "网页链接 (.url)",
            DesktopItemType.Executable => "独立程序 (.exe)",
            DesktopItemType.Folder => "文件夹",
            _ => "其他"
        };

        public string StatusString => IsAlreadyInStartMenu ? "已在开始菜单" : "未添加";
    }
}
