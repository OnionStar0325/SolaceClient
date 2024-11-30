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

        private StringBuilder contentBuffer = new StringBuilder();

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
                contentBuffer.Clear();
            }
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            if (OnBrowsing && Pause == false)
            {
                btnListen.Text = "&Resume";
                Pause = true;
                return;
            }

            if (OnBrowsing == false)
            {
                if (string.IsNullOrEmpty(txtQueue.Text))
                {
                    throw new Exception("QueueName is not defined");
                }

                txtContent.Text = string.Empty;
                ConnectionInfo.QueueName = txtQueue.Text;
                QueueBrowser = new QueueBrowser(ConnectionInfo);
                QueueBrowser.OnMessageReceived += QueueBrowser_OnMessageReceived;
                QueueBrowser.Run(context);

                btnListen.Text = "&Pause";
                OnBrowsing = true;
                txtQueue.ReadOnly = true;
                this.Text = "QueueBrowser (" + txtQueue.Text + ")";
            }
            else
            {
                Pause = false;
                btnListen.Text = "&Pause";

                lock (contentBuffer)
                {
                    txtContent.AppendText(contentBuffer.ToString());
                    contentBuffer.Clear();
                }
            }

        }

        private void QueueBrowser_OnMessageReceived(object? sender, BrowserEventArgs e)
        {
            txtContent.Invoke((MethodInvoker)delegate
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
                            content = Environment.NewLine + JsonConvert.SerializeObject(JsonConvert.DeserializeObject(content), Newtonsoft.Json.Formatting.Indented);
                        }
                        catch { }

                    }
                    var log = String.Format("[{0}] Message ID[{1}] {2} ", DateTime.Now.ToString("yyyyMMdd HH:mm:ss.fff"), e.Message.ADMessageId, content) + Environment.NewLine + Environment.NewLine;

                    logger.Info(String.Format("[{0}] {1}", txtQueue.Text, content));

                    lock (contentBuffer)
                    {
                        contentBuffer.Append(log);

                        if (Pause == false)
                        {
                            txtContent.AppendText(contentBuffer.ToString());
                            contentBuffer.Clear();
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
            txtContent.Clear();
        }
    }
}
