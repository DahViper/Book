using DevExpress.DocumentServices.ServiceModel.DataContracts;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Customization;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraLayout.Customization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

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

        private int totalAm;
        private decimal totalMo;
        private int CurrentExportID;
        private EXPORT_RECEIPT receipt = new EXPORT_RECEIPT();
        private List<BOOK> _books;
        private List<EXPORT_RECEIPT> _exports;
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
            ApplyPermission();
            loadExport();
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
            .Include("EXPORT_RECEIPT_DETAIL")
            .ToList();
            }
            gridControl.DataSource = exportItems;
            panelControl1.Enabled = false;
            
        }

        private void ReLoad()
        {
            onLoad();
        }


        private void loadBooks()
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

        private void loadExport()
        {
            
            using (testBookEntities db =
                   new testBookEntities())
            {
                _exports = db.EXPORT_RECEIPT
                    .Include("EXPORT_RECEIPT_DETAIL.BOOK")
                    .ToList();
                gridControl1.DataSource = _exports
                    .Where(x => x != null)
                    .Select(x => new
                    {
                        x.ExportID,
                        x.ExportCode,
                        x.TotalAmount,
                        x.Status,
                        x.ExportDate,
                        x.CreatedAt
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
                spinEdit1.Properties.MaxValue = Convert.ToInt32(selected.StockQuantity);
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
                var existing = exportItems.FirstOrDefault(x => x.BookID == bookId);

                if (existing != null)
                {
                    
                    existing.Quantity += Convert.ToInt32(spinEdit1.EditValue);
                    if (existing.Quantity >= selected.StockQuantity) 
                        existing.Quantity = Convert.ToInt32(selected.StockQuantity);

                    gridView.RefreshData();
                }
                else
                    exportItems.Add(new ExportItem
                    {
                        BookID = selected.BookID,
                        ISBN = selected.ISBN,
                        Title = selected.Title,
                        Quantity = Convert.ToInt32(spinEdit1.EditValue),
                        UnitPrice = selected.RetailPrice,
                        VATPercent = selected.VatOutPercent

                    });
                loadTotal();
                }
            barButtonItem1.Enabled = true;



        }

        private void loadTotal()
        {
            totalAm = 0;
            totalMo = 0;
            foreach (var item in exportItems)
            {
                totalAm += item.Quantity;
                totalMo += item.LineTotal;
            }
            textEdit6.Text = totalAm.ToString();
            textEdit7.Text = totalMo.ToString();

        }

        private void bbiRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            SaveDraft();
        }

        private void bbiPrintPreview_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (exportItems.Count == 0)
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
                EXPORT_RECEIPT receipt = new EXPORT_RECEIPT();

                receipt.ExportDate = DateTime.Now;
                receipt.ExporType = comboBoxEdit1.Text;
                receipt.Reason = textEdit3.Text;
                receipt.Note = textEdit5.Text;
                receipt.ExportCode = textEdit1.Text;

                receipt.TotalQuantity =
                    exportItems.Sum(x => x.Quantity);

                receipt.TotalAmount =
                    exportItems.Sum(x => x.LineTotal);

                db.EXPORT_RECEIPT.Add(receipt);

                foreach (var item in exportItems)
                {
                    BOOK book =
                        db.BOOKs.Find(item.BookID);

                    if (book == null)
                        continue;

                    
                    EXPORT_RECEIPT_DETAIL detail =
                        new EXPORT_RECEIPT_DETAIL();

                    detail.BookID = item.BookID;
                    detail.Quantity = item.Quantity;
                    detail.UnitPrice = item.UnitPrice;
                    detail.VATPercent = item.VATPercent;
                    detail.LineTotal = item.LineTotal;

                    receipt.EXPORT_RECEIPT_DETAIL.Add(detail);

                    book.StockQuantity -= item.Quantity;
                }
                db.SaveChanges();

                XtraMessageBox.Show("Saved successfully");
                exportItems.Clear();
            }
        }

        private void SaveDraft()
        {
            if (exportItems.Count == 0)
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
                    db.EXPORT_RECEIPT.Find(CurrentExportID);

                receipt.CreatedAt = DateTime.Now;
                receipt.ExporType = comboBoxEdit1.Text;
                receipt.Reason = textEdit3.Text;
                receipt.Note = textEdit5.Text;
                receipt.ExportCode = textEdit1.Text;

                receipt.TotalQuantity =
                    exportItems.Sum(x => x.Quantity);

                receipt.TotalAmount =
                    exportItems.Sum(x => x.LineTotal);
                // Delete old details
                db.EXPORT_RECEIPT_DETAIL.RemoveRange(
                    receipt.EXPORT_RECEIPT_DETAIL);

                // Recreate details
                foreach (var item in exportItems)
                {
                    receipt.EXPORT_RECEIPT_DETAIL.Add(
                        new EXPORT_RECEIPT_DETAIL
                        {
                            BookID = item.BookID,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice,
                            VATPercent = item.VATPercent
                        });
                }

                db.SaveChanges();
                XtraMessageBox.Show("Draft Saved successfully");
                barButtonItem1.Enabled = false;
            }
        }

        private void ApproveReceipt()
        {
            using (var db = new testBookEntities())
            {
                var receipt =
                    db.EXPORT_RECEIPT.Find(CurrentExportID);

                if (receipt.Status != "Draft")
                    return;

                foreach (var detail in
                    receipt.EXPORT_RECEIPT_DETAIL)
                {
                    var book =
                        db.BOOKs.Find(detail.BookID);

                    if (book.StockQuantity < detail.Quantity)
                    {
                        XtraMessageBox.Show(
                            "Not enough stock.");
                        return;
                    }
                }

                foreach (var detail in
                    receipt.EXPORT_RECEIPT_DETAIL)
                {
                    var book =
                        db.BOOKs.Find(detail.BookID);

                    book.StockQuantity -= detail.Quantity;
                }

                receipt.ExportDate =    DateTime.Now;
                receipt.Status = "Approved";

                db.SaveChanges();
            }
        }

        private void newExport()
        {
            using (testBookEntities db = new testBookEntities())
            {
                EXPORT_RECEIPT receipt = new EXPORT_RECEIPT();

                receipt.ExportDate = DateTime.Now;
                receipt.Status = "Draft";

                db.EXPORT_RECEIPT.Add(receipt);
                db.SaveChanges();

                CurrentExportID = receipt.ExportID;
                textEdit1.Text = "PX000" + CurrentExportID;
                textEdit2.Text = receipt.ExportDate.ToString();                
            }
            loadBooks();
            panelControl1.Enabled = true;
        }

        private void bbiNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            newExport();
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
                object value = gridView1.GetRowCellValue(rowHandle, "ExportID");
                int exportId = Convert.ToInt32(value);

                EXPORT_RECEIPT selected = _exports.FirstOrDefault(x => x.ExportID == exportId);
                if (selected != null)
                {
                    CurrentExportID = selected.ExportID;
                    textEdit1.Text = selected.ExportID.ToString();
                    textEdit2.Text= selected.ExportDate.ToString();
                    textEdit6.Text = selected.TotalQuantity.ToString();
                    textEdit8.Text = selected.TotalVATAmount.ToString();
                    textEdit7.Text = selected.TotalAmount.ToString();
                    textEdit3.Text = selected.Reason.ToString();
                    textEdit5.Text = selected.Note.ToString();
                    exportItems = new BindingList<ExportItem>(
                    selected.EXPORT_RECEIPT_DETAIL
                        .Select(d => new ExportItem
                        {
                            BookID = Convert.ToInt32(d.BookID),
                            ISBN = d.BOOK.ISBN,
                            Title = d.BOOK.Title,
                            Quantity = Convert.ToInt32(d.Quantity),
                            UnitPrice = Convert.ToInt32(d.UnitPrice),
                            VATPercent = Convert.ToInt32(d.VATPercent)
                        })
                        .ToList());

                    gridControl.DataSource = exportItems;
                }
            }
            gridControl1.Visible = false;
            closeBtn.Enabled = true;
            
        }

        private void closeBtn_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridControl1.Visible = true;
            closeBtn.Enabled = false;
        }

        private void bbiEdit_ItemClick(object sender, ItemClickEventArgs e)
        {
            loadBooks();
            panelControl1.Enabled = true;
        }

        private void ApplyPermission()
        {
            bbiNew.Enabled = PermissionService.Has("EXPORT_ADD");
            bbiEdit.Enabled = PermissionService.Has("EXPORT_EDIT");
            bbiDelete.Enabled = PermissionService.Has("EXPORT_EDIT");
            bbiSave.Enabled = PermissionService.Has("EXPORT_EDIT");
            barButtonItem1.Enabled = PermissionService.Has("EXPORT_APPROVE");
            
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            ApproveReceipt();
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void gridControl1_FocusedViewChanged(object sender, ViewFocusEventArgs e)
        {
            object value = gridView1.GetFocusedRowCellValue("Status");
            if (value != null) return;
            string status = value.ToString();
            if (status == "Draft")
            {
                if (PermissionService.Has("EXPORT_APPROVE"))
                {
                    barButtonItem1.Enabled = true;
                }
                else barButtonItem1.Enabled = false;
            }
        }
    }
    
}
