using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testApp
{
    public partial class Sale_Phone : DevExpress.XtraEditors.XtraForm
    {
        public string phoneNum {  get; private set; }
        public Sale_Phone()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            phoneNum = textEdit1.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void textEdit1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                simpleButton1_Click(this, new EventArgs());
            }
        }
    }
}