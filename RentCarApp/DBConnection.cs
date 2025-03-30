using MySql.Data.MySqlClient;
using System;


namespace RentCarApp
{
    /*מחלקה להתחברות עם בסיס הנתונים*/
    class DBConnection
    {
        private MySqlConnection connection = null;
        private string databaseName = string.Empty;
        private static DBConnection _instance = null;
        public DBConnection()
        {
            databaseName = "rentcar";
        }

        public string DatabaseName
        {
            get { return databaseName; }
            set { databaseName = value; }
        }

        public string Password { get; set; }

        public MySqlConnection Connection
        {
            get { return connection; }
        }


        public static DBConnection Instance()
        {
            if (_instance == null)
                _instance = new DBConnection();
            return _instance;
        }
        //פונקציה מחזירה אמת אחרי בדיקת שהחיבור תקין
        public bool IsConnect()
        {
            try
            {
                if (Connection == null)
                {
                    if (String.IsNullOrEmpty(databaseName))
                        return false;
                    string connstring = string.Format("Server='localhost'; database={0}; UID={1}; password={2};CharSet=utf8; Convert Zero Datetime=True;", databaseName, "user1DB_user", "12345678");
                    //string connstring = string.Format("Server='localhost'; UID='id11833079_user1db_user'; pwd=12345678; CharSet=utf8; database='id11833079_user1db_user'; Convert Zero Datetime=True;") ;//"xQ5ym5v" "Server='petacil.org' "user1DB_user","12345678"
                    connection = new MySqlConnection(connstring);
                    connection.Open();
                }
                else if (connection.State.ToString().Equals("Closed"))
                {
                    connection.Open();
                }
                if (connection.State.ToString().Equals("Open"))
                    return true;
                return false;
            }
            catch
            {
                return false;
            }
        }

        public void Close()
        {
            if (connection != null)
            {
                connection.Close();
            }
        }
    }
}

