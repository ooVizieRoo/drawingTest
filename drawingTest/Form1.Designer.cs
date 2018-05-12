namespace drawingTest
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnTopologyAdd = new System.Windows.Forms.Button();
            this.btnTopologyAddDependency = new System.Windows.Forms.Button();
            this.btnTopologySelect = new System.Windows.Forms.Button();
            this.btnTopologyDelete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Location = new System.Drawing.Point(12, 69);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(823, 355);
            this.panel1.TabIndex = 0;
            this.panel1.Click += new System.EventHandler(this.panel1_Click);
            // 
            // btnTopologyAdd
            // 
            this.btnTopologyAdd.Location = new System.Drawing.Point(196, 13);
            this.btnTopologyAdd.Name = "btnTopologyAdd";
            this.btnTopologyAdd.Size = new System.Drawing.Size(89, 38);
            this.btnTopologyAdd.TabIndex = 1;
            this.btnTopologyAdd.Text = "Add node";
            this.btnTopologyAdd.UseVisualStyleBackColor = true;
            this.btnTopologyAdd.Click += new System.EventHandler(this.btnTopologyAdd_Click);
            // 
            // btnTopologyAddDependency
            // 
            this.btnTopologyAddDependency.Location = new System.Drawing.Point(291, 13);
            this.btnTopologyAddDependency.Name = "btnTopologyAddDependency";
            this.btnTopologyAddDependency.Size = new System.Drawing.Size(90, 38);
            this.btnTopologyAddDependency.TabIndex = 2;
            this.btnTopologyAddDependency.Text = "Add dependency";
            this.btnTopologyAddDependency.UseVisualStyleBackColor = true;
            this.btnTopologyAddDependency.Click += new System.EventHandler(this.btnTopologyAddDependency_Click);
            // 
            // btnTopologySelect
            // 
            this.btnTopologySelect.Location = new System.Drawing.Point(93, 13);
            this.btnTopologySelect.Name = "btnTopologySelect";
            this.btnTopologySelect.Size = new System.Drawing.Size(97, 38);
            this.btnTopologySelect.TabIndex = 3;
            this.btnTopologySelect.Text = "Select";
            this.btnTopologySelect.UseVisualStyleBackColor = true;
            this.btnTopologySelect.Click += new System.EventHandler(this.btnTopologySelect_Click);
            // 
            // btnTopologyDelete
            // 
            this.btnTopologyDelete.Location = new System.Drawing.Point(387, 13);
            this.btnTopologyDelete.Name = "btnTopologyDelete";
            this.btnTopologyDelete.Size = new System.Drawing.Size(91, 38);
            this.btnTopologyDelete.TabIndex = 4;
            this.btnTopologyDelete.Text = "Delete";
            this.btnTopologyDelete.UseVisualStyleBackColor = true;
            this.btnTopologyDelete.Click += new System.EventHandler(this.btnTopologyDelete_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(847, 436);
            this.Controls.Add(this.btnTopologyDelete);
            this.Controls.Add(this.btnTopologySelect);
            this.Controls.Add(this.btnTopologyAddDependency);
            this.Controls.Add(this.btnTopologyAdd);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnTopologyAdd;
        private System.Windows.Forms.Button btnTopologyAddDependency;
        private System.Windows.Forms.Button btnTopologySelect;
        private System.Windows.Forms.Button btnTopologyDelete;
    }
}

