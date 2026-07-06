using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testApp
{
    public partial class XtraUserControl4 : DevExpress.XtraEditors.XtraUserControl
    {
        private List<CUSTOMER> _customers;
        public XtraUserControl4()
        {
            InitializeComponent();

        }
        void bbiSave_ItemClick(object sender, ItemClickEventArgs e)
        {
            int id = Convert.ToInt32(textEdit1.Text);
            using (testBookEntities db = new testBookEntities())
            {
                CUSTOMER customer = db.CUSTOMERs.Find(id);
                if (customer != null)
                {
                    customer.FullName = textEdit3.Text;
                    customer.PhoneNumber = textEdit4.Text;
                    customer.Email = textEdit5.Text;
                    customer.DateOfBirth = dateEdit1.DateTime;
                    customer.Gender = Convert.ToBoolean(comboBoxEdit1.SelectedIndex);
                    customer.CustomerTypeID = Convert.ToInt32(comboBoxEdit2.SelectedIndex +1);

                    db.SaveChanges();
                }
            }

            loadTable();
        }

        private void XtraUserControl4_Load(object sender, EventArgs e)
        {
            loadTable();
            panelControl1.Enabled = false;
        }

        private void loadTable()
        {
            using (testBookEntities db =
                   new testBookEntities())
            {
                _customers = db.CUSTOMERs.Include("CUSTOMER_TYPE").ToList();
                gridControl.DataSource = _customers.Select(c => new
                {
                    c.CustomerID,
                    c.CustomerCode,
                    c.FullName,
                    c.PhoneNumber,
                    c.Email,
                    c.DateOfBirth,
                    c.Gender,
                    c.JoinDate,
                    Type = c.CUSTOMER_TYPE.TypeName
                }).ToList();
            }
        }

        private void gridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            object value = gridView.GetFocusedRowCellValue("CustomerID");
            if (value == null) return;

            int id = Convert.ToInt32(value);
            CUSTOMER selected = _customers.FirstOrDefault(c => c.CustomerID == id);
            if (selected != null)
            {
                textEdit1.Text = selected.CustomerID.ToString();
                textEdit2.Text = selected.CustomerCode;
                textEdit3.Text = selected.FullName;
                textEdit4.Text = selected.PhoneNumber;
                textEdit5.Text = selected.Email;
                dateEdit1.EditValue = selected.DateOfBirth;
                dateEdit2.EditValue = selected.JoinDate;
                comboBoxEdit2.SelectedIndex = selected.CustomerTypeID;
                comboBoxEdit1.SelectedIndex = Convert.ToInt32(selected.Gender);
            }
        }

        private void bbiNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            XtraForm2 form = new XtraForm2();
            form.customerSaved += XtraUserControl4_Load;
            form.Show();
        }

        private void bbiRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            loadTable();
        }

        private void bbiDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            int customerID = Convert.ToInt32(gridView.GetFocusedRowCellValue("CustomerID"));
            DialogResult result = XtraMessageBox.Show(
        "Delete this customer?",
        "Confirm",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                using (testBookEntities db = new testBookEntities())
                {
                    var customer = db.CUSTOMERs.Find(customerID);

                    if (customer != null)
                    {
                        db.CUSTOMERs.Remove(customer);
                        db.SaveChanges();
                    }
                }

                loadTable();
            }
        }

        private void bbiEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            panelControl1.Enabled = !panelControl1.Enabled;
            bbiSave.Enabled = !bbiSave.Enabled;
        }

    }
}
