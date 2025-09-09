namespace proyectoCajero
{
    partial class CambiarPinForm
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
            txtPinActual = new TextBox();
            label2 = new Label();
            label3 = new Label();
            txtPinNuevo = new TextBox();
            txtPinConfirmar = new TextBox();
            btnAceptar = new Button();
            btnCancelar = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(199, 100);
            label1.Name = "label1";
            label1.Size = new Size(81, 20);
            label1.TabIndex = 0;
            label1.Text = "PIN Actual:";
            // 
            // txtPinActual
            // 
            txtPinActual.Location = new Point(365, 97);
            txtPinActual.MaxLength = 4;
            txtPinActual.Name = "txtPinActual";
            txtPinActual.PasswordChar = '*';
            txtPinActual.Size = new Size(128, 27);
            txtPinActual.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(199, 154);
            label2.Name = "label2";
            label2.Size = new Size(82, 20);
            label2.TabIndex = 2;
            label2.Text = "Nuevo PIN:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(199, 206);
            label3.Name = "label3";
            label3.Size = new Size(152, 20);
            label3.TabIndex = 3;
            label3.Text = "Confirmar Nuevo PIN:";
            // 
            // txtPinNuevo
            // 
            txtPinNuevo.Location = new Point(368, 151);
            txtPinNuevo.MaxLength = 4;
            txtPinNuevo.Name = "txtPinNuevo";
            txtPinNuevo.PasswordChar = '*';
            txtPinNuevo.Size = new Size(125, 27);
            txtPinNuevo.TabIndex = 4;
            // 
            // txtPinConfirmar
            // 
            txtPinConfirmar.Location = new Point(368, 203);
            txtPinConfirmar.MaxLength = 4;
            txtPinConfirmar.Name = "txtPinConfirmar";
            txtPinConfirmar.PasswordChar = '*';
            txtPinConfirmar.Size = new Size(125, 27);
            txtPinConfirmar.TabIndex = 5;
            // 
            // btnAceptar
            // 
            btnAceptar.Location = new Point(288, 299);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(94, 29);
            btnAceptar.TabIndex = 6;
            btnAceptar.Text = "Aceptar";
            btnAceptar.UseVisualStyleBackColor = true;
            btnAceptar.Click += btnAceptar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.Location = new Point(512, 299);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(94, 29);
            btnCancelar.TabIndex = 7;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // CambiarPinForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnCancelar);
            Controls.Add(btnAceptar);
            Controls.Add(txtPinConfirmar);
            Controls.Add(txtPinNuevo);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(txtPinActual);
            Controls.Add(label1);
            Name = "CambiarPinForm";
            Text = "Cambio de PIN";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtPinActual;
        private Label label2;
        private Label label3;
        private TextBox txtPinNuevo;
        private TextBox txtPinConfirmar;
        private Button btnAceptar;
        private Button btnCancelar;
    }
}