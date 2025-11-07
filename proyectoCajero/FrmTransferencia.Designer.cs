namespace proyectoCajero
{
    partial class FrmTransferencia
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
            btnBuscar = new Button();
            cboBancoExt = new ComboBox();
            label1 = new Label();
            lblNomDest = new Label();
            btnConfirmar = new Button();
            txtMontoExt = new TextBox();
            txtConceptoExt = new TextBox();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            mtxtTarjetaExt = new MaskedTextBox();
            lblNombreExt = new Label();
            SuspendLayout();
            // 
            // btnBuscar
            // 
            btnBuscar.Location = new Point(227, 354);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(126, 49);
            btnBuscar.TabIndex = 0;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // cboBancoExt
            // 
            cboBancoExt.FormattingEnabled = true;
            cboBancoExt.Location = new Point(123, 170);
            cboBancoExt.Name = "cboBancoExt";
            cboBancoExt.Size = new Size(196, 28);
            cboBancoExt.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(123, 137);
            label1.Name = "label1";
            label1.Size = new Size(124, 20);
            label1.TabIndex = 2;
            label1.Text = "Banco de destino";
            // 
            // lblNomDest
            // 
            lblNomDest.AutoSize = true;
            lblNomDest.Location = new Point(281, 98);
            lblNomDest.Name = "lblNomDest";
            lblNomDest.Size = new Size(141, 20);
            lblNomDest.TabIndex = 3;
            lblNomDest.Text = "Nombre de destino:";
            // 
            // btnConfirmar
            // 
            btnConfirmar.Location = new Point(415, 354);
            btnConfirmar.Name = "btnConfirmar";
            btnConfirmar.Size = new Size(126, 49);
            btnConfirmar.TabIndex = 4;
            btnConfirmar.Text = "Confirmar";
            btnConfirmar.UseVisualStyleBackColor = true;
            btnConfirmar.Click += btnConfirmar_Click;
            // 
            // txtMontoExt
            // 
            txtMontoExt.Location = new Point(123, 260);
            txtMontoExt.Name = "txtMontoExt";
            txtMontoExt.Size = new Size(196, 27);
            txtMontoExt.TabIndex = 6;
            // 
            // txtConceptoExt
            // 
            txtConceptoExt.Location = new Point(445, 260);
            txtConceptoExt.Name = "txtConceptoExt";
            txtConceptoExt.Size = new Size(196, 27);
            txtConceptoExt.TabIndex = 7;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(445, 137);
            label2.Name = "label2";
            label2.Size = new Size(53, 20);
            label2.TabIndex = 8;
            label2.Text = "Tarjeta";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(123, 237);
            label3.Name = "label3";
            label3.Size = new Size(53, 20);
            label3.TabIndex = 9;
            label3.Text = "Monto";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(445, 237);
            label4.Name = "label4";
            label4.Size = new Size(73, 20);
            label4.TabIndex = 10;
            label4.Text = "Concepto";
            // 
            // mtxtTarjetaExt
            // 
            mtxtTarjetaExt.Location = new Point(445, 170);
            mtxtTarjetaExt.Name = "mtxtTarjetaExt";
            mtxtTarjetaExt.Size = new Size(196, 27);
            mtxtTarjetaExt.TabIndex = 11;
            // 
            // lblNombreExt
            // 
            lblNombreExt.AutoSize = true;
            lblNombreExt.Location = new Point(126, 201);
            lblNombreExt.Name = "lblNombreExt";
            lblNombreExt.Size = new Size(0, 20);
            lblNombreExt.TabIndex = 12;
            // 
            // FrmTransferencia
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(lblNombreExt);
            Controls.Add(mtxtTarjetaExt);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(txtConceptoExt);
            Controls.Add(txtMontoExt);
            Controls.Add(btnConfirmar);
            Controls.Add(lblNomDest);
            Controls.Add(label1);
            Controls.Add(cboBancoExt);
            Controls.Add(btnBuscar);
            Name = "FrmTransferencia";
            Text = "Transferencia";
            Load += FrmTransferencia_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnBuscar;
        private ComboBox cboBancoExt;
        private Label label1;
        private Label lblNomDest;
        private Button btnConfirmar;
        private TextBox txtMontoExt;
        private TextBox txtConceptoExt;
        private Label label2;
        private Label label3;
        private Label label4;
        private MaskedTextBox mtxtTarjetaExt;
        private Label lblNombreExt;
    }
}