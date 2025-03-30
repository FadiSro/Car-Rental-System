using MySql.Data.MySqlClient;
using System;

namespace RentCarApp
{
    class DbLogIn : DBConnection
    {
        private DBConnection conn;
        private string database;
        public DbLogIn() : base()
        {
            database = "rentcar";
            conn = new DBConnection();
            conn.DatabaseName = database;
        }
        /*פונקציה לבדוק אם שם משתמש וסיסמה תקינים*/
        public Employee LogIn(string username, string password)
        {
            if (conn.IsConnect())
            {
                Employee user = new Employee();
                string cmdText = "SELECT * From employee JOIN user ON user.ID = employee.ID_Employee WHERE ID_Employee=" + username + " And Password='" + password + "' And Employee_Availability=" + 1;
                MySqlCommand cmd = conn.Connection.CreateCommand();
                cmd.CommandText = cmdText;
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        user.Employye_type = reader[4].ToString();
                        user.Id = reader[0].ToString();
                        user.First_name = reader["First_Name"].ToString();
                        user.Last_name = reader["Last_Name"].ToString();
                        /*lazem nqem*/
                        user.Password_date = DateTime.Parse(reader["Password_Date"].ToString());//Password_Date
                        user.Password = reader["Password"].ToString();
                        conn.Close();
                        return user;
                    }
                }
                conn.Close();
            }
            return null;
        }
        public Employee ForgetPassword(string username)
        {
            if (conn.IsConnect())
            {
                Employee user = new Employee();
                string cmdText = "SELECT * From employee JOIN user ON user.ID = employee.ID_Employee WHERE ID_Employee=" + username + "  And Employee_Availability=" + 1;
                MySqlCommand cmd = conn.Connection.CreateCommand();
                cmd.CommandText = cmdText;
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        user.Employye_type = reader[4].ToString();//Employye_type
                        user.Id = reader[0].ToString();//ID
                        user.First_name = reader["First_Name"].ToString();//First_Name
                        user.Last_name = reader["Last_Name"].ToString();//Last_Name
                        user.Password_date = DateTime.Parse(reader["Password_Date"].ToString());//Password_Date
                        user.Password = reader["Password"].ToString();
                        conn.Close();
                        return user;
                    }
                }
                conn.Close();
            }
            return null;
        }
    }
}
