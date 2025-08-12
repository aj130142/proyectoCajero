namespace proyectoCajero
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
            menuStrip1 = new MenuStrip();
            administarToolStripMenuItem = new ToolStripMenuItem();
            insertarUsuariosToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { administarToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 28);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            // 
            // administarToolStripMenuItem
            // 
            administarToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { insertarUsuariosToolStripMenuItem });
            administarToolStripMenuItem.Name = "administarToolStripMenuItem";
            administarToolStripMenuItem.Size = new Size(95, 24);
            administarToolStripMenuItem.Text = "Administar";
            // 
            // insertarUsuariosToolStripMenuItem
            // 
            insertarUsuariosToolStripMenuItem.Name = "insertarUsuariosToolStripMenuItem";
            insertarUsuariosToolStripMenuItem.Size = new Size(224, 26);
            insertarUsuariosToolStripMenuItem.Text = "Insertar usuarios";
            insertarUsuariosToolStripMenuItem.Click += insertarUsuariosToolStripMenuItem_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem administarToolStripMenuItem;
        private ToolStripMenuItem insertarUsuariosToolStripMenuItem;
    }
}
