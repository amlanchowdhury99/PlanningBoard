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
    public partial class DiaInfo : Form
    {
        public static string diaName = "";
        public static int rowIndexID = 0;
        public DiaInfo()
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
                if (txtSave.Text.Trim() == "")
                    Str = "SELECT Id,Dia FROM Dia where Id<>0 order by Dia asc";
                else
                    Str = "SELECT Id,Dia FROM Dia where Id<>0 and Dia = '" + txtSave.Text.Trim() + "' order by Dia asc";

                SqlDataAdapter ADAP = new SqlDataAdapter(Str, cn);
                DataSet DS = new DataSet();
                ADAP.Fill(DS, "Dia");
                Grid_Dia_Info.DataSource = DS.Tables["Dia"];
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
            DataGridViewRow row = Grid_Dia_Info.Rows[rowIndex];
            rowIndexID = rowIndex;
            txtUpdate.Text = row.Cells[1].Value.ToString();
            diaName = txtUpdate.Text;

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string diaVal = txtSave.Text.Trim();

                if (txtSave.Text.Trim() == "")
                {
                    MessageBox.Show("Please Enter Dia Information", VariableDecleration_Class.sMSGBOX);
                    txtSave.Focus();
                    return;
                }

                if (CommonFunctions.recordExist("Select * from Dia where Dia ='" + diaVal + "'"))
                {
                    MessageBox.Show("Sorry! Dia Name Already Exist", VariableDecleration_Class.sMSGBOX);
                    txtSave.Focus();
                    return;
                }

                string query = " INSERT INTO Dia (Dia) VALUES ('" + diaVal + "') ";

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

                string diaVal = txtUpdate.Text.Trim();

                if (diaVal == "")
                {
                    MessageBox.Show("Please Enter Dia Information", VariableDecleration_Class.sMSGBOX);
                    txtUpdate.Focus();
                    return;
                }

                int rowID = (int)Grid_Dia_Info.Rows[rowIndexID].Cells[0].Value;
                string query = "Select Id from Dia where Dia ='" + diaName + "'";

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

                query = " UPDATE Dia SET Dia = '" + txtUpdate.Text + "' WHERE Id = " + rowID;
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

        private void txtSave_TextChanged(object sender, EventArgs e)
        {
            txtSave.CharacterCasing = CharacterCasing.Upper;
            LoadAgent();
        }

        private void Grid_Dia_Info_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // make sure user select at least 1 row 
            {
                SetValue(e.RowIndex);
            }
        }

        private void txtSave_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void txtUpdate_TextChanged(object sender, EventArgs e)
        {
            txtUpdate.CharacterCasing = CharacterCasing.Upper;
            TextBox textBox = sender as TextBox;
            if (textBox != null)
            {
                diaName = textBox.Text;
            }
        }
    }
}
