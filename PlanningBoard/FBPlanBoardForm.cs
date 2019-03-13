using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SqlClient;

namespace PlanningBoard
{
    public partial class FBPlanBoardForm : Form
    {

        public Boolean Flag = false;
        public DateTime planDate = DateTime.Now;
        string orderids = "";
        int DifferenceInDays = 1;
        string query = "";
        public int MachineNo = 0;

        public FBPlanBoardForm(bool flag, int mcNo, DateTime taskDate, string Ids, bool changeDate)
        {
            Flag = flag;
            planDate = DateTime.ParseExact(taskDate.Date.ToString("dd/MM/yyy"), "dd/MM/yyyy", null);
            orderids = Ids;
            MachineNo = mcNo;
            InitializeComponent();
            if (changeDate)
            {
                FBPlanDateDateTimePicker.Enabled = true;
                label1.Visible = false;
                daysFBTextBox.Visible = false;
            }
            else
            {
                FBPlanDateDateTimePicker.Enabled = false;
                label1.Visible = true;
                daysFBTextBox.Visible = true;
            }
        }

        private void FBPlanBoardForm_Load(object sender, EventArgs e)
        {
            FBPlanDateDateTimePicker.Value = planDate;
            Orderlabel.Text = "Order ID : " + orderids;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (FBPlanDateDateTimePicker.Enabled == false)
            {
                if (daysFBTextBox.Text == "" || Convert.ToInt32(daysFBTextBox.Text) < 1)
                {
                    MessageBox.Show("Please Enter Valid Number!!!!");
                    daysFBTextBox.Text = "";
                    return;
                }
                ReGenerate_Board();
            }
            else
            {
                if(!CommonFunctions.recordExist("SELECT * FROM PlanTable WHERE MachineNo = "+MachineNo+" and TaskDate = '"+planDate+"'"))
                {
                    int Capacity = 0;
                    int PlanQty = 0;
                    string GetDate = "";
                    int Efficiency = 0;
                    double SAM = 0;
                    int GetCount = 1;
                    string query = "select Id, MachineNo, TaskDate, OrderID, Capacity, PlanQty, RemainingQty, OrderQty, ColIndex, Efficiency, Minute, Status,(select SAM from Order_Info where Id=OrderID) as SAM from PlanTable where TaskDate = '" + planDate.ToString() + "' and MachineNo='" + MachineNo + "' and Status=0 order by TaskDate,Id asc";
                    string connectionStr = ConnectionManager.connectionString;
                    SqlCommand cm = new SqlCommand();
                    SqlConnection cn = new SqlConnection(connectionStr);
                    SqlCommand cm1 = new SqlCommand();
                    SqlConnection cn1 = new SqlConnection(connectionStr);
                    SqlCommand cm2 = new SqlCommand();
                    SqlConnection cn2 = new SqlConnection(connectionStr);
                    SqlDataReader reader, reader1, reader2;
                    cn.Open();
                    cm.Connection = cn;
                    cn1.Open();
                    cm1.Connection = cn1;
                    cn2.Open();
                    cm2.Connection = cn2;

                    cm.CommandText = query;
                    reader = cm.ExecuteReader();
                    while (reader.Read())
                    {
                        int GetMinute = 0;
                        cm1.CommandText = "SELECT Minute FROM WorkingDays where MachineNo='" + MachineNo + "' and Active = 1 and WorkDate='" + Convert.ToDateTime(reader["TaskDate"]) + "'";
                        reader1 = cm1.ExecuteReader();
                        while (reader1.Read())
                        {
                            GetMinute = Convert.ToInt16(reader1["Minute"]);
                        }

                        SAM = SAM + Convert.ToDouble(reader["SAM"]);
                        Efficiency = Efficiency + Convert.ToInt16(reader["Efficiency"]);
                        SAM = SAM / GetCount;
                        Efficiency = Convert.ToInt32(Math.Floor((Double)(Efficiency / GetCount)));
                        Capacity = (int)(Math.Floor((GetMinute * Efficiency / 100) / SAM));
                        PlanQty = PlanQty + Convert.ToInt16(reader["PlanQty"]);
                        
                        cm2.CommandText = "UPDATE PlanTable SET TaskDate='" + Convert.ToDateTime(reader["TaskDate"]) + "',Capacity='" + Capacity + "',RemainingQty='" + (Capacity - PlanQty) + "',Status=1 where Id='" + Convert.ToInt16(reader["Id"]) + "'";
                        cm2.ExecuteReader();

                        GetCount++;
                    }
                    
                    cn.Close();
                    cn1.Close();
                    cn2.Close();
                }
                else
                {
                    MessageBox.Show("Orders Exist in that date! Please Choose Another Date!!!");
                    FBPlanDateDateTimePicker.Value = DateTime.Now;
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
                query = "select top(1) TaskDate from PlanTable where MachineNo='"+MachineNo+"' AND Capacity != 0 order by TaskDate " + orderByString;
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
                query = Flag == true ? "update PlanTable set Status='0' where MachineNo='"+MachineNo+"' and TaskDate>='" + planDate + "'" : "update PlanTable set Status='0' where MachineNo='"+MachineNo+"' and TaskDate <='" + planDate + "'";
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

            if (Flag == true)
            {
                Getdays = Convert.ToInt16(daysFBTextBox.Text);
            }
            else
            {
                Getdays = -Convert.ToInt16(daysFBTextBox.Text);
            }

            try
            {
                int GetMinute = 0;
                int Capacity = 0;
                int PlanQty = 0;
                string GetDate = "";
                int Efficiency = 0;
                double SAM = 0;
                int GetCount = 1;
                int OneTime = 0;
                Boolean FirstTime = true;
                DateTime PrevUpdatedDate = DateTime.MinValue;
                DateTime PrevOldDate = DateTime.MinValue;
                Boolean DateDifference = false;

                query = Flag == true ? "select Id, MachineNo, TaskDate, OrderID, Capacity, PlanQty, RemainingQty, OrderQty, ColIndex, Efficiency, Minute, Status,(select SAM from Order_Info where Id=OrderID) as SAM from PlanTable where TaskDate between '" + planDate.ToString() + "' and '" + TargateDate.ToString() + "' and MachineNo='"+MachineNo+"' and Status=0 order by TaskDate,Id asc" : "select Id, MachineNo, TaskDate, OrderID, Capacity, PlanQty, RemainingQty, OrderQty, ColIndex, Efficiency, Minute, Status,(select SAM from Order_Info where Id=OrderID) as SAM from PlanTable where TaskDate between '" + TargateDate.ToString() + "' and '" + planDate.ToString() + "' and MachineNo='"+MachineNo+"' and Status=0 order by TaskDate desc, Id asc";
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
                            //query = " Select Top 2 * from PlanTable where TaskDate between '" + planDate.ToString() + "' and '" + reader2["TaskDate"].ToString() + "' and MachineNo = '"+MachineNo+"' and Capacity != 0 and Status = 0 order by TaskDate,Id asc";
                            int diff = FirstTime == true ? 0 : Flag == true ? (Convert.ToDateTime(reader2["TaskDate"]).Date - Convert.ToDateTime(GetDate).Date).Days - 1 : (Convert.ToDateTime(GetDate).Date - Convert.ToDateTime(reader2["TaskDate"]).Date).Days - 1;
                            DateDifference = FirstTime == true ? false : GetDateDifference(Convert.ToDateTime(GetDate), (Convert.ToDateTime(reader2["TaskDate"])), diff);
                            
                            //if (GetCount == 1)
                            //{
                            //    int RecordCount = 0;
                            //    int TempDays = 1;

                            //    if (OneTime == 0)
                            //    {
                            //        if (Getdays > 1)
                            //            TempDays = Getdays;
                            //        else
                            //            TempDays = 1;
                                    
                            //        OneTime = 1;
                            //    }

                            //    //if (Flag == true)
                            //    //    TempDate = Convert.ToDateTime(reader2["TaskDate"]).AddDays(TempDays);
                            //    //else
                            //    //    TempDate = Convert.ToDateTime(reader2["TaskDate"]).AddDays(-TempDays);

                            //    while (!CommonFunctions.recordExist("select * from WorkingDays where MachineNo='"+MachineNo+"' and Active = 1 and WorkDate='" + TempDate + "'"))
                            //    {
                            //        if (Flag == true)
                            //        {
                            //            TempDate = TempDate.AddDays(1);
                            //            RecordCount = RecordCount + 1;
                            //        }
                            //        else
                            //        {
                            //            TempDate = TempDate.AddDays(-1);
                            //        }
                            //    }   
                            //    try
                            //    {
                            //        cn6.Open();
                            //        cm6.Connection = cn6;
                            //        cm6.CommandText = "select distinct(TaskDate) from PlanTable where MachineNo='"+MachineNo+"' and TaskDate>'" + Convert.ToDateTime(reader2["TaskDate"]) + "' and TaskDate<=(select top(1) TaskDate from PlanTable where MachineNo='"+MachineNo+"' and status=1 and TaskDate>'" + Convert.ToDateTime(reader2["TaskDate"]) + "')";
                            //        SqlDataReader reader6;
                            //        reader6 = cm6.ExecuteReader();
                            //        while (reader6.Read())
                            //        {
                            //            if (Flag == true)
                            //            {
                            //                RecordCount = RecordCount + 1;
                            //            }
                            //        }
                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        MessageBox.Show(ex.Message);
                            //    }
                            //    finally
                            //    {
                            //        cn6.Close();
                            //    }

                            //    if (Flag == true)
                            //    {
                            //        if ((OneTime == 1) && (RecordCount != 0))
                            //            Getdays = Getdays + RecordCount;
                            //        else if (RecordCount != 0)
                            //            Getdays = RecordCount + 1;
                            //        else
                            //            if ((OneTime != 0) && (OneTime != 1))
                            //                Getdays = 1;
                            //            else
                            //                OneTime = 2;
                                            
                            //    }
                            //    else
                            //    {
                            //        if (RecordCount != 0)
                            //            Getdays =-(RecordCount + 1);
                            //        else
                            //            Getdays = -1;
                            //    }
                            //}
                            
                            //Getdays = (TempDate.Date - Convert.ToDateTime(reader2["TaskDate"]).Date).Days;

                            Getdays = GetCount == 1 ? CalculateDays(Convert.ToDateTime(reader2["TaskDate"]), PrevUpdatedDate, GetCount, FirstTime, Getdays, DateDifference) : Getdays;
                            
                            cm3.CommandText = "select Minute from WorkingDays where MachineNo='"+MachineNo+"' and Active = 1 and WorkDate='" + Convert.ToDateTime(reader2["TaskDate"]).AddDays(Getdays) + "'";
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

                        Capacity = (int)(Math.Floor((GetMinute * Efficiency / 100) / SAM));
                        PlanQty = PlanQty + Convert.ToInt16(reader2["PlanQty"]);
                        try
                        {
                            cn4.Open();
                            cm4.Connection = cn4;

                            cm4.CommandText = "update PlanTable set TaskDate='" + Convert.ToDateTime(reader2["TaskDate"]).AddDays(Getdays) + "',Capacity='" + Capacity + "',RemainingQty='" + (Capacity - PlanQty) + "',Status=1 where Id='" + Convert.ToInt16(reader2["Id"]) + "'";
                            cm4.ExecuteReader();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        finally
                        {
                            DateDifference = false;
                            DifferenceInDays = 0;
                            cn4.Close();
                            PrevUpdatedDate = Convert.ToDateTime(reader2["TaskDate"]).AddDays(Getdays);
                            
                        }
                        //Getdays = 1;
                        GetDate = Convert.ToString(reader2["TaskDate"]);
                        //PrevOldDate = Convert.ToDateTime(reader2["TaskDate"]);
                        
                        GetCount++;
                        FirstTime = false;
                    }
                    //else
                    //{
                    //    if (Flag == true)//Backward
                    //    {
                    //        if (Getdays == 1)
                    //            Getdays = 1;
                    //        else if (Getdays > 1)
                    //            Getdays = Getdays - 1;
                    //    }
                    //    else//Forward
                    //    {
                    //        Getdays = Getdays - 1;
                    //    }

                    //}
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
                PrevUpdatedDate = Flag == true ? PrevUpdatedDate.AddDays(1) : PrevUpdatedDate.AddDays(-1);
                TempDate = Flag == true ? CurrentDate.AddDays(1) : CurrentDate.AddDays(-1);

                while (!CommonFunctions.recordExist("select * from WorkingDays where MachineNo='"+MachineNo+"' and Active = 1 and WorkDate='" + PrevUpdatedDate + "'"))
                {
                    PrevUpdatedDate = Flag == true ? PrevUpdatedDate.AddDays(1) : PrevUpdatedDate.AddDays(-1);
                }

                TempDate = DateDifference == true ? Flag == true ? PrevUpdatedDate.AddDays(DifferenceInDays) : PrevUpdatedDate.AddDays(-DifferenceInDays) // if DateDifference is true
                           : Flag == true ? TempDate < PrevUpdatedDate ? PrevUpdatedDate : TempDate : TempDate > PrevUpdatedDate ? PrevUpdatedDate : TempDate; // if DateDifference is false
            }
            else
            {
                TempDate = CurrentDate.AddDays(InputDays);
            }

            while (!CommonFunctions.recordExist("select * from WorkingDays where MachineNo='"+MachineNo+"' and Active = 1 and WorkDate='" + TempDate + "'"))
            {
                TempDate = Flag == true ? TempDate.AddDays(1) : TempDate.AddDays(-1);
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
                DifferenceInDays = 0;
                DateTime tempDate = Flag == true ? PrevOldDate.AddDays(1) : PrevOldDate.AddDays(-1);

                if (Flag) // Backward
                {
                    while (tempDate < CurrentDate)
                    {
                        if (CommonFunctions.recordExist("SELECT * FROM WorkingDays WHERE MachineNo = '"+MachineNo+"' and WorkDate = '" + tempDate + "' and Active = 1 "))
                        {
                            DifferenceInDays++;
                        }
                        tempDate = tempDate.AddDays(1);
                    }
                }
                else // Forward
                {
                    while (tempDate > CurrentDate)
                    {
                        if (CommonFunctions.recordExist("SELECT * FROM WorkingDays WHERE MachineNo = '"+MachineNo+"' and WorkDate = '" + tempDate + "' and Active = 1 "))
                        {
                            DifferenceInDays++;
                        }
                        tempDate = tempDate.AddDays(-1);
                    }
                }
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
        
        }

        private void Orderlabel_Click(object sender, EventArgs e)
        {

        }
    }
}
