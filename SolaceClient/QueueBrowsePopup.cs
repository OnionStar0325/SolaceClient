using log4net;
using Newtonsoft.Json;
using SolaceManagement;
using SolaceSystems.Solclient.Messaging;
using System.Text;
using System.Text.RegularExpressions;

namespace SolaceClient
{
    public partial class QueueBrowsePopup : Form
    {
        static ILog logger = LogManager.GetLogger(typeof(QueueBrowsePopup));

        public bool OnBrowsing { get; set; }
        public bool Pause { get; set; }

        IContext context;
        QueueBrowser QueueBrowser;
        ConnectionInfo ConnectionInfo;

        List<Regex> ExcludePatterns = new List<Regex>();

        private List<string> _contentBuffer = new List<string>();

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

            // Clear previous highlights
            grdContent.SelectAll();
            grdContent.SelectionBackColor = grdContent.BackColor;
            grdContent.DeselectAll();

            if (string.IsNullOrEmpty(filterText))
            {
                return;
            }

            int start = 0;
            while (start < grdContent.Text.Length)
            {
                int wordStartIndex = grdContent.Find(filterText, start, RichTextBoxFinds.None);
                if (wordStartIndex == -1)
                {
                    break;
                }
                grdContent.SelectionStart = wordStartIndex;
                grdContent.SelectionLength = filterText.Length;
                grdContent.SelectionBackColor = Color.Yellow;

                start = wordStartIndex + filterText.Length;
            }
        }

        private void AutoScrollToBottom()
        {
            if (chkAutoScroll.Checked == false)
            {
                return;
            }

            grdContent.SelectionStart = grdContent.Text.Length;
            grdContent.ScrollToCaret();
        }


        private void QueueBrowsePopup_Load(object sender, EventArgs e)
        {
            grdContent.ReadOnly = true;
            grdContent.ScrollBars = RichTextBoxScrollBars.ForcedVertical;
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
                var sb = new StringBuilder();
                while (_contentBuffer.Count > 0)
                {
                    sb.AppendLine(_contentBuffer[0]);
                    _contentBuffer.RemoveAt(0);
                }
                grdContent.AppendText(sb.ToString());
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
                    var log = String.Format("[{0}] Message ID[{1}]\n{2}\n", DateTime.Now.ToString("yyyyMMdd HH:mm:ss.fff"), e.Message.ADMessageId, content);
                    logger.Info(String.Format("[{0}] {1}", txtQueue.Text, content));

                    lock (_contentBuffer)
                    {
                        _contentBuffer.Add(log);

                        if (Pause == false)
                        {
                            var sb = new StringBuilder();
                            while (_contentBuffer.Count > 0)
                            {
                                sb.AppendLine(_contentBuffer[0]);
                                _contentBuffer.RemoveAt(0);
                            }
                            grdContent.AppendText(sb.ToString());

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
            grdContent.Clear();
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
    }
}
