namespace proyectoCajero
{
    partial class OperacionForm
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
            num1 = new NumericUpDown();
            num5 = new NumericUpDown();
            num10 = new NumericUpDown();
            num20 = new NumericUpDown();
            num50 = new NumericUpDown();
            num100 = new NumericUpDown();
            num200 = new NumericUpDown();
            label8 = new Label();
            label7 = new Label();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            lblTotalOperacion = new Label();
            btnAceptar = new Button();
            btnCancelar = new Button();
            ((System.ComponentModel.ISupportInitialize)num1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num10).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num20).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num50).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num100).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num200).BeginInit();
            SuspendLayout();
            // 
            // num1
            // 
            num1.Location = new Point(278, 291);
            num1.Name = "num1";
            num1.Size = new Size(128, 27);
            num1.TabIndex = 31;
            num1.ValueChanged += num1_ValueChanged;
            // 
            // num5
            // 
            num5.Location = new Point(278, 258);
            num5.Name = "num5";
            num5.Size = new Size(128, 27);
            num5.TabIndex = 30;
            num5.ValueChanged += num5_ValueChanged;
            // 
            // num10
            // 
            num10.Location = new Point(278, 225);
            num10.Name = "num10";
            num10.Size = new Size(128, 27);
            num10.TabIndex = 29;
            num10.ValueChanged += num10_ValueChanged;
            // 
            // num20
            // 
            num20.Location = new Point(278, 192);
            num20.Name = "num20";
            num20.Size = new Size(128, 27);
            num20.TabIndex = 28;
            num20.ValueChanged += num20_ValueChanged;
            // 
            // num50
            // 
            num50.Location = new Point(278, 159);
            num50.Name = "num50";
            num50.Size = new Size(128, 27);
            num50.TabIndex = 27;
            num50.ValueChanged += num50_ValueChanged;
            // 
            // num100
            // 
            num100.Location = new Point(278, 126);
            num100.Name = "num100";
            num100.Size = new Size(128, 27);
            num100.TabIndex = 26;
            num100.ValueChanged += num100_ValueChanged;
            // 
            // num200
            // 
            num200.Location = new Point(278, 93);
            num200.Name = "num200";
            num200.Size = new Size(128, 27);
            num200.TabIndex = 25;
            num200.ValueChanged += num200_ValueChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(188, 293);
            label8.Name = "label8";
            label8.Size = new Size(35, 20);
            label8.TabIndex = 24;
            label8.Text = "Q. 1";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(188, 260);
            label7.Name = "label7";
            label7.Size = new Size(35, 20);
            label7.TabIndex = 23;
            label7.Text = "Q. 5";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(188, 227);
            label6.Name = "label6";
            label6.Size = new Size(43, 20);
            label6.TabIndex = 22;
            label6.Text = "Q. 10";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(188, 194);
            label5.Name = "label5";
            label5.Size = new Size(43, 20);
            label5.TabIndex = 21;
            label5.Text = "Q. 20";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(188, 161);
            label4.Name = "label4";
            label4.Size = new Size(43, 20);
            label4.TabIndex = 20;
            label4.Text = "Q. 50";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(188, 133);
            label3.Name = "label3";
            label3.Size = new Size(51, 20);
            label3.TabIndex = 19;
            label3.Text = "Q. 100";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(188, 95);
            label2.Name = "label2";
            label2.Size = new Size(51, 20);
            label2.TabIndex = 18;
            label2.Text = "Q. 200";
            // 
            // lblTotalOperacion
            // 
            lblTotalOperacion.AutoSize = true;
            lblTotalOperacion.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTotalOperacion.Location = new Point(510, 93);
            lblTotalOperacion.Name = "lblTotalOperacion";
            lblTotalOperacion.Size = new Size(124, 28);
            lblTotalOperacion.TabIndex = 17;
            lblTotalOperacion.Text = "Total: Q. 0.00";
            // 
            // btnAceptar
            // 
            btnAceptar.Location = new Point(188, 367);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(94, 29);
            btnAceptar.TabIndex = 32;
            btnAceptar.Text = "Aceptar";
            btnAceptar.UseVisualStyleBackColor = true;
            btnAceptar.Click += btnAceptar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(406, 367);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(94, 29);
            btnCancelar.TabIndex = 33;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // OperacionForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnCancelar);
            Controls.Add(btnAceptar);
            Controls.Add(num1);
            Controls.Add(num5);
            Controls.Add(num10);
            Controls.Add(num20);
            Controls.Add(num50);
            Controls.Add(num100);
            Controls.Add(num200);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(lblTotalOperacion);
            Name = "OperacionForm";
            Text = "OperacionForm";
            Load += OperacionForm_Load;
            ((System.ComponentModel.ISupportInitialize)num1).EndInit();
            ((System.ComponentModel.ISupportInitialize)num5).EndInit();
            ((System.ComponentModel.ISupportInitialize)num10).EndInit();
            ((System.ComponentModel.ISupportInitialize)num20).EndInit();
            ((System.ComponentModel.ISupportInitialize)num50).EndInit();
            ((System.ComponentModel.ISupportInitialize)num100).EndInit();
            ((System.ComponentModel.ISupportInitialize)num200).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private NumericUpDown num1;
        private NumericUpDown num5;
        private NumericUpDown num10;
        private NumericUpDown num20;
        private NumericUpDown num50;
        private NumericUpDown num100;
        private NumericUpDown num200;
        private Label label8;
        private Label label7;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label lblTotalOperacion;
        private Button btnAceptar;
        private Button btnCancelar;
    }
}