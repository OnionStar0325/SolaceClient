namespace SolaceClient
{
    partial class QueueListPopup
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(QueueListPopup));
            txtSearch = new TextBox();
            panel1 = new Panel();
            panel2 = new Panel();
            dataGridView1 = new DataGridView();
            datumBindingSource = new BindingSource(components);
            queueNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            accessTypeDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            permissionDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)datumBindingSource).BeginInit();
            SuspendLayout();
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(23, 12);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(623, 23);
            txtSearch.TabIndex = 0;
            txtSearch.TextChanged += txtSearch_TextChanged;
            // 
            // panel1
            // 
            panel1.Controls.Add(txtSearch);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 48);
            panel1.TabIndex = 1;
            // 
            // panel2
            // 
            panel2.Controls.Add(dataGridView1);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 48);
            panel2.Name = "panel2";
            panel2.Size = new Size(800, 402);
            panel2.TabIndex = 2;
            // 
            // dataGridView1
            // 
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { queueNameDataGridViewTextBoxColumn, accessTypeDataGridViewTextBoxColumn, permissionDataGridViewTextBoxColumn });
            dataGridView1.DataSource = datumBindingSource;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.GridColor = Color.DimGray;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new Size(800, 402);
            dataGridView1.TabIndex = 0;
            // 
            // datumBindingSource
            // 
            datumBindingSource.DataSource = typeof(SolaceRestClient.Json.Datum);
            // 
            // queueNameDataGridViewTextBoxColumn
            // 
            queueNameDataGridViewTextBoxColumn.DataPropertyName = "queueName";
            queueNameDataGridViewTextBoxColumn.HeaderText = "QUEUE NAME";
            queueNameDataGridViewTextBoxColumn.Name = "queueNameDataGridViewTextBoxColumn";
            queueNameDataGridViewTextBoxColumn.ReadOnly = true;
            queueNameDataGridViewTextBoxColumn.Width = 107;
            // 
            // accessTypeDataGridViewTextBoxColumn
            // 
            accessTypeDataGridViewTextBoxColumn.DataPropertyName = "accessType";
            accessTypeDataGridViewTextBoxColumn.HeaderText = "ACCESS TYPE";
            accessTypeDataGridViewTextBoxColumn.Name = "accessTypeDataGridViewTextBoxColumn";
            accessTypeDataGridViewTextBoxColumn.ReadOnly = true;
            accessTypeDataGridViewTextBoxColumn.Width = 106;
            // 
            // permissionDataGridViewTextBoxColumn
            // 
            permissionDataGridViewTextBoxColumn.DataPropertyName = "permission";
            permissionDataGridViewTextBoxColumn.HeaderText = "PERMISSION";
            permissionDataGridViewTextBoxColumn.Name = "permissionDataGridViewTextBoxColumn";
            permissionDataGridViewTextBoxColumn.ReadOnly = true;
            permissionDataGridViewTextBoxColumn.Width = 101;
            // 
            // QueueListPopup
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimizeBox = false;
            Name = "QueueListPopup";
            Text = "Queues";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)datumBindingSource).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TextBox txtSearch;
        private Panel panel1;
        private Panel panel2;
        private DataGridView dataGridView1;
        private BindingSource datumBindingSource;
        private DataGridViewTextBoxColumn queueNameDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn accessTypeDataGridViewTextBoxColumn;
        private DataGridViewTextBoxColumn permissionDataGridViewTextBoxColumn;
    }
}