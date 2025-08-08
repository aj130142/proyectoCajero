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
            label3.Size = new Size(154, 20);
            label3.TabIndex = 3;
            label3.Text = "Nuevo No. Max. saldo";
            // 
            // tarjetaNewTB
            // 
            tarjetaNewTB.Location = new Point(328, 193);
            tarjetaNewTB.Name = "tarjetaNewTB";
            tarjetaNewTB.Size = new Size(125, 27);
            tarjetaNewTB.TabIndex = 4;
            // 
            // maxSaldoTB
            // 
            maxSaldoTB.Location = new Point(328, 259);
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
            modTarjCheckB.Location = new Point(12, 200);
            modTarjCheckB.Name = "modTarjCheckB";
            modTarjCheckB.Size = new Size(95, 24);
            modTarjCheckB.TabIndex = 7;
            modTarjCheckB.Text = "Modificar";
            modTarjCheckB.UseVisualStyleBackColor = true;
            // 
            // modMaxSaldoCheckB
            // 
            modMaxSaldoCheckB.AutoSize = true;
            modMaxSaldoCheckB.Location = new Point(12, 259);
            modMaxSaldoCheckB.Name = "modMaxSaldoCheckB";
            modMaxSaldoCheckB.Size = new Size(95, 24);
            modMaxSaldoCheckB.TabIndex = 8;
            modMaxSaldoCheckB.Text = "Modificar";
            modMaxSaldoCheckB.UseVisualStyleBackColor = true;
            modMaxSaldoCheckB.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // modificarUsuarios
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(746, 450);
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
    }
}