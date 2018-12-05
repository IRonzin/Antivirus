namespace antivir
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
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
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.bSelectDir = new System.Windows.Forms.Button();
            this.bHeuristic = new System.Windows.Forms.Button();
            this.tbDirectory = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.dgvStatistic = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button3 = new System.Windows.Forms.Button();
            this.bClear = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatistic)).BeginInit();
            this.SuspendLayout();
            // 
            // bSelectDir
            // 
            this.bSelectDir.Location = new System.Drawing.Point(583, 10);
            this.bSelectDir.Name = "bSelectDir";
            this.bSelectDir.Size = new System.Drawing.Size(132, 23);
            this.bSelectDir.TabIndex = 0;
            this.bSelectDir.Text = "Выбрать директорию";
            this.bSelectDir.UseVisualStyleBackColor = true;
            this.bSelectDir.Click += new System.EventHandler(this.bSelectDir_Click);
            // 
            // bHeuristic
            // 
            this.bHeuristic.Location = new System.Drawing.Point(12, 39);
            this.bHeuristic.Name = "bHeuristic";
            this.bHeuristic.Size = new System.Drawing.Size(308, 23);
            this.bHeuristic.TabIndex = 1;
            this.bHeuristic.Text = "Эвристический анализ";
            this.bHeuristic.UseVisualStyleBackColor = true;
            this.bHeuristic.Click += new System.EventHandler(this.bHeuristic_Click);
            // 
            // tbDirectory
            // 
            this.tbDirectory.Location = new System.Drawing.Point(12, 13);
            this.tbDirectory.Name = "tbDirectory";
            this.tbDirectory.Size = new System.Drawing.Size(557, 20);
            this.tbDirectory.TabIndex = 2;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // dgvStatistic
            // 
            this.dgvStatistic.AllowUserToAddRows = false;
            this.dgvStatistic.AllowUserToDeleteRows = false;
            this.dgvStatistic.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvStatistic.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStatistic.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column3,
            this.Column2});
            this.dgvStatistic.Location = new System.Drawing.Point(12, 68);
            this.dgvStatistic.Name = "dgvStatistic";
            this.dgvStatistic.ReadOnly = true;
            this.dgvStatistic.Size = new System.Drawing.Size(703, 223);
            this.dgvStatistic.TabIndex = 4;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Файл";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 200;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Вероятность";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Результат анализа";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 550;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(326, 39);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(308, 23);
            this.button3.TabIndex = 5;
            this.button3.Text = "Сигнатурный анализатор";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.bSignature_Click);
            // 
            // bClear
            // 
            this.bClear.Location = new System.Drawing.Point(640, 39);
            this.bClear.Name = "bClear";
            this.bClear.Size = new System.Drawing.Size(75, 23);
            this.bClear.TabIndex = 6;
            this.bClear.Text = "Очистить";
            this.bClear.UseVisualStyleBackColor = true;
            this.bClear.Click += new System.EventHandler(this.bClear_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(731, 348);
            this.Controls.Add(this.bClear);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.dgvStatistic);
            this.Controls.Add(this.tbDirectory);
            this.Controls.Add(this.bHeuristic);
            this.Controls.Add(this.bSelectDir);
            this.Name = "Form1";
            this.Text = "Antivir";
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatistic)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button bSelectDir;
        private System.Windows.Forms.Button bHeuristic;
        private System.Windows.Forms.TextBox tbDirectory;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.DataGridView dgvStatistic;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button bClear;
    }
}

