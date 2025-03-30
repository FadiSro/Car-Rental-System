using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RentCarApp
{
    public partial class PasswordChange : Form
    {
        private string hash = "f0xle@rn";
        private Employee employee = null;
        private DBVerification PasswordVerification;
        private DataSet Questions = null;
        public PasswordChange(Employee emp,bool forgetPass)
        {
            InitializeComponent();
            PasswordVerification = new DBVerification();
            employee = emp;
            if (forgetPass==false&&emp.Password != passEncrypt(emp.Id))//
            {
                QuestionsGB.Enabled = false;
               // FillQuestionsAnswers();
            }
            if (forgetPass == true)
                ForgetPassword();
        }
        private void ForgetPassword()
        {
            //ShowPassBTN1.Enabled = false;
            OldPassword.Enabled = false;
            Question1.Enabled = false;
            Question2.Enabled = false;
            Question3.Enabled = false;
            QuestionCheckedBTN.Visible = true;
            PasswordGB.Enabled = false;
            UpdatePass.Enabled = false;
            FillQuestions();
        }
        private void FillQuestions()
        {
            Questions = PasswordVerification.AllQuestions(employee);
            if (Questions != null)
            {
                Question1.Text = Questions.Tables[0].Rows[0][1].ToString();
                Question2.Text = Questions.Tables[0].Rows[0][3].ToString();
                Question3.Text = Questions.Tables[0].Rows[0][5].ToString();
            }
        }
        private void FillQuestionsAnswers()
        {
            Questions = PasswordVerification.AllQuestions(employee);
            if(Questions!=null)
            {
                Question1.Text = Questions.Tables[0].Rows[0][1].ToString();
                Answer1.Text = Questions.Tables[0].Rows[0][2].ToString();
                Question2.Text = Questions.Tables[0].Rows[0][3].ToString();
                Answer2.Text = Questions.Tables[0].Rows[0][4].ToString();
                Question3.Text = Questions.Tables[0].Rows[0][5].ToString();
                Answer3.Text = Questions.Tables[0].Rows[0][6].ToString();
            }
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
        private void PasswordChange_FormClosing(object sender, FormClosingEventArgs e)
        {
            Program.frmLogin.Show();
        }
        private bool PassIsValid()
        {
            if (OldPassword.Text != string.Empty && NewPassword1.Text != string.Empty && NewPassword2.Text != string.Empty)
                if (employee.Password == passEncrypt(OldPassword.Text) && NewPassword1.Text == NewPassword2.Text)
                    if (employee.Password != passEncrypt(NewPassword1.Text) && employee.Id != NewPassword1.Text)
                        return true;
            return false;
        }
        private bool QuesIsValid()
        {
            if (QuestionsGB.Enabled == false)
                return true;
            if (QuestionsGB.Enabled == true && Question1.Text != string.Empty && Question2.Text != string.Empty && Question3.Text != string.Empty)
                if (Answer1.Text != string.Empty && Answer2.Text != string.Empty && Answer3.Text != string.Empty)
                    if (Question1.Text != Question2.Text && Question1.Text != Question3.Text && Question2.Text != Question3.Text)
                        return true;
            return false;
        }
        private void UpdatePass_Click(object sender, EventArgs e)
        {
            if (PassIsValid() && QuesIsValid()&&!NewPassword1.Text.IsDigDouble()&&!NewPassword1.Text.IsHeOrEn())
            {
                employee.Password = passEncrypt(NewPassword1.Text);
                string[] Questions = { Question1.Text, Answer1.Text, Question2.Text, Answer2.Text, Question3.Text, Answer3.Text };//elias encrypt
                if ((QuestionsGB.Enabled == true && PasswordVerification.ChangePassword(employee, Questions)) || ((QuestionsGB.Enabled == false && PasswordVerification.ChangePassword(employee))))
                {
                    this.Hide();
                    Program.frmLogin.Show();
                    this.Close();   
                }
                else
                    MessageBox.Show("Failed To Update Password", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("INFO: Check your information\nPassword must contain letters and numbers", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private string passDecrypt(string encrypt_password)
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
        }
        private void ShowPassBTN1_MouseDown(object sender, MouseEventArgs e)
        {
            OldPassword.UseSystemPasswordChar = false;
        }

        private void ShowPassBTN1_MouseUp(object sender, MouseEventArgs e)
        {
            OldPassword.UseSystemPasswordChar = true;
        }

        private void ShowPassBTN2_MouseDown(object sender, MouseEventArgs e)
        {
            NewPassword1.UseSystemPasswordChar = false;
        }

        private void ShowPassBTN2_MouseUp(object sender, MouseEventArgs e)
        {
            NewPassword1.UseSystemPasswordChar = true;
        }

        private void ShowPassBTN3_MouseDown(object sender, MouseEventArgs e)
        {
            NewPassword2.UseSystemPasswordChar = false;
        }

        private void ShowPassBTN3_MouseUp(object sender, MouseEventArgs e)
        {
            NewPassword2.UseSystemPasswordChar = true;
        }

        private void QuestionCheckedBTN_Click(object sender, EventArgs e)
        {
            if(Answer1.Text== Questions.Tables[0].Rows[0][2].ToString()&& Answer2.Text == Questions.Tables[0].Rows[0][4].ToString()&& Answer3.Text == Questions.Tables[0].Rows[0][6].ToString())
            {
                QuestionsGB.Enabled = false;
                PasswordGB.Enabled = true;
                QuestionCheckedBTN.Visible = false;
                UpdatePass.Enabled = true;
                OldPassword.Text = passDecrypt(employee.Password);
            }
            else
                MessageBox.Show("INFO: Check your Answers", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


    }
}
