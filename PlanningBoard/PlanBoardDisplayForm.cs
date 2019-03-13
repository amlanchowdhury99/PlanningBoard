using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Drawing.Drawing2D;
//System.Drawing.Color.FromArgb(0, 128, 255);
namespace PlanningBoard
{
    public partial class PlanBoardDisplayForm : Form
    {
        public Dictionary<int, string> MachineDiaList = new Dictionary<int, string>();
        public static DateTime fromDate = DateTime.Now.Date;
        public static DateTime toDate = DateTime.Now.Date;

        public static string orderIDs = "";
        public static int orderID = 0;
        public static int MachineNo = 0;
        public static string orderQuantity = "";
        public static string plnQuantity = "";
        public static string sam = "";
        public static string style = "";
        public static string size = "";
        public static string dia = "";
        public static string part = "";
        public static string eff = "";
        public static string capacity = "";
        public static string remarks = "";
        public static string planDate = "";
        public static string shipDate = "";
        public static string ActualProduction = "";
        public static int rowIndex = -1;
        public static int colIndex = -1;
        public int OrderFactorsCount = 12;
        public static Boolean EditMode = false;

        public static DateTime planStartDate = DateTime.Now.Date;
        public static DateTime planEndDate = DateTime.Now.Date;
        public List<int> gridMachineRowStartIndex = new List<int>();
        public int extraCols = 0;
        public static List<string> Paras = new List<string>() { "STYLE :", "PARTS :", "SIZE :", "SAM :", "EFFICIENCY :", "CAPACITY :", "PLAN QTY :", "CHD :", "ORDER-QTY :", "ACTUAL PRODUCTION :", "ORDER ID :" };

        public PlanBoardDisplayForm()
        {
            InitializeComponent();
            fromDateTimePicker.Value = DateTime.Now.Date;
            toDateTimePicker.Value = DateTime.Now.Date;
            planBoardDataGridView.AutoGenerateColumns = false;
            planBoardDataGridView.ClearSelection();
            LoadMachine();
        }

        private void PlanBoardDisplayForm_Load(object sender, EventArgs e)
        {
            if (orderID != 0)
            {
                orderIDs = "";
                orderID = 0;
                MachineNo = 0;
                orderQuantity = "";
                plnQuantity = "";
                rowIndex = -1;
                colIndex = -1;
                sam = "";
                style = "";
                size = "";
                dia = "";
                part = "";
                eff = "";
                capacity = "";
                remarks = "";
                shipDate = "";
                planDate = "";
                planStartDate = DateTime.Now.Date;
                planEndDate = DateTime.Now.Date;
                gridMachineRowStartIndex.Clear();
                fromDate = DateTime.Now.Date;
                toDate = DateTime.Now.Date;
                extraCols = 0;
                OrderFactorsCount = 12;
                EditMode = false;
            }
        }

        private void LoadMachine()
        {
            try
            {
                string query = "SELECT * FROM Machine_Info WHERE Status != " + 0 + " order by MachineNo asc";

                SqlDataReader reader = CommonFunctions.GetFromDB(query);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (!MachineDiaList.Keys.Contains(Convert.ToInt32(reader["Id"])))
                        {
                            MachineDiaList.Add(Convert.ToInt32(reader["MachineNo"]), (reader["MachineDia"]).ToString());
                        }
                        else
                        {
                            MachineDiaList[Convert.ToInt32(reader["MachineNo"])] = reader["MachineDia"].ToString();
                        }
                    }
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

        private void GridInitialize()
        {
            if (orderID < 100)
            {
                planBoardDataGridView.Rows.Clear();
                planBoardDataGridView.Columns.Clear();
                planBoardDataGridView.EnableHeadersVisualStyles = false;
                int totalCols = (Convert.ToDateTime(toDateTimePicker.Value) - Convert.ToDateTime(fromDateTimePicker.Value)).Days + 1;

                for (int i = 0; i < totalCols + 3; i++)
                {
                    planBoardDataGridView.Columns.Add(i.ToString(), i.ToString());
                    if (i > 2)
                    {
                        planBoardDataGridView.Columns[i].Width = 170; 
                    }
                }
                SetColWidth();
                if (MachineDiaList.Count > 0)
                {
                    SetRowIndex();
                }
            }
            else
            {
                if (toDate < planEndDate)
                {
                    extraCols = (planEndDate - toDate).Days + 1;
                    int prevColCount = planBoardDataGridView.Columns.Count;
                    for (int i = planBoardDataGridView.Columns.Count; i < prevColCount + extraCols; i++)
                    {
                        planBoardDataGridView.Columns.Add(i.ToString(), i.ToString());
                    }
                    SetColWidth();
                }
            }

            AddCommonRows();
            //FetchDateFromDB();
            SetGridColor();
            planBoardDataGridView.CellPainting += new DataGridViewCellPaintingEventHandler(planBoardDataGridView_CellPainting);
            planBoardDataGridView.CurrentCellChanged += new EventHandler(planBoardDataGridView_CurrentCellChanged);
        }

        private void Generate_Plan_Board()
        {
            string connectionStr = ConnectionManager.connectionString;
            SqlCommand cm = new SqlCommand();
            SqlConnection cn = new SqlConnection(connectionStr);
            cn.Open();
            cm.Connection = cn;

            try
            {
                for (int y = 3; y < planBoardDataGridView.ColumnCount; y++)
                {
                    DateTime GetDate = DateTime.ParseExact(planBoardDataGridView.Rows[1].Cells[y].Value.ToString(), "dd/MM/yyyy", null);

                    for (int x = 3; x < planBoardDataGridView.RowCount - 1; x++)
                    {
                        try
                        {
                            string Buyer = "";
                            string Style = "";
                            string Parts = "";
                            string Size = "";
                            string SAM = "";
                            string Efficiency = "";
                            string Capacity = "";
                            string PlanQty = "";
                            string CHD = "";
                            string OrderQty = "";
                            string ActualQty = "";
                            string Style1 = "";
                            string OrderIDList = "";
                            
                            cm.CommandText = "select * from Planing_Board_Details where TaskDate='" + GetDate + "' and MachineNo='" + Convert.ToInt16(planBoardDataGridView.Rows[x].Cells[0].Value.ToString()) + "'";

                            SqlDataReader reader;
                            reader = cm.ExecuteReader();

                            while (reader.Read())
                            {
                                if (Convert.ToInt16(reader["Capacity"]) != 0)
                                {
                                    Buyer = Buyer == "" ? Convert.ToString(reader["BuyerName"]) : Buyer + ";" + Convert.ToString(reader["BuyerName"]);

                                    if (Style1 == "")
                                    {
                                        if (Style != "")
                                            Style = Style + ";" + Convert.ToString(reader["StyleName"]);
                                        else
                                            Style = Convert.ToString(reader["StyleName"]);
                                    }
                                    else if (Style1 != Convert.ToString(reader["StyleName"]))
                                    {
                                        if (Style != "")
                                            Style = Style + ";" + Convert.ToString(reader["StyleName"]);
                                        else
                                            Style = Convert.ToString(reader["StyleName"]);
                                    }

                                    Parts = Parts == "" ? Convert.ToString(reader["PartName"]) : Parts + ";" + Convert.ToString(reader["PartName"]);
                                    Size = Size == "" ? Convert.ToString(reader["SizeName"]) : Size + ";" + Convert.ToString(reader["SizeName"]);
                                    SAM = SAM == "" ? Convert.ToString(reader["SAM"]) : SAM + ";" + Convert.ToString(reader["SAM"]);
                                    Efficiency = Efficiency == "" ? Convert.ToString(reader["Efficiency"]) : Efficiency + ";" + Convert.ToString(reader["Efficiency"]);
                                    Capacity = Capacity == "" ? Convert.ToString(reader["Capacity"]) : Capacity + ";" + Convert.ToString(reader["Capacity"]);
                                    PlanQty = PlanQty == "" ? Convert.ToString(reader["PlanQty"]) : PlanQty + ";" + Convert.ToString(reader["PlanQty"]);
                                    DateTime SetTime = Convert.ToDateTime(reader["ShipmentDate"]);
                                    CHD = CHD == "" ? SetTime.ToString("dd/MM/yyyy") : CHD + ";" + SetTime.ToString("dd/MM/yyyy");
                                    OrderQty = OrderQty == "" ? Convert.ToString(reader["OrderQty"]) : OrderQty + ";" + Convert.ToString(reader["OrderQty"]);
                                    //ActualQty = ActualQty == "" ? Convert.ToString(reader["ActualQty"]) : OrderQty + "-" + Convert.ToString(reader["ActualQty"]);
                                    OrderIDList = OrderIDList == "" ? Convert.ToString(reader["OrderID"]) : OrderIDList + "," + Convert.ToString(reader["OrderID"]);

                                    Style1 = Convert.ToString(reader["StyleName"]);
                                }
                            }

                            planBoardDataGridView.Rows[x].Cells[y].Value = Buyer;
                            x++;
                            planBoardDataGridView.Rows[x].Cells[y].Value = Style;
                            x++;
                            planBoardDataGridView.Rows[x].Cells[y].Value = Parts;
                            x++;
                            planBoardDataGridView.Rows[x].Cells[y].Value = Size;
                            x++;
                            planBoardDataGridView.Rows[x].Cells[y].Value = SAM;
                            x++;
                            planBoardDataGridView.Rows[x].Cells[y].Value = Efficiency;
                            x++;
                            planBoardDataGridView.Rows[x].Cells[y].Value = Capacity;
                            x++;
                            planBoardDataGridView.Rows[x].Cells[y].Value = PlanQty;
                            x++;
                            planBoardDataGridView.Rows[x].Cells[y].Value = CHD;
                            x++;
                            planBoardDataGridView.Rows[x].Cells[y].Value = OrderQty;
                            x++;
                            planBoardDataGridView.Rows[x].Cells[y].Value = ActualQty;
                            x++;
                            planBoardDataGridView.Rows[x].Cells[y].Value = OrderIDList;

                            reader.Dispose();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally
            {
                cn.Close();
                ResetPlanBoardColor();
            }

        }

        private void SetRowIndex()
        {
            for (int i = 0; i < MachineDiaList.Count; i++ )
            {
                if (i == 0)
                {
                    gridMachineRowStartIndex.Add(3);
                }
                else
                {
                    gridMachineRowStartIndex.Add(gridMachineRowStartIndex[i - 1] + OrderFactorsCount);
                }
            }
        }

        private void SetGridColor() 
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < planBoardDataGridView.Columns.Count; j++)
                {
                    if (j == 0)
                        planBoardDataGridView.Rows[i].Cells[j].Style.BackColor = Color.CornflowerBlue;
                    else if (j == 1 || (i == 0 && j > 2))
                        planBoardDataGridView.Rows[i].Cells[j].Style.BackColor = Color.Moccasin;
                    else if (j == 2)
                        planBoardDataGridView.Rows[i].Cells[j].Style.BackColor = Color.LightPink;
                    else if (i == 1 && j > 2)
                        planBoardDataGridView.Rows[i].Cells[j].Style.BackColor = Color.DarkKhaki;
                }
            }
        }

        private void SetColWidth()
        {
            planBoardDataGridView.Columns[2].Width = 170; 
        }

        private void FetchDateFromDB()
        {
            try
            {
                string query = "";
                SqlDataReader reader = null;
                for (DateTime date = fromDateTimePicker.Value; date <= fromDateTimePicker.Value; date = date.AddDays(1))
                {
                    foreach(var item in MachineDiaList)
                    {
                        style = " ";
                        part = " ";
                        size = " ";
                        sam = " ";
                        eff = " ";
                        capacity = " ";
                        orderQuantity = " ";
                        rowIndex = -1;
                        colIndex = -1;
                        shipDate = " ";
                        plnQuantity = " ";
                        ActualProduction = " ";
                        query = "SELECT * FROM PlanTable WHERE MachineNo = " + item.Key + " AND Dia = " + item.Value + " AND TaskDate = '" + DateTime.ParseExact(date.ToString(), "dd/MM/yyyy", null) + "'";
                        reader = CommonFunctions.GetFromDB(query);
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                style = style + "+" + reader["Style"].ToString();
                                part = part + "+" + reader["Part"].ToString();
                                size = size + "+" + reader["Size"].ToString();
                                sam = sam + "+" + reader["SAM"].ToString();
                                eff = eff + "+" + reader["Efficiency"].ToString();
                                capacity = capacity + "+" + reader["Capacity"].ToString();
                                plnQuantity = plnQuantity + "+" + reader["PlanQuantity"].ToString();
                                shipDate = shipDate + "+" + reader["CHD"].ToString();
                                orderQuantity = orderQuantity + "+" + reader["OrderQty"].ToString();
                                colIndex = (int)reader["ColIndex"];
                            }
                            SetValueMachineWise(item.Key);
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }

        private void SetValueMachineWise(int McNo)
        {
            for (int i = 3; i < planBoardDataGridView.Rows.Count; i = i + OrderFactorsCount)
            {
                if ((string)planBoardDataGridView.Rows[i].Cells[0].Value == McNo.ToString())
                {
                    int col = Convert.ToInt32(colIndex);
                    planBoardDataGridView.Rows[i].Cells[col].Value = style;
                    planBoardDataGridView.Rows[i + 1].Cells[col].Value = part;
                    planBoardDataGridView.Rows[i + 2].Cells[col].Value = size;
                    planBoardDataGridView.Rows[i + 3].Cells[col].Value = sam;
                    planBoardDataGridView.Rows[i + 4].Cells[col].Value = eff;
                    planBoardDataGridView.Rows[i + 5].Cells[col].Value = capacity;
                    planBoardDataGridView.Rows[i + 6].Cells[col].Value = plnQuantity;
                    planBoardDataGridView.Rows[i + 7].Cells[col].Value = shipDate;
                    planBoardDataGridView.Rows[i + 8].Cells[col].Value = orderQuantity;
                    planBoardDataGridView.Rows[i + 9].Cells[col].Value = ActualProduction;
                    planBoardDataGridView.Rows[i + 10].Cells[col].Value = ActualProduction;
                    planBoardDataGridView.Rows[i + 11].Cells[col].Value = ActualProduction;
                }
            }
        }

        private void AddCommonRows()
        {
            DateTime currentDate;
            string currentDay = "";
            var queue = new Queue<string>(Paras);
            for (int i = 0; i < (MachineDiaList.Count * OrderFactorsCount) + 3; i++) // Row Loop
            {
                currentDate = fromDateTimePicker.Value;
                var row = new DataGridViewRow();
                for (int j = 0; j < planBoardDataGridView.Columns.Count; j++) // Column Loop
                {
                    if (i == 0)
                    {
                        if (j == 0)
                        {
                            row.Cells.Add(new DataGridViewTextBoxCell { Value = "       Machine" });
                            row.Cells[j].Style.BackColor = Color.CornflowerBlue;
                        }
                        else if (j == 1)
                        {
                            row.Cells.Add(new DataGridViewTextBoxCell { Value = "       Dia" });
                        }
                        else if (j == 2)
                        {
                            row.Cells.Add(new DataGridViewTextBoxCell { Value = "         Daily Plan Qty" });
                        }
                        else
                        {
                            row.Cells.Add(new DataGridViewTextBoxCell { Value = i.ToString() });
                        }
                    }
                    else if (i == 1)
                    {
                        if (j == 0)
                        {
                            row.Cells.Add(new DataGridViewTextBoxCell { Value = "       Machine" });
                            row.Cells[j].Style.BackColor = Color.CornflowerBlue;

                        }
                        else if (j == 1)
                        {
                            row.Cells.Add(new DataGridViewTextBoxCell { Value = "       Dia" });
                        }
                        else if (j == 2)
                        {
                            row.Cells.Add(new DataGridViewTextBoxCell { Value = "         Order" });
                        }
                        else
                        {
                            if (fromDateTimePicker.Value <= toDateTimePicker.Value)
                            {
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = currentDate.ToString("dd/MM/yyyy") });
                                currentDate = currentDate.AddDays(1);
                            }
                        }
                    }
                    else if (i == 2)
                    {
                        if (j == 2)
                        {
                            row.Cells.Add(new DataGridViewTextBoxCell { Value = "         Description" });
                        }
                        else
                        {
                            if (j == 0)
                            {
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = "       Machine" });
                            }
                            else if (j == 1)
                            {
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = "       Dia" });
                            }
                            else
                            {
                                if (fromDateTimePicker.Value <= toDateTimePicker.Value)
                                {
                                    currentDay = currentDate.DayOfWeek.ToString();
                                    row.Cells.Add(new DataGridViewTextBoxCell { Value = currentDay });
                                    currentDate = currentDate.AddDays(1);
                                }
                            }

                            if (j == 0)
                                row.Cells[j].Style.BackColor = Color.CornflowerBlue;
                        }
                    }
                    else
                    {
                        int MachineRowIndex = gridMachineRowStartIndex.IndexOf(i);
                        if (j == 0)
                        {
                            if(MachineRowIndex == -1)
                            {
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = planBoardDataGridView.Rows[i-1].Cells[0].Value.ToString() });
                            }
                            else
                            {
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = MachineDiaList.Keys.ElementAt(MachineRowIndex).ToString() });
                            }
                        }

                        if (j == 1)
                        {
                            if (MachineRowIndex == -1)
                            {
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = planBoardDataGridView.Rows[i - 1].Cells[1].Value.ToString() });
                            }
                            else
                            {
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = MachineDiaList.Values.ElementAt(MachineRowIndex).ToString() });
                            }
                        }
                        if (j == 2)
                        {
                            if (MachineRowIndex == -1)
                            {
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = queue.Dequeue() });
                            }
                            else
                            {
                                queue = new Queue<string>(Paras);
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = "BUYER :" });
                            }
                        }
                    }
                }
                
                planBoardDataGridView.Rows.Add(row);
                if (i > 3 && queue.Count == 0)
                {
                    planBoardDataGridView.Rows[i].Visible = false;
                }
            }
        }

        private void planBoardDataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            e.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            if (e.RowIndex < 1 || e.ColumnIndex < 0)
                return;

            if ((e.ColumnIndex > 2) || (e.RowIndex % OrderFactorsCount == 3 && e.ColumnIndex < 2))
            {
                e.AdvancedBorderStyle.Top = planBoardDataGridView.AdvancedCellBorderStyle.Top;
            }
            else
            {
                if (IsTheSameCellValue(e.ColumnIndex, e.RowIndex))
                {
                    e.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
                }
                else
                {
                    e.AdvancedBorderStyle.Top = planBoardDataGridView.AdvancedCellBorderStyle.Top;
                }
            } 
        }

        bool IsTheSameCellValue(int column, int row)
        {
            DataGridViewCell cell1 = planBoardDataGridView[column, row];
            DataGridViewCell cell2 = planBoardDataGridView[column, row - 1];
            if (cell1.ColumnIndex == 2 && cell1.RowIndex == 2 && cell2.ColumnIndex == 2 && cell2.RowIndex == 1)
            {
                return true;
            }
            if (cell1.Value == null || cell2.Value == null)
            {
                return false;
            }
            if (cell1.Value == "" || cell2.Value == "")
            {
                return false;
            }
            return cell1.Value.ToString() == cell2.Value.ToString();
        }

        private void planBoardDataGridView_CurrentCellChanged(object sender, EventArgs e)
        {
            //planBoardDataGridView.Refresh();
        }

        private void BtnGeneratePlan_Click(object sender, EventArgs e)
        {
            if (fromDateTimePicker.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter FROM Date Information", VariableDecleration_Class.sMSGBOX);
                fromDateTimePicker.Focus();
                return;
            }

            if (toDateTimePicker.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter TO Date Information", VariableDecleration_Class.sMSGBOX);
                toDateTimePicker.Focus();
                return;
            }

            if (toDateTimePicker.Value < fromDateTimePicker.Value)
            {
                MessageBox.Show("FROM Date Can not be greater than TO Date", VariableDecleration_Class.sMSGBOX);
                toDateTimePicker.Focus();
                return;
            }
            fromDate = fromDateTimePicker.Value;
            toDate = toDateTimePicker.Value;
            GridInitialize();
            Generate_Plan_Board();
        }

        private void BtnAddPlan_Click(object sender, EventArgs e)
        {
            //Home frm = Application.OpenForms.OfType<Home>().FirstOrDefault();
            //if (frm != null)
            //{
            //    frm.BringToFront();
            //    frm.ShowOrderInfo();
            //}

            if (planBoardDataGridView.Rows.Count < 1)
            {
                MessageBox.Show("PLease Generate Plan Board First!!!");
                return;
            }

            resetPlanInfo();
            EditMode = false;
            orderIDs = "";
            ViewOrderInfo viewOrderInfo = new ViewOrderInfo();
            viewOrderInfo.ShowDialog();
            if(orderID > 0)
            {
                if (planStartDate < fromDateTimePicker.Value)
                {
                    fromDateTimePicker.Value = planStartDate;
                }
                if (planEndDate > toDateTimePicker.Value)
                {
                    toDateTimePicker.Value = planEndDate;
                }
            }
            Generate_Plan_Board();
        }

        private void resetPlanInfo()
        {
            orderID = 0;
            planDate = "";
            planStartDate = DateTime.Now.Date;
            planEndDate = DateTime.Now.Date;
        }

        internal static void sendPlanValue(int orderId, string styleName, string sizeName, string diaName, string partName, int mcNo,  DateTime plnDate, DateTime plnStartDate, DateTime plnEndDate, string remarksText, int Qty, double efficiency, int McCapacity, string chd, int plnQty)
        {
            MachineNo = mcNo;
            orderID = orderId;
            planDate = plnDate.Date.ToString();
            planStartDate = plnStartDate.Date;
            planEndDate = plnEndDate.Date;
            style = styleName;
            part = partName;
            size = diaName;
            sam = "";
            eff = efficiency.ToString();
            capacity = McCapacity.ToString();
            orderQuantity =  Qty.ToString();
            shipDate = chd;
            plnQuantity = plnQty.ToString();
            string remarks = remarksText;
        }

        private void planBoardDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex == 0)
                return;

            if (IsTheSameCellValue(e.ColumnIndex, e.RowIndex))
            {
                if ((e.ColumnIndex == 2 && e.RowIndex == 2) || e.ColumnIndex > 2 || (e.RowIndex % OrderFactorsCount == 3 && e.ColumnIndex < 2))
                {
                    return;
                }
                e.Value = "";
                e.FormattingApplied = true;
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void planBoardDataGridView_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            e.Column.FillWeight = 1;
        }

        private void planBoardDataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            planBoardDataGridView.ClearSelection();
        }

        private void forwardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime taskDate = DateTime.ParseExact(planBoardDataGridView.Rows[1].Cells[colIndex].Value.ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            orderIDs = planBoardDataGridView.Rows[rowIndex + OrderFactorsCount - 1].Cells[colIndex].Value.ToString();
            int mcNo = Convert.ToInt32(planBoardDataGridView.Rows[rowIndex].Cells[0].Value);

            FBPlanBoardForm fbplanform = new FBPlanBoardForm(false, mcNo, taskDate, orderIDs, false);
            fbplanform.ShowDialog();
            Generate_Plan_Board();
        }

        private void backwardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime taskDate = DateTime.ParseExact(planBoardDataGridView.Rows[1].Cells[colIndex].Value.ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            orderIDs = planBoardDataGridView.Rows[rowIndex + OrderFactorsCount - 1].Cells[colIndex].Value.ToString();
            int mcNo = Convert.ToInt32(planBoardDataGridView.Rows[rowIndex].Cells[0].Value);
            FBPlanBoardForm fbplanform = new FBPlanBoardForm(true, mcNo, taskDate, orderIDs, false);
            fbplanform.ShowDialog();
            Generate_Plan_Board();
        }

        private void planBoardDataGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex % OrderFactorsCount == 3 && e.ColumnIndex > 2 && (planBoardDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != null && planBoardDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "") && e.Button == MouseButtons.Right)
            {
                rowIndex = e.RowIndex;
                colIndex = e.ColumnIndex;
                this.pBContextMenuStrip.Show(this.planBoardDataGridView, e.Location);
                pBContextMenuStrip.Show(Cursor.Position);
            }
        }

        private void planBoardDataGridView_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            //if (e.RowIndex > 2 && e.ColumnIndex > 2 && e.Button == MouseButtons.Right)
            //{
            //    //this.planBoardDataGridView.Rows[e.RowIndex].Selected = true;
            //    rowIndex = e.RowIndex;
            //    this.planBoardDataGridView.CurrentCell = this.planBoardDataGridView.Rows[e.RowIndex].Cells[1];
            //    this.pBContextMenuStrip.Show(this.planBoardDataGridView, e.Location);
            //    pBContextMenuStrip.Show(Cursor.Position);
            //}
        }

        private void planBoardDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex % OrderFactorsCount == 3 && e.ColumnIndex > 2)
            {
                colIndex = e.ColumnIndex;
                if (planBoardDataGridView.Rows[e.RowIndex + OrderFactorsCount - 1].Cells[colIndex].Value.ToString() != "")
                {
                    int McNo = Convert.ToInt32(planBoardDataGridView.Rows[e.RowIndex].Cells[0].Value);
                    orderIDs = planBoardDataGridView.Rows[e.RowIndex + OrderFactorsCount - 1].Cells[colIndex].Value.ToString();
                    string currentDate = planBoardDataGridView.Rows[1].Cells[colIndex].Value.ToString();
                    EditMode = true;
                    ViewOrderInfo viewOrderInfo = new ViewOrderInfo(McNo, currentDate);
                    viewOrderInfo.ShowDialog();
                    Generate_Plan_Board();
                }
            }
        }

        private void planBoardDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ResetPlanBoardColor();
            if (e.RowIndex > 2 && e.ColumnIndex > 2)
            {
                string buyer = "";
                string style = "";
                string parts = "";
                string size = "";

                rowIndex = e.RowIndex;
                colIndex = e.ColumnIndex;
                //if (planBoardDataGridView.Rows[(e.RowIndex + OrderFactorsCount - 1) - (e.RowIndex - 3)].Cells[colIndex].Value.ToString() != "")
                if (planBoardDataGridView.Rows[e.RowIndex].Cells[colIndex].Value != null && planBoardDataGridView.Rows[e.RowIndex].Cells[colIndex].Value.ToString() != "" )
                {
                    if (e.RowIndex % OrderFactorsCount == 3)
                    {
                        buyer = planBoardDataGridView.Rows[e.RowIndex - 0].Cells[colIndex].Value.ToString();
                        if(buyer.Contains(";"))
                            buyer = buyer.Split(';')[0].Trim();
                    }
                    else if (e.RowIndex % OrderFactorsCount == 4)
                    {
                        buyer = planBoardDataGridView.Rows[e.RowIndex - 1].Cells[colIndex].Value.ToString();
                        if (buyer.Contains(";"))
                            buyer = buyer.Split(';')[0].Trim();
                        style = planBoardDataGridView.Rows[e.RowIndex - 0].Cells[colIndex].Value.ToString();
                        if (style.Contains(";"))
                            style = style.Split(';')[0].Trim();
                    }
                    else if (e.RowIndex % OrderFactorsCount == 5)
                    {
                        buyer = planBoardDataGridView.Rows[e.RowIndex - 2].Cells[colIndex].Value.ToString();
                        if (buyer.Contains(";"))
                            buyer = buyer.Split(';')[0].Trim();
                        style = planBoardDataGridView.Rows[e.RowIndex - 1].Cells[colIndex].Value.ToString();
                        if (style.Contains(";"))
                            style = style.Split(';')[0].Trim();
                        parts = planBoardDataGridView.Rows[e.RowIndex - 0].Cells[colIndex].Value.ToString();
                        if (buyer.Contains(";"))
                            parts = parts.Split(';')[0].Trim();
                    }
                    else if (e.RowIndex % OrderFactorsCount == 6)
                    {
                        buyer = planBoardDataGridView.Rows[e.RowIndex - 3].Cells[colIndex].Value.ToString();
                        if (buyer.Contains(";"))
                            buyer = buyer.Split(';')[0].Trim();
                        style = planBoardDataGridView.Rows[e.RowIndex - 2].Cells[colIndex].Value.ToString();
                        if (style.Contains(";"))
                            style = style.Split(';')[0].Trim();
                        parts = planBoardDataGridView.Rows[e.RowIndex - 1].Cells[colIndex].Value.ToString();
                        if (buyer.Contains(";"))
                            parts = parts.Split(';')[0].Trim();
                        size = planBoardDataGridView.Rows[e.RowIndex - 0].Cells[colIndex].Value.ToString();
                        if (buyer.Contains(";"))
                            size = size.Split(';')[0].Trim();
                    }

                    for (int i = 3; i < planBoardDataGridView.ColumnCount; i++)
                    {
                        for (int j = 3; j < planBoardDataGridView.Rows.Count; j = j + OrderFactorsCount)
                        {
                            if (planBoardDataGridView.Rows[j].Cells[i].Value != null)
                            {
                                if (e.RowIndex % OrderFactorsCount == 3)
                                {
                                    if (planBoardDataGridView.Rows[j].Cells[i].Value.ToString().Contains(buyer))
                                    {
                                        if (planBoardDataGridView.Rows[j].Cells[i].Value.ToString() == buyer)
                                        {
                                            ColorPlanBoard(j, i, Color.SlateBlue);
                                        }
                                        else
                                        {
                                            ColorPlanBoard(j, i, Color.Thistle);
                                        }
                                    }
                                }
                                if (e.RowIndex % OrderFactorsCount == 4)
                                {
                                    if (planBoardDataGridView.Rows[j].Cells[i].Value.ToString().Contains(buyer) && planBoardDataGridView.Rows[j + 1].Cells[i].Value.ToString().Contains(style))
                                    {
                                        if (planBoardDataGridView.Rows[j].Cells[i].Value.ToString() == buyer && planBoardDataGridView.Rows[j + 1].Cells[i].Value.ToString() == style)
                                        {
                                            ColorPlanBoard(j, i, Color.SlateBlue);
                                        }
                                        else
                                        {
                                            ColorPlanBoard(j, i, Color.Thistle);
                                        }
                                    }
                                }
                                if (e.RowIndex % OrderFactorsCount == 5)
                                {
                                    if (planBoardDataGridView.Rows[j].Cells[i].Value.ToString().Contains(buyer) && planBoardDataGridView.Rows[j + 1].Cells[i].Value.ToString().Contains(style) && planBoardDataGridView.Rows[j + 2].Cells[i].Value.ToString().Contains(parts))
                                    {
                                        if (planBoardDataGridView.Rows[j].Cells[i].Value.ToString() == buyer && planBoardDataGridView.Rows[j + 1].Cells[i].Value.ToString() == style && planBoardDataGridView.Rows[j + 2].Cells[i].Value.ToString() == parts)
                                        {
                                            ColorPlanBoard(j, i, Color.SlateBlue);
                                        }
                                        else
                                        {
                                            ColorPlanBoard(j, i, Color.Thistle);
                                        }
                                    }
                                }
                                if (e.RowIndex % OrderFactorsCount == 6)
                                {
                                    if (planBoardDataGridView.Rows[j].Cells[i].Value.ToString().Contains(buyer) && planBoardDataGridView.Rows[j + 1].Cells[i].Value.ToString().Contains(style) && planBoardDataGridView.Rows[j + 2].Cells[i].Value.ToString().Contains(parts) && planBoardDataGridView.Rows[j + 3].Cells[i].Value.ToString().Contains(size))
                                    {
                                        if (planBoardDataGridView.Rows[j].Cells[i].Value.ToString() == buyer && planBoardDataGridView.Rows[j + 1].Cells[i].Value.ToString() == style && planBoardDataGridView.Rows[j + 2].Cells[i].Value.ToString() == parts && planBoardDataGridView.Rows[j + 3].Cells[i].Value.ToString() == size)
                                        {
                                            ColorPlanBoard(j, i, Color.SlateBlue);
                                        }
                                        else
                                        {
                                            ColorPlanBoard(j, i, Color.Thistle);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void PlanBoardColorManagement()
        {
            for (int i = 3; i < planBoardDataGridView.ColumnCount; i++)
            {
                for (int j = 3; j < planBoardDataGridView.Rows.Count; j = j + OrderFactorsCount)
                {
                    if (planBoardDataGridView.Rows[j].Cells[i].Value == null || planBoardDataGridView.Rows[j].Cells[i].Value == "")
                    {
                        int mcNo = Convert.ToInt32(planBoardDataGridView.Rows[j].Cells[0].Value);
                        DateTime workDate = DateTime.ParseExact(planBoardDataGridView.Rows[1].Cells[i].Value.ToString(), "dd/MM/yyyy", null);

                        if (CommonFunctions.recordExist("select * from WorkingDays where MachineNo=" + mcNo + " and Active = 0 and WorkDate='" + workDate + "'"))
                        {
                            ColorPlanBoard(j, i, Color.RosyBrown);
                        }
                        else
                        {
                            ColorPlanBoard(j, i, Color.DarkGray);
                        }
                        
                    }
                    else
                    {
                        var capVal = planBoardDataGridView.Rows[j + 6].Cells[i].Value.ToString();
                        var qtyVal = planBoardDataGridView.Rows[j + 7].Cells[i].Value.ToString();
                        int capacity = capVal.Contains(';') == true ? Convert.ToInt32(capVal.Split(';')[capVal.Split(';').Length - 1].Trim()) : Convert.ToInt32(capVal);
                        int plnQty = qtyVal.Contains(';') == true ? qtyVal.Split(new[] { ';' }).Select(x => int.Parse(x.Trim())).Sum() : Convert.ToInt32(qtyVal);
                        if (plnQty == capacity)
                        {
                            ColorPlanBoard(j, i, Color.DarkOliveGreen);
                        }
                        else
                        {
                            ColorPlanBoard(j, i, Color.ForestGreen);
                        }
                    }
                }
            }
        }

        private void ColorPlanBoard(int rowIndex, int colIndex, Color Clr)
        {
            int count = rowIndex + OrderFactorsCount;
            for (int k = rowIndex; k < count; k++)
            {
                planBoardDataGridView.Rows[k].Cells[colIndex].Style.BackColor = Clr;
            }
        }

        private void ResetPlanBoardColor()
        {
            for (int i = 3; i < planBoardDataGridView.ColumnCount; i++)
            {
                for (int j = 3; j < planBoardDataGridView.Rows.Count; j++)
                {
                    planBoardDataGridView.Rows[j].Cells[i].Style.BackColor = Color.White;
                }
            }
            PlanBoardColorManagement();
        }

        private void GenerateExcelBtn_Click(object sender, EventArgs e)
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

        private void changeWorkDateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime taskDate = DateTime.ParseExact(planBoardDataGridView.Rows[1].Cells[colIndex].Value.ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            orderIDs = planBoardDataGridView.Rows[rowIndex + OrderFactorsCount - 1].Cells[colIndex].Value.ToString();
            int mcNo = Convert.ToInt32(planBoardDataGridView.Rows[rowIndex].Cells[0].Value);
            FBPlanBoardForm fbplanform = new FBPlanBoardForm(true, mcNo, taskDate, orderIDs, true);
            fbplanform.ShowDialog();
            Generate_Plan_Board();
        }

    }
}
