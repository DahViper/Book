using DevExpress.XtraBars;
using DevExpress.XtraBars.Customization;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
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
    
    public partial class XtraUserControl2 : DevExpress.XtraEditors.XtraUserControl
    {
        public class ExportItem
        {
            public int BookID { get; set; }

            public string ISBN { get; set; }

            public string Title { get; set; }

            public int Quantity { get; set; }

            public decimal UnitPrice { get; set; }

            public decimal VATPercent { get; set; }

            public decimal VATPrice
            {
                get
                {
                    return VATPercent/100 * UnitPrice;
                }
            }


            public decimal LineTotal
            {
                get
                {
                    return Quantity * UnitPrice;
                }
            }
        }

        private List<BOOK> _books;
        private BindingList<ExportItem> exportItems = new BindingList<ExportItem>();
        public XtraUserControl2()
        {
            InitializeComponent();

            //BindingList<Customer> dataSource = GetDataSource();
            //gridControl.DataSource = dataSource;
            //bsiRecordsCount.Caption = "RECORDS : " + dataSource.Count;
            gridControl.DataSource = exportItems;
        }

        private void Export_Load(object sender, EventArgs e)
        {
            onLoad();
            onNew();
            //LoadCategories2();
            //LoadAuthor();
        }
        public BindingList<Customer> GetDataSource()
        {
            BindingList<Customer> result = new BindingList<Customer>();
            result.Add(new Customer()
            {
                ID = 1,
                Name = "ACME",
                Address = "2525 E El Segundo Blvd",
                City = "El Segundo",
                State = "CA",
                ZipCode = "90245",
                Phone = "(310) 536-0611"
            });
            result.Add(new Customer()
            {
                ID = 2,
                Name = "Electronics Depot",
                Address = "2455 Paces Ferry Road NW",
                City = "Atlanta",
                State = "GA",
                ZipCode = "30339",
                Phone = "(800) 595-3232"
            });
            return result;
        }
        public class Customer
        {
            [Key, Display(AutoGenerateField = false)]
            public int ID { get; set; }
            [Required]
            public string Name { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            [Display(Name = "Zip Code")]
            public string ZipCode { get; set; }
            public string Phone { get; set; }
        }

        private void onLoad()
        {
            using (testBookEntities db =
                   new testBookEntities())
            {
                _books = db.BOOKs
            .Include("EXPORT_RECEIPT_DETAIL")
            .ToList();
            }
        }

        private void onNew()
        {
            using (testBookEntities db =
                   new testBookEntities())
            {
                var books = db.BOOKs.Select(b=> new
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
            }
        }

        private void spinEdit1_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
        {
            DevExpress.XtraEditors.SpinEdit editor = sender as DevExpress.XtraEditors.SpinEdit;
            if (Convert.ToDecimal(e.NewValue) > editor.Properties.MaxValue)
            {
                //e.Cancel = true;
                this.BeginInvoke(new MethodInvoker(MyMethod));
            }
        }
        public void MyMethod()
        {
            spinEdit1.EditValue = spinEdit1.Properties.MaxValue;
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            object value = searchLookUpEdit1.Properties.View.GetFocusedRowCellValue("BookID");

            if (value == null) return;

            int bookId = Convert.ToInt32(value);
            BOOK selected = _books.FirstOrDefault(b => b.BookID == bookId);
            if (selected != null)
            {
                exportItems.Add(new ExportItem
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
