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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QueueBrowsePopup));
            pnlTop = new Panel();
            chkAutoScroll = new CheckBox();
            chkFilter = new CheckBox();
            chkExclude = new CheckBox();
            chkFormatJson = new CheckBox();
            btnListen = new Button();
            txtQueue = new TextBox();
            pnlFilter = new Panel();
            txtFilter = new TextBox();
            panel2 = new Panel();
            btnClear = new Button();
            grdContent = new DataGridView();
            pnlTop.SuspendLayout();
            pnlFilter.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)grdContent).BeginInit();
            SuspendLayout();
            // 
            // pnlTop
            // 
            pnlTop.Controls.Add(chkAutoScroll);
            pnlTop.Controls.Add(chkFilter);
            pnlTop.Controls.Add(chkExclude);
            pnlTop.Controls.Add(chkFormatJson);
            pnlTop.Controls.Add(btnListen);
            pnlTop.Controls.Add(txtQueue);
            pnlTop.Controls.Add(pnlFilter);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 0);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new Size(800, 48);
            pnlTop.TabIndex = 3;
            // 
            // chkAutoScroll
            // 
            chkAutoScroll.AutoSize = true;
            chkAutoScroll.Checked = true;
            chkAutoScroll.CheckState = CheckState.Checked;
            chkAutoScroll.Location = new Point(598, 14);
            chkAutoScroll.Name = "chkAutoScroll";
            chkAutoScroll.Size = new Size(56, 19);
            chkAutoScroll.TabIndex = 5;
            chkAutoScroll.Text = "Scroll";
            chkAutoScroll.UseVisualStyleBackColor = true;
            chkAutoScroll.CheckedChanged += chkAutoScroll_CheckedChanged;
            // 
            // chkFilter
            // 
            chkFilter.AutoSize = true;
            chkFilter.Location = new Point(656, 14);
            chkFilter.Name = "chkFilter";
            chkFilter.Size = new Size(52, 19);
            chkFilter.TabIndex = 6;
            chkFilter.Text = "Filter";
            chkFilter.UseVisualStyleBackColor = true;
            chkFilter.CheckedChanged += chkFilter_CheckedChanged;
            // 
            // chkExclude
            // 
            chkExclude.AutoSize = true;
            chkExclude.Checked = true;
            chkExclude.CheckState = CheckState.Checked;
            chkExclude.Location = new Point(533, 14);
            chkExclude.Name = "chkExclude";
            chkExclude.Size = new Size(67, 19);
            chkExclude.TabIndex = 4;
            chkExclude.Text = "Exclude";
            chkExclude.UseVisualStyleBackColor = true;
            // 
            // chkFormatJson
            // 
            chkFormatJson.AutoSize = true;
            chkFormatJson.Location = new Point(445, 14);
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
            txtQueue.Size = new Size(416, 23);
            txtQueue.TabIndex = 1;
            // 
            // pnlFilter
            // 
            pnlFilter.Controls.Add(txtFilter);
            pnlFilter.Location = new Point(23, 41);
            pnlFilter.Name = "pnlFilter";
            pnlFilter.Size = new Size(635, 49);
            pnlFilter.TabIndex = 8;
            pnlFilter.Visible = false;
            // 
            // txtFilter
            // 
            txtFilter.Location = new Point(3, 11);
            txtFilter.Name = "txtFilter";
            txtFilter.Size = new Size(413, 23);
            txtFilter.TabIndex = 7;
            txtFilter.KeyUp += txtFilter_KeyUp;
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
            // grdContent
            // 
            grdContent.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grdContent.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            grdContent.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grdContent.ColumnHeadersVisible = false;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("맑은 고딕", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            grdContent.DefaultCellStyle = dataGridViewCellStyle1;
            grdContent.Dock = DockStyle.Fill;
            grdContent.Location = new Point(0, 48);
            grdContent.Name = "grdContent";
            grdContent.ReadOnly = true;
            grdContent.RowHeadersVisible = false;
            grdContent.RowTemplate.Height = 25;
            grdContent.Size = new Size(800, 366);
            grdContent.TabIndex = 8;
            grdContent.RowHeightInfoNeeded += grdContent_RowHeightInfoNeeded;
            // 
            // QueueBrowsePopup
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(grdContent);
            Controls.Add(panel2);
            Controls.Add(pnlTop);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "QueueBrowsePopup";
            Text = "QueueBrowser";
            FormClosing += QueueBrowsePopup_FormClosing;
            Load += QueueBrowsePopup_Load;
            pnlTop.ResumeLayout(false);
            pnlTop.PerformLayout();
            pnlFilter.ResumeLayout(false);
            pnlFilter.PerformLayout();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)grdContent).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Panel pnlTop;
        private TextBox txtQueue;
        private Button btnListen;
        private CheckBox chkFormatJson;
        private CheckBox chkExclude;
        private Panel panel2;
        private Button btnClear;
        private CheckBox chkFilter;
        private Panel pnlFilter;
        private TextBox txtFilter;
        private DataGridView grdContent;
        private CheckBox chkAutoScroll;
    }
}