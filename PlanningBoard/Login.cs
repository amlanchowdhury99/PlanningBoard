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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }


        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (txtID.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter User Name", VariableDecleration_Class.sMSGBOX);
                txtID.Focus();
                return;
            }

            if (txtPass.Text.Trim() == "")
            {
                MessageBox.Show("Please Enter Password", VariableDecleration_Class.sMSGBOX);
                txtPass.Focus();
                return;
            }
            if (CommonFunctions.recordExist("select * from Employee_Info where UserName='" + txtID.Text.Trim() + "' and Password='" + txtPass.Text + "'"))
            {
                VariableDecleration_Class.sLoginStatus = 1;

                string connectionStr = ConnectionManager.connectionString;
                SqlCommand cm = new SqlCommand();
                SqlConnection cn = new SqlConnection(connectionStr);

                try
                {
                    cn.Open();
                    cm.Connection = cn;
                    cm.CommandText = "SELECT * FROM Employee_Info where UserName='" + txtID.Text.Trim() + "' and Password='" + txtPass.Text.Trim() + "'";
                    SqlDataReader reader;
                    reader = cm.ExecuteReader();
                    while (reader.Read())
                    {
                        VariableDecleration_Class.sUserType = ((VariableDecleration_Class.UserType)(Convert.ToInt32(reader["UserType"]))).ToString();
                        VariableDecleration_Class.sUserName = (reader["UserName"]).ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    cn.Close();
                }

                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid User ID or Password", VariableDecleration_Class.sMSGBOX);
                txtID.Focus();
                return;
            }

        }
        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtPass_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnConnect_Click(this, e);
            }

        }

        private void txtID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtPass.Focus();
            }
        }
    }
}
