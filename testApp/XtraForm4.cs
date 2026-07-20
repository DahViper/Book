using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static testApp.XtraUserControl2;

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

        private void newLoadBooks()
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
            if (selected != null && Convert.ToInt32(spinEdit1.EditValue) != 0)
            {
                var existing = saleItems.FirstOrDefault(x => x.BookID == bookId);

                if (existing != null)
                {

                    existing.Quantity += Convert.ToInt32(spinEdit1.EditValue);
                    if (existing.Quantity >= selected.StockQuantity)
                        existing.Quantity = Convert.ToInt32(selected.StockQuantity);

                    gridView1.RefreshData();
                }
                else
                    saleItems.Add(new SaleItem
                    {
                        BookID = selected.BookID,
                        ISBN = selected.ISBN,
                        Title = selected.Title,
                        Quantity = Convert.ToInt32(spinEdit1.EditValue),
                        UnitPrice = selected.RetailPrice,
                        VATPercent = selected.VatOutPercent

                    });
                countTotal();
            }
        }

        private void XtraForm4_Load(object sender, EventArgs e)
        {
            newLoadBooks();
            infoLoad();
            gridView2.Columns["BookID"].Visible = false;
            gridView2.Columns["ISBN"].Visible = false;
            gridView2.Columns["VATPercent"].Visible = false;

        }

        private void infoLoad()
        {

            textEdit5.Text = UserSession.CurrentUser?.FullName;
        }
        private void countTotal()
        {

            var subtotal = saleItems.Sum(x => x.LineTotal);
            textEdit2.Text = subtotal.ToString();
            textEdit1.Text = (saleItems.Sum(x => x.Quantity)).ToString();
            double.TryParse(textEdit2.Text, out double num1);
            double.TryParse(textEdit3.Text, out double num2);
            double total =num1 * (1 + num2 / 100);
            textEdit4.Text = total.ToString();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            _timer.Tick += onTimerTick;
            _timer.Start();
        }

        private void onTimerTick(object sender, EventArgs e)
        {
            var now = DateTime.Now;
            Text = $"Main Form - {now.ToShortDateString()} {now.ToLongTimeString()}";
        }

        System.Windows.Forms.Timer _timer = new System.Windows.Forms.Timer { Interval = 1000 };

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            int bookID = Convert.ToInt32(gridView2.GetFocusedRowCellValue("BookID"));
            DialogResult result = XtraMessageBox.Show(
        "Remove this item?",
        "Confirm",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                using (testBookEntities db = new testBookEntities())
                {
                    var item = saleItems.FirstOrDefault(x => x.BookID == bookID);

                    if (item != null)
                    {
                        saleItems.Remove(item);
                        gridControl2.Refresh();
                    }
                }
                countTotal();
            }

        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            using (XtraForm5 form5 = new XtraForm5())
            {
                if (form5.ShowDialog() == DialogResult.OK)
                {
                    string phone = form5.phoneNum;
                    using (testBookEntities db = new testBookEntities())
                    {
                        CUSTOMER customer = db.CUSTOMERs.FirstOrDefault(x => x.PhoneNumber == phone);
                        if (customer != null)
                        {
                            textEdit3.Text = (customer.CUSTOMER_TYPE.DiscountPercent).ToString();
                        }
                        countTotal();
                    }
                }

            }
        }

    }
}