using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Filtering.Templates;
using Microsoft.IdentityModel.Tokens;
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
    public partial class XtraUserControl7 : DevExpress.XtraEditors.XtraUserControl
    {
        private List<VOUCHER> _vounchers;
        public XtraUserControl7()
        {
            InitializeComponent();

        }
        void bbiSave_ItemClick(object sender, ItemClickEventArgs e)
        {

            int id = Convert.ToInt32(textEdit1.Text);
                using (testBookEntities db = new testBookEntities())
                {
                    VOUCHER voucher = db.VOUCHERs.Find(id);
                    if (voucher != null)
                    {
                    voucher.VoucherCode = textEdit2.Text;
                    voucher.DiscountType = comboBoxEdit1.Text;
                    voucher.DiscountValue = Convert.ToDecimal(textEdit3.Text);
                    voucher.MinimumSubtotal = Convert.ToDecimal(textEdit4.Text);
                    if (textEdit5.Text.IsNullOrEmpty())
                    {
                        voucher.MaximumDiscount = null;
                    }
                    else
                    {
                        voucher.MaximumDiscount = Convert.ToDecimal(textEdit5.Text);
                    }
                        voucher.StartDate = Convert.ToDateTime(dateEdit1.EditValue);
                    voucher.EndDate = Convert.ToDateTime(dateEdit1.EditValue);

                    if (textEdit6.Text.IsNullOrEmpty())
                    {
                        voucher.UsageLimit = null;
                    }
                    else
                    {
                        voucher.UsageLimit = Convert.ToInt32(textEdit6.Text);
                    }
                        

                        db.SaveChanges();
                    }
                }

                loadTable();
            
        }

        private void XtraUserControl7_Load(object sender, EventArgs e)
        {
            loadTable();
            panelControl1.Enabled = false;
        }

        private void loadTable()
        {
            using (testBookEntities db =
                   new testBookEntities())
            {
                _vounchers = db.VOUCHERs.ToList();
                gridControl.DataSource = _vounchers.Select(v => new
                {
                    v.VoucherID,
                    v.VoucherCode,
                    v.DiscountType,
                    v.DiscountValue,
                    v.MinimumSubtotal,
                    v.MaximumDiscount,
                    v.StartDate,
                    v.EndDate,
                    v.IsActive,
                }).ToList();

            }
            gridView.Columns["VoucherID"].Visible = false;
            gridView.Columns["IsActive"].OptionsColumn.AllowEdit = true;
        }

        private void gridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            object value = gridView.GetFocusedRowCellValue("VoucherID");
            if (value == null) return;

            int id = Convert.ToInt32(value);
            var selected = _vounchers.FirstOrDefault(v => v.VoucherID == id);
            if (selected != null)
            {
                textEdit1.Text = selected.VoucherID.ToString();
                textEdit3.Text = selected.DiscountValue.ToString();
                textEdit2.Text = selected.VoucherCode.ToString();
                comboBoxEdit1.Text = selected.DiscountType.ToString();
                textEdit4.Text = selected.MinimumSubtotal.ToString();
                textEdit5.Text = selected.MaximumDiscount.ToString();
                textEdit6.Text = selected.UsageLimit.ToString();
                textEdit7.Text = selected.UsedCount.ToString();
                dateEdit1.Text = selected.StartDate.ToString();
                dateEdit2.Text = selected.EndDate.ToString();
            }
        }

        private void bbiNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (testBookEntities db = new testBookEntities()) 
            {
                VOUCHER voucher = new VOUCHER()
                {
                    VoucherCode = $"TEMP{gridView.DataRowCount + 1}$",
                    DiscountType = "Percent",
                    DiscountValue = 0,
                    MinimumSubtotal = 0,
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now,
                    UsedCount = 0,
                    IsActive = false
                };
                db.VOUCHERs.Add(voucher);
                db.SaveChanges();
                
            }
            loadTable();
            gridView.MoveLast();
        }

        private void bbiRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            loadTable();
        }

        private void bbiDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            int id = Convert.ToInt32(gridView.GetFocusedRowCellValue("VoucherID"));
            DialogResult result = XtraMessageBox.Show(
        "Delete this data?",
        "Confirm",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                using (testBookEntities db = new testBookEntities())
                {
                    var voucher = db.VOUCHERs.Find(id);

                    if (voucher != null)
                    {
                        db.VOUCHERs.Remove(voucher);
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

        private void dateEdit1_EditValueChanged(object sender, EventArgs e)
        {

        }


    }
}
