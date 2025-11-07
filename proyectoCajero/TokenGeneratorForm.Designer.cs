namespace proyectoCajero
{
    partial class TokenGeneratorForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblToken;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnRegenerarToken;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            label1 = new Label();
            lblToken = new Label();
            btnAceptar = new Button();
            btnCancelar = new Button();
            btnRegenerarToken = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            label1.Location = new Point(50, 30);
            label1.Name = "label1";
            label1.Size = new Size(251, 28);
            label1.TabIndex = 0;
            label1.Text = "Token de Autenticación";
            // 
            // lblToken
            // 
            lblToken.AutoSize = true;
            lblToken.Font = new Font("Consolas", 36F, FontStyle.Bold);
            lblToken.ForeColor = Color.FromArgb(0, 122, 204);
            lblToken.Location = new Point(70, 80);
            lblToken.Name = "lblToken";
            lblToken.Size = new Size(207, 70);
            lblToken.TabIndex = 1;
            lblToken.Text = "00000";
            // 
            // btnAceptar
            // 
            btnAceptar.BackColor = Color.FromArgb(0, 122, 204);
            btnAceptar.FlatStyle = FlatStyle.Flat;
            btnAceptar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnAceptar.ForeColor = Color.White;
            btnAceptar.Location = new Point(50, 220);
            btnAceptar.Name = "btnAceptar";
            btnAceptar.Size = new Size(120, 40);
            btnAceptar.TabIndex = 2;
            btnAceptar.Text = "Aceptar";
            btnAceptar.UseVisualStyleBackColor = false;
            btnAceptar.Click += btnAceptar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = Color.FromArgb(192, 0, 0);
            btnCancelar.FlatStyle = FlatStyle.Flat;
            btnCancelar.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(180, 220);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(120, 40);
            btnCancelar.TabIndex = 3;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // btnRegenerarToken
            // 
            btnRegenerarToken.BackColor = Color.FromArgb(255, 152, 0);
            btnRegenerarToken.FlatStyle = FlatStyle.Flat;
            btnRegenerarToken.Font = new Font("Segoe UI", 9F);
            btnRegenerarToken.ForeColor = Color.White;
            btnRegenerarToken.Location = new Point(90, 165);
            btnRegenerarToken.Name = "btnRegenerarToken";
            btnRegenerarToken.Size = new Size(160, 35);
            btnRegenerarToken.TabIndex = 4;
            btnRegenerarToken.Text = "🔄 Regenerar Token";
            btnRegenerarToken.UseVisualStyleBackColor = false;
            btnRegenerarToken.Click += btnRegenerarToken_Click;
            // 
            // TokenGeneratorForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(350, 290);
            Controls.Add(btnRegenerarToken);
            Controls.Add(btnCancelar);
            Controls.Add(btnAceptar);
            Controls.Add(lblToken);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "TokenGeneratorForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Generar Token";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
