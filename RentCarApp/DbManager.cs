using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace RentCarApp
{
    class DbManager : DbSecretary
    {
        private DBConnection conn;
        private string database;
        private string hash = "f0xle@rn";

        public DbManager() : base()
        {
            database = "rentcar";//user1DB
            conn = new DBConnection();
            conn.DatabaseName = database;
        }
        /*פונקציה שבעזרתה מוצאים דוח*/
        public DataSet IncomeByDay(string dateFrom, string dateTo)
        {
            try
            {
                if (conn.IsConnect())
                {
                    string cmdTxt = "SELECT SUM(`Payment`) as סכום_תשלומים,`Rent_Date` as תאריך From rent_details where Rent_Date between '" + dateFrom + "' AND '" + dateTo + "' GROUP BY year(`Rent_Date`), month(`Rent_Date`), day(`Rent_Date`) ASC";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmdTxt, conn.Connection);
                    DataSet DS = new DataSet();
                    adapter.Fill(DS, "rent_details");
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
        /*פונקציה שבעזרתה מוצאים דוח*/
        public DataSet IncomeByEmployee(string dateFrom, string dateTo)
        {
            try
            {
                if (conn.IsConnect())
                {
                    string cmdTxt = "SELECT sum(`Payment`) as תשלום,CONCAT(`First_Name`,' ',`Last_Name`,'\r\n',`ID_Employee_Rent`) AS שם_עובד From rent_details JOIN user on `ID_Employee_Rent`=`ID` where Rent_Date between '" + dateFrom + "' AND '" + dateTo + "' GROUP BY `ID_Employee_Rent` ORDER BY תשלום ASC";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmdTxt, conn.Connection);
                    DataSet DS = new DataSet();
                    adapter.Fill(DS, "rent_details");
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
        /*פונקציה שבעזרתה מוצאים דוח*/
        public DataSet EmployeeRentCount(string dateFrom, string dateTo)
        {
            try
            {
                if (conn.IsConnect())
                {
                    string cmdTxt = "SELECT COUNT(`ID_Employee_Rent`) as כמות_שכירות,CONCAT(`First_Name`,' ',`Last_Name`,'\r\n',`ID_Employee_Rent`) AS שם_עובד From rent_details JOIN user on `ID_Employee_Rent`=`ID` where Rent_Date between '" + dateFrom + "' AND '" + dateTo + "' GROUP BY `ID_Employee_Rent` ORDER BY כמות_שכירות ASC";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmdTxt, conn.Connection);
                    DataSet DS = new DataSet();
                    adapter.Fill(DS, "rent_details");
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
        /*פונקציה שבעזרתה מוצאים דוח*/
        public DataSet CarRentCount(string dateFrom, string dateTo)
        {
            try
            {
                if (conn.IsConnect())
                {
                    string cmdTxt = "SELECT COUNT(*) AS כמות_שכירות,Concat(`Car_Manufacturer`,' ',`Car_Model`) as רכב  From rent_details JOIN car on rent_details.Car_Number = car.Car_Number where Rent_Date between '" + dateFrom + "' AND '" + dateTo + "' GROUP BY `Car_Manufacturer`,`Car_Model` ORDER BY כמות_שכירות ASC";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmdTxt, conn.Connection);
                    DataSet DS = new DataSet();
                    adapter.Fill(DS, "rent_details");
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
        /*פונקציה לחזיר את כל נתוני העובדים*/
        public DataSet AllEmployees()
        {
            try
            {
                if (conn.IsConnect())
                {
                    MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT First_Name,Last_Name,ID,Employye_Type,Phone_Number,Address,Telephone_Number,Salary,Employee_Availability,Birthdate,Start_Work_Date,End_Work_Date From employee JOIN user ON user.ID = employee.ID_Employee where ID != '0000000000'", conn.Connection);
                    DataSet DS = new DataSet();
                    adapter.Fill(DS, "employee");
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
        public string EmployeePass(string id)//function that returns encrypt pass
        {
            if (conn.IsConnect())
            {
                string cmdText = " SELECT Password From employee WHERE `ID_Employee` = '" + id + "'";
                using (MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection))
                {
                    cmd.CommandText = cmdText;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            if (reader["Password"].ToString() != string.Empty)
                                return reader["Password"].ToString();
                            reader.Close();
                            conn.Close();
                            return PassEncrypt(id);
                        }
                    }
                }
                conn.Close();
            }
            return null;
        }
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
        /*פונקציה למחוק עוד*/
        public bool RemoveEmployee(string id) 
        {
            if (conn.IsConnect())
            {
                string cmdText = " UPDATE employee SET Employee_Availability = '0' , End_Work_Date = '" + DateTime.Now.Date.ToString("yyyy/MM/dd") + "' WHERE `ID_Employee`= " + id;
                MySqlCommand cmd = conn.Connection.CreateCommand();
                cmd.CommandText = cmdText;
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            return false;
        }
        private string EmployeeSearchText(int EmployeeComboBox, int EmployeeSearchComboBox, string EmployeeSearchTxt)
        {
            string cmd = "SELECT ID,First_Name,Last_Name,Phone_Number,Address,Telephone_Number,Salary,Employye_Type,Employee_Availability,Birthdate,Start_Work_Date,End_Work_Date From employee JOIN user ON employee.ID_Employee = user.ID where ID != '0000000000' AND ";
            if (EmployeeComboBox == 1)
                cmd = "SELECT ID,First_Name,Last_Name,Phone_Number,Address,Telephone_Number,Salary,Employye_Type,Employee_Availability,Birthdate,Start_Work_Date,End_Work_Date From employee JOIN user ON employee.ID_Employee = user.ID where Employee_Availability=1 AND ID != '0000000000' AND ";
            else if (EmployeeComboBox == 2)
                cmd = "SELECT ID,First_Name,Last_Name,Phone_Number,Address,Telephone_Number,Salary,Employye_Type,Employee_Availability,Birthdate,Start_Work_Date,End_Work_Date From employee JOIN user ON employee.ID_Employee = user.ID where Employee_Availability=0 AND ID != '0000000000' AND ";
            if (EmployeeSearchComboBox == 0 && EmployeeSearchTxt.IsDig())
                /*ID*/
                return cmd + " ID LIKE '%" + int.Parse(EmployeeSearchTxt) + "%' ORDER BY `ID` DESC";
            else if (EmployeeSearchComboBox == 1 && EmployeeSearchTxt.IsHeOrEn())
                /*שם פרטי*/
                return cmd + " First_Name LIKE '%" + EmployeeSearchTxt + "%' ORDER BY `ID` DESC";
            else if (EmployeeSearchComboBox == 2 && EmployeeSearchTxt.IsHeOrEn())
                /*שם משפחה*/
                return cmd + " Last_Name LIKE '%" + EmployeeSearchTxt + "%' ORDER BY `ID` DESC";
            else if (EmployeeSearchComboBox == 3 && EmployeeSearchTxt.IsDig())
                /*מס' פלאפון*/
                return cmd + " Phone_Number LIKE '%" + int.Parse(EmployeeSearchTxt) + "%' ORDER BY `ID` DESC";
            else if (EmployeeSearchComboBox == 4 && EmployeeSearchTxt.IsDig())
                /*מס' טלפון*/
                return cmd + " Telephone_Number LIKE '%" + int.Parse(EmployeeSearchTxt) + "%' ORDER BY `ID` DESC";
            else if (EmployeeSearchComboBox == 5)
                /*כתובת*/
                return cmd + " Address LIKE '%" + EmployeeSearchTxt + "%' ORDER BY `ID` DESC";
            else if (EmployeeSearchComboBox == 6 && EmployeeSearchTxt.IsDigFloat())
                /*משכורת*/
                return cmd + " Salary LIKE '%" + EmployeeSearchTxt + "%' ORDER BY `ID` DESC";
            else if (EmployeeSearchComboBox == 7 && EmployeeSearchTxt.IsHeOrEn())
                return cmd + " Employye_Type LIKE '%" + EmployeeSearchTxt + "%' ORDER BY `ID` DESC";
            else
                return EmployeeSearchDate(EmployeeComboBox, EmployeeSearchComboBox, EmployeeSearchTxt);
        }
        private string EmployeeSearchDate(int EmployeeComboBox, int EmployeeSearchComboBox, string EmployeeDateTimeSearch)
        {
            string cmd = "SELECT ID,First_Name,Last_Name,Phone_Number,Address,Telephone_Number,Salary,Employye_Type,Employee_Availability,Birthdate,Start_Work_Date,End_Work_Date From employee JOIN user ON employee.ID_Employee = user.ID where ID != '0000000000' AND ";
            if (EmployeeComboBox == 1)
                cmd = "SELECT ID,First_Name,Last_Name,Phone_Number,Address,Telephone_Number,Salary,Employye_Type,Employee_Availability,Birthdate,Start_Work_Date,End_Work_Date From employee JOIN user ON employee.ID_Employee = user.ID where Employee_Availability=1 AND ID != '0000000000' AND ";
            else if (EmployeeComboBox == 2)
                cmd = "SELECT ID,First_Name,Last_Name,Phone_Number,Address,Telephone_Number,Salary,Employye_Type,Employee_Availability,Birthdate,Start_Work_Date,End_Work_Date From employee JOIN user ON employee.ID_Employee = user.ID where Employee_Availability=0 AND ID != '0000000000' AND ";
            if (EmployeeSearchComboBox == 8)
                return cmd + " Birthdate  = '" + EmployeeDateTimeSearch + "' ORDER BY `ID` DESC";
            else if (EmployeeSearchComboBox == 9)
                return cmd + " Start_Work_Date  = '" + EmployeeDateTimeSearch + "' ORDER BY `ID` DESC";
            else if (EmployeeSearchComboBox == 10)
                return cmd + " End_Work_Date  = '" + EmployeeDateTimeSearch + "'" + (EmployeeComboBox == 2 ? " ORDER BY `Rental_ID` DESC" : " ");
            return null;
        }
        public DataSet SearchEmployeeTxt(int EmployeeComboBox, int EmployeeSearchComboBox, string EmployeeSearchTxt)
        {
            string cmd = EmployeeSearchText(EmployeeComboBox, EmployeeSearchComboBox, EmployeeSearchTxt);
            if (cmd == string.Empty)
                return null;
            try
            {
                if (conn.IsConnect())
                {
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd, conn.Connection);
                    DataSet DS = new DataSet();
                    adapter.Fill(DS, "employee");
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
        public DataSet EmployeeAvaibilityTrue()
        {
            try
            {
                if (conn.IsConnect())
                {
                    MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT First_Name,Last_Name,ID,Employye_Type,Phone_Number,Address,Telephone_Number,Salary,Employee_Availability,Birthdate,Start_Work_Date,End_Work_Date From employee JOIN user ON user.ID = employee.ID_Employee where ID != '0000000000' AND Employee_Availability=1", conn.Connection);
                    DataSet DS = new DataSet();
                    adapter.Fill(DS, "employee");
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
        public DataSet EmployeeAvaibilityFalse()
        {
            try
            {
                if (conn.IsConnect())
                {
                    MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT First_Name,Last_Name,ID,Employye_Type,Phone_Number,Address,Telephone_Number,Salary,Employee_Availability,Birthdate,Start_Work_Date,End_Work_Date From employee JOIN user ON user.ID = employee.ID_Employee where ID != '0000000000' AND Employee_Availability=0", conn.Connection);
                    DataSet DS = new DataSet();
                    adapter.Fill(DS, "employee");
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
        /*פונקציה לעדכן מספר רכב*/
        public bool UpdateCarNumRent(string carNum, string newCarNum)/*for updating a car we should give a num of a admin car to cahnge and replace*/
        {
            if (conn.IsConnect())
            {
                string cmdText = " UPDATE rent_details SET Car_Number = '" + newCarNum + "' WHERE `Car_Number`= '" + carNum + "'";
                MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                cmd = conn.Connection.CreateCommand();
                cmd.CommandText = cmdText;
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            return false;
        }
        /*פונקציה לעדכן רכב*/
        public bool UpdateOldCar(Car c,bool active, DateTime add_date, DateTime remove_Date, string carNum)
        {
            if ((c.Car_Number != carNum && !CheckCarNum(c.Car_Number)) || c.Car_Number == carNum)
            {
                if (UpdateCarNumRent(carNum, "0000000") && conn.IsConnect())
                {
                    string deleteQuery = "DELETE FROM car WHERE Car_Number =" + carNum;
                    MySqlCommand cmd = new MySqlCommand(deleteQuery, conn.Connection);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    /* if (c.Car_Availability == false && c.Car_Active == true && AddNewCar(c) && UpdateCarNumRent("0000000", c.Car_Number))
                         return true;*/
                    if (!active && c.Car_Active)
                        c.Car_Availability = true;
                    if (AddNewCar(c) && UpdateCarNumRent("0000000", c.Car_Number) && conn.IsConnect())
                    {
                        string cmdText = " UPDATE car SET Car_Date_Added = '" + add_date.Date.ToString("yyyy/MM/dd") + "', Car_Date_Deleted = '" + remove_Date.Date.ToString("yyyy/MM/dd") + "', Car_Active=0, Car_Availability=0 WHERE `Car_Number`= " + c.Car_Number;
                        if (c.Car_Active)
                            cmdText = " UPDATE car SET Car_Date_Added = '" + add_date.Date.ToString("yyyy/MM/dd") + "', Car_Availability= "+c.Car_Availability +" WHERE `Car_Number`= " + c.Car_Number;
                        cmd = conn.Connection.CreateCommand();
                        cmd.CommandText = cmdText;
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        return true;
                    }
                    conn.Close();
                }
            }
            return false;
        }
        /*פונקציה להוסיף רכב*/
        public bool AddNewCar(Car car)
        {
            if (!CheckCarNum(car.Car_Number) && conn.IsConnect())
            {
                string InsertText = "INSERT INTO car(Car_Number, Car_Manufacturer, Car_Model, Engine_Capacity, Licensing_Expire_Date, Insurance_Expire_Date,Doors,Seats,Gearbox_Type,Color,Distance,Fuel_Type,Fuel_Capacity,Price_For_Day,Car_Availability,Year_Production,Car_Active,Car_Date_Added,Car_Status)";
                string cmdText = InsertText + " VALUES(@Car_Number,@Car_Manufacturer,@Car_Model,@Engine_Capacity,@Licensing_Expire_Date,@Insurance_Expire_Date,@Doors,@Seats,@Gearbox_Type,@Color,@Distance,@Fuel_Type,@Fuel_Capacity,@Price_For_Day,@Car_Availability,@Year_Production,@Car_Active,@Car_Date_Added,@Car_Status)";
                MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                cmd.Parameters.AddWithValue("@Car_Number", car.Car_Number);
                cmd.Parameters.AddWithValue("@Car_Manufacturer", car.Car_Manufacturer);
                cmd.Parameters.AddWithValue("@Car_Model", car.Car_Model);
                cmd.Parameters.AddWithValue("@Engine_Capacity", car.Engine_Capacity);
                cmd.Parameters.AddWithValue("@Licensing_Expire_Date", car.Licensing_Expire_Date.Date);
                cmd.Parameters.AddWithValue("@Insurance_Expire_Date", car.Insurance_Expire_Date.Date);
                cmd.Parameters.AddWithValue("@Doors", car.Doors);
                cmd.Parameters.AddWithValue("@Seats", car.Seats);
                cmd.Parameters.AddWithValue("@Gearbox_Type", car.Gearbox_Type);
                cmd.Parameters.AddWithValue("@Color", car.Color_Car);
                cmd.Parameters.AddWithValue("@Distance", car.Distance);
                cmd.Parameters.AddWithValue("@Fuel_Type", car.Fuel_Type);
                cmd.Parameters.AddWithValue("@Fuel_Capacity", car.Fuel_Capacity);
                cmd.Parameters.AddWithValue("@Price_For_Day", car.Price_For_Day);
                cmd.Parameters.AddWithValue("@Car_Availability", 1);
                cmd.Parameters.AddWithValue("@Year_Production", car.Production_Year);
                cmd.Parameters.AddWithValue("@Car_Active", 1);
                cmd.Parameters.AddWithValue("@Car_Date_Added", DateTime.Now.Date);
                cmd.Parameters.AddWithValue("@Car_Status", car.Car_Status);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            return false;
        }
        /*פונקציה להוסיף הודעה לגבי רכב*/
        public bool AddCarComment(Car car, string Employee_ID, int LicensingORInsurance)
        {
            if (conn.IsConnect())
            {
                string InsertText = "INSERT INTO messages(Employee_ID,Car_Number,Message_For,Message_Text, Message_Type, Message_Availability, Message_ToDate, Message_Title)";
                string cmdText = InsertText + " VALUES(@Employee_ID,@Car_Number,@Message_For,@Message_Text,@Message_Type,@Message_Availability,@Message_ToDate,@Message_Title)";
                MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                cmd.Parameters.AddWithValue("@Employee_ID", Employee_ID);
                cmd.Parameters.AddWithValue("@Car_Number", car.Car_Number);
                cmd.Parameters.AddWithValue("@Message_For", "לכל העובדים");
                cmd.Parameters.AddWithValue("@Message_Text", "רכב של מספר " + car.Car_Number + " " + (LicensingORInsurance == 1 ? "תאריך תפוגה של ביטוח " + car.Insurance_Expire_Date.ToString("dd/MM/yyyy") : "תאריך תפוגה של רישיון " + car.Licensing_Expire_Date.ToString("dd/MM/yyyy")));
                cmd.Parameters.AddWithValue("@Message_Type", "רכב");
                cmd.Parameters.AddWithValue("@Message_Availability", 1);
                cmd.Parameters.AddWithValue("@Message_ToDate", LicensingORInsurance == 1 ? car.Insurance_Expire_Date.AddMonths(-1).ToString("yyyy-MM-dd H:mm:ss") : car.Licensing_Expire_Date.AddMonths(-1).ToString("yyyy-MM-dd H:mm:ss"));
                cmd.Parameters.AddWithValue("@Message_Title", LicensingORInsurance == 1 ? "תאריך תפוגה של ביטוח" : "תאריך תפוגה של רישיון");
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            return false;
        }
        /*פונקציה לעדכן את ההודעה של  הרכב*/
        public bool UpdateCarComment1(Car car, string Employee_ID, string oldCarNum, string carActive)
        {
            if (conn.IsConnect())
            {
                string updateText = "UPDATE   messages Set Car_Number='" + car.Car_Number + "'"
                  + ", Employee_ID='"+Employee_ID+"'"
                 + ", Message_Text=' רכב של מספר " + car.Car_Number + " " + "תאריך תפוגה של ביטוח " + car.Insurance_Expire_Date.ToString("dd/MM/yyyy") +"'"
                 + ", Message_ToDate='" + car.Insurance_Expire_Date.AddMonths(-1).ToString("yyyy-MM-dd H:mm:ss") + "'"
                 + ", Message_Availability='" + (carActive == "פעיל" ? 1 : 0) + "'"
                 + " WHERE `Car_Number`= " + oldCarNum + " and `Message_Title`='תאריך תפוגה של ביטוח'";
                MySqlCommand cmd = new MySqlCommand(updateText, conn.Connection);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            return false;
        }
        /*פונקציה לעדכן את ההודעה של  הרכב*/
        public bool UpdateCarComment2(Car car, string Employee_ID, string oldCarNum, string carActive)
        {
            if (conn.IsConnect())
            {
                string updateText = "UPDATE   messages Set Car_Number='" + car.Car_Number + "'"
                +", Employee_ID='" + Employee_ID + "'"
                + ", Message_Text=' רכב של מספר " + car.Car_Number + " " + "תאריך תפוגה של רישיון " + car.Licensing_Expire_Date.ToString("dd/MM/yyyy") + "'"
                + ", Message_ToDate='" + car.Licensing_Expire_Date.AddMonths(-1).ToString("yyyy-MM-dd H:mm:ss") + "'"
                + ", 	Message_Availability='" + (carActive == "פעיל" ? 1 : 0) + "'"
                + " WHERE `Car_Number`= " + oldCarNum + " and `Message_Title`='תאריך תפוגה של רישיון'";
                MySqlCommand cmd = new MySqlCommand(updateText, conn.Connection);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            return false;
        }
        /*פונקציה למחוק הועדה של רכב מוסיים*/
        public bool DeleteCarComment(string carNum)
        {
            if (conn.IsConnect())
            {
                string updateText = "UPDATE  messages Set Message_Availability='" + 0 + "' WHERE `Car_Number`= " + carNum;
                MySqlCommand cmd = new MySqlCommand(updateText, conn.Connection);
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            return false;
        }
        /*פונקציה למחוק רכב*/
        public bool RemoveCar(string carNum) 
        {
            if (conn.IsConnect())
            {
                string cmdText = " UPDATE car SET Car_Active = '0',Car_Availability= '0' , Car_Date_Deleted = '" + DateTime.Now.Date.ToString("yyyy/MM/dd") + "' WHERE `Car_Number`= " + carNum;
                MySqlCommand cmd = conn.Connection.CreateCommand();
                cmd.CommandText = cmdText;
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            return false;
        }
    }
}
        /*public DataSet carChart(DateTime From,DateTime To)
        {
            try
            {
                if (conn.IsConnect())
                {
                    /*JOIN user ON user.ID = employee.ID_Employee"*/
        //MySqlConnection connection=new MySqlConnection("Server='localhost'; database='rentcar'; UID='user1DB_user'; password='12345678'; CharSet=utf8;");
        /* MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT Payment From rent_details JOIN car ON car.Car_Number = rent_details.Car_Number where `Rental_ID` != '0000000000' And Rent_Date >= '" + From.Date.ToString("yyyy/MM/dd 00:00:00") +"' And Rent_Date <= '"+To.Date.ToString("yyyy/MM/dd 23:59:00")+"'", conn.Connection);
         DataSet DS = new DataSet(); 
         adapter.Fill(DS, "rent_details");
         conn.Close();
         return DS;
     }
     return null;
 }
 catch
 {
     return null;
 }
}*/


        ///////////////////////Rent//////////////////////


        /*  public Employee[] AllEmployees()
          {
              ArrayList employee = new ArrayList();
              string cmdText = "SELECT * From employee JOIN user ON user.ID = employee.ID_Employee";
              MySqlCommand cmd = conn.Connection.CreateCommand();
              cmd.CommandText = cmdText;
              MySqlDataReader reader = cmd.ExecuteReader();
              DataTable t = new DataTable();
              if (reader != null)
              {
                  t = reader.GetSchemaTable();
                  foreach (DataRow tTyp in t.Rows)
                      //employee.Add(new Employee());
                      Console.WriteLine(tTyp[1].ToString());
                  return (Employee[])employee.ToArray(typeof(Employee));
              }
              return null;
          }*/
        /*  public bool AddUser(User u)
          {

          }*/
        /* public void updateEmployee()
         {
             string cmdText = " UPDATE employee SET Employye_Type = 'מזכירות',`Start_Work_Date`= '2019-01-20' WHERE `ID_Employee`= '12345678' and `Password`= '12345678'";
             MySqlCommand cmd = conn.Connection.CreateCommand();
             cmd.CommandText = cmdText;
             cmd.ExecuteNonQuery();
         }*/

