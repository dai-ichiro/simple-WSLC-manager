namespace WinForm
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.labelCaption = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.checkTerminal = new System.Windows.Forms.CheckBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            //
            // labelCaption
            //
            this.labelCaption.AutoSize = true;
            this.labelCaption.Location = new System.Drawing.Point(20, 30);
            this.labelCaption.Name = "labelCaption";
            this.labelCaption.Text = "コンテナ状態:";
            //
            // labelStatus
            //
            this.labelStatus.AutoSize = true;
            this.labelStatus.ForeColor = System.Drawing.SystemColors.GrayText;
            this.labelStatus.Location = new System.Drawing.Point(110, 30);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Text = "停止中 (Stopped)";
            //
            // checkTerminal
            //
            this.checkTerminal.AutoSize = true;
            this.checkTerminal.Location = new System.Drawing.Point(320, 28);
            this.checkTerminal.Name = "checkTerminal";
            this.checkTerminal.Text = "ターミナルの表示";
            this.checkTerminal.UseVisualStyleBackColor = true;
            //
            // buttonStart
            //
            this.buttonStart.Location = new System.Drawing.Point(20, 65);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(210, 36);
            this.buttonStart.Text = "起動 (Start)";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            //
            // buttonStop
            //
            this.buttonStop.Enabled = false;
            this.buttonStop.Location = new System.Drawing.Point(245, 65);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(200, 36);
            this.buttonStop.Text = "停止 (Stop)";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            //
            // Form1
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(465, 120);
            this.Controls.Add(this.labelCaption);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.checkTerminal);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.buttonStop);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "magnitude";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label labelCaption;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.CheckBox checkTerminal;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Button buttonStop;
    }
}

