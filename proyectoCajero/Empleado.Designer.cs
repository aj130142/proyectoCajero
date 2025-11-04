namespace CapaPresentacion
{
    partial class GestionarEmpleado
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            label1 = new Label();
            nombreUsuariotxt = new TextBox();
            contrasenaTxt = new TextBox();
            label2 = new Label();
            nombresTxt = new TextBox();
            label3 = new Label();
            apellidosTxt = new TextBox();
            label4 = new Label();
            correoTxt = new TextBox();
            label5 = new Label();
            btnGuardar = new Button();
            btnEditar = new Button();
            btnEliminar = new Button();
            label6 = new Label();
            label7 = new Label();
            rolCBox = new ComboBox();
            checkBox1 = new CheckBox();
            descripcionRolLabel = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(16, 18);
            dataGridView1.Margin = new Padding(4, 5, 4, 5);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(727, 380);
            dataGridView1.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(797, 46);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(142, 20);
            label1.TabIndex = 1;
            label1.Text = "Nombre de Usuario:";
            // 
            // nombreUsuariotxt
            // 
            nombreUsuariotxt.Location = new Point(947, 43);
            nombreUsuariotxt.Margin = new Padding(4, 5, 4, 5);
            nombreUsuariotxt.Name = "nombreUsuariotxt";
            nombreUsuariotxt.Size = new Size(244, 27);
            nombreUsuariotxt.TabIndex = 2;
            // 
            // contrasenaTxt
            // 
            contrasenaTxt.Location = new Point(947, 83);
            contrasenaTxt.Margin = new Padding(4, 5, 4, 5);
            contrasenaTxt.Name = "contrasenaTxt";
            contrasenaTxt.Size = new Size(244, 27);
            contrasenaTxt.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(797, 86);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(86, 20);
            label2.TabIndex = 3;
            label2.Text = "Contraseña:";
            // 
            // nombresTxt
            // 
            nombresTxt.Location = new Point(947, 123);
            nombresTxt.Margin = new Padding(4, 5, 4, 5);
            nombresTxt.Name = "nombresTxt";
            nombresTxt.Size = new Size(244, 27);
            nombresTxt.TabIndex = 6;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(797, 126);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(73, 20);
            label3.TabIndex = 5;
            label3.Text = "Nombres:";
            // 
            // apellidosTxt
            // 
            apellidosTxt.Location = new Point(947, 163);
            apellidosTxt.Margin = new Padding(4, 5, 4, 5);
            apellidosTxt.Name = "apellidosTxt";
            apellidosTxt.Size = new Size(244, 27);
            apellidosTxt.TabIndex = 8;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(797, 166);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(75, 20);
            label4.TabIndex = 7;
            label4.Text = "Apellidos:";
            // 
            // correoTxt
            // 
            correoTxt.Location = new Point(947, 203);
            correoTxt.Margin = new Padding(4, 5, 4, 5);
            correoTxt.Name = "correoTxt";
            correoTxt.Size = new Size(244, 27);
            correoTxt.TabIndex = 10;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(797, 206);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(135, 20);
            label5.TabIndex = 9;
            label5.Text = "Correo Electronico:";
            // 
            // btnGuardar
            // 
            btnGuardar.Location = new Point(867, 398);
            btnGuardar.Margin = new Padding(4, 5, 4, 5);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(324, 54);
            btnGuardar.TabIndex = 11;
            btnGuardar.Text = "Guardar:";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnEditar
            // 
            btnEditar.Location = new Point(17, 409);
            btnEditar.Margin = new Padding(4, 5, 4, 5);
            btnEditar.Name = "btnEditar";
            btnEditar.Size = new Size(100, 35);
            btnEditar.TabIndex = 12;
            btnEditar.Text = "Editar";
            btnEditar.UseVisualStyleBackColor = true;
            btnEditar.Click += btnEditar_Click;
            // 
            // btnEliminar
            // 
            btnEliminar.Location = new Point(125, 408);
            btnEliminar.Margin = new Padding(4, 5, 4, 5);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(100, 35);
            btnEliminar.TabIndex = 13;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = true;
            btnEliminar.Click += btnEliminar_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(797, 250);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(34, 20);
            label6.TabIndex = 14;
            label6.Text = "Rol:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(797, 331);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(54, 20);
            label7.TabIndex = 15;
            label7.Text = "Activo:";
            // 
            // rolCBox
            // 
            rolCBox.FormattingEnabled = true;
            rolCBox.Location = new Point(947, 247);
            rolCBox.Name = "rolCBox";
            rolCBox.Size = new Size(244, 28);
            rolCBox.TabIndex = 16;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Checked = true;
            checkBox1.CheckState = CheckState.Checked;
            checkBox1.Location = new Point(947, 330);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(43, 24);
            checkBox1.TabIndex = 17;
            checkBox1.Text = "Si";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // descripcionRolLabel
            // 
            descripcionRolLabel.AutoSize = true;
            descripcionRolLabel.Location = new Point(797, 293);
            descripcionRolLabel.Margin = new Padding(4, 0, 4, 0);
            descripcionRolLabel.Name = "descripcionRolLabel";
            descripcionRolLabel.Size = new Size(133, 20);
            descripcionRolLabel.TabIndex = 18;
            descripcionRolLabel.Text = "Descripcion de rol:";
            // 
            // GestionarEmpleado
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1244, 525);
            Controls.Add(descripcionRolLabel);
            Controls.Add(checkBox1);
            Controls.Add(rolCBox);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(btnEliminar);
            Controls.Add(btnEditar);
            Controls.Add(btnGuardar);
            Controls.Add(correoTxt);
            Controls.Add(label5);
            Controls.Add(apellidosTxt);
            Controls.Add(label4);
            Controls.Add(nombresTxt);
            Controls.Add(label3);
            Controls.Add(contrasenaTxt);
            Controls.Add(label2);
            Controls.Add(nombreUsuariotxt);
            Controls.Add(label1);
            Controls.Add(dataGridView1);
            Margin = new Padding(4, 5, 4, 5);
            Name = "GestionarEmpleado";
            Text = "Gestionar Empleado";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox nombreUsuariotxt;
        private System.Windows.Forms.TextBox contrasenaTxt;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox nombresTxt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox apellidosTxt;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox correoTxt;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnEliminar;
        private Label label6;
        private Label label7;
        private ComboBox rolCBox;
        private CheckBox checkBox1;
        private Label descripcionRolLabel;
    }
}

