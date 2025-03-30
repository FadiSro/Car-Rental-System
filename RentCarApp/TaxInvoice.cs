using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarApp
{
    class TaxInvoice 
    {
        private int invoiceID;
        private int rentalID;
        private string carNumber;
        private string billTo;
        private DateTime date;
        private string address;
        private float cashAmount;
        private float checksAmount;
        private float visaAmount;
        private float subTotal;
        private float vat;
        private float total;
        public TaxInvoice(int BinvoiceID,int BrentalID,string BcarNumber,string BbillTo,DateTime Bdate,string Baddress,float BcashAmount,float BchecksAmount,float BvisaAmount,float BsubTotal,float Bvat,float Btotal)
        {
            invoiceID = BinvoiceID;
            rentalID = BrentalID;
            carNumber = BcarNumber;
            billTo = BbillTo;
            date = Bdate;
            address = Baddress;
            cashAmount = BcashAmount;
            checksAmount = BchecksAmount;
            visaAmount = BvisaAmount;
            subTotal = BsubTotal;
            vat = Bvat;
            total = Btotal;
        }
        public TaxInvoice(int BinvoiceID)
        {
            invoiceID = BinvoiceID;
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
        public int RentalID
        {
            get
            {
                return rentalID;
            }
            set
            {
                rentalID = value;
            }
        }
        public string CarNumber
        {
            get
            {
                return carNumber;
            }
            set
            {
                carNumber = value;
            }
        }
        public string BillTo
        {
            get
            {
                return billTo;
            }
            set
            {
                if(value!=string.Empty)
                billTo = value;
            }
        }
        public DateTime Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
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
                if (value != string.Empty)
                    address = value;
            }
        }
        public float CashAmount
        {
            get
            {
                return cashAmount;
            }
            set
            {
                cashAmount = value;
            }
        }
        public float ChecksAmount
        {
            get
            {
                return checksAmount;
            }
            set
            {
                checksAmount = value;
            }
        }
        public float VisaAmount
        {
            get
            {
                return visaAmount;
            }
            set
            {
                visaAmount = value;
            }
        }
        public float SubTotal
        {
            get
            {
                return subTotal;
            }
            set
            {
                subTotal = value;
            }
        }
        public float Vat
        {
            get
            {
                return vat;
            }
            set
            {
                vat = value;
            }
        }
        public float Total
        {
            get
            {
                return total;
            }
            set
            {
                total = value;
            }
        }
    }                     
}                         
                          
                          
                          
                          
                          
                          