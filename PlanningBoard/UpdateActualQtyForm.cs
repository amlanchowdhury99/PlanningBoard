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

namespace PlanningBoard
{
    public partial class UpdateActualQtyForm : Form
    {
        public int PreVal = -1;
        public int NewVal = -1;
        public string OrderIDs = "";
        public DateTime TaskDate = DateTime.Now;
        public int MachineNo = 0;

        public UpdateActualQtyForm(string orderIds, int mcNo, DateTime Date)
        {
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
                string S1 = ""; string S2 = ""; string S3 = ""; string S4 = ""; string S5 = ""; string S6 = ""; string S7 = ""; string S8 = ""; string S9 = "";
                string query = "SELECT BuyerName, StyleName, SizeName, DiaName, PartName, PlanQty, ActualQty, OrderID  FROM LoadWithActualQtyView WHERE MachineNo = " + MachineNo + " AND TaskDate = '" + TaskDate + "'";
                SqlDataReader reader = CommonFunctions.GetFromDB(query);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        S1 = SL.ToString();
                        S2 = reader.IsDBNull(reader.GetOrdinal("BuyerName")) == true ? "Not Defined" : reader["BuyerName"].ToString();
                        S3 = reader.IsDBNull(reader.GetOrdinal("StyleName")) == true ? "Not Defined" : reader["StyleName"].ToString();
                        S4 = reader.IsDBNull(reader.GetOrdinal("SizeName")) == true ? "Not Defined" : reader["SizeName"].ToString();
                        S5 = reader.IsDBNull(reader.GetOrdinal("DiaName")) == true ? "Not Defined" : reader["DiaName"].ToString();
                        S6 = reader.IsDBNull(reader.GetOrdinal("PartName")) == true ? "Not Defined" : reader["PartName"].ToString();
                        S7 = reader.IsDBNull(reader.GetOrdinal("PlanQty")) == true ? "Not Defined" : reader["PlanQty"].ToString();
                        S8 = reader.IsDBNull(reader.GetOrdinal("ActualQty")) == true ? S7 : reader["ActualQty"].ToString();
                        S9 = reader.IsDBNull(reader.GetOrdinal("OrderID")) == true ? "Not Defined" : reader["OrderID"].ToString();

                        actualQtyDataGridView.Rows.Add(S1, S2, S3, S4, S5, S6, S7, S8, S9);
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

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (actualQtyDataGridView.Rows.Count > 0)
                {
                    foreach (DataGridViewRow row in actualQtyDataGridView.Rows)
                    {
                        string connectionStr = ConnectionManager.connectionString;
                        SqlCommand cm = new SqlCommand();
                        SqlConnection cn = new SqlConnection(connectionStr);
                        cn.Open();
                        cm.Connection = cn;

                        int actualQty = Convert.ToInt32(row.Cells[7].Value);
                        int orderID = Convert.ToInt32(row.Cells[8].Value);
                        cm.CommandText = "UPDATE PlanTable SET ActualQty = " + actualQty + " WHERE OrderID = " + orderID + " AND MachineNo = " + MachineNo + " AND TaskDate = '" + TaskDate + "'";

                        cm.ExecuteNonQuery();
                        cn.Close();
                    }
                    MessageBox.Show("Updated Successfully!!!");
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
                return;
            }
        }
    }
}
