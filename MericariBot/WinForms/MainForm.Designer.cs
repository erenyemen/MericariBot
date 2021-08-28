
namespace MericariBot.WinForms
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.tsmAmazon = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmRakuten = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmMericari = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmGoogleChrome = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmUserManagament = new System.Windows.Forms.ToolStripMenuItem();
            this.advertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmReAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.stslblUserName = new System.Windows.Forms.ToolStripStatusLabel();
            this.stslblIpAddress = new System.Windows.Forms.ToolStripStatusLabel();
            this.stslblUserRole = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.BrowserTabControl = new System.Windows.Forms.TabControl();
            this.ımageList1 = new System.Windows.Forms.ImageList(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.AutoSize = false;
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmAmazon,
            this.tsmRakuten,
            this.tsmMericari,
            this.settingsToolStripMenuItem,
            this.tsmReAdd,
            this.tsmAdd});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1298, 43);
            this.menuStrip1.TabIndex = 11;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // tsmAmazon
            // 
            this.tsmAmazon.Image = ((System.Drawing.Image)(resources.GetObject("tsmAmazon.Image")));
            this.tsmAmazon.Name = "tsmAmazon";
            this.tsmAmazon.Size = new System.Drawing.Size(92, 39);
            this.tsmAmazon.Text = "Amazon";
            this.tsmAmazon.Click += new System.EventHandler(this.tsmAmazon_Click);
            // 
            // tsmRakuten
            // 
            this.tsmRakuten.Image = ((System.Drawing.Image)(resources.GetObject("tsmRakuten.Image")));
            this.tsmRakuten.Name = "tsmRakuten";
            this.tsmRakuten.Size = new System.Drawing.Size(90, 39);
            this.tsmRakuten.Text = "Rakuten";
            this.tsmRakuten.Click += new System.EventHandler(this.tsmRakuten_Click);
            // 
            // tsmMericari
            // 
            this.tsmMericari.Image = ((System.Drawing.Image)(resources.GetObject("tsmMericari.Image")));
            this.tsmMericari.Name = "tsmMericari";
            this.tsmMericari.Size = new System.Drawing.Size(91, 39);
            this.tsmMericari.Text = "Mericari";
            this.tsmMericari.Click += new System.EventHandler(this.tsmMericari_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmGoogleChrome,
            this.tsmUserManagament,
            this.advertToolStripMenuItem});
            this.settingsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("settingsToolStripMenuItem.Image")));
            this.settingsToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(98, 39);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // tsmGoogleChrome
            // 
            this.tsmGoogleChrome.Image = ((System.Drawing.Image)(resources.GetObject("tsmGoogleChrome.Image")));
            this.tsmGoogleChrome.Name = "tsmGoogleChrome";
            this.tsmGoogleChrome.Size = new System.Drawing.Size(218, 24);
            this.tsmGoogleChrome.Text = "Google Chrome Auth";
            this.tsmGoogleChrome.Click += new System.EventHandler(this.tsmGoogleChrome_Click);
            // 
            // tsmUserManagament
            // 
            this.tsmUserManagament.Name = "tsmUserManagament";
            this.tsmUserManagament.Size = new System.Drawing.Size(218, 24);
            this.tsmUserManagament.Text = "User Managament";
            this.tsmUserManagament.Click += new System.EventHandler(this.tsmUserManagament_Click);
            // 
            // advertToolStripMenuItem
            // 
            this.advertToolStripMenuItem.Name = "advertToolStripMenuItem";
            this.advertToolStripMenuItem.Size = new System.Drawing.Size(218, 24);
            this.advertToolStripMenuItem.Text = "Advert Settings";
            this.advertToolStripMenuItem.Click += new System.EventHandler(this.advertToolStripMenuItem_Click);
            // 
            // tsmReAdd
            // 
            this.tsmReAdd.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsmReAdd.Image = ((System.Drawing.Image)(resources.GetObject("tsmReAdd.Image")));
            this.tsmReAdd.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmReAdd.Name = "tsmReAdd";
            this.tsmReAdd.Size = new System.Drawing.Size(96, 39);
            this.tsmReAdd.Text = "Re-Add";
            this.tsmReAdd.Click += new System.EventHandler(this.tsmReAdd_Click);
            // 
            // tsmAdd
            // 
            this.tsmAdd.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsmAdd.Checked = true;
            this.tsmAdd.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsmAdd.Image = ((System.Drawing.Image)(resources.GetObject("tsmAdd.Image")));
            this.tsmAdd.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.tsmAdd.Name = "tsmAdd";
            this.tsmAdd.Size = new System.Drawing.Size(73, 39);
            this.tsmAdd.Text = "Add";
            this.tsmAdd.Click += new System.EventHandler(this.tsmAdd_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stslblUserName,
            this.stslblIpAddress,
            this.stslblUserRole,
            this.toolStripSplitButton1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 618);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1298, 22);
            this.statusStrip1.TabIndex = 12;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // stslblUserName
            // 
            this.stslblUserName.Name = "stslblUserName";
            this.stslblUserName.Size = new System.Drawing.Size(72, 17);
            this.stslblUserName.Text = "erenyemen";
            // 
            // stslblIpAddress
            // 
            this.stslblIpAddress.Name = "stslblIpAddress";
            this.stslblIpAddress.Size = new System.Drawing.Size(52, 17);
            this.stslblIpAddress.Text = "10.1.2.3";
            // 
            // stslblUserRole
            // 
            this.stslblUserRole.Name = "stslblUserRole";
            this.stslblUserRole.Size = new System.Drawing.Size(131, 17);
            this.stslblUserRole.Text = "toolStripStatusLabel1";
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton1.Image")));
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(32, 20);
            this.toolStripSplitButton1.Text = "toolStripSplitButton1";
            // 
            // BrowserTabControl
            // 
            this.BrowserTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BrowserTabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.BrowserTabControl.ImageList = this.ımageList1;
            this.BrowserTabControl.Location = new System.Drawing.Point(0, 129);
            this.BrowserTabControl.Name = "BrowserTabControl";
            this.BrowserTabControl.SelectedIndex = 0;
            this.BrowserTabControl.Size = new System.Drawing.Size(1298, 489);
            this.BrowserTabControl.TabIndex = 13;
            this.BrowserTabControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BrowserTabControl_MouseDown);
            // 
            // ımageList1
            // 
            this.ımageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ımageList1.ImageStream")));
            this.ımageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.ımageList1.Images.SetKeyName(0, "5378814_fight_game_media_play_start_icon.png");
            this.ımageList1.Images.SetKeyName(1, "245976_amazon_icon.png");
            this.ımageList1.Images.SetKeyName(2, "454-4548279_rakuten-rakuten-login-hd-png-download.jpg");
            this.ımageList1.Images.SetKeyName(3, "8133808.png");
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 46);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1277, 77);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            // 
            // timer1
            // 
            this.timer1.Interval = 60000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1298, 640);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.BrowserTabControl);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Mercari Product BOT";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem tsmAmazon;
        private System.Windows.Forms.ToolStripMenuItem tsmRakuten;
        private System.Windows.Forms.ToolStripMenuItem tsmMericari;
        private System.Windows.Forms.ToolStripMenuItem tsmReAdd;
        private System.Windows.Forms.ToolStripMenuItem tsmAdd;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel stslblUserName;
        private System.Windows.Forms.ToolStripStatusLabel stslblIpAddress;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.TabControl BrowserTabControl;
        private System.Windows.Forms.ImageList ımageList1;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmGoogleChrome;
        private System.Windows.Forms.ToolStripMenuItem tsmUserManagament;
        private System.Windows.Forms.ToolStripStatusLabel stslblUserRole;
        private System.Windows.Forms.Timer timer1;
        public System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem advertToolStripMenuItem;
    }
}