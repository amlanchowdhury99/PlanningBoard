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

        private void GenerateDetailsBtn_Click(object sender, EventArgs e)
        {
            try
            {

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

        private void GenerateSummaryBtn_Click(object sender, EventArgs e)
        {
            try
            {

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

        private void ExportBtn_Click(object sender, EventArgs e)
        {

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
                MachineList.Add("ALL");
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        MachineList.Add(Convert.ToInt32(reader["MachineNo"]));
                    }
                }
                MachineComboBox.DataSource = MachineList;
                MachineComboBox.SelectedIndex = 0;
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
                SizeList.Clear();
                SizeList.Add(0, "---Select Size---");
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
                PartList.Add(0, "---Select Part---");
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
                BuyerList.Add(0, "---Select Buyer---");

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
                StyleList.Add(0, "---Select Style---");
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
                DiaList.Add(0, "---Select Dia---");
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
    }
}
