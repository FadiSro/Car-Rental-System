namespace RentCarApp
{
    partial class AddCustomer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddCustomer));
            this.licenseExpiryDate = new System.Windows.Forms.DateTimePicker();
            this.licenseIssueDate = new System.Windows.Forms.DateTimePicker();
            this.BirthdateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label15 = new System.Windows.Forms.Label();
            this.telephoneNumTxt = new System.Windows.Forms.TextBox();
            this.label45 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.phoneNumTxt = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.addressTxt = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.lastNameTxt = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.firstNameTxt = new System.Windows.Forms.TextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.idNumTxt = new System.Windows.Forms.TextBox();
            this.addCustomerBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // licenseExpiryDate
            // 
            this.licenseExpiryDate.CustomFormat = "dd/MM/yyyy";
            this.licenseExpiryDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.licenseExpiryDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.licenseExpiryDate.Location = new System.Drawing.Point(334, 343);
            this.licenseExpiryDate.MinDate = new System.DateTime(1930, 1, 1, 0, 0, 0, 0);
            this.licenseExpiryDate.Name = "licenseExpiryDate";
            this.licenseExpiryDate.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.licenseExpiryDate.RightToLeftLayout = true;
            this.licenseExpiryDate.Size = new System.Drawing.Size(140, 26);
            this.licenseExpiryDate.TabIndex = 8;
            // 
            // licenseIssueDate
            // 
            this.licenseIssueDate.CustomFormat = "dd/MM/yyyy";
            this.licenseIssueDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.licenseIssueDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.licenseIssueDate.Location = new System.Drawing.Point(334, 301);
            this.licenseIssueDate.MinDate = new System.DateTime(1930, 1, 1, 0, 0, 0, 0);
            this.licenseIssueDate.Name = "licenseIssueDate";
            this.licenseIssueDate.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.licenseIssueDate.RightToLeftLayout = true;
            this.licenseIssueDate.Size = new System.Drawing.Size(140, 26);
            this.licenseIssueDate.TabIndex = 7;
            // 
            // BirthdateTimePicker
            // 
            this.BirthdateTimePicker.CustomFormat = "dd/MM/yyyy";
            this.BirthdateTimePicker.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.BirthdateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.BirthdateTimePicker.Location = new System.Drawing.Point(334, 187);
            this.BirthdateTimePicker.MinDate = new System.DateTime(1930, 1, 1, 0, 0, 0, 0);
            this.BirthdateTimePicker.Name = "BirthdateTimePicker";
            this.BirthdateTimePicker.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.BirthdateTimePicker.RightToLeftLayout = true;
            this.BirthdateTimePicker.Size = new System.Drawing.Size(140, 26);
            this.BirthdateTimePicker.TabIndex = 4;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label15.Location = new System.Drawing.Point(528, 263);
            this.label15.Name = "label15";
            this.label15.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label15.Size = new System.Drawing.Size(87, 22);
            this.label15.TabIndex = 60;
            this.label15.Text = "מס\' טלפון:";
            // 
            // telephoneNumTxt
            // 
            this.telephoneNumTxt.Font = new System.Drawing.Font("Narkisim", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.telephoneNumTxt.Location = new System.Drawing.Point(334, 263);
            this.telephoneNumTxt.Name = "telephoneNumTxt";
            this.telephoneNumTxt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.telephoneNumTxt.Size = new System.Drawing.Size(140, 28);
            this.telephoneNumTxt.TabIndex = 6;
            // 
            // label45
            // 
            this.label45.AutoSize = true;
            this.label45.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label45.Location = new System.Drawing.Point(480, 344);
            this.label45.Name = "label45";
            this.label45.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label45.Size = new System.Drawing.Size(135, 22);
            this.label45.TabIndex = 64;
            this.label45.Text = "תוקף הרישיון עד-";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label22.Location = new System.Drawing.Point(516, 228);
            this.label22.Name = "label22";
            this.label22.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label22.Size = new System.Drawing.Size(99, 22);
            this.label22.TabIndex = 61;
            this.label22.Text = "מס\' פלאפון:";
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label44.Location = new System.Drawing.Point(472, 304);
            this.label44.Name = "label44";
            this.label44.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label44.Size = new System.Drawing.Size(143, 22);
            this.label44.TabIndex = 62;
            this.label44.Text = "הונפקה הרישיון ב-";
            // 
            // phoneNumTxt
            // 
            this.phoneNumTxt.Font = new System.Drawing.Font("Narkisim", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.phoneNumTxt.Location = new System.Drawing.Point(334, 228);
            this.phoneNumTxt.Name = "phoneNumTxt";
            this.phoneNumTxt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.phoneNumTxt.Size = new System.Drawing.Size(140, 28);
            this.phoneNumTxt.TabIndex = 5;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label26.Location = new System.Drawing.Point(516, 190);
            this.label26.Name = "label26";
            this.label26.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label26.Size = new System.Drawing.Size(99, 22);
            this.label26.TabIndex = 63;
            this.label26.Text = "תאריך לידה:";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label27.Location = new System.Drawing.Point(553, 150);
            this.label27.Name = "label27";
            this.label27.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label27.Size = new System.Drawing.Size(62, 22);
            this.label27.TabIndex = 65;
            this.label27.Text = "כתובת:";
            // 
            // addressTxt
            // 
            this.addressTxt.Font = new System.Drawing.Font("Narkisim", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addressTxt.Location = new System.Drawing.Point(334, 147);
            this.addressTxt.Name = "addressTxt";
            this.addressTxt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.addressTxt.Size = new System.Drawing.Size(140, 28);
            this.addressTxt.TabIndex = 3;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label28.Location = new System.Drawing.Point(516, 108);
            this.label28.Name = "label28";
            this.label28.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label28.Size = new System.Drawing.Size(99, 22);
            this.label28.TabIndex = 66;
            this.label28.Text = "שם משפחה:";
            // 
            // lastNameTxt
            // 
            this.lastNameTxt.Font = new System.Drawing.Font("Narkisim", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lastNameTxt.Location = new System.Drawing.Point(334, 105);
            this.lastNameTxt.Name = "lastNameTxt";
            this.lastNameTxt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lastNameTxt.Size = new System.Drawing.Size(140, 28);
            this.lastNameTxt.TabIndex = 2;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label29.Location = new System.Drawing.Point(537, 68);
            this.label29.Name = "label29";
            this.label29.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label29.Size = new System.Drawing.Size(78, 22);
            this.label29.TabIndex = 67;
            this.label29.Text = "שם פרטי:";
            // 
            // firstNameTxt
            // 
            this.firstNameTxt.Font = new System.Drawing.Font("Narkisim", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.firstNameTxt.Location = new System.Drawing.Point(334, 65);
            this.firstNameTxt.Name = "firstNameTxt";
            this.firstNameTxt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.firstNameTxt.Size = new System.Drawing.Size(140, 28);
            this.firstNameTxt.TabIndex = 1;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label30.Location = new System.Drawing.Point(575, 31);
            this.label30.Name = "label30";
            this.label30.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label30.Size = new System.Drawing.Size(40, 22);
            this.label30.TabIndex = 68;
            this.label30.Text = "ת\"ז:";
            // 
            // idNumTxt
            // 
            this.idNumTxt.Font = new System.Drawing.Font("Narkisim", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.idNumTxt.Location = new System.Drawing.Point(334, 28);
            this.idNumTxt.Name = "idNumTxt";
            this.idNumTxt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.idNumTxt.Size = new System.Drawing.Size(140, 28);
            this.idNumTxt.TabIndex = 0;
            // 
            // addCustomerBtn
            // 
            this.addCustomerBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.addCustomerBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.addCustomerBtn.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.addCustomerBtn.Location = new System.Drawing.Point(102, 363);
            this.addCustomerBtn.Name = "addCustomerBtn";
            this.addCustomerBtn.Size = new System.Drawing.Size(119, 50);
            this.addCustomerBtn.TabIndex = 9;
            this.addCustomerBtn.Text = "הוספת לקוח";
            this.addCustomerBtn.UseVisualStyleBackColor = true;
            this.addCustomerBtn.Click += new System.EventHandler(this.addCustomerBtn_Click);
            // 
            // AddCustomer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(620, 506);
            this.Controls.Add(this.addCustomerBtn);
            this.Controls.Add(this.licenseExpiryDate);
            this.Controls.Add(this.licenseIssueDate);
            this.Controls.Add(this.BirthdateTimePicker);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.telephoneNumTxt);
            this.Controls.Add(this.label45);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.label44);
            this.Controls.Add(this.phoneNumTxt);
            this.Controls.Add(this.label26);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.addressTxt);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.lastNameTxt);
            this.Controls.Add(this.label29);
            this.Controls.Add(this.firstNameTxt);
            this.Controls.Add(this.label30);
            this.Controls.Add(this.idNumTxt);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(636, 545);
            this.MinimumSize = new System.Drawing.Size(636, 545);
            this.Name = "AddCustomer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ניהול עסק השכרת רכב";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker licenseExpiryDate;
        private System.Windows.Forms.DateTimePicker licenseIssueDate;
        private System.Windows.Forms.DateTimePicker BirthdateTimePicker;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox telephoneNumTxt;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.TextBox phoneNumTxt;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TextBox addressTxt;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.TextBox lastNameTxt;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox firstNameTxt;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.TextBox idNumTxt;
        private System.Windows.Forms.Button addCustomerBtn;
    }
}