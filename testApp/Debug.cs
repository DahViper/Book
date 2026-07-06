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
    public partial class Debug : DevExpress.XtraEditors.XtraForm
    {
        public Debug()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Form1 myForm = new Form1();
            myForm.ShowDialog();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            XtraUserControl1 myForm = new XtraUserControl1();
            myForm.Show();
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            XtraUserControl2 myForm = new XtraUserControl2();
            myForm.Show();
        }
    }
}