using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentCarApp
{
    class InvoiceDetails
    {
        private int invoiceID;
        private int qty;
        private string description;
        private float unitPrice;
        private float amountDescription;
        public InvoiceDetails(int BinvoiceID,int Bqty,string Bdescription,float BunitPrice,float BamountDescription)
        {
            invoiceID = BinvoiceID;
            qty = Bqty;
            description = Bdescription;
            unitPrice = BunitPrice;
            amountDescription = BamountDescription;
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
        public int Qty
        {
            get
            {
                return qty;
            }
            set
            {
                    qty= value;
            }
        }
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                if(value!=string.Empty)
                description= value;
            }
        }
        public float UnitPrice
        {
            get
            {
                return unitPrice;
            }
            set
            {
                unitPrice= value;
            }
        }
        public float AmountDescription
        {
            get
            {
                return amountDescription;
            }
            set
            {
                amountDescription= value;
            }
        }
    }
}
