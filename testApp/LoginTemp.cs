using DevExpress.Utils;
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
using static DevExpress.XtraEditors.Mask.MaskSettings;

namespace testApp
{
    
    public partial class LoginTemp : DevExpress.XtraEditors.XtraForm
    {
        
        public LoginTemp()
        {
            InitializeComponent();
        }

        private void LoginTemp_Load(object sender, EventArgs e)
        {
            textEdit1.Focus();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {

            using (var db = new testBookEntities())
            {
                var user = db.USERS.FirstOrDefault(u => u.Username == textEdit1.Text);
                if (user == null)
                {
                    MessageBox.Show("Invalid username or password");
                    return;
                }
                else if (!BCrypt.Net.BCrypt.Verify(textEdit2.Text, user.PasswordHash))
                {
                    MessageBox.Show("Invalid username or password");
                    return;
                }

                UserSession.CurrentUser = user;

                UserSession.Permission =
                    user.ROLE.PERMISSIONs
                        .Select(x => x.PermissionCode)
                        .ToHashSet();


                userLogin();


            }
        }
        private void Login()
        {
            throw new NotImplementedException();
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            XtraForm4 xtraForm4 = new XtraForm4();
            Hide();
            xtraForm4.Show();
        }

        private void LoginTemp_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                userLogin();
            }
            
        }

        private void userLogin()
        {
            if (UserSession.CurrentUser.ROLE.RoleName == "Cashier")
            {
                XtraForm4 xtraForm4 = new XtraForm4();
                Hide();
                xtraForm4.Show();
            }
            else
            {
                Form1 main = new Form1();

                Hide();
                main.Show();
            }
        }

        private void textEdit1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                textEdit2.Focus();
            }
        }

        private void textEdit2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                simpleButton1_Click(this, new EventArgs());
            }
        }
    }



    public static class UserSession
    {
        public static USER CurrentUser { get; set; }
        public static HashSet<string> Permission = new HashSet<string>();

    }

    public static class PermissionService
    {
        public static bool Has(string permission)
        {
            return UserSession.Permission.Contains(permission);
        }
    }


}