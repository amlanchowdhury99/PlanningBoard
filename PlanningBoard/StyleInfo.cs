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
    public partial class StyleInfo : Form
    {

        public static string styleName = "";
        public static int rowIndexID = 0;

        public StyleInfo()
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
                    Str = "SELECT Id,StyleName FROM Style where Id<>0 order by StyleName asc";
                else
                    Str = "SELECT Id,StyleName FROM Style where Id<>0 and StyleName like '%" + txtSave.Text + "%' order by StyleName asc";

                SqlDataAdapter ADAP = new SqlDataAdapter(Str, cn);
                DataSet DS = new DataSet();
                ADAP.Fill(DS, "Style");
                Grid_Style_Info.DataSource = DS.Tables["Style"];
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

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void SetValue(int rowIndex)
        {
            DataGridViewRow row = Grid_Style_Info.Rows[rowIndex];
            rowIndexID = rowIndex;
            txtUpdate.Text = row.Cells[1].Value.ToString();
            styleName = txtUpdate.Text;

        }

        private void BtnSave_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (txtSave.Text.Trim() == "")
                {
                    MessageBox.Show("Please Enter Style Information", VariableDecleration_Class.sMSGBOX);
                    txtSave.Focus();
                    return;
                }

                if (CommonFunctions.recordExist("Select * from Style where StyleName ='" + txtSave.Text + "'"))
                {
                    MessageBox.Show("Sorry! Style Name Already Exist", VariableDecleration_Class.sMSGBOX);
                    txtSave.Focus();
                    return;
                }

                string query = " INSERT INTO Style (StyleName) VALUES ('" + txtSave.Text + "') ";

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

        private void BtnUpdate_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (txtUpdate.Text.Trim() == "")
                {
                    MessageBox.Show("Please Enter Style Information", VariableDecleration_Class.sMSGBOX);
                    txtUpdate.Focus();
                    return;
                }

                int rowID = (int)Grid_Style_Info.Rows[rowIndexID].Cells[0].Value;
                string query = "Select Id from Style where StyleName ='" + styleName + "'";

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

                query = " UPDATE Style SET StyleName = '" + txtUpdate.Text + "' WHERE Id = " + rowID;
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

        private void BtnExit_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSave_TextChanged_1(object sender, EventArgs e)
        {
            txtSave.CharacterCasing = CharacterCasing.Upper;
            LoadAgent();
        }

        private void Grid_Style_Info_CellClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // make sure user select at least 1 row 
            {
                SetValue(e.RowIndex);
            }
        }

        private void txtUpdate_TextChanged(object sender, EventArgs e)
        {
            txtUpdate.CharacterCasing = CharacterCasing.Upper;
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                styleName = textBox.Text;
            }
        }
    }
}
