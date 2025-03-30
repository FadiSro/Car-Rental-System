using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RentCarApp
{
    public partial class frmReceipt : Form
    {
        public frmReceipt()
        {
            InitializeComponent();
        }

        private void FrmReceipt_Load(object sender, EventArgs e)
        {
            invoiceDateTime.Value = DateTime.Now;
            checkDate.MinDate = DateTime.Now.AddMonths(-3);
            checkDate.Value = DateTime.Now;
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
                MessageBox.Show("Can Not Remove :\n there is no checks", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void ChecksAmount()
        {
            decimal sum = 0;
            if (checkDGV.Rows.Count > 0)
                for (int i = 0; i < checkDGV.Rows.Count; i++)
                    sum += (decimal)checkDGV.Rows[i].Cells[checkAmountNum.Index].Value;
            checkSum.Value = sum;
        }

        private void CheckSum_ValueChanged(object sender, EventArgs e)
        {
            subTotalNum.Value = checkSum.Value + cashAmountNum.Value + visaAmountNum.Value;
        }

        private void CashAmountNum_ValueChanged(object sender, EventArgs e)
        {
            subTotalNum.Value = checkSum.Value + cashAmountNum.Value + visaAmountNum.Value;
        }

        private void VisaAmountNum_ValueChanged(object sender, EventArgs e)
        {
            subTotalNum.Value = checkSum.Value + cashAmountNum.Value + visaAmountNum.Value;
        }

        private void subTotalNum_ValueChanged(object sender, EventArgs e)
        {
            Total.Value = subTotalNum.Value + subTotalNum.Value * (vatAmount.Value / 100);
        }

        private void VatAmount_ValueChanged(object sender, EventArgs e)
        {
            Total.Value = subTotalNum.Value + subTotalNum.Value * (vatAmount.Value / 100);
        }
    }
}
