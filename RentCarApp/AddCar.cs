using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
namespace RentCarApp
{
    public partial class AddCar : Form
    {
        private DbManager manger;
        private bool active;
        private string carNum;
        private DateTime add_date;
        private DateTime remove_Date;
        private Car NewCar;
        private Manager m;
        public enum EditMode
        {
            Add,
            Update
        }

        EditMode editMode;
        private string id;
        public AddCar(Manager m, EditMode mode, string Employee_ID)
        {
            InitializeComponent();
            editMode = mode;
            manger = new DbManager();
            this.m = m;
            id = Employee_ID;
            if (editMode == EditMode.Update)
            {
                carActiveTxt.Visible = true;
                carActiveTxt.Enabled = false;
                label16.Visible = true;
                carAvailabeTxt.Visible = true;
                carAvailabeTxt.Enabled = false;
                label18.Visible = true;
                dateAddedDatetime.Visible = true;
                dateAddedDatetime.Enabled = false;
                label17.Visible = true;
                dateRemove.Visible = true;
                dateRemove.Enabled = false;
                label19.Visible = true;
                addCarBtn.Text = "עדכון רכב";
            }
        }
        /*פונקציה לבדוק אם מוספים רכב או מעדכנים רכב*/
        private void addCarBtn_Click(object sender, EventArgs e) 
        {
            if (editMode == EditMode.Add)
            {
                Add();
            }
            else
            {
                UpdateCar();
            }

        }
        /*פונקציה לבדיקת תקינות קלט לרכב ש רוצים להוסיף */
        private void Add() 
        {
            if (IsDig(carNumTxt.Text) && IsHeOrEn(carManufacturerTxt.Text) && carModelTxt.Text != string.Empty && carStatusTxt.Text != string.Empty && CheckAll_ComboBox() && IsDistance(distanceTxt) && Check_ProductionDate(ProductiondateTimePicker))
            {
                float Payment = float.Parse(PaymentNum.Value.ToString());
                int engine_Capacity = int.Parse(engineCapacityTxt.Text);
                int doors = int.Parse(doorsNumTxt.Text);
                int seats = int.Parse(seatsNumTxt.Text);
                double distance = double.Parse(distanceTxt.Text);
                NewCar = new Car(carNumTxt.Text, carManufacturerTxt.Text, carModelTxt.Text, engine_Capacity, licensingExpiryDateTime.Value, insuranceExpiryDateTime.Value, ProductiondateTimePicker.Value.Year, doors, seats, gearboxTypeTxt.SelectedItem.ToString(), carColorTxt.Text, distance, fuelTypeTxt.Text, fuelCapacityTxt.Text, Payment, carStatusTxt.Text);
                if (manger.AddNewCar(NewCar) && manger.AddCarComment(NewCar, id, 1) && manger.AddCarComment(NewCar, id, 2))
                {
                    m.onloadCar();
                    MessageBox.Show("Added successfuly", "Finished", MessageBoxButtons.OK);
                    ClearAddCar();
                }
                else
                    MessageBox.Show("INFO: Adding new car", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Check your information", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /*פונקציה לבדיקת תקינות קלט לרכב ש רוצים לעדכן */
        private void UpdateCar()
        {
            if (IsDig(carNumTxt.Text) && IsHeOrEn(carManufacturerTxt.Text) && carModelTxt.Text != string.Empty && carStatusTxt.Text != string.Empty && CheckAll_ComboBox() && IsDistance(distanceTxt) && Check_ProductionDate(ProductiondateTimePicker))
            {
                float Payment = float.Parse(PaymentNum.Value.ToString());
                int engine_Capacity = int.Parse(engineCapacityTxt.Text);
                int doors = int.Parse(doorsNumTxt.Text);
                int seats = int.Parse(seatsNumTxt.Text);
                double distance = double.Parse(distanceTxt.Text);
                DateTime licensing = new DateTime(licensingExpiryDateTime.Value.Year, licensingExpiryDateTime.Value.Month, licensingExpiryDateTime.Value.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                DateTime insurance = new DateTime(insuranceExpiryDateTime.Value.Year, insuranceExpiryDateTime.Value.Month, insuranceExpiryDateTime.Value.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                Car UpdateCar = new Car(carNumTxt.Text, carManufacturerTxt.Text, carModelTxt.Text, engine_Capacity, licensing, insurance, ProductiondateTimePicker.Value.Year, doors, seats, gearboxTypeTxt.SelectedItem.ToString(), carColorTxt.Text, distance, fuelTypeTxt.Text, fuelCapacityTxt.Text, Payment, carStatusTxt.Text);
                UpdateCar.Car_Availability = carAvailabeTxt.SelectedIndex == 1 ? false : true;
                UpdateCar.Car_Active = carActiveTxt.SelectedIndex == 1 ? false : true;
                if (manger.UpdateOldCar(UpdateCar,active, add_date, remove_Date, carNum) && manger.UpdateCarComment1(UpdateCar, id, carNum, carActiveTxt.Text) && manger.UpdateCarComment2(UpdateCar, id, carNum, carActiveTxt.Text))
                {
                    m.onloadCar();
                    this.Close();
                }
                else
                    MessageBox.Show("ERROR update car", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Check your information", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /*פונקציה לנקות את כל השדות בממשק אחרי שהספנו רכב*/
        private void ClearAddCar()
        {
            carNumTxt.Clear();
            carManufacturerTxt.Clear();
            carModelTxt.Clear();
            engineCapacityTxt.SelectedIndex = -1;
            licensingExpiryDateTime.Value = DateTime.Now;
            insuranceExpiryDateTime.Value = DateTime.Now;
            ProductiondateTimePicker.Value = DateTime.Now;
            doorsNumTxt.SelectedIndex = -1;
            seatsNumTxt.SelectedIndex = -1;
            gearboxTypeTxt.SelectedIndex = -1;
            carColorTxt.SelectedIndex = -1;
            distanceTxt.Clear();
            fuelTypeTxt.SelectedIndex = -1;
            fuelCapacityTxt.SelectedIndex = -1;
            carStatusTxt.Text = "הכל תקין";
        }
        /*פונקציה לבדוק תאריך יצור של הרכ אם הוא תקין*/
        private bool Check_ProductionDate(DateTimePicker d)
        {
            if (d.Value.Year <= DateTime.Now.Year)
                return true;
            return false;
        }
        /*ComboBox פונקציה שבודקת תקינות קילט לכל שדה מסוג */
        private bool CheckAll_ComboBox()
        {
            if (IsDig(engineCapacityTxt.Text) && IsDig(doorsNumTxt.Text) && IsDig(seatsNumTxt.Text) && gearboxTypeTxt.Text != string.Empty && IsHeOrEn(carColorTxt.Text) && fuelTypeTxt.Text != string.Empty && fuelCapacityTxt.Text != string.Empty)
                return true;
            return false;
        }
        /*פונקציה  לבדוק אם הנקלט בעברית או באנגלית*/
        private bool IsHeOrEn(string s) 
        {
            string txt = s.Replace(" ", "");
            if (txt != string.Empty && (Regex.IsMatch(txt, @"^[a-zA-Z]+$") | Regex.IsMatch(txt, @"^[א-ת]+$")))
                return true;
            return false;
        }
        /*int פונקציה  לבדוק אם הנקלט מספר מסוג */
        private bool IsDig(string s)
        {
            int num;
            int.TryParse(s, out num);
            if (s != string.Empty && num > 0)
                return true;
            return false;
        }
        /*double פונקציה  לבדוק אם הנקלט מספר מסוג */
        private bool IsDistance(TextBox s)//convert to double 
        {
            double num;
            if (double.TryParse(s.Text, out num))
                return true;
            return false;
        }
        /*פונקציה למלות את נתוני הרכב שרוצים לעדכן*/
        public void Update_Car(DataGridViewRow row)
        {
            carNumTxt.Text = row.Cells[0].Value.ToString();
            carManufacturerTxt.Text = row.Cells[1].Value.ToString();
            carModelTxt.Text = row.Cells[2].Value.ToString();
            engineCapacityTxt.Text = row.Cells[3].Value.ToString();
            licensingExpiryDateTime.Text = row.Cells[4].Value.ToString();
            insuranceExpiryDateTime.Text = row.Cells[5].Value.ToString();
            doorsNumTxt.Text = row.Cells[6].Value.ToString();
            seatsNumTxt.Text = row.Cells[7].Value.ToString();
            gearboxTypeTxt.Text = row.Cells[8].Value.ToString();
            carColorTxt.Text = row.Cells[9].Value.ToString();
            distanceTxt.Text = row.Cells[10].Value.ToString();
            fuelTypeTxt.Text = row.Cells[11].Value.ToString();
            fuelCapacityTxt.Text = row.Cells[12].Value.ToString();
            PaymentNum.Value = (decimal)Convert.ChangeType(row.Cells[13].Value.ToString(), typeof(decimal));
            carAvailabeTxt.Text = (bool)row.Cells[14].Value ? "זמין" : "לא זמין";
            DateTime tmp = new DateTime((int)row.Cells[15].Value, 01, 01);
            ProductiondateTimePicker.Value = tmp.Date;
            carActiveTxt.Text = (bool)row.Cells[16].Value ? "פעיל" : "לא פעיל";
            active = (bool)row.Cells[16].Value;
            if (!active)
                carActiveTxt.Enabled = true;
            dateAddedDatetime.Text = row.Cells[17].Value.ToString();
            carStatusTxt.Text = row.Cells[19].Value.ToString();
            CheckRemove_Date(row);
        }
        /*פונקציה למלות את תאריך מחיקת הרכב שרוצים לעדכן*/
        private void CheckRemove_Date(DataGridViewRow row)
        {
            DateTime tmpRemove = Convert.ToDateTime("01/01 /0001");
            if (row.Cells[18].Value != DBNull.Value)
                tmpRemove = Convert.ToDateTime(row.Cells[18].Value.ToString());
            DateTime tmp = Convert.ToDateTime("01/01 /0001");
            if (DateTime.Equals(tmpRemove, tmp))
            {
                dateRemove.Format = DateTimePickerFormat.Custom;
                dateRemove.CustomFormat = " ";
            }
            else
                dateRemove.Text = row.Cells[18].Value.ToString();
            carNum = row.Cells[0].Value.ToString();
            add_date = (DateTime)row.Cells[17].Value;
            remove_Date = tmpRemove;
        }

        private void PaymentNum_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (sender as NumericUpDown).NumericUpDown_Validating();
        }
    }
}
