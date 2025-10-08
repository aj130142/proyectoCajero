namespace proyectoCajero
{
    partial class modificarUsuarios
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
            label1 = new Label();
            nameTB = new TextBox();
            label2 = new Label();
            label3 = new Label();
            tarjetaNewTB = new TextBox();
            maxSaldoTB = new TextBox();
            label4 = new Label();
            modTarjCheckB = new CheckBox();
            modMaxSaldoCheckB = new CheckBox();
            okeyBtn = new Button();
            label5 = new Label();
            allSelect = new CheckBox();
            numericUpDown1 = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(137, 136);
            label1.Name = "label1";
            label1.Size = new Size(141, 20);
            label1.TabIndex = 0;
            label1.Text = "Nombre del usuario";
            // 
            // nameTB
            // 
            nameTB.Location = new Point(328, 129);
            nameTB.Name = "nameTB";
            nameTB.Size = new Size(361, 27);
            nameTB.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(152, 200);
            label2.Name = "label2";
            label2.Size = new Size(126, 20);
            label2.TabIndex = 2;
            label2.Text = "Nuevo No. tarjeta";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(137, 266);
            label3.Name = "label3";
            label3.Size = new Size(158, 20);
            label3.TabIndex = 3;
            label3.Text = "Nuevo Monto Maximo";
            // 
            // tarjetaNewTB
            // 
            tarjetaNewTB.Location = new Point(328, 193);
            tarjetaNewTB.Name = "tarjetaNewTB";
            tarjetaNewTB.Size = new Size(276, 27);
            tarjetaNewTB.TabIndex = 4;
            // 
            // maxSaldoTB
            // 
            maxSaldoTB.Location = new Point(328, 366);
            maxSaldoTB.Name = "maxSaldoTB";
            maxSaldoTB.Size = new Size(276, 27);
            maxSaldoTB.TabIndex = 5;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(251, 47);
            label4.Name = "label4";
            label4.Size = new Size(121, 20);
            label4.TabIndex = 6;
            label4.Text = "Ingrese los datos";
            // 
            // modTarjCheckB
            // 
            modTarjCheckB.AutoSize = true;
            modTarjCheckB.Location = new Point(98, 199);
            modTarjCheckB.Name = "modTarjCheckB";
            modTarjCheckB.Size = new Size(18, 17);
            modTarjCheckB.TabIndex = 7;
            modTarjCheckB.UseVisualStyleBackColor = true;
            modTarjCheckB.CheckedChanged += modTarjCheckB_CheckedChanged_1;
            // 
            // modMaxSaldoCheckB
            // 
            modMaxSaldoCheckB.AutoSize = true;
            modMaxSaldoCheckB.Location = new Point(98, 269);
            modMaxSaldoCheckB.Name = "modMaxSaldoCheckB";
            modMaxSaldoCheckB.Size = new Size(18, 17);
            modMaxSaldoCheckB.TabIndex = 8;
            modMaxSaldoCheckB.UseVisualStyleBackColor = true;
            modMaxSaldoCheckB.CheckedChanged += modMaxSaldoCheckB_CheckedChanged;
            // 
            // okeyBtn
            // 
            okeyBtn.Location = new Point(137, 343);
            okeyBtn.Name = "okeyBtn";
            okeyBtn.Size = new Size(94, 29);
            okeyBtn.TabIndex = 9;
            okeyBtn.Text = "Aceptar";
            okeyBtn.UseVisualStyleBackColor = true;
            okeyBtn.Click += okeyBtn_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(23, 129);
            label5.Name = "label5";
            label5.Size = new Size(49, 20);
            label5.TabIndex = 10;
            label5.Text = "Todos";
            // 
            // allSelect
            // 
            allSelect.AutoSize = true;
            allSelect.Location = new Point(98, 135);
            allSelect.Name = "allSelect";
            allSelect.Size = new Size(18, 17);
            allSelect.TabIndex = 11;
            allSelect.UseVisualStyleBackColor = true;
            allSelect.CheckedChanged += allSelect_CheckedChanged;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(328, 266);
            numericUpDown1.Maximum = new decimal(new int[] { 14000, 0, 0, 0 });
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(150, 27);
            numericUpDown1.TabIndex = 12;
            // 
            // modificarUsuarios
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(746, 450);
            Controls.Add(numericUpDown1);
            Controls.Add(allSelect);
            Controls.Add(label5);
            Controls.Add(okeyBtn);
            Controls.Add(modMaxSaldoCheckB);
            Controls.Add(modTarjCheckB);
            Controls.Add(label4);
            Controls.Add(maxSaldoTB);
            Controls.Add(tarjetaNewTB);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(nameTB);
            Controls.Add(label1);
            Name = "modificarUsuarios";
            Text = "Modificar usuarios";
            Load += modificarUsuarios_Load;
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox nameTB;
        private Label label2;
        private Label label3;
        private TextBox tarjetaNewTB;
        private TextBox maxSaldoTB;
        private Label label4;
        private CheckBox modTarjCheckB;
        private CheckBox modMaxSaldoCheckB;
        private Button okeyBtn;
        private Label label5;
        private CheckBox allSelect;
        private NumericUpDown numericUpDown1;
    }
}