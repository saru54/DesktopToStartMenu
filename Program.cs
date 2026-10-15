using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using DesktopToStartMenu.Forms;
using DesktopToStartMenu.Services;

namespace DesktopToStartMenu
{
    static class Program
    {
        private static readonly string LogFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "run.log");

        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0 && args[0].Equals("--test", StringComparison.OrdinalIgnoreCase))
            {
                RunCliTest();
                return;
            }

            try
            {
                File.WriteAllText(LogFile, $"{DateTime.Now:HH:mm:ss.fff} [Main] Starting...\n");

                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                Application.ThreadException += (s, e) =>
                {
                    File.AppendAllText(LogFile, $"{DateTime.Now:HH:mm:ss.fff} [UI Exception] {e.Exception}\n");
                };

                AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                {
                    File.AppendAllText(LogFile, $"{DateTime.Now:HH:mm:ss.fff} [AppDomain Exception] {e.ExceptionObject}\n");
                };

                ApplicationConfiguration.Initialize();
                File.AppendAllText(LogFile, $"{DateTime.Now:HH:mm:ss.fff} [Main] Configuration Initialized\n");

                Application.Run(new MainForm());
                File.AppendAllText(LogFile, $"{DateTime.Now:HH:mm:ss.fff} [Main] Normal Exit\n");
            }
            catch (Exception ex)
            {
                File.AppendAllText(LogFile, $"{DateTime.Now:HH:mm:ss.fff} [Catch Exception] {ex}\n");
            }
        }

        private static void RunCliTest()
        {
            try
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;
            }
            catch { }

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("=== [DesktopToStartMenu 自检扫描] ===");
            var scanner = new ScannerService();
            sb.AppendLine($"用户桌面: {ScannerService.UserDesktopPath}");
            sb.AppendLine($"公用桌面: {ScannerService.CommonDesktopPath}");
            sb.AppendLine($"开始菜单: {ScannerService.UserProgramsPath}");

            var items = scanner.ScanDesktopItems();
            sb.AppendLine($"\n共扫描到 {items.Count} 个项目:\n");

            foreach (var item in items)
            {
                string status = item.IsAlreadyInStartMenu ? "[已存在]" : "[未添加*]";
                sb.AppendLine($"{status,-8} | {item.CleanName,-20} | {item.ItemTypeString,-12} | 原名: {item.RawName} | 目标: {item.TargetPath}");
            }

            int unaddedCount = items.Count(x => !x.IsAlreadyInStartMenu);
            int addedCount = items.Count(x => x.IsAlreadyInStartMenu);
            sb.AppendLine($"\n统计: 待添加 {unaddedCount} 项, 已存在 {addedCount} 项");

            string report = sb.ToString();
            try { Console.WriteLine(report); } catch { }
            string testOutputFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test_output.txt");
            File.WriteAllText(testOutputFile, report);
        }
    }
}