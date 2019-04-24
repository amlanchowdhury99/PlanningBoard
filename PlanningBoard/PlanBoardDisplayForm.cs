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
using System.Configuration;
using System.Text.RegularExpressions;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Net.Http;

namespace PlanningBoard
{
    public partial class PlanBoardDisplayForm : Form
    {
        public Dictionary<int, string> MachineDiaList = new Dictionary<int, string>();
        //public Dictionary<int, int> PlanIndex = new Dictionary<int, int>();
        List<KeyValuePair<int, int>> PlanIndex = new List<KeyValuePair<int, int>>();
        ArrayList MachineList = new ArrayList();
        public static DateTime fromDate = DateTime.Now.Date;
        public static DateTime toDate = DateTime.Now.Date;

        clsResize _form_resize;

        public static string orderIDs = "";
        public static int orderID = 0;
        public static int MachineNo = 0;
        public static string orderQuantity = "";
        public static string plnQuantity = "";
        public static string sam = "";
        public static string po = "";
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
        public int OrderFactorsCount = 13;
        public static Boolean EditMode = false;

        public static DateTime planStartDate = DateTime.Now.Date;
        public static DateTime planEndDate = DateTime.Now.Date;
        public List<int> gridMachineRowStartIndex = new List<int>();
        public int extraCols = 0;
        public static List<string> Paras = new List<string>() { "STYLE :", "PURCHASE ORDER :", "PARTS :", "SIZE :", "SAM :", "EFFICIENCY :", "CAPACITY :", "PLAN QTY :", "CHD :", "ORDER-QTY :", "ACTUAL PRODUCTION :", "ORDER ID :" };

        public PlanBoardDisplayForm()
        {
            InitializeComponent();
            fromDateTimePicker.Value = DateTime.Now.Date;
            toDateTimePicker.Value = DateTime.Now.Date;
            LoadComboBox();
            planBoardDataGridView.AutoGenerateColumns = false;
            planBoardDataGridView.ClearSelection();

            _form_resize = new clsResize(this);
            this.Load += _Load;
            this.Resize += _Resize;
            this.planBoardDataGridView.DoubleBuffered(true);

        }

        private void _Load(object sender, EventArgs e)
        {
            _form_resize._get_initial_size();
        }

        private void _Resize(object sender, EventArgs e)
        {
            _form_resize._resize();
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

        private void LoadComboBox()
        {
            MStatuscomboBox.DisplayMember = "Description";
            MStatuscomboBox.ValueMember = "Value";
            MStatuscomboBox.DataSource = Enum.GetValues(typeof(VariableDecleration_Class.Status)).Cast<VariableDecleration_Class.Status>().Where(e => e != VariableDecleration_Class.Status.Pending && e != VariableDecleration_Class.Status.Complete).Cast<Enum>().Select(value => new { (Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()), typeof(DescriptionAttribute)) as DescriptionAttribute).Description, value }).OrderBy(item => item.value).ToList();
            MStatuscomboBox.SelectedIndex = 1;
        }

        private void LoadMachineStatusWise()
        {
            try
            {
                string query = "SELECT * FROM Machine_Info WHERE Status = " + MStatuscomboBox.SelectedIndex + " order by MachineNo asc";

                SqlDataReader reader = CommonFunctions.GetFromDB(query);
                MachineComboBox.DataSource = null;
                MachineComboBox.Items.Clear();
                MachineDiaList.Clear();
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

        private void LoadMachineOnPlanBoard()
        {
            try
            {
                var temp = "";

                foreach (var obj in MachineList)
                {
                    if (!obj.ToString().Equals("ALL"))
                    {
                        temp = temp == "" ? obj.ToString() : temp + "," + obj.ToString();
                    }
                }

                MachineDiaList.Clear();
                string MachineNoList = MachineComboBox.SelectedIndex == 0 ? temp : MachineComboBox.Items[MachineComboBox.SelectedIndex].ToString();
                string query = "SELECT * FROM Machine_Info WHERE MachineNo IN (" + MachineNoList + ") order by MachineNo asc";

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
                if (CommonFunctions.connection.State == ConnectionState.Open)
                {
                    CommonFunctions.connection.Close();
                }
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
            SetGridColor();
            planBoardDataGridView.CellPainting += new DataGridViewCellPaintingEventHandler(planBoardDataGridView_CellPainting);
            planBoardDataGridView.CurrentCellChanged += new EventHandler(planBoardDataGridView_CurrentCellChanged);
        }

        private void Generate_Plan_Board()
        {
            PictureBox.CheckForIllegalCrossThreadCalls = false;
            TextBox.CheckForIllegalCrossThreadCalls = false;
            this.Invoke((MethodInvoker)delegate { pinwheel.Visible = true; });
            
            planBoardDataGridView.Enabled = false;
            BtnGeneratePlan.Enabled = false;
            BtnAddPlan.Enabled = false;
            Search.Enabled = false;
            Revert.Enabled = false;
            MStatuscomboBox.Enabled = false;
            MachineComboBox.Enabled = false;
            
            Application.DoEvents();


            int DailyPlanQty = 0;
            string connectionStr = ConnectionManager.connectionString;
            SqlCommand cm = new SqlCommand(); SqlConnection cn = new SqlConnection(connectionStr); cn.Open(); cm.Connection = cn;
            Queue MachineNoList = new Queue();
            PlanIndex.Clear();

            try
            {
                for (int y = 3; y < planBoardDataGridView.ColumnCount; y++)
                {
                    DailyPlanQty = 0;
                    MachineNoList = new Queue();
                    foreach (var obj in MachineList)
                    {
                        if (!obj.ToString().Equals("ALL"))
                        {
                            MachineNoList.Enqueue(obj);
                        }
                    }

                    if (MachineComboBox.SelectedIndex != 0)
                    {
                        MachineNoList = new Queue();
                        MachineNoList.Enqueue(MachineComboBox.Items[MachineComboBox.SelectedIndex]);
                    }

                    DateTime GetDate = DateTime.ParseExact(planBoardDataGridView.Rows[1].Cells[y].Value.ToString(), "dd/MM/yyyy", null);

                    for (int x = 3; x < planBoardDataGridView.RowCount - 1; x++)
                    {
                        try
                        {
                            string Buyer = "";
                            string Style = "";
                            string Po = "";
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

                            cm.CommandText = "select * from Planing_Board_Details where RevertVal != 1 AND TaskDate='" + GetDate + "' and MachineNo = " + MachineNoList.Dequeue();

                            SqlDataReader reader;
                            reader = cm.ExecuteReader();

                            while (reader.Read())
                            {
                                if (Convert.ToInt16(reader["Capacity"]) != 0)
                                {
                                    Buyer = Buyer == "" ? Convert.ToString(reader["BuyerName"]) : Buyer + ";" + Convert.ToString(reader["BuyerName"]);
                                    Style = Style == "" ? Convert.ToString(reader["StyleName"]) : Style + ";" + Convert.ToString(reader["StyleName"]);
                                    Po = Po == "" ? Convert.ToString(reader["PurchaseOrderNo"]) : Po + ";" + Convert.ToString(reader["PurchaseOrderNo"]);
                                    Parts = Parts == "" ? Convert.ToString(reader["PartName"]) : Parts + ";" + Convert.ToString(reader["PartName"]);
                                    Size = Size == "" ? Convert.ToString(reader["SizeName"]) : Size + ";" + Convert.ToString(reader["SizeName"]);
                                    SAM = SAM == "" ? Convert.ToString(reader["SAM"]) : SAM + ";" + Convert.ToString(reader["SAM"]);
                                    Efficiency = Efficiency == "" ? Convert.ToString(reader["Efficiency"]) : Efficiency + ";" + Convert.ToString(reader["Efficiency"]);
                                    Capacity = Capacity == "" ? Convert.ToString(reader["Capacity"]) : Capacity + ";" + Convert.ToString(reader["Capacity"]);
                                    PlanQty = PlanQty == "" ? Convert.ToString(reader["PlanQty"]) : PlanQty + ";" + Convert.ToString(reader["PlanQty"]);
                                    DateTime SetTime = Convert.ToDateTime(reader["ShipmentDate"]);
                                    CHD = CHD == "" ? SetTime.ToString("dd/MM/yyyy") : CHD + ";" + SetTime.ToString("dd/MM/yyyy");
                                    OrderQty = OrderQty == "" ? Convert.ToString(reader["OrderQty"]) : OrderQty + ";" + Convert.ToString(reader["OrderQty"]);
                                    ActualQty = reader.IsDBNull(reader.GetOrdinal("ActualQty")) == true ? "0" : ActualQty == "" ? Convert.ToString(reader["ActualQty"]) : ActualQty + "," + Convert.ToString(reader["ActualQty"]);
                                    OrderIDList = OrderIDList == "" ? Convert.ToString(reader["OrderID"]) : OrderIDList + "," + Convert.ToString(reader["OrderID"]);

                                    DailyPlanQty = DailyPlanQty + Convert.ToInt32(reader["PlanQty"]);
                                }
                            }

                            if (Buyer != "")
                            {
                                PlanIndex.Add(new KeyValuePair<int, int>(x, y));
                            }

                            planBoardDataGridView.Rows[x].Cells[y].Value = Buyer;
                            x++;
                            planBoardDataGridView.Rows[x].Cells[y].Value = Style;
                            x++;
                            planBoardDataGridView.Rows[x].Cells[y].Value = Po;
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
                    planBoardDataGridView.Rows[0].Cells[y].Value = DailyPlanQty;

                    planBoardDataGridView.Rows[2].Frozen = true;
                    planBoardDataGridView.Columns[2].Frozen = true;
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
                
                planBoardDataGridView.Enabled = true;
                BtnGeneratePlan.Enabled = true;
                BtnAddPlan.Enabled = true;
                Search.Enabled = true;
                Revert.Enabled = true;
                MStatuscomboBox.Enabled = true;
                MachineComboBox.Enabled = true;
                this.Invoke((MethodInvoker)delegate { pinwheel.Visible = false; });
                
            }
        }

        private void SetRowIndex()
        {
            for (int i = 0; i < MachineDiaList.Count; i++)
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
                            if (MachineRowIndex == -1)
                            {
                                row.Cells.Add(new DataGridViewTextBoxCell { Value = planBoardDataGridView.Rows[i - 1].Cells[0].Value.ToString() });
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
            //if (!CreateRestorePint())
            //{
            //    MessageBox.Show("Connection Error!!! Please Try Again!!!");
            //    planBoardDataGridView.Rows.Clear();
            //    return;
            //}

            planBoardDataGridView.Rows.Clear();
            planBoardDataGridView.Columns.Clear();
            gridMachineRowStartIndex.Clear();

            if (MachineList.Count < 2)
            {
                planBoardDataGridView.Rows.Clear();
                return;
            }

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

            LoadMachineOnPlanBoard();
            fromDate = fromDateTimePicker.Value;
            toDate = toDateTimePicker.Value;
            GridInitialize();

            ThreadStart myThreadStart = new ThreadStart(Generate_Plan_Board);
            Thread myThread = new Thread(myThreadStart);
            myThread.Start(); 
            //Generate_Plan_Board();
        }

        private Boolean CreateRestorePint()
        {
            //string connectionStr = ConnectionManager.connectionStringSavePoint;
            //SqlCommand cm = new SqlCommand();
            //SqlConnection cn = new SqlConnection(connectionStr);
            //cm.Connection = cn;
            //cn.Open();

            //try
            //{
            //    string query = "BEGIN TRAN SELECT * FROM PlanTable SAVE TRAN SAVEPLANTABLE";
            //    cm.CommandText = query;
            //    SqlDataReader reader = cm.ExecuteReader();
            //    return reader.HasRows == true ? true : false;
            //}

            //catch (Exception e)
            //{
            //    MessageBox.Show("" + e.ToString());
            //    return false;
            //}

            //finally
            //{
            //    cn.Close();
            //}
            return true;
        }

        private Boolean ReleaseRestorePoint()
        {
            //string connectionStr = ConnectionManager.connectionStringSavePoint;
            //SqlCommand cm = new SqlCommand();
            //SqlConnection cn = new SqlConnection(connectionStr);
            //cm.Connection = cn;
            //cn.Open();
            //try
            //{
            //    string query = "RELEASE SAVEPOINT SAVEPLANTABLE";
            //    cm.CommandText = query;
            //    return cm.ExecuteNonQuery() > 0 ? true : false;
            //}

            //catch (Exception e)
            //{
            //    MessageBox.Show("" + e.ToString());
            //    return false;
            //}

            //finally
            //{
            //    cn.Close();
            //}
            return true;
        }

        private void LoadWaiting()
        {
            // some work takes 5 sec
            Thread.Sleep(1000);
        }

        private void BtnAddPlan_Click(object sender, EventArgs e)
        {
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
            if (orderID > 0)
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
            ThreadStart myThreadStart = new ThreadStart(Generate_Plan_Board);
            Thread myThread = new Thread(myThreadStart);
            myThread.Start(); 
            //Generate_Plan_Board();
        }

        private void resetPlanInfo()
        {
            orderID = 0;
            planDate = "";
            planStartDate = DateTime.Now.Date;
            planEndDate = DateTime.Now.Date;
        }

        internal static void sendPlanValue(int orderId, string styleName, string sizeName, string diaName, string partName, int mcNo, DateTime plnDate, DateTime plnStartDate, DateTime plnEndDate, string remarksText, int Qty, double efficiency, int McCapacity, string chd, int plnQty)
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
            orderQuantity = Qty.ToString();
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
            int diaNo = Convert.ToInt32(planBoardDataGridView.Rows[rowIndex].Cells[1].Value);
            FBPlanBoardForm fbplanform = new FBPlanBoardForm(false, mcNo, diaNo, taskDate, orderIDs, false);
            fbplanform.ShowDialog();

            if (FBPlanBoardForm.ChangeFlag)
            {
                ThreadStart myThreadStart = new ThreadStart(Generate_Plan_Board);
                Thread myThread = new Thread(myThreadStart);
                myThread.Start(); 
            }
            //Generate_Plan_Board();
        }

        private void backwardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime taskDate = DateTime.ParseExact(planBoardDataGridView.Rows[1].Cells[colIndex].Value.ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            orderIDs = planBoardDataGridView.Rows[rowIndex + OrderFactorsCount - 1].Cells[colIndex].Value.ToString();
            int mcNo = Convert.ToInt32(planBoardDataGridView.Rows[rowIndex].Cells[0].Value);
            int diaNo = Convert.ToInt32(planBoardDataGridView.Rows[rowIndex].Cells[1].Value);
            FBPlanBoardForm fbplanform = new FBPlanBoardForm(true, mcNo, diaNo, taskDate, orderIDs, false);
            fbplanform.ShowDialog();

            if (FBPlanBoardForm.ChangeFlag)
            {
                ThreadStart myThreadStart = new ThreadStart(Generate_Plan_Board);
                Thread myThread = new Thread(myThreadStart);
                myThread.Start(); 
            }
            //Generate_Plan_Board();
        }

        private void planBoardDataGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex % OrderFactorsCount == 3 && e.ColumnIndex > 2 && (planBoardDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != null && planBoardDataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "") && e.Button == MouseButtons.Right)
            {
                Boolean result = true;
                rowIndex = e.RowIndex;
                colIndex = e.ColumnIndex;

                DateTime CurrentPlanDate = DateTime.ParseExact(planBoardDataGridView.Rows[1].Cells[colIndex].Value.ToString(), "dd/MM/yyyy", null);
                string ActualQty = planBoardDataGridView.Rows[rowIndex + OrderFactorsCount - 2].Cells[colIndex].Value.ToString();
                int mcNo = Convert.ToInt32(planBoardDataGridView.Rows[rowIndex].Cells[0].Value);

                if (CurrentPlanDate <= DateTime.Now.Date)
                {
                    this.pBContextMenuStrip.Items[4].Visible = true;
                }
                else
                {
                    this.pBContextMenuStrip.Items[4].Visible = false;
                }

                CurrentPlanDate = CurrentPlanDate.AddDays(1);
                while (!CommonFunctions.recordExist("SELECT * FROM WorkingDays WHERE MachineNo = " + mcNo + " AND Active = 1 AND WorkDate = '" + CurrentPlanDate + "'"))
                {
                    CurrentPlanDate = CurrentPlanDate.AddDays(1);
                }

                if (CurrentPlanDate > DateTime.Now.Date)
                {
                    this.pBContextMenuStrip.Items[0].Enabled = true;
                    this.pBContextMenuStrip.Items[1].Enabled = true;
                    this.pBContextMenuStrip.Items[2].Enabled = true;
                    this.pBContextMenuStrip.Items[3].Enabled = true;

                    var tempActualQty = ActualQty.Split(',');
                    result = Array.Exists(tempActualQty, element => Convert.ToUInt32(element) > 0);

                    if (result)
                    {
                        this.pBContextMenuStrip.Items[0].Visible = false;
                        this.pBContextMenuStrip.Items[1].Visible = false;
                        this.pBContextMenuStrip.Items[2].Visible = false;
                    }
                    else
                    {
                        this.pBContextMenuStrip.Items[0].Visible = true;
                        this.pBContextMenuStrip.Items[1].Visible = true;
                        this.pBContextMenuStrip.Items[2].Visible = true;
                    }
                }
                else if (CurrentPlanDate == DateTime.Now.Date)
                {
                    this.pBContextMenuStrip.Items[0].Visible = false;
                    this.pBContextMenuStrip.Items[1].Visible = false;
                    this.pBContextMenuStrip.Items[2].Visible = false;
                }
                else
                {
                    this.pBContextMenuStrip.Items[0].Enabled = false;
                    this.pBContextMenuStrip.Items[1].Enabled = false;
                    this.pBContextMenuStrip.Items[2].Enabled = false;
                    this.pBContextMenuStrip.Items[3].Enabled = false;
                }

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

                    if (DateTime.ParseExact(currentDate, "dd/MM/yyyy", null) >= DateTime.Now.Date)
                    {
                        EditMode = true;
                        ViewOrderInfo viewOrderInfo = new ViewOrderInfo(McNo, currentDate);
                        viewOrderInfo.ShowDialog();
                        ThreadStart myThreadStart = new ThreadStart(Generate_Plan_Board);
                        Thread myThread = new Thread(myThreadStart);
                        myThread.Start(); 
                    }
                    //Generate_Plan_Board();
                }
            }
        }

        private void planBoardDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > 2 && e.ColumnIndex > 2)
            {
                string buyer = "";
                string style = "";
                string po = "";
                string parts = "";
                string size = "";

                rowIndex = e.RowIndex;
                colIndex = e.ColumnIndex;

                if (planBoardDataGridView.Rows[e.RowIndex].Cells[colIndex].Value != null && planBoardDataGridView.Rows[e.RowIndex].Cells[colIndex].Value.ToString() != "")
                {
                    //planBoardDataGridView.Rows[e.RowIndex].Cells[colIndex].Style.BackColor != Color.SlateBlue && planBoardDataGridView.Rows[e.RowIndex].Cells[colIndex].Style.BackColor != Color.Thistle
                    if (rowIndex % OrderFactorsCount < 8)
                    {
                        //if (planBoardDataGridView.Rows[e.RowIndex].Cells[colIndex].Style.BackColor != Color.SlateBlue && planBoardDataGridView.Rows[e.RowIndex].Cells[colIndex].Style.BackColor != Color.Thistle)
                        //{
                            ResetPlanBoardColor();
                            if (e.RowIndex % OrderFactorsCount == 3)
                            {
                                buyer = planBoardDataGridView.Rows[e.RowIndex - 0].Cells[colIndex].Value.ToString();
                                if (buyer.Contains(";"))
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
                                po = planBoardDataGridView.Rows[e.RowIndex - 0].Cells[colIndex].Value.ToString();
                                if (po.Contains(";"))
                                    po = po.Split(';')[0].Trim();
                            }
                            else if (e.RowIndex % OrderFactorsCount == 6)
                            {
                                buyer = planBoardDataGridView.Rows[e.RowIndex - 3].Cells[colIndex].Value.ToString();
                                if (buyer.Contains(";"))
                                    buyer = buyer.Split(';')[0].Trim();
                                style = planBoardDataGridView.Rows[e.RowIndex - 2].Cells[colIndex].Value.ToString();
                                if (style.Contains(";"))
                                    style = style.Split(';')[0].Trim();
                                po = planBoardDataGridView.Rows[e.RowIndex - 1].Cells[colIndex].Value.ToString();
                                if (po.Contains(";"))
                                    po = po.Split(';')[0].Trim();
                                parts = planBoardDataGridView.Rows[e.RowIndex - 0].Cells[colIndex].Value.ToString();
                                if (parts.Contains(";"))
                                    parts = parts.Split(';')[0].Trim();
                            }
                            else if (e.RowIndex % OrderFactorsCount == 7)
                            {
                                buyer = planBoardDataGridView.Rows[e.RowIndex - 4].Cells[colIndex].Value.ToString();
                                if (buyer.Contains(";"))
                                    buyer = buyer.Split(';')[0].Trim();
                                style = planBoardDataGridView.Rows[e.RowIndex - 3].Cells[colIndex].Value.ToString();
                                if (style.Contains(";"))
                                    style = style.Split(';')[0].Trim();
                                po = planBoardDataGridView.Rows[e.RowIndex - 2].Cells[colIndex].Value.ToString();
                                if (po.Contains(";"))
                                    po = po.Split(';')[0].Trim();
                                parts = planBoardDataGridView.Rows[e.RowIndex - 1].Cells[colIndex].Value.ToString();
                                if (parts.Contains(";"))
                                    parts = parts.Split(';')[0].Trim();
                                size = planBoardDataGridView.Rows[e.RowIndex - 0].Cells[colIndex].Value.ToString();
                                if (size.Contains(";"))
                                    size = size.Split(';')[0].Trim();
                            }

                            for (int i = 3; i < planBoardDataGridView.ColumnCount; i++)
                            {
                                for (int j = 3; j < planBoardDataGridView.Rows.Count; j = j + OrderFactorsCount)
                                {
                                    string buyerCell = planBoardDataGridView.Rows[j].Cells[i].Value.ToString();
                                    string styleCell = planBoardDataGridView.Rows[j + 1].Cells[i].Value.ToString();
                                    string poCell = planBoardDataGridView.Rows[j + 2].Cells[i].Value.ToString();
                                    string partCell = planBoardDataGridView.Rows[j + 3].Cells[i].Value.ToString();
                                    string sizeCell = planBoardDataGridView.Rows[j + 4].Cells[i].Value.ToString();

                                    if (planBoardDataGridView.Rows[j].Cells[i].Value != null)
                                    {
                                        if (e.RowIndex % OrderFactorsCount == 3)
                                        {
                                            if (buyerCell.Contains(buyer))
                                            {
                                                if (buyerCell == buyer)
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
                                            if (buyerCell.Contains(buyer) && styleCell.Contains(style))
                                            {
                                                if (buyerCell == buyer && styleCell == style)
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
                                            if (buyerCell.Contains(buyer) && styleCell.Contains(style) && poCell.Contains(po))
                                            {
                                                if (buyerCell == buyer && styleCell == style && poCell == po)
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
                                            if (buyerCell.Contains(buyer) && styleCell.Contains(style) && poCell.Contains(po) && partCell.Contains(parts))
                                            {
                                                if (buyerCell == buyer && styleCell == style && poCell == po && partCell == parts)
                                                {
                                                    ColorPlanBoard(j, i, Color.SlateBlue);
                                                }
                                                else
                                                {
                                                    ColorPlanBoard(j, i, Color.Thistle);
                                                }
                                            }
                                        }
                                        if (e.RowIndex % OrderFactorsCount == 7)
                                        {
                                            if (buyerCell.Contains(buyer) && styleCell.Contains(style) && poCell.Contains(po) && partCell.Contains(parts) && sizeCell.Contains(size))
                                            {
                                                if (buyerCell == buyer && styleCell == style && poCell == po && partCell == parts && sizeCell == size)
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
                        else
                        {
                            if (planBoardDataGridView.Rows[rowIndex].Cells[colIndex].Style.BackColor == Color.SlateBlue || planBoardDataGridView.Rows[rowIndex].Cells[colIndex].Style.BackColor == Color.Thistle)
                            {
                                SetPlanBoradDefaultColor();
                            }
                        }
                }
                else
                {
                    SetPlanBoradDefaultColor();
                }
            }
            else
            {
                SetPlanBoradDefaultColor();
            }
        }

        private void SetPlanBoradDefaultColor()
        {
            foreach (KeyValuePair<int, int> item in PlanIndex)
            {
                var capVal = planBoardDataGridView.Rows[item.Key + 7].Cells[item.Value].Value.ToString();
                var qtyVal = planBoardDataGridView.Rows[item.Key + 8].Cells[item.Value].Value.ToString();
                int capacity = capVal.Contains(';') == true ? Convert.ToInt32(capVal.Split(';')[capVal.Split(';').Length - 1].Trim()) : Convert.ToInt32(capVal);
                int plnQty = qtyVal.Contains(';') == true ? qtyVal.Split(new[] { ';' }).Select(x => int.Parse(x.Trim())).Sum() : Convert.ToInt32(qtyVal);
                if (plnQty == capacity)
                {
                    ColorPlanBoard(item.Key, item.Value, Color.DarkOliveGreen);
                }
                else
                {
                    ColorPlanBoard(item.Key, item.Value, Color.ForestGreen);
                }
            }
        }

        private void ResetPlanBoardColor()
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
                            ColorPlanBoard(j, i, Color.IndianRed);
                        }
                        else
                        {
                            ColorPlanBoard(j, i, Color.DarkGray);
                        }
                    }
                    else
                    {
                        var capVal = planBoardDataGridView.Rows[j + 7].Cells[i].Value.ToString();
                        var qtyVal = planBoardDataGridView.Rows[j + 8].Cells[i].Value.ToString();
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

        private void ResetsPlanBoardColor()
        {
            //for (int i = 3; i < planBoardDataGridView.ColumnCount; i++)
            //{
            //    for (int j = 3; j < planBoardDataGridView.Rows.Count; j++)
            //    {
            //        if (planBoardDataGridView.Rows[3].Cells[i].Value.ToString() == "" && planBoardDataGridView.Rows[3].Cells[i].Value.ToString() == null)
            //        {
            //            planBoardDataGridView.Rows[j].Cells[i].Style.BackColor = Color.White;
            //        }
            //    }
            //}
            //PlanBoardColorManagement();
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
            int diaNo = Convert.ToInt32(planBoardDataGridView.Rows[rowIndex].Cells[1].Value);
            FBPlanBoardForm fbplanform = new FBPlanBoardForm(true, mcNo, diaNo, taskDate, orderIDs, true);
            fbplanform.ShowDialog();

            if (FBPlanBoardForm.ChangeFlag)
            {
                ThreadStart myThreadStart = new ThreadStart(Generate_Plan_Board);
                Thread myThread = new Thread(myThreadStart);
                myThread.Start();
            } 
            //Generate_Plan_Board();
        }

        private void MStatuscomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMachineStatusWise();
        }

        private void updateActualQtyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime taskDate = DateTime.ParseExact(planBoardDataGridView.Rows[1].Cells[colIndex].Value.ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            orderIDs = planBoardDataGridView.Rows[rowIndex + OrderFactorsCount - 1].Cells[colIndex].Value.ToString();
            int mcNo = Convert.ToInt32(planBoardDataGridView.Rows[rowIndex].Cells[0].Value);
            UpdateActualQtyForm updateActualQtyForm = new UpdateActualQtyForm(orderIDs, mcNo, taskDate, false);
            updateActualQtyForm.ShowDialog();

            if (UpdateActualQtyForm.ChangeFlag)
            {
                ThreadStart myThreadStart = new ThreadStart(Generate_Plan_Board);
                Thread myThread = new Thread(myThreadStart);
                myThread.Start();
            }
            //Generate_Plan_Board();
        }

        private void Search_Click(object sender, EventArgs e) 
        {
            Report reportForm = new Report();
            reportForm.ShowDialog();
        }

        private void Revert_Click(object sender, EventArgs e)
        {
            string connectionStr = ConnectionManager.connectionString; string query = "";
            SqlCommand cm = new SqlCommand(); SqlConnection cn = new SqlConnection(connectionStr); cm.Connection = cn; cn.Open();
            SqlCommand cm1 = new SqlCommand(); SqlConnection cn1 = new SqlConnection(connectionStr); cm1.Connection = cn1; cn1.Open();

            string cols = "Id, MachineNo, TaskDate, OrderID, Capacity, PlanQty, RemainingQty, ActualQty, OrderQty, Efficiency, SAM, Minute, Status, RevertVal, Production";

            try
            {
                if (CommonFunctions.recordExist("SELECT Top 1* FROM TempPlanTable"))
                {
                    query = "DELETE FROM PlanTable WHERE TaskDate >= (SELECT MIN(TaskDate) FROM TempPlanTable)";
                    cm.CommandText = query;
                    if (cm.ExecuteNonQuery() > 0)
                    {
                        query = "SET IDENTITY_INSERT PlanTable ON INSERT INTO PlanTable (" + cols + ") SELECT " + cols + " FROM TempPlanTable";
                        cm1.CommandText = query;
                        if (cm1.ExecuteNonQuery() > 0)
                        {
                            BtnGeneratePlan.PerformClick();
                        }
                        else
                        {
                            MessageBox.Show("Can not Revert. Please Try Again!!!");
                        }
                    }
                }   
            }

            catch (Exception ex)
            {
                MessageBox.Show("" + ex.ToString());
            }

            finally
            {
                cn.Close();
                cn1.Close();
            }
        }

        public static Boolean SaveState()
        {
            string connectionStr = ConnectionManager.connectionString; string query = ""; Boolean result = false;
            SqlCommand cm = new SqlCommand(); SqlConnection cn = new SqlConnection(connectionStr); cm.Connection = cn; cn.Open();
            SqlCommand cm1 = new SqlCommand(); SqlConnection cn1 = new SqlConnection(connectionStr); cm1.Connection = cn1; cn1.Open();

            try
            {
                query = "DELETE FROM TempPlanTable";
                cm.CommandText = query; cm.ExecuteNonQuery();
                query = "INSERT INTO TempPlanTable SELECT * FROM PlanTable WHERE TaskDate BETWEEN '" + fromDate + "' AND '" + toDate + "'";
                cm1.CommandText = query;
                result = cm1.ExecuteNonQuery() > 0 ? true : false;
                return result;
            }
            catch (Exception ee)
            {
                MessageBox.Show("" + ee.ToString());
                return false;
            }
            finally
            {
                cn.Close();
                cn1.Close();
            }
        }

        private void PlanBoardDisplayForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (ReleaseRestorePoint())
            //{
            //    MessageBox.Show("Connection Error!!! Please Try Again!!!");
            //    e.Cancel = false;
            //    return;
            //}
            //else
            //{
            //    e.Cancel = true;
            //}
        }

        private void noProductionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime taskDate = DateTime.ParseExact(planBoardDataGridView.Rows[1].Cells[colIndex].Value.ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            orderIDs = planBoardDataGridView.Rows[rowIndex + OrderFactorsCount - 1].Cells[colIndex].Value.ToString();
            int mcNo = Convert.ToInt32(planBoardDataGridView.Rows[rowIndex].Cells[0].Value);
            UpdateActualQtyForm updateActualQtyForm = new UpdateActualQtyForm(orderIDs, mcNo, taskDate, true);
            updateActualQtyForm.ShowDialog();

            if (UpdateActualQtyForm.ChangeFlag)
            {
                ThreadStart myThreadStart = new ThreadStart(Generate_Plan_Board);
                Thread myThread = new Thread(myThreadStart);
                myThread.Start();
            }
        }
    }

    public static class ExtensionMethods
    {
        public static void DoubleBuffered(this DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }
    }
}
