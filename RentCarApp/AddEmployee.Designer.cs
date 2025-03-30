namespace RentCarApp
{
    partial class AddEmployee
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddEmployee));
            this.addEmployeeBtn = new System.Windows.Forms.Button();
            this.idNumTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.firstNameTxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lastNameTxt = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.addressTxt = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.phoneNumTxt = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.telephoneNumTxt = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.BirthdateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.SalaryNum = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.jobTxt = new System.Windows.Forms.ComboBox();
            this.activeCB = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.SalaryNum)).BeginInit();
            this.SuspendLayout();
            // 
            // addEmployeeBtn
            // 
            this.addEmployeeBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.addEmployeeBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.addEmployeeBtn.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.addEmployeeBtn.Location = new System.Drawing.Point(102, 363);
            this.addEmployeeBtn.Name = "addEmployeeBtn";
            this.addEmployeeBtn.Size = new System.Drawing.Size(119, 50);
            this.addEmployeeBtn.TabIndex = 11;
            this.addEmployeeBtn.Text = "הוספת עובד";
            this.addEmployeeBtn.UseVisualStyleBackColor = true;
            this.addEmployeeBtn.Click += new System.EventHandler(this.AddEmployeeBtn_Click);
            // 
            // idNumTxt
            // 
            this.idNumTxt.Font = new System.Drawing.Font("Narkisim", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.idNumTxt.Location = new System.Drawing.Point(357, 28);
            this.idNumTxt.Name = "idNumTxt";
            this.idNumTxt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.idNumTxt.Size = new System.Drawing.Size(140, 28);
            this.idNumTxt.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(575, 31);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label1.Size = new System.Drawing.Size(40, 22);
            this.label1.TabIndex = 6;
            this.label1.Text = "ת\"ז:";
            // 
            // firstNameTxt
            // 
            this.firstNameTxt.Font = new System.Drawing.Font("Narkisim", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.firstNameTxt.Location = new System.Drawing.Point(357, 105);
            this.firstNameTxt.Name = "firstNameTxt";
            this.firstNameTxt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.firstNameTxt.Size = new System.Drawing.Size(140, 28);
            this.firstNameTxt.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(536, 108);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label2.Size = new System.Drawing.Size(78, 22);
            this.label2.TabIndex = 6;
            this.label2.Text = "שם פרטי:";
            // 
            // lastNameTxt
            // 
            this.lastNameTxt.Font = new System.Drawing.Font("Narkisim", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lastNameTxt.Location = new System.Drawing.Point(357, 147);
            this.lastNameTxt.Name = "lastNameTxt";
            this.lastNameTxt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lastNameTxt.Size = new System.Drawing.Size(140, 28);
            this.lastNameTxt.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(516, 150);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label3.Size = new System.Drawing.Size(99, 22);
            this.label3.TabIndex = 6;
            this.label3.Text = "שם משפחה:";
            // 
            // addressTxt
            // 
            this.addressTxt.Font = new System.Drawing.Font("Narkisim", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addressTxt.Location = new System.Drawing.Point(357, 187);
            this.addressTxt.Name = "addressTxt";
            this.addressTxt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.addressTxt.Size = new System.Drawing.Size(140, 28);
            this.addressTxt.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(549, 190);
            this.label4.Name = "label4";
            this.label4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label4.Size = new System.Drawing.Size(62, 22);
            this.label4.TabIndex = 6;
            this.label4.Text = "כתובת:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(510, 228);
            this.label5.Name = "label5";
            this.label5.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label5.Size = new System.Drawing.Size(99, 22);
            this.label5.TabIndex = 6;
            this.label5.Text = "תאריך לידה:";
            // 
            // phoneNumTxt
            // 
            this.phoneNumTxt.Font = new System.Drawing.Font("Narkisim", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.phoneNumTxt.Location = new System.Drawing.Point(357, 263);
            this.phoneNumTxt.Name = "phoneNumTxt";
            this.phoneNumTxt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.phoneNumTxt.Size = new System.Drawing.Size(140, 28);
            this.phoneNumTxt.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(515, 263);
            this.label6.Name = "label6";
            this.label6.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label6.Size = new System.Drawing.Size(99, 22);
            this.label6.TabIndex = 6;
            this.label6.Text = "מס\' פלאפון:";
            // 
            // telephoneNumTxt
            // 
            this.telephoneNumTxt.Font = new System.Drawing.Font("Narkisim", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.telephoneNumTxt.Location = new System.Drawing.Point(357, 301);
            this.telephoneNumTxt.Name = "telephoneNumTxt";
            this.telephoneNumTxt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.telephoneNumTxt.Size = new System.Drawing.Size(140, 28);
            this.telephoneNumTxt.TabIndex = 8;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(525, 304);
            this.label7.Name = "label7";
            this.label7.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label7.Size = new System.Drawing.Size(87, 22);
            this.label7.TabIndex = 6;
            this.label7.Text = "מס\' טלפון:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label8.Location = new System.Drawing.Point(568, 344);
            this.label8.Name = "label8";
            this.label8.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label8.Size = new System.Drawing.Size(45, 22);
            this.label8.TabIndex = 6;
            this.label8.Text = "שכר:";
            // 
            // BirthdateTimePicker
            // 
            this.BirthdateTimePicker.CustomFormat = "dd/MM/yyyy";
            this.BirthdateTimePicker.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.BirthdateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.BirthdateTimePicker.Location = new System.Drawing.Point(357, 228);
            this.BirthdateTimePicker.MinDate = new System.DateTime(1930, 1, 1, 0, 0, 0, 0);
            this.BirthdateTimePicker.Name = "BirthdateTimePicker";
            this.BirthdateTimePicker.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.BirthdateTimePicker.RightToLeftLayout = true;
            this.BirthdateTimePicker.Size = new System.Drawing.Size(140, 26);
            this.BirthdateTimePicker.TabIndex = 6;
            // 
            // SalaryNum
            // 
            this.SalaryNum.DecimalPlaces = 1;
            this.SalaryNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.SalaryNum.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.SalaryNum.Location = new System.Drawing.Point(357, 343);
            this.SalaryNum.Maximum = new decimal(new int[] {
            6500,
            0,
            0,
            0});
            this.SalaryNum.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.SalaryNum.Name = "SalaryNum";
            this.SalaryNum.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.SalaryNum.Size = new System.Drawing.Size(140, 26);
            this.SalaryNum.TabIndex = 9;
            this.SalaryNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.SalaryNum.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.SalaryNum.Value = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.SalaryNum.Validating += new System.ComponentModel.CancelEventHandler(this.SalaryNum_Validating);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label9.Location = new System.Drawing.Point(554, 68);
            this.label9.Name = "label9";
            this.label9.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label9.Size = new System.Drawing.Size(62, 22);
            this.label9.TabIndex = 10;
            this.label9.Text = "תפקיד:";
            // 
            // jobTxt
            // 
            this.jobTxt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.jobTxt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.jobTxt.Font = new System.Drawing.Font("Playbill", 14F);
            this.jobTxt.FormattingEnabled = true;
            this.jobTxt.Items.AddRange(new object[] {
            "מנהל",
            "מזכירות"});
            this.jobTxt.Location = new System.Drawing.Point(357, 65);
            this.jobTxt.Name = "jobTxt";
            this.jobTxt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.jobTxt.Size = new System.Drawing.Size(140, 27);
            this.jobTxt.TabIndex = 1;
            // 
            // activeCB
            // 
            this.activeCB.AutoSize = true;
            this.activeCB.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.activeCB.Location = new System.Drawing.Point(370, 387);
            this.activeCB.Name = "activeCB";
            this.activeCB.Size = new System.Drawing.Size(127, 26);
            this.activeCB.TabIndex = 10;
            this.activeCB.Text = "לחזיר לעבודה";
            this.activeCB.UseVisualStyleBackColor = true;
            this.activeCB.Visible = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label10.Location = new System.Drawing.Point(562, 387);
            this.label10.Name = "label10";
            this.label10.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label10.Size = new System.Drawing.Size(49, 22);
            this.label10.TabIndex = 14;
            this.label10.Text = "פעיל:";
            this.label10.Visible = false;
            // 
            // AddEmployee
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 506);
            this.Controls.Add(this.activeCB);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.jobTxt);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.SalaryNum);
            this.Controls.Add(this.BirthdateTimePicker);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.telephoneNumTxt);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.phoneNumTxt);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.addressTxt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lastNameTxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.firstNameTxt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.idNumTxt);
            this.Controls.Add(this.addEmployeeBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(636, 545);
            this.MinimumSize = new System.Drawing.Size(636, 545);
            this.Name = "AddEmployee";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ניהול עסק השכרת רכב";
            this.Load += new System.EventHandler(this.AddEmployee_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SalaryNum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button addEmployeeBtn;
        private System.Windows.Forms.TextBox idNumTxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox firstNameTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox lastNameTxt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox addressTxt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox phoneNumTxt;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox telephoneNumTxt;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker BirthdateTimePicker;
        private System.Windows.Forms.NumericUpDown SalaryNum;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox jobTxt;
        private System.Windows.Forms.CheckBox activeCB;
        private System.Windows.Forms.Label label10;
    }
}