namespace RentCarApp
{
    partial class AddCar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddCar));
            this.engineCapacityTxt = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.PaymentNum = new System.Windows.Forms.NumericUpDown();
            this.ProductiondateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.distanceTxt = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.carManufacturerTxt = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.carNumTxt = new System.Windows.Forms.TextBox();
            this.addCarBtn = new System.Windows.Forms.Button();
            this.carModelTxt = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.insuranceExpiryDateTime = new System.Windows.Forms.DateTimePicker();
            this.licensingExpiryDateTime = new System.Windows.Forms.DateTimePicker();
            this.doorsNumTxt = new System.Windows.Forms.ComboBox();
            this.seatsNumTxt = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.gearboxTypeTxt = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.carColorTxt = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.fuelTypeTxt = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.fuelCapacityTxt = new System.Windows.Forms.ComboBox();
            this.label16 = new System.Windows.Forms.Label();
            this.carActiveTxt = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.dateAddedDatetime = new System.Windows.Forms.DateTimePicker();
            this.label18 = new System.Windows.Forms.Label();
            this.carAvailabeTxt = new System.Windows.Forms.ComboBox();
            this.dateRemove = new System.Windows.Forms.DateTimePicker();
            this.label19 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.carStatusTxt = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.PaymentNum)).BeginInit();
            this.SuspendLayout();
            // 
            // engineCapacityTxt
            // 
            this.engineCapacityTxt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.engineCapacityTxt.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.engineCapacityTxt.FormattingEnabled = true;
            this.engineCapacityTxt.ItemHeight = 18;
            this.engineCapacityTxt.Items.AddRange(new object[] {
            "900",
            "1100",
            "1400",
            "1600"});
            this.engineCapacityTxt.Location = new System.Drawing.Point(382, 142);
            this.engineCapacityTxt.Name = "engineCapacityTxt";
            this.engineCapacityTxt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.engineCapacityTxt.Size = new System.Drawing.Size(140, 26);
            this.engineCapacityTxt.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label9.Location = new System.Drawing.Point(599, 146);
            this.label9.Name = "label9";
            this.label9.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label9.Size = new System.Drawing.Size(47, 22);
            this.label9.TabIndex = 29;
            this.label9.Text = "מנוע:";
            // 
            // PaymentNum
            // 
            this.PaymentNum.DecimalPlaces = 1;
            this.PaymentNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.PaymentNum.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.PaymentNum.Location = new System.Drawing.Point(382, 401);
            this.PaymentNum.Maximum = new decimal(new int[] {
            250,
            0,
            0,
            0});
            this.PaymentNum.Minimum = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.PaymentNum.Name = "PaymentNum";
            this.PaymentNum.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.PaymentNum.Size = new System.Drawing.Size(140, 26);
            this.PaymentNum.TabIndex = 10;
            this.PaymentNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.PaymentNum.UpDownAlign = System.Windows.Forms.LeftRightAlignment.Left;
            this.PaymentNum.Value = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.PaymentNum.Validating += new System.ComponentModel.CancelEventHandler(this.PaymentNum_Validating);
            // 
            // ProductiondateTimePicker
            // 
            this.ProductiondateTimePicker.CustomFormat = "yyyy";
            this.ProductiondateTimePicker.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.ProductiondateTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.ProductiondateTimePicker.Location = new System.Drawing.Point(382, 255);
            this.ProductiondateTimePicker.MaxDate = new System.DateTime(2050, 12, 31, 0, 0, 0, 0);
            this.ProductiondateTimePicker.MinDate = new System.DateTime(1990, 1, 1, 0, 0, 0, 0);
            this.ProductiondateTimePicker.Name = "ProductiondateTimePicker";
            this.ProductiondateTimePicker.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.ProductiondateTimePicker.RightToLeftLayout = true;
            this.ProductiondateTimePicker.ShowUpDown = true;
            this.ProductiondateTimePicker.Size = new System.Drawing.Size(140, 26);
            this.ProductiondateTimePicker.TabIndex = 6;
            this.ProductiondateTimePicker.Value = new System.DateTime(2019, 7, 10, 0, 0, 0, 0);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label8.Location = new System.Drawing.Point(548, 402);
            this.label8.Name = "label8";
            this.label8.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label8.Size = new System.Drawing.Size(98, 22);
            this.label8.TabIndex = 25;
            this.label8.Text = "תשלום ליום:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(545, 332);
            this.label7.Name = "label7";
            this.label7.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label7.Size = new System.Drawing.Size(101, 22);
            this.label7.TabIndex = 24;
            this.label7.Text = "מס\' מושבים:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(553, 295);
            this.label6.Name = "label6";
            this.label6.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label6.Size = new System.Drawing.Size(93, 22);
            this.label6.TabIndex = 23;
            this.label6.Text = "מס\' דלתות:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(567, 255);
            this.label5.Name = "label5";
            this.label5.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label5.Size = new System.Drawing.Size(79, 22);
            this.label5.TabIndex = 22;
            this.label5.Text = "שנת יצור:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(536, 217);
            this.label4.Name = "label4";
            this.label4.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label4.Size = new System.Drawing.Size(110, 22);
            this.label4.TabIndex = 26;
            this.label4.Text = "תוקף הביטוח:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(536, 180);
            this.label3.Name = "label3";
            this.label3.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label3.Size = new System.Drawing.Size(110, 22);
            this.label3.TabIndex = 21;
            this.label3.Text = "תוקף הרישיון:";
            // 
            // distanceTxt
            // 
            this.distanceTxt.Font = new System.Drawing.Font("Narkisim", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.distanceTxt.Location = new System.Drawing.Point(382, 471);
            this.distanceTxt.Name = "distanceTxt";
            this.distanceTxt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.distanceTxt.Size = new System.Drawing.Size(140, 28);
            this.distanceTxt.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(603, 66);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label2.Size = new System.Drawing.Size(43, 22);
            this.label2.TabIndex = 20;
            this.label2.Text = "יצרן:";
            // 
            // carManufacturerTxt
            // 
            this.carManufacturerTxt.Font = new System.Drawing.Font("Narkisim", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.carManufacturerTxt.Location = new System.Drawing.Point(382, 65);
            this.carManufacturerTxt.Name = "carManufacturerTxt";
            this.carManufacturerTxt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.carManufacturerTxt.Size = new System.Drawing.Size(140, 28);
            this.carManufacturerTxt.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(572, 28);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label1.Size = new System.Drawing.Size(74, 22);
            this.label1.TabIndex = 19;
            this.label1.Text = "מס\' רכב:";
            // 
            // carNumTxt
            // 
            this.carNumTxt.Font = new System.Drawing.Font("Narkisim", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.carNumTxt.Location = new System.Drawing.Point(382, 25);
            this.carNumTxt.Name = "carNumTxt";
            this.carNumTxt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.carNumTxt.Size = new System.Drawing.Size(140, 28);
            this.carNumTxt.TabIndex = 0;
            // 
            // addCarBtn
            // 
            this.addCarBtn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.addCarBtn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.addCarBtn.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.addCarBtn.Location = new System.Drawing.Point(34, 523);
            this.addCarBtn.Name = "addCarBtn";
            this.addCarBtn.Size = new System.Drawing.Size(119, 50);
            this.addCarBtn.TabIndex = 20;
            this.addCarBtn.Text = "הוספת רכב";
            this.addCarBtn.UseVisualStyleBackColor = true;
            this.addCarBtn.Click += new System.EventHandler(this.addCarBtn_Click);
            // 
            // carModelTxt
            // 
            this.carModelTxt.Font = new System.Drawing.Font("Narkisim", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.carModelTxt.Location = new System.Drawing.Point(382, 102);
            this.carModelTxt.Name = "carModelTxt";
            this.carModelTxt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.carModelTxt.Size = new System.Drawing.Size(140, 28);
            this.carModelTxt.TabIndex = 2;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label10.Location = new System.Drawing.Point(604, 103);
            this.label10.Name = "label10";
            this.label10.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label10.Size = new System.Drawing.Size(42, 22);
            this.label10.TabIndex = 20;
            this.label10.Text = "דגם:";
            // 
            // insuranceExpiryDateTime
            // 
            this.insuranceExpiryDateTime.CustomFormat = "dd/MM/yyyy";
            this.insuranceExpiryDateTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.insuranceExpiryDateTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.insuranceExpiryDateTime.Location = new System.Drawing.Point(382, 217);
            this.insuranceExpiryDateTime.MinDate = new System.DateTime(2019, 1, 1, 0, 0, 0, 0);
            this.insuranceExpiryDateTime.Name = "insuranceExpiryDateTime";
            this.insuranceExpiryDateTime.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.insuranceExpiryDateTime.RightToLeftLayout = true;
            this.insuranceExpiryDateTime.Size = new System.Drawing.Size(140, 26);
            this.insuranceExpiryDateTime.TabIndex = 5;
            // 
            // licensingExpiryDateTime
            // 
            this.licensingExpiryDateTime.CustomFormat = "dd/MM/yyyy";
            this.licensingExpiryDateTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.licensingExpiryDateTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.licensingExpiryDateTime.Location = new System.Drawing.Point(382, 180);
            this.licensingExpiryDateTime.MinDate = new System.DateTime(2019, 1, 1, 0, 0, 0, 0);
            this.licensingExpiryDateTime.Name = "licensingExpiryDateTime";
            this.licensingExpiryDateTime.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.licensingExpiryDateTime.RightToLeftLayout = true;
            this.licensingExpiryDateTime.Size = new System.Drawing.Size(140, 26);
            this.licensingExpiryDateTime.TabIndex = 4;
            // 
            // doorsNumTxt
            // 
            this.doorsNumTxt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.doorsNumTxt.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.doorsNumTxt.FormattingEnabled = true;
            this.doorsNumTxt.ItemHeight = 18;
            this.doorsNumTxt.Items.AddRange(new object[] {
            "2",
            "3",
            "4",
            "5"});
            this.doorsNumTxt.Location = new System.Drawing.Point(382, 294);
            this.doorsNumTxt.Name = "doorsNumTxt";
            this.doorsNumTxt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.doorsNumTxt.Size = new System.Drawing.Size(140, 26);
            this.doorsNumTxt.TabIndex = 7;
            // 
            // seatsNumTxt
            // 
            this.seatsNumTxt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.seatsNumTxt.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.seatsNumTxt.FormattingEnabled = true;
            this.seatsNumTxt.ItemHeight = 18;
            this.seatsNumTxt.Items.AddRange(new object[] {
            "2",
            "4",
            "5"});
            this.seatsNumTxt.Location = new System.Drawing.Point(382, 331);
            this.seatsNumTxt.Name = "seatsNumTxt";
            this.seatsNumTxt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.seatsNumTxt.Size = new System.Drawing.Size(140, 26);
            this.seatsNumTxt.TabIndex = 8;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label11.Location = new System.Drawing.Point(610, 368);
            this.label11.Name = "label11";
            this.label11.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label11.Size = new System.Drawing.Size(36, 22);
            this.label11.TabIndex = 24;
            this.label11.Text = "גיר:";
            // 
            // gearboxTypeTxt
            // 
            this.gearboxTypeTxt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gearboxTypeTxt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.gearboxTypeTxt.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gearboxTypeTxt.FormattingEnabled = true;
            this.gearboxTypeTxt.ItemHeight = 18;
            this.gearboxTypeTxt.Items.AddRange(new object[] {
            "ידני",
            "אוטומטי",
            "רובוטי"});
            this.gearboxTypeTxt.Location = new System.Drawing.Point(382, 367);
            this.gearboxTypeTxt.Name = "gearboxTypeTxt";
            this.gearboxTypeTxt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gearboxTypeTxt.Size = new System.Drawing.Size(140, 26);
            this.gearboxTypeTxt.TabIndex = 9;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label12.Location = new System.Drawing.Point(602, 438);
            this.label12.Name = "label12";
            this.label12.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label12.Size = new System.Drawing.Size(44, 22);
            this.label12.TabIndex = 19;
            this.label12.Text = "צבע:";
            // 
            // carColorTxt
            // 
            this.carColorTxt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.carColorTxt.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.carColorTxt.FormattingEnabled = true;
            this.carColorTxt.ItemHeight = 18;
            this.carColorTxt.Items.AddRange(new object[] {
            "לבן",
            "כחול",
            "שחור",
            "כסף"});
            this.carColorTxt.Location = new System.Drawing.Point(382, 435);
            this.carColorTxt.Name = "carColorTxt";
            this.carColorTxt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.carColorTxt.Size = new System.Drawing.Size(140, 26);
            this.carColorTxt.TabIndex = 11;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label13.Location = new System.Drawing.Point(602, 472);
            this.label13.Name = "label13";
            this.label13.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label13.Size = new System.Drawing.Size(44, 22);
            this.label13.TabIndex = 19;
            this.label13.Text = "ק\"ם:";
            // 
            // fuelTypeTxt
            // 
            this.fuelTypeTxt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fuelTypeTxt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.fuelTypeTxt.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fuelTypeTxt.FormattingEnabled = true;
            this.fuelTypeTxt.Items.AddRange(new object[] {
            "בנזין",
            "סולר",
            "גז"});
            this.fuelTypeTxt.Location = new System.Drawing.Point(382, 510);
            this.fuelTypeTxt.Name = "fuelTypeTxt";
            this.fuelTypeTxt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.fuelTypeTxt.Size = new System.Drawing.Size(140, 26);
            this.fuelTypeTxt.TabIndex = 13;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label14.Location = new System.Drawing.Point(574, 511);
            this.label14.Name = "label14";
            this.label14.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label14.Size = new System.Drawing.Size(72, 22);
            this.label14.TabIndex = 19;
            this.label14.Text = "סוג דלק:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label15.Location = new System.Drawing.Point(558, 550);
            this.label15.Name = "label15";
            this.label15.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label15.Size = new System.Drawing.Size(88, 22);
            this.label15.TabIndex = 19;
            this.label15.Text = " כמות דלק:";
            // 
            // fuelCapacityTxt
            // 
            this.fuelCapacityTxt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fuelCapacityTxt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.fuelCapacityTxt.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fuelCapacityTxt.FormattingEnabled = true;
            this.fuelCapacityTxt.Items.AddRange(new object[] {
            "1/8",
            "2/8",
            "3/8",
            "4/8",
            "5/8",
            "6/8",
            "7/8",
            "8/8"});
            this.fuelCapacityTxt.Location = new System.Drawing.Point(382, 546);
            this.fuelCapacityTxt.Name = "fuelCapacityTxt";
            this.fuelCapacityTxt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.fuelCapacityTxt.Size = new System.Drawing.Size(140, 26);
            this.fuelCapacityTxt.TabIndex = 14;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label16.Location = new System.Drawing.Point(215, 26);
            this.label16.Name = "label16";
            this.label16.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label16.Size = new System.Drawing.Size(85, 22);
            this.label16.TabIndex = 19;
            this.label16.Text = " רכב פעיל:";
            this.label16.Visible = false;
            // 
            // carActiveTxt
            // 
            this.carActiveTxt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.carActiveTxt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.carActiveTxt.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.carActiveTxt.FormattingEnabled = true;
            this.carActiveTxt.Items.AddRange(new object[] {
            "פעיל",
            "לא פעיל"});
            this.carActiveTxt.Location = new System.Drawing.Point(26, 26);
            this.carActiveTxt.Name = "carActiveTxt";
            this.carActiveTxt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.carActiveTxt.Size = new System.Drawing.Size(140, 26);
            this.carActiveTxt.TabIndex = 15;
            this.carActiveTxt.Visible = false;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label17.Location = new System.Drawing.Point(185, 104);
            this.label17.Name = "label17";
            this.label17.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label17.Size = new System.Drawing.Size(115, 22);
            this.label17.TabIndex = 21;
            this.label17.Text = "תאריך הוספה:";
            this.label17.Visible = false;
            // 
            // dateAddedDatetime
            // 
            this.dateAddedDatetime.CustomFormat = "dd/MM/yyyy";
            this.dateAddedDatetime.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dateAddedDatetime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateAddedDatetime.Location = new System.Drawing.Point(26, 104);
            this.dateAddedDatetime.MinDate = new System.DateTime(1930, 1, 1, 0, 0, 0, 0);
            this.dateAddedDatetime.Name = "dateAddedDatetime";
            this.dateAddedDatetime.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dateAddedDatetime.RightToLeftLayout = true;
            this.dateAddedDatetime.Size = new System.Drawing.Size(140, 26);
            this.dateAddedDatetime.TabIndex = 17;
            this.dateAddedDatetime.Visible = false;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label18.Location = new System.Drawing.Point(223, 66);
            this.label18.Name = "label18";
            this.label18.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label18.Size = new System.Drawing.Size(77, 22);
            this.label18.TabIndex = 19;
            this.label18.Text = " רכב זמין:";
            this.label18.Visible = false;
            // 
            // carAvailabeTxt
            // 
            this.carAvailabeTxt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.carAvailabeTxt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.carAvailabeTxt.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.carAvailabeTxt.FormattingEnabled = true;
            this.carAvailabeTxt.Items.AddRange(new object[] {
            "זמין",
            "לא זמין"});
            this.carAvailabeTxt.Location = new System.Drawing.Point(26, 66);
            this.carAvailabeTxt.Name = "carAvailabeTxt";
            this.carAvailabeTxt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.carAvailabeTxt.Size = new System.Drawing.Size(140, 26);
            this.carAvailabeTxt.TabIndex = 16;
            this.carAvailabeTxt.Visible = false;
            // 
            // dateRemove
            // 
            this.dateRemove.CustomFormat = "dd/MM/yyyy";
            this.dateRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.dateRemove.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateRemove.Location = new System.Drawing.Point(26, 143);
            this.dateRemove.MinDate = new System.DateTime(1930, 1, 1, 0, 0, 0, 0);
            this.dateRemove.Name = "dateRemove";
            this.dateRemove.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dateRemove.RightToLeftLayout = true;
            this.dateRemove.Size = new System.Drawing.Size(140, 26);
            this.dateRemove.TabIndex = 18;
            this.dateRemove.Visible = false;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label19.Location = new System.Drawing.Point(192, 142);
            this.label19.Name = "label19";
            this.label19.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label19.Size = new System.Drawing.Size(114, 22);
            this.label19.TabIndex = 33;
            this.label19.Text = "תאריך מחיקה:";
            this.label19.Visible = false;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Font = new System.Drawing.Font("Playbill", 16F, System.Drawing.FontStyle.Bold);
            this.label25.Location = new System.Drawing.Point(212, 180);
            this.label25.Name = "label25";
            this.label25.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label25.Size = new System.Drawing.Size(88, 22);
            this.label25.TabIndex = 61;
            this.label25.Text = "מצב הרכב:";
            // 
            // carStatusTxt
            // 
            this.carStatusTxt.Font = new System.Drawing.Font("Gisha", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.carStatusTxt.Location = new System.Drawing.Point(26, 206);
            this.carStatusTxt.Multiline = true;
            this.carStatusTxt.Name = "carStatusTxt";
            this.carStatusTxt.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.carStatusTxt.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.carStatusTxt.Size = new System.Drawing.Size(270, 278);
            this.carStatusTxt.TabIndex = 19;
            this.carStatusTxt.Text = "הכל תקין";
            // 
            // AddCar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(670, 585);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.carStatusTxt);
            this.Controls.Add(this.dateRemove);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.gearboxTypeTxt);
            this.Controls.Add(this.seatsNumTxt);
            this.Controls.Add(this.doorsNumTxt);
            this.Controls.Add(this.carAvailabeTxt);
            this.Controls.Add(this.carActiveTxt);
            this.Controls.Add(this.fuelCapacityTxt);
            this.Controls.Add(this.fuelTypeTxt);
            this.Controls.Add(this.carColorTxt);
            this.Controls.Add(this.engineCapacityTxt);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.PaymentNum);
            this.Controls.Add(this.dateAddedDatetime);
            this.Controls.Add(this.licensingExpiryDateTime);
            this.Controls.Add(this.insuranceExpiryDateTime);
            this.Controls.Add(this.ProductiondateTimePicker);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.distanceTxt);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.carModelTxt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.carManufacturerTxt);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.carNumTxt);
            this.Controls.Add(this.addCarBtn);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(686, 624);
            this.MinimumSize = new System.Drawing.Size(686, 624);
            this.Name = "AddCar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ניהול עסק השכרת רכב";
            ((System.ComponentModel.ISupportInitialize)(this.PaymentNum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox engineCapacityTxt;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown PaymentNum;
        private System.Windows.Forms.DateTimePicker ProductiondateTimePicker;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox distanceTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox carManufacturerTxt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox carNumTxt;
        private System.Windows.Forms.Button addCarBtn;
        private System.Windows.Forms.TextBox carModelTxt;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DateTimePicker insuranceExpiryDateTime;
        private System.Windows.Forms.DateTimePicker licensingExpiryDateTime;
        private System.Windows.Forms.ComboBox doorsNumTxt;
        private System.Windows.Forms.ComboBox seatsNumTxt;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox gearboxTypeTxt;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox carColorTxt;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox fuelTypeTxt;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox fuelCapacityTxt;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox carActiveTxt;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.DateTimePicker dateAddedDatetime;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.ComboBox carAvailabeTxt;
        private System.Windows.Forms.DateTimePicker dateRemove;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox carStatusTxt;
    }
}