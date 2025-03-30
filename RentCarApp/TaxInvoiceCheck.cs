using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarApp
{
    class TaxInvoiceCheck
    {
        private int invoiceID;
        private string checkNO;
        private string accountNO;
        private string routingNO;
        private DateTime checkDate;
        private float checkAmount;
        public TaxInvoiceCheck(int BinvoiceID,string BcheckNO,string BaccountNO,string BroutingNO,DateTime BcheckDate,float BcheckAmount)
        {
            invoiceID = BinvoiceID;
            checkNO = BcheckNO;
            accountNO = BaccountNO;
            routingNO = BroutingNO;
            checkDate = BcheckDate;
            checkAmount = BcheckAmount;
        }
        public int InvoiceID
        {
            get
            {
                return invoiceID;
            }
            set
            {
                invoiceID = value;
            }
        }
        public string CheckNO
        {
            get
            {
                return checkNO;
            }
            set
            {
                if(value!=string.Empty)
                checkNO = value;
            }
        }
        public string AccountNO
        {
            get
            {
                return accountNO;
            }
            set
            {
                if (value != string.Empty)
                    accountNO = value;
            }
        }
        public string RoutingNO
        {
            get
            {
                return routingNO;
            }
            set
            {
                if (value != string.Empty)
                    routingNO = value;
            }
        }
        public DateTime CheckDate
        {
            get
            {
                return checkDate;
            }
            set
            {
                    checkDate = value;
            }
        }
        public float CheckAmount
        {
            get
            {
                return checkAmount;
            }
            set
            {
                checkAmount = value;
            }
        }
    }
}
