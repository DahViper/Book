namespace testApp
{
    partial class XtraForm1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.textISBN = new DevExpress.XtraEditors.TextEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.textTitle = new DevExpress.XtraEditors.TextEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.textPublisher = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl5 = new DevExpress.XtraEditors.LabelControl();
            this.lkAuthor1 = new DevExpress.XtraEditors.LookUpEdit();
            this.lkCategory1 = new DevExpress.XtraEditors.LookUpEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.textISBN.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textTitle.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textPublisher.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkAuthor1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkCategory1.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // textISBN
            // 
            this.textISBN.Location = new System.Drawing.Point(177, 93);
            this.textISBN.Name = "textISBN";
            this.textISBN.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(220)))), ((int)(((byte)(213)))));
            this.textISBN.Properties.Appearance.Font = new System.Drawing.Font("Montserrat ExtraBold", 7.8F, System.Drawing.FontStyle.Bold);
            this.textISBN.Properties.Appearance.Options.UseBackColor = true;
            this.textISBN.Properties.Appearance.Options.UseFont = true;
            this.textISBN.Size = new System.Drawing.Size(115, 26);
            this.textISBN.TabIndex = 0;
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Montserrat ExtraBold", 7.8F, System.Drawing.FontStyle.Bold);
            this.labelControl1.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Appearance.Options.UseForeColor = true;
            this.labelControl1.Location = new System.Drawing.Point(96, 96);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(32, 20);
            this.labelControl1.TabIndex = 1;
            this.labelControl1.Text = "ISBN";
            // 
            // textTitle
            // 
            this.textTitle.Location = new System.Drawing.Point(177, 121);
            this.textTitle.Name = "textTitle";
            this.textTitle.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(220)))), ((int)(((byte)(213)))));
            this.textTitle.Properties.Appearance.Font = new System.Drawing.Font("Montserrat ExtraBold", 7.8F, System.Drawing.FontStyle.Bold);
            this.textTitle.Properties.Appearance.Options.UseBackColor = true;
            this.textTitle.Properties.Appearance.Options.UseFont = true;
            this.textTitle.Size = new System.Drawing.Size(115, 26);
            this.textTitle.TabIndex = 0;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Montserrat ExtraBold", 7.8F, System.Drawing.FontStyle.Bold);
            this.labelControl2.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Appearance.Options.UseForeColor = true;
            this.labelControl2.Location = new System.Drawing.Point(96, 124);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(30, 20);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "Title";
            // 
            // textPublisher
            // 
            this.textPublisher.Location = new System.Drawing.Point(177, 149);
            this.textPublisher.Name = "textPublisher";
            this.textPublisher.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(220)))), ((int)(((byte)(213)))));
            this.textPublisher.Properties.Appearance.Font = new System.Drawing.Font("Montserrat ExtraBold", 7.8F, System.Drawing.FontStyle.Bold);
            this.textPublisher.Properties.Appearance.Options.UseBackColor = true;
            this.textPublisher.Properties.Appearance.Options.UseFont = true;
            this.textPublisher.Size = new System.Drawing.Size(115, 26);
            this.textPublisher.TabIndex = 0;
            // 
            // labelControl3
            // 
            this.labelControl3.Appearance.Font = new System.Drawing.Font("Montserrat ExtraBold", 7.8F, System.Drawing.FontStyle.Bold);
            this.labelControl3.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelControl3.Appearance.Options.UseFont = true;
            this.labelControl3.Appearance.Options.UseForeColor = true;
            this.labelControl3.Location = new System.Drawing.Point(96, 152);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(66, 20);
            this.labelControl3.TabIndex = 1;
            this.labelControl3.Text = "Publisher";
            // 
            // labelControl4
            // 
            this.labelControl4.Appearance.Font = new System.Drawing.Font("Montserrat ExtraBold", 7.8F, System.Drawing.FontStyle.Bold);
            this.labelControl4.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelControl4.Appearance.Options.UseFont = true;
            this.labelControl4.Appearance.Options.UseForeColor = true;
            this.labelControl4.Location = new System.Drawing.Point(96, 180);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(49, 20);
            this.labelControl4.TabIndex = 1;
            this.labelControl4.Text = "Author";
            // 
            // labelControl5
            // 
            this.labelControl5.Appearance.Font = new System.Drawing.Font("Montserrat ExtraBold", 7.8F, System.Drawing.FontStyle.Bold);
            this.labelControl5.Appearance.ForeColor = System.Drawing.Color.White;
            this.labelControl5.Appearance.Options.UseFont = true;
            this.labelControl5.Appearance.Options.UseForeColor = true;
            this.labelControl5.Location = new System.Drawing.Point(96, 208);
            this.labelControl5.Name = "labelControl5";
            this.labelControl5.Size = new System.Drawing.Size(64, 20);
            this.labelControl5.TabIndex = 1;
            this.labelControl5.Text = "Category";
            // 
            // lkAuthor1
            // 
            this.lkAuthor1.Location = new System.Drawing.Point(177, 177);
            this.lkAuthor1.Name = "lkAuthor1";
            this.lkAuthor1.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(220)))), ((int)(((byte)(213)))));
            this.lkAuthor1.Properties.Appearance.Font = new System.Drawing.Font("Montserrat ExtraBold", 7.8F, System.Drawing.FontStyle.Bold);
            this.lkAuthor1.Properties.Appearance.Options.UseBackColor = true;
            this.lkAuthor1.Properties.Appearance.Options.UseFont = true;
            this.lkAuthor1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkAuthor1.Properties.NullText = "";
            this.lkAuthor1.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            this.lkAuthor1.Size = new System.Drawing.Size(165, 26);
            this.lkAuthor1.TabIndex = 5;
            this.lkAuthor1.ProcessNewValue += new DevExpress.XtraEditors.Controls.ProcessNewValueEventHandler(this.lookUpEdit1_ProcessNewValue);
            // 
            // lkCategory1
            // 
            this.lkCategory1.Location = new System.Drawing.Point(177, 205);
            this.lkCategory1.Name = "lkCategory1";
            this.lkCategory1.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(220)))), ((int)(((byte)(213)))));
            this.lkCategory1.Properties.Appearance.Font = new System.Drawing.Font("Montserrat ExtraBold", 7.8F, System.Drawing.FontStyle.Bold);
            this.lkCategory1.Properties.Appearance.Options.UseBackColor = true;
            this.lkCategory1.Properties.Appearance.Options.UseFont = true;
            this.lkCategory1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.lkCategory1.Properties.NullText = "";
            this.lkCategory1.Size = new System.Drawing.Size(165, 26);
            this.lkCategory1.TabIndex = 6;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Appearance.Font = new System.Drawing.Font("Montserrat ExtraBold", 7.8F, System.Drawing.FontStyle.Bold);
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.Location = new System.Drawing.Point(177, 287);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(94, 29);
            this.simpleButton1.TabIndex = 7;
            this.simpleButton1.Text = "Confirm";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // XtraForm1
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(99)))), ((int)(((byte)(126)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(461, 382);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.lkCategory1);
            this.Controls.Add(this.lkAuthor1);
            this.Controls.Add(this.labelControl5);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.textPublisher);
            this.Controls.Add(this.textTitle);
            this.Controls.Add(this.textISBN);
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Shadow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "XtraForm1";
            this.Text = "Adding Book...";
            this.Load += new System.EventHandler(this.XtraForm1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.textISBN.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textTitle.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textPublisher.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkAuthor1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lkCategory1.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit textISBN;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.TextEdit textTitle;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit textPublisher;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private DevExpress.XtraEditors.LabelControl labelControl5;
        private DevExpress.XtraEditors.LookUpEdit lkAuthor1;
        private DevExpress.XtraEditors.LookUpEdit lkCategory1;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
    }
}