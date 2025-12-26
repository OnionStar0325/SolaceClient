using log4net;
using Newtonsoft.Json;
using SolaceManagement;
using SolaceSystems.Solclient.Messaging;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SolaceClient
{
    public partial class QueueBrowsePopup : Form
    {
        static ILog logger = LogManager.GetLogger(typeof(QueueBrowsePopup));
        private int _maximumRowHeight = 50;

        public bool OnBrowsing { get; set; }
        public bool Pause { get; set; }

        IContext context;
        QueueBrowser QueueBrowser;
        ConnectionInfo ConnectionInfo;

        List<Regex> ExcludePatterns = new List<Regex>();

        private List<Tuple<String, String>> _contentBuffer = new List<Tuple<String, String>>();
        private DataTable _contentDataTable;

        public QueueBrowsePopup(IContext context, ConnectionInfo info)
        {
            logger.Info("Initialize Component");
            InitializeComponent();

            this.context = context;
            ConnectionInfo = info;
            ActiveControl = txtQueue;
            if (info.ExcludePatterns != null)
            {
                foreach (string pattern in info.ExcludePatterns)
                {
                    Regex regex = new Regex(@pattern, RegexOptions.Multiline);
                    ExcludePatterns.Add(regex);
                }
            }
        }

        private void StopQueueBrowsing()
        {
            if (QueueBrowser != null)
            {
                QueueBrowser.Dispose();
                OnBrowsing = false;
                txtQueue.ReadOnly = false;
                _contentBuffer.Clear();
            }
        }

        private void ApplyFilter()
        {
            string filterText = txtFilter.Text.Trim();

            if (string.IsNullOrEmpty(filterText))
            {
                _contentDataTable.DefaultView.RowFilter = string.Empty; // 필터 해제
                return;
            }

            // 작은따옴표 이스케이프
            filterText = filterText.Replace("'", "''");

            _contentDataTable.DefaultView.RowFilter =
                $"Log LIKE '%{filterText}%'";
        }

        private void AutoScrollToBottom()
        {
            if (chkAutoScroll.Checked == false)
            {
                return;
            }

            if (grdContent.Rows.Count > 0)
            {
                int lastRow = grdContent.Rows.Count - 1;
                grdContent.FirstDisplayedScrollingRowIndex = lastRow;
            }
        }


        private void QueueBrowsePopup_Load(object sender, EventArgs e)
        {
            _contentDataTable = new DataTable();
            _contentDataTable.Columns.Add("Message", typeof(string));
            _contentDataTable.Columns.Add("Log", typeof(string));

            grdContent.AutoGenerateColumns = true;
            grdContent.DataSource = _contentDataTable.DefaultView;

            grdContent.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grdContent.Columns[0].Width = 240;
            grdContent.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;

            grdContent.AllowUserToAddRows = false;
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            if (OnBrowsing == false)
            {
                if (string.IsNullOrEmpty(txtQueue.Text))
                {
                    throw new Exception("QueueName is not defined");
                }

                ConnectionInfo.QueueName = txtQueue.Text;
                QueueBrowser = new QueueBrowser(ConnectionInfo);
                QueueBrowser.OnMessageReceived += QueueBrowser_OnMessageReceived;

                btnListen.Text = "&Pause";
                Pause = false;

                OnBrowsing = true;
                txtQueue.ReadOnly = true;
                this.Text = "QueueBrowser (" + txtQueue.Text + ")";

                QueueBrowser.Run(context);


                return;
            }

            if (Pause == false)
            {
                btnListen.Text = "&Resume";
                Pause = true;

                return;
            }

            Pause = false;
            btnListen.Text = "&Pause";

            lock (_contentBuffer)
            {
                while (_contentBuffer.Count > 0)
                {
                    _contentDataTable.Rows.Add(_contentBuffer[0].Item1, _contentBuffer[0].Item2);
                    _contentBuffer.RemoveAt(0);
                }
            }
            AutoScrollToBottom();

        }

        private void QueueBrowser_OnMessageReceived(object? sender, BrowserEventArgs e)
        {
            grdContent.Invoke((MethodInvoker)delegate
            {
                if (e.Message != null)
                {
                    string content = SolaceMessageUtil.GetContent(e.Message);

                    if (chkExclude.Checked)
                    {
                        foreach (var pattern in ExcludePatterns)
                        {
                            if (pattern.Match(content).Success)
                            {
                                logger.Debug("Message Skipped - " + e.Message.ADMessageId);
                                return;
                            }
                        }
                    }

                    if (chkFormatJson.Checked)
                    {
                        try
                        {
                            content = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(content), Newtonsoft.Json.Formatting.Indented);
                        }
                        catch { }

                    }
                    var log = new Tuple<string, string>(String.Format("[{0}] Message ID[{1}]", DateTime.Now.ToString("yyyyMMdd HH:mm:ss.fff"), e.Message.ADMessageId), content);
                    logger.Info(String.Format("[{0}] {1}", txtQueue.Text, content));

                    lock (_contentBuffer)
                    {
                        _contentBuffer.Add(log);

                        if (Pause == false)
                        {
                            while (_contentBuffer.Count > 0)
                            {
                                _contentDataTable.Rows.Add(_contentBuffer[0].Item1, _contentBuffer[0].Item2);
                                _contentBuffer.RemoveAt(0);
                            }

                            AutoScrollToBottom();
                        }
                    }
                }
            });
        }

        private void QueueBrowsePopup_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopQueueBrowsing();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            _contentDataTable.Rows.Clear();
        }

        private void chkFilter_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFilter.Checked == false)
            {
                pnlTop.Size = new Size(pnlTop.Width, 48);
                pnlFilter.Visible = false;
                txtFilter.Text = string.Empty;
                ApplyFilter();
            }
            else
            {
                pnlTop.Size = new Size(pnlTop.Width, 90);
                pnlFilter.Visible = true;
                this.ActiveControl = txtFilter;
            }
        }

        private void txtFilter_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }

            ApplyFilter();

        }

        private void chkAutoScroll_CheckedChanged(object sender, EventArgs e)
        {
            AutoScrollToBottom();
        }

        private void grdContent_RowHeightInfoNeeded(object sender, DataGridViewRowHeightInfoNeededEventArgs e)
        {
            if (e.Height > _maximumRowHeight )
            {
                e.Height = _maximumRowHeight;
            }
        }
    }
}
