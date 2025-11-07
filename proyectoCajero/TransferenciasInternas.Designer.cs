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
            btnGenerarToken = new Button();
            btnTransferir = new Button();
            btnAgregarCuenta = new Button();
            historialBtn = new Button();
            SuspendLayout();
            // 
            // cuentaDebitarCB
            // 
            cuentaDebitarCB.DropDownStyle = ComboBoxStyle.DropDownList;
            cuentaDebitarCB.FormattingEnabled = true;
            cuentaDebitarCB.Location = new Point(210, 71);
            cuentaDebitarCB.Name = "cuentaDebitarCB";
            cuentaDebitarCB.Size = new Size(393, 28);
            cuentaDebitarCB.TabIndex = 0;
            cuentaDebitarCB.SelectedIndexChanged += cuentaDebitarCB_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label1.Location = new Point(41, 79);
            label1.Name = "label1";
            label1.Size = new Size(126, 20);
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
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label3.Location = new Point(41, 159);
            label3.Name = "label3";
            label3.Size = new Size(139, 20);
            label3.TabIndex = 3;
            label3.Text = "Cuenta a Acreditar";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label4.Location = new Point(41, 248);
            label4.Name = "label4";
            label4.Size = new Size(56, 20);
            label4.TabIndex = 6;
            label4.Text = "Monto";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(210, 241);
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "Ingrese el monto";
            textBox1.Size = new Size(287, 27);
            textBox1.TabIndex = 7;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // Descripcion
            // 
            Descripcion.Location = new Point(210, 307);
            Descripcion.MaxLength = 100;
            Descripcion.Name = "Descripcion";
            Descripcion.PlaceholderText = "Ej: Pago de servicio";
            Descripcion.Size = new Size(393, 27);
            Descripcion.TabIndex = 8;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label5.Location = new Point(41, 310);
            label5.Name = "label5";
            label5.Size = new Size(90, 20);
            label5.TabIndex = 9;
            label5.Text = "Descripción";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            label6.Location = new Point(41, 372);
            label6.Name = "label6";
            label6.Size = new Size(51, 20);
            label6.TabIndex = 10;
            label6.Text = "Token";
            // 
            // tokenTextbox
            // 
            tokenTextbox.Location = new Point(210, 365);
            tokenTextbox.MaxLength = 5;
            tokenTextbox.Name = "tokenTextbox";
            tokenTextbox.PlaceholderText = "00000";
            tokenTextbox.ReadOnly = true;
            tokenTextbox.Size = new Size(125, 27);
            tokenTextbox.TabIndex = 11;
            tokenTextbox.TextAlign = HorizontalAlignment.Center;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.ForeColor = Color.Gray;
            label7.Location = new Point(351, 369);
            label7.Name = "label7";
            label7.Size = new Size(167, 20);
            label7.TabIndex = 12;
            label7.Text = "(máximo de 5 números)";
            // 
            // SaldoCuentaDebitarLabel
            // 
            SaldoCuentaDebitarLabel.AutoSize = true;
            SaldoCuentaDebitarLabel.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            SaldoCuentaDebitarLabel.ForeColor = Color.FromArgb(0, 122, 204);
            SaldoCuentaDebitarLabel.Location = new Point(213, 105);
            SaldoCuentaDebitarLabel.Name = "SaldoCuentaDebitarLabel";
            SaldoCuentaDebitarLabel.Size = new Size(110, 20);
            SaldoCuentaDebitarLabel.TabIndex = 13;
            SaldoCuentaDebitarLabel.Text = "Saldo de cuenta";
            // 
            // MontoretirarRestanteLabel
            // 
            MontoretirarRestanteLabel.AutoSize = true;
            MontoretirarRestanteLabel.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            MontoretirarRestanteLabel.Location = new Point(520, 248);
            MontoretirarRestanteLabel.Name = "MontoretirarRestanteLabel";
            MontoretirarRestanteLabel.Size = new Size(125, 20);
            MontoretirarRestanteLabel.TabIndex = 14;
            MontoretirarRestanteLabel.Text = "Saldo restante: Q0";
            // 
            // cuentaCreditarLabel
            // 
            cuentaCreditarLabel.Location = new Point(213, 156);
            cuentaCreditarLabel.MaxLength = 20;
            cuentaCreditarLabel.Name = "cuentaCreditarLabel";
            cuentaCreditarLabel.PlaceholderText = "Número de cuenta destino";
            cuentaCreditarLabel.Size = new Size(284, 27);
            cuentaCreditarLabel.TabIndex = 15;
            // 
            // btnGenerarToken
            // 
            btnGenerarToken.BackColor = Color.FromArgb(255, 152, 0);
            btnGenerarToken.FlatStyle = FlatStyle.Flat;
            btnGenerarToken.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnGenerarToken.ForeColor = Color.White;
            btnGenerarToken.Location = new Point(41, 410);
            btnGenerarToken.Name = "btnGenerarToken";
            btnGenerarToken.Size = new Size(180, 40);
            btnGenerarToken.TabIndex = 16;
            btnGenerarToken.Text = "🔐 Generar Token";
            btnGenerarToken.UseVisualStyleBackColor = false;
            btnGenerarToken.Click += btnGenerarToken_Click;
            // 
            // btnTransferir
            // 
            btnTransferir.BackColor = Color.FromArgb(76, 175, 80);
            btnTransferir.FlatStyle = FlatStyle.Flat;
            btnTransferir.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnTransferir.ForeColor = Color.White;
            btnTransferir.Location = new Point(240, 410);
            btnTransferir.Name = "btnTransferir";
            btnTransferir.Size = new Size(200, 40);
            btnTransferir.TabIndex = 17;
            btnTransferir.Text = "💸 Transferir";
            btnTransferir.UseVisualStyleBackColor = false;
            btnTransferir.Click += btnTransferir_Click;
            // 
            // btnAgregarCuenta
            // 
            btnAgregarCuenta.BackColor = Color.FromArgb(0, 122, 204);
            btnAgregarCuenta.FlatStyle = FlatStyle.Flat;
            btnAgregarCuenta.Font = new Font("Segoe UI", 9F);
            btnAgregarCuenta.ForeColor = Color.White;
            btnAgregarCuenta.Location = new Point(510, 153);
            btnAgregarCuenta.Name = "btnAgregarCuenta";
            btnAgregarCuenta.Size = new Size(93, 32);
            btnAgregarCuenta.TabIndex = 18;
            btnAgregarCuenta.Text = "➕ Agregar";
            btnAgregarCuenta.UseVisualStyleBackColor = false;
            btnAgregarCuenta.Click += btnAgregarCuenta_Click;
            // 
            // historialBtn
            // 
            historialBtn.Location = new Point(522, 20);
            historialBtn.Name = "historialBtn";
            historialBtn.Size = new Size(94, 29);
            historialBtn.TabIndex = 19;
            historialBtn.Text = "Historial";
            historialBtn.UseVisualStyleBackColor = true;
            historialBtn.Click += historialBtn_Click;
            // 
            // TransferenciasInternas
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(684, 471);
            Controls.Add(historialBtn);
            Controls.Add(btnAgregarCuenta);
            Controls.Add(btnTransferir);
            Controls.Add(btnGenerarToken);
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
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "TransferenciasInternas";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Transferencias Internas";
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
        private Button btnGenerarToken;
        private Button btnTransferir;
        private Button btnAgregarCuenta;
        private Button historialBtn;
    }
}
