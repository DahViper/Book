namespace testApp
{
    partial class LoginTemp
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
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.textEdit1 = new DevExpress.XtraEditors.TextEdit();
            this.textEdit2 = new DevExpress.XtraEditors.TextEdit();
            this.simpleButton2 = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit2.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // simpleButton1
            // 
            this.simpleButton1.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(220)))), ((int)(((byte)(213)))));
            this.simpleButton1.Appearance.Font = new System.Drawing.Font("Montserrat ExtraBold", 7.8F, System.Drawing.FontStyle.Bold);
            this.simpleButton1.Appearance.Options.UseBackColor = true;
            this.simpleButton1.Appearance.Options.UseFont = true;
            this.simpleButton1.Location = new System.Drawing.Point(126, 215);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(84, 26);
            this.simpleButton1.TabIndex = 0;
            this.simpleButton1.Text = "Confirm";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // textEdit1
            // 
            this.textEdit1.Location = new System.Drawing.Point(126, 101);
            this.textEdit1.Name = "textEdit1";
            this.textEdit1.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(99)))), ((int)(((byte)(126)))));
            this.textEdit1.Properties.Appearance.Font = new System.Drawing.Font("Montserrat ExtraBold", 7.8F);
            this.textEdit1.Properties.Appearance.Options.UseBackColor = true;
            this.textEdit1.Properties.Appearance.Options.UseFont = true;
            this.textEdit1.Size = new System.Drawing.Size(174, 26);
            this.textEdit1.TabIndex = 1;
            this.textEdit1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textEdit1_KeyUp);
            // 
            // textEdit2
            // 
            this.textEdit2.Location = new System.Drawing.Point(126, 149);
            this.textEdit2.Name = "textEdit2";
            this.textEdit2.Properties.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(67)))), ((int)(((byte)(99)))), ((int)(((byte)(126)))));
            this.textEdit2.Properties.Appearance.Font = new System.Drawing.Font("Montserrat ExtraBold", 7.8F);
            this.textEdit2.Properties.Appearance.Options.UseBackColor = true;
            this.textEdit2.Properties.Appearance.Options.UseFont = true;
            this.textEdit2.Size = new System.Drawing.Size(174, 26);
            this.textEdit2.TabIndex = 2;
            this.textEdit2.KeyUp += new System.Windows.Forms.KeyEventHandler(this.textEdit2_KeyUp);
            // 
            // simpleButton2
            // 
            this.simpleButton2.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(101)))), ((int)(((byte)(220)))), ((int)(((byte)(213)))));
            this.simpleButton2.Appearance.Font = new System.Drawing.Font("Montserrat ExtraBold", 7.8F, System.Drawing.FontStyle.Bold);
            this.simpleButton2.Appearance.Options.UseBackColor = true;
            this.simpleButton2.Appearance.Options.UseFont = true;
            this.simpleButton2.Location = new System.Drawing.Point(216, 215);
            this.simpleButton2.Name = "simpleButton2";
            this.simpleButton2.Size = new System.Drawing.Size(84, 26);
            this.simpleButton2.TabIndex = 0;
            this.simpleButton2.Text = "Temp";
            this.simpleButton2.Click += new System.EventHandler(this.simpleButton2_Click);
            // 
            // LoginTemp
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(30)))), ((int)(((byte)(72)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 329);
            this.Controls.Add(this.textEdit2);
            this.Controls.Add(this.textEdit1);
            this.Controls.Add(this.simpleButton2);
            this.Controls.Add(this.simpleButton1);
            this.Name = "LoginTemp";
            this.Text = "Login";
            this.Load += new System.EventHandler(this.LoginTemp_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.LoginTemp_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.textEdit1.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.textEdit2.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraEditors.TextEdit textEdit1;
        private DevExpress.XtraEditors.TextEdit textEdit2;
        private DevExpress.XtraEditors.SimpleButton simpleButton2;
    }
}