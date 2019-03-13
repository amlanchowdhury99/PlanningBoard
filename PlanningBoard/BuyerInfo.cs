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
    public partial class BuyerInfo : Form
    {

        public static string buyerName = "";
        public static int rowIndexID = 0;

        public BuyerInfo()
        {
            InitializeComponent();
            LoadAgent();
        }

        private void LoadAgent()
        {
            string connectionStr = ConnectionManager.connectionString;
            SqlCommand cm = new SqlCommand();
            SqlConnection cn = new SqlConnection(connectionStr);

            try
            {
                cn.Open();
                string Str = "";
                if (txtSave.Text == "")
                    Str = "SELECT Id,BuyerName FROM Buyer where Id<>0 order by BuyerName asc";
                else
                    Str = "SELECT Id,BuyerName FROM Buyer where Id<>0 and BuyerName like '%" + txtSave.Text + "%' order by BuyerName asc";

                SqlDataAdapter ADAP = new SqlDataAdapter(Str, cn);
                DataSet DS = new DataSet();
                ADAP.Fill(DS, "Buyer");
                Grid_Buyer_Info.DataSource = DS.Tables["Buyer"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally
            {
                cn.Close();
            }

        }

        private void setRowNumber(DataGridView dgv)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.HeaderCell.Value = String.Format("{0}", row.Index + 1);
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUpdate.Text.Trim() == "")
                {
                    MessageBox.Show("Please Enter Buyer Name", VariableDecleration_Class.sMSGBOX);
                    txtUpdate.Focus();
                    return;
                }

                int rowID = (int)Grid_Buyer_Info.Rows[rowIndexID].Cells[0].Value;
                string query = "Select Id from Buyer where BuyerName ='" + buyerName + "'";

                SqlDataReader reader = CommonFunctions.GetFromDB(query);
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (rowID != (int)reader["Id"])
                        {
                            MessageBox.Show("Sorry! this record Already Exists", VariableDecleration_Class.sMSGBOX);
                            txtUpdate.Focus();
                            return;
                        }
                    }
                }

                query = " UPDATE Buyer SET BuyerName = '" + txtUpdate.Text + "' WHERE Id = " + rowID;
                if (CommonFunctions.ExecutionToDB(query, 2))
                {
                    txtSave.Text = "";
                    txtUpdate.Text = "";
                    LoadAgent();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

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
                if (txtSave.Text.Trim() == "")
                {
                    MessageBox.Show("Please Enter Buyer Information", VariableDecleration_Class.sMSGBOX);
                    txtSave.Focus();
                    return;
                }

                if (CommonFunctions.recordExist("Select * from Buyer where BuyerName ='" + txtSave.Text + "'"))
                {
                    MessageBox.Show("Sorry! Buyer Name Already Exist", VariableDecleration_Class.sMSGBOX);
                    txtSave.Focus();
                    return;
                }

                string query = " INSERT INTO Buyer (BuyerName) VALUES ('" + txtSave.Text + "') ";

                if (CommonFunctions.ExecutionToDB(query, 1))
                {
                    txtSave.Text = "";
                    txtUpdate.Text = "";
                    LoadAgent();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally
            {
                if (CommonFunctions.connection.State == ConnectionState.Open)
                {
                    CommonFunctions.connection.Close();
                }
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Grid_Buyer_Info_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // make sure user select at least 1 row 
            {
                SetValue(e.RowIndex);
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void txtSave_TextChanged(object sender, EventArgs e)
        {
            txtSave.CharacterCasing = CharacterCasing.Upper;
            LoadAgent();
        }

        private void SetValue(int rowIndex)
        {
            DataGridViewRow row = Grid_Buyer_Info.Rows[rowIndex];
            rowIndexID = rowIndex;
            txtUpdate.Text = row.Cells[1].Value.ToString();
            buyerName = txtUpdate.Text;

        }

        private void txtUpdate_TextChanged(object sender, EventArgs e)
        {
            txtUpdate.CharacterCasing = CharacterCasing.Upper;
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                buyerName = textBox.Text;
            }
        }

    }
}
