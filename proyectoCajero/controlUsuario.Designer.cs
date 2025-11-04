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
            groupBox2 = new GroupBox();
            lblControlDiario = new Label();
            groupBoxFilters = new GroupBox();
            labelFecha = new Label();
            dtpFecha = new DateTimePicker();
            labelTipo = new Label();
            cmbTipoOperacion = new ComboBox();
            labelTarjeta = new Label();
            txtFiltroTarjeta = new TextBox();
            labelTop = new Label();
            cmbTopMode = new ComboBox();
            btnAplicarFiltro = new Button();
            dgvResultados = new DataGridView();
            lblStatus = new Label();
            label3 = new Label();
            lblInfoUsuario = new Label();
            btnBuscar = new Button();
            txtBuscarUsuario = new TextBox();
            groupBox1 = new GroupBox();
            groupBox2.SuspendLayout();
            groupBoxFilters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvResultados).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
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
            // groupBoxFilters
            // 
            groupBoxFilters.Controls.Add(labelFecha);
            groupBoxFilters.Controls.Add(dtpFecha);
            groupBoxFilters.Controls.Add(labelTipo);
            groupBoxFilters.Controls.Add(cmbTipoOperacion);
            groupBoxFilters.Controls.Add(labelTarjeta);
            groupBoxFilters.Controls.Add(txtFiltroTarjeta);
            groupBoxFilters.Controls.Add(labelTop);
            groupBoxFilters.Controls.Add(cmbTopMode);
            groupBoxFilters.Controls.Add(btnAplicarFiltro);
            groupBoxFilters.Location = new Point(12, 205);
            groupBoxFilters.Name = "groupBoxFilters";
            groupBoxFilters.Size = new Size(450, 206);
            groupBoxFilters.TabIndex = 8;
            groupBoxFilters.TabStop = false;
            groupBoxFilters.Text = "Filtros y Top 10";
            // 
            // labelFecha
            // 
            labelFecha.AutoSize = true;
            labelFecha.Location = new Point(10, 28);
            labelFecha.Name = "labelFecha";
            labelFecha.Size = new Size(50, 20);
            labelFecha.TabIndex = 0;
            labelFecha.Text = "Fecha:";
            // 
            // dtpFecha
            // 
            dtpFecha.Format = DateTimePickerFormat.Short;
            dtpFecha.Location = new Point(80, 25);
            dtpFecha.Name = "dtpFecha";
            dtpFecha.Size = new Size(120, 27);
            dtpFecha.TabIndex = 1;
            // 
            // labelTipo
            // 
            labelTipo.AutoSize = true;
            labelTipo.Location = new Point(10, 62);
            labelTipo.Name = "labelTipo";
            labelTipo.Size = new Size(42, 20);
            labelTipo.TabIndex = 6;
            labelTipo.Text = "Tipo:";
            // 
            // cmbTipoOperacion
            // 
            cmbTipoOperacion.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTipoOperacion.Items.AddRange(new object[] { "Ambos", "Retiro", "Deposito" });
            cmbTipoOperacion.Location = new Point(80, 58);
            cmbTipoOperacion.Name = "cmbTipoOperacion";
            cmbTipoOperacion.Size = new Size(120, 28);
            cmbTipoOperacion.TabIndex = 7;
            // 
            // labelTarjeta
            // 
            labelTarjeta.AutoSize = true;
            labelTarjeta.Location = new Point(210, 62);
            labelTarjeta.Name = "labelTarjeta";
            labelTarjeta.Size = new Size(56, 20);
            labelTarjeta.TabIndex = 8;
            labelTarjeta.Text = "Tarjeta:";
            // 
            // txtFiltroTarjeta
            // 
            txtFiltroTarjeta.Location = new Point(265, 58);
            txtFiltroTarjeta.Name = "txtFiltroTarjeta";
            txtFiltroTarjeta.Size = new Size(120, 27);
            txtFiltroTarjeta.TabIndex = 9;
            txtFiltroTarjeta.TextChanged += txtFiltroTarjeta_TextChanged;
            // 
            // labelTop
            // 
            labelTop.AutoSize = true;
            labelTop.Location = new Point(10, 100);
            labelTop.Name = "labelTop";
            labelTop.Size = new Size(37, 20);
            labelTop.TabIndex = 10;
            labelTop.Text = "Top:";
            // 
            // cmbTopMode
            // 
            cmbTopMode.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbTopMode.Items.AddRange(new object[] { "Ninguno", "Top 10 posición", "Top 10 monto", "Top 10 cantidad" });
            cmbTopMode.Location = new Point(80, 96);
            cmbTopMode.Name = "cmbTopMode";
            cmbTopMode.Size = new Size(170, 28);
            cmbTopMode.TabIndex = 11;
            // 
            // btnAplicarFiltro
            // 
            btnAplicarFiltro.Location = new Point(265, 136);
            btnAplicarFiltro.Name = "btnAplicarFiltro";
            btnAplicarFiltro.Size = new Size(120, 28);
            btnAplicarFiltro.TabIndex = 12;
            btnAplicarFiltro.Text = "Aplicar Filtro";
            btnAplicarFiltro.UseVisualStyleBackColor = true;
            btnAplicarFiltro.Click += btnAplicarFiltro_Click;
            // 
            // dgvResultados
            // 
            dgvResultados.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvResultados.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvResultados.Location = new Point(12, 420);
            dgvResultados.Name = "dgvResultados";
            dgvResultados.ReadOnly = true;
            dgvResultados.RowHeadersWidth = 51;
            dgvResultados.Size = new Size(1038, 223);
            dgvResultados.TabIndex = 9;
            dgvResultados.SelectionChanged += dgvResultados_SelectionChanged;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(12, 680);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(0, 20);
            lblStatus.TabIndex = 10;
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
            // txtBuscarUsuario
            // 
            txtBuscarUsuario.Location = new Point(181, 38);
            txtBuscarUsuario.Name = "txtBuscarUsuario";
            txtBuscarUsuario.Size = new Size(177, 27);
            txtBuscarUsuario.TabIndex = 3;
            txtBuscarUsuario.TextChanged += txtBuscarUsuario_TextChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(txtBuscarUsuario);
            groupBox1.Controls.Add(btnBuscar);
            groupBox1.Controls.Add(lblInfoUsuario);
            groupBox1.Controls.Add(label3);
            groupBox1.Location = new Point(12, 45);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(450, 150);
            groupBox1.TabIndex = 6;
            groupBox1.TabStop = false;
            // 
            // controlUsuario
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1062, 673);
            Controls.Add(lblStatus);
            Controls.Add(dgvResultados);
            Controls.Add(groupBoxFilters);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Name = "controlUsuario";
            Text = "Consulta de Usuario";
            Load += controlUsuario_Load;
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBoxFilters.ResumeLayout(false);
            groupBoxFilters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvResultados).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private GroupBox groupBox2;
        private Label lblControlDiario;
        // New controls
        private GroupBox groupBoxFilters;
        private Label labelFecha;
        private DateTimePicker dtpFecha;
        private Label labelTipo;
        private ComboBox cmbTipoOperacion;
        private Label labelTarjeta;
        private TextBox txtFiltroTarjeta;
        private Label labelTop;
        private ComboBox cmbTopMode;
        private Button btnAplicarFiltro;
        private DataGridView dgvResultados;
        private Label lblStatus;
        private Label label3;
        private Label lblInfoUsuario;
        private Button btnBuscar;
        private TextBox txtBuscarUsuario;
        private GroupBox groupBox1;
    }
}