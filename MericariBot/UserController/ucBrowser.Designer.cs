
namespace MericariBot.UserController
{
    partial class ucBrowser
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucBrowser));
            this.btnForward = new System.Windows.Forms.Button();
            this.btnBackward = new System.Windows.Forms.Button();
            this.txtUrl = new System.Windows.Forms.TextBox();
            this.btnGo = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.geckoWebBrowser1 = new Gecko.GeckoWebBrowser();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnForward
            // 
            this.btnForward.BackColor = System.Drawing.Color.Transparent;
            this.btnForward.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnForward.Image = ((System.Drawing.Image)(resources.GetObject("btnForward.Image")));
            this.btnForward.Location = new System.Drawing.Point(45, 12);
            this.btnForward.Name = "btnForward";
            this.btnForward.Size = new System.Drawing.Size(27, 23);
            this.btnForward.TabIndex = 7;
            this.btnForward.UseVisualStyleBackColor = false;
            this.btnForward.Click += new System.EventHandler(this.btnForward_Click);
            // 
            // btnBackward
            // 
            this.btnBackward.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBackward.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBackward.Image = ((System.Drawing.Image)(resources.GetObject("btnBackward.Image")));
            this.btnBackward.Location = new System.Drawing.Point(12, 12);
            this.btnBackward.Name = "btnBackward";
            this.btnBackward.Size = new System.Drawing.Size(27, 23);
            this.btnBackward.TabIndex = 5;
            this.btnBackward.UseVisualStyleBackColor = true;
            this.btnBackward.Click += new System.EventHandler(this.btnBackward_Click);
            // 
            // txtUrl
            // 
            this.txtUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUrl.BackColor = System.Drawing.SystemColors.Window;
            this.txtUrl.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtUrl.Location = new System.Drawing.Point(78, 12);
            this.txtUrl.Multiline = true;
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.Size = new System.Drawing.Size(1111, 23);
            this.txtUrl.TabIndex = 2;
            this.txtUrl.Text = "https://www.mercari.com/jp/";
            // 
            // btnGo
            // 
            this.btnGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGo.Location = new System.Drawing.Point(1195, 12);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(44, 23);
            this.btnGo.TabIndex = 3;
            this.btnGo.Text = "GO";
            this.btnGo.UseVisualStyleBackColor = true;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel1.Controls.Add(this.btnForward);
            this.panel1.Controls.Add(this.btnBackward);
            this.panel1.Controls.Add(this.btnGo);
            this.panel1.Controls.Add(this.txtUrl);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1249, 49);
            this.panel1.TabIndex = 9;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.geckoWebBrowser1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 49);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1249, 651);
            this.panel2.TabIndex = 10;
            // 
            // geckoWebBrowser1
            // 
            this.geckoWebBrowser1.ConsoleMessageEventReceivesConsoleLogCalls = true;
            this.geckoWebBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.geckoWebBrowser1.FrameEventsPropagateToMainWindow = false;
            this.geckoWebBrowser1.Location = new System.Drawing.Point(0, 0);
            this.geckoWebBrowser1.Name = "geckoWebBrowser1";
            this.geckoWebBrowser1.Size = new System.Drawing.Size(1249, 651);
            this.geckoWebBrowser1.TabIndex = 5;
            this.geckoWebBrowser1.UseHttpActivityObserver = false;
            this.geckoWebBrowser1.UseWaitCursor = true;
            this.geckoWebBrowser1.Navigating += new System.EventHandler<Gecko.Events.GeckoNavigatingEventArgs>(this.geckoWebBrowser1_Navigating);
            this.geckoWebBrowser1.Navigated += new System.EventHandler<Gecko.GeckoNavigatedEventArgs>(this.geckoWebBrowser1_Navigated);
            this.geckoWebBrowser1.NavigationError += new System.EventHandler<Gecko.Events.GeckoNavigationErrorEventArgs>(this.geckoWebBrowser1_NavigationError);
            this.geckoWebBrowser1.FrameNavigating += new System.EventHandler<Gecko.Events.GeckoNavigatingEventArgs>(this.geckoWebBrowser1_FrameNavigating);
            this.geckoWebBrowser1.DocumentCompleted += new System.EventHandler<Gecko.Events.GeckoDocumentCompletedEventArgs>(this.geckoWebBrowser1_DocumentCompleted);
            this.geckoWebBrowser1.CreateWindow += new System.EventHandler<Gecko.GeckoCreateWindowEventArgs>(this.geckoWebBrowser1_CreateWindow);
            this.geckoWebBrowser1.Load += new System.EventHandler<Gecko.DomEventArgs>(this.geckoWebBrowser1_Load);
            this.geckoWebBrowser1.ConsoleMessage += new System.EventHandler<Gecko.ConsoleMessageEventArgs>(this.geckoWebBrowser1_ConsoleMessage);
            this.geckoWebBrowser1.NSSError += new System.EventHandler<Gecko.Events.GeckoNSSErrorEventArgs>(this.geckoWebBrowser1_NSSError);
            this.geckoWebBrowser1.GeckoHandleCreated += new System.EventHandler(this.geckoWebBrowser1_GeckoHandleCreated);
            this.geckoWebBrowser1.Validated += new System.EventHandler(this.geckoWebBrowser1_Validated);
            // 
            // ucBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "ucBrowser";
            this.Size = new System.Drawing.Size(1249, 700);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnForward;
        private System.Windows.Forms.Button btnBackward;
        private System.Windows.Forms.TextBox txtUrl;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private Gecko.GeckoWebBrowser geckoWebBrowser1;
    }
}
