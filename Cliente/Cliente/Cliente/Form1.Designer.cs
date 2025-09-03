namespace Cliente
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            label2 = new Label();
            button1 = new Button();
            noPinTxt = new TextBox();
            notarjetaTxt = new TextBox();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(180, 142);
            label1.Name = "label1";
            label1.Size = new Size(80, 20);
            label1.TabIndex = 0;
            label1.Text = "No. Tarjeta";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(180, 238);
            label2.Name = "label2";
            label2.Size = new Size(29, 20);
            label2.TabIndex = 1;
            label2.Text = "Pin";
            // 
            // button1
            // 
            button1.Location = new Point(306, 315);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 2;
            button1.Text = "Iniciar";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // noPinTxt
            // 
            noPinTxt.Location = new Point(306, 238);
            noPinTxt.Name = "noPinTxt";
            noPinTxt.Size = new Size(125, 27);
            noPinTxt.TabIndex = 3;
            // 
            // notarjetaTxt
            // 
            notarjetaTxt.Location = new Point(306, 142);
            notarjetaTxt.Name = "notarjetaTxt";
            notarjetaTxt.Size = new Size(125, 27);
            notarjetaTxt.TabIndex = 4;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(notarjetaTxt);
            Controls.Add(noPinTxt);
            Controls.Add(button1);
            Controls.Add(label2);
            Controls.Add(label1);
            Enabled = false;
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            Paint += Form1_Paint;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Button button1;
        private TextBox noPinTxt;
        private TextBox notarjetaTxt;
    }
}
