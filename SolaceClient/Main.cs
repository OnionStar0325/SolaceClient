using CommonUtils;
using Newtonsoft.Json;
using SolaceManagement;
using SolaceRestClient;
using SolaceSystems.Solclient.Messaging;
using System.Security.Permissions;
using System.Text.RegularExpressions;

namespace SolaceClient
{
    public partial class Main : Form
    {
        private QueueProducer QueueProducer { get; set; }
        private ConnectionInfoList ConnInfoList { get; set; }
        private ConnectionInfo CurrentConnInfo { get; set; }

        private List<QueueBrowsePopup> _activePopupList = new List<QueueBrowsePopup>();
        private List<QueueBrowsePopup> ActivePopupList { get { return _activePopupList; } }

        private IContext Context { get; set; }
        private IndecatedList<string> _sentMessages = new IndecatedList<string>();

        public Main()
        {
            InitializeComponent();
        }
        private void InitializeContext()
        {
            // Initialize Solace Systems Messaging API with logging to console at Warning level
            ContextFactoryProperties cfp = new ContextFactoryProperties()
            {
                SolClientLogLevel = SolLogLevel.Warning
            };
            cfp.LogToConsoleError();
            ContextFactory.Instance.Init(cfp);
        }

        private void Main_Load(object sender, EventArgs e)
        {
            InitializeContext();
            Context = ContextFactory.Instance.CreateContext(new ContextProperties(), null);
            ConnInfoList = ConnectionInfoList.LoadConfiguration("connectioninfo.json");

            if (ConnInfoList != null && ConnInfoList.ConnInfoList.Count > 0)
            {
                Connect(ConnInfoList.ConnInfoList.FirstOrDefault());

                int aliasSequence = 1;
                foreach (var connInfo in ConnInfoList.ConnInfoList)
                {
                    if (string.IsNullOrEmpty(connInfo.Alias))
                    {
                        connInfo.Alias = string.Format("Noname-{0}", aliasSequence++);
                    }
                    if (string.IsNullOrEmpty(connInfo.TimeKeyVariable))
                    {
                        connInfo.TimeKeyVariable = ConnInfoList.TimeKeyVariable;
                    }
                    if (connInfo.ExcludePatterns == null || connInfo.ExcludePatterns.Count <= 0 && ConnInfoList.ExcludePatterns != null)
                    {
                        connInfo.ExcludePatterns = ConnInfoList.ExcludePatterns.ToList();
                    }
                }
                cmbConnectionList.DataSource = ConnInfoList.ConnInfoList;
                cmbConnectionList.DisplayMember = "Alias";
            }
        }

        private void AddSendMessage(string message)
        {
            if (_sentMessages.HasItem)
            {
                if (_sentMessages.Current.Equals(message))
                {
                    return;
                }
                else
                {
                    _sentMessages.Add(message);
                }
            }
            else
            {
                _sentMessages.Add(message);
            }
        }

        private void ChangeConnection(ConnectionInfo info)
        {
            while(ActivePopupList.Count > 0)
            {
                var popup = ActivePopupList[0];
                popup.Close();
            }

            Disconnect();
            Connect(info);
        }

        private void Connect(ConnectionInfo info)
        {
            CurrentConnInfo = info;
            QueueProducer = new QueueProducer(info);
            QueueProducer.Run(Context);

            txtSendQueue.Text = CurrentConnInfo.QueueName;
            txtReplyQueue.Text = CurrentConnInfo.ReplyQueueName;
        }

        private void Disconnect()
        {
            if (CurrentConnInfo == null)
            {
                return;
            }

            QueueProducer.Dispose();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            txtReply.Text = "[" + DateTime.Now.ToString("yyyy-M-d HH:mm:ss") + "] ";
            var content = txtContent.Text;

            AddSendMessage(content);

            if (string.IsNullOrEmpty(CurrentConnInfo.TimeKeyVariable) == false)
            {
                content = content.Replace("${" + CurrentConnInfo.TimeKeyVariable + "}", DateTime.Now.ToString("yyyyMMddHHmmssfffffff"));
            }

            if (rbtnPublish.Checked)
            {
                if (chkListenReply.Checked && string.IsNullOrEmpty(txtReplyQueue.Text) == false)
                {
                    txtReply.Text += QueueProducer.SendMessage(txtSendQueue.Text, content, txtReplyQueue.Text);
                }
                else
                {
                    txtReply.Text += QueueProducer.SendMessage(txtSendQueue.Text, content, null);
                }

            }
            else
            {
                txtReply.Text = QueueProducer.RequestMessage(txtSendQueue.Text, content);
            }
            txtReply.Text += Environment.NewLine + "¡å¡å¡å Message Sent ¡å¡å¡å";
            txtReply.Text += Environment.NewLine + content;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtContent.Text = String.Empty;
            txtReply.Text = String.Empty;
        }

        private void btnDelEscape_Click(object sender, EventArgs e)
        {
            string pattern = "\\\\([\"])([\\w\\s\\-_:\\.]*)\\\\([\"])";
            string replacement = "$1$2$3";
            txtContent.Text = Regex.Replace(txtContent.Text, pattern, replacement, RegexOptions.Multiline);

            pattern = "\\\\([\'])([\\w\\s\\-_]*)\\\\([\'])";
            txtContent.Text = Regex.Replace(txtContent.Text, pattern, replacement, RegexOptions.Multiline);

        }

        private void btnFormat_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtContent.Text))
            {
                return;
            }

            string origin = txtContent.Text;

            try
            {
                var jsonObj = JsonConvert.DeserializeObject(txtContent.Text);
                txtContent.Text = JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
            }
            catch
            {
                txtContent.Text = origin;
            }
        }

        private void btnGetInfo_Click(object sender, EventArgs e)
        {
            SmtpRestClient client = new SmtpRestClient(CurrentConnInfo.SEMPHostName + "/SEMP/v2", CurrentConnInfo.UserName, CurrentConnInfo.Password);
            var queueList = client.GetQueueList(CurrentConnInfo.VPNName);

            var popup = new QueueListPopup();
            popup.LoadQueues(queueList.data);
            popup.ShowDialog(this);
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var queueBrowser = new QueueBrowsePopup(Context, CurrentConnInfo);
            queueBrowser.FormClosed += QueueBrowser_FormClosed;
            ActivePopupList.Add(queueBrowser);
            queueBrowser.Show();
        }

        private void QueueBrowser_FormClosed(object? sender, FormClosedEventArgs e)
        {
            if (ActivePopupList.Contains(sender as QueueBrowsePopup))
            {
                ActivePopupList.Remove(sender as QueueBrowsePopup);
            }
        }

        private void txtContent_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Alt == false)
            {
                return;
            }

            if (e.KeyCode == Keys.Up)
            {
                if (_sentMessages.Count == 1)
                {
                    txtContent.Text = _sentMessages.Current;
                }
                else
                {
                    if (_sentMessages.HasPrevious == false)
                    {
                        return;
                    }
                    txtContent.Text = _sentMessages.Previous;
                }
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (_sentMessages.HasNext == false)
                {
                    return;
                }
                txtContent.Text = _sentMessages.Next;
            }
            else
            {
                return;
            }

            txtContent.SelectAll();
        }

        private void cmbConnectionList_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var combo = sender as ComboBox;
            
            if( combo.SelectedItem != CurrentConnInfo)
            {
                ChangeConnection(combo.SelectedItem as ConnectionInfo);
            }
        }
    }
}
