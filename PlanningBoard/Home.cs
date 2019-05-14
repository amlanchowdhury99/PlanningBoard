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

namespace PlanningBoard
{
    public partial class Home : Form
    {
        List<Panel> p = new List<Panel>();
        ArrayList mcNo = new ArrayList();
        private bool mouseDown;
        private Point lastLocation;
        private int rowIndex = 0;
        private string PreVal = "";
        private string NewVal = "";
        private Boolean LCFlag = false;
        List<int> LcArray = new List<int>();

        public static Dictionary<int, int> MachineList = new Dictionary<int, int>();
        public static Dictionary<int, string> BuyerList = new Dictionary<int, string>();
        public static Dictionary<int, string> StyleList = new Dictionary<int, string>();
        public static Dictionary<int, string> DiaList = new Dictionary<int, string>();
        public static Dictionary<int, string> SizeList = new Dictionary<int, string>();
        public static Dictionary<int, string> PartList = new Dictionary<int, string>();

        public Home()
        {
            InitializeComponent();
            p.Add(panel1);
            p.Add(panel3);
            p.Add(panel4);
            p.Add(HomePanel);

        }

        private void Home_Load(object sender, EventArgs e)
        {
            p[0].Hide();
            p[1].Hide();
            p[2].Hide();
            p[3].Show();
            p[3].BringToFront();
            //fromDateTimePicker.Value = DateTime.Now;
            //toDateTimePicker.Value = DateTime.Now;
            //fromDateTimePicker.Enabled = true;
            //toDateTimePicker.Enabled = true;
            //machineNoComboBox.Enabled = true;
            //GridInitialize();
            //Load_WorkingDays_ComboBox();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void machineInfoEntryButton_Click(object sender, EventArgs e)
        {
            p[0].Show();
            p[0].BringToFront();
            p[1].Hide();
            p[2].Hide();
            p[3].Hide();
            LoadMachineInfoGrid();
            LoadDiaMachine();
            ResetMachineInfo();

        }

        private void orderInfoEntryButton_Click(object sender, EventArgs e)
        {
            orderInfoWarningLbl.Visible = false;
            mcNo.Clear();
            p[1].Show();
            p[1].BringToFront();
            p[0].Hide();
            p[2].Hide();
            p[3].Hide();
            shipDatePicker.CustomFormat = "dd/MM/yyyy";
            shipDatePicker.Value = DateTime.Now.Date;
            planDateTimePicker.CustomFormat = "dd/MM/yyyy";
            planDateTimePicker.Value = DateTime.Now.Date;
            LoadComboBox();
            LoadOrderInfoGrid();
            MachineComboBox.CheckStateChanged += new System.EventHandler(MachineComboBox_CheckStateChanged);
            ResetOrderInfo();
        }

        public void ShowOrderInfo()
        {
            this.orderInfoEntryButton.PerformClick();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void Home_MouseHover(object sender, EventArgs e)
        {

        }

        private void Home_MouseLeave(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void machineEntryForm_Load(object sender, EventArgs e)
        {
            ResetMachineInfo();
        }

        private void ResetWorkingDays()
        {
            GridInitialize();
            Grid_WorkDays_Info.Rows.Clear();
            fromDateTimePicker.Value = DateTime.Now;
            toDateTimePicker.Value = DateTime.Now;
            fromDateTimePicker.Enabled = true;
            toDateTimePicker.Enabled = true;
            machineNoComboBox.Enabled = true;
            Load_WorkingDays_ComboBox();
        }

        private void GenerateWorkingDays_Click(object sender, EventArgs e)
        {
            GridInitialize();
            Grid_WorkDays_Info.Rows.Clear();
            p[2].Show();
            p[2].BringToFront();
            p[0].Hide();
            p[1].Hide();
            p[3].Hide();
            ResetWorkingDays();
            Load_WorkingDays_ComboBox();
            labelAlert.Visible = false;


        }

        private void Home_MouseEnter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string connectionStr = ConnectionManager.connectionString;
            SqlCommand cm = new SqlCommand(); SqlConnection cn = new SqlConnection(connectionStr); cm.Connection = cn; cn.Open();
            cm.CommandText = "DELETE FROM PlanTable WHERE PlanQty = 0 AND Production = 1";
            cm.ExecuteNonQuery();
            cn.Close();

            PlanBoardDisplayForm planfrm = new PlanBoardDisplayForm();
            planfrm.ShowDialog();
        }

        private void SetValue_machineInfo(int rowIndex)
        {
            DataGridViewRow row = machineInfoDataGridView.Rows[rowIndex];

            string mcNo = row.Cells[1].Value.ToString();

            Boolean result = true;

            String query = "SELECT SUM(PlanQty) AS TotalPlanQty, (SELECT SUM(OrderQty) FROM (SELECT DISTINCT OrderID, OrderQty FROM PlanTable WHERE TaskDate >= '" + DateTime.Now.AddDays(-7) + "' AND MachineNo = " + Convert.ToInt32(mcNo) + " GROUP BY OrderID,OrderQty) as e ) AS TotalOrderQty, SUM(ActualQty) AS TotalActualQty FROM PlanTable WHERE TaskDate >= '" + DateTime.Now.AddDays(-7) + "' AND MachineNo = " + Convert.ToInt32(mcNo);
            SqlDataReader reader = CommonFunctions.GetFromDB(query);
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int TotalPlanQty = reader.IsDBNull(reader.GetOrdinal("TotalPlanQty")) == true ? 0 : Convert.ToInt32(reader["TotalPlanQty"]);
                    int TotalActualQty = reader.IsDBNull(reader.GetOrdinal("TotalActualQty")) == true ? 0 : Convert.ToInt32(reader["TotalActualQty"]);
                    int TotalOrderQty = reader.IsDBNull(reader.GetOrdinal("TotalOrderQty")) == true ? 0 : Convert.ToInt32(reader["TotalOrderQty"]);
                    result = TotalOrderQty == TotalPlanQty ? true : false;
                }
            }

            if (result)
            {
                MNotextBox.Text = row.Cells[1].Value.ToString();
                DiaCombo.Text = row.Cells[2].Value.ToString();

                if (row.Cells[3].Value == VariableDecleration_Class.Status.Active.ToString())
                {
                    MStatuscomboBox.SelectedIndex = 1;
                }
                else
                {
                    MStatuscomboBox.SelectedIndex = 0;
                }
                MNotextBox.ReadOnly = true;
            }
            else
            {
                MessageBox.Show("This Machine can not be edited! It has already been used in PlanBoard! For Editing Delete from PlanBoard First!!!");
                return;
            }
        }

        private void SaveMachineInfo_Click(object sender, EventArgs e)
        {
            ResetMachineInfo();
        }

        private void ResetMachineInfo()
        {
            MNotextBox.ReadOnly = false;
            MNotextBox.Text = "";
            DiaCombo.SelectedIndex = 0;
            MStatuscomboBox.SelectedIndex = 1;
        }

        private void UpdateMachineInfo_Click(object sender, EventArgs e)
        {
            try
            {
                if (MNotextBox.Text.Trim() == "")
                {
                    MessageBox.Show("Please Enter Machine No", VariableDecleration_Class.sMSGBOX);
                    MNotextBox.Focus();
                    return;
                }

                if (DiaCombo.SelectedIndex == 0)
                {
                    MessageBox.Show("Please Enter Machine Dia Value", VariableDecleration_Class.sMSGBOX);
                    DiaCombo.Focus();
                    return;
                }

                int MachineNo = Convert.ToInt32(MNotextBox.Text);
                int MachineDiaKey = ((KeyValuePair<int, string>)DiaCombo.SelectedItem).Key;
                string MachineDiaValue = ((KeyValuePair<int, string>)DiaCombo.SelectedItem).Value;
                int MachineStatus;

                if (MStatuscomboBox.Text.Trim() == "In-Active")
                {
                    MachineStatus = Convert.ToInt32(VariableDecleration_Class.Status.InActive);
                }
                else
                {
                    MachineStatus = Convert.ToInt32(Enum.Parse(typeof(VariableDecleration_Class.Status), MStatuscomboBox.Text));
                }

                string query = " IF NOT EXISTS (SELECT * FROM Dia WHERE Dia = '" + MachineDiaValue + "' ) INSERT INTO Dia(Dia) VALUES ('" + MachineDiaValue + "') ";

                if (CommonFunctions.ExecutionToDB(query, 3))
                {
                    CommonFunctions.connection.Close();

                    query = " IF EXISTS (SELECT * FROM Machine_Info WHERE MachineNo = " + MachineNo + ") UPDATE Machine_Info SET MachineDia = '" + MachineDiaKey + "', Status = " + MachineStatus + " WHERE MachineNo = " + MachineNo +
                                " ELSE INSERT INTO Machine_Info(MachineNo, MachineDia, Status) VALUES (" + MachineNo + ",'" + MachineDiaKey + "'," + MachineStatus + ") ";

                    if (CommonFunctions.ExecutionToDB(query, 1))
                    {
                        LoadMachineInfoGrid();
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
                ResetMachineInfo();
            }
        }

        private void LoadMachineInfoGrid()
        {
            try
            {
                int SL = 1;
                int S1 = 0; int S2 = 0; string S3 = ""; string S4 = "";

                string query = "SELECT * FROM Machine_Info a, Dia b WHERE a.MachineDia = b.Id order by MachineNo";

                SqlDataReader reader = CommonFunctions.GetFromDB(query);
                machineInfoDataGridView.Rows.Clear();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        S1 = SL;
                        S2 = reader.IsDBNull(reader.GetOrdinal("MachineNo")) == true ? 0000 : Convert.ToInt32(reader["MachineNo"]);
                        S3 = reader.IsDBNull(reader.GetOrdinal("Dia")) == true ? "Not Defined Yet" : (reader["Dia"]).ToString();
                        S4 = Convert.ToInt32(reader["Status"]) == 0 ? "In-Active" : VariableDecleration_Class.Status.Active.ToString();

                        machineInfoDataGridView.Rows.Add(S1, S2, S3, S4);
                        SL++;
                    }
                }

                MStatuscomboBox.DisplayMember = "Description";
                MStatuscomboBox.ValueMember = "Value";
                MStatuscomboBox.DataSource = Enum.GetValues(typeof(VariableDecleration_Class.Status)).Cast<VariableDecleration_Class.Status>().Where(e => e != VariableDecleration_Class.Status.Pending && e != VariableDecleration_Class.Status.Complete).Cast<Enum>().Select(value => new { (Attribute.GetCustomAttribute(value.GetType().GetField(value.ToString()), typeof(DescriptionAttribute)) as DescriptionAttribute).Description, value }).OrderBy(item => item.value).ToList();
                MStatuscomboBox.SelectedIndex = 1;
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

        private void MNotextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void MDiatextBox_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void MStatuscomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void machineInfoDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void LoadOrderInfoGrid()
        {
            try
            {
                string query = "";
                string subquery = "SELECT * FROM Order_Info ";
                int SL = 1;
                int S1 = 1; string S2 = ""; string S3 = ""; string S4 = ""; string S5 = ""; string S6 = ""; string S7 = ""; string S8 = ""; string S9 = ""; string S10 = ""; string S11 = ""; string S12 = "";

                if (buyerComboBox.SelectedIndex < 0 || styleComboBox.SelectedIndex < 0)
                {
                    return;
                }

                string buyerName = buyerComboBox.SelectedIndex == 0 ? "" : ((KeyValuePair<int, string>)buyerComboBox.SelectedItem).Key.ToString();
                string styleName = styleComboBox.SelectedIndex == 0 ? "" : ((KeyValuePair<int, string>)styleComboBox.SelectedItem).Key.ToString();
                //int sizeNo = ((KeyValuePair<int, string>)sizeComboBox.SelectedItem).Key;
                //int dia = ((KeyValuePair<int, string>)diaComboBox.SelectedItem).Key;
                //int bodyPart = ((KeyValuePair<int, string>)partComboBox.SelectedItem).Key;

                if (hiddenIDtextBox.Text == "")
                {
                    if (buyerName != "")
                    {
                        subquery = subquery + " WHERE Buyer = " + buyerName;
                    }
                    if (styleName != "")
                    {
                        subquery = subquery.Contains("WHERE") == true ? subquery + " AND Style = " + styleName : subquery + " WHERE Style = " + styleName;
                    }
                }



                query = " SELECT a.Id, a.PurchaseOrderNo, a.Quantity, a.ShipmentDate, a.SAM, a.Efficiency, a.Status, b.BuyerName, c.StyleName, d.SizeName, e.Dia, f.PartName FROM (" + subquery + ") a, Buyer b, Style c, Size d, Dia e, BodyPart f " +
                    "WHERE a.Buyer = b.Id AND a.Style = c.Id AND a.Size = d.Id AND a.Dia = e.Id AND a.BodyPart = f.Id order by ShipmentDate asc";

                SqlDataReader reader = CommonFunctions.GetFromDB(query);

                if (reader.HasRows)
                {
                    orderInfoDetailsdataGridView.Rows.Clear();
                    while (reader.Read())
                    {
                        S1 = SL;
                        S2 = reader.IsDBNull(reader.GetOrdinal("BuyerName")) == true ? "Not Defined" : reader["BuyerName"].ToString();
                        S3 = reader.IsDBNull(reader.GetOrdinal("StyleName")) == true ? "Not Defined" : reader["StyleName"].ToString();
                        S4 = reader.IsDBNull(reader.GetOrdinal("SizeName")) == true ? "Not Defined" : reader["SizeName"].ToString();
                        S5 = reader.IsDBNull(reader.GetOrdinal("Dia")) == true ? "Not Defined" : reader["Dia"].ToString();
                        S6 = reader.IsDBNull(reader.GetOrdinal("PartName")) == true ? "Not Defined" : reader["PartName"].ToString();
                        S7 = reader.IsDBNull(reader.GetOrdinal("PurchaseOrderNo")) == true ? "Not Defined" : reader["PurchaseOrderNo"].ToString();
                        S8 = reader.IsDBNull(reader.GetOrdinal("Quantity")) == true ? "0000" : Convert.ToString(reader["Quantity"]);
                        S9 = reader.IsDBNull(reader.GetOrdinal("ShipmentDate")) == true ? "0/0/0000" : Convert.ToDateTime(reader["ShipmentDate"]).ToString("dd/MM/yyyy");
                        S10 = reader.IsDBNull(reader.GetOrdinal("SAM")) == true ? "0.00" : Convert.ToString(reader["SAM"]);
                        S11 = reader.IsDBNull(reader.GetOrdinal("Efficiency")) == true ? "0.00" : Convert.ToString(reader["Efficiency"]);
                        S12 = Enum.GetName(typeof(VariableDecleration_Class.Status), reader.IsDBNull(reader.GetOrdinal("Status")) == true ? Convert.ToInt32(VariableDecleration_Class.Status.Pending) : Convert.ToInt32(reader["Status"]));

                        orderInfoDetailsdataGridView.Rows.Add(S1, S2, S3, S4, S5, S6, S7, S8, S9, S10, S11, S12, Convert.ToInt32(reader["Id"]));
                        SL++;
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
                SaveOrderInfo.Enabled = true;
            }
        }

        private void LoadComboBox()
        {
            LoadBuyer();
            LoadStyle();
            LoadDia();
            LoadSize();
            LoadPart();
            LoadStatus();
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

        private void LoadStatus()
        {

        }

        private void LoadSize()
        {
            try
            {
                string query = "SELECT * FROM Size order by SizeName asc";

                SqlDataReader reader = CommonFunctions.GetFromDB(query);
                if (reader.HasRows)
                {
                    if (SizeList.Count == 0)
                    {
                        SizeList.Add(0, "-- Select Size--");
                    }

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
                string query = "SELECT * FROM BodyPart order by PartName asc";

                SqlDataReader reader = CommonFunctions.GetFromDB(query);
                if (reader.HasRows)
                {
                    if (PartList.Count == 0)
                    {
                        PartList.Add(0, "-- Select Part--");
                    }
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
                string query = "SELECT * FROM Buyer order by BuyerName asc";

                SqlDataReader reader = CommonFunctions.GetFromDB(query);
                if (reader.HasRows)
                {
                    if (BuyerList.Count == 0)
                    {
                        BuyerList.Add(0, "-- Select Buyer--");
                    }
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
                    buyerComboBox.SelectedIndex = 0;
                    //buyerComboBox.Items.Clear();
                    //buyerComboBox.DataSource = Buyer;
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
                string query = "SELECT * FROM Style order by StyleName asc";

                SqlDataReader reader = CommonFunctions.GetFromDB(query);
                if (reader.HasRows)
                {
                    if (StyleList.Count == 0)
                    {
                        StyleList.Add(0, "-- Select Style--");
                    }
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
                string query = "SELECT * FROM Dia order by Dia asc";

                SqlDataReader reader = CommonFunctions.GetFromDB(query);
                if (reader.HasRows)
                {
                    if (DiaList.Count == 0)
                    {
                        DiaList.Add(0, "-- Select Dia--");
                    }
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

        private void LoadDiaMachine()
        {
            try
            {
                string query = "SELECT * FROM Dia order by Dia asc";

                SqlDataReader reader = CommonFunctions.GetFromDB(query);
                if (reader.HasRows)
                {
                    if (DiaList.Count == 0)
                    {
                        DiaList.Add(0, "-- Select Dia--");
                    }
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
                    DiaCombo.DataSource = new BindingSource(DiaList, null);
                    DiaCombo.DisplayMember = "Value";
                    DiaCombo.ValueMember = "Key";
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

        private void SaveOrderInfo_Click(object sender, EventArgs e)
        {
            try
            {
                //if (pOTextBox.Text == "")
                //{
                //    MessageBox.Show("Please Enter Purchase Order No", VariableDecleration_Class.sMSGBOX);
                //    pOTextBox.Focus();
                //    return;
                //}
                if (buyerComboBox.SelectedIndex == 0)
                {
                    MessageBox.Show("Please Enter Buyer Name", VariableDecleration_Class.sMSGBOX);
                    buyerComboBox.Focus();
                    return;
                }

                if (styleComboBox.SelectedIndex == 0)
                {
                    MessageBox.Show("Please Enter Style Name", VariableDecleration_Class.sMSGBOX);
                    styleComboBox.Focus();
                    return;
                }

                if (sizeComboBox.SelectedIndex == 0)
                {
                    MessageBox.Show("Please Enter Size", VariableDecleration_Class.sMSGBOX);
                    sizeComboBox.Focus();
                    return;
                }

                if (diaComboBox.SelectedIndex == 0)
                {
                    MessageBox.Show("Please Enter Dia Name", VariableDecleration_Class.sMSGBOX);
                    diaComboBox.Focus();
                    return;
                }

                if (partComboBox.SelectedIndex == 0)
                {
                    MessageBox.Show("Please Enter Part Info", VariableDecleration_Class.sMSGBOX);
                    partComboBox.Focus();
                    return;
                }

                if (pOTextBox.Text.Trim() == "")
                {
                    MessageBox.Show("Please Enter Purchase Order Number", VariableDecleration_Class.sMSGBOX);
                    pOTextBox.Focus();
                    return;
                }

                if (qtyTextBox.Text.Trim() == "")
                {
                    MessageBox.Show("Please Enter Quantity Value", VariableDecleration_Class.sMSGBOX);
                    qtyTextBox.Focus();
                    return;
                }

                if (samTextBox.Text.Trim() == "")
                {
                    MessageBox.Show("Please Enter SAM Value", VariableDecleration_Class.sMSGBOX);
                    samTextBox.Focus();
                    return;
                }

                if (shipDatePicker.Text.Trim() == "")
                {
                    MessageBox.Show("Please Enter Date ", VariableDecleration_Class.sMSGBOX);
                    shipDatePicker.Focus();
                    return;
                }

                if (effTextBox.Text.Trim() == "")
                {
                    MessageBox.Show("Please Enter Efficiency Value", VariableDecleration_Class.sMSGBOX);
                    effTextBox.Focus();
                    return;
                }

                if (orderWisePlandataGridView.Rows.Count > 0 && newOrderQtyTextBox.Text.Trim() == "")
                {
                    MessageBox.Show("Please Enter New OrderQty Value", VariableDecleration_Class.sMSGBOX);
                    newOrderQtyTextBox.Focus();
                    return;
                }

                if (orderWisePlandataGridView.Rows.Count == 0 && addToProductioncheckBox.Checked == true)
                {
                    MessageBox.Show("Table has no rows to add in PlanTable Database !!!", VariableDecleration_Class.sMSGBOX);
                    addToProductioncheckBox.Focus();
                    return;
                }

                if (orderInfoID.Text != "" && !CommonFunctions.recordExist("SELECT * FROM Order_Info WHERE Status = 2 AND Id = " + Convert.ToInt32(orderInfoID.Text)))
                {
                    MessageBox.Show("This order is not yet Activated!!! Please Active this order!!!");
                    return;
                }

                if (shipDatePicker.Value > CHD.Value)
                {
                    MessageBox.Show("Knit Closing Date can not be greater than shipment date!!!");
                    return;
                }

                string purchaseOrderNumber = pOTextBox.Text;
                int buyerName = ((KeyValuePair<int, string>)buyerComboBox.SelectedItem).Key;
                int styleName = ((KeyValuePair<int, string>)styleComboBox.SelectedItem).Key;
                int sizeNo = ((KeyValuePair<int, string>)sizeComboBox.SelectedItem).Key;
                int dia = ((KeyValuePair<int, string>)diaComboBox.SelectedItem).Key;
                int bodyPart = ((KeyValuePair<int, string>)partComboBox.SelectedItem).Key;
                double qty = Convert.ToDouble(qtyTextBox.Text);
                DateTime shipdate = DateTime.ParseExact(shipDatePicker.Text, "dd/MM/yyyy", null).Date;
                DateTime chd = DateTime.ParseExact(CHD.Text, "dd/MM/yyyy", null).Date;
                double SAMNo = Convert.ToDouble(samTextBox.Text);
                int eff = Convert.ToInt32(effTextBox.Text);
                int status = Convert.ToInt32((VariableDecleration_Class.Status.Pending));
                string Remarks = remarkTextBox.Text;
                int orderQty = Convert.ToInt32(qtyTextBox.Text);
                string query = "";
                string connectionStr = ConnectionManager.connectionString; SqlConnection cn1 = new SqlConnection(connectionStr); SqlCommand cm1 = new SqlCommand(); cm1.Connection = cn1; cn1.Open();

                query = " (SELECT Count(*) FROM Order_Info WHERE Buyer = " + buyerName + " AND Style = " + styleName + " AND Size = " + sizeNo + " AND Dia = " + dia + " AND BodyPart = " + bodyPart + " AND PurchaseOrderNo = '" + purchaseOrderNumber + "' )";

                if (hiddenIDtextBox.Text.Trim() != "")
                {
                    int Id1 = 0;
                    query = "SELECT Id FROM Order_Info WHERE Buyer = " + buyerName + " AND Style = " + styleName + " AND Size = " + sizeNo + " AND Dia = " + dia + " AND BodyPart = " + bodyPart + " AND PurchaseOrderNo = '" + purchaseOrderNumber + "'";
                    cm1.CommandText = query;
                    SqlDataReader reader = cm1.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Id1 = Convert.ToInt32(reader["Id"]);
                        }
                    }
                    cn1.Close();

                    if (Id1 == Convert.ToInt32(hiddenIDtextBox.Text))
                    {
                        query = "UPDATE Order_Info SET Buyer = " + buyerName + ", Style = " + styleName + ", Size = " + sizeNo + ", Dia = " + dia + ", BodyPart = " + bodyPart + ", PurchaseOrderNo = '" + purchaseOrderNumber + "', Quantity = " + qty + ", ShipmentDate = '" + shipdate + "', CHD = '" + chd + "', SAM = " + SAMNo + ", Efficiency = " + eff + ", Status = " + status + " WHERE Id = " + Convert.ToInt32(hiddenIDtextBox.Text);
                        if (CommonFunctions.ExecutionToDB(query, 2))
                        {
                            LoadOrderInfoGrid();
                        }
                    }
                    else
                    {
                        query = "SELECT Id FROM Order_Info WHERE Buyer = " + buyerName + " AND Style = " + styleName + " AND Size = " + sizeNo + " AND Dia = " + dia + " AND BodyPart = " + bodyPart + " AND PurchaseOrderNo = '" + purchaseOrderNumber + "'";
                        if (!CommonFunctions.recordExist(query))
                        {
                            query = "INSERT INTO Order_Info(Buyer, Style, Size, Dia, BodyPart, Quantity, ShipmentDate, CHD, SAM, Efficiency, Status, Remarks, PurchaseOrderNo) VALUES (" + buyerName + "," + styleName + "," + sizeNo + "," + dia + "," + bodyPart + "," + qty + ",'" + shipdate + "','" + chd + "'," + SAMNo + "," + eff + "," + status + ", '" + Remarks + "', '" + purchaseOrderNumber + "')";
                            if (CommonFunctions.ExecutionToDB(query, 1))
                            {
                                LoadOrderInfoGrid();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Can not update another order while using this order!!!");
                            return;
                        }
                    }
                }
                else
                {
                    query = "SELECT Id FROM Order_Info WHERE Buyer = " + buyerName + " AND Style = " + styleName + " AND Size = " + sizeNo + " AND Dia = " + dia + " AND BodyPart = " + bodyPart + " AND PurchaseOrderNo = '" + purchaseOrderNumber + "'";
                    if (!CommonFunctions.recordExist(query))
                    {
                        query = "INSERT INTO Order_Info(Buyer, Style, Size, Dia, BodyPart, Quantity, ShipmentDate, CHD, SAM, Efficiency, Status, Remarks, PurchaseOrderNo) VALUES (" + buyerName + "," + styleName + "," + sizeNo + "," + dia + "," + bodyPart + "," + qty + ",'" + shipdate + "','" + chd + "'," + SAMNo + "," + eff + "," + status + ", '" + Remarks + "', '" + purchaseOrderNumber + "')";
                        if (CommonFunctions.ExecutionToDB(query, 1))
                        {
                            LoadOrderInfoGrid();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Duplicate Record Exist!!!");
                        return;
                    }

                    //query = " (SELECT Count(*) FROM Order_Info WHERE Buyer = " + buyerName + " AND Style = " + styleName + " AND Size = " + sizeNo + " AND Dia = " + dia + " AND BodyPart = " + bodyPart + " AND PurchaseOrderNo = '" + purchaseOrderNumber + "' )";
                    
                    //if (!CommonFunctions.GetNumberForRows(query))
                    //{
                    //    return;
                    //}
                }

                if (orderWisePlandataGridView.Rows.Count > 0 && addToProductioncheckBox.Checked == true)
                {
                    int planQty = Convert.ToInt32(newOrderQtyTextBox.Text);
                    int orderID = 0; Boolean result = false;
                    SqlConnection cn = new SqlConnection(connectionStr); SqlCommand cm = new SqlCommand(); cm.Connection = cn; cn.Open();
                    cm.CommandText = "SELECT Id From Order_Info WHERE Buyer = " + buyerName + " AND Style = " + styleName + " AND Size = " + sizeNo + " AND Dia = " + dia + " AND BodyPart = " + bodyPart + " AND PurchaseOrderNo = '" + purchaseOrderNumber + "'";
                    SqlDataReader reader1 = cm.ExecuteReader();
                    if (reader1.HasRows)
                    {
                        while (reader1.Read())
                        {
                            orderID = Convert.ToInt32(reader1["Id"]);
                        }
                    }
                    cn.Close();
                    foreach (DataGridViewRow row in orderWisePlandataGridView.Rows)
                    {
                        if (row.Index != orderWisePlandataGridView.Rows.Count - 1)
                        {
                            if (Convert.ToInt32(row.Cells[3].Value) > 0 && Convert.ToInt32(row.Cells[5].Value) > 0)
                            {
                                int machineNo = Convert.ToInt32(row.Cells[1].Value);
                                int capacity = Convert.ToInt32(row.Cells[3].Value);
                                int bookedQty = Convert.ToInt32(row.Cells[4].Value);
                                int plnQty = Convert.ToInt32(row.Cells[5].Value);
                                int remainQty = Convert.ToInt32(row.Cells[3].Value) - (bookedQty + plnQty);
                                remainQty = remainQty < 0 ? 0 : remainQty;
                                int efficiency = Convert.ToInt32(row.Cells[8].Value);
                                int minute = Convert.ToInt32(row.Cells[7].Value);
                                int remainingMinute = (int)Math.Floor(remainQty * Convert.ToDouble(samTextBox.Text));
                                DateTime taskDate = DateTime.ParseExact(row.Cells[2].Value.ToString(), "dd/MM/yyyy", null);
                                int active = Convert.ToInt32(row.Cells[3].Value) < 1 ? 0 : 1;
                                query = "INSERT INTO PlanTable (MachineNo, TaskDate, OrderID, Capacity, PlanQty, RemainingQty, RemainingMinute, OrderQty, Efficiency, SAM, Minute, RevertVal, ActualQty, Status, Production) " +
                                        "VALUES (" + machineNo + ",'" + taskDate + "'," + orderID + "," + capacity + "," + plnQty + "," + remainQty + "," + remainingMinute + "," + orderQty + "," + efficiency + "," + Convert.ToDouble(samTextBox.Text) + "," + minute + ", 0, 0, 0, 1)";
                                result = CommonFunctions.ExecutionToDB(query, 3);
                            }
                        }
                    }
                    if (result == true)
                    {
                        MessageBox.Show("Added to PlanTable Successfully!!!");
                        orderWisePlandataGridView.Rows.Clear();
                        return;
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
                pOTextBox.ReadOnly = false;
                buyerComboBox.Enabled = true;
                styleComboBox.Enabled = true;
            }
        }

        private void UpdateOrderInfo_Click(object sender, EventArgs e) // RESET
        {
            ResetOrderInfo();
        }

        private void ResetOrderInfo()
        {
            try
            {
                LoadComboBox();
                LoadOrderInfoGrid();
            }

            catch (Exception ee)
            {
                MessageBox.Show("" + ee.ToString());
            }

            finally
            {
                pOTextBox.ReadOnly = false;
                buyerComboBox.Enabled = true;
                styleComboBox.Enabled = true;
                sizeComboBox.Enabled = true;
                diaComboBox.Enabled = true;
                partComboBox.Enabled = true;
                effTextBox.ReadOnly = false;

                pOTextBox.Text = "";
                qtyTextBox.Text = "";
                samTextBox.Text = "";
                effTextBox.Text = "";
                newOrderQtyTextBox.Text = "";
                newDaysTextBox.Text = "";
                remarkTextBox.Text = "";
                LCText.Text = "";
                addToProductioncheckBox.Checked = true;
                orderWisePlandataGridView.Rows.Clear();
                shipDatePicker.Value = DateTime.Now;
                planDateTimePicker.Value = DateTime.Now;
                startDateTimePicker.Value = DateTime.Now;
                endDateTimePicker.Value = DateTime.Now;
                CHD.Value = DateTime.Now;

                AddBuyer.Enabled = true;
                AddStyle.Enabled = true;
                qtyTextBox.Text = "";
                samTextBox.Text = "";
                shipDatePicker.Value = DateTime.Now.Date;
                sizeComboBox.Text = "";
                effTextBox.Text = "";
                hiddenIDtextBox.Text = "";
                orderInfoID.Text = "";
                mcNo.Clear();
            }
        }

        private void SetValue_orderInfo(int rowIndex, bool OrderUsed)
        {
            SaveOrderInfo.Enabled = true;
            DataGridViewRow row = orderInfoDetailsdataGridView.Rows[rowIndex];

            pOTextBox.Text = row.Cells[6].Value.ToString();
            buyerComboBox.Text = row.Cells[1].Value.ToString();
            styleComboBox.Text = row.Cells[2].Value.ToString();
            sizeComboBox.Text = row.Cells[3].Value.ToString();
            diaComboBox.Text = row.Cells[4].Value.ToString();
            partComboBox.Text = row.Cells[5].Value.ToString();
            qtyTextBox.Text = row.Cells[7].Value.ToString();
            shipDatePicker.Value = DateTime.ParseExact(row.Cells[8].Value.ToString(), "dd/MM/yyyy", null);
            samTextBox.Text = row.Cells[9].Value.ToString();
            effTextBox.Text = row.Cells[10].Value.ToString();

            int buyerName = ((KeyValuePair<int, string>)buyerComboBox.SelectedItem).Key;
            int styleName = ((KeyValuePair<int, string>)styleComboBox.SelectedItem).Key;
            int sizeNo = ((KeyValuePair<int, string>)sizeComboBox.SelectedItem).Key;
            int dia = ((KeyValuePair<int, string>)diaComboBox.SelectedItem).Key;
            int bodyPart = ((KeyValuePair<int, string>)partComboBox.SelectedItem).Key;

            string connectionStr = ConnectionManager.connectionString; SqlDataReader reader;
            SqlCommand cm = new SqlCommand(); SqlConnection cn = new SqlConnection(connectionStr); cm.Connection = cn; cn.Open();
            cm.CommandText = "SELECT CHD,PurchaseOrderNo FROM Order_Info WHERE ID = " + Convert.ToInt32(hiddenIDtextBox.Text);
            reader = cm.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    CHD.Value = reader.IsDBNull(reader.GetOrdinal("CHD")) == true ? shipDatePicker.Value : Convert.ToDateTime(reader["CHD"].ToString());
                    pOTextBox.Text = CHD.Text;
                }
            }
            else
            {
                CHD.Value = shipDatePicker.Value;
            }
            cn.Close();

            if (OrderUsed)
            {
                buyerComboBox.Enabled = false;
                styleComboBox.Enabled = false;
                sizeComboBox.Enabled = false;
                diaComboBox.Enabled = false;
                partComboBox.Enabled = false;
                effTextBox.ReadOnly = true;
            }
            else
            {
                pOTextBox.ReadOnly = true;
                buyerComboBox.Enabled = false;
                styleComboBox.Enabled = false;
                sizeComboBox.Enabled = true;
                diaComboBox.Enabled = true;
                partComboBox.Enabled = true;
                effTextBox.ReadOnly = false;
            }
            orderInfoID.Text = hiddenIDtextBox.Text;
            //try
            //{
            //    string query = "SELECT Top 1* FROM Order_Info WHERE Buyer = " + buyerName + " AND Style = " + styleName + " AND Size = " + sizeNo + " AND Dia = " + dia + " AND BodyPart = " + bodyPart + "";
            //    SqlDataReader reader = CommonFunctions.GetFromDB(query);
            //    if (reader.HasRows)
            //    {
            //        while (reader.Read())
            //        {
            //            hiddenIDtextBox.Text = reader["Id"].ToString();
            //        }
            //    }
            //}
            //catch (Exception e)
            //{
            //    MessageBox.Show("" + e.ToString());
            //}

            //finally
            //{
            //    if (CommonFunctions.connection.State == ConnectionState.Open)
            //    {
            //        CommonFunctions.connection.Close();
            //    }
            //    orderInfoID.Text = hiddenIDtextBox.Text;
            //}
        }

        private void AddBuyer_Click(object sender, EventArgs e)
        {
            BuyerInfo buyerInfo = new BuyerInfo();
            buyerInfo.ShowDialog();
            LoadBuyer();
        }

        private void AddStyle_Click(object sender, EventArgs e)
        {
            StyleInfo styleInfo = new StyleInfo();
            styleInfo.ShowDialog();
            LoadStyle();
        }

        private void AddSize_Click(object sender, EventArgs e)
        {
            SizeInfo sizeInfo = new SizeInfo();
            sizeInfo.ShowDialog();
            LoadSize();
        }

        private void AddDia_Click(object sender, EventArgs e)
        {
            DiaInfo diaInfo = new DiaInfo();
            diaInfo.ShowDialog();
            LoadDia();
        }

        private void AddPart_Click(object sender, EventArgs e)
        {
            PartInfo partInfo = new PartInfo();
            partInfo.ShowDialog();
            LoadPart();
        }

        private void ChangeOrderStatus(int rowIndex, int Action)
        {
            try
            {
                DataGridViewRow row = orderInfoDetailsdataGridView.Rows[rowIndex];
                int rowID = Convert.ToInt32(row.Cells[12].Value);

                if (!CommonFunctions.recordExist("SELECT * FROM PlanTable WHERE OrderID = " + rowID))
                {
                    string query = " IF EXISTS (SELECT * FROM Order_Info WHERE Id = " + rowID + ") UPDATE Order_Info SET Status = " + Action + " WHERE Id = " + rowID;

                    if (CommonFunctions.ExecutionToDB(query, 3))
                    {
                        LoadOrderInfoGrid();
                    }
                }
                else
                {
                    MessageBox.Show("This Order has already been used in PlanBoard!!! To Delete First Delete from PlanBoard!!!");
                    return;
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

        private void DeleteOrderInfo_Click(object sender, EventArgs e)
        {

        }

        private void samTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void qtyTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void effTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void orderInfoDetailsdataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void LoadWorkingDaysGrid(string fromDate, string toDate, int McNo)
        {
            try
            {
                labelAlert.Visible = false;
                Grid_WorkDays_Info.Rows.Clear();
                int SL = 1;
                int S1 = 1; string S2 = ""; int S3 = 0; int S4 = 0; string S5 = ""; bool S6 = false; int S7 = 0;
                string query = "";
                if (McNo < 0)
                {
                    query = "SELECT * FROM WorkingDays WHERE WorkDate BETWEEN '" + fromDate + "' and '" + toDate + "' order by WorkDate asc";
                }
                else
                {
                    query = "SELECT * FROM WorkingDays WHERE WorkDate BETWEEN '" + fromDate + "' and '" + toDate + "' AND MachineNo = " + McNo + " order by WorkDate asc";
                }

                SqlDataReader reader = CommonFunctions.GetFromDB(query);

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        S1 = SL;
                        S2 = reader.IsDBNull(reader.GetOrdinal("WorkDate")) == true ? "0/0/000" : Convert.ToDateTime(reader["WorkDate"]).Date.ToString("dd/MM/yyyy");
                        S3 = reader.IsDBNull(reader.GetOrdinal("MachineNo")) == true ? 0 : Convert.ToInt32(reader["MachineNo"]);
                        S4 = reader.IsDBNull(reader.GetOrdinal("Minute")) == true ? 1320 : Convert.ToInt32(reader["Minute"]);
                        S5 = reader.IsDBNull(reader.GetOrdinal("DayName")) == true ? "0/0/000" : reader["DayName"].ToString();
                        S6 = Convert.ToInt32(reader["WorkDay"]) == 0 ? false : true;
                        S7 = reader.IsDBNull(reader.GetOrdinal("Active")) == true ? Convert.ToInt32(VariableDecleration_Class.Status.Active) : Convert.ToInt32(reader["Active"]);

                        Grid_WorkDays_Info.Rows.Add(S1, S2, S3, S4, S5, S6, S7, false);

                        SL++;
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
                MakeWorkingGridReadOnly();
            }
        }

        private void MakeWorkingGridReadOnly()
        {
            foreach (DataGridViewRow row in Grid_WorkDays_Info.Rows)
            {
                string date = row.Cells[1].Value.ToString();
                string McNo = row.Cells[2].Value.ToString();
                if (CommonFunctions.recordExist("SELECT * FROM PlanTable WHERE MachineNo = " + Convert.ToInt32(McNo) + " AND TaskDate = '" + DateTime.ParseExact(date, "dd/MM/yyyy", null) + "'"))
                {
                    row.Cells[3].ReadOnly = true;
                    row.Cells[5].ReadOnly = true;
                    row.Cells[6].ReadOnly = true;
                    row.Cells[7].Value = true;
                    row.DefaultCellStyle.BackColor = Color.IndianRed;
                }
            }
        }

        private void GridInitialize()
        {
            labelAlert.Visible = false;
            Grid_WorkDays_Info.Columns.Clear();
            Grid_WorkDays_Info.Columns.Add("SL", "SL");
            Grid_WorkDays_Info.Columns.Add("Date", "WorkDate");
            Grid_WorkDays_Info.Columns.Add("MachineNo", "M/C No");
            Grid_WorkDays_Info.Columns.Add("Minute", "Minute");
            Grid_WorkDays_Info.Columns.Add("DayName", "DayName");

            DataGridViewCheckBoxColumn wrkChkBox = new DataGridViewCheckBoxColumn();
            wrkChkBox.Name = "WorkDay";
            wrkChkBox.HeaderText = "WorkDay";
            //wrkChkBox.FalseValue = "0";
            //wrkChkBox.TrueValue = "1";
            Grid_WorkDays_Info.Columns.Add(wrkChkBox);

            DataGridViewComboBoxColumn cbo = new DataGridViewComboBoxColumn();
            cbo.Name = "Status";
            cbo.HeaderText = "Status";
            cbo.ValueType = typeof(VariableDecleration_Class.Status);
            cbo.DataSource = new VariableDecleration_Class.Status[] { VariableDecleration_Class.Status.InActive, VariableDecleration_Class.Status.Active }.Select(value => new { Display = value.ToString(), Value = Convert.ToInt32(value) }).ToList();
            cbo.ValueMember = "Value";
            cbo.DisplayMember = "Display";
            Grid_WorkDays_Info.Columns.Add(cbo);
            Grid_WorkDays_Info.Columns.Add("Readonly", "Readonly");

            Grid_WorkDays_Info.Columns[0].ReadOnly = true;
            Grid_WorkDays_Info.Columns[1].ReadOnly = true;
            Grid_WorkDays_Info.Columns[2].ReadOnly = true;
            Grid_WorkDays_Info.Columns[3].ReadOnly = true;
            Grid_WorkDays_Info.Columns[4].ReadOnly = true;
            Grid_WorkDays_Info.Columns[7].ReadOnly = true;
            Grid_WorkDays_Info.Columns[7].Visible = false;

        }

        private void Load_WorkingDays_ComboBox()
        {
            try
            {
                string query = "SELECT * FROM Machine_Info WHERE Status != " + 0 + " order by MachineNo asc";

                SqlDataReader reader = CommonFunctions.GetFromDB(query);
                machineNoComboBox.DataSource = null;
                machineNoComboBox.Items.Clear();
                mcNo.Clear();
                mcNo.Add("ALL");
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        mcNo.Add(Convert.ToInt32(reader["MachineNo"]));
                    }
                    machineNoComboBox.DataSource = mcNo;
                    machineNoComboBox.SelectedIndex = 0;
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

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            try
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

                if (machineNoComboBox.Text.Trim() == "")
                {
                    MessageBox.Show("Please Enter Machine Information", VariableDecleration_Class.sMSGBOX);
                    machineNoComboBox.Focus();
                    return;
                }

                if (toDateTimePicker.Value < fromDateTimePicker.Value)
                {
                    MessageBox.Show("Start Date Can not be greater than End Date", VariableDecleration_Class.sMSGBOX);
                    toDateTimePicker.Focus();
                    return;
                }

                Grid_WorkDays_Info.Rows.Clear();
                string fromDate = fromDateTimePicker.Value.ToString();
                string toDate = toDateTimePicker.Value.ToString();
                fromDateTimePicker.Enabled = false;
                toDateTimePicker.Enabled = false;
                machineNoComboBox.Enabled = false;

                int mcNo = 0;
                if (machineNoComboBox.SelectedIndex != 0)
                    mcNo = Convert.ToInt32(machineNoComboBox.Text);

                SqlDataReader reader = null;
                int SL = 1;
                int S1 = 1; string S2 = ""; int S3 = 0; int S4 = 0; string S5 = ""; bool S6 = false; int S7 = 0;

                for (DateTime date = Convert.ToDateTime(fromDate); date <= Convert.ToDateTime(toDate); date = date.AddDays(1))
                {
                    string query = "";
                    if (mcNo != 0)
                    {
                        query = "SELECT Top 1* FROM WorkingDays WHERE WorkDate = '" + date.Date.ToString() + "' AND MachineNo = " + mcNo;
                        reader = CommonFunctions.GetFromDB(query);
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                S1 = SL;
                                S2 = reader.IsDBNull(reader.GetOrdinal("WorkDate")) == true ? "0/0/000" : Convert.ToDateTime(reader["WorkDate"]).Date.ToString("dd/MM/yyyy");
                                S3 = reader.IsDBNull(reader.GetOrdinal("MachineNo")) == true ? 0 : Convert.ToInt32(reader["MachineNo"]);
                                S4 = reader.IsDBNull(reader.GetOrdinal("Minute")) == true ? 1320 : Convert.ToInt32(reader["Minute"]);
                                S5 = reader.IsDBNull(reader.GetOrdinal("DayName")) == true ? "" : reader["DayName"].ToString();
                                S6 = Convert.ToInt32(reader["WorkDay"]) == 0 ? false : true;
                                S7 = Convert.ToInt32(reader["Active"]);
                            }
                        }
                        else
                        {
                            S1 = SL;
                            S2 = date.Date.ToString("dd/MM/yyyy");
                            S3 = mcNo;
                            S4 = 1320;
                            S5 = date.DayOfWeek.ToString();
                            S6 = date.DayOfWeek == DayOfWeek.Friday ? false : true;
                            S7 = S6 == true ? Convert.ToInt32(VariableDecleration_Class.Status.Active) : Convert.ToInt32(VariableDecleration_Class.Status.InActive);
                        }

                        Grid_WorkDays_Info.Rows.Add(S1, S2, S3, S4, S5, S6, S7);

                        SL++;

                    }

                    else
                    {
                        for (int MachineNo = 1; MachineNo <= machineNoComboBox.Items.Count - 1; MachineNo++)
                        {
                            query = "SELECT Top 1* FROM WorkingDays WHERE WorkDate = '" + date.Date.ToString() + "' AND MachineNo = " + machineNoComboBox.Items[MachineNo].ToString();
                            reader = CommonFunctions.GetFromDB(query);

                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    S1 = SL;
                                    S2 = reader.IsDBNull(reader.GetOrdinal("WorkDate")) == true ? "0/0/000" : Convert.ToDateTime(reader["WorkDate"]).Date.ToString("dd/MM/yyyy");
                                    S3 = reader.IsDBNull(reader.GetOrdinal("MachineNo")) == true ? 0 : Convert.ToInt32(reader["MachineNo"]);
                                    S4 = reader.IsDBNull(reader.GetOrdinal("Minute")) == true ? 1320 : Convert.ToInt32(reader["Minute"]);
                                    S5 = reader.IsDBNull(reader.GetOrdinal("DayName")) == true ? "" : reader["DayName"].ToString();
                                    S6 = Convert.ToInt32(reader["WorkDay"]) == 0 ? false : true;
                                    S7 = Convert.ToInt32(reader["Active"]);
                                }
                            }
                            else
                            {
                                //mcNo = Convert.ToInt32(machineNoComboBox.Items[MachineNo].ToString());
                                S1 = SL;
                                S2 = date.Date.ToString("dd/MM/yyyy");
                                S3 = Convert.ToInt32(machineNoComboBox.Items[MachineNo].ToString());
                                S4 = 1320;
                                S5 = date.DayOfWeek.ToString();
                                S6 = date.DayOfWeek == DayOfWeek.Friday ? false : true;
                                S7 = S6 == true ? Convert.ToInt32(VariableDecleration_Class.Status.Active) : Convert.ToInt32(VariableDecleration_Class.Status.InActive);
                            }

                            Grid_WorkDays_Info.Rows.Add(S1, S2, S3, S4, S5, S6, S7);

                            SL++;
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

        private DataTable loadDataTable()
        {
            //Adding the Columns
            DataTable dt = new DataTable();
            foreach (DataGridViewColumn col in Grid_WorkDays_Info.Columns)
            {
                dt.Columns.Add(col.Name);
            }

            foreach (DataGridViewRow row in Grid_WorkDays_Info.Rows)
            {
                DataRow dRow = dt.NewRow();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    dRow[cell.ColumnIndex] = cell.Value;
                }
                dt.Rows.Add(dRow);
            }
            return dt;

        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {

        }

        private void Minute_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void panel4_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

        }

        private void orderInfoDetailsdataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void toDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (machineNoComboBox.SelectedIndex == 0)
            {
                LoadWorkingDaysGrid(fromDateTimePicker.Value.ToString(), toDateTimePicker.Value.ToString(), -1);
            }
            else if (machineNoComboBox.SelectedIndex > 0)
            {
                LoadWorkingDaysGrid(fromDateTimePicker.Value.ToString(), toDateTimePicker.Value.ToString(), Convert.ToInt32(machineNoComboBox.Items[machineNoComboBox.SelectedIndex]));
            }

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void fromDateTimePicker_ValueChanged(object sender, EventArgs e)
        {
            if (machineNoComboBox.SelectedIndex == 0)
            {
                LoadWorkingDaysGrid(fromDateTimePicker.Value.ToString(), toDateTimePicker.Value.ToString(), -1);
            }
            else if (machineNoComboBox.SelectedIndex > 0)
            {
                LoadWorkingDaysGrid(fromDateTimePicker.Value.ToString(), toDateTimePicker.Value.ToString(), Convert.ToInt32(machineNoComboBox.Items[machineNoComboBox.SelectedIndex]));
            }
        }

        private void machineNoComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (machineNoComboBox.SelectedIndex == 0)
            {
                LoadWorkingDaysGrid(fromDateTimePicker.Value.ToString(), toDateTimePicker.Value.ToString(), -1);
            }
            else if (machineNoComboBox.SelectedIndex > 0)
            {
                LoadWorkingDaysGrid(fromDateTimePicker.Value.ToString(), toDateTimePicker.Value.ToString(), Convert.ToInt32(machineNoComboBox.Items[machineNoComboBox.SelectedIndex]));
            }
        }

        private void machineInfoDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // make sure user select at least 1 row 
            {
                SetValue_machineInfo(e.RowIndex);
            }
        }

        private void orderInfoDetailsdataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // make sure user select at least 1 row 
            {
                DataGridViewRow row = orderInfoDetailsdataGridView.Rows[e.RowIndex];

                int rowID = Convert.ToInt32(row.Cells[12].Value);

                if (CommonFunctions.recordExist("SELECT * FROM Order_Info WHERE Status = 2 AND Id = " + rowID))
                {
                    hiddenIDtextBox.Text = rowID.ToString();
                    if (!CommonFunctions.recordExist("SELECT * FROM PlanTable WHERE OrderID = " + rowID))
                    {
                        SetValue_orderInfo(e.RowIndex, false);
                    }
                    else
                    {
                        SetValue_orderInfo(e.RowIndex, true);
                        return;
                    }
                }
                else
                {
                    SaveOrderInfo.Enabled = false;
                    MessageBox.Show("To Update the order info first activate the order!!!");
                    return;
                }
            }
        }

        private void buyerComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (hiddenIDtextBox.Text == "")
            {
                orderInfoDetailsdataGridView.Rows.Clear();
                LoadOrderInfoGrid();
                if (buyerComboBox.SelectedIndex != 0 && styleComboBox.SelectedIndex != 0 && sizeComboBox.SelectedIndex != 0 && partComboBox.SelectedIndex != 0)
                {
                    SetDiaSAM();
                }
            }
        }

        private void styleComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (hiddenIDtextBox.Text == "")
            {
                orderInfoDetailsdataGridView.Rows.Clear();
                LoadOrderInfoGrid();
                if (buyerComboBox.SelectedIndex != 0 && styleComboBox.SelectedIndex != 0 && sizeComboBox.SelectedIndex != 0 && partComboBox.SelectedIndex != 0)
                {
                    SetDiaSAM();
                }
            }
        }

        private void sizeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (buyerComboBox.SelectedIndex != 0 && styleComboBox.SelectedIndex != 0 && sizeComboBox.SelectedIndex != 0 && partComboBox.SelectedIndex != 0)
            {
                SetDiaSAM();
            }
        }

        private void diaComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (hiddenIDtextBox.Text == "")
            //{
            //LoadOrdeWiseGrid();
            LoadMachine();
            //}
        }

        private void partComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (buyerComboBox.SelectedIndex != 0 && styleComboBox.SelectedIndex != 0 && sizeComboBox.SelectedIndex != 0 && partComboBox.SelectedIndex != 0)
            {
                SetDiaSAM();
            }
        }

        private void SetDiaSAM()
        {
            string connectionStr = ConnectionManager.connectionString; SqlDataReader reader;
            SqlCommand cm = new SqlCommand(); SqlConnection cn = new SqlConnection(connectionStr); cm.Connection = cn; cn.Open();

            try
            {
                int buyerName = ((KeyValuePair<int, string>)buyerComboBox.SelectedItem).Key;
                int styleName = ((KeyValuePair<int, string>)styleComboBox.SelectedItem).Key;
                int sizeNo = ((KeyValuePair<int, string>)sizeComboBox.SelectedItem).Key;
                int bodyPart = ((KeyValuePair<int, string>)partComboBox.SelectedItem).Key;

                string query = "SELECT TOP 1 * FROM Order_Info WHERE Buyer = " + buyerName + " AND Style = " + styleName + " AND Size = " + sizeNo + " AND BodyPart = " + bodyPart;

                if (CommonFunctions.recordExist(query))
                {
                    query = "SELECT TOP 1 * FROM Order_Info WHERE Buyer = " + buyerName + " AND Style = " + styleName + " AND Size = " + sizeNo + " AND BodyPart = " + bodyPart + " order by Id desc";
                    cm.CommandText = query;
                    reader = cm.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            diaComboBox.SelectedValue = reader.IsDBNull(reader.GetOrdinal("Dia")) == true ? 0 : Convert.ToInt32(reader["Dia"]);
                            samTextBox.Text = reader.IsDBNull(reader.GetOrdinal("SAM")) == true ? "" : reader["SAM"].ToString();
                        }
                    }
                    else
                    {
                        diaComboBox.SelectedValue = 0;
                        samTextBox.Text = "";
                    }
                }
                else
                {
                    diaComboBox.SelectedValue = 0;
                    samTextBox.Text = "";
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

        private void orderInfoDetailsdataGridView_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex != -1 && e.Button == MouseButtons.Right)
            {
                this.orderInfoDetailsdataGridView.Rows[e.RowIndex].Selected = true;
                this.rowIndex = e.RowIndex;
                this.orderInfoDetailsdataGridView.CurrentCell = this.orderInfoDetailsdataGridView.Rows[e.RowIndex].Cells[1];
                this.contextMenuStrip1.Show(this.orderInfoDetailsdataGridView, e.Location);
                contextMenuStrip1.Show(Cursor.Position);
            }
        }

        private void panel3_MouseMove(object sender, MouseEventArgs e)
        {
            //if (mouseDown)
            //{
            //    this.Location = new Point(
            //        (this.Location.X - lastLocation.X) + e.X, (this.Location.Y - lastLocation.Y) + e.Y);

            //    this.Update();
            //}
        }

        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            //mouseDown = true;
            //lastLocation = e.Location;
        }

        private void deleteToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Int32 rowToDelete = orderInfoDetailsdataGridView.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            if (rowToDelete > -1)
            {
                ChangeOrderStatus(rowToDelete, 0);
            }
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            ResetWorkingDays();
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Grid_WorkDays_Info_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex == 5 && e.Button == MouseButtons.Right)
            {
                this.rowIndex = e.RowIndex;
                this.contextMenuStrip2.Show(this.Grid_WorkDays_Info, e.Location);
                contextMenuStrip2.Show(Cursor.Position);
            }
        }

        private void checkAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Grid_WorkDays_Info.Rows.Count; i++)
            {
                if (Convert.ToBoolean(Grid_WorkDays_Info.Rows[i].Cells[7].Value) == false)
                {
                    Grid_WorkDays_Info.Rows[i].Cells[5].Value = true;
                    Grid_WorkDays_Info.Rows[i].Cells[6].Value = Convert.ToInt32(VariableDecleration_Class.Status.Active);
                }
            }
        }

        private void unCheckAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Grid_WorkDays_Info.Rows.Count; i++)
            {
                if (Convert.ToBoolean(Grid_WorkDays_Info.Rows[i].Cells[7].Value) == false)
                {
                    Grid_WorkDays_Info.Rows[i].Cells[5].Value = false;
                    Grid_WorkDays_Info.Rows[i].Cells[6].Value = Convert.ToInt32(VariableDecleration_Class.Status.InActive);
                }
            }
        }

        private void entryGroupBox_Enter(object sender, EventArgs e)
        {

        }

        private void Grid_WorkDays_Info_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(Minute_KeyPress);
            if (Grid_WorkDays_Info.CurrentCell.ColumnIndex == 3) //Desired Column
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(Minute_KeyPress);
                }
            }
        }

        private void Grid_WorkDays_Info_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                Boolean readOnly = Convert.ToBoolean(Grid_WorkDays_Info.Rows[e.RowIndex].Cells[7].Value);
                if (readOnly)
                {
                    labelAlert.Visible = true;
                }
                else
                {
                    labelAlert.Visible = false;
                }
            }
        }

        private void SaveWorkDays_Click(object sender, EventArgs e)
        {
            try
            {

                if (Grid_WorkDays_Info.Rows.Count == 0)
                {
                    MessageBox.Show("Table has no rows to save!!!");
                    return;
                }

                DataTable dt = loadDataTable();

                string fromDate = fromDateTimePicker.Value.Date.ToString();
                string toDate = toDateTimePicker.Value.Date.ToString();

                //int mcNo = Convert.ToInt32(machineNoComboBox.Text);

                Boolean result = false;

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row[0].ToString() != "")
                        {

                            DateTime currentDate = DateTime.ParseExact(row[1].ToString(), "dd/MM/yyyy", null);
                            int minute = Convert.ToInt32(row[3]);
                            string currentDay = row[4].ToString();
                            int currentWorkDay = Convert.ToBoolean(row[5]) == true ? 1 : 0;
                            int currentActiveStatus = Convert.ToInt32(row[6]);
                            int mcNo = Convert.ToInt32(row[2]);

                            string query = " IF EXISTS (SELECT * FROM WorkingDays WHERE WorkDate = '" + currentDate + "' AND MachineNo = " + mcNo + ") UPDATE WorkingDays SET Minute = " + minute + ", WorkDay = " + currentWorkDay + ", Active = " + currentActiveStatus + " WHERE WorkDate = '" + currentDate + "'" + " AND MachineNo = " + mcNo +
                                " ELSE INSERT INTO WorkingDays(WorkDate, MachineNo, Minute, DayName, WorkDay, Active) VALUES ('" + currentDate + "'," + mcNo + "," + minute + ",'" + currentDay + "'," + currentWorkDay + "," + currentActiveStatus + ") ";

                            result = (CommonFunctions.ExecutionToDB(query, 3));

                        }
                    }
                    MessageBox.Show("Saved To DataBase Successfully!!!");
                }

                if (result == true)
                {
                    if (machineNoComboBox.SelectedIndex != 0)
                    {
                        int mcNo = Convert.ToInt32(machineNoComboBox.Text);
                        LoadWorkingDaysGrid(fromDate, toDate, mcNo);
                    }
                    else
                    {
                        LoadWorkingDaysGrid(fromDate, toDate, -1);
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
                fromDateTimePicker.Value = DateTime.Now.Date;
                toDateTimePicker.Value = DateTime.Now.Date;
                fromDateTimePicker.Enabled = true;
                toDateTimePicker.Enabled = true;
                machineNoComboBox.Enabled = true;
                Load_WorkingDays_ComboBox();
            }
        }

        private void completeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Int32 rowToComplete = orderInfoDetailsdataGridView.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            if (rowToComplete > -1)
            {
                ChangeOrderStatus(rowToComplete, 3);
            }
        }

        private void activeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Int32 rowToActive = orderInfoDetailsdataGridView.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            if (rowToActive > -1)
            {
                ChangeOrderStatus(rowToActive, Convert.ToInt32((VariableDecleration_Class.Status.Pending)));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void diaAddbtn_Click(object sender, EventArgs e)
        {
            DiaInfo diaInfo = new DiaInfo();
            diaInfo.ShowDialog();
            LoadDiaMachine();
        }

        private void addToProductioncheckBox_Click(object sender, EventArgs e)
        {

        }

        private void shipDatePicker_ValueChanged(object sender, EventArgs e)
        {
            if (shipDatePicker.Value < DateTime.Now.Date)
            {
                if (hiddenIDtextBox.Text != "")
                {
                    //if (!CommonFunctions.recordExist("SELECT Top 1 * FROM PlanTable WHERE OrderID = " + Convert.ToInt32(hiddenIDtextBox.Text)))
                    //{
                    //    MessageBox.Show("Knit Closing Date can not be smaller than current date!!!");
                    //    shipDatePicker.Value = DateTime.Now.Date;
                    //    return;
                    //}
                }
                else
                {
                    MessageBox.Show("Knit Closing Date can not be smaller than current date!!!");
                    shipDatePicker.Value = DateTime.Now.Date;
                    return;
                }
            }

            else
            {
                if (hiddenIDtextBox.Text == "")
                {
                    newOrderQtyTextBox.Text = qtyTextBox.Text;
                    LoadOrdeWiseGrid();
                }
            }
        }

        private void LoadOrdeWiseGrid()
        {
            try
            {
                orderWisePlandataGridView.Rows.Clear();
                if (buyerComboBox.SelectedIndex == 0 || styleComboBox.SelectedIndex == 0 || sizeComboBox.SelectedIndex == 0 || diaComboBox.SelectedIndex == 0 || partComboBox.SelectedIndex == 0 || qtyTextBox.Text == "" || shipDatePicker.Value < DateTime.Now.Date || samTextBox.Text == "" || effTextBox.Text == "" || mcNo.Count == 0)
                {
                    return;
                }
                string connectionStr = ConnectionManager.connectionString;
                SqlDataReader reader1, reader2, reader3;

                SqlConnection cn = new SqlConnection(connectionStr);
                SqlCommand cm = new SqlCommand();
                cm.Connection = cn;
                cn.Open();

                int buyerName = ((KeyValuePair<int, string>)buyerComboBox.SelectedItem).Key;
                int styleName = ((KeyValuePair<int, string>)styleComboBox.SelectedItem).Key;
                int sizeNo = ((KeyValuePair<int, string>)sizeComboBox.SelectedItem).Key;
                int Dia = ((KeyValuePair<int, string>)diaComboBox.SelectedItem).Key;
                int bodyPart = ((KeyValuePair<int, string>)partComboBox.SelectedItem).Key;
                string purchaseOrderNumber = pOTextBox.Text;

                if (CommonFunctions.recordExist("SELECT * FROM PlanTable WHERE OrderID = (SELECT Id FROM Order_Info WHERE Buyer = " + buyerName + " AND Style = " + styleName + " AND Size = " + sizeNo + " AND Dia = " + Dia + " AND BodyPart = " + bodyPart + " AND PurchaseOrderNo = '" + purchaseOrderNumber + "')"))
                {
                    MessageBox.Show("This order already exists in PlanBoard!!!");
                    return;
                }

                endDateTimePicker.Value = shipDatePicker.Value;
                DateTime endDate = shipDatePicker.Value;
                DateTime TaskDate = DateTime.Now;
                double sam = Convert.ToDouble(samTextBox.Text);
                int efficiency = 0;
                int planQty = Convert.ToInt32(newOrderQtyTextBox.Text);
                int temp = planQty;

                int j = 0; int SL = 1; string S1 = ""; string S2 = ""; string S3 = ""; string S4 = ""; string S5 = ""; string S6 = ""; string S7 = ""; string S8 = ""; string S9 = ""; Boolean S10 = false;

                SqlConnection cn1 = new SqlConnection(connectionStr);
                SqlCommand cm1 = new SqlCommand();
                cm1.Connection = cn1;
                SqlConnection cn2 = new SqlConnection(connectionStr);
                SqlCommand cm2 = new SqlCommand();
                cm2.Connection = cn2;
                SqlConnection cn3 = new SqlConnection(connectionStr);
                SqlCommand cm3 = new SqlCommand();
                cm3.Connection = cn3;

                if (mcNo.Count > 0)
                {
                    int i = 0;
                    while (i < mcNo.Count && temp > 0)
                    {
                        int rowID = 0; int Capacity = 0; int MachineStatus = 0;

                        int machineNo = Convert.ToInt32(mcNo[i]);
                        int tempMachine = 0;

                        cn1.Open();
                        cm1.CommandText = "SELECT TaskDate FROM Planing_Board_Details WHERE Id = (SELECT MAX(Id) FROM Planing_Board_Details WHERE MachineNo = " + machineNo + " AND DiaID = " + Dia + ")";
                        reader1 = cm1.ExecuteReader();

                        if (reader1.HasRows)
                        {
                            while (reader1.Read())
                            {
                                TaskDate = reader1.IsDBNull(reader1.GetOrdinal("TaskDate")) == true ? DateTime.Now.Date : DateTime.Now.Date > Convert.ToDateTime(reader1["TaskDate"]) ? DateTime.Now.Date : Convert.ToDateTime(reader1["TaskDate"]);
                            }
                        }
                        else
                        {
                            TaskDate = DateTime.Now.Date;
                        }
                        cn1.Close();

                        while (TaskDate.Date <= endDate.Date && temp > 0)
                        {
                            int TotalRestActualQty = 0; int TotalRestPlanQty = 0; int RecordCount = 0; int TotalRestSam = 0; int TotalRestEfficiency = 0; int Minute = 0; int NewCapacity = 0;

                            if (machineNo != tempMachine)
                            {
                                tempMachine = machineNo;
                                j = 0;
                            }
                            int newPlanQty = 0; int remainingQty = 0; int RemainingMinute = 0; int OldEff = 100;

                            cm2.CommandText = " SELECT Top 1*, (SELECT SUM(PlanQty) FROM PlanTable WHERE MachineNo = " + machineNo + " AND TaskDate = '" + TaskDate + "') AS TotalRestPlanQty, " +
                                              " (SELECT SUM(ActualQty) FROM PlanTable WHERE MachineNo = " + machineNo + " AND TaskDate = '" + TaskDate + "') AS TotalRestActualQty, " +
                                              //" (SELECT SUM(SAM) FROM PlanTable WHERE MachineNo = " + machineNo + " AND TaskDate = '" + TaskDate + "') AS TotalRestSam, " +
                                              " (SELECT Count(Id) FROM PlanTable WHERE MachineNo = " + machineNo + " AND TaskDate = '" + TaskDate + "') AS RecordCount, " +
                                              " (SELECT Minute FROM WorkingDays WHERE MachineNo = " + machineNo + " AND WorkDate = '" + TaskDate + "' AND Active = 1) AS Minute " +
                                              //" (SELECT SUM(Efficiency) FROM PlanTable WHERE MachineNo = " + machineNo + " AND TaskDate = '" + TaskDate + "') AS TotalRestEfficiency " +
                                              " FROM PlanTable WHERE MachineNo = " + machineNo + " AND TaskDate = '" + TaskDate + "' order by Id desc";
                            cn2.Open();
                            reader2 = cm2.ExecuteReader();

                            if (reader2.HasRows)
                            {
                                while (reader2.Read())
                                {
                                    TotalRestActualQty = reader2.IsDBNull(reader2.GetOrdinal("TotalRestActualQty")) == true ? 0 : Convert.ToInt32(reader2["TotalRestActualQty"]);
                                    TotalRestPlanQty = reader2.IsDBNull(reader2.GetOrdinal("TotalRestPlanQty")) == true ? 0 : Convert.ToInt32(reader2["TotalRestPlanQty"]);
                                    OldEff = reader2.IsDBNull(reader2.GetOrdinal("Efficiency")) == true ? 1 : Convert.ToInt32(reader2["Efficiency"]);
                                    Minute = reader2.IsDBNull(reader2.GetOrdinal("Minute")) == true ? 0 : Convert.ToInt32(reader2["Minute"]);
                                    RemainingMinute = reader2.IsDBNull(reader2.GetOrdinal("RemainingMinute")) == true ? 0 : Convert.ToInt32(reader2["RemainingMinute"]);
                                    //RecordCount = reader2.IsDBNull(reader2.GetOrdinal("RecordCount")) == true ? 0 : Convert.ToInt32(reader2["RecordCount"]);
                                    //TotalRestSam = reader2.IsDBNull(reader2.GetOrdinal("TotalRestSam")) == true ? 0 : Convert.ToInt32(reader2["TotalRestSam"]);
                                    //TotalRestEfficiency = reader2.IsDBNull(reader2.GetOrdinal("TotalRestEfficiency")) == true ? 0 : Convert.ToInt32(reader2["TotalRestEfficiency"]);
                                    
                                }
                            }
                            cn2.Close();

                            MachineStatus = CommonFunctions.recordExist("SELECT Active FROM WorkingDays WHERE MachineNo = " + machineNo + " AND WorkDate = '" + TaskDate + "' AND Active = 1 ") == true ? 1 : 0;

                            if (MachineStatus > 0)
                            {
                                cn3.Open();
                                cm3.CommandText = "SELECT Minute FROM WorkingDays WHERE MachineNo = " + machineNo + " AND WorkDate = '" + TaskDate + "' AND Active = 1";
                                reader3 = cm3.ExecuteReader();
                                if (reader3.HasRows)
                                {
                                    while (reader3.Read())
                                    {
                                        Minute = Convert.ToInt32(reader3["Minute"]);
                                        RemainingMinute = RemainingMinute == 0 ? Minute : RemainingMinute;
                                    }
                                }
                                cn3.Close();

                                if (RemainingMinute > 60)
                                {
                                    efficiency = LCFlag == true ? j > LcArray.Count - 1 ? LcArray[LcArray.Count - 1] : LcArray[j] : Convert.ToInt32(Convert.ToDouble(effTextBox.Text));
                                    double updatedRemainingMinute = (int)Math.Floor((RemainingMinute * (Convert.ToDouble(efficiency / 100.00))) / (Convert.ToDouble(OldEff / 100.00)));
                                    NewCapacity = TotalRestPlanQty + (int)Math.Floor(updatedRemainingMinute / sam);
                                    
                                    //double UpdatedSam = (TotalRestSam + sam) / (RecordCount + 1);
                                    //double UpdatedEfficiency = (double)((TotalRestEfficiency + efficiency) / (RecordCount + 1));

                                    //NewCapacity = Convert.ToInt32(Math.Floor((double)((Minute * (UpdatedEfficiency / 100.00)) / UpdatedSam)));
                                }

                                

                                if (TotalRestActualQty == 0)
                                {
                                    if (NewCapacity > TotalRestPlanQty)
                                    {
                                        newPlanQty = TotalRestPlanQty == 0 ? temp > NewCapacity ? NewCapacity : temp : temp > (NewCapacity - TotalRestPlanQty) ? (NewCapacity - TotalRestPlanQty) : temp;
                                        remainingQty = planQty - newPlanQty;
                                    }
                                }
                                else
                                {
                                    if (NewCapacity > TotalRestActualQty)
                                    {
                                        newPlanQty = TotalRestActualQty == 0 ? temp > NewCapacity ? NewCapacity : temp : NewCapacity - TotalRestActualQty;
                                        remainingQty = planQty - newPlanQty;
                                    }
                                }

                                if (newPlanQty > 0)
                                {
                                    S1 = SL.ToString();
                                    S2 = machineNo.ToString();
                                    S3 = TaskDate.ToString("dd/MM/yyyy");
                                    S4 = NewCapacity.ToString();
                                    S5 = TotalRestPlanQty.ToString();
                                    S6 = newPlanQty.ToString();
                                    S7 = Convert.ToInt32(S1) == 1 ? newPlanQty.ToString() : (Convert.ToInt32(orderWisePlandataGridView.Rows[Convert.ToInt32(S1) - 2].Cells[6].Value) + newPlanQty).ToString();
                                    S8 = Minute.ToString();
                                    S9 = efficiency.ToString();
                                    S10 = true;
                                    orderWisePlandataGridView.Rows.Add(S1, S2, S3, S4, S5, S6, S7, S8, S9, S10);
                                    SL++;
                                    j++;
                                }
                            }
                            else
                            {
                                S1 = SL.ToString();
                                S2 = machineNo.ToString();
                                S3 = TaskDate.ToString("dd/MM/yyyy");
                                S4 = "0";
                                S5 = "0";
                                S6 = "0";
                                S7 = "0";
                                S8 = Minute.ToString();
                                S9 = "0";
                                S10 = true;
                                orderWisePlandataGridView.Rows.Add(S1, S2, S3, S4, S5, S6, S7, S8, S9, S10);
                                SL++;
                            }

                            TaskDate = TaskDate.AddDays(1);
                            temp = temp - newPlanQty;
                        }
                        i++;
                    }
                }
                startDateTimePicker.Value = DateTime.ParseExact(orderWisePlandataGridView.Rows[0].Cells[2].Value.ToString(), "dd/MM/yyyy", null);
                endDateTimePicker.Value = DateTime.ParseExact(orderWisePlandataGridView.Rows[orderWisePlandataGridView.Rows.Count - 1].Cells[2].Value.ToString(), "dd/MM/yyyy", null);
                if (temp > 0)
                {
                    orderInfoWarningLbl.Text = "Knit Close Date Exceeds!!! Left Plan Qty is " + temp + ". No machine available!!! ";
                    orderInfoWarningLbl.Visible = true;
                }
                else
                {
                    orderInfoWarningLbl.Visible = false;
                }

            }
            catch (Exception ee)
            {
                MessageBox.Show("" + ee.ToString());
            }

            finally
            {
                CalculateOrderWisePlanGridSum();
                LCFlag = false;
            }
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
                    orderWisePlandataGridView.Rows.Add("Total", "", "", "", bookedQtySum, plnQtySum, "", "", "", true);
                    orderWisePlandataGridView.Rows[orderWisePlandataGridView.Rows.Count - 1].Cells[5].ReadOnly = true;
                    orderWisePlandataGridView.Rows[orderWisePlandataGridView.Rows.Count - 1].Cells[7].ReadOnly = true;
                    SetMaxMinDate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.ToString());
            }
        }

        private void SetMaxMinDate()
        {
            int i = 0;
            DateTime minDate = DateTime.MaxValue;
            DateTime maxDate = DateTime.MinValue;
            try
            {
                if (orderWisePlandataGridView.Rows.Count > 0)
                {
                    foreach (DataGridViewRow row in orderWisePlandataGridView.Rows)
                    {
                        if (row.Index < orderWisePlandataGridView.Rows.Count - 1)
                        {
                            if (minDate > DateTime.ParseExact(row.Cells[2].Value.ToString(), "dd/MM/yyyy", null))
                            {
                                minDate = DateTime.ParseExact(row.Cells[2].Value.ToString(), "dd/MM/yyyy", null);
                            }
                            if (maxDate < DateTime.ParseExact(row.Cells[2].Value.ToString(), "dd/MM/yyyy", null))
                            {
                                maxDate = DateTime.ParseExact(row.Cells[2].Value.ToString(), "dd/MM/yyyy", null);
                            }
                            if (Convert.ToInt32(row.Cells[3].Value) > 0)
                            {
                                i++;
                            }
                        }
                    }

                    startDateTimePicker.Value = minDate;
                    endDateTimePicker.Value = maxDate;
                    newDaysTextBox.Text = i.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex.ToString());
            }
        }

        private void qtyTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                newOrderQtyTextBox.Text = qtyTextBox.Text;
                LoadOrdeWiseGrid();
            }
        }

        private void samTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                newOrderQtyTextBox.Text = qtyTextBox.Text;
                LoadOrdeWiseGrid();
            }
        }

        private void effTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                newOrderQtyTextBox.Text = qtyTextBox.Text;
                LoadOrdeWiseGrid();
            }
        }

        private void newOrderQtyTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                LoadOrdeWiseGrid();
            }
        }

        private void newOrderQtyTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void LCBtn_Click(object sender, EventArgs e)
        {
            if (LCText.Text != "" && orderWisePlandataGridView.Rows.Count > 0)
            {
                LcArray = LCText.Text.Trim().Split(',').Select(Int32.Parse).ToList();
                if (LcArray.Count > 10)
                {
                    MessageBox.Show("Learning Curve can not be more than 10 values!!!");
                    return;
                }
                LCFlag = true;
                LoadOrdeWiseGrid();
            }
            else
            {
                string msg = LCText.Text == "" ? "Learning Curve Textbox is empty! Please give learning curve value!!!" : "You can not apply learning curve on table!!!";
                MessageBox.Show(msg);
                return;
            }
        }

        private void addToProductioncheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void MachineComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }

        private void MachineComboBox_TextChanged(object sender, EventArgs e)
        {

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
        }

        private void orderWisePlandataGridView_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (e.RowIndex > -1 && orderWisePlandataGridView.Rows[e.RowIndex].Cells[3].Value.ToString() != "")
            {
                if (Convert.ToInt32(orderWisePlandataGridView.Rows[e.RowIndex].Cells[3].Value) == 0)
                {
                    orderWisePlandataGridView.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.IndianRed;
                }
            }
        }

        private void Home_FormClosing(object sender, FormClosingEventArgs e)
        {
            string connectionStr = ConnectionManager.connectionString;
            SqlCommand cm = new SqlCommand(); SqlConnection cn = new SqlConnection(connectionStr); cm.Connection = cn; cn.Open();
            cm.CommandText = "DELETE FROM PlanTable WHERE PlanQty = 0 AND Production = 1";
            cm.ExecuteNonQuery();
            cn.Close();
        }

        private void CHD_ValueChanged(object sender, EventArgs e)
        {
            if (CHD.Value < shipDatePicker.Value)
            {
                MessageBox.Show("ShipmentDate can not be smaller than knit close date!!!");
                CHD.Value = shipDatePicker.Value;
                return;
            }
            else
            {
                pOTextBox.Text = CHD.Text;
            }
            ///else
            //{
            //    if (hiddenIDtextBox.Text == "")
            //    {
            //        newOrderQtyTextBox.Text = qtyTextBox.Text;
            //        LoadOrdeWiseGrid();
            //    }
            //}/
        }

    }

}
