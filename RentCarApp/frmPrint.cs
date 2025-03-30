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
    public partial class frmPrint : Form
    {
        public string PrinterName = "";
        public frmPrint()
        {
            InitializeComponent();
        }

        private void FrmPrint_Load(object sender, EventArgs e)
        {
            int defaultPrinterIndex = -1;
            System.Drawing.Printing.PrinterSettings ps = new System.Drawing.Printing.PrinterSettings();
            
            foreach (string printername in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                if (ps.PrinterName == printername)//to recognize the default printer
                {
                    defaultPrinterIndex = comboBox1.Items.Count;
                }
                comboBox1.Items.Add(printername);
            }
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.SelectedIndex = defaultPrinterIndex;
            }
        }

        private void PrintBtn_Click(object sender, EventArgs e)
        {
            PrinterName = comboBox1.Text;
        }
    }
}
