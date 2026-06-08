using DevExpress.XtraBars;
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
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadBooks();
            LoadCategories2();
            LoadAuthor();
        }
        void bbiUpdate_ItemClick(object sender, ItemClickEventArgs e)
        {
            int bookId = Convert.ToInt32(gridView.GetFocusedRowCellValue("BookID"));
            DialogResult result = XtraMessageBox.Show(
        "Delete this book?",
        "Confirm",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                using (testBookEntities db = new testBookEntities())
                {
                    var book = db.BOOKs.Find(bookId);

                    if (book != null)
                    {
                        book.Title = textTitle.Text;
                        book.ISBN = textISBN.Text;
                        book.Publisher = textPublisher.Text;
                        book.AuthorID = Convert.ToInt32(ddAuthor1.EditValue);
                        book.CategoryID = Convert.ToInt32(ddCategory1.EditValue);


                        book.CostPrice = Convert.ToDecimal(textPrice.EditValue);
                        book.WholesalePrice = Convert.ToDecimal(textWhole.EditValue);
                        book.RetailPrice = Convert.ToDecimal(textRetail.EditValue);

                        book.VatInPercent = Convert.ToDecimal(comboVat1.EditValue);
                        book.VatOutPercent = Convert.ToDecimal(comboVat2.EditValue);
                        db.SaveChanges();
                    }
                }

                LoadBooks();
            }
        }

        private void bbiEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            int id = Convert.ToInt32(textID.Text);

            using (testBookEntities db = new testBookEntities())
            {
                BOOK book = db.BOOKs.Find(id);

                if (book != null)
                {
                    book.Title = textTitle.Text;
                    book.ISBN = textISBN.Text;
                    book.Publisher = textPublisher.Text;
                    book.AuthorID = Convert.ToInt32(ddAuthor1.EditValue);
                    book.CategoryID = Convert.ToInt32(ddCategory1.EditValue);
                    db.SaveChanges();
                }
            }

            LoadBooks();
        }


        private void LoadBooks()
        {
            using (testBookEntities db =
                   new testBookEntities())
            {
                _books = db.BOOKs
            .Include("AUTHOR")
            .Include("CATEGORY")
            .ToList();

                gridControl.DataSource = _books
                    .Where(b => b != null && b.AUTHOR != null && b.CATEGORY != null)
                    .Select(b => new
                {
                    b.BookID,
                    b.ISBN,
                    b.Title,
                   // b.Publisher,
                    //Author = b.AUTHOR.AuthorName,
                    //Category = b.CATEGORY.CategoryName,
                        b.CostPrice,
                        b.VatInPercent,
                        b.WholesalePrice,
                        b.VatOutPercent,
                        b.RetailPrice

                    }).ToList();
            }
            
        }

        private void LoadBooks2()
        {
            using (testBookEntities db = new testBookEntities())
            {
                var query = db.BOOKs.AsQueryable();

                string keyword = titleSearch.Text.Trim();

                if (!string.IsNullOrWhiteSpace(keyword))
                {
                    query = query.Where(b =>
                        b.Title.Contains(keyword) ||
                        b.ISBN.Contains(keyword));
                }

                if (categorySearch.EditValue != null)
                {
                    int categoryId = Convert.ToInt32(categorySearch.EditValue);

                    query = query.Where(b => b.CategoryID == categoryId);
                }

                if (authorSearch.EditValue != null)
                {
                    int authorId = Convert.ToInt32(authorSearch.EditValue);

                    query = query.Where(b => b.AuthorID == authorId);
                }

                var books = query
                    .Select(b => new
                    {
                        b.BookID,
                        b.Title,
                        b.ISBN,
                        b.CostPrice,
                        b.VatInPercent,
                        b.WholesalePrice,
                        b.VatOutPercent,
                        b.RetailPrice
                    })
                    .ToList();

                gridControl.DataSource = books;
            }
        }

        private void gridView_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            object value = gridView.GetFocusedRowCellValue("BookID");

            if (value == null) return;

            int bookId = Convert.ToInt32(value);
            BOOK selected = _books.FirstOrDefault(b => b.BookID == bookId);

            if (selected != null)
            {
                textID.EditValue = selected.BookID;
                textTitle.Text = selected.Title;
                textISBN.Text = selected.ISBN;
                textPublisher.Text = selected.Publisher;

                ddCategory1.EditValue = selected.AuthorID;
                ddAuthor1.EditValue = selected.CategoryID;
                //textCategory1.Text = selected.AuthorID.ToString();
                //textAuthor1.Text = selected.CategoryID.ToString();

                textPrice.EditValue = selected.CostPrice;
                textRetail.EditValue = selected.RetailPrice;
                textWhole.EditValue = selected.WholesalePrice;

                comboVat1.EditValue = Convert.ToInt32(selected.VatInPercent);
                comboVat2.EditValue = Convert.ToInt32(selected.VatOutPercent);

                textRetailPercent.Text = ((selected.RetailPrice - selected.CostPrice)/100*100).ToString();
                textWholesalePercent.Text = ((selected.WholesalePrice - selected.CostPrice) / 100 * 100).ToString();
            }
        }

        private void LoadCategories2()
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

                ddCategory1.Properties.DataSource = categorySearch.Properties.DataSource =
                    categories;

                ddCategory1.Properties.DisplayMember = categorySearch.Properties.DisplayMember =
                    "CategoryName";

                ddCategory1.Properties.ValueMember = categorySearch.Properties.ValueMember =
                    "CategoryID";

                ddCategory1.Properties.PopulateColumns();
                categorySearch.Properties.PopulateColumns();

                ddCategory1.Properties.Columns["CategoryID"]
                    .Visible = false;


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

                ddAuthor1.Properties.DataSource = authorSearch.Properties.DataSource =
                    authors;

                ddAuthor1.Properties.DisplayMember = authorSearch.Properties.DisplayMember =
                    "AuthorName";

                ddAuthor1.Properties.ValueMember = authorSearch.Properties.ValueMember =
                    "AuthorID";

                ddAuthor1.Properties.PopulateColumns();
                authorSearch.Properties.PopulateColumns();

                ddAuthor1.Properties.Columns["AuthorID"]
                    .Visible = false;

                authorSearch.Properties.Columns["AuthorID"]
                    .Visible = false;


            }
        }


        private void lookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            if (ddCategory1.EditValue != null)
            {
                int categoryID =
                    Convert.ToInt32(ddCategory1.EditValue);

                textCategory1.Text =
                    categoryID.ToString();
            }
        }

        private void ddAuthor1_EditValueChanged(object sender, EventArgs e)
        {
            if (ddAuthor1.EditValue != null)
            {
                int authorID =
                    Convert.ToInt32(ddAuthor1.EditValue);

                textAuthor1.Text =
                    authorID.ToString();
            }
        }

        private void bbiNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            XtraForm1 form = new XtraForm1();
            form.BookSaved += Form1_Load;
            form.Show();
        }

        private void bbiDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            int bookId = Convert.ToInt32(gridView.GetFocusedRowCellValue("BookID"));
            DialogResult result = XtraMessageBox.Show(
        "Delete this book?",
        "Confirm",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                using (testBookEntities db = new testBookEntities())
                {
                    var book = db.BOOKs.Find(bookId);

                    if (book != null)
                    {
                        db.BOOKs.Remove(book);
                        db.SaveChanges();
                    }
                }

                LoadBooks();
            }
        }

        private void titleSearch_EditValueChanged(object sender, EventArgs e)
        {
            LoadBooks2();
        }

        private void categorySearch_EditValueChanged(object sender, EventArgs e)
        {
            LoadBooks2();
        }

        private void authorSearch_EditValueChanged(object sender, EventArgs e)
        {
            LoadBooks2();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            panelControl2.Visible = true;
        }
    }
}