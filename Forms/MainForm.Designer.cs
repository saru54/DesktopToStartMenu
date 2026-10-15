namespace DesktopToStartMenu.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // Top Panel & Containers
            this.topPanel = new System.Windows.Forms.Panel();
            this.topRightPanel = new System.Windows.Forms.Panel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.topLeftFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.btnSelectUnadded = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnSelectNone = new System.Windows.Forms.Button();
            this.btnInvertSelect = new System.Windows.Forms.Button();

            // ListView
            this.listViewItems = new System.Windows.Forms.ListView();
            this.colName = new System.Windows.Forms.ColumnHeader();
            this.colStatus = new System.Windows.Forms.ColumnHeader();
            this.colType = new System.Windows.Forms.ColumnHeader();
            this.colTargetPath = new System.Windows.Forms.ColumnHeader();
            this.colRawName = new System.Windows.Forms.ColumnHeader();
            this.imageListIcons = new System.Windows.Forms.ImageList(this.components);

            // Bottom Panel & Containers
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.bottomRightFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSync = new System.Windows.Forms.Button();
            this.btnOpenStartMenu = new System.Windows.Forms.Button();
            this.bottomLeftPanel = new System.Windows.Forms.Panel();
            this.chkCleanSuffix = new System.Windows.Forms.CheckBox();
            this.chkReloadTip = new System.Windows.Forms.CheckBox();
            this.lblStats = new System.Windows.Forms.Label();

            this.topPanel.SuspendLayout();
            this.topRightPanel.SuspendLayout();
            this.topLeftFlowPanel.SuspendLayout();
            this.bottomPanel.SuspendLayout();
            this.bottomRightFlowPanel.SuspendLayout();
            this.bottomLeftPanel.SuspendLayout();
            this.SuspendLayout();

            // 
            // topPanel
            // 
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Height = 54;
            this.topPanel.Controls.Add(this.topLeftFlowPanel);
            this.topPanel.Controls.Add(this.topRightPanel);

            // 
            // topRightPanel
            // 
            this.topRightPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.topRightPanel.Width = 120;
            this.topRightPanel.Padding = new System.Windows.Forms.Padding(0, 11, 14, 11);
            this.topRightPanel.Controls.Add(this.btnRefresh);

            // btnRefresh
            this.btnRefresh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRefresh.Text = "重新扫描 🔄";
            this.btnRefresh.UseVisualStyleBackColor = true;

            // 
            // topLeftFlowPanel
            // 
            this.topLeftFlowPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.topLeftFlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.topLeftFlowPanel.WrapContents = false;
            this.topLeftFlowPanel.Padding = new System.Windows.Forms.Padding(12, 11, 0, 11);
            this.topLeftFlowPanel.Controls.Add(this.lblSearch);
            this.topLeftFlowPanel.Controls.Add(this.txtSearch);
            this.topLeftFlowPanel.Controls.Add(this.btnSelectUnadded);
            this.topLeftFlowPanel.Controls.Add(this.btnSelectAll);
            this.topLeftFlowPanel.Controls.Add(this.btnSelectNone);
            this.topLeftFlowPanel.Controls.Add(this.btnInvertSelect);

            // lblSearch
            this.lblSearch.AutoSize = true;
            this.lblSearch.Margin = new System.Windows.Forms.Padding(0, 5, 6, 0);
            this.lblSearch.Text = "搜索:";

            // txtSearch
            this.txtSearch.Size = new System.Drawing.Size(180, 26);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(0, 2, 10, 0);
            this.txtSearch.PlaceholderText = "输入应用名称或路径...";

            // btnSelectUnadded
            this.btnSelectUnadded.Size = new System.Drawing.Size(120, 30);
            this.btnSelectUnadded.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.btnSelectUnadded.Text = "仅选未添加项";
            this.btnSelectUnadded.BackColor = System.Drawing.Color.FromArgb(235, 245, 255);

            // btnSelectAll
            this.btnSelectAll.Size = new System.Drawing.Size(70, 30);
            this.btnSelectAll.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.btnSelectAll.Text = "全选";

            // btnSelectNone
            this.btnSelectNone.Size = new System.Drawing.Size(70, 30);
            this.btnSelectNone.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.btnSelectNone.Text = "全不选";

            // btnInvertSelect
            this.btnInvertSelect.Size = new System.Drawing.Size(70, 30);
            this.btnInvertSelect.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.btnInvertSelect.Text = "反选";

            // 
            // imageListIcons
            // 
            this.imageListIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.imageListIcons.ImageSize = new System.Drawing.Size(20, 20);

            // 
            // listViewItems
            // 
            this.listViewItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewItems.CheckBoxes = true;
            this.listViewItems.FullRowSelect = true;
            this.listViewItems.GridLines = true;
            this.listViewItems.View = System.Windows.Forms.View.Details;
            this.listViewItems.SmallImageList = this.imageListIcons;
            this.listViewItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
                this.colName,
                this.colStatus,
                this.colType,
                this.colTargetPath,
                this.colRawName
            });

            this.colName.Text = "应用名称 (显示名)";
            this.colName.Width = 220;

            this.colStatus.Text = "状态";
            this.colStatus.Width = 110;

            this.colType.Text = "项目类型";
            this.colType.Width = 120;

            this.colTargetPath.Text = "目标程序路径 (Target Path)";
            this.colTargetPath.Width = 340;

            this.colRawName.Text = "桌面原文件名";
            this.colRawName.Width = 180;

            // 
            // bottomPanel
            // 
            this.bottomPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.bottomPanel.Height = 80;
            this.bottomPanel.Controls.Add(this.bottomLeftPanel);
            this.bottomPanel.Controls.Add(this.bottomRightFlowPanel);

            // 
            // bottomRightFlowPanel (使用流式布局防重叠)
            // 
            this.bottomRightFlowPanel.Dock = System.Windows.Forms.DockStyle.Right;
            this.bottomRightFlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.bottomRightFlowPanel.WrapContents = false;
            this.bottomRightFlowPanel.AutoSize = true;
            this.bottomRightFlowPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.bottomRightFlowPanel.Padding = new System.Windows.Forms.Padding(0, 18, 14, 18);
            this.bottomRightFlowPanel.Controls.Add(this.btnSync);
            this.bottomRightFlowPanel.Controls.Add(this.btnOpenStartMenu);

            // btnSync
            this.btnSync.Text = "一键添加选中项至开始菜单 🚀";
            this.btnSync.Size = new System.Drawing.Size(235, 42);
            this.btnSync.Margin = new System.Windows.Forms.Padding(0, 0, 0, 0);
            this.btnSync.BackColor = System.Drawing.Color.FromArgb(0, 122, 204);
            this.btnSync.ForeColor = System.Drawing.Color.White;
            this.btnSync.Font = new System.Drawing.Font("Microsoft YaHei UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSync.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSync.FlatAppearance.BorderSize = 0;
            this.btnSync.Cursor = System.Windows.Forms.Cursors.Hand;

            // btnOpenStartMenu
            this.btnOpenStartMenu.Text = "打开开始菜单目录 📁";
            this.btnOpenStartMenu.Size = new System.Drawing.Size(155, 42);
            this.btnOpenStartMenu.Margin = new System.Windows.Forms.Padding(0, 0, 12, 0);
            this.btnOpenStartMenu.BackColor = System.Drawing.Color.FromArgb(245, 245, 245);
            this.btnOpenStartMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenStartMenu.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(200, 200, 200);
            this.btnOpenStartMenu.Cursor = System.Windows.Forms.Cursors.Hand;

            // 
            // bottomLeftPanel
            // 
            this.bottomLeftPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bottomLeftPanel.Padding = new System.Windows.Forms.Padding(14, 12, 14, 12);
            this.bottomLeftPanel.Controls.Add(this.chkCleanSuffix);
            this.bottomLeftPanel.Controls.Add(this.chkReloadTip);
            this.bottomLeftPanel.Controls.Add(this.lblStats);

            // chkCleanSuffix
            this.chkCleanSuffix.AutoSize = true;
            this.chkCleanSuffix.Checked = true;
            this.chkCleanSuffix.Location = new System.Drawing.Point(14, 14);
            this.chkCleanSuffix.Text = "自动去除「- 快捷方式」及冗余后缀";

            // chkReloadTip
            this.chkReloadTip.AutoSize = true;
            this.chkReloadTip.Checked = true;
            this.chkReloadTip.Location = new System.Drawing.Point(265, 14);
            this.chkReloadTip.Text = "完成后提示 Flow Launcher reload";

            // lblStats
            this.lblStats.AutoSize = true;
            this.lblStats.ForeColor = System.Drawing.Color.DimGray;
            this.lblStats.Location = new System.Drawing.Point(14, 44);
            this.lblStats.Text = "正在扫描桌面项目...";

            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(950, 580);
            this.MinimumSize = new System.Drawing.Size(820, 480);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9.5F);
            this.Controls.Add(this.listViewItems);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.topPanel);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DesktopToStartMenu - 桌面快捷方式同步至开始菜单工具";

            this.topPanel.ResumeLayout(false);
            this.topRightPanel.ResumeLayout(false);
            this.topLeftFlowPanel.ResumeLayout(false);
            this.topLeftFlowPanel.PerformLayout();
            this.bottomPanel.ResumeLayout(false);
            this.bottomPanel.PerformLayout();
            this.bottomRightFlowPanel.ResumeLayout(false);
            this.bottomLeftPanel.ResumeLayout(false);
            this.bottomLeftPanel.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Panel topRightPanel;
        private System.Windows.Forms.FlowLayoutPanel topLeftFlowPanel;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnSelectUnadded;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnSelectNone;
        private System.Windows.Forms.Button btnInvertSelect;
        private System.Windows.Forms.Button btnRefresh;

        private System.Windows.Forms.ListView listViewItems;
        private System.Windows.Forms.ColumnHeader colName;
        private System.Windows.Forms.ColumnHeader colStatus;
        private System.Windows.Forms.ColumnHeader colType;
        private System.Windows.Forms.ColumnHeader colTargetPath;
        private System.Windows.Forms.ColumnHeader colRawName;
        private System.Windows.Forms.ImageList imageListIcons;

        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.FlowLayoutPanel bottomRightFlowPanel;
        private System.Windows.Forms.Panel bottomLeftPanel;
        private System.Windows.Forms.CheckBox chkCleanSuffix;
        private System.Windows.Forms.CheckBox chkReloadTip;
        private System.Windows.Forms.Label lblStats;
        private System.Windows.Forms.Button btnOpenStartMenu;
        private System.Windows.Forms.Button btnSync;
    }
}
