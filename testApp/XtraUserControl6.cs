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
    public partial class XtraUserControl6 : DevExpress.XtraEditors.XtraUserControl
    {
        private List<USER> _users;
        private List<BOOK> _books;
        private DateTime? pStart;
        private DateTime? pEnd;
        private decimal discount;
        public XtraUserControl6()
        {
            InitializeComponent();

        }
        void bbiSave_ItemClick(object sender, ItemClickEventArgs e)
        {

            int id = Convert.ToInt32(textEdit1.Text);
                using (testBookEntities db = new testBookEntities())
                {
                    BOOK book = db.BOOKs.Find(id);
                    if (book != null)
                    {
                        book.PromotionPercent = discount;
                        book.PromotionStart = pStart;
                        book.PromotionEnd = pEnd;
                        db.SaveChanges();
                    }
                }

                loadTable();
            
        }

        private void XtraUserControl6_Load(object sender, EventArgs e)
        {
            loadTable();
            panelControl1.Enabled = false;
        }

        private void loadTable()
        {
            using (testBookEntities db =
                   new testBookEntities())
            {
                _books = db.BOOKs.ToList();
                gridControl.DataSource = _books.Select(b => new
                {
                    b.BookID,
                    b.Title,
                    b.PromotionPercent,
                    b.PromotionStart,
                    b.PromotionEnd
                }).ToList();

            }
            gridView.Columns["BookID"].Visible = false;
        }

        private void gridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            object value = gridView.GetFocusedRowCellValue("BookID");
            if (value == null) return;

            int id = Convert.ToInt32(value);
            var selected = _books.FirstOrDefault(c => c.BookID == id);
            if (selected != null)
            {
                textEdit1.Text = selected.BookID.ToString();
                textEdit3.Text = selected.PromotionPercent.ToString();
                discount = selected.PromotionPercent;
                textEdit2.Text = selected.Title.ToString();
                dateEdit1.Text = selected.PromotionStart.ToString();
                pStart = selected.PromotionStart;
                dateEdit2.Text = selected.PromotionEnd.ToString();
                pEnd = selected.PromotionEnd;
            }
        }

        private void bbiNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            XtraForm3 form = new XtraForm3();
            form.userSaved += XtraUserControl6_Load;
            form.Show();
        }

        private void bbiRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            loadTable();
        }

        private void bbiDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            int userID = Convert.ToInt32(gridView.GetFocusedRowCellValue("UserID"));
            DialogResult result = XtraMessageBox.Show(
        "Delete this user?",
        "Confirm",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                using (testBookEntities db = new testBookEntities())
                {
                    var user = db.USERS.Find(userID);

                    if (user != null)
                    {
                        db.USERS.Remove(user);
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
            discount = Convert.ToDecimal(textEdit3.Text);
            pStart = dateEdit1.EditValue as DateTime?;
            pEnd = dateEdit2.EditValue as DateTime?;
        }
    }
}
