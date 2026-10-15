using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DesktopToStartMenu.Models;
using DesktopToStartMenu.Services;

namespace DesktopToStartMenu.Forms
{
    public partial class MainForm : Form
    {
        private readonly ScannerService _scannerService = new();
        private readonly SyncService _syncService = new();

        private List<DesktopItem> _allItems = new();
        private bool _isUpdatingUi = false;

        public MainForm()
        {
            InitializeComponent();
            SetupEvents();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadDesktopItems();
        }

        private void SetupEvents()
        {
            txtSearch.TextChanged += (s, e) => ApplyFilter();
            btnRefresh.Click += (s, e) => LoadDesktopItems();

            btnSelectUnadded.Click += (s, e) =>
            {
                _isUpdatingUi = true;
                foreach (ListViewItem lvi in listViewItems.Items)
                {
                    if (lvi.Tag is DesktopItem item)
                    {
                        lvi.Checked = !item.IsAlreadyInStartMenu;
                        item.IsSelected = lvi.Checked;
                    }
                }
                _isUpdatingUi = false;
                UpdateStats();
            };

            btnSelectAll.Click += (s, e) =>
            {
                _isUpdatingUi = true;
                foreach (ListViewItem lvi in listViewItems.Items)
                {
                    lvi.Checked = true;
                    if (lvi.Tag is DesktopItem item) item.IsSelected = true;
                }
                _isUpdatingUi = false;
                UpdateStats();
            };

            btnSelectNone.Click += (s, e) =>
            {
                _isUpdatingUi = true;
                foreach (ListViewItem lvi in listViewItems.Items)
                {
                    lvi.Checked = false;
                    if (lvi.Tag is DesktopItem item) item.IsSelected = false;
                }
                _isUpdatingUi = false;
                UpdateStats();
            };

            btnInvertSelect.Click += (s, e) =>
            {
                _isUpdatingUi = true;
                foreach (ListViewItem lvi in listViewItems.Items)
                {
                    lvi.Checked = !lvi.Checked;
                    if (lvi.Tag is DesktopItem item) item.IsSelected = lvi.Checked;
                }
                _isUpdatingUi = false;
                UpdateStats();
            };

            listViewItems.ItemChecked += (s, e) =>
            {
                if (_isUpdatingUi) return;
                if (e.Item.Tag is DesktopItem item)
                {
                    item.IsSelected = e.Item.Checked;
                    UpdateStats();
                }
            };

            btnOpenStartMenu.Click += (s, e) =>
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "explorer.exe",
                        Arguments = ScannerService.UserProgramsPath,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"无法打开目录: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            btnSync.Click += async (s, e) => await ExecuteSyncAsync();
        }

        private async void LoadDesktopItems()
        {
            lblStats.Text = "正在扫描桌面项目与开始菜单比对，请稍候...";
            btnRefresh.Enabled = false;
            btnSync.Enabled = false;

            try
            {
                var items = await Task.Run(() => _scannerService.ScanDesktopItems());
                _allItems = items;
                ApplyFilter();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"扫描失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStats.Text = "扫描失败";
            }
            finally
            {
                btnRefresh.Enabled = true;
                btnSync.Enabled = true;
            }
        }

        private void ApplyFilter()
        {
            _isUpdatingUi = true;
            listViewItems.BeginUpdate();
            listViewItems.Items.Clear();
            imageListIcons.Images.Clear();

            string keyword = txtSearch.Text.Trim();
            var filtered = _allItems.Where(item =>
                string.IsNullOrEmpty(keyword) ||
                item.CleanName.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                item.RawName.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                item.TargetPath.Contains(keyword, StringComparison.OrdinalIgnoreCase)
            ).ToList();

            int iconIndex = 0;
            foreach (var item in filtered)
            {
                // 添加图标
                if (item.Icon != null)
                {
                    imageListIcons.Images.Add(item.Icon);
                }
                else
                {
                    // 默认图标占位
                    imageListIcons.Images.Add(SystemIcons.Application);
                }

                var lvi = new ListViewItem(item.CleanName, iconIndex++)
                {
                    Tag = item,
                    Checked = item.IsSelected,
                    UseItemStyleForSubItems = false
                };

                // 状态列
                var statusSubItem = lvi.SubItems.Add(item.StatusString);
                if (!item.IsAlreadyInStartMenu)
                {
                    statusSubItem.ForeColor = Color.ForestGreen;
                    statusSubItem.Font = new Font(listViewItems.Font, FontStyle.Bold);
                }
                else
                {
                    statusSubItem.ForeColor = Color.Gray;
                }

                // 类型列
                lvi.SubItems.Add(item.ItemTypeString);

                // 目标路径列
                lvi.SubItems.Add(string.IsNullOrEmpty(item.TargetPath) ? "-" : item.TargetPath);

                // 原始名称列
                lvi.SubItems.Add(item.RawName);

                listViewItems.Items.Add(lvi);
            }

            listViewItems.EndUpdate();
            _isUpdatingUi = false;

            UpdateStats();
        }

        private void UpdateStats()
        {
            int total = _allItems.Count;
            int unadded = _allItems.Count(x => !x.IsAlreadyInStartMenu);
            int added = _allItems.Count(x => x.IsAlreadyInStartMenu);
            int selected = _allItems.Count(x => x.IsSelected);

            lblStats.Text = $"共扫描到 {total} 项 (待添加 {unadded} 项，已存在 {added} 项) | 已勾选 {selected} 项";
        }

        private async Task ExecuteSyncAsync()
        {
            var selectedItems = _allItems.Where(x => x.IsSelected).ToList();
            if (selectedItems.Count == 0)
            {
                MessageBox.Show("请先勾选需要添加到开始菜单的项目！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string confirmMsg = $"确定将选中的 {selectedItems.Count} 个快捷方式添加至 Windows 开始菜单吗？";
            if (MessageBox.Show(confirmMsg, "确认添加", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            btnSync.Enabled = false;
            lblStats.Text = "正在生成并同步快捷方式...";

            bool cleanSuffix = chkCleanSuffix.Checked;

            var result = await Task.Run(() => _syncService.SyncToStartMenu(selectedItems, cleanNameSuffix: cleanSuffix));

            string summaryMsg = $"同步完成！\n\n- 成功添加: {result.SuccessCount} 项\n- 失败: {result.FailedCount} 项";

            if (result.ErrorMessages.Count > 0)
            {
                summaryMsg += "\n\n部分失败原因:\n" + string.Join("\n", result.ErrorMessages.Take(5));
            }

            if (chkReloadTip.Checked)
            {
                summaryMsg += "\n\n━━━━━━━━━━━━━━━━━━━━━━━━\n💡【Flow Launcher 刷新建议】\n现在唤出 Flow Launcher，在搜索框输入 reload 回车，即可立刻检索刚添加的程序！";
            }

            MessageBox.Show(summaryMsg, "同步成功", MessageBoxButtons.OK, MessageBoxIcon.Information);

            // 重新刷新列表状态
            LoadDesktopItems();
        }
    }
}
