using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testApp
{
    public partial class Form1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {

        private List<BOOK> _books;
        public Form1()
        {
            InitializeComponent();
            documentManager1.ContainerControl = this;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ApplyPermission();
            Text = $"Main Form - {UserSession.CurrentUser.FullName} - {UserSession.CurrentUser.ROLE.RoleName}";
            //LoadBooks();
            //LoadCategories2();
            //LoadAuthor();
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            documentManager1.View.AddDocument("Book", "XtraUserControl1");
            documentManager1.View.AddDocument(new XtraUserControl1());
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            documentManager1.View.AddDocument("Export", "XtraUserControl2");
            documentManager1.View.AddDocument(new XtraUserControl2());
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            documentManager1.View.AddDocument("Import", "XtraUserControl3");
            documentManager1.View.AddDocument(new XtraUserControl3());
        }

        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
        {
            documentManager1.View.AddDocument("Customer", "XtraUserControl4");
            documentManager1.View.AddDocument(new XtraUserControl4());
        }

        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        {
            documentManager1.View.AddDocument("User", "XtraUserControl5");
            documentManager1.View.AddDocument(new XtraUserControl5());
        }


        private void ApplyPermission()
        {
            barButtonItem1.Enabled = PermissionService.Has("BOOK_VIEW");
            barButtonItem3.Enabled = PermissionService.Has("IMPORT_VIEW");
            barButtonItem4.Enabled = PermissionService.Has("EXPORT_VIEW");
            barButtonItem5.Enabled = PermissionService.Has("CUSTOMER_VIEW");
        }

        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
        {
            documentManager1.View.AddDocument("User", "XtraUserControl6");
            documentManager1.View.AddDocument(new XtraUserControl6());
        }

        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        {
            documentManager1.View.AddDocument("User", "XtraUserControl7");
            documentManager1.View.AddDocument(new XtraUserControl7());
        }
    }
}