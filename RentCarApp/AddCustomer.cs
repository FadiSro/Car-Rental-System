using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RentCarApp
{
    public partial class AddCustomer : Form
    {
        private DbManager manger;
        private Manager m;
        private Driver_Licensing newCustomer;
        private string id;
        private string idEmployee;
        public enum EditMode
        {
            Add,
            Update
        }
        EditMode editMode;
        public AddCustomer(Manager m, EditMode mode)
        {
            InitializeComponent();
            manger = new DbManager();
            this.m = m;
            editMode = mode;
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
       /* private bool IsDig(string s)
        {
            int num;
            int.TryParse(s, out num);
            if (s != string.Empty && num > 0)
                return true;
            return false;
        }*/
        /*פונקציה לבדיקת תקינות תוקף רשיון ואם הנהג הוא נהג חדש*/
        private bool CheckDriverLicense(DateTime dateissue, DateTime dateExpiry, DateTime bDate)//check the driver license and if the driver not a new driver
        {
            int driverLicenseYears = (int)(DateTime.Now - dateissue).TotalDays / 365;
            int age = (int)(DateTime.Now - bDate).TotalDays / 365;
            if ((dateExpiry - dateissue).TotalDays > 0 && (dateExpiry - DateTime.Now).TotalDays > 0 && driverLicenseYears >= 2 && (age - driverLicenseYears) >= 17)
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
        /*פונקציה לבדוק אם מוספים לקוח או מעדכנים לקוח*/
        private void addCustomerBtn_Click(object sender, EventArgs e)
        {
            if (editMode == EditMode.Add)
            {
                Add();
            }
            else
            {
                UpdateCustomer();
            }
        }
        /*פונקציה לבדיקת תקינות קלט ללקוח ש רוצים להוסיף */
        private void Add() 
        {
            if (idNumTxt.Text.IsDig() && IsHeOrEn(firstNameTxt.Text) && IsHeOrEn(lastNameTxt.Text) && addressTxt.Text != string.Empty && licenseIssueDate.Text != string.Empty && licenseExpiryDate.Text != string.Empty && IsAge21(BirthdateTimePicker) && phoneNumTxt.Text.IsDig() && (telephoneNumTxt.Text.IsDig() || telephoneNumTxt.Text == string.Empty) && CheckDriverLicense(licenseIssueDate.Value.Date, licenseExpiryDate.Value.Date, BirthdateTimePicker.Value.Date))
            {
                newCustomer = new Driver_Licensing(idNumTxt.Text, firstNameTxt.Text, lastNameTxt.Text, addressTxt.Text, BirthdateTimePicker.Value.Date, phoneNumTxt.Text, telephoneNumTxt.Text, licenseExpiryDate.Value.Date, licenseIssueDate.Value.Date);
                if (manger.AddNewCustomer(newCustomer))
                {
                    MessageBox.Show("Added Finished", "Success", MessageBoxButtons.OK);
                    ClearAddDriver();
                    m.onloadCustomer();
                }
                else
                    MessageBox.Show("Failed To Add : or(Customer exists)", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Failed To Add driver Check your information", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /*פונקציה לבדיקת תקינות קלט ללקוח ש רוצים לעדכן */
        private void UpdateCustomer()
        {
            if (idNumTxt.Text.IsDig() && IsHeOrEn(firstNameTxt.Text) && IsHeOrEn(lastNameTxt.Text) && addressTxt.Text != string.Empty && licenseIssueDate.Text != string.Empty && licenseExpiryDate.Text != string.Empty && IsAge21(BirthdateTimePicker) && phoneNumTxt.Text.IsDig() && (telephoneNumTxt.Text.IsDig() || telephoneNumTxt.Text == string.Empty) && CheckDriverLicense(licenseIssueDate.Value.Date, licenseExpiryDate.Value.Date, BirthdateTimePicker.Value.Date))
            {
                newCustomer = new Driver_Licensing(idNumTxt.Text, firstNameTxt.Text, lastNameTxt.Text, addressTxt.Text, BirthdateTimePicker.Value.Date, phoneNumTxt.Text, telephoneNumTxt.Text, licenseExpiryDate.Value.Date, licenseIssueDate.Value.Date);
                if (manger.UpdateOldCustomer(newCustomer, id))
                {
                   // MessageBox.Show("Update Finished", "Success", MessageBoxButtons.OK);
                    m.onloadCustomer();
                    if (idEmployee == id)
                    {
                        this.Hide();
                        m.Hide();
                        Program.frmLogin.Show();
                        this.Close();
                        m.Close();
                    }
                    else
                        this.Close();
                }
                else
                    MessageBox.Show("Failed To Update : or(Customer exists)", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Failed To Update driver Check your information", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /*פונקציה למלות את נתוני הלקוח שרוצים לעדכן*/
        public void Update_CustomerLoad(DataGridViewRow row, Employee employee)
        {
            if (employee.Employye_type == "מזכירות")
                idNumTxt.Enabled = false;
            id = idNumTxt.Text = row.Cells[0].Value.ToString();
            firstNameTxt.Text = row.Cells[1].Value.ToString();
            lastNameTxt.Text = row.Cells[2].Value.ToString();
            phoneNumTxt.Text = row.Cells[3].Value.ToString();
            addressTxt.Text = row.Cells[4].Value.ToString();
            telephoneNumTxt.Text = row.Cells[5].Value.ToString();
            BirthdateTimePicker.Text = row.Cells[6].Value.ToString();
            licenseExpiryDate.Text = row.Cells[7].Value.ToString();
            licenseIssueDate.Text = row.Cells[8].Value.ToString();
            addCustomerBtn.Text = "עדכון לקוח";
            idEmployee = employee.Id;
        }
    }
}
        /* if (addCustomerBtn.Text == "הוספת לקוח")
         {
             if (IsDig(idNumTxt.Text) && IsHeOrEn(firstNameTxt.Text) && IsHeOrEn(lastNameTxt.Text) && addressTxt.Text != string.Empty && licenseIssueDate.Text != string.Empty && licenseExpiryDate.Text != string.Empty && IsAge21(BirthdateTimePicker) && IsDig(phoneNumTxt.Text) && IsDig(telephoneNumTxt.Text) && CheckDriverLicense(licenseIssueDate.Value.Date, licenseExpiryDate.Value.Date, BirthdateTimePicker.Value.Date))
             {
                 newCustomer = new Driver_Licensing(idNumTxt.Text, firstNameTxt.Text, lastNameTxt.Text, addressTxt.Text, BirthdateTimePicker.Value.Date, phoneNumTxt.Text, telephoneNumTxt.Text, licenseExpiryDate.Value.Date, licenseIssueDate.Value.Date);
                 if (manger.addNewCustomer(newCustomer))
                 {
                     MessageBox.Show("Added Finished", "Success", MessageBoxButtons.OK);
                     ClearAddDriver();
                      m.onloadCustomerAdd();
                 }
                 else
                     MessageBox.Show("Failed To Add : or(Customer exists)", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
             }
             else
                 MessageBox.Show("Failed To Add driver Check your information", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
         }
         else
             UpdateCustomerBtn_Click();*/

