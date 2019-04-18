using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Data.SqlClient;

namespace PlanningBoard
{
    public partial class FBPlanBoardForm : Form
    {
        ArrayList MachineList = new ArrayList();
        public Boolean Flag = false;
        public DateTime planDate = DateTime.Now;
        string orderids = "";
        int DifferenceInDays = 1;
        string query = "";
        public int MachineNo = 0;
        public int dia = 0;
        public static Boolean ChangeFlag = false;

        public FBPlanBoardForm(bool flag, int mcNo, int diaNo, DateTime taskDate, string Ids, bool changeDate)
        {
            Flag = flag;
            planDate = DateTime.ParseExact(taskDate.Date.ToString("dd/MM/yyy"), "dd/MM/yyyy", null);
            orderids = Ids;
            MachineNo = mcNo;
            dia = diaNo;
            InitializeComponent();
            ChangeFlag = false;
            if (changeDate)
            {
                FBPlanDateDateTimePicker.Enabled = true;
                label1.Text = "Mc No";
                MachineComboBox.Visible = true;
                daysFBTextBox.Visible = false;
                LoadMachine();
            }
            else
            {
                FBPlanDateDateTimePicker.Enabled = false;
                MachineComboBox.Visible = false;
                label1.Visible = true;
                daysFBTextBox.Visible = true;
            }
        }

        private void FBPlanBoardForm_Load(object sender, EventArgs e)
        {
            FBPlanDateDateTimePicker.Value = planDate;
            Orderlabel.Text = Flag == true ? "Backward Direction" : "Forward Direction";
        }

        private void LoadMachine()
        {
            try
            {

                string query = "SELECT * FROM Machine_Info WHERE Status = 1 AND MachineDia = (SELECT Top 1 DiaID FROM Planing_Board_Details WHERE MachineNo ="+MachineNo+") order by MachineNo asc";

                SqlDataReader reader = CommonFunctions.GetFromDB(query);
                MachineComboBox.DataSource = null;
                MachineComboBox.Items.Clear();
                MachineList.Clear();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        MachineList.Add(Convert.ToInt32(reader["MachineNo"]));
                    }
                }
                if (MachineList.Count > 0)
                {
                    MachineComboBox.DataSource = MachineList;
                    MachineComboBox.SelectedIndex = MachineList.IndexOf(MachineNo);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("" + e.ToString());
            }

            finally
            {
                CommonFunctions.connection.Close();
            }
        }

        private void IsInValidForForwarding()
        {
            try
            {
                //string query = "SELECT * FROM"
            }
            catch (Exception ee)
            {
                MessageBox.Show("" + ee.ToString());
            }
            finally
            {
                CommonFunctions.connection.Close();
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (FBPlanDateDateTimePicker.Enabled == false)
            {
                DateTime currentDate = DateTimeOffset.Now.Date;
                if (daysFBTextBox.Text == "" || Convert.ToInt32(daysFBTextBox.Text) < 1)
                {
                    MessageBox.Show("Please Enter Valid Number!!!!");
                    daysFBTextBox.Text = "";
                    return;
                }
                if (CommonFunctions.recordExist("SELECT * FROM PlanTable WHERE MachineNo = " + MachineNo + " AND ActualQty != 0 AND TaskDate >= '" + FBPlanDateDateTimePicker.Value + "'"))
                {
                    MessageBox.Show("Forwarding or Backwording Plan is not applicable for the orders where Auto Adjustment for Actual Qty has been used applied !!!!");
                    daysFBTextBox.Text = "";
                    return;
                }
                if(Flag == false)
                {
                    int daysAdded = Convert.ToInt16(daysFBTextBox.Text);

                    if (currentDate > FBPlanDateDateTimePicker.Value.AddDays(-daysAdded).Date)
                    {
                        MessageBox.Show("Invalid Backwarding Plan! You can not backward date smaller than current date !!!!");
                        daysFBTextBox.Text = "";
                        return;
                    }

                    while (!CommonFunctions.recordExist("SELECT * FROM WorkingDays WHERE MachineNo = " + MachineNo + " AND Active = 1 AND WorkDate = '" + FBPlanDateDateTimePicker.Value.AddDays(-daysAdded) + "'"))
                    {
                        daysAdded++;
                    }
                    if (CommonFunctions.recordExist("SELECT * FROM PlanTable WHERE MachineNo = " + MachineNo + " AND TaskDate = '" + FBPlanDateDateTimePicker.Value.AddDays(-daysAdded) + "'"))
                    {
                        MessageBox.Show("Invalid Backwarding Plan! Order already exists in that Date !!!!");
                        daysFBTextBox.Text = "";
                        return;
                    }
                    if (CommonFunctions.recordExist("SELECT * FROM PlanTable WHERE MachineNo = " + MachineNo + " AND TaskDate < '" + FBPlanDateDateTimePicker.Value + "' AND TaskDate > '" + FBPlanDateDateTimePicker.Value.AddDays(-daysAdded) + "'"))
                    {
                        MessageBox.Show("Invalid Backwarding Plan! You can not jump over another dates while forwarding !!!!");
                        daysFBTextBox.Text = "";
                        return;
                    }
                }
                //else
                //{
                //    if (CommonFunctions.recordExist("SELECT * FROM WorkingDays WHERE MachineNo = " + MachineNo + " AND WorkDate = '" + FBPlanDateDateTimePicker.Value.AddDays(Convert.ToInt16(daysFBTextBox.Text)) + "'"))
                //    {
                //        MessageBox.Show("Invalid Backwording Plan!!!");
                //        daysFBTextBox.Text = "";
                //        return;
                //    }
                //}
                ChangeFlag = true;
                ReGenerate_Board();
            }
            else
            {
                DateTime TaskDate = FBPlanDateDateTimePicker.Value;
                if (CommonFunctions.recordExist("SELECT * FROM WorkingDays WHERE MachineNo = " + MachineComboBox.SelectedItem + " and WorkDate = '" + TaskDate + "'"))
                {
                    if (CommonFunctions.recordExist("SELECT * FROM WorkingDays WHERE MachineNo = " + MachineComboBox.SelectedItem + " AND Active = 1 AND WorkDate = '" + TaskDate + "'"))
                    {
                        if (!CommonFunctions.recordExist("SELECT * FROM PlanTable WHERE  RevertVal !=  1 AND MachineNo = " + MachineComboBox.SelectedItem + " AND TaskDate = '" + TaskDate + "'"))
                        {
                            try
                            {
                                ChangeFlag = true;
                                int Capacity = 0;
                                int PlanQty = 0;
                                string GetDate = "";
                                int Efficiency = 0;
                                double SAM = 0;
                                int GetCount = 1;
                                int TotalPlanQty = 0;
                                string query = "select (SELECT COUNT (Id) from PlanTable where TaskDate = '" + planDate.ToString() + "' and MachineNo='" + MachineNo + "') as RecordCount, Id, MachineNo, TaskDate, OrderID, Capacity, PlanQty, RemainingQty, OrderQty, Efficiency, Minute, Status, SAM from PlanTable where TaskDate = '" + planDate.ToString() + "' and MachineNo='" + MachineNo + "' order by TaskDate,Id asc";
                                string connectionStr = ConnectionManager.connectionString;
                                SqlCommand cm = new SqlCommand();
                                SqlConnection cn = new SqlConnection(connectionStr);
                                SqlCommand cm1 = new SqlCommand();
                                SqlConnection cn1 = new SqlConnection(connectionStr);
                                SqlCommand cm2 = new SqlCommand();
                                SqlConnection cn2 = new SqlConnection(connectionStr);
                                SqlDataReader reader, reader1;
                                cn.Open();
                                cm.Connection = cn;

                                cm.CommandText = query;
                                reader = cm.ExecuteReader();
                                while (reader.Read())
                                {
                                    int GetMinute = 0;
                                    cn1.Open();
                                    cm1.Connection = cn1;
                                    cm1.CommandText = "SELECT Minute FROM WorkingDays where MachineNo='" + MachineComboBox.SelectedItem + "' and Active = 1 and WorkDate='" + TaskDate + "'";
                                    reader1 = cm1.ExecuteReader();
                                    while (reader1.Read())
                                    {
                                        GetMinute = Convert.ToInt16(reader1["Minute"]);
                                    }
                                    cn1.Close();

                                    SAM = SAM + Convert.ToDouble(reader["SAM"]);
                                    Efficiency = Efficiency + Convert.ToInt16(reader["Efficiency"]);
                                    SAM = SAM / GetCount;
                                    Efficiency = Convert.ToInt32(Math.Floor((Double)(Efficiency / GetCount)));
                                    Capacity = (int)(Math.Floor((GetMinute * Efficiency / 100) / SAM));
                                    PlanQty = Convert.ToInt16(reader["PlanQty"]);
                                    TotalPlanQty = TotalPlanQty + PlanQty;
                                    PlanQty = Convert.ToInt32(reader["RecordCount"]) - 1 == GetCount ? TotalPlanQty > Capacity ? PlanQty - (TotalPlanQty - Capacity) : PlanQty : PlanQty;

                                    cn2.Open();
                                    cm2.Connection = cn2;
                                    cm2.CommandText = "UPDATE PlanTable SET MachineNo = " + MachineComboBox.SelectedItem + ", TaskDate='" + TaskDate + "',Capacity='" + Capacity + "',RemainingQty='" + (Capacity - PlanQty) + "',Status=1 where Id='" + Convert.ToInt16(reader["Id"]) + "'";
                                    cm2.ExecuteReader();
                                    cn2.Close();

                                    GetCount++;
                                }
                                cn.Close();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            finally
                            {
                                ResetStatus();
                                this.Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Orders exist in that date! Please Choose Another Date!!!");
                            FBPlanDateDateTimePicker.Value = planDate;
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Orders can not be placed on Holiday! Please Choose Another Date!!!");
                        FBPlanDateDateTimePicker.Value = planDate;
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("This Date has not been scheduled yet for this Machine! Please First Create Schedule this date for this machine!!!");
                    FBPlanDateDateTimePicker.Value = planDate;
                    return;
                }
            }  
        }

        private void ReGenerate_Board()
        {
            string connectionStr = ConnectionManager.connectionString;
            SqlCommand cm = new SqlCommand();
            SqlConnection cn = new SqlConnection(connectionStr);
            SqlCommand cm1 = new SqlCommand();
            SqlConnection cn1 = new SqlConnection(connectionStr);
            SqlCommand cm2= new SqlCommand();
            SqlConnection cn2 = new SqlConnection(connectionStr);
            SqlCommand cm3 = new SqlCommand();
            SqlConnection cn3 = new SqlConnection(connectionStr);
            SqlCommand cm4 = new SqlCommand();
            SqlConnection cn4 = new SqlConnection(connectionStr);
            SqlCommand cm5 = new SqlCommand();
            SqlConnection cn5 = new SqlConnection(connectionStr);
            SqlCommand cm6 = new SqlCommand();
            SqlConnection cn6 = new SqlConnection(connectionStr);

            cn.Open();
            cm.Connection = cn;
            cn1.Open();
            cm1.Connection = cn1;
            cn2.Open();
            cm2.Connection = cn2;
            cn3.Open();
            cm3.Connection = cn3;

            int Getdays = 0;
 
            DateTime TargateDate = DateTime.Now;

            try
            {
                string orderByString = Flag == true ? "desc" : "asc";
                query = "select top(1) TaskDate from PlanTable where MachineNo='"+MachineNo+"' AND Capacity != 0 order by TaskDate desc";
                //query = "select top(1) TaskDate from PlanTable where MachineNo='"+MachineNo+"' AND Capacity != 0 order by TaskDate " + orderByString;
                cm.CommandText = query;
                SqlDataReader reader;
                reader = cm.ExecuteReader();
                while (reader.Read())
                {
                    string date = Convert.ToDateTime(reader["TaskDate"]).ToString("dd/MM/yyyy");
                    TargateDate = DateTime.ParseExact(date, "dd/MM/yyyy", null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                cn.Close();
            }
            try
            {
                query = "UPDATE PlanTable SET Status='0' where MachineNo='" + MachineNo + "' and TaskDate>='" + planDate + "'";
                //query = Flag == true ? "UPDATE PlanTable SET Status='0' where MachineNo='"+MachineNo+"' and TaskDate>='" + planDate + "'" : "update PlanTable set Status='0' where MachineNo='"+MachineNo+"' and TaskDate <='" + planDate + "'";
                cm1.CommandText = query;
                cm1.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                cn1.Close();
            }

            Getdays = Flag == true ? Convert.ToInt16(daysFBTextBox.Text) : -Convert.ToInt16(daysFBTextBox.Text);

            try
            {
                int GetMinute = 0;
                int Capacity = 0;
                int PlanQty = 0;
                int TotalPlanQty = 0;
                string GetDate = "";
                int Efficiency = 0;
                double SAM = 0;
                int GetCount = 1;
                Boolean FirstTime = true;
                DateTime PrevUpdatedDate = DateTime.MinValue;
                DateTime PrevOldDate = DateTime.MinValue;
                Boolean DateDifference = false;
                query = "SELECT Id, MachineNo, TaskDate, OrderID, Capacity, PlanQty, RemainingQty, OrderQty, Efficiency, Minute, Status, SAM from PlanTable where TaskDate between '" + planDate.ToString() + "' and '" + TargateDate.ToString() + "' and MachineNo='" + MachineNo + "' and Status=0 order by TaskDate,Id asc";
                //query = Flag == true ? "SELECT Id, MachineNo, TaskDate, OrderID, Capacity, PlanQty, RemainingQty, OrderQty, Efficiency, Minute, Status, SAM from PlanTable where TaskDate between '" + planDate.ToString() + "' and '" + TargateDate.ToString() + "' and MachineNo='"+MachineNo+"' and Status=0 order by TaskDate,Id asc" : "select Id, MachineNo, TaskDate, OrderID, Capacity, PlanQty, RemainingQty, OrderQty, Efficiency, Minute, Status, SAM from PlanTable where TaskDate between '" + TargateDate.ToString() + "' and '" + planDate.ToString() + "' and MachineNo='"+MachineNo+"' and Status=0 order by TaskDate desc, Id asc";
                cm2.CommandText = query;
                SqlDataReader reader2;
                reader2 = cm2.ExecuteReader();

                while (reader2.Read())
                {
                    if (Convert.ToInt32(reader2["Capacity"]) != 0)
                    {
                        //For Forward && Backward
                        if (GetDate != "")
                        {
                            if (GetDate != Convert.ToString(reader2["TaskDate"]))
                            {
                                PlanQty = 0;
                                TotalPlanQty = 0;
                                Efficiency = 0;
                                SAM = 0;
                                GetCount = 1;
                                PrevOldDate = DateTime.MinValue;
                            }
                        }
                        try
                        {
                            GetMinute = 0;
                            DateTime TempDate = DateTime.Now;
                            int diff = FirstTime == true ? 0 : (Convert.ToDateTime(reader2["TaskDate"]).Date - Convert.ToDateTime(GetDate).Date).Days;
                            //int diff = FirstTime == true ? 0 : Flag == true ? (Convert.ToDateTime(reader2["TaskDate"]).Date - Convert.ToDateTime(GetDate).Date).Days - 1 : (Convert.ToDateTime(GetDate).Date - Convert.ToDateTime(reader2["TaskDate"]).Date).Days + 1;
                            DateDifference = FirstTime == true ? false : GetDateDifference(Convert.ToDateTime(GetDate), (Convert.ToDateTime(reader2["TaskDate"])), diff);

                            Getdays = GetCount == 1 ? CalculateDays(Convert.ToDateTime(reader2["TaskDate"]), PrevUpdatedDate, GetCount, FirstTime, Getdays, DateDifference) : Getdays;
                            
                            cm3.CommandText = "SELECT Minute FROM WorkingDays WHERE MachineNo='"+MachineNo+"' and Active = 1 and WorkDate='" + Convert.ToDateTime(reader2["TaskDate"]).AddDays(Getdays) + "'";
                            SqlDataReader reader3;
                            reader3 = cm3.ExecuteReader();
                            while (reader3.Read())
                            {
                                GetMinute = Convert.ToInt16(reader3["Minute"]);
                            }
                            reader3.Dispose();
                        }
                        
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }

                        SAM = SAM + Convert.ToDouble(reader2["SAM"]);
                        Efficiency = Efficiency + Convert.ToInt16(reader2["Efficiency"]);
                        SAM = SAM / GetCount;
                        Efficiency = Convert.ToInt32(Math.Floor((Double)(Efficiency / GetCount)));
                        Capacity = Convert.ToInt32(Math.Floor((GetMinute * Efficiency / 100) / SAM));
                        PlanQty = Convert.ToInt32(reader2["PlanQty"]);
                        TotalPlanQty = TotalPlanQty + PlanQty;
                        PlanQty = TotalPlanQty > Capacity ? PlanQty - (TotalPlanQty - Capacity) : PlanQty;

                        try
                        {
                            cn4.Open();
                            cm4.Connection = cn4;

                            cm4.CommandText = "UPDATE PlanTable SET TaskDate ='" + Convert.ToDateTime(reader2["TaskDate"]).AddDays(Getdays) + "', Capacity ='" + Capacity + "', RemainingQty ='" + (Capacity - PlanQty) + "', Status = 1 where Id='" + Convert.ToInt16(reader2["Id"]) + "'";
                            cm4.ExecuteReader();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            DateDifference = false;
                            DifferenceInDays = 1;
                            cn4.Close();
                            PrevUpdatedDate = Convert.ToDateTime(reader2["TaskDate"]).AddDays(Getdays);
                            
                        }

                        GetDate = Convert.ToString(reader2["TaskDate"]);
                        GetCount++;
                        FirstTime = false;
                    }
                }
                reader2.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                cn2.Close();
                cn3.Close();
                ResetStatus();
                this.Close();     
            }
        }

        private int CalculateDays(DateTime CurrentDate, DateTime PrevUpdatedDate, int GetCount, bool FirstTime, int InputDays, Boolean DateDifference)
         {
            DateTime TempDate = DateTime.Now;

            if (!FirstTime)
            {
                if (DateDifference == false)
                {
                    PrevUpdatedDate = PrevUpdatedDate.AddDays(1);
                    //PrevUpdatedDate = Flag == true ? PrevUpdatedDate.AddDays(1) : PrevUpdatedDate.AddDays(-1);
                    TempDate = Flag == true ? CurrentDate.AddDays(1) : CurrentDate.AddDays(-1);
                    if (!Flag)
                    {
                        while (!CommonFunctions.recordExist("select * from WorkingDays where MachineNo='" + MachineNo + "' and Active = 1 and WorkDate='" + PrevUpdatedDate + "'"))
                        {
                            PrevUpdatedDate = PrevUpdatedDate.AddDays(1);
                            //PrevUpdatedDate = Flag == true ? PrevUpdatedDate.AddDays(1) : PrevUpdatedDate.AddDays(-1);
                        }
                    }
                }

                if (Flag)
                {
                    while (!CommonFunctions.recordExist("select * from WorkingDays where MachineNo='" + MachineNo + "' and Active = 1 and WorkDate='" + PrevUpdatedDate + "'"))
                    {
                        PrevUpdatedDate = PrevUpdatedDate.AddDays(1);
                        //PrevUpdatedDate = Flag == true ? PrevUpdatedDate.AddDays(1) : PrevUpdatedDate.AddDays(-1);
                    }
                }

                if (!DateDifference)
                    TempDate = Flag == true ? TempDate < PrevUpdatedDate ? PrevUpdatedDate : TempDate : TempDate > PrevUpdatedDate ? PrevUpdatedDate : TempDate;

                else
                {
                    while (DifferenceInDays > 1)
                    {
                        PrevUpdatedDate = PrevUpdatedDate.AddDays(1);
                        if (CommonFunctions.recordExist("select * from WorkingDays where MachineNo='" + MachineNo + "' and Active = 1 and WorkDate='" + PrevUpdatedDate + "'"))
                        {
                            DifferenceInDays--;
                        }
                    }
                    TempDate = PrevUpdatedDate.AddDays(1);
                }

                //TempDate = DateDifference == true ? PrevUpdatedDate.AddDays(DifferenceInDays) // if DateDifference is true
                //           : Flag == true ? TempDate < PrevUpdatedDate ? PrevUpdatedDate : TempDate : TempDate > PrevUpdatedDate ? PrevUpdatedDate : TempDate;


                //TempDate = DateDifference == true ? Flag == true ? PrevUpdatedDate.AddDays(DifferenceInDays) : PrevUpdatedDate.AddDays(-DifferenceInDays) // if DateDifference is true
                //           : Flag == true ? TempDate < PrevUpdatedDate ? PrevUpdatedDate : TempDate : TempDate > PrevUpdatedDate ? PrevUpdatedDate : TempDate; // if DateDifference is false
            }
            else
            {
                TempDate = CurrentDate.AddDays(InputDays);
            }

            while (!CommonFunctions.recordExist("select * from WorkingDays where MachineNo='"+MachineNo+"' and Active = 1 and WorkDate='" + TempDate + "'"))
            {
                if(DateDifference)
                    TempDate = TempDate.AddDays(1);
                else
                    TempDate = Flag == true ? TempDate.AddDays(1) : TempDate.AddDays(-1);
                //TempDate = Flag == true ? TempDate.AddDays(1) : TempDate.AddDays(-1);
            }

            return Flag == true ? (TempDate.Date - CurrentDate.Date).Days : -(CurrentDate.Date - TempDate.Date).Days;
        }

        private bool GetDateDifference(DateTime PrevOldDate, DateTime CurrentDate, int diff)
        {
            if (PrevOldDate == DateTime.MinValue)
            {
                return false;
            }

            if (diff > 1)
            {
                DifferenceInDays = 1;
                DateTime tempDate = PrevOldDate.AddDays(1);
                //DateTime tempDate = Flag == true ? PrevOldDate.AddDays(1) : PrevOldDate.AddDays(-1);

                while (tempDate < CurrentDate)
                {
                    if (CommonFunctions.recordExist("SELECT * FROM WorkingDays WHERE MachineNo = '" + MachineNo + "' and WorkDate = '" + tempDate + "' and Active = 1 "))
                    {
                        DifferenceInDays++;
                    }
                    tempDate = tempDate.AddDays(1);
                }

                //if (Flag) // Backward
                //{
                //    while (tempDate < CurrentDate)
                //    {
                //        if (CommonFunctions.recordExist("SELECT * FROM WorkingDays WHERE MachineNo = '"+MachineNo+"' and WorkDate = '" + tempDate + "' and Active = 1 "))
                //        {
                //            DifferenceInDays++;
                //        }
                //        tempDate = tempDate.AddDays(1);
                //    }
                //}
                //else // Forward
                //{
                //    while (tempDate > CurrentDate)
                //    {
                //        if (CommonFunctions.recordExist("SELECT * FROM WorkingDays WHERE MachineNo = '"+MachineNo+"' and WorkDate = '" + tempDate + "' and Active = 1 "))
                //        {
                //            DifferenceInDays++;
                //        }
                //        tempDate = tempDate.AddDays(-1);
                //    }
                //}
            }
            else
            {
                return false;
            }

            return DifferenceInDays > 1 ? true : false ;
        }

        private void ResetStatus()
        {
            string connectionStr = ConnectionManager.connectionString;
            SqlCommand cm = new SqlCommand();
            SqlConnection cn = new SqlConnection(connectionStr);
            cn.Open();
            cm.Connection = cn;
            cm.CommandText = "UPDATE PlanTable SET Status = 0";
            cm.ExecuteReader();
            cn.Close();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void daysFBTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void FBPlanDateDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (FBPlanDateDateTimePicker.Value < DateTimeOffset.UtcNow.Date)
            {
                MessageBox.Show("You can not switch date smaller tha current date!!!");
                FBPlanDateDateTimePicker.Value = DateTimeOffset.UtcNow.Date;
                return;
            }
        }

        private void Orderlabel_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void MachineComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
