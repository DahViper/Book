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
            int lastIndex = saleItems.Count -1;
            saleItems.RemoveAt(lastIndex);
            gridView2.RefreshData();
            countTotal();
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            using (Sale_Phone form5 = new Sale_Phone())
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

        private void XtraForm4_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F1:
                    simpleButton1_Click(this, new EventArgs());
                    break;

                case Keys.F2:
                    simpleButton3_Click(this, new EventArgs());
                    break;

                case Keys.F3:
                    break;

                case Keys.F4:
                    break;

                case Keys.F5:
                    panelControl4.Visible = true;
                    textEdit6.Focus();
                    //simpleButton5_Click(this, new EventArgs());
                    break;

                case Keys.F6:
                    break;


            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            using (Sale_Product form5 = new Sale_Product())
            {
                if (form5.ShowDialog() == DialogResult.OK)
                {
                    string product = form5.productCode;
                    string amount = form5.quantity;

                    using (testBookEntities db = new testBookEntities())
                    {
                        BOOK selected = db.BOOKs.FirstOrDefault(x => x.BookID.ToString() == product);
                        if (selected != null)
                        {
                            saleItems.Add(new SaleItem
                            {
                                BookID = selected.BookID,
                                ISBN = selected.ISBN,
                                Title = selected.Title,
                                Quantity = Convert.ToInt32(amount),
                                UnitPrice = selected.RetailPrice,
                                VATPercent = selected.VatOutPercent

                            });
                        }
                        countTotal();
                    }
                }

            }
        }

        private void CompleteSale()
        {
            if (saleItems.Count == 0)
            {
                XtraMessageBox.Show(
                    "Please add at least one book.");

                return;
            }

            using (var db = new testBookEntities())
            using (var transaction =
                   db.Database.BeginTransaction())
            {
                try
                {
                    SALE sale = new SALE();

                    //sale.SaleCode = GenerateSaleCode();

                    //sale.CustomerID = selectedCustomerID;

                    sale.UserID =
                        UserSession.CurrentUser.UserID;

                    sale.SaleDate = DateTime.Now;

                    //sale.PaymentMethod = cboPaymentMethod.Text;

                    sale.Subtotal =
                        saleItems.Sum(x => x.LineTotal);

                    //sale.TotalAmount = sale.Subtotal - discountAmount + vatAmount;

                    sale.Status = "Completed";

                    db.SALEs.Add(sale);

                    foreach (var item in saleItems)
                    {
                        BOOK book =
                            db.BOOKs.Find(item.BookID);

                        if (book == null)
                        {
                            throw new Exception(
                                "Book not found.");
                        }

                        if (book.StockQuantity < item.Quantity)
                        {
                            throw new Exception(
                                $"Not enough stock for {book.Title}.");
                        }

                        SALE_DETAIL detail =
                            new SALE_DETAIL();

                        detail.BookID = item.BookID;
                        detail.Quantity = item.Quantity;
                        detail.UnitPrice = item.UnitPrice;
                        detail.VATPercent =
                            item.VATPercent;

                        detail.LineTotal =
                            item.LineTotal;

                        sale.SALE_DETAIL.Add(detail);

                        book.StockQuantity -= item.Quantity;
                    }

                    db.SaveChanges();

                    transaction.Commit();

                    XtraMessageBox.Show(
                        "Sale completed successfully.");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();

                    XtraMessageBox.Show(
                        ex.Message);
                }
            }
        }


        private void simpleButton17_Click(object sender, EventArgs e)
        {
            string phone = textEdit6.Text;
            using (testBookEntities db = new testBookEntities())
            {
                CUSTOMER customer = db.CUSTOMERs.FirstOrDefault(x => x.PhoneNumber == phone);
                if (customer != null)
                {
                    textEdit3.Text = (customer.CUSTOMER_TYPE.DiscountPercent).ToString();
                }
                else
                {
                    XtraMessageBox.Show(
                    "Not existing customer.");

                    return;
                }
                    countTotal();
            }
        }

        private void textEdit6_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                simpleButton17_Click(this, new EventArgs());
            }

        }
    }
}