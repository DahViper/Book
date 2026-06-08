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
    public partial class XtraForm1 : DevExpress.XtraEditors.XtraForm
    {
        public event EventHandler BookSaved;
        public XtraForm1()
        {
            InitializeComponent();
        }

        private void XtraForm1_Load(object sender, EventArgs e)
        {
            LoadCategories();
            LoadAuthor();
        }

        private void lookUpEdit1_ProcessNewValue(object sender, DevExpress.XtraEditors.Controls.ProcessNewValueEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(e.DisplayValue?.ToString()))
                return;
            if (XtraMessageBox.Show("Do you want to add a new value?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                using (testBookEntities db = new testBookEntities())
                {
                    string authorName = e.DisplayValue.ToString().Trim();

                    AUTHOR existing = db.AUTHORs
                        .FirstOrDefault(a => a.AuthorName == authorName);

                    int authorId;

                    if (existing != null)
                    {
                        authorId = existing.AuthorID;
                    }
                    else 
                    {
                        AUTHOR author = new AUTHOR
                        {
                            AuthorName = authorName
                        };

                        db.AUTHORs.Add(author);
                        db.SaveChanges();

                        authorId = author.AuthorID;
                    }

                    LoadAuthor();

                    lkAuthor1.EditValue = authorId;

                    e.Handled = true;
                }
             }
        }

        private void LoadAuthor()
        {
            using (testBookEntities db =
           new testBookEntities())
            {
                var authors = db.AUTHORs
                    .Select(c => new
                    {
                        c.AuthorID,
                        c.AuthorName
                    })
                    .ToList();

                lkAuthor1.Properties.DataSource =
                    authors;

                lkAuthor1.Properties.DisplayMember =
                    "AuthorName";

                lkAuthor1.Properties.ValueMember =
                    "AuthorID";

                lkAuthor1.Properties.PopulateColumns();

                lkAuthor1.Properties.Columns["AuthorID"]
                    .Visible = false;
            }
        }

        private void LoadCategories()
        {
            using (testBookEntities db =
           new testBookEntities())
            {
                var categories = db.CATEGORies
                    .Select(c => new
                    {
                        c.CategoryID,
                        c.CategoryName
                    })
                    .ToList();

                lkCategory1.Properties.DataSource =
                    categories;

                lkCategory1.Properties.DisplayMember =
                    "CategoryName";

                lkCategory1.Properties.ValueMember =
                    "CategoryID";

                lkCategory1.Properties.PopulateColumns();

                lkCategory1.Properties.Columns["CategoryID"]
                    .Visible = false;
            }
        }


        private void simpleButton1_Click(object sender, EventArgs e)
        {
            using (testBookEntities db = new testBookEntities())
            {
                BOOK existing = db.BOOKs
                        .FirstOrDefault(a => a.ISBN == textISBN.Text);
                if (existing != null)
                {
                    XtraMessageBox.Show("Existing Data!", "OK", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (XtraMessageBox.Show("Do you want to add a new value?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    BOOK book = new BOOK()
                    {
                        ISBN = textISBN.Text,
                        Title = textTitle.Text,
                        Publisher = textPublisher.Text,
                        AuthorID = Convert.ToInt32(lkAuthor1.EditValue),
                        CategoryID = Convert.ToInt32(lkCategory1.EditValue)
                    }; 
                    db.BOOKs .Add(book);
                    db.SaveChanges();
                    BookSaved?.Invoke(this, EventArgs.Empty);
                }
            }
        }
    }
}