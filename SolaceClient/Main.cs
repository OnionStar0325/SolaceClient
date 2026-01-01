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
            AliasList.LoadAliases();

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

        private void ExpandAlias(Keys lastKey)
        {
            try
            {
                int cursorPosition = txtContent.SelectionStart;
                string text = txtContent.Text;

                // 1. Identify where the word ends. 
                // We assume the cursor is AFTER the delimiter (Space/Enter).
                // So search backwards from cursorPosition - 1 for the first non-whitespace character.

                int i = cursorPosition - 1;

                // Skip immediate trailing whitespace (the trigger(s))
                while (i >= 0 && i < text.Length && char.IsWhiteSpace(text[i]))
                {
                    i--;
                }

                // If i < 0, we only have whitespace or empty string before cursor
                if (i < 0) return;

                int endWordIndex = i;

                // 2. Find the start of the word
                while (i >= 0 && !char.IsWhiteSpace(text[i]))
                {
                    i--;
                }

                int startWordIndex = i + 1;
                int length = endWordIndex - startWordIndex + 1;

                if (length <= 0) return;

                string word = text.Substring(startWordIndex, length);

                // 3. Check for alias match
                var match = AliasList.Aliases.FirstOrDefault(a => a.Alias == word);
                if (match != null)
                {
                    // 4. Replace using explicit indices to avoid selection issues
                    // Select the word range
                    txtContent.Select(startWordIndex, length);

                    // Replace with keyword
                    txtContent.SelectedText = match.Keyword.Replace("\r\n", "\n")
                                                           .Replace("\r", "\n")
                                                           .Replace("\n", "\r\n"); ;

                    // 5. Restore cursor / handle trailing space
                    // SelectedText places cursor at the end of the inserted text.
                    // But we still have the trailing whitespace (Space/Enter) that triggered this.
                    // The text content shifted, but the trailing whitespace should remain "after" the insertion 
                    // if we only replaced the word.
                    // However, we need to ensure the cursor is placed AFTER the trailing whitespace 
                    // so typing continues normally.

                    // Calculate where the cursor implies it should be:
                    // new cursor = startWordIndex + match.Keyword.Length + (original_cursor - (endWordIndex + 1))

                    int trailingWhitespaceLen = cursorPosition - (endWordIndex + 1);
                    txtContent.SelectionStart = startWordIndex + match.Keyword.Length + trailingWhitespaceLen;
                    txtContent.SelectionLength = 0;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error expanding alias: {ex.Message}");
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
            if (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter)
            {
                ExpandAlias(e.KeyCode);
                return;
            }

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
