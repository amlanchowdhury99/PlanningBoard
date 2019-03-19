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
using System.Collections;

namespace PlanningBoard
{
    public partial class Report : Form
    {
        ArrayList MachineList = new ArrayList();
        
        public Dictionary<int, string> BuyerList = new Dictionary<int, string>();
        public Dictionary<int, string> StyleList = new Dictionary<int, string>();
        public Dictionary<int, string> DiaList = new Dictionary<int, string>();
        public Dictionary<int, string> SizeList = new Dictionary<int, string>();
        public Dictionary<int, string> PartList = new Dictionary<int, string>();

        public bool isShipChecked = false;
        public bool isProChecked = false;
        public string mcNo = "";
        public string mcStatus = "";
        public string buyer = "";
        public string style = "";
        public string size = "";
        public string dia = "";
        public string part = "";
        public string shipFromDate = "";
        public string shipToDate = "";
        public string proFromDate = "";
        public string proToDate = "";

        public Boolean FirstTime = true;

        public Report()
        {
            InitializeComponent();
            mcNo = "";
            mcStatus = "";
            buyer = "";
            style = "";
            size = "";
            dia = "";
            part = "";
            shipFromDate = "";
            shipToDate = "";
            proFromDate = "";
            proToDate = "";
            FirstTime = true;
            LoadComboBox();
            LoadDatePicker();
        }

        private void GenerateReport(bool isDetailed)
        {
            try
            {
                int orderID = -1;
                int count = 0;
                if (MachineList.Count < 2)
                {
                    planBoardDataGridView.Rows.Clear();
                    return;
                }

                planBoardDataGridView.Rows.Clear();
                planBoardDataGridView.Columns.Clear();
                planBoardDataGridView.Columns.Add("SL", "SL");
                planBoardDataGridView.Columns.Add("MachineNo", "MachineNo");
                planBoardDataGridView.Columns.Add("MachineStatus", "MachineStatus");
                planBoardDataGridView.Columns.Add("Buyer", "Buyer");
                planBoardDataGridView.Columns.Add("Style", "Style");
                planBoardDataGridView.Columns.Add("Size", "Size");
                planBoardDataGridView.Columns.Add("Dia", "Dia");
                planBoardDataGridView.Columns.Add("Part", "Part");
                planBoardDataGridView.Columns.Add("ShipmentDate", "ShipmentDate");
                if (isDetailed)
                {
                    planBoardDataGridView.Columns.Add("ProductionDate", "ProductionDate");
                }   
                planBoardDataGridView.Columns.Add("OrderQty", "OrderQty");
                planBoardDataGridView.Columns.Add("OrderStatus", "OrderStatus");
                if (isDetailed)
                {
                    planBoardDataGridView.Columns.Add("SAM", "SAM");
                    planBoardDataGridView.Columns.Add("Efficiency", "Efficiency");
                }
                if (isDetailed)
                {
                    planBoardDataGridView.Columns.Add("PlanQty", "PlanQty");
                    planBoardDataGridView.Columns.Add("ActualQty", "ActualQty");
                }
                else
                {
                    planBoardDataGridView.Columns.Add("TotalPlanQty", "TotalPlanQty");
                    planBoardDataGridView.Columns.Add("TotalActualQty", "TotalActualQty");
                }
                
                planBoardDataGridView.Columns.Add("OrderQtyLeft", "OrderQtyLeft");
                planBoardDataGridView.Columns.Add("PlanQtyLeft", "PlanQtyLeft");

                foreach (DataGridViewColumn col in planBoardDataGridView.Columns)
                {
                    col.ReadOnly = true;
                }

                string query = QueryForSearch();

                SqlDataReader reader = CommonFunctions.GetFromDB(query);
                int SL = 1;
                string S1 = ""; string S2 = ""; string S3 = ""; string S4 = ""; string S5 = ""; string S6 = ""; string S7 = ""; string S8 = ""; string S9 = ""; string S10 = ""; string S11 = "";
                string S12 = ""; string S13 = ""; string S14 = ""; string S15 = ""; string S16 = ""; string S17 = ""; string S18 = "";
                if (reader.HasRows)
                {
                    if (isDetailed)
                    {
                        while (reader.Read())
                        {
                            S1 = SL.ToString();
                            S2 = reader["MachineNo"].ToString();
                            S3 = MStatuscomboBox.Text;
                            S4 = reader["BuyerName"].ToString();
                            S5 = reader["StyleName"].ToString();
                            S6 = reader["SizeName"].ToString();
                            S7 = reader["Dia"].ToString();
                            S8 = reader["PartName"].ToString();
                            S9 = Convert.ToDateTime(reader["ShipmentDate"]).ToString("dd/MM/yyyy");
                            S10 = Convert.ToDateTime(reader["TaskDate"]).ToString("dd/MM/yyyy");
                            S11 = reader["OrderQty"].ToString();
                            S12 = orderStatusComboBox.Text;
                            S13 = reader["SAM"].ToString();
                            S14 = reader["Efficiency"].ToString();
                            S15 = reader["PlanQty"].ToString();
                            S16 = reader.IsDBNull(reader.GetOrdinal("ActualQty")) == true ? "0" : reader["ActualQty"].ToString();
                            S17 = (Convert.ToInt32(reader["OrderQty"]) - Convert.ToInt32(reader["PlanQty"])).ToString();
                            S18 = (Convert.ToInt32(reader["OrderQty"]) - Convert.ToInt32(S16)).ToString();

                            planBoardDataGridView.Rows.Add(S1, S2, S3, S4, S5, S6, S7, S8, S9, S10, S11, S12, S13, S14, S15, S16, S17, S18);

                            SL++;
                        }
                    }
                    else
                    {
                        int orderQty = 0;
                        int totalPlanQty = 0;
                        int totalActualQty = 0;
                        while (reader.Read())
                        {
                            if (orderID != Convert.ToInt32(reader["OrderID"]))
                            {
                                if (count != 0)
                                {
                                    S12 = totalPlanQty.ToString();
                                    S13 = totalActualQty.ToString();
                                    S14 = (Convert.ToInt32(reader["OrderQty"]) - totalPlanQty).ToString();
                                    S15 = (Convert.ToInt32(reader["OrderQty"]) - totalActualQty).ToString();
                                    planBoardDataGridView.Rows.Add(S1, S2, S3, S4, S5, S6, S7, S8, S9, S10, S11, S12, S13, S14, S15);
                                    totalPlanQty = 0;
                                    totalActualQty = 0;
                                    SL++;
                                }
                                orderID = Convert.ToInt32(reader["OrderID"]);
                                count = 1;
                            }
                            else
                            {
                                count++;
                            }

                            S1 = SL.ToString();
                            S2 = reader["MachineNo"].ToString();
                            S3 = MStatuscomboBox.Text;
                            S4 = reader["BuyerName"].ToString();
                            S5 = reader["StyleName"].ToString();
                            S6 = reader["SizeName"].ToString();
                            S7 = reader["Dia"].ToString();
                            S8 = reader["PartName"].ToString();
                            S9 = Convert.ToDateTime(reader["ShipmentDate"]).ToString("dd/MM/yyyy");
                            S10 = reader["OrderQty"].ToString();
                            orderQty = Convert.ToInt32(S10);
                            S11 = orderStatusComboBox.Text;

                            totalPlanQty = totalPlanQty + Convert.ToInt32(reader["PlanQty"]);
                            totalActualQty = totalActualQty + (reader.IsDBNull(reader.GetOrdinal("ActualQty")) == true ? 0 : Convert.ToInt32(reader["ActualQty"]));
                        }

                        S12 = totalPlanQty.ToString();
                        S13 = totalActualQty.ToString();
                        S14 = (orderQty - totalPlanQty).ToString();
                        S15 = (orderQty - totalActualQty).ToString();
                        planBoardDataGridView.Rows.Add(S1, S2, S3, S4, S5, S6, S7, S8, S9, S10, S11, S12, S13, S14, S15);
                        totalPlanQty = 0;
                        totalActualQty = 0;
                        SL++;
                    }
                }
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

        private void GenerateDetailsBtn_Click(object sender, EventArgs e)
        {
            GenerateReport(true);
        }

        private void GenerateSummaryBtn_Click(object sender, EventArgs e)
        {
            GenerateReport(false);
        }

        private void ExportBtn_Click(object sender, EventArgs e)
        {
            try
            {
                if (planBoardDataGridView.Rows.Count < 1)
                {
                    MessageBox.Show("No rows exist to export excel!!!");
                    return;
                }

                if (planBoardDataGridView.Rows.Count > 0)
                {

                    Microsoft.Office.Interop.Excel._Application XcelApp = new Microsoft.Office.Interop.Excel.Application();
                    XcelApp.Application.Workbooks.Add(Type.Missing);
                    for (int i = 0; i < planBoardDataGridView.Columns.Count; i++)
                    {
                        XcelApp.Cells[1, (i + 1)] = planBoardDataGridView.Columns[i].HeaderText;
                    }
                    for (int i = 0; i < planBoardDataGridView.Rows.Count; i++)
                    {
                        for (int j = 0; j < planBoardDataGridView.Columns.Count; j++)
                        {
                            XcelApp.Cells[(i + 2), ((j + 1))] = " " + planBoardDataGridView.Rows[i].Cells[j].Value.ToString();
                        }
                    }
                    XcelApp.Columns.AutoFit();
                    XcelApp.Rows.AutoFit();
                    XcelApp.Visible = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ExportToExcel: \n" + ex.Message);
            }
        }

        private void LoadDatePicker()
        {
            shipFromDateTimePicker.Value = DateTime.Now.Date;
            shipToDateTimePicker.Value = DateTime.Now.Date;
            proFromDateTimePicker.Value = DateTime.Now.Date;
            proToDateTimePicker.Value = DateTime.Now.Date;
        }

        private void LoadComboBox()
        {
            LoadOrderStatus();
            LoadMachineStatus();
            LoadMachine();
            LoadBuyer();
            LoadStyle();
            LoadDia();
            LoadSize();
            LoadPart();
            FirstTime = false;
            
        }

        private void LoadMachineStatus()
        {
            MStatuscomboBox.DisplayMember = "Description";
            MStatuscomboBox.ValueMember = "Value";
            MStatuscomboBox.DataSource = Enum.GetValues(typeof(VariableDecleration_Class.Status)).Cast<VariableDecleration_Class.Status>().Where(e => e != VariableDecleration_Class.Status.Pending && e != VariableDecleration_Class.Status.Complete).Cast<Enum>().Select(value => new { (Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()), typeof(DescriptionAttribute)) as DescriptionAttribute).Description, value }).OrderBy(item => item.value).ToList();
            MStatuscomboBox.SelectedIndex = 1;
        }

        private void LoadMachine()
        {
            try
            {
                string query = "SELECT * FROM Machine_Info WHERE Status = " + MStatuscomboBox.SelectedIndex + " order by MachineNo asc";

                SqlDataReader reader = CommonFunctions.GetFromDB(query);
                MachineComboBox.DataSource = null;
                MachineComboBox.Items.Clear();
                MachineList.Clear();

                if (reader.HasRows)
                {
                    MachineList.Add("ALL");
                    while (reader.Read())
                    {
                        MachineList.Add(Convert.ToInt32(reader["MachineNo"]));
                    }
                }
                if (MachineList.Count > 0)
                {
                    MachineComboBox.DataSource = MachineList;
                    MachineComboBox.SelectedIndex = 0;
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

        private void LoadOrderStatus()
        {
            orderStatusComboBox.DisplayMember = "Description";
            orderStatusComboBox.ValueMember = "Value";
            orderStatusComboBox.DataSource = Enum.GetValues(typeof(VariableDecleration_Class.Status)).Cast<VariableDecleration_Class.Status>().Where(e => e != VariableDecleration_Class.Status.InActive && e != VariableDecleration_Class.Status.Active).Cast<Enum>().Select(value => new { (Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()), typeof(DescriptionAttribute)) as DescriptionAttribute).Description, value }).OrderBy(item => item.value).ToList();
            orderStatusComboBox.SelectedIndex = 1;
        }

        private void LoadSize()
        {
            try
            {
                SizeList.Clear();
                SizeList.Add(0, "Select Size");
                string query = "SELECT * FROM Size order by SizeName asc";

                SqlDataReader reader = CommonFunctions.GetFromDB(query);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        SizeList.Add(Convert.ToInt32(reader["Id"]), (reader["SizeName"]).ToString());
                    }
                    sizeComboBox.DataSource = new BindingSource(SizeList, null);
                    sizeComboBox.DisplayMember = "Value";
                    sizeComboBox.ValueMember = "Key";
                    sizeComboBox.SelectedIndex = 0;
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

        private void LoadPart()
        {
            try
            {
                PartList.Clear();
                PartList.Add(0, "Select Part");
                string query = "SELECT * FROM BodyPart order by PartName asc";

                SqlDataReader reader = CommonFunctions.GetFromDB(query);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        PartList.Add(Convert.ToInt32(reader["Id"]), (reader["PartName"]).ToString());
                    }
                    partComboBox.DataSource = new BindingSource(PartList, null);
                    partComboBox.DisplayMember = "Value";
                    partComboBox.ValueMember = "Key";
                    partComboBox.SelectedIndex = 0;
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

        private void LoadBuyer()
        {
            try
            {
                BuyerList.Clear();
                BuyerList.Add(0, "Select Buyer");

                string query = "SELECT * FROM Buyer order by BuyerName asc";

                SqlDataReader reader = CommonFunctions.GetFromDB(query);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        BuyerList.Add(Convert.ToInt32(reader["Id"]), (reader["BuyerName"]).ToString());
                    }
                    buyerComboBox.DataSource = new BindingSource(BuyerList, null);
                    buyerComboBox.DisplayMember = "Value";
                    buyerComboBox.ValueMember = "Key";
                    buyerComboBox.SelectedIndex = 0;
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

        private void LoadStyle()
        {
            try
            {
                StyleList.Clear();
                StyleList.Add(0, "Select Style");
                string query = "SELECT * FROM Style order by StyleName asc";

                SqlDataReader reader = CommonFunctions.GetFromDB(query);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        StyleList.Add(Convert.ToInt32(reader["Id"]), (reader["StyleName"]).ToString());
                    }
                    styleComboBox.DataSource = new BindingSource(StyleList, null);
                    styleComboBox.DisplayMember = "Value";
                    styleComboBox.ValueMember = "Key";
                    styleComboBox.SelectedIndex = 0;
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

        private void LoadDia()
        {
            try
            {
                DiaList.Clear();
                DiaList.Add(0, "Select Dia");
                string query = "SELECT * FROM Dia order by Dia asc";

                SqlDataReader reader = CommonFunctions.GetFromDB(query);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        DiaList.Add(Convert.ToInt32(reader["Id"]), (reader["Dia"]).ToString());
                    }
                    diaComboBox.DataSource = new BindingSource(DiaList, null);
                    diaComboBox.DisplayMember = "Value";
                    diaComboBox.ValueMember = "Key";
                    diaComboBox.SelectedIndex = 0;
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

        private void QueryForCombo()
        {
            FirstTime = true;
            string query = "SELECT BuyerID, BuyerName, StyleID, StyleName, SizeID, SizeName, DiaID, Dia, PartID, PartName FROM OrderDetailsView";
            if (buyer != "")
            {
                query = query.Contains("WHERE") == true ? query = query + " AND " : query + " WHERE ";

                query = query + " BuyerID = " + buyer;
            }
            if (style != "")
            {
                query = query.Contains("WHERE") == true ? query = query + " AND " : query + " WHERE ";

                query = query + " StyleID = " + style;
            }
            if (size != "")
            {
                query = query.Contains("WHERE") == true ? query = query + " AND " : query + " WHERE ";

                query = query + " SizeID = " + size;
            }
            if (dia != "")
            {
                query = query.Contains("WHERE") == true ? query = query + " AND " : query + " WHERE ";

                query = query + " DiaID = " + dia;
            }
            if (part != "")
            {
                query = query.Contains("WHERE") == true ? query = query + " AND " : query + " WHERE ";

                query = query + " PartID = " + part;
            }

            try
            {
                if (buyer == "")
                {
                    BuyerList = (Dictionary<int, string>)BuyerList.Where(p => p.Key == 0).ToDictionary(p => p.Key, p => p.Value);
                }
                if (style == "")
                {
                    StyleList = (Dictionary<int, string>)StyleList.Where(p => p.Key == 0).ToDictionary(p => p.Key, p => p.Value);
                }
                if (style == "")
                {
                    SizeList = (Dictionary<int, string>)SizeList.Where(p => p.Key == 0).ToDictionary(p => p.Key, p => p.Value);
                }
                if (style == "")
                {
                    DiaList = (Dictionary<int, string>)DiaList.Where(p => p.Key == 0).ToDictionary(p => p.Key, p => p.Value);
                }
                if (style == "")
                {
                    PartList = (Dictionary<int, string>)PartList.Where(p => p.Key == 0).ToDictionary(p => p.Key, p => p.Value);
                }
                SqlDataReader reader = CommonFunctions.GetFromDB(query);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (!BuyerList.Keys.Contains(Convert.ToInt32(reader["BuyerID"])))
                        {
                            BuyerList.Add(Convert.ToInt32(reader["BuyerID"]), (reader["BuyerName"]).ToString());
                        }
                        if (!StyleList.Keys.Contains(Convert.ToInt32(reader["StyleID"])))
                        {
                            StyleList.Add(Convert.ToInt32(reader["StyleID"]), (reader["StyleName"]).ToString());
                        }
                        if (!SizeList.Keys.Contains(Convert.ToInt32(reader["SizeID"])))
                        {
                            SizeList.Add(Convert.ToInt32(reader["SizeID"]), (reader["SizeName"]).ToString());
                        }
                        if (!DiaList.Keys.Contains(Convert.ToInt32(reader["DiaID"])))
                        {
                            DiaList.Add(Convert.ToInt32(reader["DiaID"]), (reader["Dia"]).ToString());
                        }
                        if (!PartList.Keys.Contains(Convert.ToInt32(reader["PartID"])))
                        {
                            PartList.Add(Convert.ToInt32(reader["PartID"]), (reader["PartName"]).ToString());
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
                if (buyer == "")
                {
                    buyerComboBox.DataSource = null;
                    buyerComboBox.DataBindings.Clear();
                    buyerComboBox.DataSource = BuyerList.ToList();
                    buyerComboBox.DisplayMember = "Value";
                    buyerComboBox.ValueMember = "Key";
                }

                if (style == "")
                {
                    styleComboBox.DataSource = null;
                    styleComboBox.DataBindings.Clear();
                    styleComboBox.DataSource = StyleList.ToList();
                    styleComboBox.DisplayMember = "Value";
                    styleComboBox.ValueMember = "Key";
                }

                if (size == "")
                {
                    sizeComboBox.DataSource = null;
                    sizeComboBox.DataBindings.Clear();
                    sizeComboBox.DataSource = SizeList.ToList();
                    sizeComboBox.DisplayMember = "Value";
                    sizeComboBox.ValueMember = "Key";
                }

                if (dia == "")
                {
                    diaComboBox.DataSource = null;
                    diaComboBox.DataBindings.Clear();
                    diaComboBox.DataSource = DiaList.ToList();
                    diaComboBox.DisplayMember = "Value";
                    diaComboBox.ValueMember = "Key";
                }

                if (part == "")
                {
                    partComboBox.DataSource = null;
                    partComboBox.DataBindings.Clear();
                    partComboBox.DataSource = PartList.ToList();
                    partComboBox.DisplayMember = "Value";
                    partComboBox.ValueMember = "Key";
                }

                CommonFunctions.connection.Close();
                FirstTime = false;
            } 
        }

        private string QueryForSearch()
        {
            string MachineNoList = "";

            int orderStatus = orderStatusComboBox.Text.Trim() == "In-Active" ? (int)VariableDecleration_Class.Status.InActive : (int)Enum.Parse(typeof(VariableDecleration_Class.Status), orderStatusComboBox.Text);

            foreach (var obj in MachineList)
            {
                if (!obj.ToString().Equals("ALL"))
                {
                    MachineNoList = MachineNoList + obj.ToString() + ",";
                }
            }

            MachineNoList = MachineNoList.TrimEnd(',');

            if (MachineComboBox.SelectedIndex != 0)
            {
                MachineNoList = MachineComboBox.Items[MachineComboBox.SelectedIndex].ToString();
            }

            string orderby = " order by OrderID, BuyerName, StyleName, SizeName, Dia, PartName asc";
            string query = "SELECT OrderID, MachineNo, TaskDate, ShipmentDate, OrderQty, SAM, Efficiency, PlanQty, ActualQty, BuyerName, StyleName, SizeName, Dia, PartName FROM Planing_Board_Details WHERE OrderStatus = " + orderStatus;
            
            if (buyer != "")
            {
                query = query + " AND BuyerID = " + buyer;
            }
            if (style != "")
            {
                query = query + " AND StyleID = " + style;
            }
            if (size != "")
            {
                query = query + " AND SizeID = " + size;
            }
            if (dia != "")
            {
                query = query + " AND DiaID = " + dia;
            }
            if (part != "")
            {
                query = query + " AND PartID = " + part;
            }

            if (MachineList.Count > 2)
            {
                query = query + " AND MachineNo IN (" + MachineNoList + ")";
            }

            if (shipRadioButton.Checked)
            {
                query = query + " AND ShipmentDate Between '"+ shipFromDateTimePicker.Value+"' AND '"+shipToDateTimePicker.Value+"'";
            }

            if (proRadioButton.Checked)
            {
                query = query + " AND TaskDate Between '" + proFromDateTimePicker.Value + "' AND '" + proToDateTimePicker.Value + "'";
            }

            query = query + orderby;

            return query;
        }

        private void buyerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (buyerComboBox.SelectedIndex > -1)
            {
                buyer = buyerComboBox.SelectedIndex == 0 ? "" : buyerComboBox.SelectedValue.ToString();
                if (!FirstTime)
                {
                    QueryForCombo();
                }
            }
        }

        private void styleComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (styleComboBox.SelectedIndex > -1)
            {
                style = styleComboBox.SelectedIndex == 0 ? "" : styleComboBox.SelectedValue.ToString();
                if (!FirstTime)
                {
                    QueryForCombo();
                }
            }
        }

        private void sizeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sizeComboBox.SelectedIndex > -1)
            {
                size = sizeComboBox.SelectedIndex == 0 ? "" : sizeComboBox.SelectedValue.ToString();
                if (!FirstTime)
                {
                    QueryForCombo();
                }
            }
        }

        private void diaComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (diaComboBox.SelectedIndex > -1)
            {
                dia = diaComboBox.SelectedIndex == 0 ? "" : diaComboBox.SelectedValue.ToString();
                if (!FirstTime)
                {
                    QueryForCombo();
                }
            }
        }

        private void partComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (partComboBox.SelectedIndex > -1)
            {
                part = partComboBox.SelectedIndex == 0 ? "" : partComboBox.SelectedValue.ToString();
                if (!FirstTime)
                {
                    QueryForCombo();
                }
            }
        }

        private void shipFromDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (shipToDateTimePicker.Text == "" || shipFromDateTimePicker.Text == "")
            {
                return;
            }
            if (shipToDateTimePicker.Value < shipFromDateTimePicker.Value)
            {
                MessageBox.Show("FROM DATE Can not be Greater than To DATE");
                shipFromDateTimePicker.Value = DateTime.Now.Date;
                return;
            }
        }

        private void shipToDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (shipToDateTimePicker.Text == "" || shipFromDateTimePicker.Text == "")
            {
                return;
            }

            if (shipToDateTimePicker.Value < shipFromDateTimePicker.Value)
            {
                MessageBox.Show("FROM DATE Can not be Greater than To DATE");
                shipToDateTimePicker.Value = DateTime.Now.Date;
                return;
            }
        }

        private void proFromDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (proToDateTimePicker.Text == "" || proFromDateTimePicker.Text == "")
            {
                return;
            }
            if (proToDateTimePicker.Value < proFromDateTimePicker.Value)
            {
                MessageBox.Show("FROM DATE Can not be Greater than To DATE");
                proFromDateTimePicker.Value = DateTime.Now.Date;
                return;
            }
        }

        private void proToDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (proToDateTimePicker.Text == "" || proFromDateTimePicker.Text == "")
            {
                return;
            }
            if (proToDateTimePicker.Value < proFromDateTimePicker.Value)
            {
                MessageBox.Show("FROM DATE Can not be Greater than To DATE");
                proToDateTimePicker.Value = DateTime.Now.Date;
                return;
            }
        }

        private void MStatuscomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMachine();
        }

        private void shipRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            isShipChecked = shipRadioButton.Checked;
        }

        private void proRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            isProChecked = proRadioButton.Checked;
        }

        private void shipRadioButton_Click(object sender, EventArgs e)
        {
            if (shipRadioButton.Checked && !isShipChecked)
            {
                shipRadioButton.Checked = false;
                shipFromDateTimePicker.Enabled = false;
                shipToDateTimePicker.Enabled = false;
            }
            else
            {
                shipRadioButton.Checked = true;
                isShipChecked = false;
                shipFromDateTimePicker.Enabled = true;
                shipToDateTimePicker.Enabled = true;
            }
        }

        private void proRadioButton_Click(object sender, EventArgs e)
        {
            if (proRadioButton.Checked && !isProChecked)
            {
                proRadioButton.Checked = false;
                proFromDateTimePicker.Enabled = false;
                proToDateTimePicker.Enabled = false;
            }
            else
            {
                proRadioButton.Checked = true;
                isProChecked = false;
                proFromDateTimePicker.Enabled = true;
                proToDateTimePicker.Enabled = true;
            }
        }

        private void orderStatusComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
