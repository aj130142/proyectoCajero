namespace proyectoCajero
{
    partial class agregarAdmin
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
            guardarBtn = new Button();
            nameTxt = new TextBox();
            passTxt = new TextBox();
            label1 = new Label();
            label2 = new Label();
            SuspendLayout();
            // 
            // guardarBtn
            // 
            guardarBtn.Location = new Point(207, 283);
            guardarBtn.Name = "guardarBtn";
            guardarBtn.Size = new Size(94, 29);
            guardarBtn.TabIndex = 0;
            guardarBtn.Text = "Guardar";
            guardarBtn.UseVisualStyleBackColor = true;
            guardarBtn.Click += guardarBtn_Click;
            // 
            // nameTxt
            // 
            nameTxt.Location = new Point(193, 97);
            nameTxt.Name = "nameTxt";
            nameTxt.Size = new Size(125, 27);
            nameTxt.TabIndex = 1;
            // 
            // passTxt
            // 
            passTxt.Location = new Point(193, 181);
            passTxt.Name = "passTxt";
            passTxt.Size = new Size(125, 27);
            passTxt.TabIndex = 2;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(54, 100);
            label1.Name = "label1";
            label1.Size = new Size(64, 20);
            label1.TabIndex = 3;
            label1.Text = "Nombre";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(54, 188);
            label2.Name = "label2";
            label2.Size = new Size(83, 20);
            label2.TabIndex = 4;
            label2.Text = "Contraseña";
            // 
            // agregarAdmin
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(473, 385);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(passTxt);
            Controls.Add(nameTxt);
            Controls.Add(guardarBtn);
            Name = "agregarAdmin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "agregarAdmin";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button guardarBtn;
        private TextBox nameTxt;
        private TextBox passTxt;
        private Label label1;
        private Label label2;
    }
}