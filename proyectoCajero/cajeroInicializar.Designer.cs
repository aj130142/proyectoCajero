namespace proyectoCajero
{
    partial class cajeroInicializar
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
            lblTotal = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            num200 = new NumericUpDown();
            num100 = new NumericUpDown();
            num50 = new NumericUpDown();
            num20 = new NumericUpDown();
            num10 = new NumericUpDown();
            num5 = new NumericUpDown();
            num1 = new NumericUpDown();
            btnGuardar = new Button();
            ((System.ComponentModel.ISupportInitialize)num200).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num100).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num50).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num20).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num10).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)num1).BeginInit();
            SuspendLayout();
            // 
            // lblTotal
            // 
            lblTotal.AutoSize = true;
            lblTotal.Location = new Point(176, 330);
            lblTotal.Name = "lblTotal";
            lblTotal.Size = new Size(94, 20);
            lblTotal.TabIndex = 2;
            lblTotal.Text = "Total: Q. 0.00";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(144, 104);
            label2.Name = "label2";
            label2.Size = new Size(51, 20);
            label2.TabIndex = 3;
            label2.Text = "Q. 200";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(144, 142);
            label3.Name = "label3";
            label3.Size = new Size(51, 20);
            label3.TabIndex = 4;
            label3.Text = "Q. 100";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(144, 170);
            label4.Name = "label4";
            label4.Size = new Size(43, 20);
            label4.TabIndex = 5;
            label4.Text = "Q. 50";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(144, 203);
            label5.Name = "label5";
            label5.Size = new Size(43, 20);
            label5.TabIndex = 6;
            label5.Text = "Q. 20";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(144, 236);
            label6.Name = "label6";
            label6.Size = new Size(43, 20);
            label6.TabIndex = 7;
            label6.Text = "Q. 10";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(144, 269);
            label7.Name = "label7";
            label7.Size = new Size(35, 20);
            label7.TabIndex = 8;
            label7.Text = "Q. 5";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(144, 302);
            label8.Name = "label8";
            label8.Size = new Size(35, 20);
            label8.TabIndex = 9;
            label8.Text = "Q. 1";
            // 
            // num200
            // 
            num200.Location = new Point(234, 102);
            num200.Name = "num200";
            num200.Size = new Size(128, 27);
            num200.TabIndex = 10;
            num200.ValueChanged += num200_ValueChanged;
            // 
            // num100
            // 
            num100.Location = new Point(234, 135);
            num100.Name = "num100";
            num100.Size = new Size(128, 27);
            num100.TabIndex = 11;
            num100.ValueChanged += num100_ValueChanged;
            // 
            // num50
            // 
            num50.Location = new Point(234, 168);
            num50.Name = "num50";
            num50.Size = new Size(128, 27);
            num50.TabIndex = 12;
            num50.ValueChanged += num50_ValueChanged;
            // 
            // num20
            // 
            num20.Location = new Point(234, 201);
            num20.Name = "num20";
            num20.Size = new Size(128, 27);
            num20.TabIndex = 13;
            num20.ValueChanged += num20_ValueChanged;
            // 
            // num10
            // 
            num10.Location = new Point(234, 234);
            num10.Name = "num10";
            num10.Size = new Size(128, 27);
            num10.TabIndex = 14;
            num10.ValueChanged += num10_ValueChanged;
            // 
            // num5
            // 
            num5.Location = new Point(234, 267);
            num5.Name = "num5";
            num5.Size = new Size(128, 27);
            num5.TabIndex = 15;
            num5.ValueChanged += num5_ValueChanged;
            // 
            // num1
            // 
            num1.Location = new Point(234, 300);
            num1.Name = "num1";
            num1.Size = new Size(128, 27);
            num1.TabIndex = 16;
            num1.ValueChanged += num1_ValueChanged;
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(234, 366);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(128, 72);
            btnGuardar.TabIndex = 17;
            btnGuardar.Text = "Inicializar Cajero";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // cajeroInicializar
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnGuardar);
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
            Controls.Add(lblTotal);
            Name = "cajeroInicializar";
            Text = "cajeroInicializar";
            ((System.ComponentModel.ISupportInitialize)num200).EndInit();
            ((System.ComponentModel.ISupportInitialize)num100).EndInit();
            ((System.ComponentModel.ISupportInitialize)num50).EndInit();
            ((System.ComponentModel.ISupportInitialize)num20).EndInit();
            ((System.ComponentModel.ISupportInitialize)num10).EndInit();
            ((System.ComponentModel.ISupportInitialize)num5).EndInit();
            ((System.ComponentModel.ISupportInitialize)num1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label lblTotal;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private NumericUpDown num200;
        private NumericUpDown num100;
        private NumericUpDown num50;
        private NumericUpDown num20;
        private NumericUpDown num10;
        private NumericUpDown num5;
        private NumericUpDown num1;
        private Button btnGuardar;
    }
}