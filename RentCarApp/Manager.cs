using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Font = iTextSharp.text.Font;
using Image = iTextSharp.text.Image;

namespace RentCarApp
{
    public partial class Manager : Form
    {
        private DbManager dbManager;
        private string fName;
        private string lName;
        private string id;
        private Employee employee;
        private Document doc;
        private PdfPTable myTable;
        private DataSet details = null;
        private Bitmap lastSignature = null;
        private bool closeForm = true;
        private DataSet rentDetails = null;
        private DataSet Cars = null;
        private DataSet Customers = null;
        private DataSet Employee = null;
        public Bitmap LastSignature
        {
            get
            {
                return lastSignature;
            }
            set
            {
                if (lastSignature != null)
                {
                    lastSignature.Dispose();
                    lastSignature = null;
                }
                lastSignature = value;
            }
        }
        //public Wacom wacom = null;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }
        private enum SearchTypes
        {
            RentDetails = 0,
            IncomingByDay = 1,
            RentAmountEmployee = 0,
            AmountPayment = 1,
            Cars = 2
        }

        public Manager(Employee emp)
        {
            InitializeComponent();
            /*this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            UpdateStyles();*/

            dbManager = new DbManager();
            employee = emp;//employee type,first name,last name,ID
            fName = employee.First_name;
            lName = employee.Last_name;
            id = employee.Id;
            if (employee.Employye_type == "מזכירות")
            {
                tabControl1.TabPages.Remove(tabControl1.TabPages["EmployeeTab"]);
                tabControl1.TabPages.Remove(tabControl1.TabPages["ReportsTab"]);
                panel4.Visible = false;
                carDGV.CellDoubleClick -= carDGV_CellDoubleClick;
            }
            RentDGV.DoubleBuffered(true);
            carDGV.DoubleBuffered(true);
            CustomerDGV.DoubleBuffered(true);
            EmployeeDGV.DoubleBuffered(true);
            //connect the wacom device and initialize it - it will now be ready to use

        }
        /*פונקציה לעביר אותנו לממשק אחר שבעזרתו מוספים עובד חדש למערכת*/
        private void addEmployeeBtn_Click(object sender, EventArgs e)
        {
            AddEmployee addEmp = new AddEmployee(this, AddEmployee.EditMode.Add,true);
            addEmp.ShowDialog();
        }
        /*פונקציה שעובדת אחרי שמוספים או מעדכנים או מוחקים עובד שהיא עושה לעדכן את הפרטים בטבלה*/
        public void onload()
        {
            EmployeeTab_Enter(null, null);
        }
        /*פונקציה למלאות את כותרות של טבלת העובדים*/
        private void EmployeeTab_Enter(object sender, EventArgs e)
        {
            EmployeeComboBox.SelectedIndex = 0;
            EmployeeSearchComboBox.SelectedIndex = 0;
            EmployeeSearchTxt.ForeColor = Color.Gray;
            EmployeeSearchTxt.Text = "חיפוש";
            DataSet employee = dbManager.AllEmployees();
            if ((employee != null) && (employee.Tables.Count > 0) && (employee.Tables["employee"].Rows.Count > 0))
            {
                /*employee.Tables["employee"].Columns["First_Name"].ColumnName = "שם פרטי";
                employee.Tables["employee"].Columns["Last_Name"].ColumnName = "שם משפחה";
                employee.Tables["employee"].Columns["ID"].ColumnName = "ת" + '"' + "ז";
                employee.Tables["employee"].Columns["Employye_Type"].ColumnName = "תפקיד";
                employee.Tables["employee"].Columns["Phone_Number"].ColumnName = "מס' פלאפון";
                employee.Tables["employee"].Columns["Address"].ColumnName = "כתובת";
                employee.Tables["employee"].Columns["Telephone_Number"].ColumnName = "טלפון בית";
                employee.Tables["employee"].Columns["Salary"].ColumnName = "שכר";
                employee.Tables["employee"].Columns["Employee_Availability"].ColumnName = "פעיל";
                employee.Tables["employee"].Columns["Birthdate"].ColumnName = "תאריך לידה";
                employee.Tables["employee"].Columns["Start_Work_Date"].ColumnName = "תאריך הוספה";
                employee.Tables["employee"].Columns["End_Work_Date"].ColumnName = "תאריך סיום";*/
                EmployeeDGV.DataSource = employee.Tables["employee"];
                EmployeeDGV.Columns[9].DefaultCellStyle.Format = "dd/MM/yyyy";
                EmployeeDGV.Columns[10].DefaultCellStyle.Format = "dd/MM/yyyy";
                EmployeeDGV.Columns[11].DefaultCellStyle.Format = "dd/MM/yyyy";
                EndDate(EmployeeDGV, 11);
            }
        }
        /*DBNull פונקציה למלות את עמודת תאריך סיום אם אין ב*/
        private void EndDate(DataGridView tmp, int j)
        {
            int i = 0;
            for (i = 0; i < tmp.RowCount; i++)
            {
                if (tmp.Rows[i].Cells[j].Value.ToString().Contains("01/01/0001"))// 0:00:00")//12:00:00 AM
                    tmp.Rows[i].Cells[j].Value = DBNull.Value;
            }
        }
        /*פונקציה למחוק עובד ממערכת אם אפשר בעזרת מחלקה אחרת*/
        private void deleteEmployeeBtn_Click(object sender, EventArgs e)
        {
            string id = EmployeeDGV.SelectedRows[0].Cells[2].Value.ToString();
            if ((bool)EmployeeDGV.SelectedRows[0].Cells[8].Value == true)
            {
                if (dbManager.RemoveEmployee(id))
                {
                    EmployeeDGV.SelectedRows[0].Cells[11].Value = DateTime.Now;
                    EmployeeDGV.SelectedRows[0].Cells[8].Value = 0;
                }
                else
                    MessageBox.Show("Failed To Delete Employee", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Employee is already deleted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /*LogIn פונקציה לחזור לממשק */
        private void logOutBtn_Click(object sender, EventArgs e)
        {
            //this.Hide();
            //this.Enabled = false;
            if (dbManager.Backup())
            {
                PerformBackUp();
            }
            else
                MessageBox.Show("couldn't get data from database to backup", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            closeForm = false;
            this.Close();
            Program.frmLogin.Show();
        }
        

        public void PerformBackUp()
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.Credentials = new NetworkCredential("yhcarrent", "1$QR!4ldhBUG@$R86SBZ");
                    client.UploadFile("ftp://files.000webhost.com/backup/backup.sql", WebRequestMethods.Ftp.UploadFile,Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\RentCarAppLast desktop\\temp\\backup.sql");
                }
            }
            catch
            {
                MessageBox.Show("couldn't backup database to server", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////
        /// 
        /*פונקציה שעובדת כשנכנסים לממשק הראשי לכתוב שם של העובד וכו*/
        private void Manager_Load(object sender, EventArgs e)
        {
            if (DateTime.Now.Hour >= 6 && DateTime.Now.Hour <= 11)
                label3.Text = " בוקר טוב" + " " + fName + " " + lName;
            if (DateTime.Now.Hour >= 12 && DateTime.Now.Hour <= 17)
                label3.Text = " צהריים טובים " + " " + fName + " " + lName;
            if (DateTime.Now.Hour >= 18 && DateTime.Now.Hour <= 24)
                label3.Text = " ערב טוב" + " " + fName + " " + lName;
            if (DateTime.Now.Hour >= 1 && DateTime.Now.Hour <= 5)
                label3.Text = " לילה טוב" + " " + fName + " " + lName;
            this.WindowState = FormWindowState.Maximized;
            this.MinimumSize = this.Size;
            this.MaximumSize = this.Size;
            commentDTB.Value = DateTime.Now;
            TodayDateBtn_Click(null, null);
           /* rentDetailCB.SelectedIndex = 0;
            RentSearchCB.SelectedIndex = 0;*/
        }
        /*פונקציה לעביר אותנו לממשק אחר שבעזרתו מוספים שכירות חדשה למערכת*/
        private void rentCarBtn_Click(object sender, EventArgs e) 
        {
            AddRent addRent = new AddRent(this, id, false, AddRent.RentType.Add);
            addRent.ShowDialog();
        }
        /*פונקציה שעובדת אחרי שמוספים או מעדכנים או מחזרים שכירות שהיא עושה לעדכן את הפרטים בטבלה*/
        public void onloadRent()//remember to change the Employee tab enter to car tab enter 
        {
            RentInfoTab_Enter(null, null);
        }
        /*פונקציה לעביר אותנו לממשק אחר שבעזרתו מחזרים שכירות */
        private void returnCarBtn_Click(object sender, EventArgs e)
        {
            if (RentDGV.SelectedRows[0].Cells[13].Value.ToString().Equals(""))
            {
                if (Convert.ToDateTime(RentDGV.SelectedRows[0].Cells[16].Value.ToString()).Date <= DateTime.Now.Date)
                {
                    AddRent returnRent = new AddRent(this, id, true, AddRent.RentType.Return);
                    returnRent.Return_Rent(RentDGV.SelectedRows[0]);
                    returnRent.ShowDialog();
                }
                else
                    MessageBox.Show("Info:You can't return the car until the return date is today ", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Info:this is already returned rent", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /*פונקציה למלות את הכתרות של טבלט השכירות*/
        private void RentInfoTab_Enter(object sender, EventArgs e) 
        {
            rentDetailCB.SelectedIndex = 0;
            RentSearchCB.SelectedIndex = 0;
            rentSearchTB.ForeColor = Color.Gray;
            rentSearchTB.Text = "חיפוש";
            rentDetails = dbManager.AllRent();
            if ((rentDetails != null) && (rentDetails.Tables.Count > 0) && (rentDetails.Tables["rent_details"].Rows.Count > 0))
            {
                /*rentDetails.Tables["rent_details"].Columns["Rental_ID"].ColumnName = "מס' שכירות";
                rentDetails.Tables["rent_details"].Columns["ID_Customer"].ColumnName = "ת" + '"' + "ז לקוח";
                rentDetails.Tables["rent_details"].Columns["First_Name"].ColumnName = "שם פרטי";
                rentDetails.Tables["rent_details"].Columns["Last_Name"].ColumnName = "שם משפחה";
                rentDetails.Tables["rent_details"].Columns["Phone_Number"].ColumnName = "מס' פלאפון";
                rentDetails.Tables["rent_details"].Columns["Telephone_Number"].ColumnName = "מס' טלפון";
                rentDetails.Tables["rent_details"].Columns["Car_Number"].ColumnName = "מס' רכב";
                rentDetails.Tables["rent_details"].Columns["Car_Manufacturer"].ColumnName = "יצרן רכב";
                rentDetails.Tables["rent_details"].Columns["Car_Model"].ColumnName = "דגם רכב";
                //rent.Tables["rent_details"].Columns["Car_Status_Rent"].ColumnName = "מצב רכב לפני";
                // rent.Tables["rent_details"].Columns["Car_Status_Returned"].ColumnName = "מצב רכב אחרי";
                rentDetails.Tables["rent_details"].Columns["Payment"].ColumnName = "סכום העסקה";
                rentDetails.Tables["rent_details"].Columns["Payment_Type"].ColumnName = "מצב תשלום";
                rentDetails.Tables["rent_details"].Columns["Price_For_Day"].ColumnName = "תשלום ליום";
                rentDetails.Tables["rent_details"].Columns["ID_Employee_Rent"].ColumnName = "ת" + '"' + "ז עובד בהשכרה";
                rentDetails.Tables["rent_details"].Columns["ID_Employee_Returned"].ColumnName = "ת" + '"' + "ז עובד בהחזרה";
                rentDetails.Tables["rent_details"].Columns["Rent_Days"].ColumnName = "ימי השכרה";
                rentDetails.Tables["rent_details"].Columns["Rent_Date"].ColumnName = "תאריך השכרה";
                rentDetails.Tables["rent_details"].Columns["Returned_Date"].ColumnName = "תאריך החזרה";
                rentDetails.Tables["rent_details"].Columns["KM_For_Day"].ColumnName = "ק" + '"' + "ם ליום";
                rentDetails.Tables["rent_details"].Columns["Rent_Distance"].ColumnName = "ק" + '"' + "ם בהשכרה";
                rentDetails.Tables["rent_details"].Columns["Return_Distance"].ColumnName = "ק" + '"' + "ם בהחזרה";
                rentDetails.Tables["rent_details"].Columns["Quantity_Fuel_Rent"].ColumnName = "דלק בהשכרה";
                rentDetails.Tables["rent_details"].Columns["Quantity_Fuel_Returned"].ColumnName = "דלק בהחזרה";
                rentDetails.Tables["rent_details"].Columns["Payed"].ColumnName = "שולם";*/
                RentDGV.DataSource = rentDetails.Tables["rent_details"];
                RentDGV.Columns[15].DefaultCellStyle.Format = "H:mm dd/MM/yyyy ";
                RentDGV.Columns[16].DefaultCellStyle.Format = "H:mm dd/MM/yyyy ";
                //RentDGV.Sort(RentDGV.Columns[0], ListSortDirection.Ascending);
            }
        }
        /*פונקציה שמציגה שם של העובד בטבלת השכירות כשעכבר נכנס לשדה של ת"ז של העובד*/
        private void RentDGV_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && (e.ColumnIndex == 12 || e.ColumnIndex == 13) && RentDGV.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != string.Empty)
            {
                RentDGV.Rows[e.RowIndex].Cells[e.ColumnIndex].ToolTipText = dbManager.EmployeeFL(RentDGV.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
            }
        }
        /*פונקציה לעביר אותנו לממשק אחר שבעזרתו מעדכנים עוביד */
        private void EmployeeDGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {//(bool)row.Cells[8].Value;
            AddEmployee addEmp = new AddEmployee(this, AddEmployee.EditMode.Update,(bool)EmployeeDGV.SelectedRows[0].Cells[8].Value);
            addEmp.Update_Employee(EmployeeDGV.SelectedRows[0], id);
            addEmp.ShowDialog();
        }
        public void updateEmployee(Employee updateEmployee)
        {
            EmployeeDGV.SelectedRows[0].Cells[0].Value = updateEmployee.First_name.ToString();
            EmployeeDGV.SelectedRows[0].Cells[1].Value = updateEmployee.Last_name.ToString();
            EmployeeDGV.SelectedRows[0].Cells[2].Value = updateEmployee.Id.ToString();
            EmployeeDGV.SelectedRows[0].Cells[3].Value = updateEmployee.Employye_type.ToString();
            EmployeeDGV.SelectedRows[0].Cells[4].Value = updateEmployee.Phone_number.ToString();
            EmployeeDGV.SelectedRows[0].Cells[5].Value = updateEmployee.Address.ToString();
            EmployeeDGV.SelectedRows[0].Cells[6].Value = updateEmployee.Telephone_number.ToString();
            EmployeeDGV.SelectedRows[0].Cells[7].Value = updateEmployee.Salary;
            EmployeeDGV.SelectedRows[0].Cells[8].Value = updateEmployee.Employee_availability;
            EmployeeDGV.SelectedRows[0].Cells[9].Value = updateEmployee.Birth_date.ToString();
            EmployeeDGV.SelectedRows[0].Cells[11].Value = updateEmployee.End_work_date;
            EndDate(EmployeeDGV, 11);
        }

        //////////////////  carTab//////////////////
        ///
        /*פונקציה לעביר אותנו לממשק אחר שבעזרתו מוספים רכב חדש למערכת*/
        private void addCarBtn_Click(object sender, EventArgs e)
        {
            AddCar addCar = new AddCar(this, AddCar.EditMode.Add, id);
            addCar.ShowDialog();
        }
        /*פונקציה שעובדת אחרי שמוספים או מעדכנים או מוחקים רכב שהיא עושה לעדכן את הפרטים בטבלה*/
        public void onloadCar()//remember to change the Employee tab enter to car tab enter
        {
            CarInfoTab_Enter(null, null);
        }
        /*פונקציה למלות את הכתרות של טבלת הרכבים*/
        private void CarInfoTab_Enter(object sender, EventArgs e)
        {
            carSearchComboBox.SelectedIndex = 0;
            carAvailabeTxt.SelectedIndex = 0;
            Cars = dbManager.AllCar();
            CarTabSearchTxt.Text = "חיפוש";
            CarTabSearchTxt.ForeColor = Color.Gray;
            if ((Cars != null) && (Cars.Tables.Count > 0) && (Cars.Tables["car"].Rows.Count > 0))//((Cars == null )|| (Cars.Tables.Count == 0)||(Cars.Tables["car"].Rows.Count == 0))
            {
                /*Cars.Tables["car"].Columns["Car_Number"].ColumnName = "מס' רכב";
                Cars.Tables["car"].Columns["Car_Manufacturer"].ColumnName = "יצרן";
                Cars.Tables["car"].Columns["Car_Model"].ColumnName = "דגם";
                Cars.Tables["car"].Columns["Engine_Capacity"].ColumnName = "מנוע";
                Cars.Tables["car"].Columns["Licensing_Expire_Date"].ColumnName = "תוקף הרישיון";
                Cars.Tables["car"].Columns["Insurance_Expire_Date"].ColumnName = "תוקף הביטוח";
                Cars.Tables["car"].Columns["Doors"].ColumnName = "מס' דלתות";
                Cars.Tables["car"].Columns["Seats"].ColumnName = "מס' מושבים";
                Cars.Tables["car"].Columns["Gearbox_Type"].ColumnName = "גיר";
                Cars.Tables["car"].Columns["Color"].ColumnName = "צבע";
                Cars.Tables["car"].Columns["Distance"].ColumnName = "ק" + '"' + "ם";
                Cars.Tables["car"].Columns["Fuel_Type"].ColumnName = "סוג דלק";
                Cars.Tables["car"].Columns["Fuel_Capacity"].ColumnName = "כמות דלק";
                Cars.Tables["car"].Columns["Price_For_Day"].ColumnName = "תשלום ליום";
                Cars.Tables["car"].Columns["Car_Availability"].ColumnName = "רכב זמין";
                Cars.Tables["car"].Columns["Year_Production"].ColumnName = "שנת יצור";
                Cars.Tables["car"].Columns["Car_Active"].ColumnName = "רכב פעיל";
                Cars.Tables["car"].Columns["Car_Date_Added"].ColumnName = "תאריך הוספה";
                Cars.Tables["car"].Columns["Car_Date_Deleted"].ColumnName = "הפסקת פעילות ב-";*/
                carDGV.DataSource = Cars.Tables["car"];
                carDGV.Columns[4].DefaultCellStyle.Format = "dd/MM/yyyy";
                carDGV.Columns[5].DefaultCellStyle.Format = "dd/MM/yyyy";
                carDGV.Columns[17].DefaultCellStyle.Format = "dd/MM/yyyy";
                carDGV.Columns[18].DefaultCellStyle.Format = "dd/MM/yyyy";
                carDGV.Columns[19].Visible = false;
                EndDate(carDGV, 18);
            }
        }
        /*פונקציה למחוק רכב ממערכת אם אפשר בעזרת מחלקה אחרת*/
        private void removeCarBtn_Click(object sender, EventArgs e)
        {
            string carNum = carDGV.SelectedRows[0].Cells[0].Value.ToString();
            if (((bool)carDGV.SelectedRows[0].Cells[14].Value == true) && ((bool)carDGV.SelectedRows[0].Cells[16].Value == true))
            {
                dbManager.DeleteCarComment(carNum);
                dbManager.RemoveCar(carNum);
                CarInfoTab_Enter(null, null);
            }
            else
                MessageBox.Show("Car is already deleted Or the car is in renting", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /*פונקציה שמחזירה מילת חיפוש לשדה החיפוש בדף של הרכב כשיוצים מהשדה*/
        private void CarTabSearchTxt_Leave(object sender, EventArgs e)
        {
            if (CarTabSearchTxt.Text == "")
            {
                CarTabSearchTxt.Text = "חיפוש";
                CarTabSearchTxt.ForeColor = Color.Gray;
            }
        }
        /*פונקציה שמוחקת מילת חיפוש לשדה החיפוש בדף של הרכב כשנכנסים להשדה*/
        private void CarTabSearchTxt_Enter(object sender, EventArgs e)
        {
            if (CarTabSearchTxt.Text == "חיפוש")
            {
                CarTabSearchTxt.Text = "";
                CarTabSearchTxt.ForeColor = Color.Black;
            }
        }
        /*פונקציה למלות הטבלה אחרי חיפוש*/
        private void CarTable()
        {
            if ((Cars == null )|| (Cars.Tables.Count == 0)||(Cars.Tables["car"].Rows.Count == 0))
            {
                MessageBox.Show("There is no car with this details", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CarInfoTab_Enter(null, null);
            }
            else
            {
               /* Cars.Tables["car"].Columns["Car_Number"].ColumnName = "מס' רכב";
                Cars.Tables["car"].Columns["Car_Manufacturer"].ColumnName = "יצרן";
                Cars.Tables["car"].Columns["Car_Model"].ColumnName = "דגם";
                Cars.Tables["car"].Columns["Engine_Capacity"].ColumnName = "מנוע";
                Cars.Tables["car"].Columns["Licensing_Expire_Date"].ColumnName = "תוקף הרישיון";
                Cars.Tables["car"].Columns["Insurance_Expire_Date"].ColumnName = "תוקף הביטוח";
                Cars.Tables["car"].Columns["Doors"].ColumnName = "מס' דלתות";
                Cars.Tables["car"].Columns["Seats"].ColumnName = "מס' מושבים";
                Cars.Tables["car"].Columns["Gearbox_Type"].ColumnName = "גיר";
                Cars.Tables["car"].Columns["Color"].ColumnName = "צבע";
                Cars.Tables["car"].Columns["Distance"].ColumnName = "ק" + '"' + "ם";
                Cars.Tables["car"].Columns["Fuel_Type"].ColumnName = "סוג דלק";
                Cars.Tables["car"].Columns["Fuel_Capacity"].ColumnName = "כמות דלק";
                Cars.Tables["car"].Columns["Price_For_Day"].ColumnName = "תשלום ליום";
                Cars.Tables["car"].Columns["Car_Availability"].ColumnName = "רכב זמין";
                Cars.Tables["car"].Columns["Year_Production"].ColumnName = "שנת יצור";
                Cars.Tables["car"].Columns["Car_Active"].ColumnName = "רכב פעיל";
                Cars.Tables["car"].Columns["Car_Date_Added"].ColumnName = "תאריך הוספה";
                Cars.Tables["car"].Columns["Car_Date_Deleted"].ColumnName = "הפסקת פעילות ב-";*/
                carDGV.DataSource = Cars.Tables["car"];
                carDGV.Columns[4].DefaultCellStyle.Format = "dd/MM/yyyy";
                carDGV.Columns[5].DefaultCellStyle.Format = "dd/MM/yyyy";
                carDGV.Columns[17].DefaultCellStyle.Format = "dd/MM/yyyy";
                carDGV.Columns[18].DefaultCellStyle.Format = "dd/MM/yyyy";
                carDGV.Columns[19].Visible = false;
                EndDate(carDGV, 18);
            }
        }
        /*של הרכבים Tab פונקציה לעשות חיפוש לפי הנתונים שהמשתמש רוצה ב*/
        private void searchCarBtn_Click(object sender, EventArgs e)
        {
            if (((carSearchComboBox.SelectedIndex >= 0 && carSearchComboBox.SelectedIndex <= 3) || (carSearchComboBox.SelectedIndex >= 6 && carSearchComboBox.SelectedIndex <= 10)) && CarTabSearchTxt.Text != string.Empty && CarTabSearchTxt.Text != "חיפוש")
                /*TEXTBOX אם מחפשים ב */
                Cars = dbManager.SearchCarTextBox(carSearchComboBox.SelectedIndex, carAvailabeTxt.SelectedIndex, CarTabSearchTxt.Text);
            else if ((carSearchComboBox.SelectedIndex == 4 || carSearchComboBox.SelectedIndex == 5 || carSearchComboBox.SelectedIndex == 11 || carSearchComboBox.SelectedIndex == 12))
                /*DATE TIME PICKER אם מחפשים ב*/
                Cars = dbManager.SearchCarDate(carSearchComboBox.SelectedIndex, carAvailabeTxt.SelectedIndex, dateTimeSearch.Value.Date);
            else if (carSearchComboBox.SelectedIndex == 13 && carActiveTxt.SelectedIndex >= 0)
                /*אם מחפשים דגם*/
                Cars = dbManager.SearchCarComboBox(carActiveTxt.SelectedIndex, carAvailabeTxt.SelectedIndex);
            else
            {
                //MessageBox.Show("INFO:Select an exist search item Or enter a detail to search", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Cars = null;
            }
            CarTable();
        }
        /*פונקציה שבודקת איזה סוג של חיפוש רוצים ואחר כך נפתח שדה מתאים*/
        private void carSearchComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (carAvailabeTxt.Enabled == false)
                carAvailabeTxt.Enabled = true;
            if (carSearchComboBox.SelectedIndex == 5 || carSearchComboBox.SelectedIndex == 4 || carSearchComboBox.SelectedIndex == 11 || carSearchComboBox.SelectedIndex == 12)
            {
                CarTabSearchTxt.Visible = false;
                dateTimeSearch.Visible = true;
                carActiveTxt.Visible = false;
            }
            else if (carSearchComboBox.SelectedIndex == 13)
            {
                CarTabSearchTxt.Visible = false;
                dateTimeSearch.Visible = false;
                carActiveTxt.Visible = true;
            }
            else
            {
                CarTabSearchTxt.Visible = true;
                dateTimeSearch.Visible = false;
                carActiveTxt.Visible = false;
            }
        }
        /*פונקציה לעביר אותנו לממשק אחר שבעזרתו מעדכנים רכב */
        private void carDGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            AddCar updateCar = new AddCar(this, AddCar.EditMode.Update, id);
            updateCar.Update_Car(carDGV.SelectedRows[0]);
            updateCar.ShowDialog();
        }
        /*item מעדכנים טבלת הרכבים אחרי שנחליף*/
        private void carAvailabeTxt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (carAvailabeTxt.SelectedIndex == 0)
                /*כל הרכבים*/
                 Cars = dbManager.AllCar();//CarInfoTab_Enter(null, null);
            else if (carAvailabeTxt.SelectedIndex == 1)
                /*רכבים זמנים*/
                Cars = dbManager.AllCar_toRent();
            else if (carAvailabeTxt.SelectedIndex == 2)
                /*רכבים לא זמנים*/
                Cars = dbManager.AllCarInRent();
            CarTable();
        }
       
        /*פונקציה לבדוק אם הרכב לא פעיל*/
        private void carActiveTxt_SelectedIndexChanged(object sender, EventArgs e) 
        {
            if (carActiveTxt.SelectedIndex == 1)
                carAvailabeTxt.Enabled = false;
            else
                carAvailabeTxt.Enabled = true;
        }
        /*פונקציה לנקות את החיפוש*/
        private void CleanSearchBtn_Click(object sender, EventArgs e) 
        {
            carSearchComboBox.SelectedIndex = -1;
            CarInfoTab_Enter(null, null);

        }

        /////////////////////////////////////////CUSTOMER TAB////////////////////////////////////////////////////////
        ///
        ////*פונקציה לעביר אותנו לממשק אחר שבעזרתו מעדכנים לקוח */
        private void CustomerDGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            AddCustomer updateCus = new AddCustomer(this, AddCustomer.EditMode.Update);
            updateCus.Update_CustomerLoad(CustomerDGV.SelectedRows[0], employee);
            updateCus.ShowDialog();
        }
        /*פונקציה שעובדת אחרי שמוספים או מעדכנים או מוחקים לקוח היא עושה עדכון את להפרטים בטבלה*/
        public void onloadCustomer() 
        {
            CustomerTab_Enter(null, null);
        }

        private void addCustomerBtn_Click(object sender, EventArgs e)/*פונקציה לעביר אותנו לממשק אחר שבעזרתו מוספים לקוח */
        {
            AddCustomer addCus = new AddCustomer(this, AddCustomer.EditMode.Add);
            addCus.ShowDialog();
        }
        /*פונקציה למלות את הטבלה הלקוח אחרי החיפוש*/
        private void CustomerTable()
        {
            if ((Customers == null )|| (Customers.Tables.Count == 0) ||( Customers.Tables["driver_licensing"].Rows.Count == 0))
            {
                MessageBox.Show("INFO:There is no Customer with this details", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                CustomerTab_Enter(null, null);
            }
            else
            {
                /*Customers.Tables["driver_licensing"].Columns["ID"].ColumnName = "ת" + '"' + "ז";
                Customers.Tables["driver_licensing"].Columns["First_Name"].ColumnName = "שם פרטי";
                Customers.Tables["driver_licensing"].Columns["Last_Name"].ColumnName = "שם משפחה";
                Customers.Tables["driver_licensing"].Columns["Phone_Number"].ColumnName = "מס' פלאפון";
                Customers.Tables["driver_licensing"].Columns["Address"].ColumnName = "כתובת";
                Customers.Tables["driver_licensing"].Columns["Telephone_Number"].ColumnName = "מס' טלפון";
                Customers.Tables["driver_licensing"].Columns["Birthdate"].ColumnName = "תאריך לידה";
                Customers.Tables["driver_licensing"].Columns["Expire_Date"].ColumnName = "תוקף רישיון";
                Customers.Tables["driver_licensing"].Columns["Date_Of_Issue"].ColumnName = "תאריך יצור רישיון";*/
                CustomerDGV.DataSource = Customers.Tables["driver_licensing"];
                CustomerDGV.Columns[6].DefaultCellStyle.Format = "dd/MM/yyyy";
                CustomerDGV.Columns[7].DefaultCellStyle.Format = "dd/MM/yyyy";
                CustomerDGV.Columns[8].DefaultCellStyle.Format = "dd/MM/yyyy";
            }
        }
        /*פונקציה למלות את הכתרות של טבלט הלקוח*/
        private void CustomerTab_Enter(object sender, EventArgs e)
        {
            customerSearchComboBox.SelectedIndex = 0;
            customerComboBox.SelectedIndex = 0;
            customerSearchTxt.ForeColor = Color.Gray;
            customerSearchTxt.Text = "חיפוש";
            Customers = dbManager.AllDrivers();
            if ((Customers != null) && (Customers.Tables.Count > 0))
            {
               /* Customers.Tables["driver_licensing"].Columns["ID"].ColumnName = "ת" + '"' + "ז";
                Customers.Tables["driver_licensing"].Columns["First_Name"].ColumnName = "שם פרטי";
                Customers.Tables["driver_licensing"].Columns["Last_Name"].ColumnName = "שם משפחה";
                Customers.Tables["driver_licensing"].Columns["Phone_Number"].ColumnName = "מס' פלאפון";
                Customers.Tables["driver_licensing"].Columns["Address"].ColumnName = "כתובת";
                Customers.Tables["driver_licensing"].Columns["Telephone_Number"].ColumnName = "מס' טלפון";
                Customers.Tables["driver_licensing"].Columns["Birthdate"].ColumnName = "תאריך לידה";
                Customers.Tables["driver_licensing"].Columns["Expire_Date"].ColumnName = "תוקף רישיון";
                Customers.Tables["driver_licensing"].Columns["Date_Of_Issue"].ColumnName = "תאריך יצור רישיון";*/
                CustomerDGV.DataSource = Customers.Tables["driver_licensing"];
                CustomerDGV.Columns[6].DefaultCellStyle.Format = "dd/MM/yyyy";
                CustomerDGV.Columns[7].DefaultCellStyle.Format = "dd/MM/yyyy";
                CustomerDGV.Columns[8].DefaultCellStyle.Format = "dd/MM/yyyy";
            }
        }
        /*פונקציה לבחור איזה סוג לקוח רוצים לחפש*/
        private void customerComboBox_SelectedIndexChanged(object sender, EventArgs e) 
        {
            if (customerComboBox.SelectedIndex == 0)
                /*כל הרכבים*/
                Customers = dbManager.AllDrivers();
            if (customerComboBox.SelectedIndex == 1)
                /*לא נשכירו*/
                Customers = dbManager.Drivers_Not_In_Rent();
            if (customerComboBox.SelectedIndex == 2)
                /*נשכירו חדש*/
                Customers = dbManager.Drivers_In_Recent_Rent();
            CustomerTable();
        }
        /*פונקציה לבדוק איזה נתון לחיפוש בלקוחות*/
        private void customerSearchComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (customerSearchComboBox.SelectedIndex >= 0 && customerSearchComboBox.SelectedIndex <= 5)
            {
                customerDateTimeSearch.Visible = false;
                customerSearchTxt.Visible = true;
            }
            else if (customerSearchComboBox.SelectedIndex >= 6 && customerSearchComboBox.SelectedIndex <= 8)
            {
                customerDateTimeSearch.Visible = true;
                customerSearchTxt.Visible = false;
            }
        }
        /*פונקציה לחיפוש לפי נתונים ו מילוי הטבלה*/
        private void searchCustomerBtn_Click(object sender, EventArgs e) 
        {
            if ((customerSearchComboBox.SelectedIndex >= 0 && customerSearchComboBox.SelectedIndex <= 5) && customerSearchTxt.Text != string.Empty)
                Customers = dbManager.SearchDriverTxt(customerComboBox.SelectedIndex, customerSearchComboBox.SelectedIndex, customerSearchTxt.Text);
            else if (customerSearchComboBox.SelectedIndex >= 6 && customerSearchComboBox.SelectedIndex <= 8)
                Customers = dbManager.SearchDriverDate(customerComboBox.SelectedIndex, customerSearchComboBox.SelectedIndex, customerDateTimeSearch.Value.Date.ToString("yyyy/MM/dd"));
            else
                Customers = null;
            CustomerTable();
        }
        /*פונקציה שמחזירה מילת חיפוש לשדה החיפוש בדף של הרכב כשיוצים מהשדה*/
        private void customerSearchTxt_Leave(object sender, EventArgs e)
        {
            if (customerSearchTxt.Text == "")
            {
                customerSearchTxt.Text = "חיפוש";
                customerSearchTxt.ForeColor = Color.Gray;
            }
        }
        /*פונקציה שמוחקת מילת חיפוש לשדה החיפוש בדף של הרכב כשנכנסים להשדה*/
        private void customerSearchTxt_Enter(object sender, EventArgs e)
        {
            if (customerSearchTxt.Text == "חיפוש")
            {
                customerSearchTxt.Text = "";
                customerSearchTxt.ForeColor = Color.Black;
            }
        }
        /*פונקציה לנקות את החיפוש*/
        private void cleanSearchCustomerBtn_Click(object sender, EventArgs e)
        {
            customerSearchComboBox.SelectedIndex = -1;
            CustomerTab_Enter(null, null);
        }
        /*פונקציה לעביר אותנו לממשק אחר שבעזרתו מעדכנים שגכירות */
        private void RentDGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            AddRent updateRent;
            if (RentDGV.Rows.Count > 0)
            {
                if (RentDGV.SelectedRows[0].Cells[13].Value.ToString().Equals(""))//employee id return
                    updateRent = new AddRent(this, id, true, AddRent.RentType.Update);
                else
                    updateRent = new AddRent(this, id, true, AddRent.RentType.UpdateReturn);
                updateRent.Update_Rent(RentDGV.SelectedRows[0]);
                updateRent.ShowDialog();
            }
        }
        /*פונקציה להציג הודעה לפי תאריך שנבחר*/
        private void ShowReminderBtn_Click(object sender, EventArgs e)
        {
            DataSet messages = dbManager.SearchReminder(commentDTB.Value.Date, id);
            if ((messages != null) && (messages.Tables.Count > 0) && (messages.Tables[0].Rows.Count > 0))
            {
                messages.Tables["messages"].Columns["Message_ID"].ColumnName = "מספר תזכורת";
                messages.Tables["messages"].Columns["Message_ToDate"].ColumnName = "שעה";
                messages.Tables["messages"].Columns["Message_Text"].ColumnName = "תזכורת";
                commentDGV.DataSource = messages.Tables["messages"];
                commentDGV.Columns[1].DefaultCellStyle.Format = "H:mm";
                commentDGV.Columns[0].Visible = false;
                commentDGV.Columns[1].FillWeight = 0.25f;
                commentDGV.Columns[2].FillWeight = 0.75f;
            }
            else
            {
                commentDGV.DataSource = null;
                commentDGV.Rows.Clear();
                commentDGV.Refresh();
                MessageBox.Show("Info:There is no messages for this date", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        /*פונקציה שעושה דוח אוטמתי כשנכנסים לדף של דוחות*/
        private void ReportsTab_Enter(object sender, EventArgs e)
        {
            reportTypeCB.SelectedIndex = 1;
            reportDateFrom.Value.AddMonths(-3);
            reportDateTo.MaxDate = DateTime.Now.Date;
            reportDateTo.Value = DateTime.Now.Date;
            IncomeByDay();
        }
        /*בעת סגירת ממשק ראשי לפתוח ממשק כניסה למערכת*/
        private void Manager_FormClosing(object sender, FormClosingEventArgs e)
        {
            //wacom.Disconnect();
            if (closeForm == true)
            {
                e.Cancel = closeForm;
                MessageBox.Show("INFO:\n you must press the LogOut button to exit", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
           
            //Program.frmLogin.Show();
        }
        /*פונקציה לבדוק סוג של דוח*/
        private void reportTypeCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (reportTypeCB.SelectedIndex == 0)
                searchTypeCB.Enabled = true;
            else
            {
                searchTypeCB.Enabled = false;
                searchTypeCB.SelectedIndex = -1;
            }
        }
        /*פונקציה ליצאת דוחות*/
        private void reportSearchBtn_Click(object sender, EventArgs e) 
        {
            details = null;
            if (reportTypeCB.SelectedIndex == (int)SearchTypes.IncomingByDay && reportDateFrom.Value <= reportDateTo.Value)
                IncomeByDay();
            else if (reportTypeCB.SelectedIndex == (int)SearchTypes.RentDetails && reportDateFrom.Value <= reportDateTo.Value)
            {
                if (searchTypeCB.SelectedIndex == (int)SearchTypes.RentAmountEmployee)
                    details = dbManager.EmployeeRentCount(reportDateFrom.Value.Date.ToString("yyyy/MM/dd 00:00:00"), reportDateTo.Value.Date.ToString("yyyy/MM/dd 23:59:59"));
                else if (searchTypeCB.SelectedIndex == (int)SearchTypes.AmountPayment)
                    details = dbManager.IncomeByEmployee(reportDateFrom.Value.Date.ToString("yyyy/MM/dd 00:00:00"), reportDateTo.Value.Date.ToString("yyyy/MM/dd 23:59:59"));
                else if (searchTypeCB.SelectedIndex == (int)SearchTypes.Cars)
                    details = dbManager.CarRentCount(reportDateFrom.Value.Date.ToString("yyyy/MM/dd 00:00:00"), reportDateTo.Value.Date.ToString("yyyy/MM/dd 23:59:59"));
                else
                    MessageBox.Show("Info:select search type", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if ((details != null) && (details.Tables.Count > 0)&&(details.Tables[0].Rows.Count>0))
                    RentDetailReport(details);
            }
            else
                MessageBox.Show("Info:check your date", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /*פונקציה ליצאת דוחות*/
        private void IncomeByDay()
        {
            details = dbManager.IncomeByDay(reportDateFrom.Value.Date.ToString("yyyy/MM/dd 00:00:00"), reportDateTo.Value.Date.ToString("yyyy/MM/dd 23:59:59"));
            if ((details != null) && (details.Tables.Count > 0)&&(details.Tables[0].Rows.Count>0))
            {
                reportChart.Series[0].Points.Clear();
                reportChart.Series[0].LegendText = "הכנסות לכל יום";
                reportChart.Series[0].XValueType = ChartValueType.Date;
                reportChart.ChartAreas[0].AxisX.LabelStyle.Format = "dd/MM/yyyy";
                reportChart.ChartAreas[0].CursorX.AutoScroll = true;
                reportChart.Series[0].IsValueShownAsLabel = true;
                // reportChart.Series[0].valued += '₪';
                for (int i = 0; i < details.Tables[0].Rows.Count; i++)
                {
                    reportChart.Series[0].Points.AddXY(details.Tables[0].Rows[i][1], details.Tables[0].Rows[i][0]);
                }
            }
            else
                MessageBox.Show("Info:There is no details for this date", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /*פונקציה ליצאת דוחות*/
        private void RentDetailReport(DataSet chartDetails) 
        {
            reportChart.Series[0].Points.Clear();
            reportChart.Series[0].LegendText = searchTypeCB.Text;
            reportChart.Series[0].XValueType = ChartValueType.Auto;
            reportChart.ChartAreas[0].CursorX.AutoScroll = true;
            reportChart.Series[0].IsValueShownAsLabel = true;
            for (int i = 0; i < chartDetails.Tables[0].Rows.Count; i++)
            {
                reportChart.Series[0].Points.AddXY(chartDetails.Tables[0].Rows[i][1], chartDetails.Tables[0].Rows[i][0]);
            }
        }
        /*PDF פונקציה ליצור מסמך */
        private void reportPdfBtn_Click(object sender, EventArgs e)
        {
            saveReportPdf.Filter = "PDF Files|*.pdf";
            try
            {
                if (saveReportPdf.ShowDialog() == DialogResult.OK)
                {
                    if ((details != null) && (details.Tables.Count > 0)&&(details.Tables[0].Rows.Count>0))
                    {
                        doc = new Document();
                        string nameFile = saveReportPdf.FileName;
                        PdfWriter.GetInstance(doc, new FileStream(nameFile, FileMode.Create));
                        doc.Open();
                        CreatePdf(details);
                        doc.Close();
                    }
                    else
                        MessageBox.Show("Info:There is no details to create pdf", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
            }
            catch (IOException IO)
            {
                MessageBox.Show(IO.Message, "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        /*PDF פונקציה ליצור מסמך */
        private void CreatePdf(DataSet details)
        {
            BaseFont tableFont1 = BaseFont.CreateFont(@"C:\Windows\Fonts\GISHA.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font tableFont = new Font(tableFont1, 12);
            Font tableFont2 = new Font(tableFont1, 12, 1);
            myTable = new PdfPTable(2);
            myTable.DefaultCell.BorderWidth = 0;
            myTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            myTable.AddCell(new Phrase(reportTypeCB.Text + ' ' + searchTypeCB.Text, tableFont2));
            myTable.AddCell(new Phrase(" מתאריך: " + reportDateFrom.Value.Date.ToString(" dd/MM/yyyy ") + "עד - " + reportDateTo.Value.Date.ToString(" dd/MM/yyyy"), tableFont2));
            doc.Add(myTable);
            MemoryStream chartImage = new MemoryStream();
            reportChart.SaveImage(chartImage, ChartImageFormat.Png);
            Image chart_image = Image.GetInstance(chartImage.GetBuffer());
            chart_image.ScalePercent(50f);
            /* var size = doc.PageSize;
             var per = chart_image.Width / chart_image.Height;
             chart_image.ScaleAbsoluteWidth(size.Width * 0.92f);*/
            //MemoryStream sigImage = new MemoryStream();
            //wacom.LastSignature.Save(sigImage, System.Drawing.Imaging.ImageFormat.Png);
            //doc.Add(Image.GetInstance(sigImage.GetBuffer()));
            // chart_image.ScaleAbsoluteHeight(size.Height / per);
            doc.Add(chart_image);
            //myTable = new PdfPTable(1);
            //myTable.DefaultCell.BorderWidth = 0;
            //myTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            // myTable.AddCell(new Phrase(reportTypeCB.Text + ' ' + searchTypeCB.Text));
            //doc.Add(myTable);
            ////////////////////////////////////////////////////////////////////////////////////////////
            myTable = new PdfPTable(2);
            myTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            float[] widthOfTable = new float[2];
            for (int i = 0; i < widthOfTable.Length; i++)
            {
                widthOfTable[i] = 100f / 2;
            }
            myTable.SetWidths(widthOfTable);
            myTable.WidthPercentage = 100;
            myTable.AddCell(new PdfPCell(new Phrase(details.Tables[0].Columns[1].ColumnName.Replace("_", " "), tableFont2)) { HorizontalAlignment = Element.ALIGN_CENTER });
            myTable.AddCell(new PdfPCell(new Phrase(details.Tables[0].Columns[0].ColumnName.Replace("_", " "), tableFont2)) { HorizontalAlignment = Element.ALIGN_CENTER });
            for (int i = 0; i < details.Tables[0].Rows.Count; i++)
            {
                myTable.AddCell(new PdfPCell(new Phrase(searchTypeCB.SelectedIndex == -1 ? DateTime.Parse(details.Tables[0].Rows[i][1].ToString()).Date.ToString("dd/MM/yyyy") : details.Tables[0].Rows[i][1].ToString(), tableFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                myTable.AddCell(new PdfPCell(new Phrase(searchTypeCB.SelectedIndex == -1 || searchTypeCB.SelectedIndex == 1 ? details.Tables[0].Rows[i][0].ToString() + "₪" : details.Tables[0].Rows[i][0].ToString(), tableFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                /*  myTable.AddCell(new Phrase(details.Tables[0].Rows[i][1].ToString(), tableFont));
                  myTable.AddCell(new Phrase(details.Tables[0].Rows[i][0].ToString(), tableFont));*/
            }

            doc.Add(myTable);
        }
        /*אחרי לחיצת ניקוי בדוחות מנקים את החיפוש של הדוחות*/
        private void CleanReport_Click(object sender, EventArgs e)
        {
            ReportsTab_Enter(null, null);
        }

        private void AddComment_Click(object sender, EventArgs e)
        {
            if (commentTB.Text != string.Empty)
            {
                dbManager.AddComment(id, commentTB.Text, commentDTB.Value, commentCB.SelectedItem.ToString());
                commentTB.Text = string.Empty;
                AddReminderBtn_Click(null, null);
            }
            else
                MessageBox.Show("INFO:There is no comment to add", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void AddReminderBtn_Click(object sender, EventArgs e)
        {
            if (AddReminderBtn.Text == "הוספת תזכורת")
            {
                timer1.Stop();
                commentTB.Text = string.Empty;
                commentTB.Visible = true;
                commentPanel.Visible = true;
                commentDGV.Visible = false;
                ShowReminderBtn.Visible = false;
                TodayDateBtn.Visible = false;
                commentlabel.Visible = true;
                commentCB.Visible = true;
                commentCB.SelectedIndex = 0;
                AddReminderBtn.Text = "חזרה לתזכורות";
            }
            else
            {
                addComment.Visible = true;
                updateComment.Visible = false;
                removeComment.Visible = false;
                timer1.Start();
                commentTB.Visible = false;
                commentPanel.Visible = false;
                commentDGV.Visible = true;
                ShowReminderBtn.Visible = true;
                TodayDateBtn.Visible = true;
                commentlabel.Visible = false;
                commentCB.Visible = false;
                AddReminderBtn.Text = "הוספת תזכורת";
            }
        }

        private void TodayDateBtn_Click(object sender, EventArgs e)
        {
            commentDTB.Value = DateTime.Now;
            DataSet messages = dbManager.SearchReminder(commentDTB.Value.Date, id);
            if ((messages != null) && (messages.Tables.Count > 0) && (messages.Tables["messages"].Rows.Count > 0))
            {
                messages.Tables["messages"].Columns["Message_ID"].ColumnName = "מספר תזכורת";
                messages.Tables["messages"].Columns["Message_ToDate"].ColumnName = "שעה";
                messages.Tables["messages"].Columns["Message_Text"].ColumnName = "תזכורת";
                commentDGV.DataSource = messages.Tables["messages"];
                commentDGV.Columns[1].DefaultCellStyle.Format = "H:mm";
                commentDGV.Columns[0].Visible = false;
                commentDGV.Columns[1].FillWeight = 0.25f;
                commentDGV.Columns[2].FillWeight = 0.75f;
            }
        }

        private void RentSearchCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(RentSearchCB.SelectedIndex==14||RentSearchCB.SelectedIndex==15)
            {
                PaymentStatusCB.Visible = false;
                rentSearchDTP.Visible = true;
                rentSearchTB.Visible = false;
            }
            else if(RentSearchCB.SelectedIndex==10)
            {
                PaymentStatusCB.Visible = true;
                rentSearchDTP.Visible = false;
                rentSearchTB.Visible = false;
            }
            else
            {
                PaymentStatusCB.Visible = false;
                rentSearchDTP.Visible = false;
                rentSearchTB.Visible = true;
            }
        }

        private void rentSearchTB_Enter(object sender, EventArgs e)
        {
            if (rentSearchTB.Text == "חיפוש")
            {
                rentSearchTB.Text = "";
                rentSearchTB.ForeColor = Color.Black;
            }
        }

        private void rentSearchTB_Leave(object sender, EventArgs e)
        {
            if (rentSearchTB.Text == "")
            {
                rentSearchTB.Text = "חיפוש";
                rentSearchTB.ForeColor = Color.Gray;
            }
        }
        private void RentTable()
        {
            if ((rentDetails == null) || (rentDetails.Tables.Count == 0) || (rentDetails.Tables["rent_details"].Rows.Count == 0))
            {
                MessageBox.Show("There is no rent with these details", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                RentInfoTab_Enter(null, null);
            }
            else
            {
                /*rentDetails.Tables["rent_details"].Columns["Rental_ID"].ColumnName = "מס' שכירות";
                rentDetails.Tables["rent_details"].Columns["ID_Customer"].ColumnName = "ת" + '"' + "ז לקוח";
                rentDetails.Tables["rent_details"].Columns["First_Name"].ColumnName = "שם פרטי";
                rentDetails.Tables["rent_details"].Columns["Last_Name"].ColumnName = "שם משפחה";
                rentDetails.Tables["rent_details"].Columns["Phone_Number"].ColumnName = "מס' פלאפון";
                rentDetails.Tables["rent_details"].Columns["Telephone_Number"].ColumnName = "מס' טלפון";
                rentDetails.Tables["rent_details"].Columns["Car_Number"].ColumnName = "מס' רכב";
                rentDetails.Tables["rent_details"].Columns["Car_Manufacturer"].ColumnName = "יצרן רכב";
                rentDetails.Tables["rent_details"].Columns["Car_Model"].ColumnName = "דגם רכב";
                //rent.Tables["rent_details"].Columns["Car_Status_Rent"].ColumnName = "מצב רכב לפני";
                // rent.Tables["rent_details"].Columns["Car_Status_Returned"].ColumnName = "מצב רכב אחרי";
                rentDetails.Tables["rent_details"].Columns["Payment"].ColumnName = "סכום העסקה";
                rentDetails.Tables["rent_details"].Columns["Payment_Type"].ColumnName = "סוג תשלום";
                rentDetails.Tables["rent_details"].Columns["Price_For_Day"].ColumnName = "תשלום ליום";
                rentDetails.Tables["rent_details"].Columns["ID_Employee_Rent"].ColumnName = "ת" + '"' + "ז עובד בהשכרה";
                rentDetails.Tables["rent_details"].Columns["ID_Employee_Returned"].ColumnName = "ת" + '"' + "ז עובד בהחזרה";
                rentDetails.Tables["rent_details"].Columns["Rent_Days"].ColumnName = "ימי השכרה";
                rentDetails.Tables["rent_details"].Columns["Rent_Date"].ColumnName = "תאריך השכרה";
                rentDetails.Tables["rent_details"].Columns["Returned_Date"].ColumnName = "תאריך החזרה";
                rentDetails.Tables["rent_details"].Columns["KM_For_Day"].ColumnName = "ק" + '"' + "ם ליום";
                rentDetails.Tables["rent_details"].Columns["Rent_Distance"].ColumnName = "ק" + '"' + "ם בהשכרה";
                rentDetails.Tables["rent_details"].Columns["Return_Distance"].ColumnName = "ק" + '"' + "ם בהחזרה";
                rentDetails.Tables["rent_details"].Columns["Quantity_Fuel_Rent"].ColumnName = "דלק בהשכרה";
                rentDetails.Tables["rent_details"].Columns["Quantity_Fuel_Returned"].ColumnName = "דלק בהחזרה";
                rentDetails.Tables["rent_details"].Columns["Payed"].ColumnName = "שולם";*/
                RentDGV.DataSource = rentDetails.Tables["rent_details"];
                RentDGV.Columns[15].DefaultCellStyle.Format = "HH:mm dd/MM/yyyy ";
                RentDGV.Columns[16].DefaultCellStyle.Format = "HH:mm dd/MM/yyyy ";
                //RentDGV.Sort(RentDGV.Columns[0], ListSortDirection.Ascending);
            }
        }
        private void rentSearchBtn_Click(object sender, EventArgs e)
        {
            if (RentSearchCB.SelectedIndex == 14 || RentSearchCB.SelectedIndex == 15)
            { /*DATE TIME PICKER אם מחפשים ב*/
                rentDetails = dbManager.SearchRentDate(RentSearchCB.SelectedIndex, rentDetailCB.SelectedIndex, rentSearchDTP.Value.Date);
                RentTable();
            }
            else if (RentSearchCB.SelectedIndex == 10 && PaymentStatusCB.SelectedIndex >= 0)
            {
                rentDetails = dbManager.SearchRentTextBox(RentSearchCB.SelectedIndex, rentDetailCB.SelectedIndex, PaymentStatusCB.Text);
                RentTable();
            }
            else if (rentSearchTB.Text != string.Empty && rentSearchTB.Text != "חיפוש")
            {/*TEXTBOX אם מחפשים ב */
                rentDetails = dbManager.SearchRentTextBox(RentSearchCB.SelectedIndex, rentDetailCB.SelectedIndex, rentSearchTB.Text);
                RentTable();
            }
            else
            {
                MessageBox.Show("INFO:Select an exist search item Or enter a detail to search", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                rentDetails = null;
            }
        }

        private void rentDetailCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rentDetailCB.SelectedIndex == 0)
                /*כל השכירות*/
                rentDetails = dbManager.AllRent();
            if (rentDetailCB.SelectedIndex == 1)
                /*שכירות פעילה*/
                rentDetails = dbManager.ActiveRent();
            if (rentDetailCB.SelectedIndex == 2)
                /*שכירות הסתימה*/
                rentDetails = dbManager.EndedRent();
            RentTable();
        }

        private void cleanRentSearchBtn_Click(object sender, EventArgs e)
        {
            rentDetailCB.SelectedIndex = 0;
            RentInfoTab_Enter(null, null);
            
        }

        private void reportPdfBtnCustomer_Click(object sender, EventArgs e)
        {
            saveReportPdf.Filter = "PDF Files|*.pdf";
            try
            {
                if (saveReportPdf.ShowDialog() == DialogResult.OK)
                {
                    details = dbManager.SearchCustomerInfo(CustomerDGV.SelectedRows[0].Cells[0].Value.ToString());
                    if ((details != null) && (details.Tables.Count > 0)&&(details.Tables[0].Rows.Count>0))
                    {
                    doc = new Document();
                    string nameFile = saveReportPdf.FileName;
                    PdfWriter.GetInstance(doc, new FileStream(nameFile, FileMode.Create));
                    doc.Open();
                    CreatePdfCustomer(details, CustomerDGV.SelectedRows[0]);
                    doc.Close();
                    }
                    else
                        MessageBox.Show("Info:There is no details to create pdf", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (IOException IO)
            {
                MessageBox.Show(IO.Message, "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void CreatePdfCustomer(DataSet details, DataGridViewRow CustomerRow)
        {
            BaseFont tableFont1 = BaseFont.CreateFont(@"C:\Windows\Fonts\GISHA.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            float[] widthOfTable = new float[2];
            Font tableFont = new Font(tableFont1, 12);
            Font tableFont2 = new Font(tableFont1, 14, 1);
            myTable = new PdfPTable(2);
            myTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            widthOfTable[0] = 100f / 3;
            widthOfTable[1] = 100f;
            myTable.SetWidths(widthOfTable);
            myTable.DefaultCell.BorderWidth = 0;
            myTable.WidthPercentage = 100;
            myTable.AddCell(new Phrase("שם לקןח: " + CustomerRow.Cells[1].Value.ToString() + " " + CustomerRow.Cells[2].Value.ToString(), tableFont2));
            myTable.AddCell(new Phrase("תאריך: " + DateTime.Now.ToString("dd/MM/yyyy"), tableFont2));
            for (int i = 0; i < 6; i++)
                myTable.AddCell(new Phrase(" ", tableFont));
            doc.Add(myTable);
            PdfCustomer(CustomerRow);
            PdfCellEmpty(3);
            pdfCustomerTitle("שכירות נהג ראשי:");
            CreatePdfCustomer2(details);
            PdfCellEmpty(2);
            details = dbManager.SearchSecondCustomerInfo(CustomerRow.Cells[0].Value.ToString());
            if ((details != null) && (details.Tables.Count > 0)&&(details.Tables[0].Rows.Count>0))
            {
                pdfCustomerTitle("שכירות נהג משני:");
                CreatePdfCustomer2(details);
            }
        }
        private void pdfCustomerTitle(string title)
        {
            BaseFont tableFont1 = BaseFont.CreateFont(@"C:\Windows\Fonts\GISHA.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font tableFont2 = new Font(tableFont1, 14, 1);
            myTable = new PdfPTable(1);
            myTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            myTable.WidthPercentage = 100f;
            myTable.AddCell(new PdfPCell(new Phrase(title, tableFont2)) { BorderWidth = 0 });
            doc.Add(myTable);
        }
        private void PdfCustomer(DataGridViewRow CustomerRow)
        {
            BaseFont tableFont1 = BaseFont.CreateFont(@"C:\Windows\Fonts\GISHA.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font tableFont = new Font(tableFont1, 12);
            myTable = new PdfPTable(3);
            myTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            myTable.WidthPercentage = 100f;
            for (int i = 0; i < 9; i += 3)
            {
                myTable.AddCell(new PdfPCell(new Phrase(CustomerDGV.Columns[i].HeaderText.ToString() + ":", tableFont)) { BorderWidthBottom = 0 });
                myTable.AddCell(new PdfPCell(new Phrase(CustomerDGV.Columns[i + 1].HeaderText.ToString() + ":", tableFont)) { BorderWidthBottom = 0 });
                myTable.AddCell(new PdfPCell(new Phrase(CustomerDGV.Columns[i + 2].HeaderText.ToString() + ":", tableFont)) { BorderWidthBottom = 0 });
                myTable.AddCell(new PdfPCell(new Phrase(i == 6 ? DateTime.Parse(CustomerRow.Cells[i].Value.ToString()).Date.ToString("dd/MM/yyyy") : CustomerRow.Cells[i].Value.ToString(), tableFont)) { BorderWidthTop = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
                myTable.AddCell(new PdfPCell(new Phrase(i + 1 == 7 ? DateTime.Parse(CustomerRow.Cells[i + 1].Value.ToString()).Date.ToString("dd/MM/yyyy") : CustomerRow.Cells[i + 1].Value.ToString(), tableFont)) { BorderWidthTop = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
                myTable.AddCell(new PdfPCell(new Phrase(i + 2 == 8 ? DateTime.Parse(CustomerRow.Cells[i + 2].Value.ToString()).Date.ToString("dd/MM/yyyy") : CustomerRow.Cells[i + 2].Value.ToString(), tableFont)) { BorderWidthTop = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
            }
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
       private void CreatePdfCustomer2(DataSet details)
        {
            BaseFont tableFont1 = BaseFont.CreateFont(@"C:\Windows\Fonts\GISHA.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            float[] widthOfTable = new float[2];
            Font tableFont = new Font(tableFont1, 12);
            Font tableFont2 = new Font(tableFont1, 16, 1);
            myTable = new PdfPTable(5);
            myTable.WidthPercentage = 100f;
            myTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            if (details.Tables[0].Rows.Count != 0)
                for (int i=0;i<5;i++)
                myTable.AddCell(new PdfPCell(new Phrase(details.Tables[0].Columns[i].ColumnName, tableFont2)) { HorizontalAlignment = Element.ALIGN_CENTER });
            for (int i = 0; i < details.Tables[0].Rows.Count; i++)
            {
                myTable.AddCell(new PdfPCell(new Phrase(details.Tables[0].Rows[i][0].ToString(), tableFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                myTable.AddCell(new PdfPCell(new Phrase(details.Tables[0].Rows[i][1].ToString(), tableFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                myTable.AddCell(new PdfPCell(new Phrase(details.Tables[0].Rows[i][2].ToString(), tableFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                myTable.AddCell(new PdfPCell(new Phrase(DateTime.Parse(details.Tables[0].Rows[i][3].ToString()).Date.ToString("dd/MM/yyyy"), tableFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                if (DateTime.Parse(details.Tables[0].Rows[i][4].ToString()).Date.ToString("dd/MM/yyyy") == "01/01/0001")
                    myTable.AddCell(new Phrase(" ", tableFont));
                else
                    myTable.AddCell(new PdfPCell(new Phrase(DateTime.Parse(details.Tables[0].Rows[i][4].ToString()).Date.ToString("dd/MM/yyyy"), tableFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
            }
            doc.Add(myTable);
        }
        private void Timer1_Tick(object sender, EventArgs e)
        {
            DateTime date = new DateTime(commentDTB.Value.Year, commentDTB.Value.Month, commentDTB.Value.Day, DateTime.Now.Hour,DateTime.Now.Minute, DateTime.Now.Second);
            commentDTB.Value = date;
        }

        private void RemoveComment_Click(object sender, EventArgs e)
        {
            dbManager.RemoveComment((int)details.Tables[0].Rows[0].ItemArray[0]);
            addComment.Visible = true;
            updateComment.Visible = false;
            removeComment.Visible = false;
            commentTB.Text = string.Empty;
            AddReminderBtn_Click(null, null);
            ShowReminderBtn_Click(null, null);
        }

        private void UpdateComment_Click(object sender, EventArgs e)
        {
            dbManager.UpdateComment(commentTB.Text, commentDTB.Value, commentCB.SelectedItem.ToString(), (int)details.Tables[0].Rows[0].ItemArray[0]);
            addComment.Visible = true;
            updateComment.Visible = false;
            removeComment.Visible = false;
            commentTB.Text = string.Empty;
            AddReminderBtn_Click(null, null);
            ShowReminderBtn_Click(null, null);
        }

        private void PdfBtnEmployee_Click(object sender, EventArgs e)
        {
            saveReportPdf.Filter = "PDF Files|*.pdf";
            try
            {
                if (saveReportPdf.ShowDialog() == DialogResult.OK)
                {
                    details = dbManager.SearchEmployeeInfoRent(EmployeeDGV.SelectedRows[0].Cells[2].Value.ToString());
                    if ((details != null) && (details.Tables.Count > 0)&&(details.Tables[0].Rows.Count>0))
                    {
                        doc = new Document();
                        string nameFile = saveReportPdf.FileName;
                        PdfWriter.GetInstance(doc, new FileStream(nameFile, FileMode.Create));
                        doc.Open();
                        CreatePdfEmployee(EmployeeDGV.SelectedRows[0]);
                        doc.Close();
                    }
                    else
                        MessageBox.Show("Info:There is no details to create pdf", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (IOException IO)
            {
                MessageBox.Show(IO.Message, "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void CommentDGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (commentDGV.Rows.Count > 0)
            {
                details = dbManager.CommentEmployeeINFO((int)commentDGV.SelectedRows[0].Cells[0].Value);
                if (details.Tables[0].Rows[0].ItemArray[5].ToString() == "תזכורת אישית" && details.Tables[0].Rows[0].ItemArray[1].ToString() == id)
                {
                    timer1.Stop();
                    commentTB.Visible = true;
                    commentPanel.Visible = true;
                    commentDGV.Visible = false;
                    ShowReminderBtn.Visible = false;
                    TodayDateBtn.Visible = false;
                    commentlabel.Visible = true;
                    commentCB.Visible = true;
                    addComment.Visible = false;
                    updateComment.Visible = true;
                    removeComment.Visible = true;
                    AddReminderBtn.Text = "חזרה לתזכורות";
                    commentDTB.Value = DateTime.Parse(details.Tables[0].Rows[0].ItemArray[7].ToString());
                    commentTB.Text = details.Tables[0].Rows[0].ItemArray[4].ToString();
                    commentCB.SelectedIndex = details.Tables[0].Rows[0].ItemArray[3].ToString() == "לכל העובדים" ? 0 : 1;
                }
                else
                    MessageBox.Show("You cant change OR delete this comment", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        private void CreatePdfEmployee(DataGridViewRow EmployeeRow)
        {
            BaseFont tableFont1 = BaseFont.CreateFont(@"C:\Windows\Fonts\GISHA.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            float[] widthOfTable = new float[2];
            Font tableFont = new Font(tableFont1, 12);
            Font tableFont2 = new Font(tableFont1, 14, 1);
            myTable = new PdfPTable(2);
            myTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            widthOfTable[0] = 100f / 3;
            widthOfTable[1] = 100f;
            myTable.SetWidths(widthOfTable);
            myTable.DefaultCell.BorderWidth = 0;
            myTable.WidthPercentage = 100;
            myTable.AddCell(new Phrase("שם עובד: " + EmployeeRow.Cells[0].Value.ToString() + " " + EmployeeRow.Cells[1].Value.ToString(), tableFont2));
            myTable.AddCell(new Phrase("תאריך: " + DateTime.Now.ToString("dd/MM/yyyy"), tableFont2));
            for (int i = 0; i < 6; i++)
                myTable.AddCell(new Phrase(" ", tableFont));
            doc.Add(myTable);
            PdfEmployee(EmployeeRow);
        }
        
        private void PdfEmployee(DataGridViewRow EmployeeRow)
        {
            BaseFont tableFont1 = BaseFont.CreateFont(@"C:\Windows\Fonts\GISHA.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font tableFont = new Font(tableFont1, 12);
            myTable = new PdfPTable(3);
            myTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            myTable.WidthPercentage = 100f;
            for (int i = 0; i < 12; i += 3)
            {//046563366
                myTable.AddCell(new PdfPCell(new Phrase(EmployeeDGV.Columns[i].HeaderText.ToString() + ":", tableFont)) { BorderWidthBottom = 0 });
                myTable.AddCell(new PdfPCell(new Phrase(EmployeeDGV.Columns[i + 1].HeaderText.ToString() + ":", tableFont)) { BorderWidthBottom = 0 });
                if (i + 2 != 8)
                    myTable.AddCell(new PdfPCell(new Phrase(EmployeeDGV.Columns[i + 2].HeaderText.ToString() + ":", tableFont)) { BorderWidthBottom = 0 });
                else
                    myTable.AddCell(new PdfPCell(new Phrase("מצב העובד" + ":", tableFont)) { BorderWidthBottom = 0 });
                myTable.AddCell(new PdfPCell(new Phrase(i == 9 ? DateTime.Parse(EmployeeRow.Cells[i].Value.ToString()).Date.ToString("dd/MM/yyyy") : EmployeeRow.Cells[i].Value.ToString(), tableFont)) { BorderWidthTop = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
                myTable.AddCell(new PdfPCell(new Phrase(i + 1 == 10 ? DateTime.Parse(EmployeeRow.Cells[i + 1].Value.ToString()).Date.ToString("dd/MM/yyyy") : EmployeeRow.Cells[i + 1].Value.ToString(), tableFont)) { BorderWidthTop = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
                if (i + 2 != 8)
                    myTable.AddCell(new PdfPCell(new Phrase(i + 2 != 11 ? EmployeeRow.Cells[i + 2].Value.ToString() : EmployeeRow.Cells[i + 2].Value == DBNull.Value ? " " : DateTime.Parse(EmployeeRow.Cells[i + 2].Value.ToString()).Date.ToString("dd/MM/yyyy"), tableFont)) { BorderWidthTop = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
                else
                    myTable.AddCell(new PdfPCell(new Phrase(EmployeeRow.Cells[i + 2].Value.ToString() == "True" ? "פעיל" : "לא פעיל", tableFont)) { BorderWidthTop = 0, HorizontalAlignment = Element.ALIGN_RIGHT });
            }
            doc.Add(myTable);
            PdfEmployeeRent(EmployeeRow);
        }
        
        private void PdfEmployeeRent(DataGridViewRow EmployeeRow)
        {
            PdfCellEmpty(3);
            BaseFont tableFont1 = BaseFont.CreateFont(@"C:\Windows\Fonts\GISHA.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font tableFont = new Font(tableFont1, 12);
            Font tableFont2 = new Font(tableFont1, 14, 1);
            myTable = new PdfPTable(1);
            myTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            myTable.WidthPercentage = 100f;
            myTable.AddCell(new PdfPCell(new Phrase("שכירות שנשכירו על ידי " + EmployeeRow.Cells[0].Value.ToString() + ":", tableFont2)) { BorderWidth = 0 });
            doc.Add(myTable);
            PdfEmployeeRentOrReturn2();
            PdfEmployeeReturn(EmployeeRow);
        }
        private void PdfEmployeeRentOrReturn2()
        {
            BaseFont tableFont1 = BaseFont.CreateFont(@"C:\Windows\Fonts\GISHA.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font tableFont = new Font(tableFont1, 12);
            Font tableFont2 = new Font(tableFont1, 14, 1);
            myTable = new PdfPTable(6);
            myTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            myTable.WidthPercentage = 100f;
            if (details.Tables[0].Rows.Count != 0)
                for (int i = 0; i < 6; i++)
                    myTable.AddCell(new PdfPCell(new Phrase(details.Tables[0].Columns[i].ColumnName, tableFont2)) { HorizontalAlignment = Element.ALIGN_CENTER });
            for (int i = 0; i < details.Tables[0].Rows.Count; i++)
            {
                myTable.AddCell(new PdfPCell(new Phrase(details.Tables[0].Rows[i].ItemArray[0].ToString(), tableFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                myTable.AddCell(new PdfPCell(new Phrase(details.Tables[0].Rows[i].ItemArray[1].ToString(), tableFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                myTable.AddCell(new PdfPCell(new Phrase(details.Tables[0].Rows[i].ItemArray[2].ToString(), tableFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                myTable.AddCell(new PdfPCell(new Phrase(details.Tables[0].Rows[i].ItemArray[3].ToString() + "₪", tableFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                myTable.AddCell(new PdfPCell(new Phrase(DateTime.Parse(details.Tables[0].Rows[i].ItemArray[4].ToString()).Date.ToString("dd/MM/yyyy"), tableFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                myTable.AddCell(new PdfPCell(new Phrase(DateTime.Parse(details.Tables[0].Rows[i].ItemArray[5].ToString()).Date.ToString("dd/MM/yyyy"), tableFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
            }
            doc.Add(myTable);
        }
        private void PdfEmployeeReturn(DataGridViewRow EmployeeRow)
        {
            PdfCellEmpty(3);
            BaseFont tableFont1 = BaseFont.CreateFont(@"C:\Windows\Fonts\GISHA.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            Font tableFont = new Font(tableFont1, 12);
            Font tableFont2 = new Font(tableFont1, 14, 1);
            myTable = new PdfPTable(1);
            myTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
            myTable.WidthPercentage = 100f;
            details = dbManager.SearchEmployeeInfoReturn(EmployeeDGV.SelectedRows[0].Cells[2].Value.ToString());
            if ((details != null) && (details.Tables.Count > 0)&&(details.Tables[0].Rows.Count>0))
            {
                myTable.AddCell(new PdfPCell(new Phrase("שכירות שהחזרו על ידי " + EmployeeRow.Cells[0].Value.ToString() + ":", tableFont2)) { BorderWidth = 0 });
                doc.Add(myTable);
                PdfEmployeeRentOrReturn2();
            }
        }

        private void RentPdfBTN_Click(object sender, EventArgs e)
        {
            AddRent PdfRent;
            if (RentDGV.SelectedRows[0].Cells[13].Value.ToString().Equals(""))
                PdfRent = new AddRent(this, id, true, AddRent.RentType.Update);
            else
                PdfRent = new AddRent(this, id, true, AddRent.RentType.UpdateReturn);
            PdfRent.Update_Rent(RentDGV.SelectedRows[0]);
            PdfRent.Call_SaveDocumentBtn_Click();
            //PdfRent.Dispose();
           
        }

        private void SearchEmployeeBtn_Click(object sender, EventArgs e)
        {
            if (EmployeeSearchComboBox.SelectedIndex >= 0 && EmployeeSearchComboBox.SelectedIndex <= 7 && EmployeeSearchTxt.Text != string.Empty)
                Employee = dbManager.SearchEmployeeTxt(EmployeeComboBox.SelectedIndex, EmployeeSearchComboBox.SelectedIndex, EmployeeSearchTxt.Text);
            else if (EmployeeSearchComboBox.SelectedIndex >= 8 && EmployeeSearchComboBox.SelectedIndex <= 10)
                Employee = dbManager.SearchEmployeeTxt(EmployeeComboBox.SelectedIndex, EmployeeSearchComboBox.SelectedIndex, EmployeeDateTimeSearch.Value.Date.ToString("yyyy/MM/dd"));
            else
                Employee = null;
            EmployeeTable();
        }
        private void EmployeeTable()
        {
            if ((Employee == null) || (Employee.Tables.Count == 0) || (Employee.Tables["employee"].Rows.Count == 0))
            {
                MessageBox.Show("INFO:There is no Employee with this details", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
                EmployeeTab_Enter(null, null);
            }
            else
            {
                /*Employee.Tables["employee"].Columns["First_Name"].ColumnName = "שם פרטי";
                Employee.Tables["employee"].Columns["Last_Name"].ColumnName = "שם משפחה";
                Employee.Tables["employee"].Columns["ID"].ColumnName = "ת" + '"' + "ז";
                Employee.Tables["employee"].Columns["Employye_Type"].ColumnName = "תפקיד";
                Employee.Tables["employee"].Columns["Phone_Number"].ColumnName = "מס' פלאפון";
                Employee.Tables["employee"].Columns["Address"].ColumnName = "כתובת";
                Employee.Tables["employee"].Columns["Telephone_Number"].ColumnName = "טלפון בית";
                Employee.Tables["employee"].Columns["Salary"].ColumnName = "שכר";
                Employee.Tables["employee"].Columns["Employee_Availability"].ColumnName = "פעיל";
                Employee.Tables["employee"].Columns["Birthdate"].ColumnName = "תאריך לידה";
                Employee.Tables["employee"].Columns["Start_Work_Date"].ColumnName = "תאריך הוספה";
                Employee.Tables["employee"].Columns["End_Work_Date"].ColumnName = "תאריך סיום";*/
                EmployeeDGV.DataSource = Employee.Tables["employee"];
                //EmployeeDGV.Columns[8].DefaultCellStyle.Format = 
                /*EmployeeDGV.Columns[9].DefaultCellStyle.Format = "dd/MM/yyyy";
                EmployeeDGV.Columns[10].DefaultCellStyle.Format = "dd/MM/yyyy";
                EmployeeDGV.Columns[11].DefaultCellStyle.Format = "dd/MM/yyyy";*/
                EndDate(EmployeeDGV, 11);
            }
        }

        private void CleanSearchEmployeeBtn_Click(object sender, EventArgs e)
        {
            EmployeeComboBox.SelectedIndex = 0;
            EmployeeSearchComboBox.SelectedIndex = -1;
            EmployeeSearchTxt.Clear();
            EmployeeDateTimeSearch.Value = DateTime.Now;
            EmployeeDateTimeSearch.Visible = false;
            EmployeeSearchTxt.Visible = true;
            EmployeeTab_Enter(null, null);
        }

        private void EmployeeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EmployeeComboBox.SelectedIndex == 0)
                Employee = dbManager.AllEmployees();
            if (EmployeeComboBox.SelectedIndex == 1)//פעיל
                Employee = dbManager.EmployeeAvaibilityTrue();
            if (EmployeeComboBox.SelectedIndex == 2)//לא פעיל
                Employee = dbManager.EmployeeAvaibilityFalse();
            EmployeeTable();
        }

        private void EmployeeSearchComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EmployeeSearchComboBox.SelectedIndex >= 0 && EmployeeSearchComboBox.SelectedIndex <= 7)
            {
                EmployeeDateTimeSearch.Visible = false;
                EmployeeSearchTxt.Visible = true;
            }
            else
            {
                EmployeeDateTimeSearch.Visible = true;
                EmployeeSearchTxt.Visible = false;
            }
        }

        private void EmployeeSearchTxt_Enter(object sender, EventArgs e)
        {
            if (EmployeeSearchTxt.Text == "חיפוש")
            {
                EmployeeSearchTxt.Text = "";
                EmployeeSearchTxt.ForeColor = Color.Black;
            }
        }

        private void EmployeeSearchTxt_Leave(object sender, EventArgs e)
        {
            if (EmployeeSearchTxt.Text == "")
            {
                EmployeeSearchTxt.Text = "חיפוש";
                EmployeeSearchTxt.ForeColor = Color.Gray;
            }
        }

        private void PrintReportBtn_Click(object sender, EventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\RentCarAppLast desktop\\temp\\toPrint.pdf";
            try
            {
                if ((details != null) && (details.Tables.Count > 0)&&(details.Tables[0].Rows.Count>0))
                {
                    doc = new Document();
                    PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));
                    doc.Open();
                    CreatePdf(details);
                    doc.Close();
                    Print();
                }
                else
                    MessageBox.Show("Info:There is no details to print", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (IOException IO)
            {
                MessageBox.Show(IO.Message, "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void Print()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\RentCarAppLast desktop\\temp\\toPrint.pdf";
            frmPrint printForm = new frmPrint();
            try
            {
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

        private void PrintCustomerBtn_Click(object sender, EventArgs e)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\RentCarAppLast desktop\\temp\\toPrint.pdf";
            try
            {
                details = dbManager.SearchCustomerInfo(CustomerDGV.SelectedRows[0].Cells[0].Value.ToString());
                if ((details != null) && (details.Tables.Count > 0)&&(details.Tables[0].Rows.Count>0))
                {
                    doc = new Document();
                    PdfWriter.GetInstance(doc, new FileStream(path, FileMode.Create));
                    doc.Open();
                    CreatePdfCustomer(details, CustomerDGV.SelectedRows[0]);
                    doc.Close();
                    Print();
                }
                else
                    MessageBox.Show("Info:There is no details to print", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (IOException IO)
            {
                MessageBox.Show(IO.Message, "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}

/*private void IncomeByDay()
{
DataSet report;
report = manager.IncomeByDay(reportDateFrom.Value.Date.ToString("yyyy/MM/dd 00:00:00"), reportDateTo.Value.Date.ToString("yyyy/MM/dd 23:59:59"));
if (report != null)
{
reportChart.Series[0].Points.Clear();
reportChart.Series[0].LegendText = "הכנסות לכל יום";
reportChart.Series[0].XValueType = ChartValueType.Date;
reportChart.ChartAreas[0].AxisX.LabelStyle.Format = "dd/MM/yyyy";
reportChart.ChartAreas[0].CursorX.AutoScroll = true;
reportChart.Series[0].IsValueShownAsLabel = true;

for (int i = 0; i < report.Tables[0].Rows.Count; i++)
{
reportChart.Series[0].Points.AddXY(report.Tables[0].Rows[i]["Rent_Date"], report.Tables[0].Rows[i]["Payment"]);
}
}
else
MessageBox.Show("Info:There is no details for this date", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
}*/

/* private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
   {
       DbManager manager = new DbManager();
       DataSet employee;
       employee = manager.AllEmployees();
   }*/
/*private void EmployeeTab_Enter(object sender, EventArgs e)
{
    DbManager manager = new DbManager();
    Employee[] employee;
    employee = manager.AllEmployees();
}*/
/* PdfCustomer();
BaseFont tableFont1 = BaseFont.CreateFont(@"C:\Windows\Fonts\GISHA.ttf", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
float[] widthOfTable = new float[2];
Font tableFont = new Font(tableFont1, 12);
Font tableFont2 = new Font(tableFont1, 16);
myTable = new PdfPTable(5);
myTable.WidthPercentage = 100f;
myTable.RunDirection = PdfWriter.RUN_DIRECTION_RTL;
PdfPCell cell = new PdfPCell(new Phrase("מס' שכירות", tableFont2));
cell.Rowspan = 2;
cell.HorizontalAlignment = Element.ALIGN_CENTER;
myTable.AddCell(cell);
//myTable.AddCell(new Phrase("מס' שכירות", tableFont2));
cell = new PdfPCell(new Phrase("מס' רכב", tableFont2));
cell.Rowspan = 2;
cell.HorizontalAlignment = Element.ALIGN_CENTER;
myTable.AddCell(cell);
//myTable.AddCell(new Phrase("מס' רכב", tableFont2));
cell = new PdfPCell(new Phrase("רכב", tableFont2));
cell.Rowspan = 2;
cell.HorizontalAlignment = Element.ALIGN_CENTER;
myTable.AddCell(cell);
//myTable.AddCell(new Phrase("רכב", tableFont2));
cell = new PdfPCell(new Phrase("תאריך שכירות", tableFont2));
cell.Rowspan = 2;
cell.HorizontalAlignment = Element.ALIGN_CENTER;
myTable.AddCell(cell);
//myTable.AddCell(new Phrase("תאריך שכירות", tableFont2));
cell = new PdfPCell(new Phrase("תאריך החזרה", tableFont2));
cell.Rowspan = 2;
cell.HorizontalAlignment = Element.ALIGN_CENTER;
myTable.AddCell(cell);
//myTable.AddCell(new Phrase("תאריך החזרה",tableFont2));
myTable.AddCell(new Phrase(" ", tableFont));
myTable.AddCell(new Phrase(" ", tableFont));
myTable.AddCell(new Phrase(" ", tableFont));
myTable.AddCell(new Phrase(" ", tableFont));
myTable.AddCell(new Phrase(" ", tableFont));

doc.Add(myTable);
*/
