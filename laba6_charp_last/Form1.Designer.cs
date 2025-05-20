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
            this.trackBarHits = new System.Windows.Forms.TrackBar();
            this.labelHits = new System.Windows.Forms.Label();
            this.labelSpeed = new System.Windows.Forms.Label();
            this.btnMedium = new System.Windows.Forms.Button();
            this.btnEasy = new System.Windows.Forms.Button();
            this.btnHard = new System.Windows.Forms.Button();
            this.btnRestart = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picDisplay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarHits)).BeginInit();
            this.SuspendLayout();
            // 
            // picDisplay
            // 
            this.picDisplay.Location = new System.Drawing.Point(12, 41);
            this.picDisplay.Name = "picDisplay";
            this.picDisplay.Size = new System.Drawing.Size(838, 556);
            this.picDisplay.TabIndex = 0;
            this.picDisplay.TabStop = false;
            this.picDisplay.Click += new System.EventHandler(this.picDisplay_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(696, 642);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(134, 38);
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
            this.trackBarSpeed.Location = new System.Drawing.Point(12, 622);
            this.trackBarSpeed.Name = "trackBarSpeed";
            this.trackBarSpeed.Size = new System.Drawing.Size(128, 56);
            this.trackBarSpeed.TabIndex = 3;
            // 
            // trackBarHits
            // 
            this.trackBarHits.Location = new System.Drawing.Point(163, 624);
            this.trackBarHits.Name = "trackBarHits";
            this.trackBarHits.Size = new System.Drawing.Size(137, 56);
            this.trackBarHits.TabIndex = 5;
            // 
            // labelHits
            // 
            this.labelHits.AutoSize = true;
            this.labelHits.Location = new System.Drawing.Point(183, 604);
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
            // btnMedium
            // 
            this.btnMedium.Location = new System.Drawing.Point(500, 606);
            this.btnMedium.Name = "btnMedium";
            this.btnMedium.Size = new System.Drawing.Size(93, 33);
            this.btnMedium.TabIndex = 9;
            this.btnMedium.Text = "30 ракет";
            this.btnMedium.UseVisualStyleBackColor = true;
            // 
            // btnEasy
            // 
            this.btnEasy.Location = new System.Drawing.Point(406, 606);
            this.btnEasy.Name = "btnEasy";
            this.btnEasy.Size = new System.Drawing.Size(88, 33);
            this.btnEasy.TabIndex = 10;
            this.btnEasy.Text = "20 ракет";
            this.btnEasy.UseVisualStyleBackColor = true;
            // 
            // btnHard
            // 
            this.btnHard.Location = new System.Drawing.Point(454, 643);
            this.btnHard.Name = "btnHard";
            this.btnHard.Size = new System.Drawing.Size(81, 37);
            this.btnHard.TabIndex = 11;
            this.btnHard.Text = "40 ракет";
            this.btnHard.UseVisualStyleBackColor = true;
            // 
            // btnRestart
            // 
            this.btnRestart.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.btnRestart.Location = new System.Drawing.Point(12, 2);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(128, 35);
            this.btnRestart.TabIndex = 12;
            this.btnRestart.Text = "Начать заново";
            this.btnRestart.UseVisualStyleBackColor = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(852, 687);
            this.Controls.Add(this.btnRestart);
            this.Controls.Add(this.btnHard);
            this.Controls.Add(this.btnEasy);
            this.Controls.Add(this.btnMedium);
            this.Controls.Add(this.labelSpeed);
            this.Controls.Add(this.labelHits);
            this.Controls.Add(this.trackBarHits);
            this.Controls.Add(this.trackBarSpeed);
            this.Controls.Add(this.lblScore);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.picDisplay);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.picDisplay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSpeed)).EndInit();
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
        private System.Windows.Forms.TrackBar trackBarHits;
        private System.Windows.Forms.Label labelHits;
        private System.Windows.Forms.Label labelSpeed;
        private System.Windows.Forms.Button btnMedium;
        private System.Windows.Forms.Button btnEasy;
        private System.Windows.Forms.Button btnHard;
        private System.Windows.Forms.Button btnRestart;
    }
}

