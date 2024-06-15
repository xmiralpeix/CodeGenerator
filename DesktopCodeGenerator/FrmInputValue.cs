using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopCodeGenerator.UI
{

    public partial class FrmInputValue : Form
    {
        public FrmInputValue()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtValue.Text)) {
                MessageBox.Show("No se ha informado ningún valor");
                return;
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
