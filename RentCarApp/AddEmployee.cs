using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RentCarApp
{
    public partial class AddEmployee : Form
    {
        private readonly DbManager manger;
        private Employee newEmployee;
        private string job;
        private DateTime start_Date;
        private DateTime end_Date;
        private bool active;
        private string id;
        private string idEmployee;
        private readonly Manager m;
        private readonly string hash = "f0xle@rn";
        public enum EditMode
        {
            Add,
            Update
        }
        readonly EditMode editMode;
        public AddEmployee(Manager m, EditMode mode,bool EmployeeActive)
        {
            InitializeComponent();
            manger = new DbManager();
            this.m = m;
            editMode = mode;
            if (editMode == EditMode.Update)
            {
                if (!EmployeeActive)
                {
                    activeCB.Visible = true;
                    label10.Visible = true;
                }
                else
                {
                    activeCB.Checked = true;
                }
            }
        }
        /*פונקציה להצפין את הסיסמה */
        private string PassEncrypt(string password)
        {
            string str;
            byte[] data = UTF8Encoding.UTF8.GetBytes(password);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDES = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDES.CreateEncryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    str = Convert.ToBase64String(results, 0, results.Length);
                }
            }
            return str;
        }
        /*פונקציה לבדוק אם מוספים עוביד  או מעדכנים עוביד*/
        private void AddEmployeeBtn_Click(object sender, EventArgs e)
        {
            if (editMode == EditMode.Add)
            {
                Add();
            }
            else
            {
                UpdateEmployeeBtn_Click();
            }
        }
        /*פונקציה לבדיקת תקינות קלט לעוביד ש רוצים להוסיף */

        private void Add()
        {
            if ((idNumTxt.Text.IsDig()) && IsHeOrEn(firstNameTxt) && IsHeOrEn(lastNameTxt) && addressTxt.Text != string.Empty && phoneNumTxt.Text.IsDig() && (telephoneNumTxt.Text.IsDig()||telephoneNumTxt.Text==string.Empty) && IsAge21(BirthdateTimePicker))
            {
                string SalaryNumS = SalaryNum.Value.ToString();
                float SalaryNumF = float.Parse(SalaryNumS);
                newEmployee = new Employee(idNumTxt.Text, firstNameTxt.Text, lastNameTxt.Text, addressTxt.Text, BirthdateTimePicker.Value.Date, phoneNumTxt.Text, SalaryNumF, jobTxt.SelectedItem.ToString(), telephoneNumTxt.Text);
                newEmployee.Password = PassEncrypt(newEmployee.Password);
                if (manger.AddNewEmployee(newEmployee))
                {
                    MessageBox.Show("Added Finished", "Success", MessageBoxButtons.OK);
                    ClearAddEmployee();
                    m.onload();
                }
                else
                    MessageBox.Show("Failed To Add : or(employee exists)", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Check your information", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /*פונקציה לבדיקת תקינות קלט של השדות*/
        private bool IsValid()
        {
            return idNumTxt.Text.IsDig() &&
                IsHeOrEn(firstNameTxt) &&
                IsHeOrEn(lastNameTxt) &&
                addressTxt.Text != string.Empty &&
                phoneNumTxt.Text.IsDig() &&
                (telephoneNumTxt.Text.IsDig() || telephoneNumTxt.Text==string.Empty)&&
                IsAge21(BirthdateTimePicker);
        }
        /*פונקציה לבדיקת תקינות קלט לעוביד ש רוצים לעדכן */
        private void UpdateEmployeeBtn_Click()//update
        {
            if (IsValid())
            {
                string SalaryNumS = SalaryNum.Value.ToString();
                float SalaryNumF = float.Parse(SalaryNumS);
                Employee updateEmployee = new Employee(idNumTxt.Text, firstNameTxt.Text, lastNameTxt.Text, addressTxt.Text, BirthdateTimePicker.Value.Date, phoneNumTxt.Text, SalaryNumF, jobTxt.SelectedItem.ToString(), telephoneNumTxt.Text);
                updateEmployee.Employee_availability = activeCB.CheckState.ToString() == "Checked"?true:false;
                updateEmployee.Password =manger.EmployeePass(id);
                if (!active && activeCB.CheckState.ToString() == "Checked")
                {
                    active = true;
                    start_Date = DateTime.Now;
                    end_Date = Convert.ToDateTime("01-01-0001");
                }
                if (manger.UpdateOldEmployee(updateEmployee, active, start_Date, end_Date, id))
                {
                    updateEmployee.End_work_date = end_Date;
                    m.updateEmployee(updateEmployee);
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
                    MessageBox.Show("Failed To update", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Check your information", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /*פונקציה לנקות את כל השדות בממשק אחרי שהספנו עוביד*/
        private void ClearAddEmployee()
        {
            idNumTxt.Clear();
            jobTxt.SelectedIndex = 1;
            firstNameTxt.Clear();
            lastNameTxt.Clear();
            addressTxt.Clear();
            BirthdateTimePicker.Value = DateTime.Now;
            phoneNumTxt.Clear();
            telephoneNumTxt.Clear();
            SalaryNum.Value = 2000;
        }
        /*פונקציה  לבדוק אם הנקלט בעברית או באנגלית*/
        private bool IsHeOrEn(TextBox s)
        {
            string txt = s.Text.Replace(" ", "");
            if (txt != string.Empty && (Regex.IsMatch(txt, @"^[a-zA-Z]+$") | Regex.IsMatch(txt, @"^[א-ת]+$")))
                return true;
            return false;
        }
        /*פונקציה לבדוק אם העובד גדול מ 21*/
        private bool IsAge21(DateTimePicker Bdate)
        {
            TimeSpan tmp = DateTime.Now.Date - Bdate.Value.Date;
            int currentAge = (int)tmp.TotalDays / 365;
            if (currentAge >= 21)
                return true;
            return false;
        }

        //defult value to comboBox jobTxt is מזכירות  
        private void AddEmployee_Load(object sender, EventArgs e)
        {
            jobTxt.SelectedIndex = 1;
            if (job == "מנהל")//do it with else
                jobTxt.SelectedIndex = 0;
        }
        /*פונקציה למלות את נתוני העובד שרוצים לעדכן*/
        public void Update_Employee(DataGridViewRow row, string idEmp)
        {
            firstNameTxt.Text = row.Cells[0].Value.ToString();
            lastNameTxt.Text = row.Cells[1].Value.ToString();
            id = idNumTxt.Text = row.Cells[2].Value.ToString();
            job = row.Cells[3].Value.ToString();
            phoneNumTxt.Text = row.Cells[4].Value.ToString();
            addressTxt.Text = row.Cells[5].Value.ToString();
            telephoneNumTxt.Text = row.Cells[6].Value.ToString();
//            string str = row.Cells[7].Value.ToString();
            decimal Salary = (decimal)Convert.ChangeType(row.Cells[7].Value.ToString(), typeof(decimal));
            SalaryNum.Value = Salary;
            active = (bool)row.Cells[8].Value;
            BirthdateTimePicker.Text = row.Cells[9].Value.ToString();
            addEmployeeBtn.Text = "עדכון עובד";
            start_Date = (DateTime)row.Cells[10].Value;
            DateTime tmpEnd = Convert.ToDateTime("01/01 /0001");
            if (row.Cells[11].Value != DBNull.Value)
                tmpEnd = Convert.ToDateTime(row.Cells[11].Value.ToString());
            end_Date = tmpEnd;
            idEmployee = idEmp;
        }

        private void SalaryNum_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (sender as NumericUpDown).NumericUpDown_Validating();
        }
    }
}
 /*private string passDecrypt(string encrypt_password)
       {
           string str;
           byte[] data = Convert.FromBase64String(encrypt_password);
           using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
           {
               byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
               using (TripleDESCryptoServiceProvider tripDES = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
               {
                   ICryptoTransform transform = tripDES.CreateDecryptor();
                   byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                   str = UTF8Encoding.UTF8.GetString(results);
               }
           }
           return str;
       }*/