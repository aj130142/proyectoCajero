namespace proyectoCajero
{
    partial class MenuUsuario
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
            lblBienvenida = new Label();
            groupBox1 = new GroupBox();
            btnCambiarPin = new Button();
            btnVerSaldo = new Button();
            btnUltimasTransacciones = new Button();
            btnDeposito = new Button();
            btnRetiro = new Button();
            btnSalir = new Button();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // lblBienvenida
            // 
            lblBienvenida.AutoSize = true;
            lblBienvenida.Font = new Font("Segoe UI", 16F);
            lblBienvenida.Location = new Point(325, 9);
            lblBienvenida.Name = "lblBienvenida";
            lblBienvenida.Size = new Size(147, 37);
            lblBienvenida.TabIndex = 0;
            lblBienvenida.Text = "Bienvenida";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnCambiarPin);
            groupBox1.Controls.Add(btnVerSaldo);
            groupBox1.Controls.Add(btnUltimasTransacciones);
            groupBox1.Controls.Add(btnDeposito);
            groupBox1.Controls.Add(btnRetiro);
            groupBox1.Location = new Point(12, 49);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(776, 304);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Operaciones Disponibles";
            // 
            // btnCambiarPin
            // 
            btnCambiarPin.Location = new Point(516, 26);
            btnCambiarPin.Name = "btnCambiarPin";
            btnCambiarPin.Size = new Size(128, 64);
            btnCambiarPin.TabIndex = 4;
            btnCambiarPin.Text = "Cambiar PIN";
            btnCambiarPin.UseVisualStyleBackColor = true;
            btnCambiarPin.Click += btnCambiarPin_Click;
            // 
            // btnVerSaldo
            // 
            btnVerSaldo.Location = new Point(97, 166);
            btnVerSaldo.Name = "btnVerSaldo";
            btnVerSaldo.Size = new Size(128, 64);
            btnVerSaldo.TabIndex = 3;
            btnVerSaldo.Text = "Consultar Saldo";
            btnVerSaldo.UseVisualStyleBackColor = true;
            btnVerSaldo.Click += btnVerSaldo_Click;
            // 
            // btnUltimasTransacciones
            // 
            btnUltimasTransacciones.Location = new Point(516, 96);
            btnUltimasTransacciones.Name = "btnUltimasTransacciones";
            btnUltimasTransacciones.Size = new Size(128, 64);
            btnUltimasTransacciones.TabIndex = 2;
            btnUltimasTransacciones.Text = "Ver Últimas Transacciones";
            btnUltimasTransacciones.UseVisualStyleBackColor = true;
            btnUltimasTransacciones.Click += btnUltimasTransacciones_Click;
            // 
            // btnDeposito
            // 
            btnDeposito.Location = new Point(97, 96);
            btnDeposito.Name = "btnDeposito";
            btnDeposito.Size = new Size(128, 64);
            btnDeposito.TabIndex = 1;
            btnDeposito.Text = "Realizar Depósito";
            btnDeposito.UseVisualStyleBackColor = true;
            btnDeposito.Click += btnDeposito_Click;
            // 
            // btnRetiro
            // 
            btnRetiro.Location = new Point(97, 26);
            btnRetiro.Name = "btnRetiro";
            btnRetiro.Size = new Size(128, 64);
            btnRetiro.TabIndex = 0;
            btnRetiro.Text = "Realizar Retiro";
            btnRetiro.UseVisualStyleBackColor = true;
            btnRetiro.Click += btnRetiro_Click;
            // 
            // btnSalir
            // 
            btnSalir.Location = new Point(660, 374);
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new Size(128, 64);
            btnSalir.TabIndex = 5;
            btnSalir.Text = "Salir";
            btnSalir.UseVisualStyleBackColor = true;
            btnSalir.Click += btnSalir_Click;
            // 
            // MenuUsuario
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnSalir);
            Controls.Add(groupBox1);
            Controls.Add(lblBienvenida);
            Name = "MenuUsuario";
            Text = "Menú Principal";
            Load += MenuUsuario_Load;
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblBienvenida;
        private GroupBox groupBox1;
        private Button btnCambiarPin;
        private Button btnVerSaldo;
        private Button btnUltimasTransacciones;
        private Button btnDeposito;
        private Button btnRetiro;
        private Button btnSalir;
    }
}