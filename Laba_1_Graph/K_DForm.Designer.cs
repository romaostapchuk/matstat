namespace Laba_1_Graph
{
    partial class K_DForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(K_DForm));
            this.button3 = new System.Windows.Forms.Button();
            this.відкритиФайлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.відновтиДаніToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.обробкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.стандартзаціяToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.вилученняАномальнихToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(560, 327);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(121, 24);
            this.button3.TabIndex = 24;
            this.button3.Text = "Повернутись";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // відкритиФайлToolStripMenuItem
            // 
            this.відкритиФайлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.відновтиДаніToolStripMenuItem});
            this.відкритиФайлToolStripMenuItem.Name = "відкритиФайлToolStripMenuItem";
            this.відкритиФайлToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.відкритиФайлToolStripMenuItem.Text = "Файл";
            // 
            // відновтиДаніToolStripMenuItem
            // 
            this.відновтиДаніToolStripMenuItem.Name = "відновтиДаніToolStripMenuItem";
            this.відновтиДаніToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.відновтиДаніToolStripMenuItem.Text = "Відновти дані";
            this.відновтиДаніToolStripMenuItem.Click += new System.EventHandler(this.відновтиДаніToolStripMenuItem_Click);
            // 
            // обробкаToolStripMenuItem
            // 
            this.обробкаToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.стандартзаціяToolStripMenuItem});
            this.обробкаToolStripMenuItem.Name = "обробкаToolStripMenuItem";
            this.обробкаToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.обробкаToolStripMenuItem.Text = "Обробка";
            // 
            // стандартзаціяToolStripMenuItem
            // 
            this.стандартзаціяToolStripMenuItem.Name = "стандартзаціяToolStripMenuItem";
            this.стандартзаціяToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.стандартзаціяToolStripMenuItem.Text = "Стандартзація";
            this.стандартзаціяToolStripMenuItem.Click += new System.EventHandler(this.стандартзаціяToolStripMenuItem_Click);
            // 
            // вилученняАномальнихToolStripMenuItem
            // 
            this.вилученняАномальнихToolStripMenuItem.Name = "вилученняАномальнихToolStripMenuItem";
            this.вилученняАномальнихToolStripMenuItem.Size = new System.Drawing.Size(149, 20);
            this.вилученняАномальнихToolStripMenuItem.Text = "Вилучення аномальних";
            this.вилученняАномальнихToolStripMenuItem.Click += new System.EventHandler(this.вилученняАномальнихToolStripMenuItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.відкритиФайлToolStripMenuItem,
            this.обробкаToolStripMenuItem,
            this.вилученняАномальнихToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(693, 24);
            this.menuStrip1.TabIndex = 21;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // dataGridView1
            // 
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.GridColor = System.Drawing.SystemColors.Control;
            this.dataGridView1.Location = new System.Drawing.Point(12, 27);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(669, 294);
            this.dataGridView1.TabIndex = 25;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 324);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 26;
            this.label1.Text = "α = 0.95";
            // 
            // K_DForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(693, 363);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "K_DForm";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ToolStripMenuItem відкритиФайлToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem відновтиДаніToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem обробкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem стандартзаціяToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem вилученняАномальнихToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
    }
}