using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace RentCarApp
{
    public partial class LogIn : Form
    {
        private DbLogIn logIn;
        private Employee employee;
        private readonly string hash = "f0xle@rn";

        public LogIn()
        {
            InitializeComponent();
            /*UserTB.Text = "207075680";
            PassTB.Text = "207075680e";*/

            logIn = new DbLogIn();
            employee = new Employee();
            //server = "localhost";
            // database = "rentcar";//user1DB
            /* user = "root";//user1DB_user
             password = "12345678";*/
            // conn = new DBConnection();

            // conn.DatabaseName = database;
            /* conn.IsConnect();
             string cmdText = "INSERT INTO car (Car_Number,Car_Manufacturer,Car_Model) VALUES (@Car_Number, @Car_Manufacturer , @Car_Model)";
             UPDATE `employee` SET `Employye_Type`="מזכירות",`Start_Work_Date`='2019-01-20' WHERE `ID_Employee`="12345678" and `Password`="12345678"
             MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
             cmd.Parameters.AddWithValue("@Car_Number","125662");
             cmd.Parameters.AddWithValue("@Car_Manufacturer","audi");
             cmd.Parameters.AddWithValue("@Car_Model", "301i");

             cmd.ExecuteNonQuery();*/
            // Console.WriteLine(conn.Connection);
            // Console.WriteLine(conn.DatabaseName);

        }

        private void passwordBtn_MouseUp(object sender, MouseEventArgs e)/*פונקציה שמסתרה את הסיסמה*/
        {
            PassTB.UseSystemPasswordChar = true;
        }

        private void passwordBtn_MouseDown(object sender, MouseEventArgs e)/*פונקציה שעוזרת לנו לראות את הסיסמה*/
        {

            PassTB.UseSystemPasswordChar = false;
        }
        private string passEncrypt(string password)/*פונקציה להצפין את הסיסמה */
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
        private void LogInBtn_Click(object sender, EventArgs e)/*פונקציה לעביר את המשתמש להדף המתאים לו אם הצליח להכנס*/
        {
            if (logIn.IsConnect())
            {
                if (UserTB.Text != string.Empty && UserTB.Text.All(char.IsDigit) && PassTB.Text != string.Empty)
                {
                    employee = logIn.LogIn(UserTB.Text, passEncrypt(PassTB.Text));//to login
                    if (employee != null)
                    {
                        TimeSpan dateDifference = DateTime.Now.Date - employee.Password_date.Date;
                        this.Hide();
                        if (UserTB.Text == PassTB.Text || dateDifference.TotalDays >= 60)//בודקים אם המשתמש נכנס פעם רשונה או שעבר 60 ימים וצריך לשנות את הסיסמה
                        {
                            PasswordChange frmPass = new PasswordChange(employee, false);
                            frmPass.ShowDialog();
                        }
                        else
                        {
                            Program.frmManager = new Manager(employee);
                            Program.frmManager.Show();
                        }
                        UserTB.Text = "";
                        PassTB.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Wrong Password Or user is not Available", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Info:Check your input ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                DialogResult result = MessageBox.Show("Info:Check your internet connection And try again \n OR click yes to open rent detail pdf file ", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                    System.Diagnostics.Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\RentCarAppLast desktop\\temp\\toRent.pdf");
            }
        }

        private void ForgetPassLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (UserTB.Text != string.Empty)
            {
                employee = logIn.ForgetPassword(UserTB.Text);
                if (employee != null && employee.Password != passEncrypt(employee.Id))
                {
                    PasswordChange frmForget = new PasswordChange(employee, true);
                    frmForget.ShowDialog();
                }
                else
                    MessageBox.Show("Wrong ID Or user is not Available", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Info: Put your ID ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
