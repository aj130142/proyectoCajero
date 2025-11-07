namespace proyectoCajero
{
    partial class TransferenciasInternas
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
            cuentaDebitarCB = new ComboBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            textBox1 = new TextBox();
            Descripcion = new TextBox();
            label5 = new Label();
            label6 = new Label();
            tokenTextbox = new TextBox();
            label7 = new Label();
            SaldoCuentaDebitarLabel = new Label();
            MontoretirarRestanteLabel = new Label();
            cuentaCreditarLabel = new TextBox();
            SuspendLayout();
            // 
            // cuentaDebitarCB
            // 
            cuentaDebitarCB.FormattingEnabled = true;
            cuentaDebitarCB.Location = new Point(210, 71);
            cuentaDebitarCB.Name = "cuentaDebitarCB";
            cuentaDebitarCB.Size = new Size(393, 28);
            cuentaDebitarCB.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(41, 79);
            label1.Name = "label1";
            label1.Size = new Size(121, 20);
            label1.TabIndex = 1;
            label1.Text = "Cuenta a Debitar";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(0, 0);
            label2.Name = "label2";
            label2.Size = new Size(0, 20);
            label2.TabIndex = 2;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(41, 159);
            label3.Name = "label3";
            label3.Size = new Size(122, 20);
            label3.TabIndex = 3;
            label3.Text = "Cuenta a creditar";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(41, 248);
            label4.Name = "label4";
            label4.Size = new Size(53, 20);
            label4.TabIndex = 6;
            label4.Text = "Monto";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(210, 241);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(287, 27);
            textBox1.TabIndex = 7;
            // 
            // Descripcion
            // 
            Descripcion.Location = new Point(210, 307);
            Descripcion.MaxLength = 24;
            Descripcion.Name = "Descripcion";
            Descripcion.Size = new Size(287, 27);
            Descripcion.TabIndex = 8;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(41, 310);
            label5.Name = "label5";
            label5.Size = new Size(87, 20);
            label5.TabIndex = 9;
            label5.Text = "Descripcion";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(41, 372);
            label6.Name = "label6";
            label6.Size = new Size(48, 20);
            label6.TabIndex = 10;
            label6.Text = "Token";
            // 
            // tokenTextbox
            // 
            tokenTextbox.Location = new Point(210, 365);
            tokenTextbox.MaxLength = 5;
            tokenTextbox.Name = "tokenTextbox";
            tokenTextbox.Size = new Size(125, 27);
            tokenTextbox.TabIndex = 11;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(427, 369);
            label7.Name = "label7";
            label7.Size = new Size(167, 20);
            label7.TabIndex = 12;
            label7.Text = "(maximo de 5 numeros)";
            // 
            // SaldoCuentaDebitarLabel
            // 
            SaldoCuentaDebitarLabel.AutoSize = true;
            SaldoCuentaDebitarLabel.Location = new Point(213, 105);
            SaldoCuentaDebitarLabel.Name = "SaldoCuentaDebitarLabel";
            SaldoCuentaDebitarLabel.Size = new Size(116, 20);
            SaldoCuentaDebitarLabel.TabIndex = 13;
            SaldoCuentaDebitarLabel.Text = "Saldo de cuenta";
            // 
            // MontoretirarRestanteLabel
            // 
            MontoretirarRestanteLabel.AutoSize = true;
            MontoretirarRestanteLabel.Location = new Point(616, 74);
            MontoretirarRestanteLabel.Name = "MontoretirarRestanteLabel";
            MontoretirarRestanteLabel.Size = new Size(172, 20);
            MontoretirarRestanteLabel.TabIndex = 14;
            MontoretirarRestanteLabel.Text = "Monto que queda retirar";
            // 
            // cuentaCreditarLabel
            // 
            cuentaCreditarLabel.Location = new Point(213, 156);
            cuentaCreditarLabel.Name = "cuentaCreditarLabel";
            cuentaCreditarLabel.Size = new Size(388, 27);
            cuentaCreditarLabel.TabIndex = 15;
            // 
            // TransferenciasInternas
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(cuentaCreditarLabel);
            Controls.Add(MontoretirarRestanteLabel);
            Controls.Add(SaldoCuentaDebitarLabel);
            Controls.Add(label7);
            Controls.Add(tokenTextbox);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(Descripcion);
            Controls.Add(textBox1);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(cuentaDebitarCB);
            Name = "TransferenciasInternas";
            Text = "TransferenciasInternas";
            Load += TransferenciasInternas_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox cuentaDebitarCB;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private TextBox textBox1;
        private TextBox Descripcion;
        private Label label5;
        private Label label6;
        private TextBox tokenTextbox;
        private Label label7;
        private Label SaldoCuentaDebitarLabel;
        private Label MontoretirarRestanteLabel;
        private TextBox cuentaCreditarLabel;
    }
}