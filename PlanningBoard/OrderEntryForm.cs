using System;
using System.Collections;
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
using System.Globalization;

namespace PlanningBoard
{
    public partial class OrderEntryForm : Form
    {
        public static Dictionary<int, string> BuyerList = new Dictionary<int, string>();
        public static Dictionary<int, string> StyleList = new Dictionary<int, string>();
        public static Dictionary<int, string> DiaList = new Dictionary<int, string>();
        public static Dictionary<int, string> SizeList = new Dictionary<int, string>();
        public static Dictionary<int, string> PartList = new Dictionary<int, string>();
        public OrderEntryForm()
        {
            InitializeComponent();
        }

        private void orderInfoDetailsdataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // make sure user select at least 1 row 
            {
                SetValue(e.RowIndex);
            }
        }

        private void OrderEntryForm_Load(object sender, EventArgs e)
        {
            LoadComboBox();
            LoadGrid();
        }

        private void LoadGrid()
        {
            try
            {
                int SL = 1;
                int S1 = 1; string S2 = ""; string S3 = ""; string S4 = ""; double S5 = 0.00; string S6 = ""; double S7 = 0.00; string S8 = ""; double S9 = 0.00; double S10 = 0.00; string S11 = ""; 

                string query = "SELECT * FROM Order_Info order by ShipmentDate asc";

                SqlDataReader reader = CommonFunctions.GetFromDB(query);

                if (reader.HasRows)
                {
                    orderInfoDetailsdataGridView.Rows.Clear();
                    while (reader.Read())
                    {
                        S1 = SL;
                        S2 = reader.IsDBNull(reader.GetOrdinal("Buyer")) == true ? "Not Defined" : reader["Buyer"].ToString();
                        S3 = reader.IsDBNull(reader.GetOrdinal("Style")) == true ? "Not Defined" : reader["Style"].ToString();
                        S4 = reader.IsDBNull(reader.GetOrdinal("Size")) == true ? "Not Defined" : reader["Size"].ToString();
                        S5 = reader.IsDBNull(reader.GetOrdinal("Dia")) == true ? 0.00 : Convert.ToDouble(reader["Dia"]);
                        S6 = reader.IsDBNull(reader.GetOrdinal("BodyPart")) == true ? "Not Defined" : reader["BodyPart"].ToString();
                        S7 = reader.IsDBNull(reader.GetOrdinal("Quantity")) == true ? 0000 : Convert.ToDouble(reader["Quantity"]);
                        S8 = reader.IsDBNull(reader.GetOrdinal("ShipmentDate")) == true ? "0/0/0000" : reader["ShipmentDate"].ToString();
                        S9 = reader.IsDBNull(reader.GetOrdinal("SAM")) == true ? 0.00 : Convert.ToDouble(reader["SAM"]);
                        S10 = reader.IsDBNull(reader.GetOrdinal("Efficiency")) == true ? 0.00 : Convert.ToDouble(reader["Efficiency"]);
                        S11 = Enum.GetName(typeof(VariableDecleration_Class.Status), reader.IsDBNull(reader.GetOrdinal("Status")) == true ? (int)VariableDecleration_Class.Status.Pending : Convert.ToInt32(reader["Status"]));

                        orderInfoDetailsdataGridView.Rows.Add(S1, S2, S3, S4, S5, S6, S7, S8, S9, S10, S11);
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
                    while (reader.Read())
                    {
                        if (!SizeList.Keys.Contains(Convert.ToInt32(reader["Id"])))
                        {
                            SizeList.Add(Convert.ToInt32(reader["Id"]), (reader["SizeName"]).ToString());
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
                    while (reader.Read())
                    {
                        if (!PartList.Keys.Contains(Convert.ToInt32(reader["Id"])))
                        {
                            PartList.Add(Convert.ToInt32(reader["Id"]), (reader["PartName"]).ToString());
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
                    while (reader.Read())
                    {
                        if (!BuyerList.Keys.Contains(Convert.ToInt32(reader["Id"])))
                        {
                            BuyerList.Add(Convert.ToInt32(reader["Id"]), (reader["BuyerName"]).ToString());
                        }
                    }
                    buyerComboBox.DataSource = new BindingSource(BuyerList, null);
                    buyerComboBox.DisplayMember = "Value";
                    buyerComboBox.ValueMember = "Key";
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
                    while (reader.Read())
                    {
                        if (!StyleList.Keys.Contains(Convert.ToInt32(reader["Id"])))
                        {
                            StyleList.Add(Convert.ToInt32(reader["Id"]), (reader["StyleName"]).ToString());
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
                    while (reader.Read())
                    {
                        if (!DiaList.Keys.Contains(Convert.ToInt32(reader["Id"])))
                        {
                            DiaList.Add(Convert.ToInt32(reader["Id"]), (reader["Dia"]).ToString());
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

        private void SaveOrderInfo_Click(object sender, EventArgs e)
        {

        }

        private void SaveOrderInfo_Click_1(object sender, EventArgs e)
        {
            try
            {
                string buyerName = buyerComboBox.Text;
                string styleName = styleComboBox.Text;
                string sizeNo = sizeComboBox.Text.ToString();
                double dia = Convert.ToDouble(diaComboBox.Text);
                string bodyPart = partComboBox.Text.ToString();
                double qty = Convert.ToDouble(qtyTextBox.Text);
                string shipdate = (shipDatePicker.Text);
                double SAMNo = Convert.ToDouble(samTextBox.Text);
                double eff = Convert.ToDouble(effTextBox.Text);
                int status = (int)(VariableDecleration_Class.Status.Pending);

                string query = " IF EXISTS (SELECT * FROM Order_Info WHERE Buyer = '" + buyerName + "' AND Style = '" + styleName + "') UPDATE Order_Info SET Size = '" + sizeNo + "', Dia = '" + dia + "', BodyPart = '" + bodyPart + "', Quantity = " + qty + ", ShipmentDate = '" + shipdate + "', SAM = " + SAMNo + ", Efficiency = " + eff + ", Status = " + status + " WHERE Buyer = '" + buyerName + "' AND Style = '" + styleName +
                                "' ELSE INSERT INTO Order_Info(Buyer, Style, Size, Dia, BodyPart, Quantity, ShipmentDate, SAM, Efficiency, Status) VALUES ('" + buyerName + "','" + styleName + "','" + sizeNo + "','" + dia + "','" + bodyPart + "'," + qty + ",'" + shipdate + "'," + SAMNo + "," + eff + "," + status + ") ";

                if (CommonFunctions.ExecutionToDB(query, 1))
                {
                    LoadGrid();
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

        private void UpdateOrderInfo_Click(object sender, EventArgs e)
        {
            try
            {
                string buyerName = buyerComboBox.Text;
                string styleName = styleComboBox.Text;
                string sizeNo = sizeComboBox.Text.ToString();
                double dia = Convert.ToDouble(diaComboBox.Text);
                string bodyPart = partComboBox.Text.ToString();
                double qty = Convert.ToDouble(qtyTextBox.Text);
                string shipdate = (shipDatePicker.Text);
                double SAMNo = Convert.ToDouble(samTextBox.Text);
                double eff = Convert.ToDouble(effTextBox.Text);
                int status = (int)(VariableDecleration_Class.Status.Pending);

                string query = " IF EXISTS (SELECT * FROM Order_Info WHERE Buyer = '" + buyerName + "' AND Style = '" + styleName + "') UPDATE Order_Info SET Size = '" + sizeNo + "', Dia = '" + dia + "', BodyPart = '" + bodyPart + "', Quantity = " + qty + ", ShipmentDate = '" + shipdate + "', SAM = " + SAMNo + ", Efficiency = " + eff + " WHERE Buyer = '" + buyerName + "' AND Style = '" + styleName + "'";

                if (CommonFunctions.ExecutionToDB(query, 2))
                {
                    LoadGrid();
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
                buyerComboBox.Enabled = true;
                styleComboBox.Enabled = true;
                AddBuyer.Enabled = true;
                AddStyle.Enabled = true;
            }
        }

        private void SetValue(int rowIndex)
        {
            DataGridViewRow row = orderInfoDetailsdataGridView.Rows[rowIndex];

            buyerComboBox.Text = row.Cells[1].Value.ToString();
            styleComboBox.Text = row.Cells[2].Value.ToString();
            sizeComboBox.Text = row.Cells[3].Value.ToString();
            diaComboBox.Text = row.Cells[4].Value.ToString();
            partComboBox.Text = row.Cells[5].Value.ToString();
            qtyTextBox.Text = row.Cells[6].Value.ToString();
            shipDatePicker.Text = row.Cells[7].Value.ToString();
            samTextBox.Text = row.Cells[8].Value.ToString();
            effTextBox.Text = row.Cells[9].Value.ToString();
            

            buyerComboBox.Enabled = false;
            styleComboBox.Enabled = false;
            AddBuyer.Enabled = false;
            AddStyle.Enabled = false;

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

        private void AddDia_Click(object sender, EventArgs e)
        {
            DiaInfo diaInfo = new DiaInfo();
            diaInfo.ShowDialog();
            LoadDia();
        }

        private void AddSize_Click(object sender, EventArgs e)
        {
            SizeInfo sizeInfo = new SizeInfo();
            sizeInfo.ShowDialog();
            LoadSize();
        }

        private void AddPart_Click(object sender, EventArgs e)
        {
            PartInfo partInfo = new PartInfo();
            partInfo.ShowDialog();
            LoadPart();
        }

        private void DeleteOrderInfo_Click(object sender, EventArgs e)
        {
            try
            {
                string buyerName = buyerComboBox.Text;
                string styleName = styleComboBox.Text;
                string sizeNo = sizeComboBox.Text.ToString();
                double dia = Convert.ToDouble(diaComboBox.Text);
                string bodyPart = partComboBox.Text.ToString();
                double qty = Convert.ToDouble(qtyTextBox.Text);
                string shipdate = (shipDatePicker.Text);
                double SAMNo = Convert.ToDouble(samTextBox.Text);
                double eff = Convert.ToDouble(effTextBox.Text);
                int status = (int)(VariableDecleration_Class.Status.Pending);

                string query = " IF EXISTS (SELECT * FROM Order_Info WHERE Buyer = '" + buyerName + "' AND Style = '" + styleName + "') UPDATE Order_Info SET Status = " + 0 + " WHERE Buyer = '" + buyerName + "' AND Style = '" + styleName +"'";

                if (CommonFunctions.ExecutionToDB(query, 2))
                {
                    LoadGrid();
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
                buyerComboBox.Enabled = true;
                styleComboBox.Enabled = true;
                AddBuyer.Enabled = true;
                AddStyle.Enabled = true;
            }
        }

    }
}
