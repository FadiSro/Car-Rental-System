using System;
using System.Linq;

namespace RentCarApp
{
    class Car
    {/*מחלקה לשמרת פרטי רכב*/
        /*נתונים של רכב ש צריך לשמור*/
        private string car_Number;
        private string car_Manufacturer;
        private string car_Model;
        private int engine_Capacity;
        private DateTime licensing_Expire_Date;
        private DateTime insurance_Expire_Date;
        private int production_Year;
        private int doors;
        private int seats;
        private string gearbox_Type;
        private string color_Car;
        private double distance;
        private string fuel_Type;
        private string fuel_Capacity;
        private float price_For_Day;
        private bool car_Active;
        private bool car_Availability;
        private string car_Status;

        public Car(string car_NumberB, string car_ManufacturerB, string car_ModelB, int engine_CapacityB, DateTime licensing_Expire_DateB, DateTime insurance_Expire_DateB, int production_YearB, int doorsB, int seatsB, string gearbox_TypeB, string color_CarB, double distanceB, string fuel_TypeB, string fuel_CapacityB, float price_For_DayB,string car_StatusB)
        {
            car_Number = car_NumberB;
            car_Manufacturer = car_ManufacturerB;
            car_Model = car_ModelB;
            engine_Capacity = engine_CapacityB;
            licensing_Expire_Date = licensing_Expire_DateB;
            insurance_Expire_Date = insurance_Expire_DateB;
            production_Year = production_YearB;
            doors = doorsB;
            seats = seatsB;
            gearbox_Type = gearbox_TypeB;
            color_Car = color_CarB;
            distance = distanceB;
            fuel_Type = fuel_TypeB;
            fuel_Capacity = fuel_CapacityB;
            price_For_Day = price_For_DayB;
            car_Availability = true;
            car_Status = car_StatusB;

        }
        /*לכל המשתנים get set */
        public string Car_Number
        {
            get
            {
                return car_Number;
            }
            set
            {
                if (value != string.Empty && value.All(char.IsDigit) && value.Length >= 6 && value.Length <= 8)
                    car_Number = value;
            }
        }
        public string Car_Manufacturer
        {
            get
            {
                return car_Manufacturer;
            }
            set
            {
                if (value != string.Empty && value.All(char.IsLetter) && value.Length < 20)
                    car_Manufacturer = value;
            }
        }
        public string Car_Model
        {
            get
            {
                return car_Model;
            }
            set
            {
                if (value != string.Empty && value.All(char.IsLetterOrDigit) && value.Length < 10)
                    car_Model = value;
            }
        }
        public int Engine_Capacity
        {
            get
            {
                return engine_Capacity;
            }
            set
            {
                if (value >= 900 && value <= 1600)//ask
                    engine_Capacity = value;
            }
        }
        public DateTime Licensing_Expire_Date
        {
            get
            {
                return licensing_Expire_Date;
            }
            set
            {
                if (value != null && value.Date.CompareTo(DateTime.Now.Date) > 0)
                    licensing_Expire_Date = value;
            }
        }
        public DateTime Insurance_Expire_Date
        {
            get
            {
                return insurance_Expire_Date;
            }
            set
            {
                if (value != null && value.Date.CompareTo(DateTime.Now.Date) > 0)
                    insurance_Expire_Date = value;
            }
        }
        public int Doors
        {
            get
            {
                return doors;
            }
            set
            {
                if (value >= 3 && value <= 5)
                    doors = value;
            }
        }
        public int Seats
        {
            get
            {
                return seats;
            }
            set
            {
                if (value >= 5 && value <= 9)
                    seats = value;
            }
        }
        public string Gearbox_Type
        {
            get
            {
                return gearbox_Type;
            }
            set
            {
                if (value != string.Empty && value.All(char.IsLetter) && value.Length < 10)
                    gearbox_Type = value;
            }
        }
        public string Color_Car
        {
            get
            {
                return color_Car;
            }
            set
            {
                if (value != string.Empty)
                    color_Car = value;
            }
        }
        public double Distance
        {
            get
            {
                return distance;
            }
            set
            {
                if (value > 0 && value > distance)
                    distance = value;
            }
        }
        public string Fuel_Type
        {
            get
            {
                return fuel_Type;
            }
            set
            {
                if (value != string.Empty && value.Length < 10 && value.All(char.IsLetter))
                    fuel_Type = value;
            }
        }
        public string Fuel_Capacity
        {
            get
            {
                return fuel_Capacity;
            }
            set
            {
                if (value != string.Empty)
                    fuel_Capacity = value;
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
                if (value > 0)
                    price_For_Day = value;
            }
        }
        public bool Car_Availability
        {
            get
            {
                return car_Availability;
            }
            set
            {
                car_Availability = value;
            }
        }

        public int Production_Year
        {
            get
            {
                return production_Year;
            }
            set
            {
                production_Year = value;
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

        public bool Car_Active
        {
            get
            {
                return car_Active;
            }
            set
            {
                car_Active = value;
            }
        }
    }
}

