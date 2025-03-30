using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Font = iTextSharp.text.Font;
using Image = iTextSharp.text.Image;
namespace RentCarApp
{
    public partial class AddRent : Form
    {
        private DbManager dbManger;
        private Manager mangerFrm;
        private bool dontRunHandler = true;
        private DataSet allCar;
        private DataSet allDrivers;
        private decimal v = 150;
        private int checkCarNum = -1;
        private int checkDriverId = -1;
        private Driver_Licensing[] drivers;
        private string oldDistance;
        private Rent_Details details;
        private string employeeID;
        private bool returnCar;//true return or update button false to rent car button
        private AutoCompleteStringCollection collection;
        private double oldPayment;
        private double payed=0;
        private int choice_update;
        private bool saveBtn_WasClicked = false;
        private bool Date = false;
        private Document doc;
        private PdfPTable myTable;
       
        public enum RentType
        {
            Update,
            UpdateReturn,
            Add,
            Return
        }
        RentType rentMode;
        public AddRent(Manager m, string idEmployeeb, bool returnCarB, RentType mode)
        {
            InitializeComponent();
            dbManger = new DbManager();
            employeeID = idEmployeeb;
            this.mangerFrm = m;
            allCar = dbManger.AllCar_toRent();
            allDrivers = dbManger.AllDrivers();
            returnCar = returnCarB;
            DateTime date = new DateTime(rentDateTime.Value.Year, rentDateTime.Value.Month, rentDateTime.Value.Date.Day, rentTime.Value.Hour, rentDateTime.Value.Minute, rentDateTime.Value.Second);// + " "+rentTime.Value.TimeOfDay.ToString());
            rentMode = mode;
            //Console.WriteLine(date.ToString());
            //string c = rentDateTime.Value.Date.ToString("dd/MM/yyyy ");// + rentTime.Value.ToString("HH:mm:ss");
            // date = Convert.ToDateTime(c);// rentDateTime.Value.Date.ToString("dd/MM/yyyy")+ rentTime.Value.TimeOfDay.ToString(" HH:mm:ss")
            // Console.WriteLine(date.Date.ToString());
            rentTypeTxt.SelectedIndex = 0;
            PayStatusCB.SelectedIndex = 1;
            if (rentMode == RentType.Add)
            {
                returnGB.Enabled = false;
                saveDocumentBtn.Enabled = false;
                printBtn.Enabled = false;
                //saveBtn.Enabled = false;
            }
            else if (rentMode == RentType.Return)
            {
                //rentGB.Enabled = false;
                EnableRentGB();
                carGB.Enabled = false;
                saveBtn.Text = "החזרה";
            }
            else if (rentMode == RentType.Update)
            {
                Update_Show(1);
                //saveBtn.Enabled = false;
            }
            else if (rentMode == RentType.UpdateReturn)
                Update_Show(0);
        }
        
        /*פונקציה למלאות את השדות של השכירות בזמן שרוצים להחזיר רכב */
        public void Return_Rent(DataGridViewRow row)
        {
            rentIdTxt.Text = row.Cells[0].Value.ToString();
            rentDateTime.Text = row.Cells[15].Value.ToString();
            returnDate.Text = row.Cells[16].Value.ToString();
            rentTime.Text = row.Cells[15].Value.ToString();
            carNumTxt.Text = row.Cells[6].Value.ToString();
            PaymentNum.Value = decimal.Parse(row.Cells[11].Value.ToString());
            oldPayment = double.Parse(sumPaymentTxt.Text.ToString());
            returnedFuelCapacityTxt.Text = fuelCapacity1Txt.Text = row.Cells[20].Value.ToString();
            distance1Txt.Text = row.Cells[18].Value.ToString();
            returnDate.MinDate = returnDate.Value.Date;
            returnDate.MaxDate = DateTime.Now;
            returnTime.Value = DateTime.Now;
            rentDaysNum2.Value = (rentDaysNum1.Value == 0 ? 1 : rentDaysNum1.Value);
            if (rentDaysNum1.Value == 0)
                sumPaymentTxt.Text = PaymentNum.Value.ToString();
            kmNum.Value = int.Parse(row.Cells[17].Value.ToString());
            payed = double.Parse(row.Cells[22].Value.ToString());
            PayStatusCB.SelectedIndex = (row.Cells[10].Value.ToString() == "שולם" ? 0 : 1);
            carReturnStatusTxt.Text = carStatusTxt.Text = dbManger.CarStatus((int)row.Cells[0].Value, "Car_Status_Rent");
            //rentDaysNum1_ValueChanged(null, null);
            DataSet drivers = dbManger.DriversInRent((int)row.Cells[0].Value);
            if ((drivers != null) && (drivers.Tables.Count > 0))
            {
                for (int i = 0; i < drivers.Tables["drivers"].Rows.Count; i++)
                {
                    idNumTxt.Text = drivers.Tables["drivers"].Rows[i].ItemArray.GetValue(0).ToString();
                    driversDGV.Rows.Add(firstNameTxt.Text, lastNameTxt.Text, idNumTxt.Text, phoneNumTxt.Text, BirthdateTimePicker.Value.Date.ToString("dd/MM/yyyy"), addressTxt.Text, telephoneNumTxt.Text, licenseIssueDate.Value.Date.ToString("dd/MM/yyyy"), licenseExpiryDate.Value.Date.ToString("dd/MM/yyyy"));
                    ClearAddDriver();
                }
                idNumTxt.Text = drivers.Tables["drivers"].Rows[0].ItemArray.GetValue(0).ToString();
            }
            else
            {
                MessageBox.Show("Failed To Load drivers Check your Connection and try again", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            DataSet taxInvoice = dbManger.TaxInvoice(rentIdTxt.Text);
            if ((taxInvoice != null) && (taxInvoice.Tables.Count > 0) && (taxInvoice.Tables["tax_invoice"].Rows.Count > 0))
                taxInvoiceDGV.DataSource = taxInvoice.Tables["tax_invoice"];
            // taxInvoiceDGV.Columns[InvoiceDateCol.Name].DefaultCellStyle.Format = "dd/MM/yyyy";
        }
        private void EnableRentGB()
        {
            addDriverBtn.Enabled = false;
            removeDriverBtn.Enabled = false;
            rentTypeTxt.Enabled = false;
            rentDateTime.Enabled = false;
            rentTime.Enabled = false;
            rentDaysNum1.Enabled = false;
            kmNum.Enabled = false;
            idNumTxt.Enabled = false;
            firstNameTxt.Enabled = false;
            lastNameTxt.Enabled = false;
            addressTxt.Enabled = false;
            BirthdateTimePicker.Enabled = false;
            phoneNumTxt.Enabled = false;
            telephoneNumTxt.Enabled = false;
            licenseIssueDate.Enabled = false;
            licenseExpiryDate.Enabled = false;
        }
        /*פונקציה  לבדוק אם הנקלט בעברית או באנגלית*/
        private bool IsHeOrEn(string s)
        {
            string txt = s.Replace(" ", "");
            if (txt != string.Empty && (Regex.IsMatch(txt, @"^[a-zA-Z]+$") | Regex.IsMatch(txt, @"^[א-ת]+$")))
                return true;
            return false;
        }
        /*int פונקציה  לבדוק אם הנקלט מספר מסוג */
        private bool IsDig(string s)
        {
            int num;
            int.TryParse(s, out num);
            if (s != string.Empty && num > 0)
                return true;
            return false;
        }
        /*פונקציה לבדיקת תקינות תוקף רשיון ואם הנהג הוא נהג חדש*/
        private bool CheckDriverLicense(DateTime dateissue, DateTime dateExpiry, DateTime rent, DateTime bDate)//check the driver license and if the driver not a new driver
        {
            int driverLicenseYears = (int)(DateTime.Now - dateissue).TotalDays / 365;
            int age = (int)(DateTime.Now - bDate).TotalDays / 365;
            if ((dateExpiry - dateissue).TotalDays > 0 && (dateExpiry - rent).TotalDays > 0 && driverLicenseYears >= 2 && (age - driverLicenseYears) >= 17)
                return true;
            return false;
        }
        /*פונקציה לבדוק אם הנהג גדול מ 21*/
        private bool IsAge21(DateTimePicker Bdate) 
        {
            TimeSpan tmp = DateTime.Now.Date - Bdate.Value.Date;
            int currentAge = (int)tmp.TotalDays / 365;
            if (currentAge >= 21)
                return true;
            return false;
        }
        /*פונקציה לנקות את כל השדות בממשק אחרי שהספנו לקוח*/
        private void ClearAddDriver()
        {
            idNumTxt.Clear();
            firstNameTxt.Clear();
            lastNameTxt.Clear();
            addressTxt.Clear();
            BirthdateTimePicker.Value = DateTime.Now;
            phoneNumTxt.Clear();
            telephoneNumTxt.Clear();
            licenseIssueDate.Value = DateTime.Now;
            licenseExpiryDate.Value = DateTime.Now;
        }
        /*פןנקציה לבדוק אם קל תז של הנהגים שהוספנו שונות*/
        private bool CheckID_Driver(string id) 
        {
            string idTemp;
            if (id.IsDig())
            {
                foreach (DataGridViewRow row in driversDGV.Rows)
                {
                    idTemp = row.Cells[2].Value.ToString();
                    if (idTemp == id)
                        return false;
                }
                return true;
            }
            return false;
        }
        /*פונקציה לבדיקת תקינות קלט לנהג ש רוצים להוסיף */
        private void addDriverBtn_Click(object sender, EventArgs e)
        {
            if ( CheckID_Driver(idNumTxt.Text) && IsHeOrEn(firstNameTxt.Text) && IsHeOrEn(lastNameTxt.Text) && addressTxt.Text != string.Empty && licenseIssueDate.Text != string.Empty && licenseExpiryDate.Text != string.Empty && IsAge21(BirthdateTimePicker) && IsDig(phoneNumTxt.Text) && (telephoneNumTxt.Text.IsDig() || telephoneNumTxt.Text == string.Empty) && CheckDriverLicense(licenseIssueDate.Value.Date, licenseExpiryDate.Value.Date, rentDateTime.Value.Date, BirthdateTimePicker.Value.Date))
            {
                driversDGV.Rows.Add(firstNameTxt.Text, lastNameTxt.Text, idNumTxt.Text, phoneNumTxt.Text, BirthdateTimePicker.Value.Date.ToString("dd/MM/yyyy"), addressTxt.Text, telephoneNumTxt.Text, licenseIssueDate.Value.Date.ToString("dd/MM/yyyy"), licenseExpiryDate.Value.Date.ToString("dd/MM/yyyy"));
                ClearAddDriver();
            }
            else
                MessageBox.Show("Failed To Add driver Check your information", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /*פונקציה למחוק נהג */
        private void removeDriverBtn_Click(object sender, EventArgs e)
        {
            if (driversDGV.SelectedRows.Count == 1)
                driversDGV.Rows.RemoveAt(driversDGV.SelectedRows[0].Index);
        }
        /*פונקציה למתי שפותחים את הממשק הזה מגדירים כמה דברים*/
        private void AddRent_Load(object sender, EventArgs e)
        {
            if (rentMode == RentType.Add)
            {
                int idRent = dbManger.ReadRent_ID();
                rentIdTxt.Text = idRent.ToString();
                rentDateTime.MaxDate = rentDateTime.Value = DateTime.Now.Date;
                rentTime.Value = DateTime.Now;
                returnTime.Value = rentTime.Value;
                //returnDate.MinDate =
                returnDate.Value = rentDateTime.Value;
            }
            else if (rentMode == RentType.Return)
            {
                Date = true;
            }
            CarNumAutoComplete();
            DriverIDAutoComplete();
            DriverNameAutoComplete();
            DriverLastNameAutoComplete();
            balanceDueTxt.Text = (double.Parse(sumPaymentTxt.Text) - payed).ToString();
            this.WindowState = FormWindowState.Maximized;
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
        }
        /*פונקציה למלות מספר הרכב */
        private void CarNumAutoComplete()
        {
            collection = new AutoCompleteStringCollection();
            if (allCar.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < allCar.Tables[0].Rows.Count; i++)
                {
                    collection.Add(allCar.Tables[0].Rows[i].ItemArray[0].ToString());
                }
                carNumTxt.AutoCompleteMode = AutoCompleteMode.Suggest;
                carNumTxt.AutoCompleteSource = AutoCompleteSource.CustomSource;
                carNumTxt.AutoCompleteCustomSource = collection;
            }
        }
        /*פונקציה למלות ת"ז של הנהג */
        private void DriverIDAutoComplete()
        {
            collection = new AutoCompleteStringCollection();
            if (allDrivers.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < allDrivers.Tables[0].Rows.Count; i++)
                {
                    collection.Add(allDrivers.Tables[0].Rows[i].ItemArray[0].ToString());
                }
                idNumTxt.AutoCompleteMode = AutoCompleteMode.Suggest;
                idNumTxt.AutoCompleteSource = AutoCompleteSource.CustomSource;
                idNumTxt.AutoCompleteCustomSource = collection;
            }
        }
        /*פונקציה למלות שם פרטי של הנהג */
        private void DriverNameAutoComplete()
        {
            collection = new AutoCompleteStringCollection();
            if (allDrivers.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < allDrivers.Tables[0].Rows.Count; i++)
                {
                    collection.Add(allDrivers.Tables[0].Rows[i].ItemArray[1].ToString());
                }
                firstNameTxt.AutoCompleteMode = AutoCompleteMode.Suggest;
                firstNameTxt.AutoCompleteSource = AutoCompleteSource.CustomSource;
                firstNameTxt.AutoCompleteCustomSource = collection;
            }
        }
        /*פונקציה למלות שם משפחה של הנהג */
        private void DriverLastNameAutoComplete()
        {
            collection = new AutoCompleteStringCollection();
            if (allDrivers.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < allDrivers.Tables[0].Rows.Count; i++)
                {
                    collection.Add(allDrivers.Tables[0].Rows[i].ItemArray[2].ToString());
                }
                lastNameTxt.AutoCompleteMode = AutoCompleteMode.Suggest;
                lastNameTxt.AutoCompleteSource = AutoCompleteSource.CustomSource;
                lastNameTxt.AutoCompleteCustomSource = collection;
            }
        }
        /*פונקציה לבדוק אם הערך השתנה של תאריך החזרה אז משנים כמה ימי השכירות*/
        private void returnDate_ValueChanged(object sender, EventArgs e)
        {
            if (dontRunHandler)
            {
                double day = (returnDate.Value.Date - rentDateTime.Value.Date).TotalDays;
                if (day >= 0 && day <= 365)
                    rentDaysNum1.Value = (int)day;
                else if (day < 0)
                    returnDate.Value = rentDateTime.Value;
                else
                {
                    rentDaysNum1.DownButton();
                    rentDaysNum1.UpButton();
                    rentDaysNum1.Value = 365;
                }
                if ((int)day == 0)
                    returnTime.Value = rentTime.Value;
                if (rentMode == RentType.Return)
                {
                    oldPayment = double.Parse(sumPaymentTxt.Text.ToString());
                    if (Date)
                        returnDistanceTxt_Leave(null, null);
                }
            }
            dontRunHandler = true;
            //return;
        }
        private void RentDateTime_ValueChanged(object sender, EventArgs e)
        {
            returnDate_ValueChanged(null, null);
        }
        /*פונקציה לבדוק אם הערך השתנה של כמה ימי השכירות  אז משנים תאריך החזרה */
        private void rentDaysNum1_ValueChanged(object sender, EventArgs e)
        {

            if (rentDaysNum1.Value == 0)
            {
                returnDate.Value = rentDateTime.Value;
                sumPaymentTxt.Text = PaymentNum.Value.ToString();//eachPaymentNumTxt.Text =
                dontRunHandler = true;
            }
            else
            {
                dontRunHandler = false;
                returnDate.Value = rentDateTime.Value.AddDays((double)rentDaysNum1.Value);
                sumPaymentTxt.Text = (PaymentNum.Value * rentDaysNum1.Value).ToString();//eachPaymentNumTxt.Text = 
            }
        }
        private void returnTime_ValueChanged(object sender, EventArgs e)
        {
            if (rentDateTime.Value.Date == returnDate.Value.Date && rentTime.Value > returnTime.Value)
                returnTime.Value = rentTime.Value;
        }
        /*double פונקציה  לבדוק אם הנקלט מספר מסוג */
        private bool IsDistance(TextBox s)//convert to double and return true
        {
            double num;
            double oldNum;
            if (double.TryParse(s.Text, out num) && double.TryParse(oldDistance, out oldNum))
                if (num >= oldNum)
                    return true;
            return false;
        }
        /*פונקציה למלאות מערך בשמות של הנהגים*/
        private void Scan_All_Drivers()
        {
            drivers = new Driver_Licensing[driversDGV.RowCount];
            for (int i = 0; i < driversDGV.RowCount; i++)
            {
                DateTime birthDate = DateTime.ParseExact(driversDGV.Rows[i].Cells[4].Value.ToString(), "dd/MM/yyyy", null);//birth
                DateTime issuedDate = DateTime.ParseExact(driversDGV.Rows[i].Cells[7].Value.ToString(), "dd/MM/yyyy", null);//issue
                DateTime expireDate = DateTime.ParseExact(driversDGV.Rows[i].Cells[8].Value.ToString(), "dd/MM/yyyy", null);//expire
                drivers[i] = new Driver_Licensing(driversDGV.Rows[i].Cells[2].Value.ToString(),
                                                   driversDGV.Rows[i].Cells[0].Value.ToString(),
                                                   driversDGV.Rows[i].Cells[1].Value.ToString(),
                                                   driversDGV.Rows[i].Cells[5].Value.ToString(),
                                                   birthDate, driversDGV.Rows[i].Cells[3].Value.ToString(),
                                                   driversDGV.Rows[i].Cells[6].Value.ToString(),
                                                   expireDate,
                                                   issuedDate);
            }
        }
        /*פונקציה לבדוק את תקינות קילט של שכירות*/
        private void saveBtn_Click(object sender, EventArgs e)
        {
//          Bitmap bmpSig = null;
            frmWacom wacomForm = null;
            try
            {
                //if (double.Parse(balanceDueTxt.Text) == 0)//צריך לשלם לפני הןספה או עדכון שכירות
                //{
                wacomForm = new frmWacom();//mangerFrm.wacom.GetLiveWindow();
                DialogResult res = wacomForm.ShowDialog();
                ///Save to report
                if (res == DialogResult.OK)
                {
                    /* SaveFileDialog sfd = new SaveFileDialog();
                     sfd.Filter = "PNG files (*.png)|*.png|All files (*.*)|*.*";
                     sfd.DefaultExt = "png";
                     res = sfd.ShowDialog();
                     if (res == DialogResult.OK)
                     {
                         bmpSig = wacomForm.signature;
                         bmpSig.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Png);
                         mangerFrm.wacom.LastSignature = bmpSig;
                         sfd.Dispose();
                     }*/
                    mangerFrm.LastSignature = wacomForm.signature;

                    if (rentMode == RentType.Add)
                        AddRent_Click();
                    else if (rentMode == RentType.Return)
                        ReturnBtn_Click();
                    else
                        UpdateBtn_Click();
                    /* }
                     else
                         MessageBox.Show("Info:Can't Add rent:\nmust pay before", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);*/
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (wacomForm != null)
                {
                    wacomForm.Dispose();
                    wacomForm = null;
                }
            }
        }
        private void AddRent_Click()
        {
            if (rentIdTxt.Text != string.Empty && rentIdTxt.Text != "-1" && driversDGV.RowCount >= 1 && checkCarNum != -1 && IsDistance(distance1Txt) && carStatusTxt.Text != string.Empty)
            {//remember to check if payed...
                Scan_All_Drivers();
                DateTime date1 = new DateTime(rentDateTime.Value.Year, rentDateTime.Value.Month, rentDateTime.Value.Date.Day, rentTime.Value.Hour, rentTime.Value.Minute, rentTime.Value.Second);
                DateTime date2 = new DateTime(returnDate.Value.Year, returnDate.Value.Month, returnDate.Value.Date.Day, returnTime.Value.Hour, returnTime.Value.Minute, returnTime.Value.Second);
                details = new Rent_Details(int.Parse(rentIdTxt.Text), date1, (int)rentDaysNum1.Value, date2, float.Parse(sumPaymentTxt.Text), PayStatusCB.SelectedItem.ToString(), (float)PaymentNum.Value, (int)kmNum.Value, double.Parse(distance1Txt.Text), fuelCapacity1Txt.Text, carStatusTxt.Text, employeeID, drivers[0].Id, carNumTxt.Text);
                if (dbManger.Add_Rent(details, drivers) && dbManger.RentUpdateCar(carNumTxt.Text, double.Parse(distance1Txt.Text), fuelCapacity1Txt.Text, carStatusTxt.Text) && dbManger.AddRentComment(details, employeeID))
                {
                    if (mangerFrm.LastSignature != null)
                    {
                        MemoryStream sigImage = new MemoryStream();
                        mangerFrm.LastSignature.Save(sigImage, System.Drawing.Imaging.ImageFormat.Png);
                        dbManger.Add_signature(sigImage.GetBuffer(), details.Rental_ID, true);
                    }
                        saveBtn_WasClicked = true;
                        // if (float.Parse(balanceDueTxt.Text) > 0)/*if we didn't add payment*/
                        TaxInvoiceBtn_Click(null, null);
                        /*after adding the rent we change the form mode to update to not close it*/
                        rentMode = RentType.Update;
                        Update_Show(1);
                        returnDate.MinDate = returnDate.Value.Date;
                        rentDaysNum1.Minimum = (int)(returnDate.Value.Date - rentDateTime.Value.Date).TotalDays;
                    }
                    else
                        MessageBox.Show("Info:Can't Add rent right now", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("ERROR Creating New Rent OR you didn't add driver", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        /*פונקציה לבדוק את תקינות קילט של ההחזרה*/
        private void ReturnBtn_Click()
        {
            double KM_Need_To_Be = double.Parse(distance1Txt.Text.ToString());//max km should be
            double KM_After_return = 0;
            double.TryParse(returnDistanceTxt.Text.ToString(), out KM_After_return);
            DateTime dateReturn = new DateTime(returnDate.Value.Year, returnDate.Value.Month, returnDate.Value.Date.Day, returnTime.Value.Hour, returnTime.Value.Minute, returnTime.Value.Second);
            if (returnDistanceTxt.Text != string.Empty && carReturnStatusTxt.Text != string.Empty && KM_After_return > KM_Need_To_Be)
            {
                details = new Rent_Details(int.Parse(rentIdTxt.Text), (int)rentDaysNum1.Value, dateReturn, float.Parse(sumPaymentTxt.Text), PayStatusCB.SelectedItem.ToString(), double.Parse(returnDistanceTxt.Text), returnedFuelCapacityTxt.Text, carReturnStatusTxt.Text, employeeID, carNumTxt.Text);
                if (dbManger.Return_Rent(details))
                {
                    dbManger.DeleteRentComment(rentIdTxt.Text.ToString());
                    if (mangerFrm.LastSignature != null)
                    {
                        MemoryStream sigImage = new MemoryStream();
                        mangerFrm.LastSignature.Save(sigImage, System.Drawing.Imaging.ImageFormat.Png);
                        //Image.GetInstance(sigImage.GetBuffer());
                        if (!dbManger.Add_signature(sigImage.GetBuffer(), details.Rental_ID, false))
                            dbManger.Update_signature(sigImage.GetBuffer(), details.Rental_ID, false);
                    }
                    saveBtn_WasClicked = true;
                    if (float.Parse(balanceDueTxt.Text) > 0)/*if we didn't add payment*/
                        TaxInvoiceBtn_Click(null, null);
                    rentMode = RentType.UpdateReturn;
                    Update_Show(0);//updateReturn
                                   //this.Close();

                }
                else
                    MessageBox.Show("Info:Can't Return rent right now", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Check your information", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /*פונקציה לבדוק אם השכירות ש רוצים לעדכן הרכב שלה הוחזרה או עדין*/
        private void UpdateBtn_Click()
        {
            if (choice_update == 1)
                /*  הרכב עדין לא נחזרה  */
                Update_Not_ReturnedRent();
            else
            {
                /*הרכב נחזירה*/
                if (carReturnStatusTxt.Text != string.Empty && dbManger.UpdateRent(carReturnStatusTxt.Text, int.Parse(rentIdTxt.Text), PayStatusCB.Text))
                {
                    if (mangerFrm.LastSignature != null)
                    {
                        MemoryStream sigImage = new MemoryStream();
                        mangerFrm.LastSignature.Save(sigImage, System.Drawing.Imaging.ImageFormat.Png);
                        if (!dbManger.Add_signature(sigImage.GetBuffer(), int.Parse(rentIdTxt.Text), false))
                            dbManger.Update_signature(sigImage.GetBuffer(), int.Parse(rentIdTxt.Text), false);
                    }
                    saveBtn_WasClicked = true;
                }
                //this.Close();
                else
                    MessageBox.Show("Check your information", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        /*פונקציה לבדיקת תקינות של השכירות שרוצים לעדכן לרכב עדין לא הוחזר*/
        private void Update_Not_ReturnedRent()
        {
            Scan_All_Drivers();
            DateTime date1 = new DateTime(rentDateTime.Value.Year, rentDateTime.Value.Month, rentDateTime.Value.Date.Day, rentTime.Value.Hour, rentTime.Value.Minute, rentTime.Value.Second);
            DateTime date2 = new DateTime(returnDate.Value.Year, returnDate.Value.Month, returnDate.Value.Date.Day, returnTime.Value.Hour, returnTime.Value.Minute, returnTime.Value.Second);
            details = new Rent_Details(int.Parse(rentIdTxt.Text), date1, (int)rentDaysNum1.Value, date2, float.Parse(sumPaymentTxt.Text), PayStatusCB.SelectedItem.ToString(), (float)PaymentNum.Value, (int)kmNum.Value, double.Parse(distance1Txt.Text), fuelCapacity1Txt.Text, carStatusTxt.Text, employeeID, drivers[0].Id, carNumTxt.Text);
            if (driversDGV.RowCount >= 1 && carStatusTxt.Text != string.Empty && IsDistance(distance1Txt) && dbManger.RemoveDrivers(int.Parse(rentIdTxt.Text)))
                if (dbManger.Update_Rent_Details(details, drivers) && dbManger.RentUpdateCar(carNumTxt.Text, double.Parse(distance1Txt.Text), fuelCapacity1Txt.Text, carStatusTxt.Text))
                {
                    if (mangerFrm.LastSignature != null)
                    {
                        MemoryStream sigImage = new MemoryStream();
                        mangerFrm.LastSignature.Save(sigImage, System.Drawing.Imaging.ImageFormat.Png);
                        if (!dbManger.Add_signature(sigImage.GetBuffer(), details.Rental_ID, true))
                            dbManger.Update_signature(sigImage.GetBuffer(), details.Rental_ID, true);
                    }
                    dbManger.UpdateRentComment(date2, employeeID, rentIdTxt.Text.ToString());
                    saveBtn_WasClicked = true;
                    //this.Close();
                }
                else
                    MessageBox.Show("Check your information", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }
        /*פונקציה לעשות מבצע לשכירות לפי תקופות*/
        private void rentTypeTxt_SelectedIndexChanged(object sender, EventArgs e)
        {
            int discount = 20;
            if (rentTypeTxt.SelectedIndex == 0)
                PaymentNum.Value = v;
            else if (rentTypeTxt.SelectedIndex == 1)
                PaymentNum.Value = v - discount;
            else if (rentTypeTxt.SelectedIndex == 2)
                PaymentNum.Value = v - discount * 2;
            else if (rentTypeTxt.SelectedIndex == 3)
                PaymentNum.Value = v - discount * 3;
        }
        /*פונקציה שעובדת מתי משנים מחיר לרכב שתשנה את המחיר הסופי*/
        private void PaymentNum_ValueChanged(object sender, EventArgs e)
        {
            if (rentDaysNum1.Value == 0)
            {
                sumPaymentTxt.Text = PaymentNum.Value.ToString();//eachPaymentNumTxt.Text =
            }
            else
                sumPaymentTxt.Text = (PaymentNum.Value * rentDaysNum1.Value).ToString();
        }

        /*private void paymentsNum_ValueChanged(object sender, EventArgs e)
        {
            double tmp = double.Parse(balanceDueTxt.Text);
            tmp = (tmp / (double)paymentsNum.Value);
            tmp = Math.Round(tmp, 3);
            eachPaymentNumTxt.Text = tmp.ToString();
        }

        private void balanceDueTxt_TextChanged(object sender, EventArgs e)
        {
            eachPaymentNumTxt.Text = balanceDueTxt.Text;
        }*/
        /*פונקציה למלות את נתוני הנהג*/
        private void complete_DriverInfoID()
        {
            checkDriverId = -1;
            for (int i = 0; i < allDrivers.Tables[0].Rows.Count; i++)
                if (allDrivers.Tables[0].Rows[i].ItemArray[0].ToString() == idNumTxt.Text)
                    checkDriverId = i;
            if (checkDriverId != -1)
            {
                firstNameTxt.Text = allDrivers.Tables[0].Rows[checkDriverId].ItemArray[1].ToString();
                lastNameTxt.Text = allDrivers.Tables[0].Rows[checkDriverId].ItemArray[2].ToString();
                phoneNumTxt.Text = allDrivers.Tables[0].Rows[checkDriverId].ItemArray[3].ToString();
                addressTxt.Text = allDrivers.Tables[0].Rows[checkDriverId].ItemArray[4].ToString();
                telephoneNumTxt.Text = allDrivers.Tables[0].Rows[checkDriverId].ItemArray[5].ToString();
                BirthdateTimePicker.Value = Convert.ToDateTime(allDrivers.Tables[0].Rows[checkDriverId].ItemArray[6].ToString());
                licenseExpiryDate.Value = Convert.ToDateTime(allDrivers.Tables[0].Rows[checkDriverId].ItemArray[7].ToString());
                licenseIssueDate.Value = Convert.ToDateTime(allDrivers.Tables[0].Rows[checkDriverId].ItemArray[8].ToString());
            }
        }
        /*פונקציה למלות את נתוני הרכב*/
        private void complete_CarInfo()
        {
            checkCarNum = -1;
            if (returnCar)
                allCar = dbManger.AllCar();
            for (int i = 0; i < allCar.Tables[0].Rows.Count; i++)
                if (allCar.Tables[0].Rows[i].ItemArray[0].ToString() == carNumTxt.Text)
                    checkCarNum = i;
            if (checkCarNum != -1)
            {
                carManufacturerTxt.Text = allCar.Tables[0].Rows[checkCarNum].ItemArray[1].ToString();
                carModelTxt.Text = allCar.Tables[0].Rows[checkCarNum].ItemArray[2].ToString();
                engineCapacityTxt.Text = allCar.Tables[0].Rows[checkCarNum].ItemArray[3].ToString();
                DateTime tmp = new DateTime((int)allCar.Tables[0].Rows[checkCarNum].ItemArray[15], 01, 01);
                productionDateTimePicker.Value = tmp.Date;
                gearboxTypeTxt.Text = allCar.Tables[0].Rows[checkCarNum].ItemArray[8].ToString();
                carColorTxt.Text = allCar.Tables[0].Rows[checkCarNum].ItemArray[9].ToString();
                decimal Payment = decimal.Parse(allCar.Tables[0].Rows[checkCarNum].ItemArray[13].ToString());
                PaymentNum.Value = v = Payment;
                fuelCapacity1Txt.Text = allCar.Tables[0].Rows[checkCarNum].ItemArray[12].ToString();
                distance1Txt.Text = oldDistance = allCar.Tables[0].Rows[checkCarNum].ItemArray[10].ToString();
                carStatusTxt.Text = allCar.Tables[0].Rows[checkCarNum].ItemArray[19].ToString();
            }
            else if (checkCarNum == -1)
                clear_CarInfo();
        }
        /*פונקציה לנקות את השדות של הרכב*/
        private void clear_CarInfo() 
        {
            carManufacturerTxt.Clear();
            carModelTxt.Clear();
            engineCapacityTxt.Text = "";
            //productionDateTimePicker.Value = DateTime.Now.Date;
            gearboxTypeTxt.SelectedIndex = -1;
            carColorTxt.Text = "";
            PaymentNum.Value = v = 150;
            fuelCapacity1Txt.SelectedIndex = -1;
            distance1Txt.Clear();
            carStatusTxt.Text = "הכל תקין";
        }
        /*פונקציה שעובדת אם השתנה הטקסט שבשדה של מספר רכב*/
        private void carNumTxt_TextChanged(object sender, EventArgs e) 
        {
            complete_CarInfo();
        }
        /*פונקציה שעובדת אם השתנה הטקסט שבשדה של ת"ז של הנהג*/
        private void idNumTxt_TextChanged(object sender, EventArgs e)
        {
            complete_DriverInfoID();
        }
        /*פונקציה שעובדת אם השתנה הטקסט שבשדה של כמות דלק בהחזרה*/
        private void returnedFuelCapacityTxt_SelectedIndexChanged(object sender, EventArgs e)
        {
            returnDistanceTxt_Leave(null, null);
        }
        /*אחרי ההחזרה KM פונקציה שבודקת את ההפרש בדלק ב  */
        private void returnDistanceTxt_Leave(object sender, EventArgs e)
        {
            int KM_To_Drive = (int)kmNum.Value * ((int)rentDaysNum1.Value == 0 ? 1 : (int)rentDaysNum1.Value);//km for the rent
            double KM_Need_To_Be = double.Parse(distance1Txt.Text.ToString()) + KM_To_Drive;//max km need to be
            double KM_After_return = 0;
            if (!double.TryParse(returnDistanceTxt.Text.ToString(), out KM_After_return))
                returnDistanceTxt.Text = "";
            double KM_Difference = KM_After_return - KM_Need_To_Be;
            int indexDifference = fuelCapacity1Txt.SelectedIndex - returnedFuelCapacityTxt.SelectedIndex;
            if (indexDifference < 0)
                indexDifference = 0;
            if (KM_Difference < 0)
                KM_Difference = 0;
            double NewPayment = oldPayment + KM_Difference + indexDifference * 25;
            sumPaymentTxt.Text = NewPayment.ToString();
            differenceDistanceTxt.Text = KM_Difference.ToString();
            differenceFuelCapacityTxt.SelectedIndex = indexDifference;
        }
        /* שיההיה בהחזרה בזמן שהעכבר על השדהKM פונקציה שמראה כמה צריך   */
        private void returnDistanceTxt_MouseHover(object sender, EventArgs e) 
        {
            string str = "ק" + '"' + "ם ההחזרה צריך להיות יותר מ:" + distance1Txt.Text + "\r\nהגבלה:" + (double.Parse(distance1Txt.Text) + ((int)rentDaysNum1.Value==0?1: (int)rentDaysNum1.Value) * (int)kmNum.Value).ToString();
            toolTip1.ToolTipTitle = "                 " + "הגדרת ק" + '"' + "ם החזרה";
            toolTip1.ToolTipIcon = ToolTipIcon.Info;
            toolTip1.IsBalloon = true;
            //toolTip1.OwnerDraw = true;
            //toolTip1.Draw += toolTip1_Draw;
            //toolTip1.ToolTipTitle.PadLeft(150);
            toolTip1.SetToolTip(returnDistanceTxt, str);
            toolTip1.Show(str, returnDistanceTxt, 10, returnDistanceTxt.Height - 10);
        }
        /* שיההיה בהחזרה בזמן שהעכבר חוץ השדה KM פונקציה מסתירה כמה צריך   */
        private void returnDistanceTxt_MouseLeave(object sender, EventArgs e)
        {
            toolTip1.Hide(returnDistanceTxt);
        }


        /************************Update Rent*****************************/

        /*פונקציה למלות את הפרטים בבמשק השכירות לעדכן שכירות*/
        public void Update_Rent(DataGridViewRow row)
        {
            rentDateTime.Enabled = false;
            carNumTxt.Text = row.Cells[6].Value.ToString();
            DataSet drivers = dbManger.DriversInRent((int)row.Cells[0].Value);
            if ((drivers != null) && (drivers.Tables.Count > 0))
                for (int i = 0; i < drivers.Tables["drivers"].Rows.Count; i++)
                {
                    idNumTxt.Text = drivers.Tables["drivers"].Rows[i].ItemArray.GetValue(0).ToString();
                    driversDGV.Rows.Add(firstNameTxt.Text, lastNameTxt.Text, idNumTxt.Text, phoneNumTxt.Text, BirthdateTimePicker.Value.Date.ToString("dd/MM/yyyy"),
                                        addressTxt.Text, telephoneNumTxt.Text, licenseIssueDate.Value.Date.ToString("dd/MM/yyyy"), licenseExpiryDate.Value.Date.ToString("dd/MM/yyyy"));
                    ClearAddDriver();
                }
            else
            {
                MessageBox.Show("Failed To Load drivers Check your Connection and try again", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            Update_Rent_Fill(row);
            if (choice_update == 0)
            {
                returnDistanceTxt.Text = row.Cells[19].Value.ToString();
                returnedFuelCapacityTxt.Text = row.Cells[21].Value.ToString();
                rentDaysNum2 = rentDaysNum1;
            }
            if (choice_update == 1)//update car that's in rent
            {
                returnDate.MinDate = returnDate.Value.Date;
                rentDaysNum1.Minimum = (int)(returnDate.Value.Date - rentDateTime.Value.Date).TotalDays;
                if (returnDate.Value.Date <= DateTime.Now.Date)
                    returnDate.Value = DateTime.Now.AddDays(1);
                /*returnDate.MinDate = returnDate.Value.Date;
                rentDaysNum1.Minimum = (int)(returnDate.Value.Date - rentDateTime.Value.Date).TotalDays;*/
                /*returnDate.MinDate = DateTime.Now.AddDays(1).Date;
                rentDaysNum1.Minimum = (int)(DateTime.Now.AddDays(1).Date - rentDateTime.Value.Date).TotalDays;*/
            }
        }
        /*פונקציה לעדכן שכירות*/
        private void Update_Rent_Fill(DataGridViewRow row)
        {
            distance1Txt.Text = row.Cells[18].Value.ToString();
            fuelCapacity1Txt.Text = row.Cells[20].Value.ToString();
            rentIdTxt.Text = row.Cells[0].Value.ToString();
            rentDateTime.Text = row.Cells[15].Value.ToString();
            rentTime.Text = row.Cells[15].Value.ToString();
            PaymentNum.Value = decimal.Parse(row.Cells[11].Value.ToString());
            idNumTxt.Text = row.Cells[1].Value.ToString();
            //returnDate.MinDate = rentDateTime.Value;
            returnDate.Text = row.Cells[16].Value.ToString();
            returnTime.Text = row.Cells[16].Value.ToString();
            if (rentDaysNum1.Value == 0)
                sumPaymentTxt.Text = PaymentNum.Value.ToString();
            kmNum.Value = int.Parse(row.Cells[17].Value.ToString());
            oldPayment = double.Parse(sumPaymentTxt.Text.ToString());
            payed = double.Parse(row.Cells[22].Value.ToString());
            balanceDueTxt.Text = (oldPayment - double.Parse(row.Cells[22].Value.ToString())).ToString();
            carStatusTxt.Text = dbManger.CarStatus((int)row.Cells[0].Value, "Car_Status_Rent");
            carReturnStatusTxt.Text = dbManger.CarStatus((int)row.Cells[0].Value, "Car_Status_Returned");
            PayStatusCB.SelectedIndex = row.Cells[10].Value.ToString() == "שולם" ? 0 : 1;
            DataSet taxInvoice = dbManger.TaxInvoice(rentIdTxt.Text);
            if ((taxInvoice != null) && (taxInvoice.Tables.Count > 0) && (taxInvoice.Tables["tax_invoice"].Rows.Count > 0))
                taxInvoiceDGV.DataSource = taxInvoice.Tables["tax_invoice"];
            //taxInvoiceDGV.Columns[InvoiceDateCol.Name].DefaultCellStyle.Format = "dd/MM/yyyy";

        }
        /*פונקציה שבודקת אם רוצים להשכיר אז השדות של השכירות עובדות וכו*/
        public void Update_Show(int choice)/*enable things for update if choice is equal to 1 it still in rent else we return the rent*/
        {
            saveDocumentBtn.Enabled = true;
            printBtn.Enabled = true;
            if (choice == 1)
            {
                returnGB.Enabled = false;
                carNumTxt.Enabled = false;
                PaymentNum.Enabled = false;

            }
            else
            {
                rentGB.Enabled = false;
                carGB.Enabled = false;
                returnedFuelCapacityTxt.Enabled = false;
                returnDistanceTxt.Enabled = false;
            }
            choice_update = choice;
            saveBtn.Text = "עדכון";
            //this.ShowDialog();
        }
        /*פונקציה שעובדת אם שינינו את שעת השכירות שתהיה שעת ההחזרה זהה*/
        private void rentTime_ValueChanged(object sender, EventArgs e)
        {
            returnTime.Value = rentTime.Value;
        }

        private void AddRent_FormClosed(object sender, FormClosedEventArgs e)
        {
            mangerFrm.onloadRent();
        }
        public void Call_SaveDocumentBtn_Click()
        {
            saveDocumentBtn_Click(null, null);
        }
        /*PDF פונקציה להמרת נתוני השכירות למסמך */
        private void saveDocumentBtn_Click(object sender, EventArgs e)
        {
            BaseFont tableFont1 = BaseFont.CreateFont(@"C:\Windows\Fonts\GISHA.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            myTable = new PdfPTable(5) { RunDirection = PdfWriter.RUN_DIRECTION_RTL };


        /*Font tableFont = new Font(tableFont1, 12)
        {
            Color = BaseColor.BLACK
        };*/
        saveRentPdf.Filter = "PDF Files|*.pdf";
            saveRentPdf.FileName= "שכירות מס'" + rentIdTxt.Text;
            try
            {
                if (saveRentPdf.ShowDialog() == DialogResult.OK)

                {
                    /*myTable.DefaultCell.BorderWidth = 0;
                    float[] widthOfTable = new float[5];
                    for (int i = 0; i < widthOfTable.Length; i++)
                    {
                        widthOfTable[i] = 100f / 5;*/
                        /*if (i == 0)
                            widthOfTable[i] = 30f;*/
                    /*}
                    myTable.SetWidths(widthOfTable);
                    myTable.WidthPercentage = 100f;
                    for (int i = 0; i < 5; i++)
                    {
                        myTable.AddCell(new Phrase(" ", tableFont));
                    }*/
                    Scan_All_Drivers();
                    DateTime date1 = new DateTime(rentDateTime.Value.Year, rentDateTime.Value.Month, rentDateTime.Value.Date.Day, rentTime.Value.Hour, rentTime.Value.Minute, rentTime.Value.Second);
                    DateTime date2 = new DateTime(returnDate.Value.Year, returnDate.Value.Month, returnDate.Value.Date.Day, returnTime.Value.Hour, returnTime.Value.Minute, returnTime.Value.Second);
                    details = new Rent_Details(int.Parse(rentIdTxt.Text), date1, int.Parse(rentDaysNum1.Value.ToString()), date2, float.Parse(sumPaymentTxt.Text), PayStatusCB.SelectedItem.ToString(), float.Parse(PaymentNum.Value.ToString()), int.Parse(kmNum.Value.ToString()), double.Parse(distance1Txt.Text), fuelCapacity1Txt.Text, carStatusTxt.Text, employeeID, drivers[0].Id, carNumTxt.Text);
                    doc = new Document();
                    string nameFile = saveRentPdf.FileName;// + " מס-" + rentIdTxt.Text;
                    PdfWriter.GetInstance(doc, new FileStream(nameFile, FileMode.Create));
                    doc.Open();
                    SetIntTable(details);
                    doc.Close();
                }
            }
            catch (IOException IO)
            {
                MessageBox.Show(IO.Message, "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        /*PDF פונקציה להמרת נתוני השכירות למסמך */
        public void SetIntTable(Rent_Details details)
        {
            BaseFont tableFont1 = BaseFont.CreateFont(@"C:\Windows\Fonts\GISHA.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font tableFont = new Font(tableFont1, 12);
            Font tableBoldFont = new Font(tableFont1, 14,1);
            TableCells(1, 0);
            myTable.AddCell(new PdfPCell(new Phrase("חוזה השכרה מס' " + rentIdTxt.Text, tableBoldFont)) { BorderWidth = 0 });
            doc.Add(myTable);
            AddDriversTitleDoc(0);
            ////////////////////////////////////////////////////////////////////////////////////////////
           /* myTable = new PdfPTable(5);
            myTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
             float[] widthOfTable = new float[5];
             for (int i = 0; i < widthOfTable.Length; i++)
             {
                 widthOfTable[i] = 100f / 5;
                 /*if (i == 0)
                     widthOfTable[i] = 30f;*/
            /* }
             myTable.SetWidths(widthOfTable);
            myTable.WidthPercentage = 100f;
            AddDriversTitleDoc(0);*/
            //AddPhrase(new Phrase("שם פרטי:" , tableFont));/*lazm a5od mn ljadwal sater 0*/
            /* AddPhrase(new Phrase("שם משפחה:" , tableFont));

             AddPhrase(new Phrase("ת" + '"' + "ז:" , tableFont));
             AddPhrase(new Phrase("נייד:" , tableFont));
             AddPhrase(new Phrase("טלפון:" , tableFont));*/
            /* myTable.AddCell(new PdfPCell(new Phrase("שם פרטי:", tableFont)) { BorderWidthBottom = 0 });/*lazm a5od mn ljadwal sater 0*/
            /* myTable.AddCell(new PdfPCell(new Phrase("שם משפחה:", tableFont)) { BorderWidthBottom = 0 });
             myTable.AddCell(new PdfPCell(new Phrase("ת" + '"' + "ז:", tableFont)) { BorderWidthBottom = 0 });
             myTable.AddCell(new PdfPCell(new Phrase("נייד:", tableFont)) { BorderWidthBottom = 0 });
             myTable.AddCell(new PdfPCell(new Phrase("טלפון:", tableFont)) { BorderWidthBottom = 0 });*/
            /*{
           AddPhraseBottom(new Phrase("שם פרטי:", tableFont));
           AddPhraseBottom(new Phrase("שם משפחה:", tableFont));
           AddPhraseBottom(new Phrase("ת" + '"' + "ז:", tableFont));
           AddPhraseBottom(new Phrase("נייד:", tableFont));
           AddPhraseBottom(new Phrase("טלפון:", tableFont));
          } */
            //AddPhrase(new Phrase( 
            /* myTable.AddCell(new PdfPCell(new Phrase(firstNameTxt.Text, tableFont)) {BorderWidthTop=0 ,HorizontalAlignment=Element.ALIGN_RIGHT });/*lazm a5od mn ljadwal sater 0*/
            /*  myTable.AddCell(new PdfPCell(new Phrase(lastNameTxt.Text, tableFont)) { BorderWidthTop = 0, HorizontalAlignment = Element.ALIGN_RIGHT } );
              myTable.AddCell(new PdfPCell(new Phrase(idNumTxt.Text, tableFont)) { BorderWidthTop = 0, HorizontalAlignment = Element.ALIGN_RIGHT } );
              myTable.AddCell(new PdfPCell(new Phrase(phoneNumTxt.Text, tableFont)) { BorderWidthTop = 0, HorizontalAlignment = Element.ALIGN_RIGHT } );*/
            /*{ AddPhraseLeft(new Phrase(firstNameTxt.Text, tableFont));
             AddPhraseLeft(new Phrase(lastNameTxt.Text, tableFont));
             AddPhraseLeft(new Phrase(idNumTxt.Text, tableFont));
             AddPhraseLeft(new Phrase(phoneNumTxt.Text, tableFont));
             AddPhraseLeft(new Phrase(telephoneNumTxt.Text, tableFont));
             }*/
            /* AddPhrase(new Phrase(firstNameTxt.Text, tableFont));

             AddPhrase(new Phrase( lastNameTxt.Text, tableFont));
             AddPhrase(new Phrase( idNumTxt.Text, tableFont));
             AddPhrase(new Phrase( phoneNumTxt.Text, tableFont));
             AddPhrase(new Phrase( telephoneNumTxt.Text, tableFont));*/

            /*{ AddPhraseBottom(new Phrase("כתובת:" , tableFont));
             AddPhraseBottom(new Phrase("תאריך לידה:"  , tableFont));
             AddPhraseBottom(new Phrase("תוקף רשיון:" , tableFont));
             myTable.AddCell(new PdfPCell(new Phrase("קבלת רשיון:", tableFont)) { BorderWidthBottom = 0 ,Colspan=2 });
             }*/
            //AddPhraseBottom(new Phrase("מס' רכב:", tableFont));

            /*{AddPhraseLeft(new Phrase(addressTxt.Text, tableFont));
            AddPhraseLeft(new Phrase(BirthdateTimePicker.Value.Date.ToString("dd/MM/yyyy"), tableFont));
            AddPhraseLeft(new Phrase(licenseExpiryDate.Value.Date.ToString("dd/MM/yyyy"), tableFont));
            myTable.AddCell(new PdfPCell(new Phrase(licenseIssueDate.Value.Date.ToString("dd/MM/yyyy"), tableFont)) { BorderWidthTop = 0, HorizontalAlignment = Element.ALIGN_RIGHT ,Colspan=2});
            }*/
            //AddPhraseLeft(new Phrase(carNumTxt.Text, tableFont));

            //doc.Add(myTable);

            /*for(int i = 0; i < 5; i++)
             {
                 myTable.AddCell(new Phrase(" ", tableFont));
             }*/
            AddBlankRow(5);
            /* myTable= new PdfPTable(1);
             myTable.DefaultCell.BorderWidth = 0;
             myTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
             myTable.SetWidths(widthOfTable);
             myTable.WidthPercentage = 100f;*/
            //myTable.AddCell(new Phrase(" ", tableFont));

            if (drivers.Length > 1)
            {
                TableCells(1, 0f);//(cells,noBorder)
                AddBlankPhrase(new Phrase("נהגים נוספים:", tableBoldFont));
                doc.Add(myTable);
                AddDriversDoc();
            }
            doc.Add(myTable);
            
            SetIntTable1(details);
        }
        private void TableCells(float CellsNum,float borderWidth)
        {
            myTable = new PdfPTable((int)CellsNum);
            myTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            float[] widthOfTable = new float[(int)CellsNum];
            for (int i = 0; i < widthOfTable.Length; i++)
            {
                widthOfTable[i] = 100f / CellsNum;
                /*if (i == 0)
                    widthOfTable[i] = 30f;*/
            }
            myTable.DefaultCell.BorderWidth = borderWidth;
            myTable.SetWidths(widthOfTable);
            myTable.WidthPercentage = 100f;
        }
        private void AddBlankRow(int CellsNum)
        {
            BaseFont tableFont1 = BaseFont.CreateFont(@"C:\Windows\Fonts\GISHA.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font tableFont = new Font(tableFont1, 12);
            TableCells(CellsNum,0);
            for(int i=0;i<CellsNum;i++)
            {
                AddBlankPhrase(new Phrase(" ", tableFont));
            }
            doc.Add(myTable);
        }
        private void AddDriversDoc()
        {

            TableCells(5,1);
            for (int i = 1; i < drivers.Length; i++)
            {
                AddDriversTitleDoc(i);//sending row number
                AddBlankRow(5);
            }
            doc.Add(myTable);

        }
        /*function to add specific driver details in cells */
        private void AddDriversTitleDoc(int driverNum)
        {
            BaseFont tableFont1 = BaseFont.CreateFont(@"C:\Windows\Fonts\GISHA.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font tableFont = new Font(tableFont1, 12);
            TableCells(5,1);
            AddPhraseBottom(new Phrase("שם פרטי:", tableFont));
            AddPhraseBottom(new Phrase("שם משפחה:", tableFont));
            AddPhraseBottom(new Phrase("ת" + '"' + "ז:", tableFont));
            AddPhraseBottom(new Phrase("נייד:", tableFont));
            AddPhraseBottom(new Phrase("טלפון:", tableFont));

            AddPhraseLeft(new Phrase(drivers[driverNum].First_name, tableFont));
            AddPhraseLeft(new Phrase(drivers[driverNum].Last_name, tableFont));
            AddPhraseLeft(new Phrase(drivers[driverNum].Id, tableFont));
            AddPhraseLeft(new Phrase(drivers[driverNum].Phone_number, tableFont));
            AddPhraseLeft(new Phrase(drivers[driverNum].Telephone_number, tableFont));

            AddPhraseBottom(new Phrase("כתובת:", tableFont));
            AddPhraseBottom(new Phrase("תאריך לידה:", tableFont));
            AddPhraseBottom(new Phrase("תוקף רשיון:", tableFont));
            myTable.AddCell(new PdfPCell(new Phrase("קבלת רשיון:", tableFont)) { BorderWidthBottom = 0, Colspan = 2 });
            AddPhraseLeft(new Phrase(drivers[driverNum].Address, tableFont));
            AddPhraseLeft(new Phrase(drivers[driverNum].Birth_date.ToString("dd/MM/yyyy"), tableFont));
            AddPhraseLeft(new Phrase(drivers[driverNum].Expire_date.ToString("dd/MM/yyyy"), tableFont));
            myTable.AddCell(new PdfPCell(new Phrase(drivers[driverNum].Date_of_issue.ToString("dd/MM/yyyy"), tableFont)) { BorderWidthTop = 0, HorizontalAlignment = Element.ALIGN_RIGHT, Colspan = 2 });
            doc.Add(myTable);
        }
        /*PDF פונקציה להמרת נתוני השכירות למסמך */
        public void SetIntTable1(Rent_Details details)
        {
            BaseFont tableFont1 = BaseFont.CreateFont(@"C:\Windows\Fonts\GISHA.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font tableFont = new Font(tableFont1, 11);
            Font tableBoldFont = new Font(tableFont1, 14, 1);

            TableCells(1, 0f);//(cells,BorderWidth)
            AddBlankPhrase(new Phrase("פרטי העסקה:", tableBoldFont));
            doc.Add(myTable);
            TableCells(7, 0.4f);//(cells,BorderWidth)
            AddPhraseBottom(new Phrase("רכב מס" + "':", tableFont));
            AddPhraseBottom(new Phrase("סוג רכב:", tableFont));
            AddPhraseBottom(new Phrase("דגם:", tableFont));
            AddPhraseBottom(new Phrase("תאריך יציאה:", tableFont));
            AddPhraseBottom(new Phrase("תאריך החזרה:", tableFont));
            AddPhraseBottom(new Phrase("שעת יציאה:", tableFont));
            AddPhraseBottom(new Phrase("שעת החזרה:", tableFont));

            /* carManufacturerTxt.Text = allCar.Tables[0].Rows[checkCarNum].ItemArray[1].ToString();
                carModelTxt.Text = allCar.Tables[0].Rows[checkCarNum].ItemArray[2].ToString();
                engineCapacityTxt.Text = allCar.Tables[0].Rows[checkCarNum].ItemArray[3].ToString();
                DateTime tmp = new DateTime((int)allCar.Tables[0].Rows[checkCarNum].ItemArray[15], 01, 01);
                productionDateTimePicker.Value = tmp.Date;
                gearboxTypeTxt.Text = allCar.Tables[0].Rows[checkCarNum].ItemArray[8].ToString();
                carColorTxt.Text = allCar.Tables[0].Rows[checkCarNum].ItemArray[9].ToString();
                decimal Payment = decimal.Parse(allCar.Tables[0].Rows[checkCarNum].ItemArray[13].ToString());
                PaymentNum.Value = v = Payment;
                fuelCapacity1Txt.Text = allCar.Tables[0].Rows[checkCarNum].ItemArray[12].ToString();
                distance1Txt.Text = oldDistance = allCar.Tables[0].Rows[checkCarNum].ItemArray[10].ToString();
                carStatusTxt.Text = allCar.Tables[0].Rows[checkCarNum].ItemArray[19].ToString();*/
            AddPhraseLeft(new Phrase(carNumTxt.Text, tableFont));
            AddPhraseLeft(new Phrase(carManufacturerTxt.Text, tableFont));
            AddPhraseLeft(new Phrase(carModelTxt.Text, tableFont));
            AddPhraseLeft(new Phrase(details.Rent_Date.ToString("dd/MM/yyyy"), tableFont));
            AddPhraseLeft(new Phrase(details.Returned_Date.ToString("dd/MM/yyyy"), tableFont));
            AddPhraseLeft(new Phrase(details.Rent_Date.ToString("HH:mm"), tableFont));
            AddPhraseLeft(new Phrase(details.Returned_Date.ToString("HH:mm"), tableFont));

            AddPhraseBottom(new Phrase("תשלום ליום:", tableFont));
            AddPhraseBottom(new Phrase("ק" + '"' + "מ ליום:", tableFont));
            AddPhraseBottom(new Phrase("ק" + '"' + "מ מריבי:", tableFont));
            AddPhraseBottom(new Phrase("ק" + '"' + "מ יציאה:", tableFont));
            AddPhraseBottom(new Phrase("ק" + '"' + "מ חזרה:", tableFont));
            AddPhraseBottom(new Phrase("דלק יציאה:", tableFont));
            AddPhraseBottom(new Phrase("דלק חזרה:", tableFont));


            AddPhraseLeft(new Phrase(details.Price_For_Day.ToString() + "₪", tableFont));
            AddPhraseLeft(new Phrase(details.Km_For_Day.ToString(), tableFont));
            AddPhraseLeft(new Phrase((details.Km_For_Day * details.Rent_Days + details.Rent_Distance).ToString(), tableFont));
            AddPhraseLeft(new Phrase(details.Rent_Distance.ToString(), tableFont));
            AddPhraseLeft(new Phrase(returnDistanceTxt.Text, tableFont));
            AddPhraseLeft(new Phrase(details.Quantity_Fuel_Rent, tableFont));
            AddPhraseLeft(new Phrase(returnedFuelCapacityTxt.Text, tableFont));




            AddPhraseBottom(new Phrase("סכ" + '"' + "ה לתשלום:", tableFont));
            AddPhraseBottom(new Phrase("סכ" + '"' + "ה ימים:", tableFont));
            AddPhraseBottom(new Phrase("מצב תשלום:", tableFont));
            AddPhraseBottom(new Phrase("חריגת ק" + '"' + "מ:", tableFont));
            AddPhraseBottom(new Phrase("חיוב בגין חריגת ק" + '"' + "מ:", tableFont));
            AddPhraseBottom(new Phrase("חריגת דלק:", tableFont));
            AddPhraseBottom(new Phrase("חיוב בגין חריגת דלק:", tableFont));


            AddPhraseLeft(new Phrase(details.Payment.ToString() + "₪", tableFont));
            AddPhraseLeft(new Phrase(details.Rent_Days.ToString(), tableFont));
            AddPhraseLeft(new Phrase(details.Payment_Type, tableFont));
            AddPhraseLeft(new Phrase(differenceDistanceTxt.Text, tableFont));
            AddPhraseLeft(new Phrase(differenceDistanceTxt.Text==string.Empty?"" : differenceDistanceTxt.Text + "₪", tableFont));

            AddPhraseLeft(new Phrase(differenceFuelCapacityTxt.Text, tableFont));

            AddPhraseLeft(new Phrase(differenceFuelCapacityTxt.SelectedIndex != -1 ? differenceFuelCapacityTxt.SelectedIndex * 25 + "₪" : "", tableFont));


            doc.Add(myTable);
            TableCells(2, 1f);
            AddBlankRow(2);
            tableBoldFont.Size = 11;
            AddPhraseBottom(new Phrase("מצב רכב יציאה:", tableBoldFont));
            AddPhraseBottom(new Phrase("מצב רכב חזרה:", tableBoldFont));

            myTable.AddCell(new PdfPCell(new Phrase(carStatusTxt.Text, tableFont)) { BorderWidthTop = 0, HorizontalAlignment = Element.ALIGN_LEFT });
            myTable.AddCell(new PdfPCell(new Phrase(carReturnStatusTxt.Text, tableFont)) { BorderWidthTop = 0, HorizontalAlignment = Element.ALIGN_LEFT });
            doc.Add(myTable);
            AddBlankRow(2);
            TableCells(4, 1f);

            tableBoldFont.SetStyle(4);
            myTable.HorizontalAlignment = Element.ALIGN_MIDDLE;
            myTable.AddCell(new PdfPCell(new Phrase("\n\n\nחתימה ביציאה:", tableFont)) { HorizontalAlignment = Element.ALIGN_CENTER, Border = 0 });
            //AddBlankPhrase(new Phrase("חתימה ביציאה:", tableBoldFont));
            
            //MemoryStream sigImage = new MemoryStream();
            byte[] sign = dbManger.Retrieve_signature(details.Rental_ID, true);
            Image sig_image = null;
            //mangerFrm.wacom.LastSignature.Save(sigImage, System.Drawing.Imaging.ImageFormat.Png);
            if (sign!=null)
                sig_image = Image.GetInstance(sign);
            //FileStream fs;
            //sigImage.Write(sign, 0, sign.Length);
            //pb2.Image = new Bitmap(sigImage);
            /*
            fs = new FileStream("Zombie", FileMode.Create, FileAccess.Write);
            sigImage.WriteTo(fs);*/
            // sig_image.ScalePercent(40f);
            myTable.AddCell(sig_image);
            //AddBlankPhrase(new Phrase("חתימה בחזרה:", tableBoldFont));
            sig_image = null;
            myTable.AddCell(new PdfPCell(new Phrase("\n\n\nחתימה בחזרה:", tableFont)) { HorizontalAlignment = Element.ALIGN_CENTER, Border = 0 });
            sign = dbManger.Retrieve_signature(details.Rental_ID, false);
            if (sign != null)
                sig_image = Image.GetInstance(sign);
            myTable.AddCell(sig_image);
            //AddBlankPhrase(new)
            doc.Add(myTable);
            /*MemoryStream chartImage = new MemoryStream();
            reportChart.SaveImage(chartImage, ChartImageFormat.Png);
            Image chart_image = Image.GetInstance(chartImage.GetBuffer());
            chart_image.ScalePercent(50f);*/
            /* var size = doc.PageSize;
             var per = chart_image.Width / chart_image.Height;
             chart_image.ScaleAbsoluteWidth(size.Width * 0.92f);*/
            //MemoryStream sigImage = new MemoryStream();
            //wacom.LastSignature.Save(sigImage, System.Drawing.Imaging.ImageFormat.Png);
            //doc.Add(Image.GetInstance(sigImage.GetBuffer()));
            // chart_image.ScaleAbsoluteHeight(size.Height / per);
            // doc.Add(chart_image);


            /*  MemoryStream sigImage = new MemoryStream();
              m.wacom.LastSignature.Save(sigImage, System.Drawing.Imaging.ImageFormat.Png);
              Image sig_image = Image.GetInstance(sigImage.GetBuffer());
              sig_image.ScalePercent(40f);*/
            //sig_image.SetAbsolutePosition(0f,doc.PageSize.Height- sig_image.Height - 10);
            //   doc.Add(sig_image);
            //doc.Add([Bitmap]m.wacom.LastSignature);
        }
        private void AddBlankPhrase(Phrase phrase)
        {
            myTable.AddCell(new PdfPCell(phrase) { BorderWidth = 0 });
        }
        private void AddPhrase(Phrase phrase)/*PDF פונקציה הוספת תא למסמך ה  */
            {
                //{ NoWrap=true}

                myTable.AddCell(new PdfPCell(phrase) );// { FixedHeight = 20f });
        }
        private void AddPhraseLeft(Phrase phrase)/*PDF פונקציה הוספת תא למסמך ה  */
        {
            //{ NoWrap=true}

            myTable.AddCell(new PdfPCell(phrase) { BorderWidthTop = 0, HorizontalAlignment = Element.ALIGN_RIGHT });// { FixedHeight = 20f });
        }
        private void AddPhraseBottom(Phrase phrase)/*PDF פונקציה הוספת תא למסמך ה  */
        {
            //{ NoWrap=true}

            myTable.AddCell(new PdfPCell(phrase) { BorderWidthBottom = 0});// { FixedHeight = 20f });
        }

        private void TaxInvoiceBtn_Click(object sender, EventArgs e)
        {
            string fuel = differenceFuelCapacityTxt.SelectedIndex.ToString() == "-1" ? "0" : differenceFuelCapacityTxt.SelectedIndex.ToString();
            double distance = double.TryParse(differenceDistanceTxt.Text, out distance) ? distance : 0;
            if (dbManger.CheckRentId(int.Parse(rentIdTxt.Text))&&(saveBtn_WasClicked|| ((rentMode == RentType.UpdateReturn||rentMode==RentType.Update)&& float.Parse(balanceDueTxt.Text)==0)))
            {
                Rent_Details rentDetails = new Rent_Details(int.Parse(rentIdTxt.Text), carNumTxt.Text, fuel, distance, float.Parse(balanceDueTxt.Text), float.Parse(PaymentNum.Value.ToString()));
                Scan_All_Drivers();
                frmInvoice frmInvoice = null;
                if (rentMode == RentType.Return)
                {
                    frmInvoice = new frmInvoice(frmInvoice.InvoiceType.Return, this, rentDetails,drivers[0]);
                    //frmInvoice.AddInvoiceLoad();
                }
                else if (rentMode == RentType.UpdateReturn || rentMode == RentType.Update)
                {
                    rentDetails.Return_Distance = 0;
                    rentDetails.Quantity_Fuel_Returned = "0";
                    frmInvoice = new frmInvoice(frmInvoice.InvoiceType.Return, this, rentDetails, drivers[0]);
                }
                else if (double.Parse(balanceDueTxt.Text) > 0)
                {
                    frmInvoice = new frmInvoice(frmInvoice.InvoiceType.Add, this, rentDetails, drivers[0]);
                }
                if (frmInvoice != null)
                    frmInvoice.ShowDialog();
                saveBtn_WasClicked = false;
            }
            else
                MessageBox.Show("Save The new rent before Or Not Connected\ntry again later", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BalanceDueTxt_TextChanged(object sender, EventArgs e)
        {
            if (double.Parse(balanceDueTxt.Text) == 0)
            {
                balanceDueTxt.BackColor = System.Drawing.Color.LightGreen;
                //saveBtn.Enabled = true;
            }
            else
            {
                balanceDueTxt.BackColor = System.Drawing.Color.LightCoral;
                //saveBtn.Enabled = false;
            }
        }
        public void AddTaxInvoice(float invoicePayed)//,int invoiceId,string invoiceDate,float invoiceTotal)
        {
            // taxInvoiceDGV.Rows.Add(invoiceId, invoiceDate, invoiceTotal);
            DataSet taxInvoice = dbManger.TaxInvoice(rentIdTxt.Text);
            if ((taxInvoice != null) && (taxInvoice.Tables.Count > 0) && (taxInvoice.Tables["tax_invoice"].Rows.Count > 0))
                taxInvoiceDGV.DataSource = taxInvoice.Tables["tax_invoice"];
            taxInvoiceDGV.Columns[InvoiceDateCol.Name].DefaultCellStyle.Format = "dd/MM/yyyy";

            payed += invoicePayed;
            balanceDueTxt.Text = "0";
        }

        private void SumPaymentTxt_TextChanged(object sender, EventArgs e)
        {
            balanceDueTxt.Text = (double.Parse(sumPaymentTxt.Text) - payed).ToString();
        }

        private void TaxInvoiceDGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            using (frmInvoice frmInvoice = new frmInvoice(frmInvoice.InvoiceType.View, int.Parse(taxInvoiceDGV.SelectedRows[0].Cells[InvoiceIdCol.Index].Value.ToString())))
            {
                if (frmInvoice != null)
                    frmInvoice.ShowDialog();
            }
        }

        private void rentDaysNum1_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (sender as NumericUpDown).NumericUpDown_Validating();
        }

        private void PrintBtn_Click(object sender, EventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\RentCarAppLast desktop\\temp\\toPrint.pdf";
            BaseFont tableFont1 = BaseFont.CreateFont(@"C:\Windows\Fonts\GISHA.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            myTable = new PdfPTable(5) { RunDirection = PdfWriter.RUN_DIRECTION_RTL };
            Font tableFont = new Font(tableFont1, 12);
            frmPrint printForm = new frmPrint();
            try
            {
                Scan_All_Drivers();
                DateTime date1 = new DateTime(rentDateTime.Value.Year, rentDateTime.Value.Month, rentDateTime.Value.Date.Day, rentTime.Value.Hour, rentTime.Value.Minute, rentTime.Value.Second);
                DateTime date2 = new DateTime(returnDate.Value.Year, returnDate.Value.Month, returnDate.Value.Date.Day, returnTime.Value.Hour, returnTime.Value.Minute, returnTime.Value.Second);
                details = new Rent_Details(int.Parse(rentIdTxt.Text), date1, int.Parse(rentDaysNum1.Value.ToString()), date2, float.Parse(sumPaymentTxt.Text), PayStatusCB.SelectedItem.ToString(), float.Parse(PaymentNum.Value.ToString()), int.Parse(kmNum.Value.ToString()), double.Parse(distance1Txt.Text), fuelCapacity1Txt.Text, carStatusTxt.Text, employeeID, drivers[0].Id, carNumTxt.Text);
                doc = new Document();
                PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));
                doc.Open();
                SetIntTable(details);
                doc.Close();
                DialogResult dr = printForm.ShowDialog();
                if (dr == DialogResult.OK && printForm.PrinterName != string.Empty)
                {
                    RawPrint.IPrinter printer = new RawPrint.Printer();
                    printer.PrintRawFile(printForm.PrinterName, path, Path.GetFileName(path));
                }
            }
            catch (IOException IO)
            {
                MessageBox.Show(IO.Message, "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                printForm.Dispose();
            }
        }

        
    }

}
        /*  public void show2(string btnText)//if we choose rent or return car
          {
              if (btnText == "rentCarBtn")
                  returnGB.Enabled = false;
              else if (btnText == "returnCarBtn") 
              {
                  rentGB.Enabled = false;
                  carGB.Enabled = false;
                  saveBtn.Text = "החזרה";
              }
              this.Show();
          }*/


        /* private void toolTip1_Draw(object sender, DrawToolTipEventArgs e)
         {
             e.Graphics.FillRectangle(SystemBrushes.Info, e.Bounds);
             e.DrawBorder();
             e.DrawText(TextFormatFlags.RightToLeft | TextFormatFlags.Right);
         }*/


        /*private void complete_DriverInfoName()
        {
            checkDriverName = -1;
            for (int i = 0; i < allDrivers.Tables[0].Rows.Count; i++)
                if (allDrivers.Tables[0].Rows[i].ItemArray[1].ToString() == firstNameTxt.Text)
                    checkDriverName = i;
            if (checkDriverName != -1)
            {
                idNumTxt.Text = allDrivers.Tables[0].Rows[checkDriverName].ItemArray[0].ToString();
                lastNameTxt.Text = allDrivers.Tables[0].Rows[checkDriverName].ItemArray[2].ToString();
                phoneNumTxt.Text = allDrivers.Tables[0].Rows[checkDriverName].ItemArray[3].ToString();
                addressTxt.Text = allDrivers.Tables[0].Rows[checkDriverName].ItemArray[4].ToString();
                telephoneNumTxt.Text = allDrivers.Tables[0].Rows[checkDriverName].ItemArray[5].ToString();
                BirthdateTimePicker.Value = Convert.ToDateTime(allDrivers.Tables[0].Rows[checkDriverName].ItemArray[6].ToString());
                licenseExpiryDate.Value = Convert.ToDateTime(allDrivers.Tables[0].Rows[checkDriverName].ItemArray[7].ToString());
                licenseIssueDate.Value = Convert.ToDateTime(allDrivers.Tables[0].Rows[checkDriverName].ItemArray[8].ToString());

                //Telephone_Number,Birthdate,Expire_Date,Date_Of_Issue
            }
            else if (checkDriverName == -1)
                     ClearAddDriver();
        }
        private void complete_DriverInfoLastName()
        {
            checkDriverLastName = -1;
            for (int i = 0; i < allDrivers.Tables[0].Rows.Count; i++)
                if (allDrivers.Tables[0].Rows[i].ItemArray[2].ToString() == lastNameTxt.Text)
                    checkDriverLastName = i;
            if (checkDriverLastName != -1)
            {
                idNumTxt.Text = allDrivers.Tables[0].Rows[checkDriverLastName].ItemArray[0].ToString();
                firstNameTxt.Text = allDrivers.Tables[0].Rows[checkDriverLastName].ItemArray[1].ToString();
                phoneNumTxt.Text = allDrivers.Tables[0].Rows[checkDriverLastName].ItemArray[3].ToString();
                addressTxt.Text = allDrivers.Tables[0].Rows[checkDriverLastName].ItemArray[4].ToString();
                telephoneNumTxt.Text = allDrivers.Tables[0].Rows[checkDriverLastName].ItemArray[5].ToString();
                BirthdateTimePicker.Value = Convert.ToDateTime(allDrivers.Tables[0].Rows[checkDriverLastName].ItemArray[6].ToString());
                licenseExpiryDate.Value = Convert.ToDateTime(allDrivers.Tables[0].Rows[checkDriverLastName].ItemArray[7].ToString());
                licenseIssueDate.Value = Convert.ToDateTime(allDrivers.Tables[0].Rows[checkDriverLastName].ItemArray[8].ToString());

                //Telephone_Number,Birthdate,Expire_Date,Date_Of_Issue
            }
            else if (checkDriverLastName == -1)
                     ClearAddDriver();
        }*/
