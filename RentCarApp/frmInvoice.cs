using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Font = iTextSharp.text.Font;

namespace RentCarApp
{
    public partial class frmInvoice : Form
    {
        private DbManager dbManager;
        private AddRent frmAddRent;
        private InvoiceDetails[] invoiceDetails;
        private TaxInvoiceCheck[] taxInvoiceChecks;
        private TaxInvoice taxInvoice;
        private float fuelPay = 0;
        private double distancePay = 0;
        private float rentPaymentAmount = 0;
        private Rent_Details rent_Details;
        private Document doc;
        private PdfPTable myTable;
        private BaseFont tableFont1 = BaseFont.CreateFont(@"C:\Windows\Fonts\GISHA.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        private Font tableFont = null;
        private Font tableFontBold = null;
        private Font tableFontUnderline = null;

        public enum InvoiceType
        {
            Add,
            AddNotReturn,
            Return,
            View
        }
        InvoiceType invoiceType;
        public frmInvoice(InvoiceType mode, AddRent addRent, Rent_Details BrentDetails, User driver)
        {
            InitializeComponent();
            dbManager = new DbManager();
            invoiceType = mode;
            frmAddRent = addRent;
            fuelPay += float.Parse(BrentDetails.Quantity_Fuel_Returned) * 25;
            distancePay += BrentDetails.Return_Distance;
            rentPaymentAmount += BrentDetails.Payment - fuelPay - float.Parse(distancePay.ToString());//כמה לשלם
            rentIdTxt.Text = BrentDetails.Rental_ID.ToString();
            carNumTB.Text = BrentDetails.Car_Number;
            billToTB.Text = driver.First_name + " " + driver.Last_name;
            addressTB.Text = driver.Address;
            rent_Details = BrentDetails;
            invoiceDateTime.Value = DateTime.Now;
            checkDate.MinDate = DateTime.Now.AddMonths(-3);
            checkDate.MaxDate = DateTime.Now.AddMonths(3);
            checkDate.Value = DateTime.Now;
            saveDocumentBtn.Enabled = false;
            printBtn.Enabled = false;
            tableFont = new Font(tableFont1, 12);
            tableFontBold = new Font(tableFont1, 14, (int)FontStyle.Bold);
            tableFontUnderline = new Font(tableFont1, 12, (int)FontStyle.Underline);
        }
        public frmInvoice(InvoiceType mode, int BinvoiceID)//view
        {
            InitializeComponent();
            dbManager = new DbManager();
            invoiceType = mode;
            taxInvoice = dbManager.TaxInvoice(BinvoiceID);
            tableFont = new Font(tableFont1, 12);
            tableFontBold = new Font(tableFont1, 14, (int)FontStyle.Bold);
            tableFontUnderline = new Font(tableFont1, 12, (int)FontStyle.Underline);
        }

        private void FrmReceipt_Load(object sender, EventArgs e)
        {
            //invoiceInfoDGV.Rows.Add(0, "ריק", 0, 0);
            if (invoiceType != InvoiceType.View && (dbManager.ReadInvoice_ID() >= 1))
            {
                invoiceIdTB.Text = dbManager.ReadInvoice_ID().ToString();
            }
            else if (invoiceType != InvoiceType.View)
            {
                MessageBox.Show("Failed To load try again later or check your internet connection", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            if (invoiceType == InvoiceType.Add)
            {
                invoiceInfoDGV.Rows.Add(rentPaymentAmount / rent_Details.Price_For_Day, "תשלום עבור שכירות מס'" + rent_Details.Rental_ID, rent_Details.Price_For_Day, rentPaymentAmount);
            }
            else if (invoiceType == InvoiceType.Return)
            {
                if (rentPaymentAmount > 0)//אם יש יתרה לשלם עבור ההשכרה
                    invoiceInfoDGV.Rows.Add(rentPaymentAmount / rent_Details.Price_For_Day, "תשלום עבור שכירות מס'" + rent_Details.Rental_ID, rent_Details.Price_For_Day, rentPaymentAmount);
                else
                {
                    invoiceInfoDGV.Rows.Add(0, " ", 0, 0);
                    invoiceInfoDGV.Rows[0].Visible = false;
                }
                if (fuelPay > 0)
                    invoiceInfoDGV.Rows.Add(rent_Details.Quantity_Fuel_Returned, "חריגה בגין בנזין", 25, fuelPay);
                if (distancePay > 0)
                    invoiceInfoDGV.Rows.Add(distancePay, "חריגה בגין ק" + '"' + "מ", 1, distancePay);
            }
            else if (invoiceType == InvoiceType.View)
            {
                TaxInvoiceView();
            }
            Check_Invoice();
        }
        private void TaxInvoiceView()
        {
            DataSet taxInvoiceDetails = dbManager.TaxInvoiceDetails(taxInvoice.InvoiceID);
            if ((taxInvoiceDetails != null) && (taxInvoiceDetails.Tables.Count > 0) && (taxInvoiceDetails.Tables["invoice_details"].Rows.Count > 0))
                invoiceInfoDGV.DataSource = taxInvoiceDetails.Tables["invoice_details"];
            invoiceIdTB.Text = taxInvoice.InvoiceID.ToString();
            rentIdTxt.Text = taxInvoice.RentalID.ToString();
            invoiceDateTime.Value = taxInvoice.Date;
            carNumTB.Text = taxInvoice.CarNumber;
            billToTB.Text = taxInvoice.BillTo;
            addressTB.Text = taxInvoice.Address;
            DataSet taxInvoiceChecks = dbManager.TaxInvoiceChecks(taxInvoice.InvoiceID);
            if ((taxInvoiceChecks != null) && (taxInvoiceChecks.Tables.Count > 0) && (taxInvoiceChecks.Tables["tax_invoice_check"].Rows.Count > 0))
                checkDGV.DataSource = taxInvoiceChecks.Tables["tax_invoice_check"];
            checkDGV.Columns[chequeDate.Name].DefaultCellStyle.Format = "dd/MM/yyyy";
            ChecksAmount();
            cashAmountNum.Value = (decimal)taxInvoice.CashAmount;
            visaAmountNum.Value = (decimal)taxInvoice.VisaAmount;
            vatAmount.Value = (decimal)taxInvoice.Vat;
            Check_Invoice();
            TaxInvoiceEnable();
        }
        private void TaxInvoiceEnable()
        {
            billToTB.Enabled = false;
            carNumTB.Enabled = false;
            addressTB.Enabled = false;
            addItemGB.Enabled = false;
            addCheckGB.Enabled = false;
            cashAmountNum.Enabled = false;
            visaAmountNum.Enabled = false;
            vatAmount.Enabled = false;
            saveDocumentBtn.Enabled = true;
            printBtn.Enabled = true;
            saveBtn.Enabled = false;
        }
        private bool CheckValid()
        {
            if (CheckID_check(checkNumTb.Text) && bankAccountNum.Text.IsDigDouble() && bankInfo.Text != string.Empty)
                return true;
            return false;
        }
        private void ClearCheckInfo()
        {
            checkNumTb.Clear();
            checkDate.Value = DateTime.Now;
        }
        private bool CheckID_check(string checkId)
        {
            string idTemp;
            if (checkId.IsDig())
            {
                foreach (DataGridViewRow row in checkDGV.Rows)
                {
                    idTemp = row.Cells[0].Value.ToString();
                    if (idTemp == checkId)
                        return false;
                }
                return true;
            }
            return false;
        }
        private void AddCheckBtn_Click(object sender, EventArgs e)
        {
            if (checkDGV.Rows.Count < 3) //max 3 checks
            {
                if (CheckValid())
                {
                    checkDGV.Rows.Add(checkNumTb.Text, bankAccountNum.Text, bankInfo.Text, checkDate.Value.Date.ToString("dd/MM/yyyy"), amountCheck.Value);
                    checkDGV.Columns[chequeDate.Name].DefaultCellStyle.Format = "dd/MM/yyyy";
                    ClearCheckInfo();
                    ChecksAmount();
                }
                else
                    MessageBox.Show("Failed To Add Check your information", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Can Not add more than three Checks", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void RemoveCheckBtn_Click(object sender, EventArgs e)
        {
            if (checkDGV.SelectedRows.Count == 1)
            {
                checkDGV.Rows.RemoveAt(checkDGV.SelectedRows[0].Index);
                ChecksAmount();
            }
            else
                MessageBox.Show("Can Not Remove :\nthere is no checks", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void ChecksAmount()
        {
            decimal sum = 0;
            if (checkDGV.Rows.Count > 0)
                for (int i = 0; i < checkDGV.Rows.Count; i++)
                    sum += decimal.Parse(checkDGV.Rows[i].Cells[checkAmountNum.Name].Value.ToString());
            checkSum.Value = sum;
        }

        private void CheckSum_ValueChanged(object sender, EventArgs e)
        {
            //if ((checkSum.Value + cashAmountNum.Value + visaAmountNum.Value) <= total.Value)
            Check_Invoice();
        }

        private void CashAmountNum_ValueChanged(object sender, EventArgs e)
        {
            //if ((checkSum.Value + cashAmountNum.Value + visaAmountNum.Value) <= total.Value)
            Check_Invoice();
        }

        private void VisaAmountNum_ValueChanged(object sender, EventArgs e)
        {
            //if ((checkSum.Value + cashAmountNum.Value + visaAmountNum.Value) <= total.Value)
            Check_Invoice();
        }

        private void VatAmount_ValueChanged(object sender, EventArgs e)
        {
            Check_Invoice();
        }



        ////////////////detailsDGV Events///////
        private void Qty_ValueChanged(object sender, EventArgs e)
        {
            if ((qty.Value * unitPrice.Value) <= amountDescription.Maximum)
                amountDescription.Value = qty.Value * unitPrice.Value;
        }

        private void UnitPrice_ValueChanged(object sender, EventArgs e)
        {
            if ((qty.Value * unitPrice.Value) <= amountDescription.Maximum)
                amountDescription.Value = qty.Value * unitPrice.Value;
        }

        private void AddItem_Click(object sender, EventArgs e)
        {
            if (descriptionTB.Text != string.Empty && amountDescription.Value > 0)
            {
                invoiceInfoDGV.Rows.Add(qty.Value, descriptionTB.Text, unitPrice.Value, amountDescription.Value);
                Check_Invoice();
                Clear_Add_Item();
            }
            else
                MessageBox.Show("Failed To Add Check your information", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void Clear_Add_Item()
        {
            descriptionTB.Clear();
            unitPrice.Value = 0;
            qty.Value = 0;
        }

        private void RemoveItem_Click(object sender, EventArgs e)
        {
            bool rmIndexOne = true;
            bool rmIndexTwo = true;
            if ((fuelPay > 0 || distancePay > 0) && invoiceInfoDGV.SelectedRows[0].Index == 1)
                rmIndexOne = false;
            if (fuelPay > 0 && distancePay > 0 && (invoiceInfoDGV.SelectedRows[0].Index == 1 || invoiceInfoDGV.SelectedRows[0].Index == 2))
                rmIndexTwo = false;
            if (invoiceInfoDGV.SelectedRows[0].Index != 0 && rmIndexOne && rmIndexTwo)
                invoiceInfoDGV.Rows.RemoveAt(invoiceInfoDGV.SelectedRows[0].Index);
            else
                MessageBox.Show("Can Not Remove Rent Details", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Check_Invoice();
        }
        ///////////////////////

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            if (billToTB.Text != string.Empty && carNumTB.Text.IsDigDouble() && addressTB.Text != string.Empty && total.Value > 0)
            {
                Check_Invoice();
                decimal difference = decimal.Parse(balanceDueTxt.Text);
                if (difference != 0)//if the subTotal payed is not equal to how much should pay
                    MessageBox.Show("Information:\n" + (difference > 0 ? "You need to pay more " : "you have to pay less ") + difference + "₪", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (FillTaxInvoiceParameters())//if we fill all parameter and succeed
                {
                    if (dbManager.AddTaxInvoice(taxInvoice) &&//adding the tax id and other info
                        dbManager.AddInvoiceDetails(invoiceDetails) &&//adding all items for specific invoice id in TaxInvoice table
                        dbManager.UpdateRentDetailPayed(invoiceDetails[0].AmountDescription + fuelPay + (float)distancePay, taxInvoice.RentalID))//update the payed paymentSum in rentRetail table
                    {
                        bool flag = dbManager.AddChecksDetails(taxInvoiceChecks);//if checks is not empty flag == true else false
                        if (taxInvoiceChecks != null && flag == false)//if there is error with internet connection
                            MessageBox.Show("Information:\nUnSucceed Check your internet connection", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        else if (taxInvoiceChecks == null || flag == true)//if succeed -> closing the form
                        {
                            frmAddRent.AddTaxInvoice(invoiceDetails[0].AmountDescription + fuelPay + (float)distancePay);//, taxInvoice.InvoiceID, taxInvoice.Date.ToString("dd/MM/yyyy"), taxInvoice.Total);
                            invoiceType = InvoiceType.View;
                            TaxInvoiceView();

                            //this.Close();
                        }
                    }
                    else
                        MessageBox.Show("Information:\nUnSucceed Check your internet connection Or Add Items", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
                MessageBox.Show("Information:\nAdd Payment methods and check your details", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void Check_Invoice()
        {
            decimal sumTotal = 0;
            for (int i = 0; i < invoiceInfoDGV.Rows.Count; i++)
                sumTotal += decimal.Parse(invoiceInfoDGV.Rows[i].Cells[descAmountColDGV.Name].Value.ToString());
            subTotalNum.Value = sumTotal;
            total.Value = sumTotal * (vatAmount.Value / 100) + sumTotal;
            balanceDueTxt.Text = (total.Value - cashAmountNum.Value - visaAmountNum.Value - checkSum.Value).ToString("n1");
            //return total.Value - cashAmountNum.Value - visaAmountNum.Value - checkSum.Value;//the difference in payment and the payed money
        }
        private bool FillTaxInvoiceParameters()
        {
            taxInvoice = new TaxInvoice(int.Parse(invoiceIdTB.Text),
                int.Parse(rentIdTxt.Text),
                carNumTB.Text,
                billToTB.Text,
                invoiceDateTime.Value.Date,
                addressTB.Text,
                (float)cashAmountNum.Value,
                (float)checkSum.Value,
                (float)visaAmountNum.Value,
                (float)subTotalNum.Value,
                (float)vatAmount.Value,
                (float)total.Value);
            if (checkDGV.Rows != null && checkDGV.Rows.Count != 0)
            {
                taxInvoiceChecks = new TaxInvoiceCheck[checkDGV.Rows.Count];
                for (int i = 0; i < checkDGV.Rows.Count; i++)
                {
                    DateTime checkDate;
                    // if(invoiceType==InvoiceType.View)
                    checkDate = DateTime.Parse(checkDGV.Rows[i].Cells[chequeDate.Name].Value.ToString());// Convert.ToDateTime(checkDGV.Rows[i].Cells[chequeDate.Name].Value.ToString());//
                    /*else 
                        checkDate= checkDate = DateTime.ParseExact(checkDGV.Rows[i].Cells[chequeDate.Name].Value.ToString(), "dd/MM/yyyy", null);*/
                    taxInvoiceChecks[i] = new TaxInvoiceCheck(int.Parse(invoiceIdTB.Text),
                                                              checkDGV.Rows[i].Cells[checkNO.Name].Value.ToString(),
                                                              checkDGV.Rows[i].Cells[accountNO.Name].Value.ToString(),
                                                              checkDGV.Rows[i].Cells[routingNO.Name].Value.ToString(),
                                                              checkDate.Date, float.Parse(checkDGV.Rows[i].Cells[checkAmountNum.Name].Value.ToString()));
                }
            }
            invoiceDetails = new InvoiceDetails[invoiceInfoDGV.Rows.Count];
            for (int i = 0; i < invoiceInfoDGV.Rows.Count; i++)
                invoiceDetails[i] = new InvoiceDetails(int.Parse(invoiceIdTB.Text),
                                                        int.Parse(invoiceInfoDGV.Rows[i].Cells[qtyColDGV.Name].Value.ToString()),
                                                        invoiceInfoDGV.Rows[i].Cells[detailsColDGV.Name].Value.ToString(),
                                                        float.Parse(invoiceInfoDGV.Rows[i].Cells[unitPriceColDGV.Name].Value.ToString()),
                                                        float.Parse(invoiceInfoDGV.Rows[i].Cells[descAmountColDGV.Name].Value.ToString()));
            if (taxInvoice != null && invoiceDetails != null)
                return true;
            return false;
        }

        private void BalanceDueTxt_TextChanged(object sender, EventArgs e)
        {
            if (decimal.Parse(balanceDueTxt.Text) <= 0)
                balanceDueTxt.BackColor = System.Drawing.Color.LightGreen;
            else
                balanceDueTxt.BackColor = System.Drawing.Color.LightCoral;
        }

        private void FrmInvoice_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (decimal.Parse(balanceDueTxt.Text) == 0)//|| rentPaymentAmount > 0 || fuelPay > 0 || distancePay > 0)
                e.Cancel = false;
            else
                e.Cancel = true;
        }

        private void Qty_Validating(object sender, CancelEventArgs e)
        {
            (sender as NumericUpDown).NumericUpDown_Validating();
        }

        private void SaveDocumentBtn_Click(object sender, EventArgs e)
        {
            saveReportPdf.Filter = "PDF Files|*.pdf";
            saveReportPdf.FileName = "חשבונית מס'" + invoiceIdTB.Text;
            try
            {
                if (saveReportPdf.ShowDialog() == DialogResult.OK)
                {
                    doc = new Document();
                    string nameFile = saveReportPdf.FileName;
                    PdfWriter.GetInstance(doc, new FileStream(nameFile, FileMode.Create));
                    doc.Open();
                    FillTaxInvoiceParameters();
                    CreatePdfInvoice();
                    doc.Close();
                }
            }
            catch (IOException IO)
            {
                MessageBox.Show(IO.Message, "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void CreatePdfInvoice()
        {
            myTable = new PdfPTable(2);
            myTable.DefaultCell.BorderWidth = 0;
            myTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            float[] widthOfTable = new float[2];
            widthOfTable[0] = 100f / 3;
            widthOfTable[1] = 100f;
            myTable.SetWidths(widthOfTable);
            myTable.WidthPercentage = 100;
            myTable.AddCell(new Phrase("חשבונית מס/קבלה מס'" + ' ' + invoiceIdTB.Text, tableFontBold));
            myTable.AddCell(new Phrase("שכירות מס'" + ' ' + rentIdTxt.Text, tableFontBold));
            doc.Add(myTable);
            CreatePdfInvoiceCustomer1();
        }
        private void CreatePdfInvoiceCustomer1()
        {
            PdfCellEmpty(3);
            myTable = new PdfPTable(4);
            myTable.DefaultCell.BorderWidth = 0;
            myTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            float[] widthOfTable = new float[4];
            widthOfTable[3] = 4f;
            widthOfTable[2] = 71f;
            widthOfTable[1] = 11f;
            widthOfTable[0] = 14f;
            myTable.SetWidths(widthOfTable);
            myTable.WidthPercentage = 100;
            myTable.AddCell(new Phrase("לכ'", tableFont));
            myTable.AddCell(new Phrase(taxInvoice.BillTo, tableFontUnderline));
            myTable.AddCell(new Phrase("מכונית מס'", tableFont));
            myTable.AddCell(new Phrase(taxInvoice.CarNumber, tableFontUnderline));
            doc.Add(myTable);
            CreatePdfInvoiceCustomer2();
        }
        private void CreatePdfInvoiceCustomer2()
        {
            PdfCellEmpty(1);
            myTable = new PdfPTable(2);
            myTable.DefaultCell.BorderWidth = 0;
            myTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            float[] widthOfTable = new float[2];
            widthOfTable[1] = 7f;
            widthOfTable[0] = 93f;
            myTable.SetWidths(widthOfTable);
            myTable.WidthPercentage = 100;
            myTable.AddCell(new Phrase("כתובת", tableFont));
            myTable.AddCell(new Phrase(taxInvoice.Address, tableFontUnderline));
            doc.Add(myTable);
            CreatePdfInvoiceDetails();
        }
        private void CreatePdfInvoiceDetails()
        {
            PdfCellEmpty(3);
            myTable = new PdfPTable(4);
            float[] widthOfTable = { 20f, 20f, 60f, 20f };
            myTable.SetWidths(widthOfTable);
            myTable.WidthPercentage = 100;
            myTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            for (int i = 1; i < 5; i++)
                myTable.AddCell(new PdfPCell(new Phrase(invoiceInfoDGV.Columns[i].HeaderText, tableFontBold)) { VerticalAlignment = Element.ALIGN_CENTER, HorizontalAlignment = Element.ALIGN_CENTER });
            for (int i = 0; i < invoiceDetails.Length; i++)
            {
                myTable.AddCell(new PdfPCell(new Phrase(invoiceDetails[i].Qty.ToString(), tableFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_CENTER });
                myTable.AddCell(new PdfPCell(new Phrase(invoiceDetails[i].Description.ToString(), tableFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_CENTER });
                myTable.AddCell(new PdfPCell(new Phrase(invoiceDetails[i].UnitPrice.ToString() + "₪", tableFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_CENTER });
                myTable.AddCell(new PdfPCell(new Phrase(invoiceDetails[i].AmountDescription.ToString() + "₪", tableFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_CENTER });
            }
            doc.Add(myTable);
            CreatePdfInvoiceDetailsPayment();
        }
        private void CreatePdfInvoiceDetailsPayment()
        {
            myTable = new PdfPTable(4);
            float[] widthOfTable = { 20f, 20f, 60f, 20f };
            myTable.SetWidths(widthOfTable);
            myTable.WidthPercentage = 100;
            myTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            ////subtotal
            myTable.AddCell(new PdfPCell(new Phrase(" ", tableFont)) { BorderWidth = 0 });
            myTable.AddCell(new PdfPCell(new Phrase(" ", tableFont)) { BorderWidth = 0 });
            myTable.AddCell(new PdfPCell(new Phrase("סה" + '"' + "כ", tableFont)) { BorderWidth = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_CENTER });
            myTable.AddCell(new PdfPCell(new Phrase(taxInvoice.SubTotal.ToString() + "₪", tableFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_CENTER });
            ////vat
            myTable.AddCell(new PdfPCell(new Phrase(" ", tableFont)) { BorderWidth = 0 });
            myTable.AddCell(new PdfPCell(new Phrase(" ", tableFont)) { BorderWidth = 0 });
            myTable.AddCell(new PdfPCell(new Phrase("מ.ע.מ " + taxInvoice.Vat.ToString() + '%', tableFont)) { BorderWidth = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_CENTER });
            myTable.AddCell(new PdfPCell(new Phrase((taxInvoice.Vat / 100 * taxInvoice.SubTotal).ToString() + "₪", tableFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_CENTER });
            ////total
            myTable.AddCell(new PdfPCell(new Phrase(" ", tableFont)) { BorderWidth = 0 });
            myTable.AddCell(new PdfPCell(new Phrase(" ", tableFont)) { BorderWidth = 0 });
            myTable.AddCell(new PdfPCell(new Phrase("סה" + '"' + "כ" + " כללי", tableFont)) { BorderWidth = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_CENTER });
            myTable.AddCell(new PdfPCell(new Phrase(taxInvoice.Total.ToString() + "₪", tableFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_CENTER });
            doc.Add(myTable);
            CreatePdfInvoiceCheck();
        }
        private void CreatePdfInvoiceCheck()
        {//taxInvoice taxInvoiceChecks invoiceDetails
            PdfCellEmpty(3);
            myTable = new PdfPTable(5);
            float[] widthOfTable = { 12f, 14f, 48f, 14f, 12f };
            myTable.SetWidths(widthOfTable);
            myTable.WidthPercentage = 100;
            myTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            for (int i = 0; i < 5; i++)
                myTable.AddCell(new PdfPCell(new Phrase(checkDGV.Columns[i].HeaderText, tableFontBold)) { VerticalAlignment = Element.ALIGN_CENTER, HorizontalAlignment = Element.ALIGN_CENTER });
            for (int i = 0; i < 3; i++)
            {
                myTable.AddCell(new PdfPCell(new Phrase(taxInvoiceChecks != null && i < taxInvoiceChecks.Length ? taxInvoiceChecks[i].CheckNO.ToString() : " ", tableFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_CENTER });
                myTable.AddCell(new PdfPCell(new Phrase(taxInvoiceChecks != null && i < taxInvoiceChecks.Length ? taxInvoiceChecks[i].AccountNO.ToString() : " ", tableFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_CENTER });
                myTable.AddCell(new PdfPCell(new Phrase(taxInvoiceChecks != null && i < taxInvoiceChecks.Length ? taxInvoiceChecks[i].RoutingNO.ToString() : " ", tableFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_CENTER });
                myTable.AddCell(new PdfPCell(new Phrase(taxInvoiceChecks != null && i < taxInvoiceChecks.Length ? taxInvoiceChecks[i].CheckDate.Date.ToString("dd/MM/yyyy") : " ", tableFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_CENTER });
                myTable.AddCell(new PdfPCell(new Phrase(taxInvoiceChecks != null && i < taxInvoiceChecks.Length ? taxInvoiceChecks[i].CheckAmount.ToString() + "₪" : " ", tableFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_CENTER });
            }
            doc.Add(myTable);
            CreatePdfInvoiceDetailsAllPayment();
        }
        private void CreatePdfInvoiceDetailsAllPayment()
        {
            myTable = new PdfPTable(5);
            float[] widthOfTable = { 12f, 14f, 50f, 12f, 12f };
            myTable.SetWidths(widthOfTable);
            myTable.WidthPercentage = 100;
            myTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            ////checks payment
            myTable.AddCell(new PdfPCell(new Phrase(" ", tableFont)) { BorderWidth = 0 });
            myTable.AddCell(new PdfPCell(new Phrase(" ", tableFont)) { BorderWidth = 0 });
            myTable.AddCell(new PdfPCell(new Phrase(" ", tableFont)) { BorderWidth = 0 });
            myTable.AddCell(new PdfPCell(new Phrase("סה" + '"' + "כ" + " שיקים", tableFont)) { BorderWidth = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_CENTER });
            myTable.AddCell(new PdfPCell(new Phrase(taxInvoice.ChecksAmount.ToString() + "₪", tableFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_CENTER });
            ////cash payment
            myTable.AddCell(new PdfPCell(new Phrase(" ", tableFont)) { BorderWidth = 0 });
            myTable.AddCell(new PdfPCell(new Phrase(" ", tableFont)) { BorderWidth = 0 });
            myTable.AddCell(new PdfPCell(new Phrase(" ", tableFont)) { BorderWidth = 0 });
            myTable.AddCell(new PdfPCell(new Phrase("מזומן", tableFont)) { BorderWidth = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_CENTER });
            myTable.AddCell(new PdfPCell(new Phrase(taxInvoice.CashAmount.ToString() + "₪", tableFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_CENTER });
            ////visa payment
            myTable.AddCell(new PdfPCell(new Phrase(" ", tableFont)) { BorderWidth = 0 });
            myTable.AddCell(new PdfPCell(new Phrase(" ", tableFont)) { BorderWidth = 0 });
            myTable.AddCell(new PdfPCell(new Phrase(" ", tableFont)) { BorderWidth = 0 });
            myTable.AddCell(new PdfPCell(new Phrase("כ. אשראי", tableFont)) { BorderWidth = 0, HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_CENTER });
            myTable.AddCell(new PdfPCell(new Phrase(taxInvoice.VisaAmount.ToString() + "₪", tableFont)) { HorizontalAlignment = Element.ALIGN_CENTER, VerticalAlignment = Element.ALIGN_CENTER });
            doc.Add(myTable);
            CreatePdfInvoice2();
        }
        private void CreatePdfInvoice2()
        {
            PdfCellEmpty(3);
            myTable = new PdfPTable(4);
            myTable.DefaultCell.BorderWidth = 0;
            myTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            float[] widthOfTable = new float[4];
            widthOfTable[3] = 7f;
            widthOfTable[2] = 56f;
            widthOfTable[1] = 24f;
            widthOfTable[0] = 13f;
            myTable.SetWidths(widthOfTable);
            myTable.WidthPercentage = 100;
            myTable.AddCell(new Phrase("תאריך", tableFont));
            myTable.AddCell(new Phrase(taxInvoice.Date.Date.ToString("dd/MM/yyyy"), tableFontUnderline));
            myTable.AddCell(new Phrase("חתימה", tableFont));
            myTable.AddCell(new Phrase(new Chunk(new iTextSharp.text.pdf.draw.LineSeparator(0.8F, 180f, BaseColor.BLACK, Element.ALIGN_BASELINE, 0))));
            doc.Add(myTable);

        }
        private void PdfCellEmpty(int rowNum)
        {
            myTable = new PdfPTable(1);
            myTable.DefaultCell.BorderWidth = 0;
            for (int i = 0; i < rowNum; i++)
                myTable.AddCell(" ");
            doc.Add(myTable);
        }

        private void PrintBtn_Click(object sender, EventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\RentCarAppLast desktop\\temp\\toPrint.pdf";

            frmPrint printForm = new frmPrint();
            try
            {
                doc = new Document();
                PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));
                doc.Open();
                FillTaxInvoiceParameters();
                CreatePdfInvoice();
                doc.Close();
                DialogResult dr = printForm.ShowDialog();
                if (dr == DialogResult.OK && printForm.PrinterName != string.Empty)
                {
                    RawPrint.IPrinter printer = new RawPrint.Printer();

                    printer.PrintRawFile(printForm.PrinterName, path, Path.GetFileName(path));
                }
            }
            catch (IOException IO)
            {
                MessageBox.Show(IO.Message, "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                printForm.Dispose();
            }
        }
    }
}
