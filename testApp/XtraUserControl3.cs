using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
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
    public partial class XtraUserControl3 : DevExpress.XtraEditors.XtraUserControl
    {
        public class ImportItem
        {
            public int BookID { get; set; }

            public string ISBN { get; set; }

            public string Title { get; set; }

            public int Quantity { get; set; }

            public decimal CostPrice { get; set; }

            public decimal VATPercent { get; set; }

            public decimal VATPrice
            {
                get
                {
                    return VATPercent / 100 * CostPrice;
                }
            }


            public decimal LineTotal
            {
                get
                {
                    return Quantity * CostPrice;
                }
            }
        }

        private int totalAm;
        private decimal totalMo;
        private int CurrentImportID;
        private IMPORT_RECEIPT receipt = new IMPORT_RECEIPT();
        private List<BOOK> _books;
        private List<IMPORT_RECEIPT> _imports;
        private BindingList<ImportItem> importItems = new BindingList<ImportItem>();
        public XtraUserControl3()
        {
            InitializeComponent();

            //BindingList<Customer> dataSource = GetDataSource();
            //gridControl.DataSource = dataSource;
            //bsiRecordsCount.Caption = "RECORDS : " + dataSource.Count;
            gridControl.DataSource = importItems;
        }

        private void Import_Load(object sender, EventArgs e)
        {
            
            ReLoad();
            //LoadCategories2();
            //LoadAuthor();
        }

        private void onLoad()
        {
            using (testBookEntities db =
                   new testBookEntities())
            {
                _books = db.BOOKs
            .Include("IMPORT_RECEIPT_DETAIL")
            .ToList();
            }
            gridControl.DataSource = importItems;

        }

        private void ReLoad()
        {
            onLoad();
            loadImport();
            bbiDelete.Enabled = false;
        }


        private void onNew()
        {
            using (testBookEntities db =
                   new testBookEntities())
            {
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

        private void loadImport()
        {

            using (testBookEntities db =
                   new testBookEntities())
            {
                _imports = db.IMPORT_RECEIPT
                    .Include("IMPORT_RECEIPT_DETAIL.BOOK")
                    .ToList();
                gridControl1.DataSource = _imports
                    .Where(x => x != null)
                    .Select(x => new
                    {
                        x.ImportID,
                        x.ImportCode,
                        x.TotalAmount,
                        x.CreatedAt,
                        x.ImportDate,
                        
                        x.Status,
                        x.Note
                    }).ToList();
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
                spinEdit1.Properties.MaxValue = 99;
                spinEdit1.EditValue = spinEdit1.Properties.MinValue;
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
            if (Convert.ToDecimal(e.NewValue) < 0)
            {
                e.Cancel = true;
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
            if (selected != null && Convert.ToInt32(spinEdit1.EditValue) != 0)
            {
                var existing = importItems.FirstOrDefault(x => x.BookID == bookId);

                if (existing != null)
                {

                    existing.Quantity += Convert.ToInt32(spinEdit1.EditValue);
                    if (existing.Quantity >= selected.StockQuantity)
                        existing.Quantity = Convert.ToInt32(selected.StockQuantity);

                    gridView.RefreshData();
                }
                else
                    importItems.Add(new ImportItem
                    {
                        BookID = selected.BookID,
                        ISBN = selected.ISBN,
                        Title = selected.Title,
                        Quantity = Convert.ToInt32(spinEdit1.EditValue),
                        CostPrice = selected.RetailPrice,
                        VATPercent = selected.VatInPercent

                    });
                loadTotal();
            }
            barButtonItem1.Enabled = true;



        }

        private void loadTotal()
        {
            totalAm = 0;
            totalMo = 0;
            foreach (var item in importItems)
            {
                totalAm += item.Quantity;
                totalMo += item.LineTotal;
            }
            textEdit6.Text = totalAm.ToString();
            textEdit7.Text = totalMo.ToString();

        }


        private void bbiPrintPreview_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (importItems.Count == 0)
            {
                XtraMessageBox.Show(
                    "Please add at least one book before saving.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }
            using (testBookEntities db = new testBookEntities())
            {
                IMPORT_RECEIPT receipt = new IMPORT_RECEIPT();

                receipt.ImportDate = DateTime.Now;
                //receipt.Importype = comboBoxEdit1.Text;
                //receipt.Reason = textEdit3.Text;
                receipt.Note = textEdit5.Text;
                receipt.ImportCode = textEdit1.Text;

                receipt.TotalQuantity =
                    importItems.Sum(x => x.Quantity);

                receipt.TotalAmount =
                    importItems.Sum(x => x.LineTotal);

                db.IMPORT_RECEIPT.Add(receipt);

                foreach (var item in importItems)
                {
                    BOOK book =
                        db.BOOKs.Find(item.BookID);

                    if (book == null)
                        continue;


                    IMPORT_RECEIPT_DETAIL detail =
                        new IMPORT_RECEIPT_DETAIL();

                    detail.BookID = item.BookID;
                    detail.Quantity = item.Quantity;
                    detail.Costprice = item.CostPrice;
                    //detail.VATPercent = item.VATPercent;
                    detail.LineTotal = item.LineTotal;

                    receipt.IMPORT_RECEIPT_DETAIL.Add(detail);

                    book.StockQuantity -= item.Quantity;
                }
                db.SaveChanges();

                XtraMessageBox.Show("Saved successfully");
                importItems.Clear();
            }
        }

        private void SaveDraft()
        {
            if (importItems.Count == 0)
            {
                XtraMessageBox.Show(
                    "Please add at least one book before saving.",
                    "Validation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }
            using (var db = new testBookEntities())
            {
                var receipt =
                    db.IMPORT_RECEIPT.Find(CurrentImportID);

                receipt.CreatedAt = DateTime.Now;
                //receipt.Importype = comboBoxEdit1.Text;
                //receipt.Reason = textEdit3.Text;
                receipt.Note = textEdit5.Text;
                receipt.ImportCode = textEdit1.Text;

                receipt.TotalQuantity =
                    importItems.Sum(x => x.Quantity);

                receipt.TotalAmount =
                    importItems.Sum(x => x.LineTotal);
                // Delete old details
                db.IMPORT_RECEIPT_DETAIL.RemoveRange(
                    receipt.IMPORT_RECEIPT_DETAIL);

                // Recreate details
                foreach (var item in importItems)
                {
                    receipt.IMPORT_RECEIPT_DETAIL.Add(
                        new IMPORT_RECEIPT_DETAIL
                        {
                            BookID = item.BookID,
                            Quantity = item.Quantity,
                            Costprice = item.CostPrice,
                            //VATPercent = item.VATPercent
                        });
                }

                db.SaveChanges();
                XtraMessageBox.Show("Draft Saved successfully");
                barButtonItem1.Enabled = false;
                barButtonItem2.Enabled = true;
            }
        }

        private void ApproveReceipt()
        {
            using (var db = new testBookEntities())
            {
                var receipt =
                    db.IMPORT_RECEIPT.Find(CurrentImportID);

                if (receipt.Status != "Draft")
                    return;


                foreach (var detail in
                    receipt.IMPORT_RECEIPT_DETAIL)
                {
                    var book =
                        db.BOOKs.Find(detail.BookID);

                    book.StockQuantity += detail.Quantity;
                }

                receipt.Status = "Approved";
                receipt.ImportDate = DateTime.Now;

                db.SaveChanges();
                barButtonItem2.Enabled = true;
            }
        }

        private void newImport()
        {
            using (testBookEntities db = new testBookEntities())
            {
                IMPORT_RECEIPT receipt = new IMPORT_RECEIPT();

                receipt.ImportDate = DateTime.Now;
                receipt.Status = "Draft";

                db.IMPORT_RECEIPT.Add(receipt);
                db.SaveChanges();

                CurrentImportID = receipt.ImportID;
                textEdit1.Text = "PN000" + CurrentImportID;
                textEdit2.Text = receipt.ImportDate.ToString();
                onNew();
            }
        }

        private void bbiNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            newImport();
            panelControl1.Enabled = true;
        }

        private void gridView1_DoubleClick(object sender, EventArgs e)
        {
            DXMouseEventArgs ea = e as DXMouseEventArgs;
            GridView view = sender as GridView;
            GridHitInfo info = view.CalcHitInfo(ea.Location);
            if (info.InRow || info.InRowCell)
            {
                int rowHandle = info.RowHandle;
                object rowData = view.GetRow(rowHandle);
                object value = gridView1.GetRowCellValue(rowHandle, "ImportID");
                int ImportId = Convert.ToInt32(value);

                IMPORT_RECEIPT selected = _imports.FirstOrDefault(x => x.ImportID == ImportId);
                if (selected != null)
                {
                    CurrentImportID = selected.ImportID;
                    textEdit1.Text = selected.ImportID.ToString();
                    textEdit2.Text = selected.ImportDate.ToString();
                    textEdit6.Text = selected.TotalQuantity.ToString();
                    //textEdit8.Text = selected.TotalVATAmount.ToString();
                    textEdit7.Text = selected.TotalAmount.ToString();
                    //textEdit3.Text = selected.Reason.ToString();
                    textEdit5.Text = selected.Note.ToString();
                    importItems = new BindingList<ImportItem>(
                    selected.IMPORT_RECEIPT_DETAIL
                        .Select(d => new ImportItem
                        {
                            BookID = Convert.ToInt32(d.BookID),
                            ISBN = d.BOOK.ISBN,
                            Title = d.BOOK.Title,
                            Quantity = Convert.ToInt32(d.Quantity),
                            CostPrice = Convert.ToInt32(d.Costprice),
                            //VATPercent = Convert.ToInt32(d.VATPercent)
                        })
                        .ToList());

                    gridControl.DataSource = importItems;
                    panelControl1.Enabled = false;
                    bbiDelete.Enabled = false;
                    closeBtn.Visibility = BarItemVisibility.Always;
                }
            }
            gridControl1.Visible = false;

        }

        private void closeBtn_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridControl1.Visible = true;
            closeBtn.Visibility = BarItemVisibility.Never;
            ReLoad();
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            SaveDraft();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            ApproveReceipt();
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
                    var item = importItems.FirstOrDefault( i => i.BookID ==  bookId );

                    if (item != null)
                    {
                        importItems.Remove(item);
                    }
                }
                loadTotal();

                gridView.RefreshData();
            }
        }

        private void bbiEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            bbiDelete.Enabled = true;
            panelControl1.Enabled = true;
        }
    }
}
