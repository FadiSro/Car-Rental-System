using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RentCarApp
{
    public partial class frmWacom : Form
    {
        Wacom wacom = null;
        public Bitmap signature = null;
        public frmWacom()
        {
            InitializeComponent();
            this.wacom = new Wacom(this);

        }
        /*פונקציה לניקוי מסך לוח החתימה*/
        private void BtnClear_Click(object sender, EventArgs e)
        {
            wacom.ClearTabletScreen();
            pbSignature.Refresh();
        }
        /*פונקציה לשמירת החתימה */
        private void BtnSave_Click(object sender, EventArgs e)
        {
            signature = wacom.GetSigAsBitmap();
            DialogResult = DialogResult.OK;
            this.Close();
        }
        /*לניקוי מסך לוח החתימה ולסגירת חלון של החתימה*/
        private void BtnCancel_Click(object sender, EventArgs e)
        {
            wacom.ClearTabletScreen();
            DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void FrmWacom_Load(object sender, EventArgs e)
        {
            try
            {
                UpdateSize(wacom.m_capability.screenWidth, wacom.m_capability.screenHeight);
                pbSignature.Paint += pbSignature_Paint;
                this.Disposed += FrmWacom_Disposed;
            }
            catch 
            {
                DialogResult r  = 
                    MessageBox.Show( "Signatrue Pad is not connected. Continue?","Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (r == DialogResult.Yes)
                {
                    DialogResult = DialogResult.OK;
                }
                this.Close();
            }
        }

        private void FrmWacom_Disposed(object sender, EventArgs e)
        {
            try
            {
                if (this.wacom != null)
                {
                    this.wacom.ClearTabletScreen();
                    this.wacom.Disconnect();
                }
            } catch
            {

            }
        }

        private void UpdateSize(ushort screenWidth, ushort screenHeight)
        {
            Size currSize = pbSignature.Size;
            Size newSize = new Size(wacom.m_capability.screenWidth, wacom.m_capability.screenHeight);
            double percentW = (double)newSize.Width / currSize.Width;
            double percentH = (double)newSize.Height / currSize.Height;
            pbSignature.Size = newSize;
            this.Size = new Size((int)(this.Size.Width * percentW), (int)(this.Size.Height * percentH));
        }

        private void pbSignature_Paint(object sender, PaintEventArgs e)
        {
           // List<Point> pnts = new List<Point>();
            //if (wacom.m_penData.Count > 0)
            //{
            //    foreach (var f in wacom.m_penData)
            //    {
            //        pnts.Add(wacom.tabletToScreen(f));
            //        var p = wacom.tabletToScreen(f);
            //        e.Graphics.DrawEllipse(Pens.Black, p.X, p.Y, 1, 1);
            //    }
            //}
            //else
            //{
            //    e.Graphics.Clear(pbSignature.BackColor);
            //}
            var m_penData = wacom.m_penData;
            bool isDown = false;
            PointF prev = new PointF();
            for (int i = 0; i < m_penData.Count; ++i)
            {
                if (m_penData[i].sw != 0)
                {
                    if (!isDown)
                    {
                        isDown = true;
                        prev = wacom.tabletToScreen(m_penData[i]);
                    }
                    else
                    {
                        PointF curr = wacom.tabletToScreen(m_penData[i]);
                        e.Graphics.DrawLine(Pens.Black, prev, curr);
                        prev = curr;
                    }
                }
                else
                {
                    if (isDown)
                    {
                        isDown = false;
                    }
                }
            }



            //var pnts_ = (from f in wacom.m_penData
            //             select wacom.tabletToScreen(f)).ToArray();

            //if (pnts.Count() < 2)
            //{
            //    return;
            //}
            //e.Graphics.DrawLines(Pens.Black, pnts_);

        }
    }
}
