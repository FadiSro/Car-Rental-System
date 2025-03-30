using System;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace RentCarApp
{
    /*מחלקה לשימוש פרטי למשתנים מסוגים מסויימים*/
    public static class Utilities//utilities
    {
        /*DataGridView לטבלה מסוג dubleBuffered פונקציה לעשות*/
        public static void DoubleBuffered(this DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }
        /*int פונקציה  לבדוק אם הנקלט מספר מסוג */
        public static bool IsDig(this string s)
        {
            int num;
            //int.TryParse(s, out num);
            if (s != string.Empty && int.TryParse(s, out num)&& num >= 0)//i added equal zero..
                return true;
            return false;
        }
        public static bool IsDigDouble(this string s)
        {
            double num;
            //int.TryParse(s, out num);
            if (s != string.Empty && double.TryParse(s, out num) && num >= 0)//i added equal zero..
                return true;
            return false;
        }
        /*פונקציה  לבדוק אם הנקלט בעברית או באנגלית*/
        public static bool IsHeOrEn(this string s)
        {
            string txt = s.Replace(" ", "");
            if (txt != string.Empty && (Regex.IsMatch(txt, @"^[a-zA-Z]+$") | Regex.IsMatch(txt, @"^[א-ת]+$")))
                return true;
            return false;
        }
        public static bool IsDigFloat(this string s)
        {
            float num;
            if (s != string.Empty && float.TryParse(s, out num) && num >= 0)
                return true;
            return false;
        }
        public static void NumericUpDown_Validating(this NumericUpDown sender)
        {
            NumericUpDown num = (sender as NumericUpDown);
            if (num.Text == "" || num.Text == "-")
            {
                num.Value = num.Minimum;
                num.UpButton();
                num.DownButton();
            }
        }
    }
}
        /* public static bool IsValidId(this string str)
         {
             //return (str != string.Empty && str.All(char.IsDigit) && str.Length == 9);
         }
         */