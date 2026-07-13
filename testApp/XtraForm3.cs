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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace testApp
{
    public partial class XtraForm3 : DevExpress.XtraEditors.XtraForm
    {
        public event EventHandler userSaved;
        public XtraForm3()
        {
            InitializeComponent();
        }


        private void simpleButton1_Click(object sender, EventArgs e)
        {
            using (testBookEntities db = new testBookEntities())
            {
                if (string.IsNullOrWhiteSpace(textEdit1.Text) ||
        string.IsNullOrWhiteSpace(textEdit2.Text) ||
        string.IsNullOrWhiteSpace(textEdit3.Text))
                {
                    XtraMessageBox.Show(
                        "Please fill in all data fields before saving.",
                        "Validation",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }


                if (XtraMessageBox.Show("Do you want to add a new value?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    USER user = new USER()
                    {
                        FullName = textEdit1.Text,
                        Username = textEdit2.Text,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(textEdit3.Text),
                        RoleID = Convert.ToInt32(radioGroup2.EditValue)
                    };
                    db.USERS.Add(user);
                    db.SaveChanges();
                    userSaved?.Invoke(this, EventArgs.Empty);
                }

                
            }
        }

    }
}