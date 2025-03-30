using System;

namespace RentCarApp
{
    /*מחלקה לשמרת פרטי נהג*/
    class Driver_Licensing : User
    {
        /*user נתונים של נהג ש צריך לשמור במחלקה הזוונהג יורש מ*/
        private DateTime expire_date;
        private DateTime date_of_issue;
        public Driver_Licensing(string idB, string first_nameB, string last_nameB, string addressB, DateTime birth_dateB, string phone_numberB, string telephone_numberB, DateTime expire_dateB, DateTime date_of_issueB) : base(idB, first_nameB, last_nameB, addressB, birth_dateB, phone_numberB, telephone_numberB)
        {
            expire_date = expire_dateB;
            date_of_issue = date_of_issueB;
        }
        /* getters and setters*/
        public DateTime Expire_date
        {
            get
            {
                return expire_date;
            }
            set
            {
                if (value != null && date_of_issue != null && value.Date.CompareTo(date_of_issue.Date) > 0)
                    expire_date = value;
            }
        }
        public DateTime Date_of_issue
        {
            get
            {
                return date_of_issue;
            }
            set
            {
                if (value != null && expire_date != null && value.Date.CompareTo(expire_date.Date) < 0)/**/
                    date_of_issue = value;
            }
        }
    }
}
