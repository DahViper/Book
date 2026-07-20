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
    public partial class XtraForm4 : DevExpress.XtraEditors.XtraForm
    {
        public class SaleItem
        {
            public int BookID { get; set; }

            public string ISBN { get; set; }

            public string Title { get; set; }

            public int Quantity { get; set; }

            public decimal UnitPrice { get; set; }

            public decimal VATPercent { get; set; }

            public decimal LineTotal
            {
                get
                {
                    return Quantity * UnitPrice;
                }
            }
        }

        private BindingList<SaleItem> saleItems = new BindingList<SaleItem>();
        private List<BOOK> _books;
        public XtraForm4()
        {
            InitializeComponent();
            gridControl2.DataSource = saleItems;
        }

        private void onLoad()
        {
            using (testBookEntities db = new testBookEntities())
            {
                _books = db.BOOKs
                    .ToList();
                var books = db.BOOKs.Select(b => new
                {
                    b.BookID,
                    b.Title,
                }).ToList();
                searchLookUpEdit1.Properties.DataSource = books;
                searchLookUpEdit1.Properties.DisplayMember = "Title";
                searchLookUpEdit1.Properties.ValueMember = "BookId";
                searchLookUpEdit1.Properties.PopulateViewColumns();
            }
        }

        private void searchLookUpEdit1_EditValueChanged(object sender, EventArgs e)
        {
            object value = searchLookUpEdit1.Properties.View.GetFocusedRowCellValue("BookID");

            if (value == null) return;

            int bookId = Convert.ToInt32(value);
            BOOK selected = _books.FirstOrDefault(b => b.BookID == bookId);
            if (selected != null)
            {
                textEdit9.EditValue = selected.BookID;
                textEdit10.Text = selected.ISBN;
                spinEdit1.Properties.MaxValue = Convert.ToInt32(selected.StockQuantity);
                spinEdit1.EditValue = spinEdit1.Properties.MinValue;
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            object value = searchLookUpEdit1.Properties.View.GetFocusedRowCellValue("BookID");

            if (value == null) return;

            int bookId = Convert.ToInt32(value);
            BOOK selected = _books.FirstOrDefault(b => b.BookID == bookId);
            if (selected != null)
            {
                saleItems.Add(new SaleItem
                {
                    BookID = selected.BookID,
                    ISBN = selected.ISBN,
                    Title = selected.Title,
                    Quantity = Convert.ToInt32(spinEdit1.EditValue),
                    UnitPrice = selected.RetailPrice,
                    VATPercent = selected.VatOutPercent
                });
            }
        }

    }
}