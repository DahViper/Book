using DevExpress.XtraEditors;
using DevExpress.XtraReports.Native;
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
    public partial class XtraForm2 : DevExpress.XtraEditors.XtraForm
    {
        public event EventHandler customerSaved;
        public XtraForm2()
        {
            InitializeComponent();
        }


        private void simpleButton1_Click(object sender, EventArgs e)
        {
            using (testBookEntities db = new testBookEntities())
            {
                if (XtraMessageBox.Show("Do you want to add a new value?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    CUSTOMER customer = new CUSTOMER()
                    {
                        FullName = textEdit1.Text,
                        PhoneNumber = textEdit2.Text,
                        Email = textEdit3.Text,
                        Gender = Convert.ToBoolean(radioGroup1.EditValue),
                        DateOfBirth = dateEdit1.DateTime,
                        CustomerCode = "temp",
                        CustomerTypeID = Convert.ToInt32(radioGroup2.EditValue),
                        JoinDate = DateTime.Now
                    };
                    db.CUSTOMERs.Add(customer);
                    db.SaveChanges();
                    customer.CustomerCode = $"CUST{customer.CustomerID:D3}";
                    db.SaveChanges();
                    customerSaved?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private void XtraForm2_Load(object sender, EventArgs e)
        {

        }
    }
}