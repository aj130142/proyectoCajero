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
            buscarUsuariosToolStripMenuItem = new ToolStripMenuItem();
            modificarUsuariosToolStripMenuItem = new ToolStripMenuItem();
            sesionBtn = new Button();
            contAdm = new TextBox();
            admName = new TextBox();
            label2 = new Label();
            label1 = new Label();
            newAdmBtn = new Button();
            activarCajerosToolStripMenuItem = new ToolStripMenuItem();
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
            administarToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { insertarUsuariosToolStripMenuItem, buscarUsuariosToolStripMenuItem, modificarUsuariosToolStripMenuItem, activarCajerosToolStripMenuItem });
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
            // buscarUsuariosToolStripMenuItem
            // 
            buscarUsuariosToolStripMenuItem.Name = "buscarUsuariosToolStripMenuItem";
            buscarUsuariosToolStripMenuItem.Size = new Size(224, 26);
            buscarUsuariosToolStripMenuItem.Text = "Buscar usuarios";
            buscarUsuariosToolStripMenuItem.Click += buscarUsuariosToolStripMenuItem_Click;
            // 
            // modificarUsuariosToolStripMenuItem
            // 
            modificarUsuariosToolStripMenuItem.Name = "modificarUsuariosToolStripMenuItem";
            modificarUsuariosToolStripMenuItem.Size = new Size(224, 26);
            modificarUsuariosToolStripMenuItem.Text = "Modificar Usuarios";
            modificarUsuariosToolStripMenuItem.Click += modificarUsuariosToolStripMenuItem_Click;
            // 
            // sesionBtn
            // 
            sesionBtn.Location = new Point(284, 302);
            sesionBtn.Name = "sesionBtn";
            sesionBtn.Size = new Size(134, 29);
            sesionBtn.TabIndex = 9;
            sesionBtn.Text = "Inicio de sesion";
            sesionBtn.UseVisualStyleBackColor = true;
            sesionBtn.Click += sesionBtn_Click;
            // 
            // contAdm
            // 
            contAdm.Location = new Point(324, 211);
            contAdm.Name = "contAdm";
            contAdm.Size = new Size(177, 27);
            contAdm.TabIndex = 8;
            // 
            // admName
            // 
            admName.Location = new Point(324, 146);
            admName.Name = "admName";
            admName.Size = new Size(177, 27);
            admName.TabIndex = 7;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(198, 218);
            label2.Name = "label2";
            label2.Size = new Size(83, 20);
            label2.TabIndex = 6;
            label2.Text = "Contraseña";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(222, 149);
            label1.Name = "label1";
            label1.Size = new Size(59, 20);
            label1.TabIndex = 5;
            label1.Text = "Usuario";
            // 
            // newAdmBtn
            // 
            newAdmBtn.Location = new Point(546, 302);
            newAdmBtn.Name = "newAdmBtn";
            newAdmBtn.Size = new Size(111, 29);
            newAdmBtn.TabIndex = 10;
            newAdmBtn.Text = "Nuevo admin";
            newAdmBtn.UseVisualStyleBackColor = true;
            newAdmBtn.Click += newAdmBtn_Click;
            // 
            // activarCajerosToolStripMenuItem
            // 
            activarCajerosToolStripMenuItem.Name = "activarCajerosToolStripMenuItem";
            activarCajerosToolStripMenuItem.Size = new Size(224, 26);
            activarCajerosToolStripMenuItem.Text = "Activar cajeros";
            activarCajerosToolStripMenuItem.Click += activarCajerosToolStripMenuItem_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(newAdmBtn);
            Controls.Add(sesionBtn);
            Controls.Add(contAdm);
            Controls.Add(admName);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            FormClosed += Form1_FormClosed;
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
        private ToolStripMenuItem buscarUsuariosToolStripMenuItem;
        private ToolStripMenuItem modificarUsuariosToolStripMenuItem;
        private Button sesionBtn;
        private TextBox contAdm;
        private TextBox admName;
        private Label label2;
        private Label label1;
        private Button newAdmBtn;
        private ToolStripMenuItem activarCajerosToolStripMenuItem;
    }
}
