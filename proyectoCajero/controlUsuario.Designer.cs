namespace proyectoCajero
{
    partial class controlUsuario
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
            groupBox1 = new GroupBox();
            txtBuscarUsuario = new TextBox();
            btnBuscar = new Button();
            lblInfoUsuario = new Label();
            label3 = new Label();
            groupBox2 = new GroupBox();
            lblControlDiario = new Label();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(txtBuscarUsuario);
            groupBox1.Controls.Add(btnBuscar);
            groupBox1.Controls.Add(lblInfoUsuario);
            groupBox1.Controls.Add(label3);
            groupBox1.Location = new Point(12, 45);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(450, 366);
            groupBox1.TabIndex = 6;
            groupBox1.TabStop = false;
            groupBox1.Text = "Consulta de Usuario Específico";
            // 
            // txtBuscarUsuario
            // 
            txtBuscarUsuario.Location = new Point(181, 38);
            txtBuscarUsuario.Name = "txtBuscarUsuario";
            txtBuscarUsuario.Size = new Size(177, 27);
            txtBuscarUsuario.TabIndex = 3;
            // 
            // btnBuscar
            // 
            btnBuscar.Location = new Point(364, 33);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(77, 37);
            btnBuscar.TabIndex = 2;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // lblInfoUsuario
            // 
            lblInfoUsuario.AutoSize = true;
            lblInfoUsuario.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblInfoUsuario.Location = new Point(6, 74);
            lblInfoUsuario.Name = "lblInfoUsuario";
            lblInfoUsuario.Size = new Size(117, 28);
            lblInfoUsuario.TabIndex = 1;
            lblInfoUsuario.Text = "Informacion";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(19, 41);
            label3.Name = "label3";
            label3.Size = new Size(142, 20);
            label3.TabIndex = 0;
            label3.Text = "Nombre de Usuario:";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(lblControlDiario);
            groupBox2.Location = new Point(520, 45);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(450, 366);
            groupBox2.TabIndex = 7;
            groupBox2.TabStop = false;
            groupBox2.Text = "Control Diario General";
            // 
            // lblControlDiario
            // 
            lblControlDiario.AutoSize = true;
            lblControlDiario.Font = new Font("Segoe UI", 12F);
            lblControlDiario.Location = new Point(6, 23);
            lblControlDiario.Name = "lblControlDiario";
            lblControlDiario.Size = new Size(65, 28);
            lblControlDiario.TabIndex = 0;
            lblControlDiario.Text = "Diario";
            // 
            // controlUsuario
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(982, 538);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "controlUsuario";
            Text = "Consulta de Usuario";
            Load += controlUsuario_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private GroupBox groupBox1;
        private Label lblInfoUsuario;
        private Label label3;
        private TextBox txtBuscarUsuario;
        private Button btnBuscar;
        private GroupBox groupBox2;
        private Label lblControlDiario;
    }
}