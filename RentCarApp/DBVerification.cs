using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarApp
{
    class DBVerification:DBConnection
    {
        private DBConnection conn;
        private string database;
        public DBVerification() : base()
        {
            database = "rentcar";
            conn = new DBConnection();
            conn.DatabaseName = database;
        }
        public bool ChangePassword(Employee employee,string[] Questions)
        {
            if (conn.IsConnect())
            {
                string cmdText = " UPDATE employee SET Password_Date = '" + DateTime.Now.Date.ToString("yyyy/MM/dd") + "' , Password = '" + employee.Password + "' WHERE `ID_Employee`= " + employee.Id;
                MySqlCommand cmd = conn.Connection.CreateCommand();
                cmd.CommandText = cmdText;
                cmd.ExecuteNonQuery();
                cmdText = "INSERT INTO password_verification(ID_Employee, Question_1, Answer_1, Question_2, Answer_2, Question_3,Answer_3) VALUES(@ID_Employee, @Question_1, @Answer_1, @Question_2, @Answer_2, @Question_3,@Answer_3)";
                cmd = conn.Connection.CreateCommand();
                cmd.CommandText = cmdText;
                cmd.Parameters.AddWithValue("@ID_Employee",employee.Id);
                cmd.Parameters.AddWithValue("@Question_1", Questions[0]);
                cmd.Parameters.AddWithValue("@Answer_1", Questions[1]);
                cmd.Parameters.AddWithValue("@Question_2", Questions[2]);
                cmd.Parameters.AddWithValue("@Answer_2", Questions[3]);
                cmd.Parameters.AddWithValue("@Question_3", Questions[4]);
                cmd.Parameters.AddWithValue("@Answer_3", Questions[5]);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            return false;
        }
        public bool ChangePassword(Employee employee)
        {
            if (conn.IsConnect())
            {
                string cmdText = " UPDATE employee SET Password_Date = '" + DateTime.Now.Date.ToString("yyyy/MM/dd") + "' , Password = '" + employee.Password + "' WHERE `ID_Employee`= " + employee.Id;
                MySqlCommand cmd = conn.Connection.CreateCommand();
                cmd.CommandText = cmdText;
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            return false;
        }
        public DataSet AllQuestions(Employee employee)
        {
            try
            {
                if (conn.IsConnect())
                {
                    string cmdTxt = "SELECT * From password_verification where ID_Employee ='" + employee.Id + "'";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmdTxt, conn.Connection);
                    DataSet DS = new DataSet();
                    adapter.Fill(DS, "password_verification");
                    conn.Close();
                    return DS;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
