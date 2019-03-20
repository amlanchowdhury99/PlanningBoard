using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlanningBoard
{
    public partial class DeleteOrderForm : Form
    {
        public Boolean dltSingle = true;

        public DeleteOrderForm()
        {
            InitializeComponent();
        }

        private void DeleteOrderForm_Load(object sender, EventArgs e)
        {
            DeleteSingle.Checked = true;
            DeleteAll.Checked = false;
        }

        private void DeleteAll_Click(object sender, EventArgs e)
        {
            DeleteSingle.Checked = false;
            DeleteAll.Checked = true;
            dltSingle = false;
        }

        private void DeleteSingle_Click(object sender, EventArgs e)
        {
            DeleteAll.Checked = false;
            DeleteSingle.Checked = true;
            dltSingle = true;
        }

        private void OkBtn_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
