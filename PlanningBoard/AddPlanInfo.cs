using System;
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
using System.Runtime.InteropServices;

namespace PlanningBoard
{
    public partial class ViewOrderInfo : Form
    {
        ArrayList mcNo = new ArrayList();
        public Dictionary<int, string> MachineList = new Dictionary<int, string>();
        public Dictionary<int, string> BuyerList = new Dictionary<int, string>();
        public Dictionary<int, string> StyleList = new Dictionary<int, string>();
        public Dictionary<int, string> DiaList = new Dictionary<int, string>();
        public Dictionary<int, string> SizeList = new Dictionary<int, string>();
        public Dictionary<int, string> PartList = new Dictionary<int, string>();
        public Dictionary<int, int> EffList = new Dictionary<int, int>();

        DateTimePicker TaskDateDTP = new DateTimePicker();
        Rectangle rectangle;

        public List<int> StyleStore = new List<int>();
        public List<int> SizeStore = new List<int>();
        public List<int> PartStore = new List<int>();
        public List<int> SamStore = new List<int>();
        public List<int> EffStore = new List<int>();
        public List<int> PlnQtyStore = new List<int>();
        public List<int> OrderQtyStore = new List<int>();
        public List<int> IDs = new List<int>();

        public int PrePlanQty = 0;
        public int LeftQty = 0;
        public int AvgCapacity = 0;
        public int totalMinute = 0;
        public int daydiff = 0;
        public int PreVal = -1;
        public int NewVal = -1;
        public string PreDate = "";
        public string NewDate = "";
        public int PreIndex = -1;
        public Boolean LCFlag = false;
        public Boolean AutoFlag = false;
        public int MachineNo = -1;
        public DateTime shipDate = DateTime.Now;

        public int LC1 = 0;
        public int LC2 = 0;
        public int LC3 = 0;
        public int LC4 = 0;
        public int LC5 = 0;
        public int LC6 = 0;
        public int LC7 = 0;
        public int LC8 = 0;
        public int LC9 = 0;
        public int LC10 = 0;

        public int[] LcArray = null;
        public int LcCount = int.MaxValue;
        public int LcLastIndex = 0;

        public static int orderID = 0;
        public int rowIndex = -1;
        public int OrderWisePlanGridRowIndex = -1;
        public DateTime CurrentTaskDate = DateTime.Now;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd,int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public ViewOrderInfo(int McNo = -1, string Date = "")
        {
            InitializeComponent();
            planDateTimePicker.Value = DateTime.Now.Date;
            fromDateTimePicker.Value = DateTime.Now.Date;
            toDateTimePicker.Value = DateTime.Now.Date;
            hiddenTextBox.Text = "";
            LCFlag = false;
            AutoFlag = false;
            orderID = 0;
            labelAlert.Text = "This Order has Already Been Used!!!";
            MachineNo = McNo;
            EffList.Clear();
            MachineComboBox.CheckStateChanged += new System.EventHandler(MachineComboBox_CheckStateChanged);
            if (PlanBoardDisplayForm.EditMode)
            {
                CurrentTaskDate = DateTime.ParseExact(Date, "dd/MM/yyyy", null);
            }   
        }

        private void ViewOrderInfo_Load(object sender, EventArgs e)
        {
            LoadComboBox();
            LoadDatePicker();
            if (PlanBoardDisplayForm.EditMode)
            {
                UpdateButton.Visible = true;
                AddPlanButton.Enabled = false;
                AutoPlanButton.Enabled = false;
                resetButton.Enabled = false;
                ApplyEffButton.Enabled = false;
                LCText.Enabled = false;
                orderWisePlandataGridView.Columns[2].ReadOnly = false;

                orderWisePlandataGridView.Controls.Add(TaskDateDTP);
                TaskDateDTP.Visible = false;
                TaskDateDTP.Format = DateTimePickerFormat.Custom;
                TaskDateDTP.CustomFormat = "dd/MM/yyyy";
                //TaskDateDTP.TextChanged += new EventHandler(TaskDateDTP_TextChange);

                LoadOrderInfoGridOnEditMode();
            }
            else
            {
                AddPlanButton.Enabled = true;
                AutoPlanButton.Enabled = true;
                resetButton.Enabled = true;
                ApplyEffButton.Enabled = true;
                LCText.Enabled = true;

                LoadOrderInfoGrid();
            }
            
        }

        private void TaskDateDTP_TextChange(object sender, EventArgs e)
        {
            //orderWisePlandataGridView.CurrentCell.Value = CurrentTaskDate.ToString("dd/MM/yyyy");
            //orderWisePlandataGridView.CurrentCell.Value = TaskDateDTP.Text.ToString();
        }

        private void LoadDatePicker()
        {
            fromDateTimePicker.Value = DateTime.Now.Date;
            toDateTimePicker.Value = DateTime.Now.Date;
            if (PlanBoardDisplayForm.EditMode)
            {
                fromDateTimePicker.Enabled = false;
                toDateTimePicker.Enabled = false;
            }
        }

        private void ViewOrderInfo_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void LoadOrderInfoGridOnEditMode()
        {
            try
            {
                int SL = 1;
                int S1 = 1; string S2 = ""; string S3 = ""; string S4 = ""; string S5 = ""; string S6 = ""; string S7 = ""; string S8 = ""; string S9 = ""; string S10 = ""; string S11 = ""; string S12 = "";
                string query = "SELECT Id,  FROM Order_Info WHERE Id IN (" + PlanBoardDisplayForm.orderIDs + ")";
                query = " SELECT a.Id, a.PurchaseOrderNo, a.Quantity, a.ShipmentDate, a.SAM, a.Efficiency, a.Status, b.BuyerName, c.StyleName, d.SizeName, e.Dia, f.PartName FROM (SELECT * FROM Order_Info WHERE Id IN (" + PlanBoardDisplayForm.orderIDs + ")) a, Buyer b, Style c, Size d, Dia e, BodyPart f " +
                    "WHERE a.Buyer = b.Id AND a.Style = c.Id AND a.Size = d.Id AND a.Dia = e.Id AND a.BodyPart = f.Id order by a.ShipmentDate asc";
                SqlDataReader reader = CommonFunctions.GetFromDB(query);
                if (reader.HasRows)
                {
                    orderInfoDetailsdataGridView.Rows.Clear();
                    while (reader.Read())
                    {
                        S1 = SL;
                        IDs.Add((int)reader["Id"]);
                        S2 = reader.IsDBNull(reader.GetOrdinal("BuyerName")) == true ? "Not Defined" : reader["BuyerName"].ToString();
                        S3 = reader.IsDBNull(reader.GetOrdinal("StyleName")) == true ? "Not Defined" : reader["StyleName"].ToString();
                        S4 = reader.IsDBNull(reader.GetOrdinal("SizeName")) == true ? "Not Defined" : reader["SizeName"].ToString();
                        S5 = reader.IsDBNull(reader.GetOrdinal("Dia")) == true ? "Not Defined" : reader["Dia"].ToString();
                        S6 = reader.IsDBNull(reader.GetOrdinal("PartName")) == true ? "Not Defined" : reader["PartName"].ToString();
                        S7 = reader.IsDBNull(reader.GetOrdinal("PurchaseOrderNo")) == true ? "" : Convert.ToString(reader["PurchaseOrderNo"]);
                        S8 = reader.IsDBNull(reader.GetOrdinal("Quantity")) == true ? 0000.ToString() : Convert.ToString(reader["Quantity"]);
                        S9 = reader.IsDBNull(reader.GetOrdinal("ShipmentDate")) == true ? "0/0/0000" : Convert.ToDateTime(reader["ShipmentDate"]).ToString("dd/MM/yyyy");
                        S10 = reader.IsDBNull(reader.GetOrdinal("SAM")) == true ? 0.00.ToString() : Convert.ToString(reader["SAM"]);
                        S11 = reader.IsDBNull(reader.GetOrdinal("Efficiency")) == true ? 0.00.ToString() : Convert.ToString(reader["Efficiency"]);
                        S12 = Enum.GetName(typeof(VariableDecleration_Class.Status), reader.IsDBNull(reader.GetOrdinal("Status")) == true ? (int)VariableDecleration_Class.Status.Pending : Convert.ToInt32(reader["Status"]));

                        orderInfoDetailsdataGridView.Rows.Add(S1, S2, S3, S4, S5, S6, S7, S8, S9, S10, S11, S12, (int)reader["Id"]);
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
                if (CommonFunctions.connection.State == ConnectionState.Open)
                {
                    CommonFunctions.connection.Close();
                }
            }
        }

        private void LoadOrderInfoGrid()
        {
            try
            {
                int SL = 1;
                int S1 = 1; string S2 = ""; string S3 = ""; string S4 = ""; string S5 = ""; string S6 = ""; string S7 = ""; string S8 = ""; string S9 = ""; string S10 = ""; string S11 = ""; string S12 = "";

                if (hiddenTextBox.Text != "")
                {
                    return;
                }

                if (buyerComboBox.SelectedIndex == -1 || styleComboBox.SelectedIndex == -1 || sizeComboBox.SelectedIndex == -1 || diaComboBox.SelectedIndex == -1 || partComboBox.SelectedIndex == -1)
                {
                    return;
                }

                if (buyerComboBox.SelectedIndex == 0 && styleComboBox.SelectedIndex == 0 && sizeComboBox.SelectedIndex == 0 && diaComboBox.SelectedIndex == 0 && partComboBox.SelectedIndex == 0)
                {
                    orderInfoDetailsdataGridView.Rows.Clear();
                    return;
                }

                int buyerName = ((KeyValuePair<int, string>)buyerComboBox.SelectedItem).Key;
                int styleName = ((KeyValuePair<int, string>)styleComboBox.SelectedItem).Key;
                int sizeNo = ((KeyValuePair<int, string>)sizeComboBox.SelectedItem).Key;
                int dia = ((KeyValuePair<int, string>)diaComboBox.SelectedItem).Key;
                int bodyPart = ((KeyValuePair<int, string>)partComboBox.SelectedItem).Key;

                string query = "";

                if (buyerName > 0 && styleName == 0 && sizeNo == 0 && dia == 0 && bodyPart == 0)
                {
                    query = " SELECT a.Id, a.PurchaseOrderNo, a.Quantity, a.ShipmentDate, a.SAM, a.Efficiency, a.Status, b.BuyerName, c.StyleName, d.SizeName, e.Dia, f.PartName FROM (SELECT * FROM Order_Info WHERE Buyer = " + buyerName + ") a, Buyer b, Style c, Size d, Dia e, BodyPart f " +
                    "WHERE a.Buyer = b.Id AND a.Style = c.Id AND a.Size = d.Id AND a.Dia = e.Id AND a.BodyPart = f.Id";
                }
                if (styleName > 0 && buyerName == 0 && sizeNo == 0 && dia == 0 && bodyPart == 0)
                {
                    query = " SELECT a.Id, a.PurchaseOrderNo, a.Quantity, a.ShipmentDate, a.SAM, a.Efficiency, a.Status, b.BuyerName, c.StyleName, d.SizeName, e.Dia, f.PartName FROM (SELECT * FROM Order_Info WHERE Style = " + styleName + ") a, Buyer b, Style c, Size d, Dia e, BodyPart f " +
                    "WHERE a.Buyer = b.Id AND a.Style = c.Id AND a.Size = d.Id AND a.Dia = e.Id AND a.BodyPart = f.Id";
                }
                if (sizeNo > 0 && styleName == 0 && buyerName == 0 && dia == 0 && bodyPart == 0)
                {
                    query = " SELECT a.Id, a.PurchaseOrderNo, a.Quantity, a.ShipmentDate, a.SAM, a.Efficiency, a.Status, b.BuyerName, c.StyleName, d.SizeName, e.Dia, f.PartName FROM (SELECT * FROM Order_Info WHERE Size = " + sizeNo + ") a, Buyer b, Style c, Size d, Dia e, BodyPart f " +
                    "WHERE a.Buyer = b.Id AND a.Style = c.Id AND a.Size = d.Id AND a.Dia = e.Id AND a.BodyPart = f.Id";
                }
                if (dia > 0 && styleName == 0 && buyerName == 0 && sizeNo == 0 && bodyPart == 0)
                {
                    query = " SELECT a.Id, a.PurchaseOrderNo, a.Quantity, a.ShipmentDate, a.SAM, a.Efficiency, a.Status, b.BuyerName, c.StyleName, d.SizeName, e.Dia, f.PartName FROM (SELECT * FROM Order_Info WHERE Dia = " + dia + ") a, Buyer b, Style c, Size d, Dia e, BodyPart f " +
                    "WHERE a.Buyer = b.Id AND a.Style = c.Id AND a.Size = d.Id AND a.Dia = e.Id AND a.BodyPart = f.Id";
                }
                if (bodyPart > 0 && styleName == 0 && buyerName == 0 && sizeNo == 0 && dia == 0)
                {
                    query = " SELECT a.Id, a.PurchaseOrderNo, a.Quantity, a.ShipmentDate, a.SAM, a.Efficiency, a.Status, b.BuyerName, c.StyleName, d.SizeName, e.Dia, f.PartName FROM (SELECT * FROM Order_Info WHERE BodyPart = " + bodyPart + ") a, Buyer b, Style c, Size d, Dia e, BodyPart f " +
                    "WHERE a.Buyer = b.Id AND a.Style = c.Id AND a.Size = d.Id AND a.Dia = e.Id AND a.BodyPart = f.Id";
                }
                if (buyerName > 0 && styleName > 0 && sizeNo == 0 && dia == 0 && bodyPart == 0)
                {
                    query = " SELECT a.Id, a.PurchaseOrderNo, a.Quantity, a.ShipmentDate, a.SAM, a.Efficiency, a.Status, b.BuyerName, c.StyleName, d.SizeName, e.Dia, f.PartName FROM (SELECT * FROM Order_Info WHERE Buyer = " + buyerName + " AND Style = " + styleName + ") a, Buyer b, Style c, Size d, Dia e, BodyPart f " +
                    "WHERE a.Buyer = b.Id AND a.Style = c.Id AND a.Size = d.Id AND a.Dia = e.Id AND a.BodyPart = f.Id";
                }
                if (buyerName > 0 && styleName > 0 && sizeNo > 0 && dia == 0 && bodyPart == 0)
                {
                    query = " SELECT a.Id, a.PurchaseOrderNo, a.Quantity, a.ShipmentDate, a.SAM, a.Efficiency, a.Status, b.BuyerName, c.StyleName, d.SizeName, e.Dia, f.PartName FROM (SELECT * FROM Order_Info WHERE Buyer = " + buyerName + " AND Style = " + styleName + " AND Size = " + sizeNo + ") a, Buyer b, Style c, Size d, Dia e, BodyPart f " +
                    "WHERE a.Buyer = b.Id AND a.Style = c.Id AND a.Size = d.Id AND a.Dia = e.Id AND a.BodyPart = f.Id";
                }
                if (buyerName > 0 && styleName > 0 && sizeNo > 0 && dia > 0 && bodyPart == 0)
                {
                    query = " SELECT a.Id, a.PurchaseOrderNo, a.Quantity, a.ShipmentDate, a.SAM, a.Efficiency, a.Status, b.BuyerName, c.StyleName, d.SizeName, e.Dia, f.PartName FROM (SELECT * FROM Order_Info WHERE Buyer = " + buyerName + " AND Style = " + styleName + " AND Size = " + sizeNo + " AND Dia = " + dia + ") a, Buyer b, Style c, Size d, Dia e, BodyPart f " +
                    "WHERE a.Buyer = b.Id AND a.Style = c.Id AND a.Size = d.Id AND a.Dia = e.Id AND a.BodyPart = f.Id";
                }
                if (buyerName > 0 && sizeNo > 0 && styleName == 0 && dia == 0 && bodyPart == 0)
                {
                    query = " SELECT a.Id, a.PurchaseOrderNo, a.Quantity, a.ShipmentDate, a.SAM, a.Efficiency, a.Status, b.BuyerName, c.StyleName, d.SizeName, e.Dia, f.PartName FROM (SELECT * FROM Order_Info WHERE Buyer = " + buyerName + " AND Size = " + sizeNo + ") a, Buyer b, Style c, Size d, Dia e, BodyPart f " +
                    "WHERE a.Buyer = b.Id AND a.Style = c.Id AND a.Size = d.Id AND a.Dia = e.Id AND a.BodyPart = f.Id";
                }
                if (buyerName > 0 && dia > 0 && styleName == 0 && sizeNo == 0 && bodyPart == 0)
                {
                    query = " SELECT a.Id, a.PurchaseOrderNo, a.Quantity, a.ShipmentDate, a.SAM, a.Efficiency, a.Status, b.BuyerName, c.StyleName, d.SizeName, e.Dia, f.PartName FROM (SELECT * FROM Order_Info WHERE Buyer = " + buyerName + " AND Dia = " + dia + ") a, Buyer b, Style c, Size d, Dia e, BodyPart f " +
                    "WHERE a.Buyer = b.Id AND a.Style = c.Id AND a.Size = d.Id AND a.Dia = e.Id AND a.BodyPart = f.Id";
                }
                if (buyerName > 0 && bodyPart > 0 && styleName == 0 && sizeNo == 0 && dia == 0)
                {
                    query = " SELECT a.Id, a.PurchaseOrderNo, a.Quantity, a.ShipmentDate, a.SAM, a.Efficiency, a.Status, b.BuyerName, c.StyleName, d.SizeName, e.Dia, f.PartName FROM (SELECT * FROM Order_Info WHERE Buyer = " + buyerName + " AND BodyPart = " + bodyPart + ") a, Buyer b, Style c, Size d, Dia e, BodyPart f " +
                    "WHERE a.Buyer = b.Id AND a.Style = c.Id AND a.Size = d.Id AND a.Dia = e.Id AND a.BodyPart = f.Id";
                }
                if (buyerName > 0 && styleName > 0 && bodyPart > 0 && sizeNo == 0 && dia == 0)
                {
                    query = " SELECT a.Id, a.PurchaseOrderNo, a.Quantity, a.ShipmentDate, a.SAM, a.Efficiency, a.Status, b.BuyerName, c.StyleName, d.SizeName, e.Dia, f.PartName FROM (SELECT * FROM Order_Info WHERE Buyer = " + buyerName + " AND Style = " + styleName + " AND BodyPart = " + bodyPart + ") a, Buyer b, Style c, Size d, Dia e, BodyPart f " +
                    "WHERE a.Buyer = b.Id AND a.Style = c.Id AND a.Size = d.Id AND a.Dia = e.Id AND a.BodyPart = f.Id";
                }
                if (buyerName > 0 && sizeNo > 0 && bodyPart > 0 && styleName == 0 && dia == 0)
                {
                    query = " SELECT a.Id, a.PurchaseOrderNo, a.Quantity, a.ShipmentDate, a.SAM, a.Efficiency, a.Status, b.BuyerName, c.StyleName, d.SizeName, e.Dia, f.PartName FROM (SELECT * FROM Order_Info WHERE Buyer = " + buyerName + " AND Size = " + sizeNo + " AND BodyPart = " + bodyPart + ") a, Buyer b, Style c, Size d, Dia e, BodyPart f " +
                    "WHERE a.Buyer = b.Id AND a.Style = c.Id AND a.Size = d.Id AND a.Dia = e.Id AND a.BodyPart = f.Id";
                }
                if (buyerName > 0 && dia > 0 && bodyPart > 0 && styleName == 0 && sizeNo == 0)
                {
                    query = " SELECT a.Id, a.PurchaseOrderNo, a.Quantity, a.ShipmentDate, a.SAM, a.Efficiency, a.Status, b.BuyerName, c.StyleName, d.SizeName, e.Dia, f.PartName FROM (SELECT * FROM Order_Info WHERE Buyer = " + buyerName + " AND Dia = " + dia + " AND BodyPart = " + bodyPart + ") a, Buyer b, Style c, Size d, Dia e, BodyPart f " +
                    "WHERE a.Buyer = b.Id AND a.Style = c.Id AND a.Size = d.Id AND a.Dia = e.Id AND a.BodyPart = f.Id";
                }
                if (buyerName > 0 && styleName > 0 && dia > 0 && bodyPart == 0 && sizeNo == 0)
                {
                    query = " SELECT a.Id, a.PurchaseOrderNo, a.Quantity, a.ShipmentDate, a.SAM, a.Efficiency, a.Status, b.BuyerName, c.StyleName, d.SizeName, e.Dia, f.PartName FROM (SELECT * FROM Order_Info WHERE Buyer = " + buyerName + " AND Dia = " + dia + " AND Style = " + styleName + ") a, Buyer b, Style c, Size d, Dia e, BodyPart f " +
                    "WHERE a.Buyer = b.Id AND a.Style = c.Id AND a.Size = d.Id AND a.Dia = e.Id AND a.BodyPart = f.Id";
                }
                if (buyerName > 0 && sizeNo > 0 && dia > 0 && bodyPart == 0 && styleName == 0)
                {
                    query = " SELECT a.Id, a.PurchaseOrderNo, a.Quantity, a.ShipmentDate, a.SAM, a.Efficiency, a.Status, b.BuyerName, c.StyleName, d.SizeName, e.Dia, f.PartName FROM (SELECT * FROM Order_Info WHERE Buyer = " + buyerName + " AND Dia = " + dia + " AND Size = " + sizeNo + ") a, Buyer b, Style c, Size d, Dia e, BodyPart f " +
                    "WHERE a.Buyer = b.Id AND a.Style = c.Id AND a.Size = d.Id AND a.Dia = e.Id AND a.BodyPart = f.Id";
                }
                if (buyerName > 0 && styleName > 0 && dia > 0 && bodyPart > 0 && sizeNo == 0)
                {
                    query = " SELECT a.Id, a.PurchaseOrderNo, a.Quantity, a.ShipmentDate, a.SAM, a.Efficiency, a.Status, b.BuyerName, c.StyleName, d.SizeName, e.Dia, f.PartName FROM (SELECT * FROM Order_Info WHERE Buyer = " + buyerName + " AND Dia = " + dia + " AND Style = " + styleName + " AND BodyPart = " + bodyPart + ") a, Buyer b, Style c, Size d, Dia e, BodyPart f " +
                    "WHERE a.Buyer = b.Id AND a.Style = c.Id AND a.Size = d.Id AND a.Dia = e.Id AND a.BodyPart = f.Id";
                }
                if (buyerName > 0 && sizeNo > 0 && dia > 0 && bodyPart > 0 && styleName == 0)
                {
                    query = " SELECT a.Id, a.PurchaseOrderNo, a.Quantity, a.ShipmentDate, a.SAM, a.Efficiency, a.Status, b.BuyerName, c.StyleName, d.SizeName, e.Dia, f.PartName FROM (SELECT * FROM Order_Info WHERE Buyer = " + buyerName + " AND Dia = " + dia + " AND Size = " + sizeNo + " AND BodyPart = " + bodyPart + ") a, Buyer b, Style c, Size d, Dia e, BodyPart f " +
                    "WHERE a.Buyer = b.Id AND a.Style = c.Id AND a.Size = d.Id AND a.Dia = e.Id AND a.BodyPart = f.Id";
                }
                if (buyerName > 0 && styleName > 0 && sizeNo > 0 && bodyPart > 0 && dia == 0)
                {
                    query = " SELECT a.Id, a.PurchaseOrderNo, a.Quantity, a.ShipmentDate, a.SAM, a.Efficiency, a.Status, b.BuyerName, c.StyleName, d.SizeName, e.Dia, f.PartName FROM (SELECT * FROM Order_Info WHERE Buyer = " + buyerName + " AND Style = " + styleName + " AND Size = " + sizeNo + " AND BodyPart = " + bodyPart + ") a, Buyer b, Style c, Size d, Dia e, BodyPart f " +
                    "WHERE a.Buyer = b.Id AND a.Style = c.Id AND a.Size = d.Id AND a.Dia = e.Id AND a.BodyPart = f.Id";
                }
                if (styleName > 0 && sizeNo > 0 && bodyPart == 0 && buyerName == 0 && dia == 0)
                {
                    query = " SELECT a.Id, a.PurchaseOrderNo, a.Quantity, a.ShipmentDate, a.SAM, a.Efficiency, a.Status, b.BuyerName, c.StyleName, d.SizeName, e.Dia, f.PartName FROM (SELECT * FROM Order_Info WHERE Style = " + styleName + " AND Size = " + sizeNo + ") a, Buyer b, Style c, Size d, Dia e, BodyPart f " +
                    "WHERE a.Buyer = b.Id AND a.Style = c.Id AND a.Size = d.Id AND a.Dia = e.Id AND a.BodyPart = f.Id";
                }
                if (styleName > 0 && sizeNo > 0 && dia > 0 && bodyPart == 0 && buyerName == 0)
                {
                    query = " SELECT a.Id, a.PurchaseOrderNo, a.Quantity, a.ShipmentDate, a.SAM, a.Efficiency, a.Status, b.BuyerName, c.StyleName, d.SizeName, e.Dia, f.PartName FROM (SELECT * FROM Order_Info WHERE Style = " + styleName + " AND Size = " + sizeNo + " AND Dia = " + dia + ") a, Buyer b, Style c, Size d, Dia e, BodyPart f " +
                    "WHERE a.Buyer = b.Id AND a.Style = c.Id AND a.Size = d.Id AND a.Dia = e.Id AND a.BodyPart = f.Id";
                }
                if (styleName > 0 && sizeNo > 0 && dia > 0 && bodyPart > 0 && buyerName == 0)
                {
                    query = " SELECT a.Id, a.PurchaseOrderNo, a.Quantity, a.ShipmentDate, a.SAM, a.Efficiency, a.Status, b.BuyerName, c.StyleName, d.SizeName, e.Dia, f.PartName FROM (SELECT * FROM Order_Info WHERE Style = " + styleName + " AND Size = " + sizeNo + " AND Dia = " + dia + " AND BodyPart = " + bodyPart + ") a, Buyer b, Style c, Size d, Dia e, BodyPart f " +
                    "WHERE a.Buyer = b.Id AND a.Style = c.Id AND a.Size = d.Id AND a.Dia = e.Id AND a.BodyPart = f.Id";
                }
                if (styleName > 0 && dia > 0 && bodyPart == 0 && buyerName == 0 && sizeNo == 0)
                {
                    query = " SELECT a.Id, a.PurchaseOrderNo, a.Quantity, a.ShipmentDate, a.SAM, a.Efficiency, a.Status, b.BuyerName, c.StyleName, d.SizeName, e.Dia, f.PartName FROM (SELECT * FROM Order_Info WHERE Style = " + styleName + " AND Dia = " + dia + ") a, Buyer b, Style c, Size d, Dia e, BodyPart f " +
                    "WHERE a.Buyer = b.Id AND a.Style = c.Id AND a.Size = d.Id AND a.Dia = e.Id AND a.BodyPart = f.Id";
                }
                if (styleName > 0 && bodyPart > 0 && buyerName == 0 && sizeNo == 0 && dia == 0)
                {
                    query = " SELECT a.Id, a.PurchaseOrderNo, a.Quantity, a.ShipmentDate, a.SAM, a.Efficiency, a.Status, b.BuyerName, c.StyleName, d.SizeName, e.Dia, f.PartName FROM (SELECT * FROM Order_Info WHERE Style = " + styleName + " AND BodyPart = " + bodyPart + ") a, Buyer b, Style c, Size d, Dia e, BodyPart f " +
                    "WHERE a.Buyer = b.Id AND a.Style = c.Id AND a.Size = d.Id AND a.Dia = e.Id AND a.BodyPart = f.Id";
                }
                if (styleName > 0 && sizeNo > 0 && bodyPart > 0 && buyerName == 0 && dia == 0)
                {
                    query = " SELECT a.Id, a.PurchaseOrderNo, a.Quantity, a.ShipmentDate, a.SAM, a.Efficiency, a.Status, b.BuyerName, c.StyleName, d.SizeName, e.Dia, f.PartName FROM (SELECT * FROM Order_Info WHERE Buyer = " + buyerName + " AND Style = " + styleName + " AND Size = " + sizeNo + " AND BodyPart = " + bodyPart + ") a, Buyer b, Style c, Size d, Dia e, BodyPart f " +
                    "WHERE a.Buyer = b.Id AND a.Style = c.Id AND a.Size = d.Id AND a.Dia = e.Id AND a.BodyPart = f.Id";
                }
                if (styleName > 0 && dia > 0 && bodyPart > 0 && buyerName == 0 && sizeNo == 0)
                {
                    query = " SELECT a.Id, a.PurchaseOrderNo, a.Quantity, a.ShipmentDate, a.SAM, a.Efficiency, a.Status, b.BuyerName, c.StyleName, d.SizeName, e.Dia, f.PartName FROM (SELECT * FROM Order_Info WHERE Buyer = " + buyerName + " AND Style = " + styleName + " AND Dia = " + dia + " AND BodyPart = " + bodyPart + ") a, Buyer b, Style c, Size d, Dia e, BodyPart f " +
                    "WHERE a.Buyer = b.Id AND a.Style = c.Id AND a.Size = d.Id AND a.Dia = e.Id AND a.BodyPart = f.Id";
                }
                if (sizeNo > 0 && dia > 0 && bodyPart == 0 && buyerName == 0 && styleName == 0)
                {
                    query = " SELECT a.Id, a.PurchaseOrderNo, a.Quantity, a.ShipmentDate, a.SAM, a.Efficiency, a.Status, b.BuyerName, c.StyleName, d.SizeName, e.Dia, f.PartName FROM (SELECT * FROM Order_Info WHERE Size = " + sizeNo + " AND Dia = " + dia + ") a, Buyer b, Style c, Size d, Dia e, BodyPart f " +
                    "WHERE a.Buyer = b.Id AND a.Style = c.Id AND a.Size = d.Id AND a.Dia = e.Id AND a.BodyPart = f.Id";
                }
                if (sizeNo > 0 && dia > 0 && bodyPart > 0 && buyerName == 0 && styleName == 0)
                {
                    query = " SELECT a.Id, a.PurchaseOrderNo, a.Quantity, a.ShipmentDate, a.SAM, a.Efficiency, a.Status, b.BuyerName, c.StyleName, d.SizeName, e.Dia, f.PartName FROM (SELECT * FROM Order_Info WHERE Size = " + sizeNo + " AND Dia = " + dia + " AND BodyPart = " + bodyPart + ") a, Buyer b, Style c, Size d, Dia e, BodyPart f " +
                    "WHERE a.Buyer = b.Id AND a.Style = c.Id AND a.Size = d.Id AND a.Dia = e.Id AND a.BodyPart = f.Id";
                }
                if (sizeNo > 0 && bodyPart > 0 && buyerName == 0 && styleName == 0 && dia == 0)
                {
                    query = " SELECT a.Id, a.PurchaseOrderNo, a.Quantity, a.ShipmentDate, a.SAM, a.Efficiency, a.Status, b.BuyerName, c.StyleName, d.SizeName, e.Dia, f.PartName FROM (SELECT * FROM Order_Info WHERE Size = " + sizeNo + " AND BodyPart = " + bodyPart + ") a, Buyer b, Style c, Size d, Dia e, BodyPart f " +
                    "WHERE a.Buyer = b.Id AND a.Style = c.Id AND a.Size = d.Id AND a.Dia = e.Id AND a.BodyPart = f.Id";
                }
                if (dia > 0 && bodyPart > 0 && buyerName == 0 && styleName == 0 && sizeNo == 0)
                {
                    query = " SELECT a.Id, a.PurchaseOrderNo, a.Quantity, a.ShipmentDate, a.SAM, a.Efficiency, a.Status, b.BuyerName, c.StyleName, d.SizeName, e.Dia, f.PartName FROM (SELECT * FROM Order_Info WHERE Dia = " + dia + " AND BodyPart = " + bodyPart + ") a, Buyer b, Style c, Size d, Dia e, BodyPart f " +
                    "WHERE a.Buyer = b.Id AND a.Style = c.Id AND a.Size = d.Id AND a.Dia = e.Id AND a.BodyPart = f.Id";
                }
                if (buyerName > 0 && styleName > 0 && sizeNo > 0 && dia > 0 && bodyPart > 0)
                {
                    query = " SELECT a.Id, a.PurchaseOrderNo, a.Quantity, a.ShipmentDate, a.SAM, a.Efficiency, a.Status, b.BuyerName, c.StyleName, d.SizeName, e.Dia, f.PartName FROM (SELECT * FROM Order_Info WHERE Buyer = " + buyerName + " AND Style = " + styleName + " AND Size = " + sizeNo + " AND Dia = " + dia + " AND BodyPart = " + bodyPart + ") a, Buyer b, Style c, Size d, Dia e, BodyPart f " +
                    "WHERE a.Buyer = b.Id AND a.Style = c.Id AND a.Size = d.Id AND a.Dia = e.Id AND a.BodyPart = f.Id";
                }

                query = query + " AND a.ShipmentDate >= '" + DateTime.Now.Date + "' order by a.ShipmentDate asc";

                SqlDataReader reader = CommonFunctions.GetFromDB(query);

                if (reader.HasRows)
                {
                    orderInfoDetailsdataGridView.Rows.Clear();
                    while (reader.Read())
                    {
                        if (Convert.ToInt32(reader["Status"]) == 2)
                        {
                            S1 = SL;
                            IDs.Add((int)reader["Id"]);
                            S2 = reader.IsDBNull(reader.GetOrdinal("BuyerName")) == true ? "Not Defined" : reader["BuyerName"].ToString();
                            S3 = reader.IsDBNull(reader.GetOrdinal("StyleName")) == true ? "Not Defined" : reader["StyleName"].ToString();
                            S4 = reader.IsDBNull(reader.GetOrdinal("SizeName")) == true ? "Not Defined" : reader["SizeName"].ToString();
                            S5 = reader.IsDBNull(reader.GetOrdinal("Dia")) == true ? "Not Defined" : reader["Dia"].ToString();
                            S6 = reader.IsDBNull(reader.GetOrdinal("PartName")) == true ? "Not Defined" : reader["PartName"].ToString();
                            S7 = reader.IsDBNull(reader.GetOrdinal("PurchaseOrderNo")) == true ? "" : Convert.ToString(reader["PurchaseOrderNo"]);
                            S8 = reader.IsDBNull(reader.GetOrdinal("Quantity")) == true ? 0000.ToString() : Convert.ToString(reader["Quantity"]);
                            S9 = reader.IsDBNull(reader.GetOrdinal("ShipmentDate")) == true ? "0/0/0000" : Convert.ToDateTime(reader["ShipmentDate"]).ToString("dd/MM/yyyy");
                            S10 = reader.IsDBNull(reader.GetOrdinal("SAM")) == true ? 0.00.ToString() : Convert.ToString(reader["SAM"]);
                            S11 = reader.IsDBNull(reader.GetOrdinal("Efficiency")) == true ? 0.00.ToString() : Convert.ToString(reader["Efficiency"]);
                            S12 = Enum.GetName(typeof(VariableDecleration_Class.Status), reader.IsDBNull(reader.GetOrdinal("Status")) == true ? (int)VariableDecleration_Class.Status.Pending : Convert.ToInt32(reader["Status"]));

                            orderInfoDetailsdataGridView.Rows.Add(S1, S2, S3, S4, S5, S6, S7, S8, S9, S10, S11, S12, (int)reader["Id"]);
                            SL++;
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

        private void LoadComboBox()
        {
            LoadBuyer();
            LoadStyle();
            LoadDia();
            LoadSize();
            LoadPart();
        }

        private void LoadMachine()
        {
            try
            {
                if (diaComboBox.SelectedIndex < 1)
                {
                    return;
                }

                int dia = ((KeyValuePair<int, string>)diaComboBox.SelectedItem).Key;
                string query = "SELECT * FROM Machine_Info WHERE Status = 1 AND MachineDia = " + dia + " order by MachineNo asc";

                SqlDataReader reader = CommonFunctions.GetFromDB(query);
                MachineComboBox.Items.Clear();
                mcNo.Clear();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        mcNo.Add(Convert.ToInt32(reader["MachineNo"]));
                        MachineComboBox.Items.Add(new PlanningBoard.CheckComboBoxItem(reader["MachineNo"].ToString(), true));
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

        private void LoadSize()
        {
            try
            {
                SizeList.Add(0, "---Select Size---");
                string query = "SELECT * FROM Size order by SizeName asc";

                SqlDataReader reader = CommonFunctions.GetFromDB(query);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (!SizeList.Keys.Contains(Convert.ToInt32(reader["Id"])))
                        {
                            SizeList.Add(Convert.ToInt32(reader["Id"]), (reader["SizeName"]).ToString());
                        }
                        else
                        {
                            SizeList[Convert.ToInt32(reader["Id"])] = reader["SizeName"].ToString();
                        }
                    }
                    sizeComboBox.DataSource = new BindingSource(SizeList, null);
                    sizeComboBox.DisplayMember = "Value";
                    sizeComboBox.ValueMember = "Key";
                }
                if (PlanBoardDisplayForm.EditMode)
                {
                    sizeComboBox.Enabled = false;
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
                PartList.Add(0, "---Select Part---");
                string query = "SELECT * FROM BodyPart order by PartName asc";

                SqlDataReader reader = CommonFunctions.GetFromDB(query);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (!PartList.Keys.Contains(Convert.ToInt32(reader["Id"])))
                        {
                            PartList.Add(Convert.ToInt32(reader["Id"]), (reader["PartName"]).ToString());
                        }
                        else
                        {
                            PartList[Convert.ToInt32(reader["Id"])] = reader["PartName"].ToString();
                        }
                    }
                    partComboBox.DataSource = new BindingSource(PartList, null);
                    partComboBox.DisplayMember = "Value";
                    partComboBox.ValueMember = "Key";
                }
                if (PlanBoardDisplayForm.EditMode)
                {
                    partComboBox.Enabled = false;
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
                BuyerList.Add(0, "---Select Buyer---");
                string query = "SELECT * FROM Buyer order by BuyerName asc";

                SqlDataReader reader = CommonFunctions.GetFromDB(query);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (!BuyerList.Keys.Contains(Convert.ToInt32(reader["Id"])))
                        {
                            BuyerList.Add(Convert.ToInt32(reader["Id"]), (reader["BuyerName"]).ToString());
                        }
                        else
                        {
                            BuyerList[Convert.ToInt32(reader["Id"])] = reader["BuyerName"].ToString();
                        }
                    }
                    buyerComboBox.DataSource = new BindingSource(BuyerList, null);
                    buyerComboBox.DisplayMember = "Value";
                    buyerComboBox.ValueMember = "Key";
                }

                if (PlanBoardDisplayForm.EditMode)
                {
                    buyerComboBox.Enabled = false;
                }
                //MStatuscomboBox.DataSource = Enum.GetValues(typeof(VariableDecleration_Class.Status));
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
                StyleList.Add(0, "---Select Style---");
                string query = "SELECT * FROM Style order by StyleName asc";

                SqlDataReader reader = CommonFunctions.GetFromDB(query);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (!StyleList.Keys.Contains(Convert.ToInt32(reader["Id"])))
                        {
                            StyleList.Add(Convert.ToInt32(reader["Id"]), (reader["StyleName"]).ToString());
                        }
                        else
                        {
                            StyleList[Convert.ToInt32(reader["Id"])] = reader["StyleName"].ToString();
                        }
                    }
                    styleComboBox.DataSource = new BindingSource(StyleList, null);
                    styleComboBox.DisplayMember = "Value";
                    styleComboBox.ValueMember = "Key";
                }
                if (PlanBoardDisplayForm.EditMode)
                {
                    styleComboBox.Enabled = false;
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
                DiaList.Add(0, "---Select Dia---");
                string query = "SELECT * FROM Dia order by Dia asc";

                SqlDataReader reader = CommonFunctions.GetFromDB(query);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (!DiaList.Keys.Contains(Convert.ToInt32(reader["Id"])))
                        {
                            DiaList.Add(Convert.ToInt32(reader["Id"]), (reader["Dia"]).ToString());
                        }
                        else
                        {
                            DiaList[Convert.ToInt32(reader["Id"])] = reader["Dia"].ToString();
                        }
                    }
                    diaComboBox.DataSource = new BindingSource(DiaList, null);
                    diaComboBox.DisplayMember = "Value";
                    diaComboBox.ValueMember = "Key";
                }
                if (PlanBoardDisplayForm.EditMode)
                {
                    diaComboBox.Enabled = false;
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

        private void LoadOrderWisePlanGridEditMode()
        {
            try
            {
                orderWisePlandataGridView.Rows.Clear();
                int SL = 1; int S1 = 1; string S2 = ""; string S3 = ""; string S4 = ""; string S5 = ""; string S6 = ""; string S7 = ""; string S8 = ""; Boolean S9 = false;
                string query = "";
                var queue = new Queue<DateTime>();
                //DateTime taskDate = DateTime.Now;
                SqlDataReader reader = null;

                //query = "SELECT TaskDate FROM PlanTable WHERE MachineNo = " + MachineNo + " AND OrderID = " + orderID;
                //reader = CommonFunctions.GetFromDB(query);
                //if (reader.HasRows)
                //{
                //    while (reader.Read())
                //    {
                //        taskDate = Convert.ToDateTime(reader["TaskDate"]);
                //        queue.Enqueue(taskDate);
                //    }
                //}
                //while (queue.Count != 0)
                //{
                    DateTime taskDate = CurrentTaskDate;
                    query = "SELECT MachineNo, OrderID, TaskDate, Capacity, PlanQty, RemainingQty, Efficiency, Minute, (SELECT SUM(PlanQty) FROM PlanTable WHERE MachineNo = " + MachineNo +
                            " AND TaskDate = '" + taskDate + "' AND OrderID != " + orderID + ") As RestPlanQty, (SELECT SUM(SAM) FROM PlanTable WHERE MachineNo = " + MachineNo +
                            " AND TaskDate = '" + taskDate + "' AND OrderID != " + orderID + ") As TotalSAM, (SELECT COUNT(Efficiency) FROM PlanTable WHERE MachineNo = " + MachineNo +
                            " AND TaskDate = '" + taskDate + "' AND OrderID != " + orderID + ") As RestEfficiencyNumber, (SELECT SUM(Efficiency) FROM PlanTable WHERE MachineNo = " + MachineNo +
                            " AND TaskDate = '" + taskDate + "' AND OrderID != " + orderID + ") As TotalRestEfficiency, (SELECT TOP 1 Active FROM WorkingDays WHERE MachineNo = "
                            + MachineNo + " AND WorkDate = '" + taskDate + "') AS Active FROM PlanTable WHERE MachineNo = "
                            + MachineNo + " AND TaskDate = '" + taskDate + "' AND OrderID = " + orderID;

                    reader = CommonFunctions.GetFromDB(query);

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            S1 = SL;
                            S2 = Convert.ToString(reader["MachineNo"]);
                            S3 = Convert.ToDateTime(reader["TaskDate"]).ToString("dd/MM/yyyy");
                            S4 = Convert.ToString(reader["Capacity"]);
                            S5 = reader.IsDBNull(reader.GetOrdinal("RestPlanQty")) == true ? "0" : Convert.ToString(reader["RestPlanQty"]);
                            S6 = Convert.ToString(reader["PlanQty"]);
                            S7 = Convert.ToString(reader["Minute"]);
                            S8 = Convert.ToString(reader["Efficiency"]);
                            S9 = Convert.ToInt32(reader["Active"]) == 0 ? true : false;
                            orderWisePlandataGridView.Rows.Add(S1, S2, S3, S4, S5, S6, S7, S8, S9);
                            orderWisePlandataGridView.Rows[S1 - 1].Cells[7].ReadOnly = true;
                            if (S9)
                            {
                                orderWisePlandataGridView.Rows[S1 - 1].Cells[4].ReadOnly = true;
                                orderWisePlandataGridView.Rows[S1 - 1].Cells[6].ReadOnly = true;
                            }
                            SL++;
                        }
                    }
                //}
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
                CalculateOrderWisePlanGridSum();
            }
        }

        private void LoadOrderWisePlanGrid(int temp = 0, string sdate = null)
        {
            try
            {
                newOrderQtyTextBox.ReadOnly = false;
                AddPlanButton.Enabled = true;
                ApplyEffButton.Enabled = true;
                DateTime date = DateTime.Now;

                int j = 0; int SL = 1; int S1 = 1; string S2 = ""; string S3 = ""; int S4 = 0; int S5 = 0; int S6 = 0; int S7 = 0; double S8 = 0.0; Boolean S9 = false;

                String query = "";
                SqlDataReader reader = null;
                DateTime fromDate;
                DateTime toDate;
                if (AutoFlag)
                {
                    toDate = DateTime.MaxValue.Date.AddDays(-1);
                }
                else
                {
                    toDate = toDateTimePicker.Value.Date;
                }
                
                if (LCFlag && LcArray != null)
                {
                    LcCount = LcArray.Length > 0 ? 0 : int.MaxValue;
                }
                else
                {
                    LcCount = int.MaxValue;
                }
                if (NewVal < 0)
                {
                    orderWisePlandataGridView.Rows.Clear();
                    fromDate = fromDateTimePicker.Value.Date;
                    temp = Convert.ToInt32(newOrderQtyTextBox.Text);
                }
                else
                {
                    fromDate = Convert.ToDateTime(sdate).Date;
                    SL = orderWisePlandataGridView.Rows.Count + 1;
                }
                if (temp == 0)
                {
                    temp = Convert.ToInt32(qtyTextBox.Text);
                }


                if (LeftQty == 0)
                {
                    //query = " SELECT CASE WHEN SUM(PlanQty) = " + Convert.ToInt32(qtyTextBox.Text) + " THEN CAST( 1 as BIT ) ELSE CAST( 0 as BIT ) END AS A FROM PlanTable WHERE OrderID = " + orderID +
                    //        " AND TaskDate between '" + DateTime.ParseExact(fromDateTimePicker.Text, "dd/MM/yyyy", null) + "' AND '" + DateTime.ParseExact(toDateTimePicker.Text, "dd/MM/yyyy", null) + "'";

                    //if (CommonFunctions.IsTrue(query))
                    //{
                        newOrderQtyTextBox.ReadOnly = true;
                        AddPlanButton.Enabled = false;
                        ApplyEffButton.Enabled = false;
                        S9 = true;

                        for (date = fromDate; date <= toDate; date = date.AddDays(1))
                        {
                            query = " SELECT MachineNo, OrderID, TaskDate, PlanQty, (SELECT SUM(PlanQty) FROM PlanTable WHERE TaskDate = '" + date + "' AND OrderID != " + orderID + ") As RestPlanQty, Minute, Capacity, Efficiency, RemainingQty FROM PlanTable WHERE TaskDate = '" + date + "' AND OrderID = " + orderID;
                            reader = CommonFunctions.GetFromDB(query);
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    S1 = SL;
                                    S2 = Convert.ToString(reader["MachineNo"]);
                                    S3 = Convert.ToDateTime(reader["TaskDate"]).ToString("dd/MM/yyyy");
                                    S4 = (int)reader["Capacity"];
                                    S5 = reader.IsDBNull(reader.GetOrdinal("RestPlanQty")) == true ? 0 : (int)reader["RestPlanQty"];
                                    S6 = Convert.ToInt32(reader["PlanQty"]);
                                    S7 = Convert.ToInt32(reader["Minute"]);
                                    S8 = (int)reader["Efficiency"];
                                    temp = temp - S5;
                                    orderWisePlandataGridView.Rows.Add(S1, S2, S3, S4, S5, S6, S7, S8, S9);
                                    orderWisePlandataGridView.Rows[S1 - 1].Cells[5].ReadOnly = true;
                                    orderWisePlandataGridView.Rows[S1 - 1].Cells[7].ReadOnly = true;
                                    SL++;
                                }
                            }
                            else
                            {
                                temp = 0;
                                date = toDate.AddDays(1);
                            }
                        }
                    //}
                }
                
                else
                {
                    if (mcNo.Count > 0)
                    {
                        int i = 0;
                        while (i < mcNo.Count && temp > 0)
                        {
                            MachineNo = Convert.ToInt32(mcNo[i]);
                            int tempMachine = 0;
                            date = fromDate;

                            while (date <= toDate && temp > 0)
                            {
                                if (MachineNo != tempMachine)
                                {
                                    tempMachine = MachineNo;
                                    j = 0;
                                    LcCount = 0;
                                }
                                query = "SELECT * FROM PlanTable WHERE MachineNo = " + MachineNo + " AND TaskDate = '" + date.Date + "'";
                                if (CommonFunctions.recordExist(query))
                                {
                                    query = "SELECT * FROM PlanTable WHERE MachineNo = " + MachineNo + " AND TaskDate = '" + date.Date + "' AND OrderID = " + orderID;
                                    if (CommonFunctions.recordExist(query))
                                    {
                                        query = "SELECT MachineNo, OrderID, TaskDate, Capacity, PlanQty, RemainingQty, Efficiency, Minute, (SELECT SUM(PlanQty) FROM PlanTable WHERE MachineNo = " + MachineNo +
                                                " AND TaskDate = '" + date.Date + "' AND OrderID != " + orderID + ") As RestPlanQty, (SELECT SUM(SAM) FROM PlanTable WHERE MachineNo = " + MachineNo +
                                                " AND TaskDate = '" + date.Date + "' AND OrderID != " + orderID + ") As TotalSAM, (SELECT COUNT(Efficiency) FROM PlanTable WHERE MachineNo = " + MachineNo +
                                                " AND TaskDate = '" + date.Date + "' AND OrderID != " + orderID + ") As RestEfficiencyNumber, (SELECT SUM(Efficiency) FROM PlanTable WHERE MachineNo = " + MachineNo +
                                                " AND TaskDate = '" + date.Date + "' AND OrderID != " + orderID + ") As TotalRestEfficiency, (SELECT TOP 1 Active FROM WorkingDays WHERE MachineNo = "
                                                + MachineNo + " AND WorkDate = '" + date.Date + "') AS Active FROM PlanTable WHERE MachineNo = "
                                                + MachineNo + " AND TaskDate = '" + date.Date + "' AND OrderID = " + orderID;

                                        reader = CommonFunctions.GetFromDB(query);
                                        if (reader.HasRows)
                                        {
                                            while (reader.Read())
                                            {
                                                S1 = SL;
                                                S2 = Convert.ToString(reader["MachineNo"]);
                                                S3 = Convert.ToDateTime(reader["TaskDate"]).ToString("dd/MM/yyyy");
                                                S5 = reader.IsDBNull(reader.GetOrdinal("RestPlanQty")) == true ? 0 : (int)reader["RestPlanQty"];
                                                S7 = Convert.ToInt32(reader["Minute"]);
                                                if (MachineNo == Convert.ToInt32(reader["MachineNo"]) && orderID == Convert.ToInt32(reader["OrderID"]))
                                                {
                                                    S4 = (int)reader["Capacity"];
                                                    S6 = Convert.ToInt32(reader["PlanQty"]);
                                                    S8 = (int)reader["Efficiency"];
                                                    S9 = true;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        query = "SELECT SUM(PlanQty) AS RestPlanQty, (SELECT SUM(SAM) FROM PlanTable WHERE MachineNo = " + MachineNo +
                                                " AND TaskDate = '" + date.Date + "' AND OrderID != " + orderID + ") As TotalSAM, Count(*) AS RestEfficiencyNumber, SUM(Efficiency) AS TotalRestEfficiency, SUM(Minute) AS TotalRestMinute, (SELECT TOP 1 Minute FROM WorkingDays WHERE MachineNo = "
                                                + MachineNo + " AND WorkDate = '" + date.Date + "') AS Minute, (SELECT TOP 1 Active FROM WorkingDays WHERE MachineNo = "
                                                + MachineNo + " AND WorkDate = '" + date.Date + "') AS Active FROM PlanTable WHERE MachineNo = "
                                                + MachineNo + " AND TaskDate = '" + date.Date + "'";
                                        reader = CommonFunctions.GetFromDB(query);
                                        if (reader.HasRows)
                                        {
                                            newOrderQtyTextBox.ReadOnly = false;
                                            AddPlanButton.Enabled = true;
                                            ApplyEffButton.Enabled = true;

                                            while (reader.Read())
                                            {
                                                S9 = false;
                                                S1 = SL;
                                                S2 = MachineNo.ToString();
                                                S3 = date.ToString("dd/MM/yyyy");
                                                S5 = reader.IsDBNull(reader.GetOrdinal("RestPlanQty")) == true ? 0 : (int)reader["RestPlanQty"];

                                                Double RestEfficiencyNumber = reader.IsDBNull(reader.GetOrdinal("RestEfficiencyNumber")) == true ? 0 : Convert.ToDouble(reader["RestEfficiencyNumber"]);

                                                Double TotalRestEfficiency = reader.IsDBNull(reader.GetOrdinal("TotalRestEfficiency")) == true ? 0 : Convert.ToDouble(reader["TotalRestEfficiency"]);
                                                Double TotalSam = reader.IsDBNull(reader.GetOrdinal("TotalSAM")) == true ? 0 + Convert.ToDouble(samTextBox.Text) : (Convert.ToDouble(reader["TotalSAM"]) + Convert.ToDouble(samTextBox.Text));
                                                Double AVGSAM = TotalSam / (RestEfficiencyNumber + 1);

                                                int TotalMinute = reader.IsDBNull(reader.GetOrdinal("TotalRestMinute")) == true ? 0 + Convert.ToInt32(reader["Minute"]) : Convert.ToInt32(reader["TotalRestMinute"]) + Convert.ToInt32(reader["Minute"]);
                                                Double AVGMinute = Math.Floor(TotalMinute / (RestEfficiencyNumber + 1));
                                                S7 = Convert.ToInt32(AVGMinute);

                                                if (LCFlag)
                                                {
                                                    if (LcArray != null && LcCount < LcArray.Length)
                                                    {
                                                        Double AvgEff = (Convert.ToDouble((TotalRestEfficiency + Convert.ToDouble(LcArray[LcCount])) / (RestEfficiencyNumber + 1)));
                                                        if (Convert.ToInt32(reader["Active"]) == 0)
                                                        {
                                                            S4 = 0;
                                                            S8 = 0;
                                                        }
                                                        else
                                                        {
                                                            S4 = (int)Math.Floor((Convert.ToDouble(S7) * (Convert.ToDouble(AvgEff) / 100.00)) / (AVGSAM));
                                                            S8 = Convert.ToInt32(LcArray[LcCount]);
                                                            LcLastIndex = S1 - 1;
                                                            LcCount++;
                                                        }
                                                    }
                                                    else if (LcArray != null && LcCount == LcArray.Length)
                                                    {
                                                        Double AvgEff = 0;

                                                        AvgEff = (Convert.ToDouble((TotalRestEfficiency + Convert.ToDouble(LcArray[LcArray.Length - 1])) / (RestEfficiencyNumber + 1)));
                                                        S8 = Convert.ToInt32(LcArray[LcArray.Length - 1]);
                                                        S4 = Convert.ToInt32(reader["Active"]) == 0 ? 0 : (int)Math.Floor((Convert.ToDouble(S7) * (Convert.ToDouble(AvgEff) / 100.00)) / (AVGSAM));
                                                    }
                                                    else
                                                    {
                                                        Double AvgEff = 0;
                                                        if (NewVal > -1)
                                                        {
                                                            S8 = EffList.Count == 0 ? Convert.ToInt32(effTextBox.Text) : EffList.ContainsKey(S1 - 1) == true ? EffList[S1 - 1] : EffList[EffList.Count - 1];
                                                            AvgEff = (Convert.ToDouble((TotalRestEfficiency + S8) / (RestEfficiencyNumber + 1)));
                                                            //S7 = NewVal;
                                                        }
                                                        S4 = Convert.ToInt32(reader["Active"]) == 0 ? 0 : (int)Math.Floor((Convert.ToDouble(S7) * (Convert.ToDouble(AvgEff) / 100.00)) / (AVGSAM));
                                                    }
                                                }
                                                else
                                                {
                                                    Double AvgEff = (Convert.ToDouble((TotalRestEfficiency + Convert.ToDouble(effTextBox.Text)) / (RestEfficiencyNumber + 1)));
                                                    S4 = Convert.ToInt32(reader["Active"]) == 0 ? 0 : (int)Math.Floor((Convert.ToDouble(S7) * (Convert.ToDouble(AvgEff) / 100.00)) / (AVGSAM));
                                                    S8 = EffList.Count == 0 ? Convert.ToInt32(effTextBox.Text) : EffList.ContainsKey(S1 - 1) == true ? EffList[S1 - 1] : EffList[EffList.Count - 1];
                                                    //S7 = Convert.ToInt32(effTextBox.Text);
                                                }
                                                if (Convert.ToInt32(reader["Active"]) == 0)
                                                {
                                                    S6 = 0;
                                                    S9 = true;
                                                }
                                                else
                                                {
                                                    if (S4 - S5 > temp)
                                                    {
                                                        S6 = temp;
                                                        temp = 0;
                                                    }
                                                    else
                                                    {
                                                        if (S4 - S5 < 0)
                                                        {
                                                            S6 = 0;
                                                        }
                                                        else
                                                        {
                                                            S6 = S4 - S5;
                                                        }
                                                        temp = temp - S6;
                                                    }
                                                    S9 = false;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    S9 = false;
                                    query = "SELECT WorkDate, Minute, Active FROM WorkingDays WHERE MachineNo = " + MachineNo + " AND WorkDate = '" + date.Date + "'";
                                    reader = CommonFunctions.GetFromDB(query);
                                    if (reader.HasRows)
                                    {
                                        while (reader.Read())
                                        {
                                            S1 = SL;
                                            S2 = MachineNo.ToString();
                                            S3 = Convert.ToDateTime(reader["WorkDate"]).ToString("dd/MM/yyyy");
                                            if (LCFlag)
                                            {
                                                S4 = Convert.ToInt32(reader["Active"]) == 0 ? 0 : GetEachCapacity(S1, Convert.ToInt32(reader["Minute"]));
                                            }
                                            else
                                            {
                                                S4 = Convert.ToInt32(reader["Active"]) == 0 ? 0 : (int)Math.Floor((Convert.ToInt32(reader["Minute"]) * (Convert.ToDouble(effTextBox.Text) / 100.00)) / (Convert.ToDouble(samTextBox.Text)));
                                            }
                                            S5 = 0;
                                            if (Convert.ToInt32(reader["Active"]) == 0)
                                            {
                                                S6 = 0;
                                                S9 = true;
                                            }
                                            else
                                            {
                                                if (S4 - S5 >= temp)
                                                {
                                                    S6 = temp;
                                                    temp = 0;
                                                }
                                                else
                                                {
                                                    S6 = S4 - S5;
                                                    temp = temp - S6;
                                                }
                                            }
                                            S7 = Convert.ToInt32(reader["Active"]) == 0 ? 0 : Convert.ToInt32(reader["Minute"]);
                                            if (LCFlag && Convert.ToInt32(reader["Active"]) != 0)
                                            {
                                                if (LcArray != null && LcCount < LcArray.Length)
                                                {
                                                    if (Convert.ToInt32(reader["Active"]) == 0)
                                                    {
                                                        S8 = 0;
                                                    }
                                                    else
                                                    {
                                                        S8 = Convert.ToInt32(LcArray[LcCount]);
                                                        LcLastIndex = S1 - 1;
                                                        LcCount++;
                                                    }
                                                }
                                                else if (LcArray != null && LcCount >= LcArray.Length)
                                                {
                                                    S8 = Convert.ToInt32(LcArray[LcArray.Length - 1]);
                                                }
                                                else
                                                {
                                                    S8 = EffList.Count == 0 ? Convert.ToInt32(effTextBox.Text) : EffList.ContainsKey(S1 - 1) == true ? EffList[S1 - 1] : EffList[EffList.Count - 1];
                                                    //S7 = NewVal;
                                                }
                                            }
                                            else
                                            {
                                                //S7 = Convert.ToInt32(effTextBox.Text);
                                                S8 = EffList.Count == 0 ? Convert.ToInt32(effTextBox.Text) : EffList.ContainsKey(S1 - 1) == true ? EffList[S1 - 1] : EffList[EffList.Count - 1];
                                            }
                                        }
                                    }
                                    else
                                    {
                                        S1 = SL;
                                        S2 = "0";
                                        S3 = date.ToString("dd/MM/yyyy");
                                        S4 = 0;
                                        S5 = 0;
                                        S6 = 0;
                                        S7 = 0;
                                        S8 = 0;
                                    }
                                }

                                //if ((OrderWisePlanGridRowIndex == S1 - 1) && LCFlag == true && NewVal > -1)
                                //{
                                //    if (LcArray != null && S1 <= LcArray.Length)
                                //    {
                                //        orderWisePlandataGridView.Rows[S1 - 1].Cells[6].ReadOnly = true;
                                //    }
                                //    if (S8 == false)
                                //    {
                                //        orderWisePlandataGridView.Rows[OrderWisePlanGridRowIndex].Cells[2].Value = S3;
                                //        orderWisePlandataGridView.Rows[OrderWisePlanGridRowIndex].Cells[4].Value = S5;
                                //        orderWisePlandataGridView.Rows[OrderWisePlanGridRowIndex].Cells[6].Value = S7;
                                //        LCFlag = false;
                                //    }
                                //}
                                //else
                                //{
                                orderWisePlandataGridView.Rows.Add(S1, S2, S3, S4, S5, S6, S7, S8, S9);
                                if (S9)
                                {
                                    orderWisePlandataGridView.Rows[S1 - 1].Cells[5].ReadOnly = true;
                                    orderWisePlandataGridView.Rows[S1 - 1].Cells[7].ReadOnly = true;
                                }
                                if (LCFlag)
                                {
                                    if (LcArray != null && LcCount <= LcArray.Length)
                                    {
                                        orderWisePlandataGridView.Rows[S1 - 1].Cells[7].ReadOnly = true;
                                        if (LcCount == LcArray.Length)
                                        {
                                            LcCount = int.MaxValue;
                                        }
                                    }
                                }
                                //}

                                if (temp == 0)
                                {
                                    if (AutoFlag)
                                    {
                                        toDateTimePicker.Value = GetMaxDate();
                                        toDate = date.Date;
                                        AutoFlag = false;
                                    }
                                    date = toDateTimePicker.Value.AddDays(1);
                                }
                                else if (temp != 0 && AutoFlag == true && date == DateTime.ParseExact(shipDateTextBox.Text, "dd/MM/yyyy", null))
                                {
                                    if (i == mcNo.Count - 1)
                                    {
                                        DialogResult dialogResult = MessageBox.Show("Next Date Exceeds Given Shipment Date! Do you want to continue further?", "Confirmation", MessageBoxButtons.YesNo);
                                        if (dialogResult == DialogResult.No)
                                        {
                                            toDateTimePicker.Value = GetMaxDate();
                                            toDate = date.Date;
                                            date = toDateTimePicker.Value.AddDays(1);
                                            AutoFlag = false;
                                        }
                                    }
                                    else
                                    {
                                        date = toDate;
                                    }
                                }
                                SL++;
                                S9 = false;
                                date = date.AddDays(1);
                            }
                            i++;
                            fromDate = fromDateTimePicker.Value.Date;
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
                if (CommonFunctions.connection.State == ConnectionState.Open)
                {
                    CommonFunctions.connection.Close();
                }
                CalculateOrderWisePlanGridSum();
            }
        }

        private DateTime GetMaxDate()
        {
            DateTime maxDate = DateTime.MinValue.Date;
            if (orderWisePlandataGridView.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in orderWisePlandataGridView.Rows)
                {
                    if (DateTime.ParseExact(row.Cells[2].Value.ToString(), "dd/MM/yyyy", null) > maxDate)
                    {
                        maxDate = DateTime.ParseExact(row.Cells[2].Value.ToString(), "dd/MM/yyyy", null);
                    }
                }
            }
            return maxDate;
        }

        private void CalculateOrderWisePlanGridSum()
        {
            try
            {
                int bookedQtySum = 0;
                int plnQtySum = 0;

                if (orderWisePlandataGridView.Rows.Count > 0)
                {
                    foreach (DataGridViewRow row in orderWisePlandataGridView.Rows)
                    {
                        bookedQtySum = bookedQtySum + Convert.ToInt32(row.Cells[4].Value);
                        plnQtySum = plnQtySum + Convert.ToInt32(row.Cells[5].Value);
                    }
                    orderWisePlandataGridView.Rows.Add("Total:", "", "", "", bookedQtySum, plnQtySum, "", "");
                    orderWisePlandataGridView.Rows[orderWisePlandataGridView.Rows.Count - 1].Cells[5].ReadOnly = true;
                    orderWisePlandataGridView.Rows[orderWisePlandataGridView.Rows.Count - 1].Cells[7].ReadOnly = true;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("" + ex.ToString());
            }
            finally
            {
                LCFlag = false;
                mcNo.Clear();
                foreach (CheckComboBoxItem item in MachineComboBox.Items)
                {
                    if (item.CheckState == true)
                    {
                        mcNo.Add(Convert.ToInt32(item.Text));
                    }
                }
            }
        }

        private void calculateValues()
        {
            try
            {
                labelAlert.Visible = false;
                String query = "SELECT * FROM PlanTable WHERE OrderID = "+orderID;
                if (CommonFunctions.recordExist(query))
                {
                    PrePlanQty = 0;
                    query = "SELECT Count(Id) AS TotalOrdersCount, AVG(Capacity) AS AVGCapacity, SUM(Minute) AS TotalMinute, SUM(PlanQty) AS TotalPlanQty FROM PlanTable WHERE OrderID = " + orderID + " AND Capacity != 0";
                    SqlDataReader reader = CommonFunctions.GetFromDB(query);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            PrePlanQty = PrePlanQty + (reader.IsDBNull(reader.GetOrdinal("TotalPlanQty")) == true ? 0 : Convert.ToInt32(reader["TotalPlanQty"]));

                            if (PlanBoardDisplayForm.EditMode)
                            {
                                avgCapcityTextBox.Text = reader["AVGCapacity"].ToString();
                                ttlMinuteTextBox.Text = reader["TotalMinute"].ToString();
                                calculatedDaysNeedTextBox.Text = reader["TotalOrdersCount"].ToString();
                            }
                        }
                    }
                }
                else
                {
                    PrePlanQty = 0;
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
                LeftQty = Convert.ToInt32(qtyTextBox.Text) - PrePlanQty;
                if (LeftQty == 0 && PlanBoardDisplayForm.EditMode == false  )
                {
                    labelAlert.Visible = true;
                }
                leftQtyTextBox.Text = LeftQty.ToString();
                prevPlnQtyTextBox.Text = PrePlanQty.ToString();
                if (newOrderQtyTextBox.Text == "")
                {
                    newOrderQtyTextBox.Text = LeftQty.ToString();
                }
                newDaysTextBox.Text = ((Convert.ToDouble(newOrderQtyTextBox.Text)) / (Convert.ToDouble(avgCapcityTextBox.Text))).ToString("###,###.00");
                if(PlanBoardDisplayForm.EditMode)
                {
                    LoadOrderWisePlanGridEditMode();
                }
                else
                {
                    LoadOrderWisePlanGrid();
                } 
            }
        }

        private int GetEachCapacity(int SL, Double minute, int newEff = -1)
        {
            if (LcArray != null)
            {
                if (LcCount < LcArray.Length)
                    return (int)Math.Floor((minute * (Convert.ToDouble(LcArray[LcCount]) / 100.00)) / (Convert.ToDouble(samTextBox.Text)));
                else if (LcCount == LcArray.Length)
                    return (int)Math.Floor((minute * (Convert.ToDouble(LcArray[LcArray.Length - 1]) / 100.00)) / (Convert.ToDouble(samTextBox.Text)));
            }
            else
            {
                if (newEff > -1)
                {
                    return (int)Math.Floor((minute * (Convert.ToDouble(NewVal) / 100.00)) / (Convert.ToDouble(samTextBox.Text)));
                }
                else
                {
                    int eff = EffList.Count == 0 ? Convert.ToInt32(effTextBox.Text) : EffList.ContainsKey(SL - 1) == true ? EffList[SL - 1] : EffList[EffList.Count - 1];
                    return (int)Math.Floor((minute * (Convert.ToDouble(eff) / 100.00)) / (Convert.ToDouble(samTextBox.Text)));
                }
            }

            return (int)Math.Floor((minute * (Convert.ToDouble(LcArray[LcArray.Length - 1]) / 100.00)) / (Convert.ToDouble(samTextBox.Text)));
            
        }

        private int GetTotalMinute()
        {
            try
            {
                int minuteCap = 0;
                int j = 0;
                while (j < mcNo.Count)
                {
                    MachineNo = (int)mcNo[j];
                    String query = "SELECT * FROM WorkingDays WHERE WorkDate between '" + DateTime.ParseExact(fromDateTimePicker.Text, "dd/MM/yyyy", null) + "' AND '" + DateTime.ParseExact(toDateTimePicker.Text, "dd/MM/yyyy", null) + "' AND MachineNo = " + MachineNo + " AND Active != " + 0;
                    int i = 0;
                    SqlDataReader reader = CommonFunctions.GetFromDB(query);
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            minuteCap = minuteCap + (int)reader["Minute"];
                            i++;
                        }
                    }
                    else
                    {
                        minuteCap = 0;
                    }
                    daydiff = i;
                    j++;
                }
                
                return minuteCap;
            }

            catch (Exception ee)
            {
                MessageBox.Show("" + ee.ToString());
                return 0;
            }

            finally
            {
                if (CommonFunctions.connection.State == ConnectionState.Open)
                {
                    CommonFunctions.connection.Close();
                }
            }
        }

        private void SetRowID(int Index)
        {
            DataGridViewRow row = orderInfoDetailsdataGridView.Rows[Index];

            int buyerName = BuyerList.FirstOrDefault(x => x.Value == row.Cells[2].Value.ToString()).Key;
            int styleName = StyleList.FirstOrDefault(x => x.Value == row.Cells[3].Value.ToString()).Key;
            int sizeNo = SizeList.FirstOrDefault(x => x.Value == row.Cells[4].Value.ToString()).Key;
            int dia = DiaList.FirstOrDefault(x => x.Value == row.Cells[5].Value.ToString()).Key;
            int bodyPart = PartList.FirstOrDefault(x => x.Value == row.Cells[6].Value.ToString()).Key;

            try
            {
                string query = "SELECT Top 1* FROM Order_Info WHERE Buyer = " + buyerName + " AND Style = " + styleName + " AND Size = " + sizeNo + " AND Dia = " + dia + " AND BodyPart = " + bodyPart + "";
                SqlDataReader reader = CommonFunctions.GetFromDB(query);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        orderID = (int) reader["Id"];
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

        private void buyerComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (hiddenTextBox.Text == "")
            {
                orderInfoDetailsdataGridView.Rows.Clear();
                LoadOrderInfoGrid();
            }
        }

        private void styleComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (hiddenTextBox.Text == "")
            {
                orderInfoDetailsdataGridView.Rows.Clear();
                LoadOrderInfoGrid();
            }
        }

        private void sizeComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (hiddenTextBox.Text == "")
            {
                orderInfoDetailsdataGridView.Rows.Clear();
                LoadOrderInfoGrid();
            }
        }

        private void diaComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (hiddenTextBox.Text == "")
            {
                orderInfoDetailsdataGridView.Rows.Clear();
                LoadOrderInfoGrid();
            }
            LoadMachine();
        }

        private void partComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (hiddenTextBox.Text == "")
            {
                orderInfoDetailsdataGridView.Rows.Clear();
                LoadOrderInfoGrid();
            }
        }

        private void orderInfoDetailsdataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void CloseBtn_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void textBox2_TextChanged(object sender, System.EventArgs e)
        {

        }

        private void AddPlanButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (!IsValid())
                {
                    labelAlert.Visible = true;
                    return;
                }
                string query = "";
                bool result = false;
                if (orderInfoDetailsdataGridView.Rows.Count == 0)
                {
                    MessageBox.Show("Table has no rows to save!!!");
                    return;
                }

                if (orderID == 0)
                {
                    MessageBox.Show("Please Select a Row!!!");
                    return;
                }

                if (mcNo.Count == 0)
                {
                    MessageBox.Show("Please Select Machine No!!!", VariableDecleration_Class.sMSGBOX);
                    toDateTimePicker.Focus();
                    return;
                }

                if (dayDiffTextBox.Text == "" && calculatedDaysNeedTextBox.Text == "")
                {
                    MessageBox.Show("Empty!!!", VariableDecleration_Class.sMSGBOX);
                    return;
                }
                if (Convert.ToInt32(orderWisePlandataGridView.Rows[orderWisePlandataGridView.Rows.Count - 1].Cells[5].Value) < 1)
                {
                    MessageBox.Show("Plan Qty is Zero! No Qty is Left to Add!!!", VariableDecleration_Class.sMSGBOX);
                    return;
                }

                double daysInHand = Convert.ToDouble(dayDiffTextBox.Text);
                double calculatedDaysNeed = Convert.ToDouble(calculatedDaysNeedTextBox.Text);

                calculatedDaysNeed = Math.Floor(calculatedDaysNeed);
                string style = (StyleList[(int)styleComboBox.SelectedValue]).ToString();
                string size = (SizeList[(int)sizeComboBox.SelectedValue]).ToString();
                string dia = (DiaList[(int)diaComboBox.SelectedValue]).ToString();
                string part = (PartList[(int)partComboBox.SelectedValue]).ToString();
                double sam = Convert.ToDouble(samTextBox.Text);
                double eff = Convert.ToInt32(effTextBox.Text);
                int orderQty = Convert.ToInt32(qtyTextBox.Text);
                int planQty = newOrderQtyTextBox.Text != "" ? Convert.ToInt32(newOrderQtyTextBox.Text) : Convert.ToInt32(leftQtyTextBox.Text);
                
                string Remarks = remarkTextBox.Text;
                DateTime PlanDate = DateTime.ParseExact(planDateTimePicker.Text, "dd/MM/yyyy", null);
                DateTime PlanStartDate = DateTime.ParseExact(fromDateTimePicker.Text, "dd/MM/yyyy", null);
                DateTime PlanEndDate = DateTime.ParseExact(toDateTimePicker.Text, "dd/MM/yyyy", null);
                DateTime shipDate = DateTime.ParseExact(shipDateTextBox.Text, "dd/MM/yyyy", null);
                PlanBoardDisplayForm.orderID = orderID;
                PlanBoardDisplayForm.MachineNo = MachineNo;
                query = " IF NOT EXISTS (SELECT * FROM OrderPlanTable WHERE MachineNo = " + MachineNo + " AND OrderID = " + orderID + " AND PlanStartDate = '" + PlanStartDate + "') INSERT INTO OrderPlanTable " +
                        "(MachineNo, OrderID, PlanDate, PlanStartDate, PlanEndDate, Remarks) VALUES (" + MachineNo + "," + orderID + ",'" + PlanDate + "','" + PlanStartDate + "','" + PlanEndDate + "','" + Remarks + "')";
                if (CommonFunctions.ExecutionToDB(query, 3))
                {
                    foreach (DataGridViewRow row in orderWisePlandataGridView.Rows)
                    {
                        if (row.Index != orderWisePlandataGridView.Rows.Count - 1)
                        {
                            if (Convert.ToBoolean(row.Cells[8].Value) == false && Convert.ToInt32(row.Cells[5].Value) > 0)
                            {
                                MachineNo = Convert.ToInt32(row.Cells[1].Value);
                                int capacity = Convert.ToInt32(row.Cells[3].Value);
                                int remainQty = Convert.ToInt32(row.Cells[3].Value) - (Convert.ToInt32(row.Cells[4].Value) + Convert.ToInt32(row.Cells[5].Value));
                                remainQty = remainQty < 0 ? 0 : remainQty;
                                int plnQty = Convert.ToInt32(row.Cells[5].Value);
                                int efficiency = Convert.ToInt32(row.Cells[7].Value);
                                int minute = Convert.ToInt32(row.Cells[6].Value);
                                DateTime taskDate = DateTime.ParseExact(row.Cells[2].Value.ToString(), "dd/MM/yyyy", null);
                                int active = (int)row.Cells[3].Value < 1 ? 0 : 1;
                                query = "IF EXISTS (SELECT * FROM PlanTable WHERE MachineNo = " + MachineNo + " AND OrderID = " + orderID + " AND TaskDate = '" + taskDate + "' ) UPDATE PlanTable SET " +
                                        "Capacity = " + capacity + ", PlanQty = " + plnQty + ", RemainingQty = " + remainQty + ", OrderQty = " + orderQty + ", Efficiency = " + efficiency +
                                        " WHERE MachineNo = " + MachineNo + " AND OrderID = " + orderID + " AND TaskDate = '" + taskDate + "' ELSE " +
                                        "INSERT INTO PlanTable (MachineNo, TaskDate, OrderID, Capacity, PlanQty, RemainingQty, OrderQty, Efficiency, SAM, Minute, RevertVal, ActualQty, Status, Production) " +
                                        "VALUES (" + MachineNo + ",'" + taskDate + "'," + orderID + "," + capacity + "," + plnQty + "," + remainQty + "," + orderQty + "," + efficiency + "," + Convert.ToDouble(samTextBox.Text) + "," + minute + ", 0, 0, 0, 1)";
                                result = CommonFunctions.ExecutionToDB(query, 3);
                            }
                        }   
                    }
                    if (result)
                    {
                        MessageBox.Show("Added Successfully!!!");
                        resetAll();
                        PlanBoardDisplayForm.planStartDate = fromDateTimePicker.Value;
                        PlanBoardDisplayForm.planEndDate = toDateTimePicker.Value;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Failed To Add!!!");
                        return;
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

        private void setValue()
        {
            resetMachinePart();
            if (PlanBoardDisplayForm.EditMode && orderID != 0)
            {
                qtyTextBox.Text = orderInfoDetailsdataGridView.Rows[rowIndex].Cells[7].Value.ToString();
                samTextBox.Text = orderInfoDetailsdataGridView.Rows[rowIndex].Cells[9].Value.ToString();
                effTextBox.Text = orderInfoDetailsdataGridView.Rows[rowIndex].Cells[10].Value.ToString();
                shipDateTextBox.Text = orderInfoDetailsdataGridView.Rows[rowIndex].Cells[8].Value.ToString();
                ttlMinuteTextBox.ReadOnly = true;
                avgCapcityTextBox.ReadOnly = true;
                calculatedDaysNeedTextBox.ReadOnly = true;
                newOrderQtyTextBox.ReadOnly = true;
                newDaysTextBox.ReadOnly = true;

                calculateValues();
            }
            else 
            {
                if (mcNo.Count > 0 && orderID != 0)
                {
                    shipDate = DateTime.ParseExact(orderInfoDetailsdataGridView.Rows[rowIndex].Cells[8].Value.ToString(), "dd/MM/yyyy", null);
                    if (toDateTimePicker.Value > shipDate)
                    {
                        MessageBox.Show("End Date Can not be greater than Knitting Close Date", VariableDecleration_Class.sMSGBOX);
                        toDateTimePicker.Value = shipDate;
                        toDateTimePicker.Focus();
                        return;
                    }

                    //MachineNo = Convert.ToInt32(MachineList[(int)MachineComboBox.SelectedValue]);
                    qtyTextBox.Text = orderInfoDetailsdataGridView.Rows[rowIndex].Cells[7].Value.ToString();
                    samTextBox.Text = orderInfoDetailsdataGridView.Rows[rowIndex].Cells[9].Value.ToString();
                    effTextBox.Text = orderInfoDetailsdataGridView.Rows[rowIndex].Cells[10].Value.ToString();
                    shipDateTextBox.Text = orderInfoDetailsdataGridView.Rows[rowIndex].Cells[8].Value.ToString();
                    totalMinute = GetTotalMinute();
                    if (totalMinute == 0)
                    {
                        resetMachinePart();
                        MessageBox.Show("Machine Does not Exist in those dates!!!");
                        return; 
                    }
                    else
                    {
                        AvgCapacity = Convert.ToInt32(Math.Floor((Convert.ToDouble(effTextBox.Text) / 100.00) * (totalMinute / daydiff) / Convert.ToDouble(samTextBox.Text)));
                        dayDiffTextBox.Text = daydiff.ToString();
                        avgCapcityTextBox.Text = AvgCapacity.ToString();
                        ttlMinuteTextBox.Text = totalMinute.ToString();
                        calculatedDaysNeedTextBox.Text = (Convert.ToDouble(qtyTextBox.Text) / Convert.ToDouble(avgCapcityTextBox.Text)).ToString("###,###.00");
                        
                        calculateValues();
                    }
                }
                else if (mcNo.Count > 0 && orderID == 0)
                {
                    //MachineNo = Convert.ToInt32(MachineList[(int)MachineComboBox.SelectedValue]);
                    totalMinute = GetTotalMinute();
                    dayDiffTextBox.Text = daydiff.ToString();
                }
                else if (mcNo.Count < 1 && orderID != 0)
                {
                    qtyTextBox.Text = orderInfoDetailsdataGridView.Rows[rowIndex].Cells[7].Value.ToString();
                    samTextBox.Text = orderInfoDetailsdataGridView.Rows[rowIndex].Cells[9].Value.ToString();
                    effTextBox.Text = orderInfoDetailsdataGridView.Rows[rowIndex].Cells[10].Value.ToString();
                    shipDateTextBox.Text = orderInfoDetailsdataGridView.Rows[rowIndex].Cells[8].Value.ToString();
                    resetMachinePart();
                    orderWisePlandataGridView.Rows.Clear();
                }
            }
        }

        private bool IsValid()
        {
            try
            {
                string query = "SELECT SUM(PlanQty) AS TotalPlanQty FROM PlanTable WHERE MachineNo = " + MachineNo + " AND OrderID = "+orderID;
                //string query = "SELECT SUM(PlanQty) FROM PlanTable WHERE MachineNo = " + MachineNo + " AND TaskDate between '" + DateTime.ParseExact(fromDateTimePicker.Text, "dd/MM/yyyy", null) + "' AND '" + DateTime.ParseExact(toDateTimePicker.Text, "dd/MM/yyyy", null) + "'";
                SqlDataReader reader = CommonFunctions.GetFromDB(query);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (!reader.IsDBNull(reader.GetOrdinal("TotalPlanQty")))
                        {
                            if (Convert.ToInt32(reader["TotalPlanQty"]) == Convert.ToInt32(qtyTextBox.Text))
                            {
                                return false;
                            }
                            else
                            {
                                return true;
                            }
                        }
                        else
                        {
                            return true;
                        }
                        
                    }
                }
                else
                {
                    return true;
                }

                return true;
            }
            catch (Exception ee)
            {
                MessageBox.Show("" + ee.ToString());
                return false;
            }
            finally
            {
                if (CommonFunctions.connection.State == ConnectionState.Open)
                {
                    CommonFunctions.connection.Close();
                }
            }
        }

        private void resetMachinePart()
        {
            avgCapcityTextBox.Text = "";
            calculatedDaysNeedTextBox.Text = "";
            ttlMinuteTextBox.Text = "";
            prevPlnQtyTextBox.Text = "";
            leftQtyTextBox.Text = "";
            PrePlanQty = 0;
            LeftQty = 0;
            daydiff = 0;
            AvgCapacity = 0;
            newOrderQtyTextBox.Text = "";
            LCFlag = false;
            newDaysTextBox.Text = "";
        }

        private void resetAll()
        {
            orderInfoDetailsdataGridView.Rows.Clear();
            orderWisePlandataGridView.Rows.Clear();
            orderID = 0;
            qtyTextBox.Text = "";
            LoadDatePicker();
            remarkTextBox.Text = "";
            hiddenTextBox.Text = "";
            newOrderQtyTextBox.Text = "";
            newDaysTextBox.Text = "";
            rowIndex = -1;
            PrePlanQty = 0;
            LeftQty = 0;
            AvgCapacity = 0;
            totalMinute = 0;
            daydiff = 0;
            labelAlert.Visible = false;
            CurrentTaskDate = DateTime.Now;
            buyerComboBox.SelectedIndex = 0;
            styleComboBox.SelectedIndex = 0;
            sizeComboBox.SelectedIndex = 0;
            diaComboBox.SelectedIndex = 0;
            partComboBox.SelectedIndex = 0;
            LCFlag = false;
            LcArray = null;
            EffList.Clear();
            mcNo.Clear();

        }

        private void resetButton_Click(object sender, System.EventArgs e)
        {
            resetAll();
        }

        private void qtyTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void sizeComboBox_SelectedIndexChanged_1(object sender, System.EventArgs e)
        {
            if (hiddenTextBox.Text == "")
            {
                orderInfoDetailsdataGridView.Rows.Clear();
                LoadOrderInfoGrid();
            }
        }

        private void orderInfoDetailsdataGridView_CellDoubleClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                rowIndex = e.RowIndex;
                toDateTimePicker.Value = DateTime.ParseExact(orderInfoDetailsdataGridView.Rows[e.RowIndex].Cells["KnitCloseDate"].Value.ToString(), "dd/MM/yyyy", null);
                hiddenTextBox.Text = orderInfoDetailsdataGridView.Rows[e.RowIndex].Cells["ID"].Value.ToString();
                orderID = Convert.ToInt32(hiddenTextBox.Text);
                if (!PlanBoardDisplayForm.EditMode)
                {
                    buyerComboBox.Text = orderInfoDetailsdataGridView.Rows[e.RowIndex].Cells["Buyer"].Value.ToString();
                    styleComboBox.Text = orderInfoDetailsdataGridView.Rows[e.RowIndex].Cells["Style"].Value.ToString();
                    sizeComboBox.Text = orderInfoDetailsdataGridView.Rows[e.RowIndex].Cells["Size"].Value.ToString();
                    diaComboBox.Text = orderInfoDetailsdataGridView.Rows[e.RowIndex].Cells["Dia"].Value.ToString();
                    partComboBox.Text = orderInfoDetailsdataGridView.Rows[e.RowIndex].Cells["Part"].Value.ToString();
                }
                setValue();
                LCFlag = false;
                LcArray = null;
                
                hiddenTextBox.Text = "";
            }
        }

        private void fromDateTimePicker_ValueChanged(object sender, System.EventArgs e)
        {
            if (toDateTimePicker.Text == "" || fromDateTimePicker.Text == "")
            {
                return;
            }
            if (toDateTimePicker.Value < fromDateTimePicker.Value)
            {
                MessageBox.Show("Start DATE Can not be Greater than End DATE");
                fromDateTimePicker.Value = DateTime.Now.Date;
                return;
            }
            setValue();
        }

        private void toDateTimePicker_ValueChanged(object sender, System.EventArgs e)
        {

            if (toDateTimePicker.Text == "" || fromDateTimePicker.Text == "")
            {
                return;
            }

            if (toDateTimePicker.Value < fromDateTimePicker.Value)
            {
                MessageBox.Show("Start DATE Can not be Greater than End DATE");
                toDateTimePicker.Value = DateTime.Now.Date;
                return;
            }
            if (!AutoFlag)
            {
                setValue();
            }
            
        }

        private void newOrderQtyTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void newOrderQtyTextBox_TextChanged(object sender, System.EventArgs e)
        {
            if (mcNo.Count > 0 && orderID != 0 && AvgCapacity != 0)
            {
                if (newOrderQtyTextBox.Text == "")
                {
                    MessageBox.Show("Qty can not be Empty!!!");
                    newOrderQtyTextBox.Text = leftQtyTextBox.Text;
                    return;
                }
                if (Convert.ToInt32(newOrderQtyTextBox.Text) > Convert.ToInt32(qtyTextBox.Text))
                {
                    MessageBox.Show("New Qty can not exceed to Predefined Order Qty!!!");
                    newOrderQtyTextBox.Text = leftQtyTextBox.Text;
                    return;
                }
                LoadOrderWisePlanGrid();
            }
        }

        private void orderWisePlandataGridView_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (orderWisePlandataGridView.CurrentCell.ColumnIndex == 5) //Desired Column
            {
                e.Control.KeyPress -= new KeyPressEventHandler(PlanQty_KeyPress);
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(PlanQty_KeyPress);
                }
            }
            else if (orderWisePlandataGridView.CurrentCell.ColumnIndex == 7) //Desired Column
            {
                e.Control.KeyPress -= new KeyPressEventHandler(Eff_KeyPress);
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Eff_KeyPress);
                }
            }
            else
            {
                return;
            }
        }

        private void Eff_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void PlanQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void loadEffList()
        {
            EffList.Clear();
            foreach (DataGridViewRow row in orderWisePlandataGridView.Rows)
            {
                if (row.Index != orderWisePlandataGridView.Rows.Count - 1)
                {
                    EffList.Add(Convert.ToInt32(row.Index), Convert.ToInt32(row.Cells[7].Value));
                }
            }
        }

        private void orderWisePlandataGridView_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (PlanBoardDisplayForm.EditMode)
            {
                if (orderID != 0)
                {
                    if (e.RowIndex > -1 && e.RowIndex < orderWisePlandataGridView.Rows.Count - 1 && (e.ColumnIndex == 5 || e.ColumnIndex == 7 || e.ColumnIndex == 2) && OrderWisePlanGridRowIndex == -1 && PreVal < 0)
                    {
                        if (orderWisePlandataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "")
                        {
                            PreIndex = e.RowIndex;
                            if(e.ColumnIndex != 2)
                            {
                                PreVal = Convert.ToInt32(orderWisePlandataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                            }
                            else
                            {
                                rectangle = orderWisePlandataGridView.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true); //  
                                TaskDateDTP.Size = new Size(rectangle.Width, rectangle.Height); //  
                                TaskDateDTP.Location = new Point(rectangle.X, rectangle.Y); //  
                                TaskDateDTP.Visible = true;
                            }
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else
            {
                if (mcNo.Count > 0 && orderID != 0)
                {
                    if (e.RowIndex > -1 && e.RowIndex < orderWisePlandataGridView.Rows.Count - 1 && (e.ColumnIndex == 5 || e.ColumnIndex == 7) && OrderWisePlanGridRowIndex == -1 && PreVal < 0)
                    {
                        if (orderWisePlandataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() != "")
                        {
                            PreVal = Convert.ToInt32(orderWisePlandataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                            PreIndex = e.RowIndex;
                            loadEffList();
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        private void UpdateButton_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (PlanBoardDisplayForm.EditMode)
                {
                    string query = "";
                    DateTime TaskDate = DateTime.ParseExact(orderWisePlandataGridView.Rows[0].Cells[2].Value.ToString(), "dd/MM/yyyy", null);

                    int capacity = Convert.ToInt32(orderWisePlandataGridView.Rows[0].Cells[3].Value);
                    int planQty = Convert.ToInt32(orderWisePlandataGridView.Rows[0].Cells[5].Value);
                    int remainingQty = capacity - planQty;
                    int efficiency = Convert.ToInt32(orderWisePlandataGridView.Rows[0].Cells[7].Value);
                    int orderQty = Convert.ToInt32(qtyTextBox.Text);
                    int minute = Convert.ToInt32(orderWisePlandataGridView.Rows[0].Cells[6].Value);

                    if (CurrentTaskDate != TaskDate)
                    {
                        query = "DELETE FROM PlanTable WHERE MachineNo = " + MachineNo + " AND OrderID = " + orderID + " AND TaskDate = '" + CurrentTaskDate + "'";
                        Boolean result1 = CommonFunctions.ExecutionToDB(query, 3);
                    }

                    query = "IF EXISTS (SELECT * FROM PlanTable WHERE MachineNo = " + MachineNo + " AND OrderID = " + orderID + " AND TaskDate = '" + TaskDate + "' ) UPDATE PlanTable SET " +
                            "Capacity = " + capacity + ", PlanQty = " + planQty + ", RemainingQty = " + remainingQty + ", Efficiency = " + efficiency +
                            " WHERE MachineNo = " + MachineNo + " AND OrderID = " + orderID + " AND TaskDate = '" + TaskDate + "' ELSE " +
                            "INSERT INTO PlanTable (MachineNo, TaskDate, OrderID, Capacity, PlanQty, RemainingQty, OrderQty, Efficiency, SAM, Minute, RevertVal, ActualQty, Status, Production) " +
                            "VALUES (" + MachineNo + ",'" + TaskDate + "'," + orderID + "," + capacity + "," + planQty + "," + remainingQty + "," + orderQty + "," + efficiency + "," + Convert.ToDouble(samTextBox.Text) + "," + minute + ", 0, 0, 0, 1)";

                    
                    Boolean result = CommonFunctions.ExecutionToDB(query, 3);

                    if (result)
                    {
                        MessageBox.Show("Updated Successfully!!!");
                    }
                    else
                    {
                        MessageBox.Show("Failed To Update!!! Try Again!!!");
                        return;
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

        private void orderWisePlandataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (PlanBoardDisplayForm.EditMode)
            {
                TaskDateDTP.Visible = false;

                if (orderWisePlandataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null || orderWisePlandataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == "")
                {
                    MessageBox.Show("Null Value can not used!!!");
                    orderWisePlandataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = PreVal;
                    ResetOrderWiseePlanTableParameters();
                    return;
                }
                if (e.RowIndex != PreIndex)
                {
                    ResetOrderWiseePlanTableParameters();
                    return;
                }
                if (e.ColumnIndex == 7)
                {
                    if (Convert.ToInt32(orderWisePlandataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) > 100)
                    {
                        MessageBox.Show("Efficiency can not be greater than 100!!!");
                        orderWisePlandataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = PreVal;
                        ResetOrderWiseePlanTableParameters();
                        return;
                    }
                    
                }
                if (e.ColumnIndex == 5)
                {
                    if (Convert.ToInt32(orderWisePlandataGridView.Rows[e.RowIndex].Cells[5].Value) > (Convert.ToInt32(orderWisePlandataGridView.Rows[e.RowIndex].Cells[3].Value) - Convert.ToInt32(orderWisePlandataGridView.Rows[e.RowIndex].Cells[4].Value)))
                    {
                        MessageBox.Show("Plan Qty can not be greater than Remaining Qty!!!");
                        orderWisePlandataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = PreVal;
                        ResetOrderWiseePlanTableParameters();
                        return;
                    }
                }
                

                if (orderWisePlandataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && e.RowIndex > -1 && e.RowIndex < (orderWisePlandataGridView.Rows.Count - 1) && (e.ColumnIndex == 2 || e.ColumnIndex == 5 || e.ColumnIndex == 7) && OrderWisePlanGridRowIndex == -1 && NewVal < 0)
                {
                    if (e.ColumnIndex == 2) // TaskDate Column
                    {
                        //DateTime NewDate = Convert.ToDateTime(TaskDateDTP.Text);
                        DateTime NewDate = DateTime.ParseExact(TaskDateDTP.Text, "dd/MM/yyyy", null);

                        if(CommonFunctions.recordExist("SELECT * FROM WorkingDays WHERE MachineNo = "+MachineNo+" AND WorkDate = '"+NewDate+"'")) // Check if Date is Scheduled on Workding Days
                        {
                            if(CommonFunctions.recordExist("SELECT * FROM WorkingDays WHERE MachineNo = "+MachineNo+" AND Active = 1 AND WorkDate = '"+NewDate+"'")) // Check if Date is Active
                            {
                                if (CommonFunctions.recordExist("SELECT * FROM PlanTable WHERE MachineNo = " + MachineNo + " AND TaskDate = '" + NewDate + "'")) // Check if Date is already Present on PlanTable
                                {
                                    int minute = 0;
                                    int capacity = 0;
                                    int TotalPlanQty = 0;
                                    int efficiency = 0;
                                    int planQty = 0;
                                    int remainingQty = 0;

                                    if (CommonFunctions.recordExist("SELECT * FROM PlanTable WHERE MachineNo = " + MachineNo + " AND TaskDate = '" + NewDate + "' AND OrderID = " + orderID)) // Check if same order exists on that day on PlanTable
                                    {
                                        string query = "SELECT Capacity, Efficiency,"
                                        +" (SELECT SUM(PlanQty) FROM PlanTable WHERE MachineNo = " + MachineNo + " AND OrderID != "+orderID+" AND TaskDate = '" + NewDate + "') As RemainingQty,"
                                        +" PlanQty, (SELECT SUM(PlanQty) FROM PlanTable WHERE MachineNo = " + MachineNo + " AND TaskDate = '" + NewDate + "') AS TotalPlanQty,"
                                        +" Minute FROM PlanTable WHERE MachineNo = " + MachineNo + " AND TaskDate = '" + NewDate + "' AND OrderID = " + orderID;
                                        SqlDataReader reader = CommonFunctions.GetFromDB(query);
                                        try
                                        {
                                            if (reader.HasRows)
                                            {
                                                while (reader.Read())
                                                {
                                                    capacity = reader.IsDBNull(reader.GetOrdinal("Capacity")) == true ? 0 : Convert.ToInt32(reader["Capacity"]);
                                                    TotalPlanQty = reader.IsDBNull(reader.GetOrdinal("TotalPlanQty")) == true ? 0 : Convert.ToInt32(reader["TotalPlanQty"]);
                                                    minute = reader.IsDBNull(reader.GetOrdinal("Minute")) == true ? 0 : Convert.ToInt32(reader["Minute"]);
                                                    efficiency = reader.IsDBNull(reader.GetOrdinal("Efficiency")) == true ? 0 : Convert.ToInt32(reader["Efficiency"]);
                                                    planQty = reader.IsDBNull(reader.GetOrdinal("PlanQty")) == true ? 0 : Convert.ToInt32(reader["PlanQty"]);
                                                    remainingQty = reader.IsDBNull(reader.GetOrdinal("RemainingQty")) == true ? 0 : Convert.ToInt32(reader["RemainingQty"]);
                                                }
                                            }
                                            if (capacity > TotalPlanQty)
                                            {
                                                int currentPlanQty = Convert.ToInt32(orderWisePlandataGridView.Rows[e.RowIndex].Cells[5].Value);
                                                //double UpdatedEfficiency = (double)(efficiency + Convert.ToInt32(orderWisePlandataGridView.Rows[e.RowIndex].Cells[6].Value)) / 2.00;
                                                //int newCapacity = Convert.ToInt32(Math.Floor((double)((minute * (UpdatedEfficiency / 100.00)) / Convert.ToDouble(samTextBox.Text))));
                                                int newPlanQty = currentPlanQty + planQty > capacity ? currentPlanQty - ((currentPlanQty + planQty) - capacity) : currentPlanQty + planQty;

                                                orderWisePlandataGridView.Rows[e.RowIndex].Cells[2].Value = NewDate.ToString("dd/MM/yyyy");
                                                orderWisePlandataGridView.Rows[e.RowIndex].Cells[3].Value = capacity;
                                                orderWisePlandataGridView.Rows[e.RowIndex].Cells[4].Value = remainingQty;
                                                orderWisePlandataGridView.Rows[e.RowIndex].Cells[5].Value = newPlanQty;
                                                orderWisePlandataGridView.Rows[e.RowIndex].Cells[6].Value = minute;
                                                orderWisePlandataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = TaskDateDTP.Text;
                                            }
                                            else
                                            {
                                                MessageBox.Show("This date has no capacity left to add plan quantity!!!");
                                                orderWisePlandataGridView.Rows[e.RowIndex].Cells[2].Value = CurrentTaskDate.ToString("dd/MM/yyyy");
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
                                    else
                                    {
                                        string query = "SELECT Capacity, Minute, (SELECT SUM(PlanQty) FROM PlanTable WHERE MachineNo = " + MachineNo + " AND TaskDate = '" + NewDate + "') As TotalPlanQty"
                                        +" FROM PlanTable WHERE MachineNo = " + MachineNo + " AND TaskDate = '" + NewDate + "' order by TaskDate desc, Id asc";
                                        SqlDataReader reader = CommonFunctions.GetFromDB(query);
                                        try
                                        {
                                            int currentPlanQty = Convert.ToInt32(orderWisePlandataGridView.Rows[e.RowIndex].Cells[5].Value);
                                            if (reader.HasRows)
                                            {
                                                while (reader.Read())
                                                {
                                                    capacity = reader.IsDBNull(reader.GetOrdinal("Capacity")) == true ? 0 : Convert.ToInt32(reader["Capacity"]);
                                                    TotalPlanQty = reader.IsDBNull(reader.GetOrdinal("TotalPlanQty")) == true ? 0 : Convert.ToInt32(reader["TotalPlanQty"]);
                                                    minute = reader.IsDBNull(reader.GetOrdinal("Minute")) == true ? 0 : Convert.ToInt32(reader["Minute"]);
                                                }

                                                if (capacity > TotalPlanQty)
                                                {
                                                    query = "SELECT Count(Id) AS RecordCount, SUM(SAM) AS RestTotalSAM, SUM(Efficiency) AS RestTotalEfficiency FROM PlanTable WHERE MachineNo = " + MachineNo + " AND TaskDate = '" + NewDate + "'";
                                                    SqlDataReader reader1 = CommonFunctions.GetFromDB(query);

                                                    int NewCapacity = 0;
                                                    if (reader1.HasRows)
                                                    {
                                                        while (reader1.Read())
                                                        {
                                                            double RestTotalSAM = reader1.IsDBNull(reader1.GetOrdinal("RestTotalSAM")) == true ? 0.00 : Convert.ToDouble(reader1["RestTotalSAM"]);
                                                            int RestTotalEfficiency = reader1.IsDBNull(reader1.GetOrdinal("RestTotalEfficiency")) == true ? 0 : Convert.ToInt32(reader1["RestTotalEfficiency"]);
                                                            double UpdatedSam = (RestTotalSAM + Convert.ToDouble(samTextBox.Text)) / (Convert.ToInt32(reader1["RecordCount"]) + 1);
                                                            double UpdatedEfficiency = (double)(RestTotalEfficiency + Convert.ToInt32(orderWisePlandataGridView.Rows[e.RowIndex].Cells[7].Value)) / (Convert.ToInt32(reader1["RecordCount"]) + 1);
                                                            NewCapacity = Convert.ToInt32(Math.Floor((double)((minute * (UpdatedEfficiency / 100.00)) / UpdatedSam)));
                                                        }
                                                        if (TotalPlanQty + currentPlanQty <= NewCapacity)
                                                        {
                                                            orderWisePlandataGridView.Rows[e.RowIndex].Cells[2].Value = NewDate.ToString("dd/MM/yyyy");
                                                            orderWisePlandataGridView.Rows[e.RowIndex].Cells[3].Value = NewCapacity;
                                                            orderWisePlandataGridView.Rows[e.RowIndex].Cells[4].Value = TotalPlanQty;
                                                            orderWisePlandataGridView.Rows[e.RowIndex].Cells[5].Value = currentPlanQty;
                                                            orderWisePlandataGridView.Rows[e.RowIndex].Cells[6].Value = minute;
                                                            orderWisePlandataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = TaskDateDTP.Text;
                                                        }
                                                        else
                                                        {
                                                            MessageBox.Show("This date has no capacity left to add plan quantity!!!");
                                                            orderWisePlandataGridView.Rows[e.RowIndex].Cells[2].Value = CurrentTaskDate.ToString("dd/MM/yyyy");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("Failed To Update Capacity!!!");
                                                        orderWisePlandataGridView.Rows[e.RowIndex].Cells[2].Value = CurrentTaskDate.ToString("dd/MM/yyyy");
                                                    }
                                                }
                                                else
                                                {
                                                    MessageBox.Show("This date has no capacity left to add plan quantity!!!");
                                                    orderWisePlandataGridView.Rows[e.RowIndex].Cells[2].Value = CurrentTaskDate.ToString("dd/MM/yyyy");
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show("Failed To Update Capacity!!!");
                                                orderWisePlandataGridView.Rows[e.RowIndex].Cells[2].Value = CurrentTaskDate.ToString("dd/MM/yyyy");
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
                                }
                                else
                                {
                                    int minute = 0;
                                    int efficiency = Convert.ToInt32(orderWisePlandataGridView.Rows[e.RowIndex].Cells[7].Value);
                                    int currentPlanQty = Convert.ToInt32(orderWisePlandataGridView.Rows[e.RowIndex].Cells[5].Value);
                                    double Sam = Convert.ToDouble(samTextBox.Text);
                                    string query = "SELECT Minute FROM WorkingDays WHERE MachineNo =" + MachineNo + " AND WorkDate = '" + NewDate + "'";
                                    SqlDataReader reader = CommonFunctions.GetFromDB(query);
                                    if (reader.HasRows)
                                    {
                                        while(reader.Read())
                                        {
                                            minute = reader.IsDBNull(reader.GetOrdinal("Minute")) == true ? 0 : Convert.ToInt32(reader["Minute"]);
                                        }
                                    }
                                    int capacity = Convert.ToInt32(Math.Floor((double)((minute * (efficiency / 100.00)) / Sam)));
                                    currentPlanQty = capacity < currentPlanQty ? capacity : currentPlanQty;
                                    orderWisePlandataGridView.Rows[e.RowIndex].Cells[3].Value = capacity;
                                    orderWisePlandataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = TaskDateDTP.Text;
                                }
                            }
                            else
                            {
                                MessageBox.Show("Orders can not be placed on Holiday! Please Choose Another Date!!!");
                                orderWisePlandataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = CurrentTaskDate.ToString("dd/MM/yyyy");
                            }
                        }
                        else
                        {
                            MessageBox.Show("This Date has not been scheduled yet for this Machine! Please First Create Schedule this date for this machine!!!");
                            orderWisePlandataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = CurrentTaskDate.ToString("dd/MM/yyyy");
                        }
                    }
                    else if (e.ColumnIndex == 4) // PlanQuantity Column
                    {
                        if (Convert.ToInt32(orderWisePlandataGridView.Rows[e.RowIndex].Cells[3].Value) - Convert.ToInt32(orderWisePlandataGridView.Rows[e.RowIndex].Cells[4].Value) < NewVal)
                        {
                            MessageBox.Show("Plan Quantity can not be greater than remamining Capacity!!!");
                            orderWisePlandataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = PreVal;
                            return;
                        }
                    }
                    else if (e.ColumnIndex == 6) // For Efficiency Column
                    {
                        int newPlanQty = 0;
                        int currentMinute = Convert.ToInt32(orderWisePlandataGridView.Rows[e.RowIndex].Cells[6].Value);
                        int CurrentPlanQty = Convert.ToInt32(orderWisePlandataGridView.Rows[e.RowIndex].Cells[5].Value);
                        NewVal = Convert.ToInt32(orderWisePlandataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                        OrderWisePlanGridRowIndex = e.RowIndex;

                        DateTime sDate = DateTime.ParseExact(orderWisePlandataGridView.Rows[OrderWisePlanGridRowIndex].Cells[2].Value.ToString(), "dd/MM/yyyy", null);
                        string query = "SELECT (SELECT Count(Id) FROM PlanTable WHERE MachineNo = " + MachineNo + " AND TaskDate = '" + sDate + "') As RecordCount, SUM(SAM) AS RestTotalSAM, SUM(Minute) AS RestTotalMinute," + 
                                        "SUM(PlanQty) AS RestTotalRemainingQty, SUM(Efficiency) AS RestTotalEfficiency FROM PlanTable WHERE MachineNo = " + MachineNo + " AND TaskDate = '" + sDate + "' AND OrderID != " + orderID;
                        
                        SqlDataReader reader = CommonFunctions.GetFromDB(query);
                        try
                        {
                            int NewCapacity = 0;
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    int RestTotalMinute = reader.IsDBNull(reader.GetOrdinal("RestTotalMinute")) == true ? 0 : Convert.ToInt32(reader["RestTotalMinute"]);
                                    double RestTotalSAM = reader.IsDBNull(reader.GetOrdinal("RestTotalSAM")) == true ?  0 : Convert.ToDouble(reader["RestTotalSAM"]);
                                    int RestTotalEfficiency = reader.IsDBNull(reader.GetOrdinal("RestTotalEfficiency")) == true ? 0 : Convert.ToInt32(reader["RestTotalEfficiency"]);

                                    int UpdatedMinute = Convert.ToInt32(Math.Floor((Double)((RestTotalMinute + currentMinute) / (Convert.ToInt32(reader["RecordCount"]) + 1))));
                                    double UpdatedSam = (RestTotalSAM + Convert.ToDouble(samTextBox.Text)) / (Convert.ToInt32(reader["RecordCount"]) + 1);
                                    double UpdatedEfficiency = (double)(RestTotalEfficiency + NewVal) / (Convert.ToInt32(reader["RecordCount"]) + 1);

                                    NewCapacity = Convert.ToInt32(Math.Floor((double)((UpdatedMinute * (UpdatedEfficiency / 100.00)) / UpdatedSam)));
                                    int RestTotalRemainingQty = reader.IsDBNull(reader.GetOrdinal("RestTotalRemainingQty")) == true ?  0 : Convert.ToInt32(reader["RestTotalRemainingQty"]);
                                    newPlanQty = CurrentPlanQty + RestTotalRemainingQty > NewCapacity ? NewCapacity - RestTotalRemainingQty : CurrentPlanQty;
                                }
                                orderWisePlandataGridView.Rows[e.RowIndex].Cells[3].Value = NewCapacity;
                                orderWisePlandataGridView.Rows[e.RowIndex].Cells[5].Value = newPlanQty;
                            }
                            else
                            {
                                MessageBox.Show("Failed To Update Capacity!!!");
                                orderWisePlandataGridView.Rows[e.RowIndex].Cells[3].Value = PreVal;
                            }
                        }
                        catch (Exception ee)
                        {
                            MessageBox.Show("" + ee.ToString());
                            orderWisePlandataGridView.Rows[e.RowIndex].Cells[3].Value = PreVal;
                        }
                        finally
                        {
                            if (CommonFunctions.connection.State == ConnectionState.Open)
                            {
                                CommonFunctions.connection.Close();
                            }
                        }
                    }
                }
                ResetOrderWiseePlanTableParameters();
                orderWisePlandataGridView.Rows.RemoveAt(1);
                CalculateOrderWisePlanGridSum();
            }
            else
            {
                if (mcNo.Count > 0 && orderID != 0)
                {
                    string sDate = "";
                    int mc = Convert.ToInt32(orderWisePlandataGridView.Rows[e.RowIndex].Cells[1].Value);
                    int mcIndex = mcNo.IndexOf(mc);

                    if (orderWisePlandataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value == null)
                    {
                        MessageBox.Show("Null Value can not used!!!");
                        orderWisePlandataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = PreVal;
                        ResetOrderWiseePlanTableParameters();
                        return;
                    }
                    if (Convert.ToInt32(orderWisePlandataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value) == PreVal || e.RowIndex != PreIndex)
                    {
                        ResetOrderWiseePlanTableParameters();
                        return;
                    }

                    if (IsValid() == false)
                    {
                        orderWisePlandataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = PreVal;
                        ResetOrderWiseePlanTableParameters();
                        labelAlert.Visible = true;
                        return;
                    }

                    if (orderWisePlandataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null && e.RowIndex > -1 && e.RowIndex < (orderWisePlandataGridView.Rows.Count - 1) && (e.ColumnIndex == 5 || e.ColumnIndex == 7) && OrderWisePlanGridRowIndex == -1 && NewVal < 0)
                    {
                        if (e.ColumnIndex == 7) // For Efficiency Column
                        {
                            LcArray = null;
                            NewVal = Convert.ToInt32(orderWisePlandataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                            OrderWisePlanGridRowIndex = e.RowIndex;
                            LCFlag = true;
                            int temp = 0;
                            int rowCount = orderWisePlandataGridView.Rows.Count - 1;

                            DateTime currentRowDate = DateTime.ParseExact(orderWisePlandataGridView.Rows[OrderWisePlanGridRowIndex].Cells[2].Value.ToString(), "dd/MM/yyyy", null);

                             //= OrderWisePlanGridRowIndex < rowCount - 1 ? DateTime.ParseExact(orderWisePlandataGridView.Rows[OrderWisePlanGridRowIndex + 1].Cells[2].Value.ToString(), "dd/MM/yyyy", null).ToString()
                             //   : toDateTimePicker.Value > currentRowDate ? currentRowDate.AddDays(1).ToString() : "";

                            //if (OrderWisePlanGridRowIndex < rowCount - 1)
                            //{
                            //    sDate = DateTime.ParseExact(orderWisePlandataGridView.Rows[OrderWisePlanGridRowIndex + 1].Cells[2].Value.ToString(), "dd/MM/yyyy", null).ToString();
                            //}
                            //else
                            //{
                            //    sDate = toDateTimePicker.Value > currentRowDate ? currentRowDate.AddDays(1).ToString() : "";
                            //}

                            if (DateTime.ParseExact(orderWisePlandataGridView.Rows[OrderWisePlanGridRowIndex].Cells[2].Value.ToString(), "dd/MM/yyyy", null) < toDateTimePicker.Value)
                            {
                                sDate = DateTime.ParseExact(orderWisePlandataGridView.Rows[OrderWisePlanGridRowIndex].Cells[2].Value.ToString(), "dd/MM/yyyy", null).AddDays(1).ToString();
                                mcNo.RemoveRange(0, mcIndex);
                            }
                            else
                            {
                                if (mcIndex == mcNo.Count - 1)
                                {
                                    sDate = "";
                                }
                                else
                                {
                                    sDate = fromDateTimePicker.Value.ToString();
                                    mcNo.RemoveRange(0, mcIndex + 1);
                                }
                            }

                            int capacity = 0;
                            int CurrentRowBookedQty = Convert.ToInt32(orderWisePlandataGridView.Rows[OrderWisePlanGridRowIndex].Cells[4].Value);
                            int CurrentRowMinute = Convert.ToInt32(orderWisePlandataGridView.Rows[OrderWisePlanGridRowIndex].Cells[6].Value);

                            if (CurrentRowBookedQty > 0)
                            {
                                string query = "SELECT SUM(PlanQty) AS RestPlanQty, (SELECT SUM(SAM) FROM PlanTable WHERE MachineNo = " + MachineNo +
                                            " AND TaskDate = '" + currentRowDate.Date + "' AND OrderID != " + orderID + ") As TotalSAM, Count(*) AS RestEfficiencyNumber, SUM(Efficiency) AS TotalRestEfficiency, SUM(Minute) AS TotalRestMinute, (SELECT TOP 1 Minute FROM WorkingDays WHERE MachineNo = "
                                            + MachineNo + " AND WorkDate = '" + currentRowDate.Date + "') AS Minute, (SELECT TOP 1 Active FROM WorkingDays WHERE MachineNo = "
                                            + MachineNo + " AND WorkDate = '" + currentRowDate.Date + "') AS Active FROM PlanTable WHERE MachineNo = "
                                            + MachineNo + " AND TaskDate = '" + currentRowDate.Date + "'";
                                SqlDataReader reader = CommonFunctions.GetFromDB(query);
                                if (reader.HasRows)
                                {
                                    while (reader.Read())
                                    {
                                        int TotalRestPlanQty = reader.IsDBNull(reader.GetOrdinal("RestPlanQty")) == true ? 0 : (int)reader["RestPlanQty"];
                                        Double TotalRestEfficiency = reader.IsDBNull(reader.GetOrdinal("TotalRestEfficiency")) == true ? 0 : Convert.ToDouble(reader["TotalRestEfficiency"]);
                                        Double RestEfficiencyNumber = reader.IsDBNull(reader.GetOrdinal("RestEfficiencyNumber")) == true ? 0 : Convert.ToDouble(reader["RestEfficiencyNumber"]);
                                        Double TotalSam = reader.IsDBNull(reader.GetOrdinal("TotalSAM")) == true ? 0 + Convert.ToDouble(samTextBox.Text) : (Convert.ToDouble(reader["TotalSAM"]) + Convert.ToDouble(samTextBox.Text));
                                        Double AVGSAM = TotalSam / (RestEfficiencyNumber + 1);
                                        Double AvgEff = (Convert.ToDouble((TotalRestEfficiency + NewVal) / (RestEfficiencyNumber + 1)));

                                        int TotalMinute = reader.IsDBNull(reader.GetOrdinal("TotalRestMinute")) == true ? 0 + Convert.ToInt32(reader["Minute"]) : Convert.ToInt32(reader["TotalRestMinute"]) + Convert.ToInt32(reader["Minute"]);
                                        int AVGMinute = Convert.ToInt32(Math.Floor(TotalMinute / (RestEfficiencyNumber + 1)));

                                        capacity = Convert.ToInt32(reader["Active"]) == 0 ? 0 : (int)Math.Floor((Convert.ToDouble(AVGMinute) * (Convert.ToDouble(AvgEff) / 100.00)) / (AVGSAM));
                                    }
                                }
                            }
                            else
                            {
                                capacity = GetEachCapacity(OrderWisePlanGridRowIndex, CurrentRowMinute, NewVal);
                            }

                            for (int i = rowCount; i >= 0; i--)
                            {
                                if (i < OrderWisePlanGridRowIndex)
                                {
                                    if (Convert.ToBoolean(orderWisePlandataGridView.Rows[i].Cells[8].Value) == false)
                                    {
                                        temp = temp + Convert.ToInt32(orderWisePlandataGridView.Rows[i].Cells[5].Value);
                                    }  
                                }
                                if (i > OrderWisePlanGridRowIndex)
                                {
                                    orderWisePlandataGridView.Rows.RemoveAt(i);
                                }
                            }

                            int UpdatedPlanQty = (Convert.ToInt32(newOrderQtyTextBox.Text) - temp) > (capacity - CurrentRowBookedQty) ? (capacity - CurrentRowBookedQty) : (Convert.ToInt32(newOrderQtyTextBox.Text) - temp);
                            orderWisePlandataGridView.Rows[OrderWisePlanGridRowIndex].Cells[3].Value = capacity;
                            orderWisePlandataGridView.Rows[OrderWisePlanGridRowIndex].Cells[5].Value = UpdatedPlanQty;
                            temp = Convert.ToInt32(newOrderQtyTextBox.Text) - (temp + UpdatedPlanQty);

                            if (sDate != "" && temp != 0)
                            {
                                LoadOrderWisePlanGrid(temp, sDate);
                            }
                            else
                            {
                                CalculateOrderWisePlanGridSum();
                                LCFlag = false;
                            }
                        }
                        else if (e.ColumnIndex == 5) // For Plan Quantity Column
                        {
                            OrderWisePlanGridRowIndex = e.RowIndex;
                            NewVal = Convert.ToInt32(orderWisePlandataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                            if (Convert.ToInt32(orderWisePlandataGridView.Rows[e.RowIndex].Cells[3].Value) - Convert.ToInt32(orderWisePlandataGridView.Rows[e.RowIndex].Cells[4].Value) < 0)
                            {
                                MessageBox.Show("No Qty is Left!!!");
                                orderWisePlandataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = PreVal;
                                ResetOrderWiseePlanTableParameters();
                                return;
                            }
                            if (Convert.ToInt32(orderWisePlandataGridView.Rows[e.RowIndex].Cells[3].Value) - Convert.ToInt32(orderWisePlandataGridView.Rows[e.RowIndex].Cells[4].Value) < NewVal)
                            {
                                MessageBox.Show("Plan Quantity can not be greater than remamining Capacity!!!");
                                orderWisePlandataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = PreVal;
                                ResetOrderWiseePlanTableParameters();
                                return;
                            }
                            if (NewVal < 0)
                            {
                                MessageBox.Show("Negative Value is not permitted!!!");
                                orderWisePlandataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = PreVal;
                                ResetOrderWiseePlanTableParameters();
                                return;
                            }
                            int temp = 0;
                            int sum = 0;
                            int remainingSum = 0;
                            int LastRowIndex = orderWisePlandataGridView.Rows.Count - 1;

                            if (NewVal < 0)
                            {
                                MessageBox.Show("Plan Quantity can not be zero!!!");
                            }

                            else if (mcIndex == mcNo.Count-1 && DateTime.ParseExact(orderWisePlandataGridView.Rows[OrderWisePlanGridRowIndex].Cells[2].Value.ToString(), "dd/MM/yyyy", null) == toDateTimePicker.Value && e.RowIndex == LastRowIndex - 1)
                            {
                                orderWisePlandataGridView.Rows.RemoveAt(LastRowIndex);
                                CalculateOrderWisePlanGridSum();
                            }

                            else if (NewVal > PreVal)
                            {
                                if (NewVal > (int)orderWisePlandataGridView.Rows[e.RowIndex].Cells[3].Value || e.RowIndex == LastRowIndex - 1)
                                {
                                    MessageBox.Show("Plan Quantity can not be greater than capacity!!!");
                                    orderWisePlandataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = PreVal;
                                    ResetOrderWiseePlanTableParameters();
                                }
                                else
                                {
                                    temp = NewVal - PreVal;
                                    foreach (DataGridViewRow row in orderWisePlandataGridView.Rows)
                                    {
                                        if (row.Index > OrderWisePlanGridRowIndex && row.Index != orderWisePlandataGridView.Rows.Count - 1)
                                        {
                                            if ((int)row.Cells[3].Value > 0)
                                            {
                                                remainingSum = remainingSum + Convert.ToInt32(row.Cells[e.ColumnIndex].Value);
                                            }
                                        }
                                    }
                                    if (NewVal > -1)
                                    {
                                        temp = temp > remainingSum ? temp - remainingSum : remainingSum - temp;

                                        if (DateTime.ParseExact(orderWisePlandataGridView.Rows[OrderWisePlanGridRowIndex].Cells[2].Value.ToString(), "dd/MM/yyyy", null) < toDateTimePicker.Value)
                                        {
                                            sDate = DateTime.ParseExact(orderWisePlandataGridView.Rows[OrderWisePlanGridRowIndex].Cells[2].Value.ToString(), "dd/MM/yyyy", null).AddDays(1).ToString();
                                            mcNo.RemoveRange(0, mcIndex);
                                        }
                                        else
                                        {
                                            if (mcIndex == mcNo.Count - 1)
                                            {
                                                sDate = toDateTimePicker.Value.AddDays(1).ToString();
                                            }
                                            else
                                            {
                                                sDate = fromDateTimePicker.Value.ToString();
                                                mcNo.RemoveRange(0, mcIndex + 1);
                                            }
                                        }

                                        Int32 rowCount = orderWisePlandataGridView.Rows.Count - 1;
                                        for (int i = rowCount; i >= 0; i--)
                                        {
                                            if (i > OrderWisePlanGridRowIndex)
                                            {
                                                orderWisePlandataGridView.Rows.RemoveAt(i);
                                            }
                                        }
                                        LoadOrderWisePlanGrid(temp, sDate);
                                    }
                                }
                            }
                            else
                            {
                                if (OrderWisePlanGridRowIndex == e.RowIndex)
                                {
                                    temp = PreVal - NewVal;
                                    foreach (DataGridViewRow row in orderWisePlandataGridView.Rows)
                                    {
                                        if (row.Index > OrderWisePlanGridRowIndex && row.Index != orderWisePlandataGridView.Rows.Count - 1)
                                        {
                                            if (row.Cells[3].Value != "")
                                            {
                                                if ((int)row.Cells[3].Value > 0)
                                                {
                                                    remainingSum = remainingSum + Convert.ToInt32(row.Cells[e.ColumnIndex].Value);
                                                }
                                            }
                                        }
                                    }
                                    if (NewVal > -1)
                                    {
                                        temp = temp + remainingSum;
                                        Int32 rowCount = orderWisePlandataGridView.Rows.Count - 1;
                                        for (int i = rowCount; i >= 0; i--)
                                        {
                                            if (i > OrderWisePlanGridRowIndex)
                                            {
                                                orderWisePlandataGridView.Rows.RemoveAt(i);
                                            }
                                        }
                                        
                                        if (DateTime.ParseExact(orderWisePlandataGridView.Rows[OrderWisePlanGridRowIndex].Cells[2].Value.ToString(), "dd/MM/yyyy", null) < toDateTimePicker.Value)
                                        {
                                            sDate = DateTime.ParseExact(orderWisePlandataGridView.Rows[OrderWisePlanGridRowIndex].Cells[2].Value.ToString(), "dd/MM/yyyy", null).AddDays(1).ToString();
                                            mcNo.RemoveRange(0, mcIndex);
                                        }
                                        else
                                        {
                                            if (mcIndex == mcNo.Count - 1)
                                            {
                                                sDate = toDateTimePicker.Value.AddDays(1).ToString();
                                            }
                                            else
                                            {
                                                sDate = fromDateTimePicker.Value.ToString();
                                                mcNo.RemoveRange(0, mcIndex + 1);
                                            }
                                        }
                                        LoadOrderWisePlanGrid(temp, sDate);
                                    }
                                }
                            }
                            temp = 0;
                            remainingSum = 0;
                        }
                    }
                    ResetOrderWiseePlanTableParameters();
                }
            }
        }

        private void ResetOrderWiseePlanTableParameters()
        {
            PreVal = -1;
            NewVal = -1;
            PreIndex = -1;
            OrderWisePlanGridRowIndex = -1;
            EffList.Clear();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            if (mcNo.Count > 0 && orderID != 0 && AvgCapacity != 0)
            {
                if (LCText.Text != "")
                {
                    string[] values = LCText.Text.Trim().Split(',');
                    if (values.Length > 10)
                    {
                        MessageBox.Show("Learning Curve can not be more than 10 values!!!");
                        return;
                    }
                    LcArray = Array.ConvertAll<string, int>(values, int.Parse);
                    LCFlag = true;
                    LcCount = 0;
                    LoadOrderWisePlanGrid();
                    LCFlag = false;
                }
                else
                {
                    MessageBox.Show("Please Give Learning Curves Value!!!");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Please Select All Criteria!!!");
                return;
            }
        }

        private void LCText_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != ','))
            {
                e.Handled = true;
            }
        }

        private void orderWisePlandataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (Convert.ToBoolean(orderWisePlandataGridView.Rows[e.RowIndex].Cells[8].Value) == true)
                {
                    if (Convert.ToInt32(orderWisePlandataGridView.Rows[e.RowIndex].Cells[6].Value) == 0)
                    {
                        orderWisePlandataGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.IndianRed;
                    }
                    else
                    {
                        orderWisePlandataGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.DarkOliveGreen;
                    }
                }
            }
        }

        private void orderWisePlandataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex > orderWisePlandataGridView.Rows.Count - 2)
            {
                labelShow.Visible = false;
                return;
            }

            int index = e.RowIndex;
            DateTime TaskDate = DateTime.ParseExact(orderWisePlandataGridView.Rows[index].Cells[2].Value.ToString(), "dd/MM/yyyy", null);
            ShowDateWiseOrderHistory(MachineNo, TaskDate);
        }

        private void ShowDateWiseOrderHistory(int MachineNo, DateTime TaskDate)
        {
            try
            {
                string Label = "PreOrdersHistory :- ";
                string query = "SELECT OrderID, Efficiency, SAM FROM PlanTable WHERE MachineNo = " + MachineNo + " AND TaskDate = '" + TaskDate + "'";
                SqlDataReader reader = CommonFunctions.GetFromDB(query);
                if (reader.HasRows)
                {
                    labelShow.Visible = true;
                    while (reader.Read())
                    {
                        int OrderID = Convert.ToInt32(reader["OrderID"]);
                        Double SAM = Convert.ToInt32(reader["SAM"]);
                        Double Efficiency = Convert.ToInt32(reader["Efficiency"]);
                        Label = Label + " OrderID : "+OrderID+", SAM : "+SAM+", Effi : "+Efficiency+"; ";
                    }
                    labelShow.Text = Label;
                    labelShow.Visible = true;
                }
                else
                {
                    labelShow.Visible = false;
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

        private void AutoPlanButton_Click(object sender, System.EventArgs e)
        {
            if (mcNo.Count < 1)
            {
                return;
            }
            if (orderID < 1)
            {
                return;
            }
            if (leftQtyTextBox.Text == "" || LeftQty == 0)
            {
                return;
            }
            AutoFlag = true;
            LoadOrderWisePlanGrid();
        }

        private void leftQtyTextBox_TextChanged(object sender, System.EventArgs e)
        {

        }

        private void deleteToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            String query = "";
            if (PlanBoardDisplayForm.EditMode)
            {
                DeleteOrderForm dltOrderForm = new DeleteOrderForm();
                DialogResult dr = dltOrderForm.ShowDialog();

                if (dr == System.Windows.Forms.DialogResult.OK)
                {
                    int OrderID = Convert.ToInt32(orderInfoDetailsdataGridView.SelectedRows[0].Cells["Id"].Value);
                    if (dltOrderForm.dltSingle)
                    {
                        query = "DELETE FROM PlanTable WHERE MachineNo = " + MachineNo + " AND TaskDate = '" + CurrentTaskDate + "' AND OrderID = " + OrderID;
                    }
                    else
                    {
                        query = "DELETE FROM PlanTable WHERE OrderID = " + OrderID;
                    }
                    Boolean result = CommonFunctions.ExecutionToDB(query, 3);
                    if (result)
                    {
                        MessageBox.Show("Deleted Successully!!!");
                        IDs.RemoveAll(item => item == OrderID);

                        if (OrderID == orderID)
                            orderWisePlandataGridView.Rows.Clear();

                        if (IDs.Count != 0)
                        {
                            PlanBoardDisplayForm.orderIDs = string.Join(",", IDs.ToArray());
                            LoadOrderInfoGridOnEditMode();
                        }
                        else
                        {
                            orderInfoDetailsdataGridView.Rows.Clear();
                        }
                    }
                }
            }
        }

        private void orderInfoDetailsdataGridView_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (PlanBoardDisplayForm.EditMode)
            {
                if (e.RowIndex > -1 && e.Button == MouseButtons.Right)
                {
                    rowIndex = e.RowIndex;
                    orderInfoDetailsdataGridView.Rows[rowIndex].Selected = true;
                    this.OrderInfoPBcontextMenuStrip.Show(this.orderInfoDetailsdataGridView, e.Location);
                    OrderInfoPBcontextMenuStrip.Show(Cursor.Position);
                }
            }
        }

        private void orderWisePlandataGridView_Scroll(object sender, ScrollEventArgs e)
        {
            TaskDateDTP.Visible = false;
        }

        private void orderWisePlandataGridView_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            TaskDateDTP.Visible = false;
        }

        private void MachineComboBox_CheckStateChanged(object sender, EventArgs e)
        {
            mcNo.Clear();
            foreach (CheckComboBoxItem item in MachineComboBox.Items)
            {
                if (item.CheckState == true)
                {
                    mcNo.Add(Convert.ToInt32(item.Text));
                }
            }
            setValue();
        }

        private void MachineComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

    }
}
