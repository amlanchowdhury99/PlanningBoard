﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Collections;
using System.Globalization;

namespace PlanningBoard
{
    public partial class UpdateActualQtyForm : Form
    {
        public int PreVal = -1;
        public Dictionary<int, int> PreValStore = new Dictionary<int, int>();
        public string OrderIDs = "";
        public DateTime TaskDate = DateTime.Now;
        public int MachineNo = 0;
        public int value = 0;
        public static Boolean ChangeFlag = false;
        public Boolean NoProduction = false;

        public UpdateActualQtyForm(string orderIds, int mcNo, DateTime Date, Boolean NoPro)
        {
            ChangeFlag = false;
            NoProduction = NoPro;
            OrderIDs = orderIds;
            TaskDate = Date;
            MachineNo = mcNo;
            InitializeComponent();
            LoadActualQtyDataGridView();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void LoadActualQtyDataGridView()
        {
            try
            {
                int SL = 1;
                string S1 = ""; string S2 = ""; string S3 = ""; string S4 = ""; string S5 = ""; string S6 = ""; string S7 = ""; string S8 = ""; string S9 = ""; Boolean S10 = true;
                string query = "SELECT BuyerName, StyleName, SizeName, Dia, PartName, PlanQty, ActualQty, OrderID, Production FROM Planing_Board_Details WHERE MachineNo = " + MachineNo + " AND TaskDate = '" + TaskDate + "'";
                SqlDataReader reader = CommonFunctions.GetFromDB(query);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int production = -1; int actualQty = 0;

                        S1 = SL.ToString();
                        S2 = reader.IsDBNull(reader.GetOrdinal("BuyerName")) == true ? "Not Defined" : reader["BuyerName"].ToString();
                        S3 = reader.IsDBNull(reader.GetOrdinal("StyleName")) == true ? "Not Defined" : reader["StyleName"].ToString();
                        S4 = reader.IsDBNull(reader.GetOrdinal("SizeName")) == true ? "Not Defined" : reader["SizeName"].ToString();
                        S5 = reader.IsDBNull(reader.GetOrdinal("Dia")) == true ? "Not Defined" : reader["Dia"].ToString();
                        S6 = reader.IsDBNull(reader.GetOrdinal("PartName")) == true ? "Not Defined" : reader["PartName"].ToString();
                        S7 = reader.IsDBNull(reader.GetOrdinal("PlanQty")) == true ? "Not Defined" : reader["PlanQty"].ToString();
                        S8 = reader.IsDBNull(reader.GetOrdinal("ActualQty")) == true ? S7 : reader["ActualQty"].ToString();
                        S9 = reader.IsDBNull(reader.GetOrdinal("OrderID")) == true ? "Not Defined" : reader["OrderID"].ToString();
                        S10 = reader.IsDBNull(reader.GetOrdinal("Production")) == true ? true : Convert.ToInt32(reader["Production"]) == 1 ? true : false;

                        if (NoProduction)
                        {
                            actualQtyDataGridView.Columns["ActualQty"].Visible = false;
                            actualQtyDataGridView.Columns["PlanQty"].Visible = false;
                            actualQtyDataGridView.Columns["Production"].Visible = true;
                            if (!S10)
                            {
                                actualQtyDataGridView.Columns["Production"].ReadOnly = true;
                            }
                        }
                        else
                        {
                            string connectionStr = ConnectionManager.connectionString; SqlDataReader reader1, reader2;
                            SqlCommand cm = new SqlCommand(); SqlConnection cn = new SqlConnection(connectionStr); cm.Connection = cn; cn.Open();
                            SqlCommand cm1 = new SqlCommand(); SqlConnection cn1 = new SqlConnection(connectionStr); cm1.Connection = cn1; cn1.Open();
                            DateTime TempDate = TaskDate.AddDays(-1);
                            cm1.CommandText = "SELECT * FROM WorkingDays WHERE MachineNo = " + MachineNo + " AND Active = 1 AND WorkDate = '" + TempDate + "'";
                            while (!cm1.ExecuteReader().HasRows)
                            {
                                TempDate = TempDate.AddDays(-1);
                                cn1.Close();
                                cn1.Open();
                                cm1.CommandText = "SELECT * FROM WorkingDays WHERE MachineNo = " + MachineNo + " AND Active = 1 AND WorkDate = '" + TempDate + "'";
                            }
                            cn1.Close();

                            string query1 = " SELECT * FROM PlanTable WHERE TaskDate = '" + TempDate + "' AND OrderID = " + Convert.ToInt32(S9);
                            cm.CommandText = query1;
                            reader1 = cm.ExecuteReader();
                            if (reader1.HasRows)
                            {
                                while (reader1.Read())
                                {
                                    production = Convert.ToInt32(reader1["Production"]);
                                    actualQty = Convert.ToInt32(reader1["ActualQty"]);
                                    //if (production == 1 && actualQty == 0)
                                    //{
                                    //    actualQtyDataGridView.Rows[SL-1].Cells["ActualQty"].ReadOnly = true;
                                    //}
                                }
                            }
                            cn.Close();
                        }

                        actualQtyDataGridView.Rows.Add(S1, S2, S3, S4, S5, S6, S7, S8, S10, S9);

                        if (production == 1 && actualQty == 0)
                        {
                            actualQtyDataGridView.Rows[SL - 1].Cells["ActualQty"].ReadOnly = true;
                            actualQtyDataGridView.Rows[SL - 1].Cells["ActualQty"].Style.BackColor = Color.IndianRed;
                        }

                        SL++;
                        PreValStore.Add(Convert.ToInt32(S9), Convert.ToInt32(S8));
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("" + ee.ToString());
            }
            finally
            {
                if (CommonFunctions.connection.State == ConnectionState.Open)
                {
                    CommonFunctions.connection.Close();
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (actualQtyDataGridView.Rows.Count > 0)
                {
                    foreach (DataGridViewRow row in actualQtyDataGridView.Rows)
                    {
                        if (!NoProduction)
                        {
                            if (PreValStore[Convert.ToInt32(row.Cells["Id"].Value)] != Convert.ToInt32(row.Cells["ActualQty"].Value))
                            {
                                ChangeFlag = true;
                                int orderID = Convert.ToInt32(row.Cells["Id"].Value);
                                int PlanQty = Convert.ToInt32(row.Cells["PlanQty"].Value);
                                int ActualQty = Convert.ToInt32(row.Cells["ActualQty"].Value);
                                //int ActualQty = IsLastEntry(orderID, TaskDate, MachineNo) ? PlanQty > Convert.ToInt32(row.Cells["ActualQty"].Value) ? Convert.ToInt32(row.Cells["ActualQty"].Value) : PlanQty : Convert.ToInt32(row.Cells["ActualQty"].Value);

                                string query = "UPDATE PlanTable SET ActualQty = " + ActualQty + " WHERE OrderID = " + orderID + " AND MachineNo = " + MachineNo + " AND TaskDate = '" + TaskDate + "'";
                                Boolean Result = CommonFunctions.ExecutionToDB(query, 3);
                                CommonFunctions.connection.Close();

                                Boolean flag = PreValStore[orderID] == 0 ? ActualQty > PlanQty ? true : false : ActualQty > PreValStore[orderID] ? true : false;

                                if (flag && !IsLastEntry(orderID, TaskDate, MachineNo))
                                {
                                    value = IsLastEntry(orderID, TaskDate, MachineNo) ? 0 : PreValStore[orderID] == 0 ? ActualQty - PlanQty : ActualQty - PreValStore[orderID];
                                    AdjustActualQty(flag, value, orderID);
                                }
                                else
                                {
                                    value = PreValStore[orderID] == 0 ? PlanQty - ActualQty : PreValStore[orderID] - ActualQty;
                                    AdjustActualQty(flag, value, orderID);
                                }
                            }
                        }
                        else
                        {
                            ChangeFlag = true;
                            int orderID = Convert.ToInt32(row.Cells["Id"].Value); string query = "";
                            int PlanQty = Convert.ToInt32(row.Cells["PlanQty"].Value);

                            if (Convert.ToBoolean(row.Cells["Production"].Value) == false)
                            {
                                query = "UPDATE PlanTable SET ActualQty = 0, Production = 0 WHERE OrderID = " + orderID + " AND MachineNo = " + MachineNo + " AND TaskDate = '" + TaskDate + "'";
                                Boolean Result = CommonFunctions.ExecutionToDB(query, 3);
                                CommonFunctions.connection.Close();
                                AdjustActualQty(false, PlanQty, orderID);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("No Rows To Save!!!");
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("" + ee.ToString());
            }
            finally
            {
                this.Close();
            }
        }

        private ArrayList GetMachineList(int orderID)
        {
            ArrayList mcNo = new ArrayList();
            try
            {
                string query = "SELECT MachineNo FROM Machine_Info WHERE MachineDia = (SELECT Dia FROM Order_Info WHERE Id = " + orderID +")";
                SqlDataReader reader = CommonFunctions.GetFromDB(query);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        mcNo.Add(Convert.ToInt32(reader["MachineNo"]));
                    }
                }
                return mcNo;
            }
            catch (Exception ee)
            {
                MessageBox.Show("" + ee.ToString());
                return mcNo;
            }
            finally
            {
                if (CommonFunctions.connection.State == ConnectionState.Open)
                {
                    CommonFunctions.connection.Close();
                }
            }
        }

        private int GetLeftPlanQty(int mc, DateTime tempDate, int orderID)
        {
            int LeftPlanQty = -1; string query = "";
            string connectionStr = ConnectionManager.connectionString; SqlDataReader reader, reader1, reader2;
            SqlCommand cm = new SqlCommand(); SqlConnection cn = new SqlConnection(connectionStr); cm.Connection = cn; cn.Open(); 
            SqlCommand cm1 = new SqlCommand(); SqlConnection cn1 = new SqlConnection(connectionStr); cm1.Connection = cn1; cn1.Open();
            SqlCommand cm2 = new SqlCommand(); SqlConnection cn2 = new SqlConnection(connectionStr); cm2.Connection = cn2; cn2.Open();

            try
            {
                query = "SELECT Id FROM PlanTable WHERE MachineNo = " + mc + " AND TaskDate = '" + tempDate + "'";
                if (CommonFunctions.recordExist(query))
                {
                    query = "SELECT (SELECT TOP 1 Capacity FROM PlanTable WHERE MachineNo = " + mc + " AND TaskDate = '" + tempDate + "' order by Id desc) AS Capacity, SUM(PlanQty) AS TotalPlanQty FROM PlanTable WHERE MachineNo = " + mc + " AND TaskDate = '" + tempDate + "'";
                    cm.CommandText = query;
                    reader = cm.ExecuteReader();
                    while (reader.Read())
                    {
                        LeftPlanQty = Convert.ToInt32(reader["Capacity"]) - Convert.ToInt32(reader["TotalPlanQty"]);
                    }
                }
                else
                {
                    query = "SELECT Id FROM WorkingDays WHERE MachineNo = " + mc + " AND Active = 1 AND WorkDate = '" + tempDate + "'";
                    if (CommonFunctions.recordExist(query))
                    {
                        double SAM = 0.00; int Efficiency = 0; int Capacity = 0; int Minute = GetMinute(mc, tempDate); int orderQty = 0;
                        query = "SELECT Quantity, SAM, Efficiency FROM Order_Info WHERE Id = " + orderID;
                        cm1.CommandText = query;
                        reader1 = cm1.ExecuteReader();
                        while (reader1.Read())
                        {
                            orderQty = Convert.ToInt32(reader1["Quantity"]);
                            SAM = Convert.ToDouble(reader1["SAM"]);
                            Efficiency = Convert.ToInt32(reader1["Efficiency"]);
                        }
                        Capacity = Convert.ToInt32(Math.Floor((Double)((Minute * ((Double)Efficiency / 100.00)) / SAM)));
                        //query = "INSERT INTO PlanTable (MachineNo, TaskDate, OrderID, Capacity, PlanQty, RemainingQty, OrderQty, Efficiency, SAM, Minute, RevertVal, ActualQty, Status) " +
                        //        "VALUES (" + mc + ",'" + tempDate + "'," + orderID + "," + Capacity + "," + 0 + "," + Capacity + "," + 0 + "," + Efficiency + "," + SAM + "," + Minute + ", 0, 0, 0)";
                        //cm2.CommandText = query;
                        //cm2.ExecuteNonQuery();
                        LeftPlanQty = Capacity;
                    }
                    else
                    {
                        LeftPlanQty = 0;
                    }
                }
                return LeftPlanQty;
            }
            catch (Exception ee)
            {
                MessageBox.Show("" + ee.ToString());
                return LeftPlanQty;
            }
            finally
            {
                cn.Close();
                cn1.Close();
                //cn2.Close();
            }
        }

        private void UpdateRemainingQty(int mc, DateTime tempDate)
        {
            int ttlplanQty = 0;
            string connectionStr = ConnectionManager.connectionString;
            SqlCommand cm = new SqlCommand(); SqlConnection cn = new SqlConnection(connectionStr); cm.Connection = cn; cn.Open();

            try
            {
                string query = "SELECT Id, Capacity, PlanQty, RemainingQty FROM PlanTable WHERE MachineNo = " + mc + " AND TaskDate = '" + tempDate + "'";
                cm.CommandText = query;
                SqlDataReader reader = cm.ExecuteReader();
                while (reader.Read())
                {
                    int rowID = Convert.ToInt32(reader["Id"]);
                    int cap = Convert.ToInt32(reader["Capacity"]);
                    ttlplanQty = ttlplanQty + Convert.ToInt32(reader["PlanQty"]);
                    int RemainingQty = cap - ttlplanQty;
                    RemainingQty = RemainingQty < 0 ? 0 : RemainingQty;
                    SqlCommand cm1 = new SqlCommand(); SqlConnection cn1 = new SqlConnection(connectionStr); cm1.Connection = cn1; cn1.Open();
                    query = "UPDATE PlanTable SET RemainingQty = " + RemainingQty + " WHERE Id = " + rowID;
                    cm1.CommandText = query;
                    cm1.ExecuteNonQuery();
                    cn1.Close();
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("" + ee.ToString());
            }
            finally
            {
                cn.Close();
            }
        }

        private void AdjustActualQty(Boolean flag, int Val, int orderID)
        {
            try
            {
                DateTime TempDate = TaskDate; int mc = 0; string query = ""; SqlDataReader reader; value = Val; Boolean Result; Boolean Continue = true;
                string connectionStr = ConnectionManager.connectionString; SqlDataReader reader1;
                SqlCommand cm = new SqlCommand(); SqlConnection cn = new SqlConnection(connectionStr); cm.Connection = cn; 

                if (flag)
                {
                    mc = MachineNo;
                    while (!IsLastEntry(orderID, TempDate, mc))
                    {
                        TempDate = (DateTime)LastEntryDate(orderID); mc = GetLastMachineNo(orderID, TempDate);

                        query = " SELECT CASE WHEN PlanQty >= " + value + " THEN CAST( 1 as BIT ) ELSE CAST( 0 as BIT ) END AS A FROM PlanTable WHERE OrderID = " + orderID + " AND MachineNo =  " + mc + " AND TaskDate = '" + TempDate + "'";
                        if (CommonFunctions.IsTrue(query))
                        {
                            query = "UPDATE PlanTable SET PlanQty = PlanQty - " + value + ", RemainingQty = RemainingQty + " + value + " WHERE OrderID = " + orderID + " AND MachineNo =  " + mc + " AND TaskDate = '" + TempDate + "'";
                            Result = CommonFunctions.ExecutionToDB(query, 3);
                            UpdateRemainingQty(mc, TempDate);
                        }
                        else
                        {
                            cn.Open();
                            int tempo = 0;
                            cm.CommandText = "SELECT PlanQty FROM PlanTable WHERE OrderID = " + orderID + " AND MachineNo =  " + mc + " AND TaskDate = '" + TempDate + "'";
                            reader1 = cm.ExecuteReader();
                            while (reader1.Read())
                            {
                                tempo = Convert.ToInt32(reader1["PlanQty"]);
                            }
                            cn.Close();
                            query = "UPDATE PlanTable SET RemainingQty = RemainingQty + " + tempo + ", PlanQty = 0 WHERE OrderID = " + orderID + " AND MachineNo =  " + mc + " AND TaskDate = '" + TempDate + "'";
                            Result = CommonFunctions.ExecutionToDB(query, 3);
                            UpdateRemainingQty(mc, TempDate);
                            AdjustImmediateEntry(orderID, mc, TempDate, value - tempo);
                            value = tempo;
                        }

                        query = " SELECT CASE WHEN COUNT(*) > 1 THEN CAST( 1 as BIT ) ELSE CAST( 0 as BIT ) END AS A FROM PlanTable WHERE MachineNo = " + mc +
                                " AND TaskDate = '" + TempDate + "'";

                        if (CommonFunctions.IsTrue(query))
                        {
                            int rowID = 0;
                            query = "SELECT Top 1 * FROM PlanTable WHERE MachineNo = " + mc + " AND TaskDate = '" + TempDate + "' AND OrderID != " + orderID + " order by Id desc";
                            reader = CommonFunctions.GetFromDB(query);
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    orderID = Convert.ToInt32(reader["OrderID"]);
                                    rowID = Convert.ToInt32(reader["Id"]);
                                }
                            }
                            if (!IsLastEntry(orderID, TempDate, mc))
                            {
                                if (GetLeftPlanQty(mc, TempDate, orderID) >= value)
                                {
                                //}
                                //query = " SELECT CASE WHEN RemainingQty >= " + value + " THEN CAST( 1 as BIT ) ELSE CAST( 0 as BIT ) END AS A FROM PlanTable WHERE Id = " + rowID;
                                //if (CommonFunctions.IsTrue(query))
                                //{
                                    query = "UPDATE PlanTable SET PlanQty = PlanQty + " + value + ", RemainingQty = RemainingQty - " + value + " WHERE Id = " + rowID;
                                }
                                else
                                {
                                    int tempo = 0;
                                    cn.Open();
                                    cm.CommandText = "SELECT PlanQty FROM PlanTable WHERE OrderID = " + orderID + " AND MachineNo =  " + mc + " AND TaskDate = '" + TempDate + "'";
                                    reader1 = cm.ExecuteReader();
                                    while (reader1.Read())
                                    {
                                        tempo = Convert.ToInt32(reader1["PlanQty"]);
                                    }
                                    cn.Close();
                                    query = "UPDATE PlanTable SET PlanQty = PlanQty + RemainingQty, RemainingQty = 0 WHERE Id = " + rowID;
                                }
                                
                                Result = CommonFunctions.ExecutionToDB(query, 3);
                                UpdateRemainingQty(mc, TempDate);
                            }
                            else
                            {
                                mc = GetLastMachineNo(orderID, TempDate);
                            }
                        }
                        else
                        {
                            mc = GetLastMachineNo(orderID, TempDate);
                        }
                    }
                }
                else
                {
                    TempDate = (DateTime)LastEntryDate(orderID); mc = GetLastMachineNo(orderID, TempDate);  int LeftPlanQty = 0; int tempVal = value; var mcNo = GetMachineList(orderID); int i = 0;
                    TempDate = TaskDate; mc = MachineNo;

                    if (!IsLastEntry(orderID, TempDate, mc)) // if last entry then will not enter
                    {
                        DateTime KnitCloseDate = GetKnitCloseDate(orderID);
                        int j = 0;
                        while (j < mcNo.Count && value > 0)
                        {
                            mc = Convert.ToInt32(mcNo[j]); TempDate = TaskDate.AddDays(1);
                            while (TempDate <= KnitCloseDate && value > 0)
                            {
                                query = "SELECT Id FROM PlanTable WHERE MachineNo = "+mc+" AND TaskDate = '"+TempDate+"' AND OrderID ="+orderID;
                                if (CommonFunctions.recordExist(query))
                                {
                                    LeftPlanQty = GetLeftPlanQty(mc, TempDate, orderID);

                                    if (LeftPlanQty != 0)
                                    {
                                        tempVal = LeftPlanQty > value ? value : LeftPlanQty;
                                        value = value - tempVal;

                                        if (LeftPlanQty > tempVal)
                                        {
                                            query = "UPDATE PlanTable SET PlanQty = PlanQty + " + tempVal + ", RemainingQty = RemainingQty - " + tempVal + " WHERE MachineNo = " + mc + " AND OrderID = " + orderID + " AND TaskDate = '" + TempDate + "'";
                                        }
                                        else
                                        {
                                            query = "UPDATE PlanTable SET PlanQty = PlanQty + " + tempVal + ", RemainingQty = 0 WHERE MachineNo = " + mc + " AND OrderID = " + orderID + " AND TaskDate = '" + TempDate + "'";
                                        }

                                        Boolean result = CommonFunctions.ExecutionToDB(query, 3);
                                        UpdateRemainingQty(mc, TempDate);
                                    }
                                }
                                TempDate = TempDate.AddDays(1);
                            }
                            j++;
                        }
                    }

                    if (value > 0)
                    {
                        //TempDate = (DateTime)LastEntryDate(orderID); mc = GetLastMachineNo(orderID, TempDate);
                        while (value > 0)
                        {
                            TempDate = TaskDate.AddDays(1); int tempMC = MachineNo;
                            LeftPlanQty = GetLeftPlanQty(tempMC, TaskDate.AddDays(1), orderID);

                            while (LeftPlanQty <= 0)
                            {
                                i = 0;
                                while (i < mcNo.Count)
                                {
                                    tempMC = Convert.ToInt32(mcNo[i]);
                                    LeftPlanQty = GetLeftPlanQty(tempMC, TempDate, orderID);
                                    i = LeftPlanQty > 0 ? int.MaxValue : i + 1;
                                }
                                if (LeftPlanQty <= 0)
                                {
                                    TempDate = TempDate.AddDays(1); 
                                }
                            }

                            tempVal = LeftPlanQty > value ? value : LeftPlanQty;
                            AddNewPlan(value, tempVal, tempMC, TempDate, orderID);
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("" + ee.ToString());
            }
            finally
            {
                if (CommonFunctions.connection.State == ConnectionState.Open)
                {
                    CommonFunctions.connection.Close();
                }
            }
        }

        private DateTime GetKnitCloseDate(int orderID)
        {
            string connectionStr = ConnectionManager.connectionString;
            SqlCommand cm = new SqlCommand();
            SqlConnection cn = new SqlConnection(connectionStr);
            cm.Connection = cn;
            cn.Open();
            string query = ""; SqlDataReader reader; DateTime KnitCloseDate = DateTime.MinValue;

            try
            {
                query = "SELECT ShipmentDate FROM Order_Info WHERE Id = " + orderID;
                cm.CommandText = query;
                reader = cm.ExecuteReader();
                while (reader.Read())
                {
                    KnitCloseDate = Convert.ToDateTime(reader["ShipmentDate"]).Date;
                }
                return KnitCloseDate;
            }
            catch (Exception ee)
            {
                MessageBox.Show("" + ee.ToString());
                return KnitCloseDate;
            }
            finally
            {
                cn.Close();
            }
        }

        private void AdjustImmediateEntry(int orderID, int mc, DateTime TempDate, int val)
        {
            try
            {
                string connectionStr = ConnectionManager.connectionString; SqlDataReader reader, reader1;
                SqlCommand cm = new SqlCommand(); SqlConnection cn = new SqlConnection(connectionStr); cm.Connection = cn; cn.Open();
                SqlCommand cm1 = new SqlCommand(); SqlConnection cn1 = new SqlConnection(connectionStr); cm1.Connection = cn1; cn1.Open();
                SqlCommand cm2 = new SqlCommand(); SqlConnection cn2 = new SqlConnection(connectionStr); cm2.Connection = cn2;
                string date = null; int id1 = 0; int id2 = 0; int MachineNumber = 0; int ActualQty = 0;

                string query = " SELECT * FROM PlanTable WHERE TaskDate = (SELECT MAX(TaskDate) FROM PlanTable WHERE OrderID = " + orderID + " AND TaskDate < (SELECT MAX(TaskDate) FROM PlanTable WHERE OrderID = " + orderID + ")) AND "
                              + " MachineNo = (SELECT MAX(MachineNo) FROM PlanTable WHERE (SELECT MAX(TaskDate) FROM PlanTable WHERE OrderID = " + orderID + " AND TaskDate < (SELECT MAX(TaskDate) FROM PlanTable WHERE OrderID = " + orderID + ")))";
                cm.CommandText = query;
                reader = cm.ExecuteReader();
                while (reader.Read())
                {
                    id1 = Convert.ToInt32(reader["Id"]);
                    MachineNumber = Convert.ToInt32(reader["MachineNo"]);
                    date = reader["TaskDate"].ToString();
                    ActualQty = Convert.ToInt32(reader["ActualQty"]);
                }
                cn.Close();

                query = " SELECT Id FROM PlanTable WHERE MachineNo = " + mc + " AND TaskDate = '" + TempDate + "' AND OrderID = " + orderID;
                cm1.CommandText = query;
                reader1 = cm1.ExecuteReader();
                while (reader1.Read())
                {
                    id2 = Convert.ToInt32(reader1["Id"]);
                }
                cn1.Close();

                if (id1 != id2 && ActualQty == 0)
                {
                    cn2.Open();
                    query = "UPDATE PlanTable SET RemainingQty = RemainingQty + " + val + ", PlanQty = PlanQty - " + val + " WHERE OrderID = " + orderID + " AND MachineNo =  " + MachineNumber + " AND TaskDate = '" + date + "'";
                    cm2.CommandText = query;
                    cm2.ExecuteNonQuery();
                    cn2.Close();
                    UpdateRemainingQty(MachineNumber, Convert.ToDateTime(date));
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show("" + ee.ToString());
            }
        }

        private void AddNewPlan(int Val, int tempVal, int mc, DateTime TempDate, int orderID)
        {
            string connectionStr = ConnectionManager.connectionString; SqlDataReader reader, reader1; value = Val; string query = "";
            SqlCommand cm = new SqlCommand(); SqlConnection cn = new SqlConnection(connectionStr); cm.Connection = cn; cn.Open();
            SqlCommand cm1 = new SqlCommand(); SqlConnection cn1 = new SqlConnection(connectionStr); cm1.Connection = cn1; cn1.Open();
            SqlCommand cm2 = new SqlCommand(); SqlConnection cn2 = new SqlConnection(connectionStr); cm2.Connection = cn2;cn2.Open();

            try
            {
                double SAM = 0.00; int Efficiency = 0; int Capacity = 0; int Minute = GetMinute(mc, TempDate); int orderQty = 0; int RestTotalPlanQty = 0;
                query = "SELECT Quantity, SAM, Efficiency FROM Order_Info WHERE Id = " + orderID;
                cm.CommandText = query;
                reader = cm.ExecuteReader();
                while (reader.Read())
                {
                    orderQty = Convert.ToInt32(reader["Quantity"]);
                    SAM = Convert.ToDouble(reader["SAM"]);
                    Efficiency = Convert.ToInt32(reader["Efficiency"]);
                }

                if (CommonFunctions.recordExist("SELECT Id FROM PlanTable WHERE MachineNo = " + mc + " AND TaskDate = '" + TempDate + "'"))
                {
                    Capacity = GetCapacity(orderID, TempDate, mc, true);
                    query = "SELECT SUM(PlanQty) AS RestTotalPlanQty FROM PlanTable WHERE MachineNo = " + mc + " AND TaskDate = '" + TempDate + "'";
                    cm1.CommandText = query;
                    reader1 = cm1.ExecuteReader();
                    while (reader1.Read())
                    {
                        RestTotalPlanQty = Convert.ToInt32(reader1["RestTotalPlanQty"]);
                    }
                }
                else
                {
                    Capacity = GetCapacity(orderID, TempDate, mc, false);
                }

                int PreTemoVal = tempVal;
                tempVal = Capacity >= (RestTotalPlanQty + tempVal) ? tempVal : (Capacity - RestTotalPlanQty);
                //value = value + (PreTemoVal > tempVal ? PreTemoVal - tempVal : tempVal - PreTemoVal);
                value = value - tempVal;

                query = "INSERT INTO PlanTable (MachineNo, TaskDate, OrderID, Capacity, PlanQty, RemainingQty, OrderQty, Efficiency, SAM, Minute, RevertVal, ActualQty, Status, Production) " +
                        "VALUES (" + mc + ",'" + TempDate + "'," + orderID + "," + Capacity + "," + tempVal + "," + (Capacity - tempVal) + "," + orderQty + "," + Efficiency + "," + SAM + "," + Minute + ", 0, 0, 0, 1)";
                cm2.CommandText = query;
                cm2.ExecuteNonQuery();
            }
            catch (Exception ee)
            {
                MessageBox.Show("" + ee.ToString());
            }
            finally
            {
                cn.Close();
                cn1.Close();
                cn2.Close();
            }
        }

        private int GetCapacity(int orderID, DateTime TempDate, int mc, Boolean RecordExist)
        {
            string connectionStr = ConnectionManager.connectionString;
            SqlCommand cm = new SqlCommand();
            SqlConnection cn = new SqlConnection(connectionStr);
            cm.Connection = cn;
            cn.Open();
            
            SqlCommand cm1 = new SqlCommand();
            SqlConnection cn1 = new SqlConnection(connectionStr);
            cm1.Connection = cn1;
            cn1.Open();
            
            string query = ""; int Capacity = 0; SqlDataReader reader, reader1;
            try
            {
                Double SAM = 0.00; Double Efficiency = 0.00; int Minute = GetMinute(mc, TempDate); int orderQty = 0;
                query = "SELECT Quantity, SAM, Efficiency FROM Order_Info WHERE Id = " + orderID;
                cm.CommandText = query;
                reader = cm.ExecuteReader();
                while (reader.Read())
                {
                    orderQty = Convert.ToInt32(reader["Quantity"]);
                    SAM = Convert.ToDouble(reader["SAM"]);
                    Efficiency = Convert.ToInt32(reader["Efficiency"]);
                }

                if (RecordExist)
                {
                    query = "SELECT SUM(PlanQty) AS RestPlanQty, (SELECT SUM(SAM) FROM PlanTable WHERE MachineNo = " + MachineNo +
                    " AND TaskDate = '" + TempDate.Date + "' AND OrderID != " + orderID + ") As TotalSAM, Count(*) AS RestEfficiencyNumber, SUM(Efficiency) AS TotalRestEfficiency, SUM(Minute) AS TotalRestMinute, (SELECT TOP 1 Minute FROM WorkingDays WHERE MachineNo = "
                    + MachineNo + " AND WorkDate = '" + TempDate.Date + "') AS Minute, (SELECT TOP 1 Active FROM WorkingDays WHERE MachineNo = "
                    + MachineNo + " AND WorkDate = '" + TempDate.Date + "') AS Active FROM PlanTable WHERE MachineNo = "
                    + MachineNo + " AND TaskDate = '" + TempDate.Date + "'";
                    cm1.CommandText = query;
                    reader1 = cm1.ExecuteReader();
                    while (reader1.Read())
                    {
                        int TotalRestPlanQty = reader1.IsDBNull(reader1.GetOrdinal("RestPlanQty")) == true ? 0 : (int)reader1["RestPlanQty"];
                        Double TotalRestEfficiency = reader1.IsDBNull(reader1.GetOrdinal("TotalRestEfficiency")) == true ? 0 : Convert.ToDouble(reader1["TotalRestEfficiency"]);
                        Double RestEfficiencyNumber = reader1.IsDBNull(reader1.GetOrdinal("RestEfficiencyNumber")) == true ? 0 : Convert.ToDouble(reader1["RestEfficiencyNumber"]);
                        Double TotalSam = reader1.IsDBNull(reader1.GetOrdinal("TotalSAM")) == true ? 0 + SAM : (Convert.ToDouble(reader1["TotalSAM"]) + SAM);
                        Double AVGSAM = TotalSam / (RestEfficiencyNumber + 1);
                        Double AvgEff = (Convert.ToDouble((TotalRestEfficiency + Efficiency) / (RestEfficiencyNumber + 1)));

                        //int TotalMinute = reader.IsDBNull(reader.GetOrdinal("TotalRestMinute")) == true ? 0 + Convert.ToInt32(reader["Minute"]) : Convert.ToInt32(reader["TotalRestMinute"]) + Convert.ToInt32(reader["Minute"]);
                        //int AVGMinute = Convert.ToInt32(Math.Floor(TotalMinute / (RestEfficiencyNumber + 1)));
                        Capacity = Convert.ToInt32(reader1["Active"]) == 0 ? 0 : (int)Math.Floor((Convert.ToDouble(Minute) * (Convert.ToDouble(AvgEff) / 100.00)) / (AVGSAM));
                    }
                }
                else
                {
                    Capacity = Convert.ToInt32(Math.Floor((Double)((Minute * ((Double)Efficiency / 100.00)) / SAM)));
                }
                return Capacity;
            }
            catch (Exception ee)
            {
                MessageBox.Show("" + ee.ToString());
                return 0;
            }
            finally
            {
                cn.Close();
                cn1.Close();
            }
        }

        private int GetMinute(int mc, DateTime TempDate)
        {
            string connectionStr = ConnectionManager.connectionString;
            SqlCommand cm = new SqlCommand();
            SqlConnection cn = new SqlConnection(connectionStr);
            cm.Connection = cn;
            cn.Open();
            string query = ""; SqlDataReader reader; int Minute = 0;

            try
            {
                query = "SELECT Minute FROM WorkingDays WHERE MachineNo = " + mc + " AND WorkDate = '" + TempDate + "'";
                cm.CommandText = query;
                reader = cm.ExecuteReader();
                while (reader.Read())
                {
                    Minute = Convert.ToInt32(reader["Minute"]);
                }
                return Minute;
            }
            catch (Exception ee)
            {
                MessageBox.Show("" + ee.ToString());
                return 0;
            }
            finally
            {
                cn.Close();
            }
        }

        private Boolean IsLastEntry(int orderID, DateTime Date, int mc)
        {
            int rowId1 = 0; int rowId2 = 0;
            try
            {
                string query = "SELECT Id FROM PlanTable WHERE MachineNo = " + mc + " AND OrderID = " + orderID + " AND TaskDate = '" + Date + "'";
                SqlDataReader reader = CommonFunctions.GetFromDB(query);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        rowId1 = Convert.ToInt32(reader["Id"]);
                    }
                }
                CommonFunctions.connection.Close();
                query = " SELECT Id FROM PlanTable WHERE TaskDate = (SELECT Max(TaskDate) AS MaxTaskDate FROM PlanTable WHERE OrderID = " + orderID + ") AND MachineNo = (SELECT Max(MachineNo) AS MaxMachineNo FROM PlanTable WHERE OrderID = " + orderID + " AND "
                       + " TaskDate = (SELECT Max(TaskDate) AS MaxTaskDate FROM PlanTable WHERE OrderID = " + orderID + "))";
                reader = CommonFunctions.GetFromDB(query);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        rowId2 = Convert.ToInt32(reader["Id"]);
                    }
                }
                CommonFunctions.connection.Close();

                return rowId1 == rowId2 ? true : false;
            }
            catch (Exception ee)
            {
                MessageBox.Show("" + ee.ToString());
                return false;
            }
        }

        private int GetLastMachineNo(int orderID, DateTime TempDate)
        {
            int mc = 0;
            string query = "SELECT MAX(MachineNo) AS MachineNo FROM PlanTable WHERE OrderID = " + orderID + " AND TaskDate = '" + TempDate + "'";
            SqlDataReader reader = CommonFunctions.GetFromDB(query);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    mc = Convert.ToInt32(reader["MachineNo"]);
                }
            }
            CommonFunctions.connection.Close();
            return mc;
        }

        private DateTime? LastEntryDate(int orderID)
        {
            try
            {
                string date = null; int MachineNo = 0;
                string query = "SELECT Max(TaskDate) AS TaskDate FROM PlanTable WHERE OrderID = " + orderID;
                SqlDataReader reader = CommonFunctions.GetFromDB(query);
                while (reader.Read())
                {
                    date = reader["TaskDate"].ToString();
                }
                return Convert.ToDateTime(date).Date;
            }
            catch (Exception ee)
            {
                MessageBox.Show("" + ee.ToString());
                return null;
            }
            finally
            {
                if (CommonFunctions.connection.State == ConnectionState.Open)
                {
                    CommonFunctions.connection.Close();
                }
            }
        }

        private void ActualQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void actualQtyDataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (actualQtyDataGridView.CurrentCell.ColumnIndex == 7) //Desired Column
            {
                e.Control.KeyPress -= new KeyPressEventHandler(ActualQty_KeyPress);
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(ActualQty_KeyPress);
                }
            }
        }

        private void actualQtyDataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex == 7 && PreVal < 0)
            {
                if (actualQtyDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "")
                {
                    PreVal = Convert.ToInt32(actualQtyDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                }
            }
            else
            {
                return;
            }
        }

        private void actualQtyDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (actualQtyDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null || actualQtyDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == "")
            {
                MessageBox.Show("Null Value can not used!!!");
                actualQtyDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = PreVal;
                PreVal = -1;
                return;
            }

            if (Convert.ToInt32(actualQtyDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) == PreVal)
            {
                actualQtyDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = PreVal;
            }

            //if (!NoProduction)
            //{
            //    if (Convert.ToInt32(actualQtyDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) == 0 && PreVal != 0)
            //    {
            //        MessageBox.Show("Zero value means no production!!!");
            //        actualQtyDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = PreVal;
            //        PreVal = -1;
            //        return;
            //    }
            //}
            PreVal = -1;
        }
    }
}
