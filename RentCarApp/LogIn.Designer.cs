namespace RentCarApp
{
    partial class LogIn
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogIn));
            this.PassTB = new System.Windows.Forms.TextBox();
            this.UserTB = new System.Windows.Forms.TextBox();
            this.LogInBtn = new System.Windows.Forms.Button();
            this.passwordBtn = new System.Windows.Forms.Button();
            this.forgetPassLinkLabel = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // PassTB
            // 
            this.PassTB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(202)))), ((int)(((byte)(204)))));
            this.PassTB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.PassTB.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.PassTB.ForeColor = System.Drawing.Color.Black;
            this.PassTB.Location = new System.Drawing.Point(139, 325);
            this.PassTB.Margin = new System.Windows.Forms.Padding(3, 5, 3, 2);
            this.PassTB.MaximumSize = new System.Drawing.Size(260, 25);
            this.PassTB.MaxLength = 25;
            this.PassTB.MinimumSize = new System.Drawing.Size(260, 25);
            this.PassTB.Name = "PassTB";
            this.PassTB.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.PassTB.ShortcutsEnabled = false;
            this.PassTB.Size = new System.Drawing.Size(260, 25);
            this.PassTB.TabIndex = 0;
            this.PassTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.PassTB.UseSystemPasswordChar = true;
            // 
            // UserTB
            // 
            this.UserTB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(202)))), ((int)(((byte)(204)))));
            this.UserTB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.UserTB.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(177)));
            this.UserTB.ForeColor = System.Drawing.Color.Black;
            this.UserTB.Location = new System.Drawing.Point(134, 252);
            this.UserTB.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.UserTB.MaximumSize = new System.Drawing.Size(268, 25);
            this.UserTB.MinimumSize = new System.Drawing.Size(268, 25);
            this.UserTB.Name = "UserTB";
            this.UserTB.Size = new System.Drawing.Size(268, 22);
            this.UserTB.TabIndex = 0;
            this.UserTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // LogInBtn
            // 
            this.LogInBtn.BackColor = System.Drawing.Color.Transparent;
            this.LogInBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LogInBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.LogInBtn.FlatAppearance.BorderColor = System.Drawing.Color.Red;
            this.LogInBtn.FlatAppearance.BorderSize = 0;
            this.LogInBtn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.LogInBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.LogInBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.LogInBtn.ForeColor = System.Drawing.Color.Transparent;
            this.LogInBtn.Location = new System.Drawing.Point(365, 410);
            this.LogInBtn.Name = "LogInBtn";
            this.LogInBtn.Size = new System.Drawing.Size(145, 43);
            this.LogInBtn.TabIndex = 1;
            this.LogInBtn.UseVisualStyleBackColor = false;
            this.LogInBtn.Click += new System.EventHandler(this.LogInBtn_Click);
            // 
            // passwordBtn
            // 
            this.passwordBtn.BackColor = System.Drawing.Color.Transparent;
            this.passwordBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.passwordBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.passwordBtn.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.passwordBtn.FlatAppearance.BorderSize = 0;
            this.passwordBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.passwordBtn.ForeColor = System.Drawing.SystemColors.ControlText;
            this.passwordBtn.Location = new System.Drawing.Point(109, 325);
            this.passwordBtn.Name = "passwordBtn";
            this.passwordBtn.Size = new System.Drawing.Size(27, 24);
            this.passwordBtn.TabIndex = 2;
            this.passwordBtn.UseVisualStyleBackColor = false;
            this.passwordBtn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.passwordBtn_MouseDown);
            this.passwordBtn.MouseUp += new System.Windows.Forms.MouseEventHandler(this.passwordBtn_MouseUp);
            // 
            // forgetPassLinkLabel
            // 
            this.forgetPassLinkLabel.AutoSize = true;
            this.forgetPassLinkLabel.BackColor = System.Drawing.Color.Transparent;
            this.forgetPassLinkLabel.DisabledLinkColor = System.Drawing.Color.Transparent;
            this.forgetPassLinkLabel.Font = new System.Drawing.Font("Calibri", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.forgetPassLinkLabel.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(51)))), ((int)(((byte)(51)))));
            this.forgetPassLinkLabel.Location = new System.Drawing.Point(107, 361);
            this.forgetPassLinkLabel.Name = "forgetPassLinkLabel";
            this.forgetPassLinkLabel.Size = new System.Drawing.Size(120, 24);
            this.forgetPassLinkLabel.TabIndex = 3;
            this.forgetPassLinkLabel.TabStop = true;
            this.forgetPassLinkLabel.Text = "שכחתי סיסמא";
            this.forgetPassLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ForgetPassLinkLabel_LinkClicked);
            // 
            // LogIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(620, 506);
            this.Controls.Add(this.forgetPassLinkLabel);
            this.Controls.Add(this.LogInBtn);
            this.Controls.Add(this.UserTB);
            this.Controls.Add(this.PassTB);
            this.Controls.Add(this.passwordBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(636, 545);
            this.MinimumSize = new System.Drawing.Size(636, 545);
            this.Name = "LogIn";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ניהול עסק השכרת רכב";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox PassTB;
        private System.Windows.Forms.TextBox UserTB;
        private System.Windows.Forms.Button LogInBtn;
        private System.Windows.Forms.Button passwordBtn;
        private System.Windows.Forms.LinkLabel forgetPassLinkLabel;
    }
}

