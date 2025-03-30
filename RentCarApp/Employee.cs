using System;

namespace RentCarApp
{
    /*מחלקה לשמרת פרטי עובד*/
    public class Employee : User
    {
        /*user נתונים של עובד ש צריך לשמור במחלקה זו והמחלקה יורשת מ*/
        private float salary;
        private string password;
        private bool employee_availability;
        private string employye_type;
        private DateTime start_work_date;
        private DateTime end_work_date;
        private DateTime password_date;
        /*בנאים*/
        public Employee()
        {

        }
        public Employee(string idB, string first_nameB, string last_nameB, string addressB, DateTime birth_dateB, string phone_numberB, float salaryB, string employye_typeB, string telephone_numberB) : base(idB, first_nameB, last_nameB, addressB, birth_dateB, phone_numberB, telephone_numberB)
        {
            salary = salaryB;
            password = idB;
            employee_availability = true;
            employye_type = employye_typeB;
        }
        /* getters and setters */
        public float Salary
        {
            get
            {
                return salary;
            }
            set
            {
                if (value > 0)
                    salary = value;
            }
        }
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                if (value != string.Empty)
                    password = value;
            }
        }
        public bool Employee_availability
        {
            get
            {
                return employee_availability;
            }
            set
            {
                employee_availability = value;
            }
        }
        public string Employye_type
        {
            get
            {
                return employye_type;
            }
            set
            {
                employye_type = value;
            }
        }

        public DateTime Start_work_date
        {
            get
            {
                return start_work_date;
            }
            set
            {
                start_work_date = value;
            }
        }
        public DateTime End_work_date
        {
            get
            {
                return end_work_date;
            }
            set
            {
                end_work_date = value;
            }
        }
        public DateTime Password_date
        {
            get
            {
                return password_date;
            }
            set
            {
                password_date = value;
            }
        }
    }
}
