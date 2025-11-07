namespace proyectoCajero
{
    partial class HistorialtransaccionCuenta
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
            dataGridView1 = new DataGridView();
            NumeroCuentaTB = new TextBox();
            IniciodateTimePicker = new DateTimePicker();
            FindateTimePicker = new DateTimePicker();
            label1 = new Label();
            label2 = new Label();
            btnBuscar = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(12, 125);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(762, 313);
            dataGridView1.TabIndex = 0;
            // 
            // NumeroCuentaTB
            // 
            NumeroCuentaTB.Location = new Point(88, 44);
            NumeroCuentaTB.Name = "NumeroCuentaTB";
            NumeroCuentaTB.ReadOnly = true;
            NumeroCuentaTB.Size = new Size(255, 27);
            NumeroCuentaTB.TabIndex = 1;
            NumeroCuentaTB.TextChanged += NumeroCuentaTB_TextChanged;
            // 
            // IniciodateTimePicker
            // 
            IniciodateTimePicker.Format = DateTimePickerFormat.Short;
            IniciodateTimePicker.Location = new Point(362, 44);
            IniciodateTimePicker.Name = "IniciodateTimePicker";
            IniciodateTimePicker.Size = new Size(129, 27);
            IniciodateTimePicker.TabIndex = 2;
            IniciodateTimePicker.Value = new DateTime(2025, 11, 6, 0, 0, 0, 0);
            // 
            // FindateTimePicker
            // 
            FindateTimePicker.Format = DateTimePickerFormat.Short;
            FindateTimePicker.Location = new Point(521, 44);
            FindateTimePicker.Name = "FindateTimePicker";
            FindateTimePicker.Size = new Size(129, 27);
            FindateTimePicker.TabIndex = 3;
            FindateTimePicker.Value = new DateTime(2025, 11, 6, 0, 0, 0, 0);
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(89, 14);
            label1.Name = "label1";
            label1.Size = new Size(129, 20);
            label1.TabIndex = 4;
            label1.Text = "numero de cuenta";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(362, 14);
            label2.Name = "label2";
            label2.Size = new Size(47, 20);
            label2.TabIndex = 5;
            label2.Text = "Fecha";
            // 
            // btnBuscar
            // 
            btnBuscar.Location = new Point(88, 90);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(94, 29);
            btnBuscar.TabIndex = 6;
            btnBuscar.Text = "button1";
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // HistorialtransaccionCuenta
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnBuscar);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(FindateTimePicker);
            Controls.Add(IniciodateTimePicker);
            Controls.Add(NumeroCuentaTB);
            Controls.Add(dataGridView1);
            Name = "HistorialtransaccionCuenta";
            Text = "HistorialtransaccionCuenta";
            Load += HistorialtransaccionCuenta_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridView1;
        private TextBox NumeroCuentaTB;
        private DateTimePicker IniciodateTimePicker;
        private DateTimePicker FindateTimePicker;
        private Label label1;
        private Label label2;
        private Button btnBuscar;
    }
}