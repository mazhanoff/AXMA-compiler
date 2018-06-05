namespace AXMA_compiler
{
    partial class Compiler
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.openFileBtn = new System.Windows.Forms.Button();
            this.logsRTB = new System.Windows.Forms.RichTextBox();
            this.playBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // openFileBtn
            // 
            this.openFileBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.openFileBtn.Location = new System.Drawing.Point(678, 403);
            this.openFileBtn.Name = "openFileBtn";
            this.openFileBtn.Size = new System.Drawing.Size(110, 35);
            this.openFileBtn.TabIndex = 0;
            this.openFileBtn.Text = "Выбрать файл";
            this.openFileBtn.UseVisualStyleBackColor = true;
            this.openFileBtn.Click += new System.EventHandler(this.openFileBtn_Click);
            // 
            // logsRTB
            // 
            this.logsRTB.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logsRTB.Location = new System.Drawing.Point(12, 12);
            this.logsRTB.Name = "logsRTB";
            this.logsRTB.Size = new System.Drawing.Size(776, 385);
            this.logsRTB.TabIndex = 1;
            this.logsRTB.Text = "";
            // 
            // playBtn
            // 
            this.playBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.playBtn.Location = new System.Drawing.Point(551, 403);
            this.playBtn.Name = "playBtn";
            this.playBtn.Size = new System.Drawing.Size(110, 35);
            this.playBtn.TabIndex = 2;
            this.playBtn.Text = "Запустить";
            this.playBtn.UseVisualStyleBackColor = true;
            this.playBtn.Click += new System.EventHandler(this.playBtn_Click);
            // 
            // Compiler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.playBtn);
            this.Controls.Add(this.logsRTB);
            this.Controls.Add(this.openFileBtn);
            this.Name = "Compiler";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button openFileBtn;
        private System.Windows.Forms.RichTextBox logsRTB;
        private System.Windows.Forms.Button playBtn;
    }
}

