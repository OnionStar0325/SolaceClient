namespace SolaceClient
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            panel1 = new Panel();
            btnBrowse = new Button();
            groupBox2 = new GroupBox();
            btnGetInfo = new Button();
            chkListenReply = new CheckBox();
            label2 = new Label();
            label1 = new Label();
            txtReplyQueue = new TextBox();
            txtSendQueue = new TextBox();
            btnClear = new Button();
            groupBox1 = new GroupBox();
            rbtnRequest = new RadioButton();
            rbtnPublish = new RadioButton();
            btnSend = new Button();
            panel2 = new Panel();
            splitContainer1 = new SplitContainer();
            txtContent = new TextBox();
            panel3 = new Panel();
            btnFormat = new Button();
            btnDelEscape = new Button();
            txtReply = new TextBox();
            panel4 = new Panel();
            cmbConnectionList = new ComboBox();
            label3 = new Label();
            panel1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(btnBrowse);
            panel1.Controls.Add(groupBox2);
            panel1.Controls.Add(btnClear);
            panel1.Controls.Add(groupBox1);
            panel1.Controls.Add(btnSend);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 121);
            panel1.TabIndex = 0;
            // 
            // btnBrowse
            // 
            btnBrowse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBrowse.Location = new Point(713, 46);
            btnBrowse.Name = "btnBrowse";
            btnBrowse.Size = new Size(75, 23);
            btnBrowse.TabIndex = 6;
            btnBrowse.Text = "&BROWSE";
            btnBrowse.UseVisualStyleBackColor = true;
            btnBrowse.Click += btnBrowse_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(btnGetInfo);
            groupBox2.Controls.Add(chkListenReply);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(label1);
            groupBox2.Controls.Add(txtReplyQueue);
            groupBox2.Controls.Add(txtSendQueue);
            groupBox2.Location = new Point(158, 11);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(534, 87);
            groupBox2.TabIndex = 3;
            groupBox2.TabStop = false;
            groupBox2.Text = "Queue";
            // 
            // btnGetInfo
            // 
            btnGetInfo.Location = new Point(439, 24);
            btnGetInfo.Name = "btnGetInfo";
            btnGetInfo.Size = new Size(75, 23);
            btnGetInfo.TabIndex = 8;
            btnGetInfo.Text = "&INFO";
            btnGetInfo.UseVisualStyleBackColor = true;
            btnGetInfo.Click += btnGetInfo_Click;
            // 
            // chkListenReply
            // 
            chkListenReply.AutoSize = true;
            chkListenReply.Location = new Point(440, 53);
            chkListenReply.Name = "chkListenReply";
            chkListenReply.Size = new Size(86, 19);
            chkListenReply.TabIndex = 7;
            chkListenReply.Text = "ListenReply";
            chkListenReply.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(20, 51);
            label2.Name = "label2";
            label2.Size = new Size(40, 15);
            label2.TabIndex = 5;
            label2.Text = "REPLY";
            label2.TextAlign = ContentAlignment.TopRight;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(24, 25);
            label1.Name = "label1";
            label1.Size = new Size(38, 15);
            label1.TabIndex = 4;
            label1.Text = "SEND";
            label1.TextAlign = ContentAlignment.TopRight;
            // 
            // txtReplyQueue
            // 
            txtReplyQueue.Location = new Point(69, 51);
            txtReplyQueue.Name = "txtReplyQueue";
            txtReplyQueue.Size = new Size(360, 23);
            txtReplyQueue.TabIndex = 3;
            // 
            // txtSendQueue
            // 
            txtSendQueue.Location = new Point(69, 22);
            txtSendQueue.Name = "txtSendQueue";
            txtSendQueue.Size = new Size(360, 23);
            txtSendQueue.TabIndex = 2;
            // 
            // btnClear
            // 
            btnClear.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClear.Location = new Point(713, 75);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(75, 23);
            btnClear.TabIndex = 7;
            btnClear.Text = "C&LEAR";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(rbtnRequest);
            groupBox1.Controls.Add(rbtnPublish);
            groupBox1.Location = new Point(10, 11);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(143, 87);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Type";
            // 
            // rbtnRequest
            // 
            rbtnRequest.AutoSize = true;
            rbtnRequest.Location = new Point(72, 34);
            rbtnRequest.Name = "rbtnRequest";
            rbtnRequest.Size = new Size(67, 19);
            rbtnRequest.TabIndex = 1;
            rbtnRequest.Text = "Request";
            rbtnRequest.UseVisualStyleBackColor = true;
            // 
            // rbtnPublish
            // 
            rbtnPublish.AutoSize = true;
            rbtnPublish.Checked = true;
            rbtnPublish.Location = new Point(6, 34);
            rbtnPublish.Name = "rbtnPublish";
            rbtnPublish.Size = new Size(64, 19);
            rbtnPublish.TabIndex = 0;
            rbtnPublish.TabStop = true;
            rbtnPublish.Text = "Publish";
            rbtnPublish.UseVisualStyleBackColor = true;
            // 
            // btnSend
            // 
            btnSend.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSend.Location = new Point(713, 19);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(75, 23);
            btnSend.TabIndex = 5;
            btnSend.Text = "&SEND";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(splitContainer1);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 121);
            panel2.Name = "panel2";
            panel2.Size = new Size(800, 329);
            panel2.TabIndex = 1;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(txtContent);
            splitContainer1.Panel1.Controls.Add(panel3);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(txtReply);
            splitContainer1.Panel2.Controls.Add(panel4);
            splitContainer1.Size = new Size(800, 329);
            splitContainer1.SplitterDistance = 192;
            splitContainer1.TabIndex = 0;
            // 
            // txtContent
            // 
            txtContent.Dock = DockStyle.Fill;
            txtContent.Location = new Point(0, 0);
            txtContent.Multiline = true;
            txtContent.Name = "txtContent";
            txtContent.ScrollBars = ScrollBars.Both;
            txtContent.Size = new Size(800, 150);
            txtContent.TabIndex = 8;
            txtContent.KeyUp += txtContent_KeyUp;
            // 
            // panel3
            // 
            panel3.Controls.Add(btnFormat);
            panel3.Controls.Add(btnDelEscape);
            panel3.Dock = DockStyle.Bottom;
            panel3.Location = new Point(0, 150);
            panel3.Name = "panel3";
            panel3.Size = new Size(800, 42);
            panel3.TabIndex = 5;
            // 
            // btnFormat
            // 
            btnFormat.Location = new Point(3, 9);
            btnFormat.Name = "btnFormat";
            btnFormat.Size = new Size(91, 23);
            btnFormat.TabIndex = 7;
            btnFormat.Text = "&FORMAT";
            btnFormat.UseVisualStyleBackColor = true;
            btnFormat.Click += btnFormat_Click;
            // 
            // btnDelEscape
            // 
            btnDelEscape.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDelEscape.Location = new Point(706, 9);
            btnDelEscape.Name = "btnDelEscape";
            btnDelEscape.Size = new Size(91, 23);
            btnDelEscape.TabIndex = 8;
            btnDelEscape.Text = "&DEL ESCAPE";
            btnDelEscape.UseVisualStyleBackColor = true;
            btnDelEscape.Click += btnDelEscape_Click;
            // 
            // txtReply
            // 
            txtReply.BackColor = SystemColors.Window;
            txtReply.Dock = DockStyle.Fill;
            txtReply.Location = new Point(0, 0);
            txtReply.Multiline = true;
            txtReply.Name = "txtReply";
            txtReply.ReadOnly = true;
            txtReply.ScrollBars = ScrollBars.Both;
            txtReply.Size = new Size(800, 91);
            txtReply.TabIndex = 4;
            txtReply.WordWrap = false;
            // 
            // panel4
            // 
            panel4.Controls.Add(cmbConnectionList);
            panel4.Controls.Add(label3);
            panel4.Dock = DockStyle.Bottom;
            panel4.Location = new Point(0, 91);
            panel4.Name = "panel4";
            panel4.Size = new Size(800, 42);
            panel4.TabIndex = 5;
            // 
            // cmbConnectionList
            // 
            cmbConnectionList.FormattingEnabled = true;
            cmbConnectionList.Location = new Point(49, 10);
            cmbConnectionList.Name = "cmbConnectionList";
            cmbConnectionList.Size = new Size(132, 23);
            cmbConnectionList.TabIndex = 1;
            cmbConnectionList.SelectionChangeCommitted += cmbConnectionList_SelectionChangeCommitted;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(9, 13);
            label3.Name = "label3";
            label3.Size = new Size(36, 15);
            label3.TabIndex = 0;
            label3.Text = "Conn";
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Main";
            Text = "Solace Client";
            Load += Main_Load;
            panel1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            panel2.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private RadioButton rbtnPublish;
        private Panel panel2;
        private GroupBox groupBox1;
        private RadioButton rbtnRequest;
        private Button btnSend;
        private Button btnClear;
        private GroupBox groupBox2;
        private TextBox txtSendQueue;
        private SplitContainer splitContainer1;
        private Panel panel3;
        private Button btnFormat;
        private Button btnDelEscape;
        private TextBox txtReply;
        private Panel panel4;
        private TextBox txtReplyQueue;
        private Label label2;
        private Label label1;
        private CheckBox chkListenReply;
        private Button btnGetInfo;
        private Button btnBrowse;
        private TextBox txtContent;
        private ComboBox cmbConnectionList;
        private Label label3;
    }
}
