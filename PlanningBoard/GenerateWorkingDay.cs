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
    public partial class GenerateWorkingDay : Form
    {
        ArrayList mcNo = new ArrayList();

        public GenerateWorkingDay()
        {
            this.WindowState = FormWindowState.Maximized;
            InitializeComponent();
            LoadComboBox();
            //LoadGrid();
        }

        private void LoadGrid()
        {
            try
            {
                int SL = 1;
                int S1 = 1; string S2 = ""; int S3 = 0; int S4 = 0; string S5 = ""; string S6 = ""; string S7 = "";

                string query = "SELECT * FROM WorkingDays order by WorkDate asc";

                SqlDataReader reader = CommonFunctions.GetFromDB(query);

                if (reader.HasRows)
                {
                    Grid_WorkDays_Info.Rows.Clear();
                    while (reader.Read())
                    {
                        S1 = SL;
                        S2 = reader.IsDBNull(reader.GetOrdinal("WorkDate")) == true ? "0/0/000" : reader["WorkDate"].ToString();
                        S3 = reader.IsDBNull(reader.GetOrdinal("MachineNo")) == true ? 0 : Convert.ToInt32(reader["MachineNo"]);
                        S4 = reader.IsDBNull(reader.GetOrdinal("Minute")) == true ? 1320 : Convert.ToInt32(reader["Minute"]);
                        S5 = reader.IsDBNull(reader.GetOrdinal("DayName")) == true ? "0/0/000" : reader["DayName"].ToString();
                        S6 = reader.IsDBNull(reader.GetOrdinal("WorkDay")) == true ? "Yes" : reader["WorkDay"].ToString();
                        S7 = Enum.GetName(typeof(VariableDecleration_Class.Status), reader.IsDBNull(reader.GetOrdinal("Active")) == true ? (int)VariableDecleration_Class.Status.Active : Convert.ToInt32(reader["Active"]));

                        Grid_WorkDays_Info.Rows.Add(S1, S2, S3, S4, S5, S6, S7);
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
            try
            {
                string query = "SELECT * FROM Machine_Info order by MachineNo asc";

                SqlDataReader reader = CommonFunctions.GetFromDB(query);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        mcNo.Add(Convert.ToInt32(reader["MachineNo"]));
                    }
                    machineNoComboBox.DataSource = mcNo; 
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

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void Grid_Dia_Info_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Grid_Dia_Info_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {

        }

        private void Grid_Dia_Info_CellEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Grid_Dia_Info_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Grid_Dia_Info_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            try
            {

                if (fromDateTimePicker.Text.Trim() == "")
                {
                    MessageBox.Show("Please Enter Date Information", VariableDecleration_Class.sMSGBOX);
                    fromDateTimePicker.Focus();
                    return;
                }

                if (toDateTimePicker.Text.Trim() == "")
                {
                    MessageBox.Show("Please Enter Date Information", VariableDecleration_Class.sMSGBOX);
                    toDateTimePicker.Focus();
                    return;
                }

                if (machineNoComboBox.Text.Trim() == "")
                {
                    MessageBox.Show("Please Enter Machine Information", VariableDecleration_Class.sMSGBOX);
                    machineNoComboBox.Focus();
                    return;
                }

                if( DateTime.Parse(toDateTimePicker.Text) <=  DateTime.Parse(fromDateTimePicker.Text))
                {
                    MessageBox.Show("From From Date Information", VariableDecleration_Class.sMSGBOX);
                    toDateTimePicker.Focus();
                    return;
                }

                Boolean result = false;
                string fromDate = fromDateTimePicker.Text;
                string toDate = toDateTimePicker.Text;
                int mcNo = Convert.ToInt32(machineNoComboBox.Text);
                string currentDate = fromDate;
                int minute = 1320;
                string currentDay = "";
                string currentWorkDay = "";
                int currentActiveStatus = (int)VariableDecleration_Class.Status.Active;

                for (DateTime date = Convert.ToDateTime(fromDate); date <= Convert.ToDateTime(toDate); date = date.AddDays(1))
                {
                    currentDate = date.ToString();
                    currentDay = date.DayOfWeek.ToString();
                    currentWorkDay = date.DayOfWeek == DayOfWeek.Friday ? "No" : "Yes";
                    currentActiveStatus = date.DayOfWeek == DayOfWeek.Friday ? (int)VariableDecleration_Class.Status.Active : (int)VariableDecleration_Class.Status.InActive;

                    string query = " IF NOT EXISTS (SELECT * FROM WorkingDays WHERE WorkDate = '" + currentDate + "')" +
                                "INSERT INTO WorkingDays(WorkDate, MachineNo, Minute, DayName, WorkDay, Active) VALUES ('" + currentDate + "'," + mcNo + "," + minute + ",'" + currentDay + "','" + currentWorkDay + "'," + currentActiveStatus + ") ";

                    result = (CommonFunctions.ExecutionToDB(query, 3));

                    //UPDATE Order_Info SET Size = '" + sizeNo + "', Dia = '" + dia + "', BodyPart = '" + bodyPart + "', Quantity = " + qty + ", ShipmentDate = '" + shipdate + "', SAM = " + SAMNo + ", Efficiency = " + eff + ", Status = " + status + " WHERE Buyer = '" + buyerName + "' AND Style = '" + styleName +
                }

                if (result == true)
                    LoadGrid();
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

        private void Grid_WorkDays_Info_CellEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Grid_WorkDays_Info_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
