namespace proyectoCajero
{
    partial class insertarUsuario
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
            label6 = new Label();
            maxsaldoTB = new TextBox();
            saldoTB = new TextBox();
            pinTB = new TextBox();
            tarjetaTB = new TextBox();
            nameTB = new TextBox();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            okeyBtn = new Button();
            numericMonto = new NumericUpDown();
            numericSaldo = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)numericMonto).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericSaldo).BeginInit();
            SuspendLayout();
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(186, 46);
            label6.Name = "label6";
            label6.Size = new Size(121, 20);
            label6.TabIndex = 21;
            label6.Text = "Ingrese los datos";
            // 
            // maxsaldoTB
            // 
            maxsaldoTB.Location = new Point(186, 313);
            maxsaldoTB.Name = "maxsaldoTB";
            maxsaldoTB.Size = new Size(279, 27);
            maxsaldoTB.TabIndex = 20;
            // 
            // saldoTB
            // 
            saldoTB.Location = new Point(186, 258);
            saldoTB.Name = "saldoTB";
            saldoTB.Size = new Size(279, 27);
            saldoTB.TabIndex = 19;
            // 
            // pinTB
            // 
            pinTB.Location = new Point(186, 207);
            pinTB.MaxLength = 4;
            pinTB.Name = "pinTB";
            pinTB.Size = new Size(125, 27);
            pinTB.TabIndex = 18;
            // 
            // tarjetaTB
            // 
            tarjetaTB.Location = new Point(186, 154);
            tarjetaTB.MaxLength = 16;
            tarjetaTB.Name = "tarjetaTB";
            tarjetaTB.Size = new Size(279, 27);
            tarjetaTB.TabIndex = 17;
            // 
            // nameTB
            // 
            nameTB.Location = new Point(186, 110);
            nameTB.Name = "nameTB";
            nameTB.Size = new Size(473, 27);
            nameTB.TabIndex = 16;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(56, 320);
            label5.Name = "label5";
            label5.Size = new Size(111, 20);
            label5.TabIndex = 15;
            label5.Text = "Monto Maximo";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(56, 261);
            label4.Name = "label4";
            label4.Size = new Size(47, 20);
            label4.TabIndex = 14;
            label4.Text = "Saldo";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(56, 214);
            label3.Name = "label3";
            label3.Size = new Size(56, 20);
            label3.TabIndex = 13;
            label3.Text = "No. Pin";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(56, 161);
            label2.Name = "label2";
            label2.Size = new Size(80, 20);
            label2.TabIndex = 12;
            label2.Text = "No. Tarjeta";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(56, 113);
            label1.Name = "label1";
            label1.Size = new Size(64, 20);
            label1.TabIndex = 11;
            label1.Text = "Nombre";
            // 
            // okeyBtn
            // 
            okeyBtn.Location = new Point(189, 384);
            okeyBtn.Name = "okeyBtn";
            okeyBtn.Size = new Size(94, 29);
            okeyBtn.TabIndex = 22;
            okeyBtn.Text = "Aceptar";
            okeyBtn.UseVisualStyleBackColor = true;
            okeyBtn.Click += okeyBtn_Click;
            okeyBtn.MouseClick += okeyBtn_MouseClick;
            // 
            // numericMonto
            // 
            numericMonto.Location = new Point(471, 313);
            numericMonto.Maximum = new decimal(new int[] { 14000, 0, 0, 0 });
            numericMonto.Name = "numericMonto";
            numericMonto.Size = new Size(279, 27);
            numericMonto.TabIndex = 23;
            // 
            // numericSaldo
            // 
            numericSaldo.Location = new Point(471, 258);
            numericSaldo.Name = "numericSaldo";
            numericSaldo.Size = new Size(279, 27);
            numericSaldo.TabIndex = 24;
            // 
            // insertarUsuario
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(779, 450);
            Controls.Add(numericSaldo);
            Controls.Add(numericMonto);
            Controls.Add(okeyBtn);
            Controls.Add(label6);
            Controls.Add(maxsaldoTB);
            Controls.Add(saldoTB);
            Controls.Add(pinTB);
            Controls.Add(tarjetaTB);
            Controls.Add(nameTB);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Name = "insertarUsuario";
            Text = "Nuevo usuario";
            Load += insertarUsuario_Load;
            ((System.ComponentModel.ISupportInitialize)numericMonto).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericSaldo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label6;
        private TextBox maxsaldoTB;
        private TextBox saldoTB;
        private TextBox pinTB;
        private TextBox tarjetaTB;
        private TextBox nameTB;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private Button okeyBtn;
        private NumericUpDown numericMonto;
        private NumericUpDown numericSaldo;
    }
}