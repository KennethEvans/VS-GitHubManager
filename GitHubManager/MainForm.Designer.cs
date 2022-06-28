
namespace GitHubManager {
    partial class MainForm {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tableLayoutPanelTop = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanelButtons = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonClear = new System.Windows.Forms.Button();
            this.buttonQuit = new System.Windows.Forms.Button();
            this.textBox = new System.Windows.Forms.TextBox();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItemFile = new System.Windows.Forms.ToolStripMenuItem();
            this.loginToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRepositories = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemGetUserRepositories = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemGetUserInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRateLimits = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSaveCsv = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.showLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanelTop.SuspendLayout();
            this.flowLayoutPanelButtons.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanelTop
            // 
            this.tableLayoutPanelTop.AutoSize = true;
            this.tableLayoutPanelTop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanelTop.ColumnCount = 1;
            this.tableLayoutPanelTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelTop.Controls.Add(this.flowLayoutPanelButtons, 0, 2);
            this.tableLayoutPanelTop.Controls.Add(this.textBox, 0, 1);
            this.tableLayoutPanelTop.Controls.Add(this.menuStrip, 0, 0);
            this.tableLayoutPanelTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelTop.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelTop.Name = "tableLayoutPanelTop";
            this.tableLayoutPanelTop.RowCount = 3;
            this.tableLayoutPanelTop.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelTop.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelTop.Size = new System.Drawing.Size(1576, 952);
            this.tableLayoutPanelTop.TabIndex = 0;
            // 
            // flowLayoutPanelButtons
            // 
            this.flowLayoutPanelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.flowLayoutPanelButtons.AutoSize = true;
            this.flowLayoutPanelButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanelButtons.Controls.Add(this.buttonClear);
            this.flowLayoutPanelButtons.Controls.Add(this.buttonQuit);
            this.flowLayoutPanelButtons.Location = new System.Drawing.Point(692, 892);
            this.flowLayoutPanelButtons.Name = "flowLayoutPanelButtons";
            this.flowLayoutPanelButtons.Size = new System.Drawing.Size(192, 57);
            this.flowLayoutPanelButtons.TabIndex = 2;
            // 
            // buttonClear
            // 
            this.buttonClear.AutoSize = true;
            this.buttonClear.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonClear.Location = new System.Drawing.Point(3, 3);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(95, 51);
            this.buttonClear.TabIndex = 0;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.OnClearClick);
            // 
            // buttonQuit
            // 
            this.buttonQuit.AutoSize = true;
            this.buttonQuit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonQuit.Location = new System.Drawing.Point(104, 3);
            this.buttonQuit.Name = "buttonQuit";
            this.buttonQuit.Size = new System.Drawing.Size(85, 51);
            this.buttonQuit.TabIndex = 0;
            this.buttonQuit.Text = "Quit";
            this.buttonQuit.UseVisualStyleBackColor = true;
            this.buttonQuit.Click += new System.EventHandler(this.OnExitClick);
            // 
            // textBox
            // 
            this.textBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox.Location = new System.Drawing.Point(3, 55);
            this.textBox.Multiline = true;
            this.textBox.Name = "textBox";
            this.textBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox.Size = new System.Drawing.Size(1570, 831);
            this.textBox.TabIndex = 3;
            // 
            // menuStrip
            // 
            this.menuStrip.ImageScalingSize = new System.Drawing.Size(40, 40);
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemFile,
            this.toolStripMenuItemHelp});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1576, 52);
            this.menuStrip.TabIndex = 4;
            this.menuStrip.Text = "menuStrip1";
            // 
            // toolStripMenuItemFile
            // 
            this.toolStripMenuItemFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loginToolStripMenuItem,
            this.toolStripMenuItemRepositories,
            this.toolStripMenuItemGetUserRepositories,
            this.toolStripMenuItemGetUserInfo,
            this.toolStripMenuItemRateLimits,
            this.toolStripMenuItemSaveCsv,
            this.toolStripSeparator1,
            this.showLogToolStripMenuItem,
            this.toolStripMenuItemExit});
            this.toolStripMenuItemFile.Name = "toolStripMenuItemFile";
            this.toolStripMenuItemFile.Size = new System.Drawing.Size(87, 48);
            this.toolStripMenuItemFile.Text = "File";
            // 
            // loginToolStripMenuItem
            // 
            this.loginToolStripMenuItem.Name = "loginToolStripMenuItem";
            this.loginToolStripMenuItem.Size = new System.Drawing.Size(536, 54);
            this.loginToolStripMenuItem.Text = "Login";
            this.loginToolStripMenuItem.Click += new System.EventHandler(this.OnLoginClick);
            // 
            // toolStripMenuItemRepositories
            // 
            this.toolStripMenuItemRepositories.Name = "toolStripMenuItemRepositories";
            this.toolStripMenuItemRepositories.Size = new System.Drawing.Size(536, 54);
            this.toolStripMenuItemRepositories.Text = "Get Repositories";
            this.toolStripMenuItemRepositories.Click += new System.EventHandler(this.OnGetRepositoriesClick);
            // 
            // toolStripMenuItemGetUserRepositories
            // 
            this.toolStripMenuItemGetUserRepositories.Name = "toolStripMenuItemGetUserRepositories";
            this.toolStripMenuItemGetUserRepositories.Size = new System.Drawing.Size(536, 54);
            this.toolStripMenuItemGetUserRepositories.Text = "Get Repositories for User...";
            this.toolStripMenuItemGetUserRepositories.Click += new System.EventHandler(this.OnGetUserRepositoriesClick);
            // 
            // toolStripMenuItemGetUserInfo
            // 
            this.toolStripMenuItemGetUserInfo.Name = "toolStripMenuItemGetUserInfo";
            this.toolStripMenuItemGetUserInfo.Size = new System.Drawing.Size(536, 54);
            this.toolStripMenuItemGetUserInfo.Text = "Get Info for User...";
            this.toolStripMenuItemGetUserInfo.Click += new System.EventHandler(this.OnGetUserInformationClick);
            // 
            // toolStripMenuItemRateLimits
            // 
            this.toolStripMenuItemRateLimits.Name = "toolStripMenuItemRateLimits";
            this.toolStripMenuItemRateLimits.Size = new System.Drawing.Size(536, 54);
            this.toolStripMenuItemRateLimits.Text = "Get Rate Limits";
            this.toolStripMenuItemRateLimits.Click += new System.EventHandler(this.OnGetRateLimitsClick);
            // 
            // toolStripMenuItemSaveCsv
            // 
            this.toolStripMenuItemSaveCsv.Name = "toolStripMenuItemSaveCsv";
            this.toolStripMenuItemSaveCsv.Size = new System.Drawing.Size(536, 54);
            this.toolStripMenuItemSaveCsv.Text = "Save CSV...";
            this.toolStripMenuItemSaveCsv.Click += new System.EventHandler(this.OnSaveCsvClick);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(533, 6);
            // 
            // showLogToolStripMenuItem
            // 
            this.showLogToolStripMenuItem.Name = "showLogToolStripMenuItem";
            this.showLogToolStripMenuItem.Size = new System.Drawing.Size(536, 54);
            this.showLogToolStripMenuItem.Text = "Show Error Log...";
            this.showLogToolStripMenuItem.Click += new System.EventHandler(this.OnShowLogClick);
            // 
            // toolStripMenuItemExit
            // 
            this.toolStripMenuItemExit.Name = "toolStripMenuItemExit";
            this.toolStripMenuItemExit.Size = new System.Drawing.Size(536, 54);
            this.toolStripMenuItemExit.Text = "Exit";
            this.toolStripMenuItemExit.Click += new System.EventHandler(this.OnExitClick);
            // 
            // toolStripMenuItemHelp
            // 
            this.toolStripMenuItemHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemAbout});
            this.toolStripMenuItemHelp.Name = "toolStripMenuItemHelp";
            this.toolStripMenuItemHelp.Size = new System.Drawing.Size(104, 48);
            this.toolStripMenuItemHelp.Text = "Help";
            // 
            // toolStripMenuItemAbout
            // 
            this.toolStripMenuItemAbout.Name = "toolStripMenuItemAbout";
            this.toolStripMenuItemAbout.Size = new System.Drawing.Size(287, 54);
            this.toolStripMenuItemAbout.Text = "About...";
            this.toolStripMenuItemAbout.Click += new System.EventHandler(this.OnAboutClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(17F, 41F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1576, 952);
            this.Controls.Add(this.tableLayoutPanelTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "GitHub Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OnFormClosed);
            this.Load += new System.EventHandler(this.OnFormLoad);
            this.tableLayoutPanelTop.ResumeLayout(false);
            this.tableLayoutPanelTop.PerformLayout();
            this.flowLayoutPanelButtons.ResumeLayout(false);
            this.flowLayoutPanelButtons.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTop;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelButtons;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Button buttonQuit;
        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFile;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemExit;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRateLimits;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRepositories;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemHelp;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAbout;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSaveCsv;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemGetUserRepositories;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemGetUserInfo;
        private System.Windows.Forms.ToolStripMenuItem showLogToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loginToolStripMenuItem;
    }
}

