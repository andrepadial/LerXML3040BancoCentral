namespace LerXML3040BancoCentral
{
    partial class frmExport3040Excel
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
            btnLerXml3040 = new Button();
            lblArquivo3040 = new Label();
            txtDiretorioArquivo3040 = new TextBox();
            opfArquivo3040 = new OpenFileDialog();
            svfArquivo3040 = new SaveFileDialog();
            SuspendLayout();
            // 
            // btnLerXml3040
            // 
            btnLerXml3040.Location = new Point(1474, 73);
            btnLerXml3040.Name = "btnLerXml3040";
            btnLerXml3040.Size = new Size(97, 62);
            btnLerXml3040.TabIndex = 0;
            btnLerXml3040.Text = "...";
            btnLerXml3040.UseVisualStyleBackColor = true;
            // 
            // lblArquivo3040
            // 
            lblArquivo3040.AutoSize = true;
            lblArquivo3040.Location = new Point(12, 22);
            lblArquivo3040.Name = "lblArquivo3040";
            lblArquivo3040.Size = new Size(328, 48);
            lblArquivo3040.TabIndex = 1;
            lblArquivo3040.Text = "Diretório XML 3040";
            // 
            // txtDiretorioArquivo3040
            // 
            txtDiretorioArquivo3040.Location = new Point(12, 73);
            txtDiretorioArquivo3040.Name = "txtDiretorioArquivo3040";
            txtDiretorioArquivo3040.ReadOnly = true;
            txtDiretorioArquivo3040.Size = new Size(1456, 55);
            txtDiretorioArquivo3040.TabIndex = 2;
            // 
            // opfArquivo3040
            // 
            opfArquivo3040.FileName = "opfArquivo3040";
            // 
            // frmExport3040Excel
            // 
            AutoScaleDimensions = new SizeF(20F, 48F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1655, 450);
            Controls.Add(txtDiretorioArquivo3040);
            Controls.Add(lblArquivo3040);
            Controls.Add(btnLerXml3040);
            MaximizeBox = false;
            Name = "frmExport3040Excel";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Exportar XML 3040 p/ Excel";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnLerXml3040;
        private Label lblArquivo3040;
        private TextBox txtDiretorioArquivo3040;
        private OpenFileDialog opfArquivo3040;
        private SaveFileDialog svfArquivo3040;
    }
}