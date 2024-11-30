namespace SolaceClient
{
    partial class QueueBrowsePopup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QueueBrowsePopup));
            panel1 = new Panel();
            chkExclude = new CheckBox();
            chkFormatJson = new CheckBox();
            btnListen = new Button();
            txtQueue = new TextBox();
            txtContent = new TextBox();
            panel2 = new Panel();
            btnClear = new Button();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(chkExclude);
            panel1.Controls.Add(chkFormatJson);
            panel1.Controls.Add(btnListen);
            panel1.Controls.Add(txtQueue);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 48);
            panel1.TabIndex = 3;
            // 
            // chkExclude
            // 
            chkExclude.AutoSize = true;
            chkExclude.Checked = true;
            chkExclude.CheckState = CheckState.Checked;
            chkExclude.Location = new Point(591, 15);
            chkExclude.Name = "chkExclude";
            chkExclude.Size = new Size(67, 19);
            chkExclude.TabIndex = 4;
            chkExclude.Text = "Exclude";
            chkExclude.UseVisualStyleBackColor = true;
            // 
            // chkFormatJson
            // 
            chkFormatJson.AutoSize = true;
            chkFormatJson.Location = new Point(503, 15);
            chkFormatJson.Name = "chkFormatJson";
            chkFormatJson.Size = new Size(89, 19);
            chkFormatJson.TabIndex = 3;
            chkFormatJson.Text = "format Json";
            chkFormatJson.UseVisualStyleBackColor = true;
            // 
            // btnListen
            // 
            btnListen.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnListen.Location = new Point(722, 12);
            btnListen.Name = "btnListen";
            btnListen.Size = new Size(75, 23);
            btnListen.TabIndex = 2;
            btnListen.Text = "&LISTEN";
            btnListen.UseVisualStyleBackColor = true;
            btnListen.Click += btnListen_Click;
            // 
            // txtQueue
            // 
            txtQueue.Location = new Point(23, 12);
            txtQueue.Name = "txtQueue";
            txtQueue.Size = new Size(474, 23);
            txtQueue.TabIndex = 1;
            // 
            // txtContent
            // 
            txtContent.Dock = DockStyle.Fill;
            txtContent.Location = new Point(0, 48);
            txtContent.Multiline = true;
            txtContent.Name = "txtContent";
            txtContent.ReadOnly = true;
            txtContent.ScrollBars = ScrollBars.Both;
            txtContent.Size = new Size(800, 366);
            txtContent.TabIndex = 5;
            // 
            // panel2
            // 
            panel2.Controls.Add(btnClear);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(0, 414);
            panel2.Name = "panel2";
            panel2.Size = new Size(800, 36);
            panel2.TabIndex = 6;
            // 
            // btnClear
            // 
            btnClear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClear.Location = new Point(721, 7);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(75, 23);
            btnClear.TabIndex = 0;
            btnClear.Text = "&CLEAR";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // QueueBrowsePopup
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(txtContent);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "QueueBrowsePopup";
            Text = "QueueBrowser";
            FormClosing += QueueBrowsePopup_FormClosing;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Panel panel1;
        private TextBox txtQueue;
        private Button btnListen;
        private TextBox txtContent;
        private CheckBox chkFormatJson;
        private CheckBox chkExclude;
        private Panel panel2;
        private Button btnClear;
    }
}