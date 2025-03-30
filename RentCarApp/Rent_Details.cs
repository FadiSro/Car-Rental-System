using System;

namespace RentCarApp
{
    /*מחלקה לשמרת פרטי שכירות*/
    public class Rent_Details
    {
        /*נתונים של שכירות ש צריך לשמור*/
        private int rental_ID;
        private DateTime rent_Date;
        private int rent_Days;
        private DateTime returned_Date;
        private float payment;
        private string payment_Type;
        private float price_For_Day;
        private int km_For_Day;
        private double rent_Distance;
        private double return_Distance;
        private string quantity_Fuel_Rent;
        private string quantity_Fuel_Returned;
        private string car_Status;
        private String return_Car_Status;
        private string iD_Employee;
        private string iD_Customer;
        private string car_Number;

        public Rent_Details(int rental_IDB, DateTime rent_DateB, int rent_DaysB, DateTime returned_DateB, float paymentB, string payment_TypeB, float price_For_DayB, int km_For_DayB, double rent_DistanceB, string quantity_Fuel_RentB, string car_StatusB, string iD_EmployeeB, string iD_CustomerB, string car_NumberB)
        {
            rental_ID = rental_IDB;
            rent_Date = rent_DateB;
            rent_Days = rent_DaysB;
            returned_Date = returned_DateB;
            payment = paymentB;
            payment_Type = payment_TypeB;
            price_For_Day = price_For_DayB;
            km_For_Day = km_For_DayB;
            rent_Distance = rent_DistanceB;
            quantity_Fuel_Rent = quantity_Fuel_RentB;
            car_Status = car_StatusB;
            iD_Employee = iD_EmployeeB;
            iD_Customer = iD_CustomerB;
            car_Number = car_NumberB;
        }
        public Rent_Details(int rental_IDB, int rent_DaysB, DateTime returned_DateB, float paymentB, string payment_TypeB, double return_DistanceB, string quantity_Fuel_ReturnedB, string return_Car_StatusB, string iD_EmployeeB, string car_NumberB)
        {
            rental_ID = rental_IDB;
            rent_Days = rent_DaysB;
            returned_Date = returned_DateB;
            payment = paymentB;
            payment_Type = payment_TypeB;
            return_Distance = return_DistanceB;
            quantity_Fuel_Returned = quantity_Fuel_ReturnedB;
            return_Car_Status = return_Car_StatusB;
            iD_Employee = iD_EmployeeB;
            car_Number = car_NumberB;
        }
        public Rent_Details(int rental_IDB, string car_NumberB, string quantity_Fuel_ReturnedB,double return_DistanceB, float paymentB,float price_For_DayB)
        {
            rental_ID = rental_IDB;
            car_Number = car_NumberB;
            quantity_Fuel_Returned = quantity_Fuel_ReturnedB;
            return_Distance = return_DistanceB;
            payment = paymentB;
            price_For_Day = price_For_DayB;
        }
        /*getters and setters*/
        public int Rental_ID
        {
            get
            {
                return rental_ID;
            }
            set
            {
                rental_ID = value;
            }
        }
        public DateTime Rent_Date
        {
            get
            {
                return rent_Date;
            }
            set
            {
                rent_Date = value;
            }
        }
        public DateTime Returned_Date
        {
            get
            {
                return returned_Date;
            }
            set
            {
                returned_Date = value;
            }
        }
        public int Rent_Days
        {
            get
            {
                return rent_Days;
            }
            set
            {
                rent_Days = value;
            }
        }
        public float Payment
        {
            get
            {
                return payment;
            }
            set
            {
                payment = value;
            }
        }
        public string Payment_Type
        {
            get
            {
                return payment_Type;
            }
            set
            {
                payment_Type = value;
            }
        }
        public double Rent_Distance
        {
            get
            {
                return rent_Distance;
            }
            set
            {
                rent_Distance = value;
            }
        }
        public double Return_Distance
        {
            get
            {
                return return_Distance;
            }
            set
            {
                return_Distance = value;
            }
        }
        public string Quantity_Fuel_Rent
        {
            get
            {
                return quantity_Fuel_Rent;
            }
            set
            {
                quantity_Fuel_Rent = value;
            }
        }
        public string Quantity_Fuel_Returned
        {
            get
            {
                return quantity_Fuel_Returned;
            }
            set
            {
                quantity_Fuel_Returned = value;
            }
        }
        public string Car_Status
        {
            get
            {
                return car_Status;
            }
            set
            {
                car_Status = value;
            }
        }
        public string ID_Employee
        {
            get
            {
                return iD_Employee;
            }
            set
            {
                iD_Employee = value;
            }
        }
        public string ID_Customer
        {
            get
            {
                return iD_Customer;
            }
            set
            {
                iD_Customer = value;
            }
        }
        public string Car_Number
        {
            get
            {
                return car_Number;
            }
            set
            {
                car_Number = value;
            }
        }

        public float Price_For_Day
        {
            get
            {
                return price_For_Day;
            }
            set
            {
                price_For_Day = value;
            }
        }
        public int Km_For_Day
        {
            get
            {
                return km_For_Day;
            }
            set
            {
                km_For_Day = value;
            }
        }

        public string Return_Car_Status
        {
            get
            {
                return return_Car_Status;
            }
            set
            {
                return_Car_Status = value;
            }
        }
    }
}
