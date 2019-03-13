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
    public partial class MachineEntryForm : Form
    {
        public MachineEntryForm()
        {
            InitializeComponent();
            LoadGrid();
        }

        private void MachineEntryForm_Load(object sender, EventArgs e)
        {
            machineInfoDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            machineInfoDataGridView.MultiSelect = false;
        }

        private void SaveMachineInfo_Click(object sender, EventArgs e)
        {

            try
            {
                int MachineNo = Convert.ToInt32(MNotextBox.Text);
                double MachineDia = Convert.ToDouble(MDiatextBox.Text);
                int MachineStatus = (int)Enum.Parse(typeof(VariableDecleration_Class.Status), MStatuscomboBox.Text);

                string query = " IF EXISTS (SELECT * FROM Machine_Info WHERE MachineNo = " + MachineNo + ") UPDATE Machine_Info SET MachineDia = " + MachineDia + ", Status = " + MachineStatus + " WHERE MachineNo = " + MachineNo +
                                " ELSE INSERT INTO Machine_Info(MachineNo, MachineDia, Status) VALUES (" + MachineNo + "," + MachineDia + ",'" + MachineStatus + "') ";

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

        private void UpdateMachineInfo_Click(object sender, EventArgs e)
        {
            try
            {
                int MachineNo = Convert.ToInt32(MNotextBox.Text);
                double MachineDia = Convert.ToDouble(MDiatextBox.Text);
                int MachineStatus = (int)Enum.Parse(typeof(VariableDecleration_Class.Status), MStatuscomboBox.Text);

                string query = " IF EXISTS (SELECT * FROM Machine_Info WHERE MachineNo = " + MachineNo + ") UPDATE Machine_Info SET MachineDia = " + MachineDia + ", Status = " + MachineStatus + " WHERE MachineNo = " + MachineNo;

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
                MNotextBox.ReadOnly = false;
            }
        }

        private void LoadGrid()
        {

            try
            {
                int SL = 1;
                int S1 = 0; int S2 = 0; double S3 = 0.0; string S4 = "";

                string query = "SELECT * FROM Machine_Info";

                SqlDataReader reader = CommonFunctions.GetFromDB(query);

                if (reader.HasRows)
                {
                    machineInfoDataGridView.Rows.Clear();
                    while (reader.Read())
                    {
                        S1 = SL;
                        S2 = reader.IsDBNull(reader.GetOrdinal("MachineNo")) == true ? 0000 : Convert.ToInt32(reader["MachineNo"]);
                        S3 = reader.IsDBNull(reader.GetOrdinal("MachineDia")) == true ? 0.00 : Convert.ToDouble(reader["MachineDia"]);
                        S4 = Enum.GetName(typeof(VariableDecleration_Class.Status), reader.IsDBNull(reader.GetOrdinal("Status")) == true ? (int)VariableDecleration_Class.Status.InActive : Convert.ToInt32(reader["Status"]));

                        machineInfoDataGridView.Rows.Add(S1, S2, S3, S4);
                        SL++;
                    }
                }

                MStatuscomboBox.DataSource = Enum.GetValues(typeof(VariableDecleration_Class.Status));

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

        private void machineInfoDataGridView_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void machineInfoDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // make sure user select at least 1 row 
            {
                SetValue(e.RowIndex);
            }
        }

        private void machineInfoDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // make sure user select at least 1 row 
            {
                SetValue(e.RowIndex);
            }
        }

        private void machineInfoDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // make sure user select at least 1 row 
            {
                SetValue(e.RowIndex);
            }
        }

        private void SetValue(int rowIndex)
        {
            DataGridViewRow row = machineInfoDataGridView.Rows[rowIndex];

            MNotextBox.Text = row.Cells[1].Value.ToString();
            MDiatextBox.Text = row.Cells[2].Value.ToString();
            MStatuscomboBox.Text = row.Cells[3].Value.ToString();

            MNotextBox.ReadOnly = true;
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
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void MStatuscomboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void MachineEntryForm_MouseHover(object sender, EventArgs e)
        {

        }

    }
}
