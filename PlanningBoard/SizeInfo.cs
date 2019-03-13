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
    public partial class SizeInfo : Form
    {

        public static string sizeName = "";
        public static int rowIndexID = 0;

        public SizeInfo()
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
                    Str = "SELECT Id,SizeName FROM Size where Id<>0 order by SizeName asc";
                else
                    Str = "SELECT Id,SizeName FROM Size where Id<>0 and SizeName like '%" + txtSave.Text + "%' order by SizeName asc";

                SqlDataAdapter ADAP = new SqlDataAdapter(Str, cn);
                DataSet DS = new DataSet();
                ADAP.Fill(DS, "Size");
                Grid_Size_Info.DataSource = DS.Tables["Size"];
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

        private void SetValue(int rowIndex)
        {
            DataGridViewRow row = Grid_Size_Info.Rows[rowIndex];
            rowIndexID = rowIndex;
            txtUpdate.Text = row.Cells[1].Value.ToString();
            sizeName = txtUpdate.Text;

        }

        private void Grid_Size_Info_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // make sure user select at least 1 row 
            {
                SetValue(e.RowIndex);
            }
        }

        private void groupBox1_TextChanged(object sender, EventArgs e)
        {
            LoadAgent();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtSave.Text.Trim() == "")
                {
                    MessageBox.Show("Please Enter Size Information", VariableDecleration_Class.sMSGBOX);
                    txtSave.Focus();
                    return;
                }

                if (CommonFunctions.recordExist("Select * from Size where SizeName ='" + txtSave.Text + "'"))
                {
                    MessageBox.Show("Sorry! Size Name Already Exist", VariableDecleration_Class.sMSGBOX);
                    txtSave.Focus();
                    return;
                }

                string query = " INSERT INTO Size (SizeName) VALUES ('" + txtSave.Text + "') ";

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

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUpdate.Text.Trim() == "")
                {
                    MessageBox.Show("Please Enter Size Information", VariableDecleration_Class.sMSGBOX);
                    txtUpdate.Focus();
                    return;
                }

                int rowID = (int)Grid_Size_Info.Rows[rowIndexID].Cells[0].Value;
                string query = "Select Id from Size where SizeName ='" + sizeName + "'";

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

                query = " UPDATE Size SET SizeName = '" + txtUpdate.Text + "' WHERE Id = " + rowID;
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

        private void txtUpdate_TextChanged(object sender, EventArgs e)
        {
            txtUpdate.CharacterCasing = CharacterCasing.Upper;
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                sizeName = textBox.Text;
            }
        }

        private void txtSave_TextChanged(object sender, EventArgs e)
        {
            txtSave.CharacterCasing = CharacterCasing.Upper;
            LoadAgent();
        }
    }
}
