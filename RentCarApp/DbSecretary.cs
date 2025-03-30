using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;

namespace RentCarApp
{
    class DbSecretary : DBConnection
    {
        private DBConnection conn;
        private string database = "rentcar";
        private string hash = "f0xle@rn";
        public DbSecretary() : base()
        {
            //database = "rentcar";
            conn = new DBConnection();
            conn.DatabaseName = database;
        }

        /*חיפוש להערות לפי תאריך*/
        public DataSet SearchReminder(DateTime commentDate, string employeeID)
        {
            try
            {
                if (conn.IsConnect())
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT `Message_ID`,`Message_ToDate`,`Message_Text` FROM `messages` WHERE (`Message_ToDate` LIKE '%" + commentDate.Date.ToString("yyyy-MM-dd") + "%' OR `Message_Type`='" + "רכב" + "' AND DATE('" + commentDate.Date.ToString("yyyy-MM-dd") + "')>DATE(`Message_ToDate`)) and `Message_Availability`=1 and (`Message_For`='" + "לכל העובדים" + "' OR `Message_For`='" + "אישית" + "' and `Employee_ID`=" + employeeID + ")", conn.Connection))
                    {
                        DataSet DS = new DataSet();
                        adapter.Fill(DS, "messages");
                        conn.Close();
                        return DS;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        public DataSet SearchCustomerInfo(string IDCustomer)
        {
            try
            {
                if (conn.IsConnect())
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT `Rental_ID` AS 'מספר שכירות',rent_details.Car_Number AS 'מספר רכב',CONCAT(car.Car_Manufacturer,' ',car.Car_Model) AS 'רכב',`Rent_Date` AS 'תאריך שכירות',`Returned_Date` AS 'תאריך החזרה' FROM `rent_details` JOIN `car` ON rent_details.Car_Number=car.Car_Number WHERE `ID_Customer`='" + IDCustomer + "'", conn.Connection))
                    {
                        DataSet DS = new DataSet();
                        adapter.Fill(DS, "customerInfo");
                        conn.Close();
                        return DS;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        public DataSet SearchSecondCustomerInfo(string IDCustomer)
        {
            try
            {
                if (conn.IsConnect())
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT rent_details.Rental_ID AS 'מספר שכירות',rent_details.Car_Number AS 'מספר רכב',CONCAT(car.Car_Manufacturer,' ',car.Car_Model) AS 'רכב',`Rent_Date` AS 'תאריך שכירות',`Returned_Date` AS 'תאריך החזרה' FROM `rent_details` JOIN `car` ON rent_details.Car_Number=car.Car_Number JOIN `drivers` ON rent_details.Rental_ID=drivers.Rental_ID WHERE `ID_Driver`='" + IDCustomer + "' AND rent_details.ID_Customer!='" + IDCustomer + "'", conn.Connection))
                    {
                        DataSet DS = new DataSet();
                        adapter.Fill(DS, "customerInfo");
                        conn.Close();
                        return DS;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        /*פונקציה שמחזירה את כל הרכים שבהשכרה*/
        public DataSet AllCarInRent()
        {
            try
            {
                if (conn.IsConnect())
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT* From car where Car_Availability='0' and Car_Active='1'", conn.Connection))
                    {
                        DataSet DS = new DataSet();
                        adapter.Fill(DS, "car");
                        conn.Close();
                        return DS;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        /*פונקציה שמחזירה את כל הרכים שאפשר להשכיר*/
        public DataSet AllCar_toRent()//for auto complete car num
        {
            try
            {
                if (conn.IsConnect())
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT* From car where Car_Availability='1' and Car_Active='1'", conn.Connection))
                    {
                        DataSet DS = new DataSet();
                        adapter.Fill(DS, "car");
                        conn.Close();
                        return DS;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        /*פונקציה שמחזירה את כל הרכבים*/
        public DataSet AllCar()
        {
            try
            {
                if (conn.IsConnect())
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT* From car where Car_Number != '0000000' ", conn.Connection))
                    {
                        DataSet DS = new DataSet();
                        adapter.Fill(DS, "car");
                        conn.Close();
                        return DS;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }/*
        public DataSet CarDetail(string carNum)
        {
            try
            {
                if (conn.IsConnect())
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT* From car where Car_Number = '" + carNum + "' ", conn.Connection))
                    {
                        DataSet DS = new DataSet();
                        adapter.Fill(DS, "car");
                        conn.Close();
                        return DS;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }*/
        public bool Backup()
        {
            try
            {
                if (conn.IsConnect())
                {
                    string file = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\RentCarAppLast desktop\\temp\\backup.sql";

                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        using (MySqlBackup mb = new MySqlBackup(cmd))
                        {
                            cmd.Connection = conn.Connection;
                            mb.ExportToFile(file);
                            conn.Close();
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
        private void Restore()
        {
            string constring = "server=localhost;user=root;pwd=qwerty;database=test;";
            string file = "C:\\backup.sql";
            using (MySqlConnection conn = new MySqlConnection(constring))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = conn;
                        conn.Open();
                        mb.ImportFromFile(file);
                        conn.Close();
                    }
                }
            }
        }

        /*פונקציה מחזירה הרכבים לפי הטקסט שמחפשים*/
        public DataSet SearchCarTextBox(int index, int carAvailable, string searchTxt)
        {
            string cmd = checkCarSearchTB(index, carAvailable, searchTxt);
            if (cmd == string.Empty)
                return null;
            try
            {
                if (conn.IsConnect())
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd, conn.Connection))
                    {
                        DataSet DS = new DataSet();
                        adapter.Fill(DS, "car");
                        conn.Close();
                        return DS;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        /*פונקציה מחזירה שאילתא מתאימה  לחיפוש בבסיס הנתונים*/
        private string checkCarSearchTB(int index, int carAvailable, string searchTxt)
        {
            string cmd = "SELECT* From car where " + (carAvailable == 1 ? " Car_Availability = '1' and Car_Active = '1' and " : ((carAvailable == 2 ? " Car_Availability = '0' and Car_Active = '1' and " : " Car_Number != '0000000' and ")));
            if (index == 0 && searchTxt.IsDig())
                /*מס' רכב*/
                return cmd + " Car_Number LIKE '%" + int.Parse(searchTxt) + "%'";
            else if (index == 1 && searchTxt.IsHeOrEn())
                /*יצרן*/
                return cmd + " Car_Manufacturer LIKE '%" + searchTxt + "%'";
            else if (index == 2)
                /*דגם*/
                return cmd + " Car_Model LIKE '%" + searchTxt + "%'";
            else if (index == 3 && searchTxt.IsDig())
                /*מנוע*/
                return cmd + " Engine_Capacity LIKE '%" + int.Parse(searchTxt) + "%'";
            else if (index == 6 && searchTxt.IsDig())
                /*מס' דלתות*/
                return cmd + " Doors LIKE '%" + int.Parse(searchTxt) + "%'";
            else if (index == 7 && searchTxt.IsHeOrEn())
                /*גיר*/
                return cmd + " Gearbox_Type LIKE '%" + searchTxt + "%'";
            else if (index == 8 && searchTxt.IsHeOrEn())
                /*צבע*/
                return cmd + " Color LIKE '%" + searchTxt + "%'";
            else if (index == 9 && searchTxt.IsDig())
                /*תשלום ליום*/
                return cmd + " Price_For_Day LIKE '%" + int.Parse(searchTxt) + "%'";
            else if (index == 10 && searchTxt.IsDig())
                /*שנת יצור*/
                return cmd + " Year_Production LIKE '%" + int.Parse(searchTxt) + "%'";
            else
                return string.Empty;
        }
        /*פונקציה מחזירה הרכבים לפי התאריך שמחפשים*/
        public DataSet SearchCarDate(int index, int carAvailable, DateTime searchDate)
        {
            string cmd = checkCarSearchDate(index, carAvailable, searchDate);
            if (cmd == string.Empty)
                return null;
            try
            {
                if (conn.IsConnect())
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd, conn.Connection))
                    {
                        DataSet DS = new DataSet();
                        adapter.Fill(DS, "car");
                        conn.Close();
                        return DS;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        /*פונקציה מחזירה שאילתא מתאימה לחיפוש בבסיס הנתונים*/
        private string checkCarSearchDate(int index, int carAvailable, DateTime searchDate)
        {
            string cmd = "SELECT* From car where " + (carAvailable == 1 ? " Car_Availability = '1' and Car_Active = '1' and " : ((carAvailable == 2 ? " Car_Availability = '0' and Car_Active = '1' and " : " ")));
            if (index == 4)
                /*תוקף רשיון רכב*/
                return cmd + " Licensing_Expire_Date = '" + searchDate.Date.ToString("yyyy/MM/dd") + "' and Car_Number != '0000000'";
            else if (index == 5)
                /*תוקף ביטוח הרכב*/
                return cmd + " Insurance_Expire_Date = '" + searchDate.Date.ToString("yyyy/MM/dd") + "' and Car_Number != '0000000'";
            else if (index == 11)
                /*תאריך הוספה*/
                return cmd + " Car_Date_Added = '" + searchDate.Date.ToString("yyyy/MM/dd") + "' and Car_Number != '0000000'";
            else if (index == 12)
                /*תאריך הפסקת פעילות*/
                return cmd + " Car_Date_Deleted = '" + searchDate.Date.ToString("yyyy/MM/dd") + "' and Car_Number != '0000000'";
            else
                return string.Empty;
        }
        /*פונקציה מחזירה הרכבים שפעילות או לא פעיעלות שמחפשים*/
        public DataSet SearchCarComboBox(int index, int carAvailable)
        {
            string cmd = CheckCarSearchComboBox(index, carAvailable);
            if (cmd == string.Empty)
                return null;
            try
            {
                if (conn.IsConnect())
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd, conn.Connection))
                    {
                        DataSet DS = new DataSet();
                        adapter.Fill(DS, "car");
                        conn.Close();
                        return DS;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        /*פונקציה מחזירה שאילתא מתאימה  לחיפוש בבסיס הנתונים*/
        private string CheckCarSearchComboBox(int index, int carAvailable)
        {
            if (index == 0)
                return "SELECT* From car where " + (carAvailable == 1 ? " Car_Availability = '1' and Car_Active = '1' and " : ((carAvailable == 2 ? " Car_Availability = '0' and Car_Active = '1' " : " Car_Active = '1'")));
            if (index == 1)
                return "SELECT* From car where Car_Active = '0' and Car_Number != '0000000'";
            else
                return string.Empty;
        }
        /*פונקציה שמחזירה את כל השכירות*/
        public DataSet AllRent()
        {
            try
            {
                if (conn.IsConnect())
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT Rental_ID , ID_Customer,First_Name,Last_Name,Phone_Number,Telephone_Number , rent_details.Car_Number,Car_Manufacturer,Car_Model,Payment,Payment_Type ,rent_details.Price_For_Day, ID_Employee_Rent,ID_Employee_Returned,Rent_Days,Rent_Date,Returned_Date ,KM_For_Day, Rent_Distance,Return_Distance,Quantity_Fuel_Rent,Quantity_Fuel_Returned,Payed From rent_details JOIN user ON user.ID = rent_details.ID_Customer JOIN car ON car.Car_Number = rent_details.Car_Number ORDER BY rent_details.Rental_ID DESC;", conn.Connection))
                    {
                        DataSet DS = new DataSet();
                        adapter.Fill(DS, "rent_details");
                        conn.Close();
                        return DS;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        /*פונקציה שמחזירה את כל השכירות שהם עדין בתקופת השכירות*/
        public DataSet ActiveRent()
        {
            try
            {
                if (conn.IsConnect())
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT Rental_ID , ID_Customer,First_Name,Last_Name,Phone_Number,Telephone_Number , rent_details.Car_Number,Car_Manufacturer,Car_Model,Payment,Payment_Type ,rent_details.Price_For_Day, ID_Employee_Rent,ID_Employee_Returned,Rent_Days,Rent_Date,Returned_Date ,KM_For_Day, Rent_Distance,Return_Distance,Quantity_Fuel_Rent,Quantity_Fuel_Returned,Payed From rent_details JOIN user ON user.ID = rent_details.ID_Customer JOIN car ON car.Car_Number = rent_details.Car_Number where `ID_Employee_Returned` = '' ORDER BY rent_details.Rental_ID ASC;", conn.Connection))//Returned_Date > '" + DateTime.Now.Date.ToString("yyyy-MM-dd H:mm:ss") + "'"
                    {
                        DataSet DS = new DataSet();
                        adapter.Fill(DS, "rent_details");
                        conn.Close();
                        return DS;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        /*פונקציה שמחזירה את כל השכירות שהם עדין בתקופת השכירות*/
        public DataSet EndedRent()
        {
            try
            {
                if (conn.IsConnect())
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT Rental_ID , ID_Customer,First_Name,Last_Name,Phone_Number,Telephone_Number , rent_details.Car_Number,Car_Manufacturer,Car_Model,Payment,Payment_Type ,rent_details.Price_For_Day, ID_Employee_Rent,ID_Employee_Returned,Rent_Days,Rent_Date,Returned_Date ,KM_For_Day, Rent_Distance,Return_Distance,Quantity_Fuel_Rent,Quantity_Fuel_Returned,Payed From rent_details JOIN user ON user.ID = rent_details.ID_Customer JOIN car ON car.Car_Number = rent_details.Car_Number where `ID_Employee_Returned` !='' ORDER BY rent_details.Rental_ID DESC;", conn.Connection))// Returned_Date <= '" + DateTime.Now.ToString("yyyy-MM-dd H:mm:ss") + "'"
                    {
                        DataSet DS = new DataSet();
                        adapter.Fill(DS, "rent_details");
                        conn.Close();
                        return DS;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        private enum RentSearch
        {
            Rental_ID = 0,
            ID_Customer,
            First_Name,
            Last_Name,
            Phone_Number,
            Telephone_Number,
            Car_Number,
            Car_Manufacturer,
            Car_Model,
            Payment,
            Payment_Status,
            Price_For_Day,
            ID_Employee_Rent,
            ID_Employee_Returned,
            Rent_Days,
            Rent_Date,
            Returned_Date,
            KM_For_Day,
            Rent_Distance,
            Return_Distance
        }
        ///RentSearch rentsearch;

        /*פונקציה מחזירה השכירות לפי הטקסט שמחפשים*/
        public DataSet SearchRentTextBox(int index, int rentStatus, string searchTxt)
        {
            string cmd = checkRentSearchTB(index, rentStatus, searchTxt);
            if (cmd == string.Empty)
                return null;
            try
            {
                if (conn.IsConnect())
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd, conn.Connection)) {
                        DataSet DS = new DataSet();
                        adapter.Fill(DS, "rent_details");
                        conn.Close();
                        return DS;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        private string checkRentSearchTB(int index, int rentStatus, string searchTxt)
        {
            string cmd = "SELECT Rental_ID, ID_Customer, First_Name, Last_Name, Phone_Number, Telephone_Number, rent_details.Car_Number,Car_Manufacturer,Car_Model,Payment,Payment_Type ,rent_details.Price_For_Day, ID_Employee_Rent,ID_Employee_Returned,Rent_Days,Rent_Date,Returned_Date ,KM_For_Day, Rent_Distance,Return_Distance,Quantity_Fuel_Rent,Quantity_Fuel_Returned,Payed From rent_details JOIN user ON user.ID = rent_details.ID_Customer JOIN car ON car.Car_Number = rent_details.Car_Number where "
                + (rentStatus == 1 ? "`ID_Employee_Returned` = ''" /*" Returned_Date > '" + DateTime.Now.Date.ToString("yyyy-MM-dd H:mm:ss") */+ " and "
                : (rentStatus == 2 ? "`ID_Employee_Returned` != ''" /*" Returned_Date <= '" + DateTime.Now.Date.ToString("yyyy-MM-dd H:mm:ss") */+ " and " : " "));
            if (index == (int)RentSearch.Rental_ID && searchTxt.IsDig())
                /*מס' ההשכרה*/
                return cmd + " Rental_ID LIKE '%" + searchTxt + "%'";//int.Parse(searchTxt)
            else if (index == (int)RentSearch.ID_Customer && searchTxt.IsDig())
                /*ת"ז הלקוח*/
                return cmd + " ID_Customer LIKE '%" + searchTxt + "%'";
            else if (index == (int)RentSearch.First_Name && searchTxt.IsHeOrEn())
                /*שם פרטי*/
                return cmd + " First_Name LIKE '%" + searchTxt + "%'";
            else if (index == (int)RentSearch.Last_Name && searchTxt.IsHeOrEn())
                /*שם משפחה*/
                return cmd + " Last_Name LIKE '%" + searchTxt + "%'";
            else if (index == (int)RentSearch.Phone_Number && searchTxt.IsDig())
                /*מס' פלאפון*/
                return cmd + " Phone_Number LIKE '%" + searchTxt + "%'";
            else if (index == (int)RentSearch.Telephone_Number && searchTxt.IsDig())
                /*מס' טלפון*/
                return cmd + " Telephone_Number LIKE '%" + searchTxt + "%'";
            else if (index == (int)RentSearch.Car_Number && searchTxt.IsDig())
                /*מס' רכב*/
                return cmd + " rent_details.Car_Number LIKE '%" + searchTxt + "%'";
            else if (index == (int)RentSearch.Car_Manufacturer && searchTxt.IsHeOrEn())
                /*יצרן הרכב*/
                return cmd + " Car_Manufacturer LIKE '%" + searchTxt + "%'";
            else if (index == (int)RentSearch.Car_Model)
                /*דגם הרכב*/
                return cmd + " Car_Model LIKE '%" + searchTxt + "%'";
            else if (index == (int)RentSearch.Payment && searchTxt.IsDig())
                /*תשלום*/
                return cmd + " Payment  LIKE '%" + searchTxt + "%'";
            else if (index == (int)RentSearch.Payment_Status && searchTxt.IsHeOrEn())
                /* מצב תשלום*/
                return cmd + " Payment_Type  = '" + searchTxt + "'";
            else if (index == (int)RentSearch.Price_For_Day && searchTxt.IsDig())
                /*תשלום ליום*/
                return cmd + " rent_details.Price_For_Day LIKE '%" + searchTxt + "%'";
            else if (index == (int)RentSearch.ID_Employee_Rent && searchTxt.IsDig())
                /*ת"ז העובד בהשכרה*/
                return cmd + " ID_Employee_Rent LIKE '%" + searchTxt + "%'";
            else if (index == (int)RentSearch.ID_Employee_Returned && searchTxt.IsDig() && rentStatus != 1)
                /*ת"ז עובד החזרת הרכב*/
                return cmd + " ID_Employee_Returned LIKE '%" + searchTxt + "%'";
            else if (index == (int)RentSearch.Rent_Days && searchTxt.IsDig())
                /*סכ"ה ימי השכרה*/
                return cmd + " Rent_Days LIKE '%" + searchTxt + "%'";
            else if (index == (int)RentSearch.KM_For_Day && searchTxt.IsDig())
                /*הגבלת ק"ם ליום*/
                return cmd + " KM_For_Day LIKE '%" + int.Parse(searchTxt) + "%'";
            else if (index == (int)RentSearch.Rent_Distance && searchTxt.IsDig())
                /*ק"ם בהשכרה*/
                return cmd + " Rent_Distance LIKE '%" + int.Parse(searchTxt) + "%'";
            else if (index == (int)RentSearch.Return_Distance && searchTxt.IsDig())
                /*ק"ם בהחזרה*/
                return cmd + " Return_Distance LIKE '%" + int.Parse(searchTxt) + "%'";
            else
                return string.Empty;
        }
        /*פונקציה מחזירה השכירות לפי התאריך שמחפשים*/

        public DataSet SearchRentDate(int index, int rentStatus, DateTime searchDate)
        {
            string cmd = CheckRentSearchDate(index, rentStatus, searchDate);
            if (cmd == string.Empty)
                return null;
            try
            {
                if (conn.IsConnect())
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd, conn.Connection))
                    {
                        DataSet DS = new DataSet();
                        adapter.Fill(DS, "rent_details");
                        conn.Close();
                        return DS;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        /*פונקציה מחזירה שאילתא מתאימה לחיפוש בבסיס הנתונים*/
        private string CheckRentSearchDate(int index, int rentStatus, DateTime searchDate)
        {
            string cmd = "SELECT Rental_ID , ID_Customer,First_Name,Last_Name,Phone_Number,Telephone_Number , rent_details.Car_Number,Car_Manufacturer,Car_Model,Payment,Payment_Type ,rent_details.Price_For_Day, ID_Employee_Rent,ID_Employee_Returned,Rent_Days,Rent_Date,Returned_Date ,KM_For_Day, Rent_Distance,Return_Distance,Quantity_Fuel_Rent,Quantity_Fuel_Returned,Payed From rent_details JOIN user ON user.ID = rent_details.ID_Customer JOIN car ON car.Car_Number = rent_details.Car_Number where "
                + (rentStatus == 1 ? "`ID_Employee_Returned` = ''" /*" Returned_Date > '" + DateTime.Now.Date.ToString("yyyy-MM-dd H:mm:ss")*/+ " and "
                : (rentStatus == 2 ? "`ID_Employee_Returned` != ''" /*" Returned_Date <= '" + DateTime.Now.Date.ToString("yyyy-MM-dd H:mm:ss")*/ + " and " : " "));
            if (index == (int)RentSearch.Rent_Date)
                /*תוקף רשיון רכב*/
                return cmd + " Rent_Date Like '%" + searchDate.Date.ToString("yyyy-MM-dd") + "%'";
            else if (index == (int)RentSearch.Returned_Date)
                /*תוקף ביטוח הרכב*/
                return cmd + " Returned_Date Like '%" + searchDate.Date.ToString("yyyy-MM-dd") + "%'";
            else
                return string.Empty;
        }
        /*פונקציה שמחזירה את כל הנהגים*/
        public DataSet AllDrivers()
        {
            try
            {
                if (conn.IsConnect())
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT ID,First_Name,Last_Name,Phone_Number,Address,Telephone_Number,Birthdate,Expire_Date,Date_Of_Issue From driver_licensing JOIN user ON user.ID = driver_licensing.ID_Customer where ID != '0000000000' ", conn.Connection))
                    {
                        DataSet DS = new DataSet();
                        adapter.Fill(DS, "driver_licensing");
                        conn.Close();
                        return DS;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        /*פונקציה שמחזירה את כל הנהגים שלא היו בהשכרה*/
        public DataSet Drivers_Not_In_Rent()
        {
            try
            {
                if (conn.IsConnect())
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT ID,First_Name,Last_Name,Phone_Number,Address,Telephone_Number,Birthdate,Expire_Date,Date_Of_Issue From user JOIN driver_licensing ON user.ID = driver_licensing.ID_Customer LEFT JOIN drivers ON driver_licensing.ID_Customer = drivers.ID_Driver WHERE `ID_Driver` IS NULL AND `ID_Customer` != '0000000000' ", conn.Connection))
                    {
                        DataSet DS = new DataSet();
                        adapter.Fill(DS, "driver_licensing");
                        conn.Close();
                        return DS;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        /*פונקציה שמחזירה את כל הנהגים שהיו בהשכרה מהחדש ביותר*/
        public DataSet Drivers_In_Recent_Rent()
        {
            try
            {
                if (conn.IsConnect())
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT DISTINCT ID,First_Name,Last_Name,Phone_Number,Address,Telephone_Number,Birthdate,Expire_Date,Date_Of_Issue From  drivers JOIN driver_licensing ON ID_Customer = ID_Driver JOIN user  ON ID = ID_Customer ORDER BY `Rental_ID` DESC", conn.Connection))
                    {
                        DataSet DS = new DataSet();
                        adapter.Fill(DS, "driver_licensing");
                        conn.Close();
                        return DS;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        /*פונקציה מחזירה הלקוחות לפי הטקסט שמחפשים*/
        public DataSet SearchDriverTxt(int customerComboBox, int customerSearchComboBox, string customerSearchTxt)
        {
            string cmd = CustomerSearchText(customerComboBox, customerSearchComboBox, customerSearchTxt);
            if (cmd == string.Empty)
                return null;
            try
            {
                if (conn.IsConnect())
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd, conn.Connection))
                    {
                        DataSet DS = new DataSet();
                        adapter.Fill(DS, "driver_licensing");
                        conn.Close();
                        return DS;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        /*פונקציה מחזירה שאילתא מתאימה  לחיפוש בבסיס הנתונים*/
        private string CustomerSearchText(int customerComboBox, int customerSearchComboBox, string customerSearchTxt)
        {
            /*first*/
            string cmd = "SELECT ID,First_Name,Last_Name,Phone_Number,Address,Telephone_Number,Birthdate,Expire_Date,Date_Of_Issue From driver_licensing JOIN user ON user.ID = driver_licensing.ID_Customer where ID != '0000000000' AND ";
            if (customerComboBox == 1)
                cmd = "SELECT ID,First_Name,Last_Name,Phone_Number,Address,Telephone_Number,Birthdate,Expire_Date,Date_Of_Issue From user JOIN driver_licensing ON user.ID = driver_licensing.ID_Customer LEFT JOIN drivers ON driver_licensing.ID_Customer = drivers.ID_Driver WHERE `ID_Driver` IS NULL AND `ID_Customer` != '0000000000' AND ";
            else if (customerComboBox == 2)
                /*נשכירו חדש*/
                cmd = "SELECT DISTINCT ID,First_Name,Last_Name,Phone_Number,Address,Telephone_Number,Birthdate,Expire_Date,Date_Of_Issue From  drivers JOIN driver_licensing ON ID_Customer = ID_Driver JOIN user  ON ID = ID_Customer where";
            /*second*/
            if (customerSearchComboBox == 0 && customerSearchTxt.IsDig())
                /*ת"ז*/
                return cmd + " ID LIKE '%" + int.Parse(customerSearchTxt) + "%'" + (customerComboBox == 2 ? " ORDER BY `Rental_ID` DESC" : " ");
            else if (customerSearchComboBox == 1 && customerSearchTxt.IsHeOrEn())
                /*שם פרטי*/
                return cmd + " First_Name LIKE '%" + customerSearchTxt + "%'" + (customerComboBox == 2 ? " ORDER BY `Rental_ID` DESC" : " ");
            else if (customerSearchComboBox == 2 && customerSearchTxt.IsHeOrEn())
                /*שם משפחה*/
                return cmd + " Last_Name LIKE '%" + customerSearchTxt + "%'" + (customerComboBox == 2 ? " ORDER BY `Rental_ID` DESC" : " ");
            else if (customerSearchComboBox == 3 && customerSearchTxt.IsDig())
                /*מס' פלאפון*/
                return cmd + " Phone_Number LIKE '%" + int.Parse(customerSearchTxt) + "%'" + (customerComboBox == 2 ? " ORDER BY `Rental_ID` DESC" : " ");
            else if (customerSearchComboBox == 4 && customerSearchTxt.IsDig())
                /*מס' טלפון*/
                return cmd + " Telephone_Number LIKE '%" + int.Parse(customerSearchTxt) + "%'" + (customerComboBox == 2 ? " ORDER BY `Rental_ID` DESC" : " ");
            else if (customerSearchComboBox == 5)
                /*כתובת*/
                return cmd + " Address LIKE '%" + customerSearchTxt + "%'" + (customerComboBox == 2 ? " ORDER BY `Rental_ID` DESC" : " ");
            else
                return string.Empty;
        }
        /*פונקציה מחזירה הלקוחות לפי התאריך שמחפשים*/
        public DataSet SearchDriverDate(int customerComboBox, int customerSearchComboBox, string customerDateTimeSearch)
        {
            string cmd = CustomerSearchDate(customerComboBox, customerSearchComboBox, customerDateTimeSearch);
            if (cmd == string.Empty)
                return null;
            try
            {
                if (conn.IsConnect())
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd, conn.Connection))
                    {
                        DataSet DS = new DataSet();
                        adapter.Fill(DS, "driver_licensing");
                        conn.Close();
                        return DS;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        /*פונקציה מחזירה שאילתא מתאימה  לחיפוש בבסיס הנתונים*/
        private string CustomerSearchDate(int customerComboBox, int customerSearchComboBox, string customerDateTimeSearch)
        {
            /*first*/
            string cmd = "SELECT ID,First_Name,Last_Name,Phone_Number,Address,Telephone_Number,Birthdate,Expire_Date,Date_Of_Issue From driver_licensing JOIN user ON user.ID = driver_licensing.ID_Customer where ID != '0000000000' AND ";
            if (customerComboBox == 1)
                cmd = "SELECT ID,First_Name,Last_Name,Phone_Number,Address,Telephone_Number,Birthdate,Expire_Date,Date_Of_Issue From user JOIN driver_licensing ON user.ID = driver_licensing.ID_Customer LEFT JOIN drivers ON driver_licensing.ID_Customer = drivers.ID_Driver WHERE `ID_Driver` IS NULL AND `ID_Customer` != '0000000000' AND ";
            else if (customerComboBox == 2)
                /*נשכירו חדש*/
                cmd = "SELECT DISTINCT ID,First_Name,Last_Name,Phone_Number,Address,Telephone_Number,Birthdate,Expire_Date,Date_Of_Issue From  drivers JOIN driver_licensing ON ID_Customer = ID_Driver JOIN user  ON ID = ID_Customer where";
            /*second*/
            if (customerSearchComboBox == 6)
                /*תאריך לידה*/
                return cmd + " Birthdate  = '" + customerDateTimeSearch + "'" + (customerComboBox == 2 ? " ORDER BY `Rental_ID` DESC" : " ");
            else if (customerSearchComboBox == 7)
                /*תוקף רשיון*/
                return cmd + " Expire_Date  = '" + customerDateTimeSearch + "'" + (customerComboBox == 2 ? " ORDER BY `Rental_ID` DESC" : " ");
            else if (customerSearchComboBox == 8)
                /*תאריך יצור רישיון*/
                return cmd + " Date_Of_Issue = '" + customerDateTimeSearch + "'" + (customerComboBox == 2 ? " ORDER BY `Rental_ID` DESC" : " ");
            else
                return string.Empty;
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
        /*פונקציה לבדיקת את ת"ז אם קיימת*/
        public bool CheckId(string id, int i)/*if i = 2 check id in employee table else if i= 1 in user else if i= 3 in driver_licensing  */
        {
            if (conn.IsConnect())
            {
                string cmdText = "SELECT * From " + (i == 1 ? " user " : (i == 2 ? " employee " : " driver_licensing ")) + " WHERE " + (i == 1 ? " ID= " : (i == 2 ? " ID_Employee = " : " ID_Customer = ")) + id;
                MySqlCommand cmd = conn.Connection.CreateCommand();
                cmd.CommandText = cmdText;
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            reader.Close();
                            conn.Close();
                            return true;
                        }
                    }
                }
                conn.Close();
            }
            return false;
        }
        /*פונקציה להוסיף משתמש חדש*/
        public bool AddNewUser(User e)/*function to add user*/
        {
            if (!CheckId(e.Id, 1))
            {
                if (conn.IsConnect())
                {
                    string cmdText = "INSERT INTO user (ID,First_Name,Last_Name,Birthdate,Address,Phone_Number,Telephone_Number) VALUES (@ID,@First_Name,@Last_Name,@Birthdate,@Address,@Phone_Number,@Telephone_Number)";
                    using (MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection))
                    {
                        // cmd = new MySqlCommand(cmdText, conn.Connection);
                        cmd.Parameters.AddWithValue("@ID", e.Id);
                        cmd.Parameters.AddWithValue("@First_Name", e.First_name);
                        cmd.Parameters.AddWithValue("@Last_Name", e.Last_name);
                        cmd.Parameters.AddWithValue("@Birthdate", e.Birth_date.Date);
                        cmd.Parameters.AddWithValue("@Address", e.Address);
                        cmd.Parameters.AddWithValue("@Phone_Number", e.Phone_number);
                        cmd.Parameters.AddWithValue("@Telephone_Number", e.Telephone_number);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        return true;
                    }
                }
            }
            if (CheckId(e.Id, 1))
                return true;
            return false;
        }
        /*פונקציה להוסיף לקוח חדש*/
        public bool AddNewEmployee(Employee e)/*function to add employee and user if needed*/
        {
            if (!CheckId(e.Id, 2))//check if id is not exists in employee table
            {
                if (AddNewUser(e))//add and check user
                {
                    if (conn.IsConnect())
                    {
                        string cmdText = "INSERT INTO employee(ID_Employee, Salary, Password, Employee_Availability, Employye_Type, Start_Work_Date) VALUES(@ID_Employee, @Salary, @Password, @Employee_Availability, @Employye_Type, @Start_Work_Date)";
                        using (MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection))
                        {
                            cmd.Parameters.AddWithValue("@ID_Employee", e.Id);
                            cmd.Parameters.AddWithValue("@Salary", e.Salary);
                            cmd.Parameters.AddWithValue("@Password", e.Password);
                            cmd.Parameters.AddWithValue("@Employee_Availability", 1);
                            cmd.Parameters.AddWithValue("@Employye_Type", e.Employye_type);
                            cmd.Parameters.AddWithValue("@Start_Work_Date", DateTime.Now.Date);
                            cmd.ExecuteNonQuery();
                            conn.Close();
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        /*פונקציה לעדכן את ת"ז של העובד או הלקוח בתוך השכירות*/
        public bool UpdateEmployeeIdRent(string employeeID, string newEmployeeID, int i)/*for updating an employee we should give a id of a admin employee to cahnge and replace*/
        {
            if (conn.IsConnect())
            {
                string cmdText = " UPDATE rent_details SET ID_Employee_Rent = '" + newEmployeeID + "' WHERE `ID_Employee_Rent`= '" + employeeID + "'";
                /*ת"ז של העובד שהחזיר הרכב*/
                if (i == 1)
                    cmdText = " UPDATE rent_details SET ID_Employee_Returned = '" + newEmployeeID + "' WHERE `ID_Employee_Returned`= '" + employeeID + "'";
                if (i == 2)
                    cmdText = " UPDATE rent_details SET ID_Customer = '" + newEmployeeID + "' WHERE `ID_Customer`= '" + employeeID + "'";
                using (MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection))
                {
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return true;
                }
            }
            return false;
        }
        /*פונקציה שמחזירה פרטי של רישיון נהיגה של נהג מסויים*/
        public Driver_Licensing Driver_licensingDetail(User user, string id)
        {
            Driver_Licensing license;
            if (conn.IsConnect())
            {
                string cmdText = " SELECT Expire_Date,Date_Of_Issue From driver_licensing WHERE `ID_Customer` = '" + id + "'";
                using (MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection))
                {
                    cmd.CommandText = cmdText;
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            license = new Driver_Licensing(user.Id, user.First_name, user.Last_name, user.Address, user.Birth_date, user.Phone_number, user.Telephone_number, Convert.ToDateTime(reader.GetString("Expire_Date")), Convert.ToDateTime(reader.GetString("Date_Of_Issue")));
                            reader.Close();
                            conn.Close();
                            return license;
                        }
                    }
                }
                conn.Close();
            }
            return null;
        }
        /*פונקציה לעדכן את ת"ז של נהג*/
        public bool UpdateDriverId_InRent(string ID, string newID)
        {
            if (conn.IsConnect())
            {
                string cmdText = " UPDATE drivers SET ID_Driver = '" + newID + "' WHERE `ID_Driver` = '" + ID + "'";
                using (MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection))
                {
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return true;
                }
            }
            return false;
        }
        public bool UpdateDriverId_InMessages(string ID, string newID)
        {
            if (conn.IsConnect())
            {
                string cmdText = " UPDATE messages SET Employee_ID = '" + newID + "' WHERE `Employee_ID` = '" + ID + "'";
                using (MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection))
                {
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return true;
                }
            }
            return false;
        }
        public bool UpdateEmployeeId_InPassword_verification(string ID, string newID)
        {
            if (conn.IsConnect())
            {
                string cmdText = " UPDATE password_verification SET ID_Employee = '" + newID + "' WHERE `ID_Employee` = '" + ID + "'";
                using (MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection))
                {
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return true;
                }
            }
            return false;
        }
        /*פונקציה שמעדכנת את כל ת"ז בכל הטבלאות אם היה עובד*/
        public bool Update_All_ID(string ID, string newID)/*if employee*/
        {//////////////////////////////add to password verification... elias
            if (UpdateDriverId_InRent(ID, newID) && UpdateEmployeeIdRent(ID, newID, 0) && UpdateEmployeeIdRent(ID, newID, 1) &&
                UpdateEmployeeIdRent(ID, newID, 2) && UpdateEmployeeIdRent(ID, newID, 2) && UpdateDriverId_InMessages(ID, newID) && UpdateEmployeeId_InPassword_verification(ID, newID))
                return true;
            return false;
        }

        /*פונקציה לעדכן את פרטי העובד*/
        public bool UpdateOldEmployee(Employee e, bool active, DateTime start_date, DateTime end_Date, string id)
        {
            if ((id != e.Id && !CheckId(e.Id, 2) && !CheckId(e.Id, 1)) || e.Id == id)
            {
                if (Update_All_ID(id, "0000000000") && conn.IsConnect())
                {
                    Driver_Licensing license;
                    string deleteQuery = "DELETE FROM employee WHERE ID_Employee =" + id;
                    MySqlCommand cmd = new MySqlCommand(deleteQuery, conn.Connection);
                    cmd.ExecuteNonQuery();
                    license = Driver_licensingDetail(e, id);
                    if (CheckId(id, 3) && conn.IsConnect())
                    {
                        deleteQuery = "DELETE FROM driver_licensing WHERE ID_Customer =" + id;
                        cmd = new MySqlCommand(deleteQuery, conn.Connection);
                        cmd.ExecuteNonQuery();
                    }
                    if (conn.IsConnect())
                    {
                        deleteQuery = "DELETE FROM user WHERE ID =" + id;
                        cmd = new MySqlCommand(deleteQuery, conn.Connection);
                        cmd.ExecuteNonQuery();
                        conn.Close();
                    }
                    if (license != null)
                        Add_Driver_Licensing(license);
                    if (e.Employee_availability == false && active == true && AddNewEmployee(e) && Update_All_ID("0000000000", e.Id))
                        return true;
                    if (((e.Employee_availability == true && AddNewEmployee(e)) || (e.Employee_availability == false && AddNewEmployee(e))) && Update_All_ID("0000000000", e.Id) && conn.IsConnect())
                    {
                        string cmdText = " UPDATE employee SET Start_Work_Date = '" + start_date.Date.ToString("yyyy/MM/dd") + "', End_Work_Date = '" + end_Date.Date.ToString("yyyy/MM/dd") + "', Employee_Availability=0 WHERE `ID_Employee`= " + e.Id;
                        if (e.Employee_availability)
                            cmdText = " UPDATE employee SET Start_Work_Date = '" + start_date.Date.ToString("yyyy/MM/dd") + "', Employee_Availability=1 WHERE `ID_Employee`= " + e.Id;
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
        /*פונקציה שבודקת אם מספר הרכב קיים*/
        public bool CheckCarNum(string carNum)
        {
            if (conn.IsConnect())
            {
                string cmdText = "SELECT * From car WHERE Car_Number=" + carNum;
                MySqlCommand cmd = conn.Connection.CreateCommand();
                cmd.CommandText = cmdText;
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            reader.Close();
                            conn.Close();
                            return true;
                        }
                    }
                }
                conn.Close();
            }
            return false;
        }

        ///////////////////////Rent//////////////////////
        ///
        ////*פונקציה מחזירה את מספר השכירות הבא*/
        public int ReadRent_ID()
        {
            int rentID = 1;
            if (conn.IsConnect())
            {
                string cmdText = "SELECT Rental_ID From rent_details ORDER BY Rental_ID DESC LIMIT 1";
                MySqlCommand cmd = conn.Connection.CreateCommand();
                cmd.CommandText = cmdText;
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            rentID = int.Parse(reader.GetString("Rental_ID")) + 1;
                        }
                        conn.Close();
                        return rentID;
                    }
                }
                conn.Close();
            }
            return -1;//empty table
        }
        ///////////////////////////////check if there is rentDetails/////////////////
        public bool CheckRentId(int rentId)
        {
            if (conn.IsConnect())
            {
                string cmdText = "SELECT Rental_ID From rent_details where Rental_ID = "+ rentId;
                MySqlCommand cmd = conn.Connection.CreateCommand();
                cmd.CommandText = cmdText;
                using (MySqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            reader.Close();
                            conn.Close();
                            return true;
                        }
                    }
                }
                conn.Close();
            }
            return false;
        }
        /*פונקציה לעדכן רכב*/
        public bool RentUpdateCar(string carNum, double distance, string fuelCapacity, string carStatus)
        {
            if (conn.IsConnect())
            {
                string cmdText = " UPDATE car SET Car_Availability = '0' ,Distance=" + distance + ", Fuel_Capacity = '" + fuelCapacity + "' ,Car_Status = '" + carStatus + "' WHERE `Car_Number`= " + carNum;
                MySqlCommand cmd = conn.Connection.CreateCommand();
                cmd.CommandText = cmdText;
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            return false;
        }
        /*פונקציה למחוק נהגים משכירות מסוימת*/
        public bool RemoveDrivers(int rentId)
        {
            if (conn.IsConnect())
            {
                string deleteQuery = "DELETE FROM drivers WHERE Rental_ID =" + rentId;
                using (MySqlCommand cmd = new MySqlCommand(deleteQuery, conn.Connection))
                {
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return true;
                }
            }
            return false;
        }
        /*פונקציה בודקת אם נהג קיים מחזירים אמת אחרת שקר*/
        public bool Check_Driver_Licensing(string driver_id)
        {
            if (conn.IsConnect())
            {
                string cmdText = "SELECT * From driver_licensing WHERE ID_Customer=" + driver_id;
                MySqlCommand cmd = conn.Connection.CreateCommand();
                cmd.CommandText = cmdText;
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        reader.Close();
                        conn.Close();
                        return true;
                    }
                }
                conn.Close();
            }
            return false;
        }
        /*הוספת כל הנהגים להשכרה מסוימת*/
        public bool Add_Driver(int rentalID, string driverID)
        {
            if (conn.IsConnect())
            {
                string cmdText = "INSERT INTO  drivers(Rental_ID,ID_Driver) VALUES(@Rental_ID,@ID_Driver)";
                using (MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection))
                {
                    cmd.Parameters.AddWithValue("@Rental_ID", rentalID);
                    cmd.Parameters.AddWithValue("@ID_Driver", driverID);
                    cmd.CommandText = cmdText;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return true;
                }
            }
            return false;
        }
        /*מוסיפים נהג שלא קיים */
        public bool Add_Driver_Licensing(Driver_Licensing driver)
        {
            /*אם לא קיים ואם לא קיים נהג נוסיף אותו USER מוסיפים*/
            if (AddNewUser(driver) && !Check_Driver_Licensing(driver.Id) && conn.IsConnect())
            {
                string cmdText = "INSERT INTO  driver_licensing(ID_Customer,Expire_Date,Date_Of_Issue) VALUES(@ID_Customer,@Expire_Date,@Date_Of_Issue)";
                using (MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection))
                {
                    cmd.Parameters.AddWithValue("@ID_Customer", driver.Id);
                    cmd.Parameters.AddWithValue("@Expire_Date", driver.Expire_date);
                    cmd.Parameters.AddWithValue("@Date_Of_Issue", driver.Date_of_issue);
                    cmd.CommandText = cmdText;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return true;
                }
            }
            /*אם קיים נהג מעדכנים תוקף הרשיון */
            else if (Check_Driver_Licensing(driver.Id) && UpdateOldCustomerNOID(driver))
            {
                return true;
            }
            return false;
        }
        /*פונקציה להוסיף שכירות*/
        public bool Add_Rent(Rent_Details details, Driver_Licensing[] drivers)
        {
            for (int i = 0; i < drivers.Length; i++)
                if (!Add_Driver_Licensing(drivers[i]))
                    /*אם לא מצליחים להוסיף נהג*/
                    return false;
            if (conn.IsConnect())
            {
                string InsertText = "INSERT INTO  rent_details(Rental_ID,Rent_Date,Rent_Days,Returned_Date,Payment,Payment_Type,Price_For_Day,KM_For_Day,Rent_Distance,Quantity_Fuel_Rent,Car_Status_Rent,ID_Employee_Rent,ID_Customer,Car_Number)";
                string cmdText = InsertText + " VALUES(@Rental_ID,@Rent_Date,@Rent_Days,@Returned_Date,@Payment,@Payment_Type,@Price_For_Day,@KM_For_Day,@Rent_Distance,@Quantity_Fuel_Rent,@Car_Status_Rent,@ID_Employee_Rent,@ID_Customer,@Car_Number)";
                using (MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection))
                {
                    cmd.Parameters.AddWithValue("@Rental_ID", "");
                    cmd.Parameters.AddWithValue("@Rent_Date", details.Rent_Date.ToString("yyyy-MM-dd H:mm:ss"));
                    cmd.Parameters.AddWithValue("@Rent_Days", details.Rent_Days);
                    cmd.Parameters.AddWithValue("@Returned_Date", details.Returned_Date.ToString("yyyy-MM-dd H:mm:ss"));
                    cmd.Parameters.AddWithValue("@Payment", details.Payment);
                    cmd.Parameters.AddWithValue("@Payment_Type", details.Payment_Type);
                    cmd.Parameters.AddWithValue("@Price_For_Day", details.Price_For_Day);
                    cmd.Parameters.AddWithValue("@KM_For_Day", details.Km_For_Day);
                    cmd.Parameters.AddWithValue("@Rent_Distance", details.Rent_Distance);
                    cmd.Parameters.AddWithValue("@Quantity_Fuel_Rent", details.Quantity_Fuel_Rent);
                    cmd.Parameters.AddWithValue("@Car_Status_Rent", details.Car_Status);
                    cmd.Parameters.AddWithValue("@ID_Employee_Rent", details.ID_Employee);
                    cmd.Parameters.AddWithValue("@ID_Customer", details.ID_Customer);
                    cmd.Parameters.AddWithValue("@Car_Number", details.Car_Number);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
                for (int i = 0; i < drivers.Length; i++)
                    if (!Add_Driver(details.Rental_ID, drivers[i].Id))
                        /*אם לא מצליחים להוסיף נהג*/
                        return false;
                return true;
            }
            return false;
        }
        public bool Add_signature(Byte[] sign, int rentID, bool rentMode)
        {
            try
            {
                Byte[] signature = new byte[sign.Length];
                signature = sign;
                MySqlParameter picpara;
                if (conn.IsConnect())
                {
                    string InsertText = "INSERT INTO  signature(Rental_ID," + (rentMode == true ? "Rent_Signature" : "Return_Signature") + ")";
                    string cmdText = InsertText + " VALUES(@Rental_ID," + (rentMode == true ? "@Rent_Signature" : "@Return_Signature") + ")";
                    using (MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection))
                    {
                        cmd.Parameters.AddWithValue("@Rental_ID", rentID);
                        picpara = cmd.Parameters.Add((rentMode == true ? "@Rent_Signature" : "@Return_Signature"), MySqlDbType.Blob);
                        cmd.Prepare();
                        picpara.Value = signature;
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                    //cmd = new MySqlCommand("INSERT INTO mypic (pic) VALUES(?pic)", conn);
                    //txtPicPath is the path of the image, e.g. C:\MyPic.png
                    /*
                                    signature = new byte[Convert.ToInt32(sign.Length)];
                                    fs.Read(bindata, 0, Convert.ToInt32(fs.Length));
                                    fs.Close();

                                    cmd.ExecuteNonQuery();
                                    */
                    return true;
                }
                return false;
            }
            catch { return false; }
        }
        public bool Update_signature(Byte[] sign,int rentID,bool rentMode)
        {
            Byte[] signature=new byte[sign.Length];
            signature = sign;
            MySqlParameter picpara;
            if (conn.IsConnect())
            {
                string cmdText = "Update signature set " + (rentMode==true?"Rent_Signature =": "Return_Signature =")+(rentMode==true?"@Rent_Signature": "@Return_Signature")+ " Where Rental_ID = @Rental_ID";
                using (MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection))
                {
                    cmd.Parameters.AddWithValue("@Rental_ID", rentID);
                    picpara = cmd.Parameters.Add((rentMode == true ? "@Rent_Signature" : "@Return_Signature"), MySqlDbType.Blob);
                    cmd.Prepare();
                    picpara.Value = signature;
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
                //cmd = new MySqlCommand("INSERT INTO mypic (pic) VALUES(?pic)", conn);
                //txtPicPath is the path of the image, e.g. C:\MyPic.png
/*
                signature = new byte[Convert.ToInt32(sign.Length)];
                fs.Read(bindata, 0, Convert.ToInt32(fs.Length));
                fs.Close();

                cmd.ExecuteNonQuery();
                */return true;
            }
            return false;
        }
        public Byte[] Retrieve_signature(int rentID, bool rentMode)
        {
            Byte[] signature = null;
            try
            {
                if (conn.IsConnect())
                {
                    string cmdText = "SELECT " + (rentMode == true ? "Rent_Signature" : "Return_Signature") + " From  signature where `Rental_ID`= @Rental_ID";
                    using (MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection))
                    {
                        cmd.Parameters.AddWithValue("@Rental_ID", rentID);
                        cmd.Parameters.Add((rentMode == true ? "@Rent_Signature" : "@Return_Signature"), MySqlDbType.Blob);
                        signature = (byte[])(cmd.ExecuteScalar());
                    }
                    conn.Close();
                    return signature;
                    /*MemoryStream ms = new MemoryStream();
                    FileStream fs;
                    ms.Write(bindata, 0, bindata.Length);
                    pb2.Image = new Bitmap(ms);
                    fs = new FileStream(name, FileMode.Create, FileAccess.Write);
                    ms.WriteTo(fs);*/
                    //cmd = new MySqlCommand("INSERT INTO mypic (pic) VALUES(?pic)", conn);
                    //txtPicPath is the path of the image, e.g. C:\MyPic.png
                    /*
                                    signature = new byte[Convert.ToInt32(sign.Length)];
                                    fs.Read(bindata, 0, Convert.ToInt32(fs.Length));
                                    fs.Close();
                                    cmd.ExecuteNonQuery();
                                    */
                    //return true;
                }
                return signature;
            }
            catch
            {
                conn.Close();
                return null;
            }
        }
        /*פונקציה לעדכן הועדה של שכירות מוסימת*/
        public bool UpdateRentComment(DateTime commentDate, string Employee_ID, string rentID)
        {
            if (conn.IsConnect())
            {
                string updateText = "UPDATE   messages Set Employee_ID='" + Employee_ID + "'";
                updateText += ", Message_ToDate='" + commentDate.AddDays(-1).ToString("yyyy-MM-dd H:mm:ss") + "'";
                updateText += " WHERE `Car_Number`= " + rentID + " and `Message_Title`='החזרת שכירות'";
                using (MySqlCommand cmd = new MySqlCommand(updateText, conn.Connection))
                {
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
                return true;
            }
            return false;
        }
        /*פונקציה למחוק הועדה של שכירות מוסיים*/
        public bool DeleteRentComment(string rentID)
        {
            if (conn.IsConnect())
            {
                string updateText = "UPDATE  messages Set Message_Availability='" + 0 + "' WHERE `Car_Number`= " + rentID + " and `Message_Title`= 'החזרת שכירות'";
                using (MySqlCommand cmd = new MySqlCommand(updateText, conn.Connection))
                {
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
                return true;
            }
            return false;
        }
        public bool AddComment(string Employee_ID, string commentText, DateTime commentDate, string messageFor)
        {
            if (conn.IsConnect())
            {
                string InsertText = "INSERT INTO messages(Employee_ID,Car_Number,Message_For,Message_Text, Message_Type, Message_Availability, Message_ToDate, Message_Title)";
                string cmdText = InsertText + " VALUES(@Employee_ID,@Car_Number,@Message_For,@Message_Text,@Message_Type,@Message_Availability,@Message_ToDate,@Message_Title)";
                using (MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection))
                {
                    cmd.Parameters.AddWithValue("@Employee_ID", Employee_ID);
                    cmd.Parameters.AddWithValue("@Car_Number", "0000");
                    cmd.Parameters.AddWithValue("@Message_For", messageFor);
                    cmd.Parameters.AddWithValue("@Message_Text", commentText);
                    cmd.Parameters.AddWithValue("@Message_Type", "תזכורת אישית");
                    cmd.Parameters.AddWithValue("@Message_Availability", 1);
                    cmd.Parameters.AddWithValue("@Message_ToDate", commentDate.ToString("yyyy-MM-dd H:mm:ss"));
                    cmd.Parameters.AddWithValue("@Message_Title", messageFor);
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
                return true;
            }
            return false;
        }
        /*פונקציה להוסיף הודעה על שכירות*/
        public bool AddRentComment(Rent_Details details, string Employee_ID)
        {
            if (conn.IsConnect())
            {
                string InsertText = "INSERT INTO messages(Employee_ID,Car_Number,Message_For,Message_Text, Message_Type, Message_Availability, Message_ToDate, Message_Title)";
                string cmdText = InsertText + " VALUES(@Employee_ID,@Car_Number,@Message_For,@Message_Text,@Message_Type,@Message_Availability,@Message_ToDate,@Message_Title)";
                using (MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection))
                {
                    cmd.Parameters.AddWithValue("@Employee_ID", Employee_ID);
                    cmd.Parameters.AddWithValue("@Car_Number", details.Rental_ID.ToString());
                    cmd.Parameters.AddWithValue("@Message_For", "לכל העובדים");
                    cmd.Parameters.AddWithValue("@Message_Text", "מחר צריך לחזיר רכב של מספר שכירות " + details.Rental_ID.ToString());
                    cmd.Parameters.AddWithValue("@Message_Type", "שכירות");
                    cmd.Parameters.AddWithValue("@Message_Availability", 1);
                    cmd.Parameters.AddWithValue("@Message_ToDate", details.Returned_Date.AddDays(-1).ToString("yyyy-MM-dd H:mm:ss"));
                    cmd.Parameters.AddWithValue("@Message_Title", "החזרת שכירות");
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
                return true;
            }
            return false;
        }
        /*פונקציה לעדכן שכירות*/
        public bool Update_Rent_Details(Rent_Details details, Driver_Licensing[] drivers)
        {
            for (int i = 0; i < drivers.Length; i++)
                if (!Add_Driver_Licensing(drivers[i]))
                    /*אם לא מצליחים להוסיף נהג*/
                    return false;
            for (int i = 0; i < drivers.Length; i++)
                if (!Add_Driver(details.Rental_ID, drivers[i].Id))
                    /*אם לא מצליחים להוסיף נהג*/
                    return false;
            if (conn.IsConnect())
            {
                string updateText = "UPDATE   rent_details Set Rent_Days='" + details.Rent_Days + "' , Returned_Date='" + details.Returned_Date.ToString("yyyy-MM-dd H:mm:ss") + "', Payment ='" + details.Payment;
                updateText += "', Payment_Type='" + details.Payment_Type + "' , Price_For_Day='" + details.Price_For_Day + "',KM_For_Day='" + details.Km_For_Day + "',Rent_Distance='" + details.Rent_Distance;
                updateText += "',Quantity_Fuel_Rent='" + details.Quantity_Fuel_Rent + "',Car_Status_Rent='" + details.Car_Status + "',ID_Customer='" + details.ID_Customer + "' where `Rental_ID`=" + details.Rental_ID;
                using (MySqlCommand cmd = new MySqlCommand(updateText, conn.Connection))
                {
                    cmd.ExecuteNonQuery();
                }
                conn.Close();
                return true;
            }
            return false;
        }
        /*פונקציה להחזיר שכירות*/
        public bool Return_Rent(Rent_Details details)
        {
            if (conn.IsConnect())
            {
                if (ReturnUpdateRent(details) && ReturnUpdateCar(details.Car_Number, details.Return_Distance, details.Quantity_Fuel_Returned, details.Return_Car_Status))
                    return true;
            }
            return false;
        }
        /*פונקציה לעדכן רכב אחרי שהחזרנו שכירות*/
        public bool ReturnUpdateCar(string carNum, double distance, string fuelCapacity, string carStatus)
        {
            if (conn.IsConnect())
            {
                string cmdText = " UPDATE car SET Car_Availability = '1' ,Distance=" + distance + ", Fuel_Capacity = '" + fuelCapacity + "' , Car_Status ='" + carStatus + "' WHERE `Car_Number`= " + carNum;
                MySqlCommand cmd = conn.Connection.CreateCommand();
                cmd.CommandText = cmdText;
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            return false;
        }
        /*פונקציה לעדכן שכירות שכבר הוחזירה*/
        public bool ReturnUpdateRent(Rent_Details details)
        {
            if (conn.IsConnect())
            {
                string cmdText = " UPDATE rent_details SET Rent_Days=" + details.Rent_Days + ", Returned_Date = '" + details.Returned_Date.ToString("yyyy-MM-dd H:mm:ss") +
                    "' , Payment = " + details.Payment + " , Payment_Type = '" + details.Payment_Type + "' , Return_Distance = " + details.Return_Distance + 
                    " , Quantity_Fuel_Returned = '" + details.Quantity_Fuel_Returned + "' , Car_Status_Returned = '" + details.Return_Car_Status + 
                    "' , ID_Employee_Returned = '" + details.ID_Employee + "' WHERE `Rental_ID`= " + details.Rental_ID;
                MySqlCommand cmd = conn.Connection.CreateCommand();
                cmd.CommandText = cmdText;
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            return false;
        }
        /*פונקציה מחזירה שם פרטי ומשפחה לעובד לפי ת"ז*/
        public string EmployeeFL(string employee_ID)
        {
            if (conn.IsConnect())
            {
                string FL;
                string cmdText = "SELECT First_Name,Last_Name From user WHERE ID=" + employee_ID;
                MySqlCommand cmd = conn.Connection.CreateCommand();
                cmd.CommandText = cmdText;
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        FL = reader.GetString("First_Name") + " " + reader.GetString("Last_Name");
                        reader.Close();
                        conn.Close();
                        return FL;
                    }
                }
                conn.Close();
            }
            return null;
        }
        /*פונקציה להוספת לקוח*/
        public bool AddNewCustomer(Driver_Licensing newCustomer)
        {
            User newUser = new User(newCustomer.Id, newCustomer.First_name, newCustomer.Last_name, newCustomer.Address, newCustomer.Birth_date, newCustomer.Phone_number, newCustomer.Telephone_number);
            if (!Check_Driver_Licensing(newCustomer.Id) && AddNewUser(newUser) && Add_Driver_Licensing(newCustomer))
                return true;
            return false;
        }
        /*פונקציה לחזיר פרטי עובד מסויים*/
        private Employee EmployeeDetalis(string id, Driver_Licensing NewE)
        {
            Employee employee;
            float salary;
            if (conn.IsConnect())
            {
                string cmdText = " SELECT * From employee JOIN user ON user.ID = employee.ID_Employee WHERE `ID_Employee` = '" + id + "'";
                MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection);
                cmd.CommandText = cmdText;
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        float.TryParse(reader["Salary"].ToString(), out salary);
                        employee = new Employee(NewE.Id, NewE.First_name, NewE.Last_name, NewE.Address, NewE.Birth_date, NewE.Phone_number, salary, reader["Employye_Type"].ToString(), NewE.Telephone_number);
                        //employee.Password = PassEncrypt(NewE.Id); elias
                        employee.Password = reader["Password"].ToString();
                        reader.Close();
                        conn.Close();
                        return employee;
                    }
                }
                conn.Close();
            }
            return null;
        }
        /*פונקציה לחזיר פרטי עובד מסויים*/
        private Employee EmployeeDetalis(string id)
        {
            Employee employee;
            DateTime start_date;
            DateTime end_date;
            bool active;
            if (conn.IsConnect())
            {
                string cmdText = " SELECT * From employee WHERE `ID_Employee` = '" + id + "'";
                using (MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection))
                {
                    cmd.CommandText = cmdText;
                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader != null)
                    {
                        if (reader.Read())
                        {
                            employee = new Employee();
                            bool.TryParse(reader["Employee_Availability"].ToString(), out active);
                            employee.Employee_availability = active;
                            DateTime.TryParse(reader["Start_Work_Date"].ToString(), out start_date);
                            employee.Start_work_date = start_date;
                            DateTime.TryParse(reader["End_Work_Date"].ToString(), out end_date);
                            employee.End_work_date = end_date;
                            reader.Close();
                            conn.Close();
                            return employee;
                        }
                    }
                }
                conn.Close();
            }
            return null;
        }
        /*פונקציה לעדכן ת"ז של לקוח קיים*/
        private bool UpdateOldCustomerID(Driver_Licensing OldCustomer, string id)
        {
            if (UpdateDriverId_InRent(id, "0000000000") && UpdateEmployeeIdRent(id, "0000000000", 2))
            {
                string deleteQuery;
                MySqlCommand cmd;
                if (CheckId(id, 3) && conn.IsConnect())
                {
                    deleteQuery = "DELETE FROM driver_licensing WHERE ID_Customer =" + id;
                    cmd = new MySqlCommand(deleteQuery, conn.Connection);
                    cmd.ExecuteNonQuery();
                }
                if (conn.IsConnect())
                {
                    deleteQuery = "DELETE FROM user WHERE ID =" + id;
                    using (cmd = new MySqlCommand(deleteQuery, conn.Connection))
                    {
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                }
                if (OldCustomer != null && Add_Driver_Licensing(OldCustomer) && UpdateDriverId_InRent("0000000000", OldCustomer.Id) && UpdateEmployeeIdRent("0000000000", OldCustomer.Id, 2))
                    return true;
            }
            return false;
        }
        /*פונקציה לעדכן לקוח*/
        public bool UpdateOldCustomer(Driver_Licensing OldCustomer, string id)
        {
            Employee employeeUser;
            Employee employee;
            if (id == OldCustomer.Id || (id != OldCustomer.Id && !Check_Driver_Licensing(OldCustomer.Id)))
            {
                //אם הלקוח הוא גם עובד ושנינו ת"ז
                if (id != OldCustomer.Id && !CheckId(OldCustomer.Id, 2) && CheckId(id, 2))
                {
                    employee = EmployeeDetalis(id);
                    employeeUser = EmployeeDetalis(id, OldCustomer);
                    if (UpdateOldEmployee(employeeUser, employee.Employee_availability, employee.Start_work_date, employee.End_work_date, id) && UpdateOldCustomerNOID(OldCustomer))
                        return true;
                }
                //עדכון פרטי לקוח עם שינוי של ת"ז
                if (id != OldCustomer.Id && !CheckId(OldCustomer.Id, 1) && CheckId(id, 3))
                {
                    if (UpdateOldCustomerID(OldCustomer, id))
                        return true;
                }
                //עדכון פרטי לקוח ללא שינוי ב ת"ז
                if (id == OldCustomer.Id && UpdateOldCustomerNOID(OldCustomer))
                    return true;
            }
            return false;
        }
        /*פונקציה לעדכן לקוח בלי לעדכן ת"ז שלו*/
        private bool UpdateOldCustomerNOID(Driver_Licensing OldCustomer)
        {
            if (conn.IsConnect())
            {
                string cmdText = " UPDATE user SET First_Name ='" + OldCustomer.First_name + "', Last_Name='" + OldCustomer.Last_name + "', Birthdate = '" + OldCustomer.Birth_date.Date.ToString("yyyy/MM/dd") + "', Address='" + OldCustomer.Address + "', Phone_Number='" + OldCustomer.Phone_number + "', Telephone_Number='" + OldCustomer.Telephone_number + "' WHERE `ID`= " + OldCustomer.Id;
                MySqlCommand cmd = conn.Connection.CreateCommand();
                cmd.CommandText = cmdText;
                cmd.ExecuteNonQuery();
                cmdText = " UPDATE driver_licensing SET Expire_Date ='" + OldCustomer.Expire_date.Date.ToString("yyyy/MM/dd") + "', Date_Of_Issue='" + OldCustomer.Date_of_issue.Date.ToString("yyyy/MM/dd") + "' WHERE `ID_Customer`= " + OldCustomer.Id;
                cmd = conn.Connection.CreateCommand();
                cmd.CommandText = cmdText;
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            return false;
        }
        /*פונקציה לחזיר מצב הרכב לפני השכירות*/
        public string CarStatus(int rentID, string cmdTxt)
        {
            if (conn.IsConnect())
            {
                string carStatus;
                string cmdText = "SELECT " + cmdTxt + " From rent_details WHERE `Rental_ID` = " + rentID;
                MySqlCommand cmd = conn.Connection.CreateCommand();
                cmd.CommandText = cmdText;
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.Read())
                    {
                        carStatus = reader.GetString(cmdTxt) + "\r\n";
                        reader.Close();
                        conn.Close();
                        return carStatus;
                    }
                }
                conn.Close();
            }
            return null;
        }
        /*פונקציה שמחזירה את ת"ז של כל הנהגים בשכירות מסוימת*/
        public DataSet DriversInRent(int rentID)
        {
            try
            {
                if (conn.IsConnect())
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT ID_Driver From drivers WHERE `Rental_ID` = " + rentID, conn.Connection))
                    {
                        DataSet DS = new DataSet();
                        adapter.Fill(DS, "drivers");
                        conn.Close();
                        return DS;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        /*פונקציה לעדכן שכירות*/
        public bool UpdateRent(string carReturnStatus, int rentId, string paymentStatus)
        {
            if (conn.IsConnect())
            {
                string cmdText = "UPDATE rent_details SET Car_Status_Returned = '" + carReturnStatus + "' ,Payment_Type = '" + paymentStatus + "' WHERE `Rental_ID`= '" + rentId + "'";
                using (MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection))
                {
                    cmd.ExecuteNonQuery();
                }
                /* cmdText = "UPDATE car SET Car_Status = '" + carReturnStatus + "' WHERE `Car_Number`= '" + carNum + "'";
                 cmd = new MySqlCommand(cmdText, conn.Connection);
                 cmd = conn.Connection.CreateCommand();
                 cmd.CommandText = cmdText;
                 cmd.ExecuteNonQuery();*/
                conn.Close();
                return true;
            }
            return false;
        }

        public DataSet CommentEmployeeINFO(int commentID)
        {
            try
            {
                if (conn.IsConnect())
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM `messages` WHERE `Message_ID`=" + commentID, conn.Connection))
                    {
                        DataSet DS = new DataSet();
                        adapter.Fill(DS, "messages");
                        conn.Close();
                        return DS;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public bool RemoveComment(int commentID)
        {
            if (conn.IsConnect())
            {
                string cmdText = " UPDATE messages SET Message_Availability= 0 WHERE `Message_ID`= " + commentID;
                MySqlCommand cmd = conn.Connection.CreateCommand();
                cmd.CommandText = cmdText;
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            return false;
        }

        public bool UpdateComment(string commentText, DateTime commentDate, string messageFor, int commentID)
        {
            if (conn.IsConnect())
            {
                string cmdText = " UPDATE messages SET Message_For=" + '"' + messageFor + '"' + ", Message_Title=" + '"' + messageFor + '"' + ", Message_ToDate=" + '"' + commentDate.ToString("yyyy-MM-dd H:mm:ss") + '"' + ", Message_Text=" + '"' + commentText + '"' + " WHERE `Message_ID`=" + commentID;
                MySqlCommand cmd = conn.Connection.CreateCommand();
                cmd.CommandText = cmdText;
                cmd.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            return false;
        }

        public DataSet SearchEmployeeInfoRent(string IDEmployee)
        {
            try
            {
                if (conn.IsConnect())
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT `Rental_ID` AS 'מספר שכירות',rent_details.Car_Number AS 'מספר רכב',CONCAT(car.Car_Manufacturer,' ',car.Car_Model) AS 'רכב'," +
                                                                 "`Payment` AS 'תשלום',`Rent_Date` AS 'תאריך שכירות',`Returned_Date` AS 'תאריך החזרה' " +
                                                                 "FROM `rent_details` JOIN `car` ON rent_details.Car_Number=car.Car_Number WHERE `ID_Employee_Rent`='" + IDEmployee + "'", conn.Connection))
                    {
                        DataSet DS = new DataSet();
                        adapter.Fill(DS, "employeeInfo");
                        conn.Close();
                        return DS;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        public DataSet SearchEmployeeInfoReturn(string IDEmployee)
        {
            try
            {
                if (conn.IsConnect())
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT `Rental_ID` AS 'מספר שכירות',rent_details.Car_Number AS 'מספר רכב',CONCAT(car.Car_Manufacturer,' ',car.Car_Model) AS 'רכב',`Payment` AS 'תשלום',`Rent_Date` AS 'תאריך שכירות',`Returned_Date` AS 'תאריך החזרה' FROM `rent_details` JOIN `car` ON rent_details.Car_Number=car.Car_Number WHERE `ID_Employee_Returned`='" + IDEmployee + "'", conn.Connection))
                    {
                        DataSet DS = new DataSet();
                        adapter.Fill(DS, "employeeInfo");
                        conn.Close();
                        return DS;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        /////////////////////////////////Invoice/////////////////
        public int ReadInvoice_ID()
        {
            try
            {
                int invoiceID = 1;
                if (conn.IsConnect())
                {
                    string cmdText = "SELECT Invoice_ID From tax_invoice ORDER BY Invoice_ID DESC LIMIT 1";
                    MySqlCommand cmd = conn.Connection.CreateCommand();
                    cmd.CommandText = cmdText;
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            if (reader.Read())
                            {
                                invoiceID = int.Parse(reader.GetString("Invoice_ID")) + 1;
                            }
                            conn.Close();
                            return invoiceID;
                        }
                    }
                    conn.Close();
                }
                return -1;//empty table
            }
            catch
            {
                return -1;
            }
        }

        /////////////function to add to TaxInvoice Table//////////
        public bool AddTaxInvoice(TaxInvoice taxInvoice)
        {
            try
            {
                if (conn.IsConnect())
                {
                    string InsertText = "INSERT INTO tax_invoice(Invoice_ID,Rental_ID,Car_Number,Bill_To,Date,Address,Cash_Amount,Checks_Amount,Visa_Amount,Sub_Total,VAT,Total)";
                    string cmdText = InsertText + " VALUES(@Invoice_ID,@Rental_ID,@Car_Number,@Bill_To,@Date,@Address,@Cash_Amount,@Checks_Amount,@Visa_Amount,@Sub_Total,@VAT,@Total)";
                    using (MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection))
                    {
                        cmd.Parameters.AddWithValue("@Invoice_ID",taxInvoice.InvoiceID);
                        cmd.Parameters.AddWithValue("@Rental_ID", taxInvoice.RentalID);
                        cmd.Parameters.AddWithValue("@Car_Number", taxInvoice.CarNumber);
                        cmd.Parameters.AddWithValue("@Bill_To", taxInvoice.BillTo.ToString());
                        cmd.Parameters.AddWithValue("@Date", taxInvoice.Date.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@Address", taxInvoice.Address);
                        cmd.Parameters.AddWithValue("@Cash_Amount", taxInvoice.CashAmount);
                        cmd.Parameters.AddWithValue("@Checks_Amount", taxInvoice.ChecksAmount);
                        cmd.Parameters.AddWithValue("@Visa_Amount", taxInvoice.VisaAmount);
                        cmd.Parameters.AddWithValue("@Sub_Total", taxInvoice.SubTotal);
                        cmd.Parameters.AddWithValue("@VAT", taxInvoice.Vat);
                        cmd.Parameters.AddWithValue("@Total", taxInvoice.Total);
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /////////// two functions to add array with invoice details to InvoiceDetails Table
        public bool AddInvoiceDetails(InvoiceDetails[] invoiceDetails)
        {
            int i = 0;
            /*if (invoiceDetails[0].AmountDescription == 0 && invoiceDetails.Length == 1)
                return false;*/
            if (invoiceDetails[0].AmountDescription == 0 && invoiceDetails.Length > 1)
                i++;
            for (; i < invoiceDetails.Length; i++)
            {
                if (!AddInvoiceDetail(invoiceDetails[i]))
                    return false;
            }
            return true;
        }
        public bool AddInvoiceDetail(InvoiceDetails invoiceDetails)
        {
            try
            {
                if (conn.IsConnect())
                {
                    string InsertText = "INSERT INTO invoice_details(Invoice_ID,Qty,Description,Unit_Price,Amount_Description)";
                    string cmdText = InsertText + " VALUES(@Invoice_ID,@Qty,@Description,@Unit_Price,@Amount_Description)";
                    using (MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection))
                    {
                        cmd.Parameters.AddWithValue("@Invoice_ID", invoiceDetails.InvoiceID);
                        cmd.Parameters.AddWithValue("@Qty", invoiceDetails.Qty);
                        cmd.Parameters.AddWithValue("@Description", invoiceDetails.Description);
                        cmd.Parameters.AddWithValue("@Unit_Price", invoiceDetails.UnitPrice);
                        cmd.Parameters.AddWithValue("@Amount_Description", invoiceDetails.AmountDescription);
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        /////////////////function to add checks to tax_invoice_check Table////////
        public bool AddChecksDetails(TaxInvoiceCheck[] taxInvoiceChecks)
        {
            for (int i = 0; (taxInvoiceChecks != null) && (i < taxInvoiceChecks.Length); i++)
            {
                if (!AddCheckDetail(taxInvoiceChecks[i]))
                    return false;
            }
            if ((taxInvoiceChecks != null) && (taxInvoiceChecks.Length > 0))
                return true;
            return false;
        }
        public bool AddCheckDetail(TaxInvoiceCheck taxInvoiceChecks)
        {
            try
            {
                if (conn.IsConnect())
                {
                    string InsertText = "INSERT INTO tax_invoice_check(Check_NO,Invoice_ID,Account_NO,Routing_NO,Check_Date,Check_Amount)";
                    string cmdText = InsertText + " VALUES(@Check_NO,@Invoice_ID,@Account_NO,@Routing_NO,@Check_Date,@Check_Amount)";
                    using (MySqlCommand cmd = new MySqlCommand(cmdText, conn.Connection))
                    {
                        cmd.Parameters.AddWithValue("@Check_NO", taxInvoiceChecks.CheckNO);
                        cmd.Parameters.AddWithValue("@Invoice_ID", taxInvoiceChecks.InvoiceID);
                        cmd.Parameters.AddWithValue("@Account_NO", taxInvoiceChecks.AccountNO);
                        cmd.Parameters.AddWithValue("@Routing_NO", taxInvoiceChecks.RoutingNO);
                        cmd.Parameters.AddWithValue("@Check_Date", taxInvoiceChecks.CheckDate.ToString("yyyy-MM-dd"));
                        cmd.Parameters.AddWithValue("@Check_Amount", taxInvoiceChecks.CheckAmount);
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        ////////////function to add the SubTotal payed to rentDetails Table//////
        public bool UpdateRentDetailPayed(float payed, int RentID)
        {
            try
            {
                if (conn.IsConnect())
                {
                    string cmdText = " UPDATE rent_details SET Payed = Payed + @Payed WHERE `Rental_ID`= @Rental_ID";
                    MySqlCommand cmd = conn.Connection.CreateCommand();
                    cmd.CommandText = cmdText;
                    cmd.Parameters.AddWithValue("@Payed", payed);
                    cmd.Parameters.AddWithValue("@Rental_ID", RentID);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        ///function to return TaxInvoice with specific rentalID
        public DataSet TaxInvoice(string rentId)
        {
            try
            {
                if (conn.IsConnect())
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT Invoice_ID,Date,Total FROM `tax_invoice` WHERE `Rental_ID`=" + rentId, conn.Connection))
                    {
                        DataSet DS = new DataSet();
                        adapter.Fill(DS, "tax_invoice");
                        conn.Close();
                        return DS;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        public TaxInvoice TaxInvoice(int invoiceId)
        {
            try
            {
                if (conn.IsConnect())
                {
                    string cmdText = "SELECT * FROM `tax_invoice` WHERE `Invoice_ID`=" + invoiceId;
                    MySqlCommand cmd = conn.Connection.CreateCommand();
                    cmd.CommandText = cmdText;
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            if (reader.Read())
                            {
                                TaxInvoice taxInvoice = new TaxInvoice(int.Parse(reader.GetString("Invoice_ID")), int.Parse(reader.GetString("Rental_ID")), reader.GetString("Car_Number"),
                                    reader.GetString("Bill_To"), Convert.ToDateTime(reader.GetString("Date")), reader.GetString("Address"), float.Parse(reader.GetString("Cash_Amount")),
                                    float.Parse(reader.GetString("Checks_Amount")), float.Parse(reader.GetString("Visa_Amount")), float.Parse(reader.GetString("Sub_Total")),
                                    float.Parse(reader.GetString("VAT")), float.Parse(reader.GetString("Total")));
                                conn.Close();
                                return taxInvoice;
                            }

                        }
                    }
                    conn.Close();
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        public DataSet TaxInvoiceChecks(int invoiceId)
        {
            try
            {
                if (conn.IsConnect())
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT Check_NO,Account_NO,Routing_NO,Check_Date,Check_Amount FROM `tax_invoice_check` WHERE `Invoice_ID`=" + invoiceId, conn.Connection))
                    {
                        DataSet DS = new DataSet();
                        adapter.Fill(DS, "tax_invoice_check");
                        conn.Close();
                        return DS;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
        public DataSet TaxInvoiceDetails(int invoiceId)
        {
            try
            {
                if (conn.IsConnect())
                {
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM `invoice_details` WHERE `Invoice_ID`=" + invoiceId, conn.Connection))
                    {
                        DataSet DS = new DataSet();
                        adapter.Fill(DS, "invoice_details");
                        conn.Close();
                        return DS;
                    }
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
/*פונקציה כללית להוציאת להחיר מהטבלה*/
/* public DataSet SearchDataBase(string cmd, string table)
 {
     try
     {
         if (conn.IsConnect())
         {
             MySqlDataAdapter adapter = new MySqlDataAdapter(cmd, conn.Connection);
             DataSet DS = new DataSet();
             adapter.Fill(DS, table);
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
