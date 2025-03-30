using System;
using System.Collections.Generic;
using System.Drawing;

namespace RentCarApp
{
    public class Wacom
    {/*מחלקה לשימוש עם לוח החתימה האלקטרוני*/

        public wgssSTU.ICapability m_capability = null;
        public wgssSTU.IInformation m_information = null;
        public wgssSTU.Tablet m_tablet = null;
        frmWacom liveWindow = null;/*משתנה לצפות מה נרשם על המכשיר בשידור ישיר*/
        public List<wgssSTU.IPenData> m_penData = new List<wgssSTU.IPenData>();/*שרטוט*/


        public void Disconnect()
        {
            if (m_tablet != null)
            {
                m_information = null;
                m_capability = null;
                m_tablet.onPenData -= onPenData;
                m_tablet.disconnect();
                m_tablet = null;
                m_penData.Clear();
            }
        }

        public Point tabletToScreen(wgssSTU.IPenData penData)
        {
            if (penData.sw > 0)
            {
                // Screen means LCD screen of the tablet.
                return Point.Round(new PointF(
                (float)penData.x * m_capability.screenWidth / m_capability.tabletMaxX,
                (float)penData.y * m_capability.screenHeight / m_capability.tabletMaxY));
            }
            else
            {
                return Point.Empty;
            }
        }


        void onPenData(wgssSTU.IPenData penData)
        {
            // Process
            //penData.rdy
            // System.Console.WriteLine(tabletToScreen(penData));
            //if (penData.sw > 0)
            {
                m_penData.Add(penData);
            }
            if (m_penData.Count % 10 == 0)
            {
                if (liveWindow != null)
                {
                    liveWindow.Refresh();
                }
            }
        }
        public Wacom(frmWacom liveWindow)
        {
            this.liveWindow = liveWindow;
            var usbDevices = new wgssSTU.UsbDevices();
            if (usbDevices.Count != 0)
            {
                wgssSTU.IUsbDevice usbDevice = usbDevices[0]; //

                m_tablet = new wgssSTU.Tablet();
               wgssSTU.IErrorCode ec = m_tablet.usbConnect(usbDevice, true);
                
                if (ec.value == 0)
                {
                    m_capability = m_tablet.getCapability();
                    m_information = m_tablet.getInformation();
                    m_tablet.setInkingMode(0x01);
                    var rec = new wgssSTU.Rectangle();
                    rec.upperLeftXPixel = 0;
                    rec.upperLeftYPixel = 0;
                    rec.lowerRightXPixel = (ushort)(m_capability.screenWidth - 1);
                    rec.lowerRightYPixel =  (ushort)(m_capability.screenHeight-1);
                    m_tablet.setClearScreenArea(rec);
                    m_tablet.onPenData += new wgssSTU.ITabletEvents2_onPenDataEventHandler(onPenData);
                    
                    //  m_tablet.onGetReportException += new
                    // wgssSTU.ITabletEvents2_onGetReportExceptionEventHandler(onGetReportException);
                }

                /*

                var bitmap = new Bitmap(m_capability.screenWidth, m_capability.screenHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Bitmap b = new Bitmap(@"C:\temp\draw.bmp");

                //Bitmap n = new Bitmap()
                //bitmap = new Bitmap(@"C:\temp\draw.bmp");
               // bitmap.PixelFormat = System.Drawing.Imaging.PixelFormat.Format16bppGrayScale;
                Graphics gfx = Graphics.FromImage(bitmap);
                gfx.Clear(Color.White);
                for (int i = 0; i < b.Width; i++)
                    for (int j = 0; j < b.Height; j++)
                    {
                        bitmap.SetPixel(i, j, b.GetPixel(i, j));
                    }

                // Calculate the encodingMode that will be used to update the image 
                wgssSTU.ProtocolHelper protocolHelper = new wgssSTU.ProtocolHelper();
                wgssSTU.encodingMode m_encodingMode = wgssSTU.encodingMode.EncodingMode_Raw;
                ushort idP = m_tablet.getProductId();
                wgssSTU.encodingFlag encodingFlag = (wgssSTU.encodingFlag)protocolHelper.simulateEncodingFlag(idP, 0);

                bool useColor = false;
                if ((encodingFlag & (wgssSTU.encodingFlag.EncodingFlag_16bit | wgssSTU.encodingFlag.EncodingFlag_24bit)) != 0)
                {
                    if (m_tablet.supportsWrite())
                        useColor = true;
                }
                if ((encodingFlag & wgssSTU.encodingFlag.EncodingFlag_24bit) != 0)
                {
                    m_encodingMode = m_tablet.supportsWrite() ? wgssSTU.encodingMode.EncodingMode_24bit_Bulk : wgssSTU.encodingMode.EncodingMode_24bit;
                }
                else if ((encodingFlag & wgssSTU.encodingFlag.EncodingFlag_16bit) != 0)
                {
                    m_encodingMode = m_tablet.supportsWrite() ? wgssSTU.encodingMode.EncodingMode_16bit_Bulk : wgssSTU.encodingMode.EncodingMode_16bit;
                }
                else
                {     // assumes 1bit is available
                    m_encodingMode = wgssSTU.encodingMode.EncodingMode_1bit;
                }
                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                var bitmapData = (byte[])protocolHelper.resizeAndFlatten(stream.ToArray(), 0, 0, (uint)bitmap.Width, (uint)bitmap.Height, m_capability.screenWidth, m_capability.screenHeight, (byte)m_encodingMode, wgssSTU.Scale.Scale_Fit, 0, 0);

                m_tablet.writeImage((byte)m_encodingMode, bitmapData);*/
            }
        }
        /*public void ClearTabletScreen()
        {
            var rec = new wgssSTU.Rectangle();
            rec.upperLeftXPixel = 0;
            rec.upperLeftYPixel = 0;
            rec.lowerRightXPixel = (ushort)(m_capability.screenWidth - 1);
            rec.lowerRightYPixel = (ushort)(m_capability.screenHeight - 1);
            m_tablet.setClearScreenArea(rec);
            m_penData.Clear();
        }*/
        public void ClearTabletScreen()
        {
            try
            {
                m_penData.Clear();
                var bitmap = new Bitmap(m_capability.screenWidth, m_capability.screenHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                //Bitmap b = new Bitmap(@"C:\temp\draw.bmp");

                Graphics gfx = Graphics.FromImage(bitmap);
                gfx.Clear(Color.White);

                // Calculate the encodingMode that will be used to update the image 
                wgssSTU.ProtocolHelper protocolHelper = new wgssSTU.ProtocolHelper();
                wgssSTU.encodingMode m_encodingMode = wgssSTU.encodingMode.EncodingMode_Raw;
                ushort idP = m_tablet.getProductId();
                wgssSTU.encodingFlag encodingFlag = (wgssSTU.encodingFlag)protocolHelper.simulateEncodingFlag(idP, 0);

                /*bool useColor = false;
                if ((encodingFlag & (wgssSTU.encodingFlag.EncodingFlag_16bit | wgssSTU.encodingFlag.EncodingFlag_24bit)) != 0)
                {
                    if (m_tablet.supportsWrite())
                        useColor = true;
                }*/
                if ((encodingFlag & wgssSTU.encodingFlag.EncodingFlag_24bit) != 0)
                {
                    m_encodingMode = m_tablet.supportsWrite() ? wgssSTU.encodingMode.EncodingMode_24bit_Bulk : wgssSTU.encodingMode.EncodingMode_24bit;
                }
                else if ((encodingFlag & wgssSTU.encodingFlag.EncodingFlag_16bit) != 0)
                {
                    m_encodingMode = m_tablet.supportsWrite() ? wgssSTU.encodingMode.EncodingMode_16bit_Bulk : wgssSTU.encodingMode.EncodingMode_16bit;
                }
                else
                {     // assumes 1bit is available
                    m_encodingMode = wgssSTU.encodingMode.EncodingMode_1bit;
                }


                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                var bitmapData = (byte[])protocolHelper.resizeAndFlatten(stream.ToArray(), 0, 0, (uint)bitmap.Width, (uint)bitmap.Height, m_capability.screenWidth, m_capability.screenHeight, (byte)m_encodingMode, wgssSTU.Scale.Scale_Fit, 0, 0);

                m_tablet.writeImage((byte)m_encodingMode, bitmapData);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /*public frmWacom GetLiveWindow()/*pun*/
       /* {
            liveWindow = new frmWacom(this);
            return liveWindow;
        }*/

        public Bitmap GetSigAsBitmap()/*Bitmap פונקציה מחזירה החתימה בסוג*/
        {
            var bitmap = new Bitmap(m_capability.screenWidth, m_capability.screenHeight, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics gfx = Graphics.FromImage(bitmap);
            gfx.Clear(Color.White);
            bool isDown = false;
            PointF prev = new PointF();
            for (int i = 0; i < m_penData.Count; ++i)
            {
                if (m_penData[i].sw != 0)
                {
                    if (!isDown)
                    {
                        isDown = true;
                        prev = tabletToScreen(m_penData[i]);
                    }
                    else
                    {
                        PointF curr = tabletToScreen(m_penData[i]);
                        gfx.DrawLine(Pens.Black, prev, curr);
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
            return bitmap;
            //bitmap.Save(@"C:\temp\tttt.bmp", System.Drawing.Imaging.ImageFormat.Jpeg);
        }
    }
}
