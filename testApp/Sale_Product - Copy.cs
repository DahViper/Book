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
    public partial class Sale_Product : DevExpress.XtraEditors.XtraForm
    {
        public string productCode {  get; private set; }
        public string quantity {  get; private set; }
        public Sale_Product()
        {
            InitializeComponent();
            textEdit1.Focus();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            productCode  = textEdit1.Text;
            quantity = textEdit2.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void textEdit1_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    textEdit2.Focus();
                    break;
                case Keys.Down:
                    textEdit2.Focus();
                    break ;
            }
        }

        private void textEdit2_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    simpleButton1_Click(this, new EventArgs());
                    break;
                case Keys.Up:
                    textEdit1.Focus();
                    break;
            }
        }
    }
}