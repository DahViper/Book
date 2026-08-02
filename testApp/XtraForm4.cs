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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
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
                    return Quantity * UnitPrice*(1+ VATPercent/100);
                }
            }
        }

        private BindingList<SaleItem> saleItems = new BindingList<SaleItem>();
        private List<BOOK> _books;
        private Control lastInputControl;
        private bool inSale;
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
            //gridView2.Columns["VATPercent"].Visible = false;

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
            double total = num1 * (1 - num2 / 100);
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
            if (saleItems.Count > 0)
            {
                int lastIndex = saleItems.Count - 1;
                saleItems.RemoveAt(lastIndex);
                gridView2.RefreshData();
                countTotal();
            }
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            panelControl4.Visible = false;
            panelControl5.Visible = false;

            panelControl4.Visible = true;

            textEdit6.Focus();
            textEdit6.Clear();
        }

        private void XtraForm4_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (inSale == true) panelControl6.Visible = false;
            }
            //switch (e.KeyCode)
            //{
            //    case Keys.F1:
            //        panelControl5.Visible = true;
            //        textEdit8.Focus();
            //        //simpleButton1_Click(this, new EventArgs());
            //        break;

            //    case Keys.F2:
            //        simpleButton3_Click(this, new EventArgs());
            //        break;

            //    case Keys.F3:
            //        break;

            //    case Keys.F4:
            //        break;

            //    case Keys.F5:
            //        panelControl4.Visible = true;
            //        textEdit6.Focus();
            //        //simpleButton5_Click(this, new EventArgs());
            //        break;

            //    case Keys.F6:
            //        break;


            //}
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            panelControl4.Visible = false;
            panelControl5.Visible = false;

            panelControl5.Visible = true;
            textEdit7.Clear();
            textEdit8.Clear();
            textEdit7.Focus();
            
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
                    textEdit5.Text = customer.FullName;
                }
                else
                {
                    XtraMessageBox.Show(
                    "No existing customer.");

                    return;
                }
                countTotal();
                panelControl4.Visible = false;
            }
        }

        private void textEdit6_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                simpleButton17_Click(this, new EventArgs());
            }

        }

        private void simpleButton18_Click(object sender, EventArgs e)
        {
            string product = textEdit7.Text;
            string amount = textEdit8.Text;

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
                panelControl5.Visible = false;
            }
        }

        private void textEdit7_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    textEdit8.Focus();
                    break;
                case Keys.Right: 
                    textEdit8.Focus();
                    break;
            }
        }

        private void textEdit8_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    simpleButton18_Click(this, new EventArgs());
                    break;
                case Keys.Left:
                    textEdit7.Focus();
                    break;
            }
        }

        private void KeyButton_Click(object sender, EventArgs e)
        {
            if (lastInputControl == null)
                return;

            SimpleButton button = sender as SimpleButton;

            string number = button.Text;

            if (lastInputControl is TextEdit textEdit)
            {
                int cursorPosition = textEdit.SelectionStart;

                textEdit.Text =
                    textEdit.Text.Insert(cursorPosition, number);

                textEdit.SelectionStart =
                    cursorPosition + number.Length;
            }
        }

        private void clearButton_Click(object sender, EventArgs e)
        {
            if (lastInputControl is TextEdit textEdit)
            {
                textEdit.Clear();
            }
        }

        private void btnBackspace_Click(object sender, EventArgs e)
        {
            if (lastInputControl is TextEdit textEdit)
            {
                int position = textEdit.SelectionStart;

                if (position > 0)
                {
                    textEdit.Text =
                        textEdit.Text.Remove(position - 1, 1);

                    textEdit.SelectionStart = position - 1;
                }
            }
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.F1:
                    simpleButton1_Click(this, new EventArgs());
                    return true; // Return true to signal that you have handled the key

                case Keys.F2:
                    simpleButton3_Click(this, new EventArgs());
                    return true;

                case Keys.F3:
                    return true;

                case Keys.F4:
                    // Add code for F4 here
                    return true;

                case Keys.F5:
                    simpleButton5_Click(this, new EventArgs());
                    return true;

                case Keys.F7:
                    simpleButton35_Click(this, new EventArgs());
                    return true;

                case Keys.Escape:
                    if (inSale == true)
                    {
                        panelControl6.Visible = false;
                    }
                    break;
            }

            // Let the form process everything else normally
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void simpleButton35_Click(object sender, EventArgs e)
        {
            textEdit9.Text = textEdit4.Text;
            panelControl6.Visible = true;
            inSale = true;
            textEdit10.Clear();
            textEdit10.Focus();
        }

        private void textEdit10_EditValueChanged(object sender, EventArgs e)
        {
            decimal.TryParse(textEdit9.Text, out decimal result1);
            decimal.TryParse(textEdit10.Text, out decimal result2);
            decimal result = result1 - result2;
            textEdit12.Text = result.ToString();
        }

        private void simpleButton33_Click(object sender, EventArgs e)
        {
            CompleteSale();
            panelControl6.Visible = false;
        }

        private void simpleButton1_Click_1(object sender, EventArgs e)
        {
            panelControl6.Visible= false;
            inSale = false;
        }

        private void textEdit_Enter(object sender, EventArgs e)
        {
            lastInputControl = sender as Control;
        }
    }
}