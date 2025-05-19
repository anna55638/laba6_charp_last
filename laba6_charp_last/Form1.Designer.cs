namespace laba6_charp_last
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
            this.components = new System.ComponentModel.Container();
            this.picDisplay = new System.Windows.Forms.PictureBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblScore = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.trackBarSpeed = new System.Windows.Forms.TrackBar();
            this.trackBarFireRate = new System.Windows.Forms.TrackBar();
            this.trackBarHits = new System.Windows.Forms.TrackBar();
            this.labelHits = new System.Windows.Forms.Label();
            this.labelSpeed = new System.Windows.Forms.Label();
            this.labelFireRate = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picDisplay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFireRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarHits)).BeginInit();
            this.SuspendLayout();
            // 
            // picDisplay
            // 
            this.picDisplay.Location = new System.Drawing.Point(12, 12);
            this.picDisplay.Name = "picDisplay";
            this.picDisplay.Size = new System.Drawing.Size(838, 600);
            this.picDisplay.TabIndex = 0;
            this.picDisplay.TabStop = false;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(718, 642);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(112, 33);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Начать игру";
            this.btnStart.UseVisualStyleBackColor = true;
            // 
            // lblScore
            // 
            this.lblScore.AutoSize = true;
            this.lblScore.Location = new System.Drawing.Point(732, 619);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(53, 16);
            this.lblScore.TabIndex = 2;
            this.lblScore.Text = "Очки: 0";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 30;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // trackBarSpeed
            // 
            this.trackBarSpeed.Location = new System.Drawing.Point(12, 619);
            this.trackBarSpeed.Name = "trackBarSpeed";
            this.trackBarSpeed.Size = new System.Drawing.Size(128, 56);
            this.trackBarSpeed.TabIndex = 3;
            // 
            // trackBarFireRate
            // 
            this.trackBarFireRate.Location = new System.Drawing.Point(192, 619);
            this.trackBarFireRate.Name = "trackBarFireRate";
            this.trackBarFireRate.Size = new System.Drawing.Size(131, 56);
            this.trackBarFireRate.TabIndex = 4;
            // 
            // trackBarHits
            // 
            this.trackBarHits.Location = new System.Drawing.Point(382, 619);
            this.trackBarHits.Name = "trackBarHits";
            this.trackBarHits.Size = new System.Drawing.Size(137, 56);
            this.trackBarHits.TabIndex = 5;
            // 
            // labelHits
            // 
            this.labelHits.AutoSize = true;
            this.labelHits.Location = new System.Drawing.Point(395, 600);
            this.labelHits.Name = "labelHits";
            this.labelHits.Size = new System.Drawing.Size(0, 16);
            this.labelHits.TabIndex = 6;
            // 
            // labelSpeed
            // 
            this.labelSpeed.AutoSize = true;
            this.labelSpeed.Location = new System.Drawing.Point(28, 600);
            this.labelSpeed.Name = "labelSpeed";
            this.labelSpeed.Size = new System.Drawing.Size(0, 16);
            this.labelSpeed.TabIndex = 7;
            // 
            // labelFireRate
            // 
            this.labelFireRate.AutoSize = true;
            this.labelFireRate.Location = new System.Drawing.Point(206, 600);
            this.labelFireRate.Name = "labelFireRate";
            this.labelFireRate.Size = new System.Drawing.Size(0, 16);
            this.labelFireRate.TabIndex = 8;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(852, 687);
            this.Controls.Add(this.labelFireRate);
            this.Controls.Add(this.labelSpeed);
            this.Controls.Add(this.labelHits);
            this.Controls.Add(this.trackBarHits);
            this.Controls.Add(this.trackBarFireRate);
            this.Controls.Add(this.trackBarSpeed);
            this.Controls.Add(this.lblScore);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.picDisplay);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.picDisplay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFireRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarHits)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picDisplay;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblScore;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TrackBar trackBarSpeed;
        private System.Windows.Forms.TrackBar trackBarFireRate;
        private System.Windows.Forms.TrackBar trackBarHits;
        private System.Windows.Forms.Label labelHits;
        private System.Windows.Forms.Label labelSpeed;
        private System.Windows.Forms.Label labelFireRate;
    }
}

