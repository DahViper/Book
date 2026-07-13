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
    public partial class XtraUserControl5 : DevExpress.XtraEditors.XtraUserControl
    {
        private List<USER> _users;
        public XtraUserControl5()
        {
            InitializeComponent();

        }
        void bbiSave_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textEdit2.Text) ||
        string.IsNullOrWhiteSpace(textEdit3.Text) ||
        string.IsNullOrWhiteSpace(textEdit4.Text))
            {
                XtraMessageBox.Show(
                    "Please fill in all data fields before saving.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(textEdit1.Text);
                using (testBookEntities db = new testBookEntities())
                {
                    USER user = db.USERS.Find(id);
                    if (user != null)
                    {
                        user.FullName = textEdit2.Text;
                        user.Username = textEdit3.Text;
                        user.RoleID = Convert.ToInt32(comboBoxEdit1.SelectedIndex +2);
                        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(textEdit4.Text);
                        db.SaveChanges();
                    }
                }

                loadTable();
            
        }

        private void XtraUserControl5_Load(object sender, EventArgs e)
        {
            loadTable();
            panelControl1.Enabled = false;
        }

        private void loadTable()
        {
            using (testBookEntities db =
                   new testBookEntities())
            {
                _users = db.USERS.Include("ROLE").ToList();
                gridControl.DataSource = _users.Select(u => new
                {
                    u.UserID,
                    u.Username,
                    u.FullName,
                    u.PasswordHash,
                    Role = u.ROLE.RoleName
                }).ToList();
            }
        }

        private void gridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            object value = gridView.GetFocusedRowCellValue("UserID");
            if (value == null) return;

            int id = Convert.ToInt32(value);
            USER selected = _users.FirstOrDefault(c => c.UserID == id);
            if (selected != null)
            {
                textEdit1.Text = selected.UserID.ToString();
                textEdit3.Text = selected.Username.ToString();
                textEdit2.Text = selected.FullName.ToString();
            }
        }

        private void bbiNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            XtraForm3 form = new XtraForm3();
            form.userSaved += XtraUserControl5_Load;
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

    }
}
