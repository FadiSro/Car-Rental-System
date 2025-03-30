using System;
using System.Linq;

namespace RentCarApp
{
    /*מחלקה לשמרת פרטי משתמש*/
    public class User
    {
     /*נתונים של המשתמש ש צריך לשמור*/
        private string id;
        private string first_name;
        private string last_name;
        private string address;
        private DateTime birth_date;
        private string phone_number;
        private string telephone_number;

        public User() { }
        public User(string idB, string first_nameB, string last_nameB, string addressB, DateTime birth_dateB, string phone_numberB, string telephone_numberB)
        {
            id = idB;
            first_name = first_nameB;
            last_name = last_nameB;
            address = addressB;
            birth_date = birth_dateB;
            phone_number = phone_numberB;
            telephone_number = telephone_numberB;
        }
        /* getters and setters  */
        public string Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
            }
        }
        public string First_name
        {
            get
            {
                return first_name;
            }
            set
            {
                if (value != string.Empty && value.All(char.IsLetter) && value.Length < 20)
                    first_name = value;
            }
        }
        public string Last_name
        {
            get
            {
                return last_name;
            }
            set
            {
                if (value != string.Empty && value.All(char.IsLetter) && value.Length < 20)
                    last_name = value;
            }
        }
        public string Address
        {
            get
            {
                return address;
            }
            set
            {
                if (value != string.Empty && value.Length < 20)
                    address = value;
            }
        }
        public DateTime Birth_date
        {
            get
            {
                return birth_date;
            }
            set
            {
                if (value != null)
                    birth_date = value;
            }
        }
        public string Phone_number
        {
            get
            {
                return phone_number;
            }
            set
            {
                if (value != string.Empty && value.Length == 10 && value.All(char.IsDigit))
                    phone_number = value;
            }
        }
        public string Telephone_number
        {
            get
            {
                return telephone_number;
            }
            set
            {
                if (value != string.Empty && value.All(char.IsDigit))
                    telephone_number = value;
            }
        }
        public static int CurrAge(DateTime birthDate)
        {
            DateTime curr_Date = DateTime.Now;
            int age = curr_Date.Year - birthDate.Year;
            if (curr_Date.Month < birthDate.Month || (curr_Date.Month == birthDate.Month && curr_Date.Day < birthDate.Day))
                age--;
            return age;
        }

    }
}
