
namespace ADW.App
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtAdventure = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCurrentPage = new System.Windows.Forms.TextBox();
            this.txtDGTotal = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pnUnlock = new System.Windows.Forms.Panel();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnUnlock = new System.Windows.Forms.Button();
            this.pnContent = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.pnUnlock.SuspendLayout();
            this.pnContent.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtAdventure);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtCurrentPage);
            this.panel1.Controls.Add(this.txtDGTotal);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.pnUnlock);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(974, 57);
            this.panel1.TabIndex = 0;
            // 
            // txtAdventure
            // 
            this.txtAdventure.Location = new System.Drawing.Point(613, 13);
            this.txtAdventure.Name = "txtAdventure";
            this.txtAdventure.ReadOnly = true;
            this.txtAdventure.Size = new System.Drawing.Size(73, 27);
            this.txtAdventure.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(504, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 20);
            this.label5.TabIndex = 8;
            this.label5.Text = "Adventures :";
            // 
            // txtCurrentPage
            // 
            this.txtCurrentPage.Location = new System.Drawing.Point(340, 12);
            this.txtCurrentPage.Name = "txtCurrentPage";
            this.txtCurrentPage.ReadOnly = true;
            this.txtCurrentPage.Size = new System.Drawing.Size(77, 27);
            this.txtCurrentPage.TabIndex = 7;
            // 
            // txtDGTotal
            // 
            this.txtDGTotal.Location = new System.Drawing.Point(114, 12);
            this.txtDGTotal.Name = "txtDGTotal";
            this.txtDGTotal.ReadOnly = true;
            this.txtDGTotal.Size = new System.Drawing.Size(73, 27);
            this.txtDGTotal.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(245, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 20);
            this.label4.TabIndex = 5;
            this.label4.Text = "CurrentPage :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Dragon Total :";
            // 
            // pnUnlock
            // 
            this.pnUnlock.Controls.Add(this.btnStart);
            this.pnUnlock.Controls.Add(this.btnUnlock);
            this.pnUnlock.Dock = System.Windows.Forms.DockStyle.Right;
            this.pnUnlock.Location = new System.Drawing.Point(703, 0);
            this.pnUnlock.Name = "pnUnlock";
            this.pnUnlock.Size = new System.Drawing.Size(271, 57);
            this.pnUnlock.TabIndex = 3;
            // 
            // btnStart
            // 
            this.btnStart.Enabled = false;
            this.btnStart.Location = new System.Drawing.Point(164, 11);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(94, 29);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnUnlock
            // 
            this.btnUnlock.Location = new System.Drawing.Point(40, 11);
            this.btnUnlock.Name = "btnUnlock";
            this.btnUnlock.Size = new System.Drawing.Size(94, 29);
            this.btnUnlock.TabIndex = 0;
            this.btnUnlock.Text = "Unlock";
            this.btnUnlock.UseVisualStyleBackColor = true;
            this.btnUnlock.Click += new System.EventHandler(this.btnUnlock_Click);
            // 
            // pnContent
            // 
            this.pnContent.Controls.Add(this.panel3);
            this.pnContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnContent.Enabled = false;
            this.pnContent.Location = new System.Drawing.Point(0, 57);
            this.pnContent.Name = "pnContent";
            this.pnContent.Size = new System.Drawing.Size(974, 498);
            this.pnContent.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 49);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(974, 449);
            this.panel3.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(974, 449);
            this.label2.TabIndex = 0;
            this.label2.Text = "label2";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(974, 555);
            this.Controls.Add(this.pnContent);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormMain";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.pnUnlock.ResumeLayout(false);
            this.pnContent.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel pnContent;
        private System.Windows.Forms.Button btnUnlock;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnUnlock;
        private System.Windows.Forms.TextBox txtCurrentPage;
        private System.Windows.Forms.TextBox txtDGTotal;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtAdventure;
        private System.Windows.Forms.Label label5;
    }
}