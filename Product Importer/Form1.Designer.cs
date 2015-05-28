namespace Product_Importer
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.imgFold = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.sourceFile = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.button4 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(67, 501);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(224, 38);
            this.button1.TabIndex = 0;
            this.button1.Text = "生成家家优品导入文件";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "XLSX (*.xlsx)|*.xlsx|CSV File (*.csv)|*.csv|All files (*.*)|*.*";
            // 
            // imgFold
            // 
            this.imgFold.Location = new System.Drawing.Point(145, 67);
            this.imgFold.Name = "imgFold";
            this.imgFold.Size = new System.Drawing.Size(394, 20);
            this.imgFold.TabIndex = 1;
            this.imgFold.Text = "E:\\BaiduYunDownload\\20150521";
            this.imgFold.MouseDown += new System.Windows.Forms.MouseEventHandler(this.imgFold_MouseDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(52, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "图片文件夹";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(64, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "产品文件";
            // 
            // sourceFile
            // 
            this.sourceFile.Location = new System.Drawing.Point(145, 19);
            this.sourceFile.Name = "sourceFile";
            this.sourceFile.Size = new System.Drawing.Size(394, 20);
            this.sourceFile.TabIndex = 3;
            this.sourceFile.Text = "E:\\jiajiayoupin";
            this.sourceFile.MouseDown += new System.Windows.Forms.MouseEventHandler(this.sourceFile_MouseDown);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(329, 501);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(224, 38);
            this.button2.TabIndex = 5;
            this.button2.Text = "生成淘宝图片";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.Location = new System.Drawing.Point(145, 111);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(394, 20);
            this.txtOutput.TabIndex = 6;
            this.txtOutput.Text = "D:\\output";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(40, 114);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "输出文件路径";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(67, 446);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(224, 38);
            this.button3.TabIndex = 8;
            this.button3.Text = "网络抓取内容图片";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Location = new System.Drawing.Point(41, 152);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(498, 277);
            this.webBrowser1.TabIndex = 9;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(329, 446);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(224, 38);
            this.button4.TabIndex = 10;
            this.button4.Text = "网络抓取内容图片";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 588);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtOutput);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.sourceFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.imgFold);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox imgFold;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox sourceFile;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.Button button4;
    }
}

