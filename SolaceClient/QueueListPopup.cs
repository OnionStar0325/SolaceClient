using FastMember;
using SolaceRestClient.Json;
using System.Data;
using System.Text.RegularExpressions;

namespace SolaceClient
{
    public partial class QueueListPopup : Form
    {
        public QueueListPopup()
        {
            InitializeComponent();
        }

        public void LoadQueues(List<Datum> queues)
        {
            if (queues == null)
            {
                return;
            }
            //datumBindingSource.DataSource = queues;
            DataTable table = new DataTable();
            using var reader = ObjectReader.Create(queues);
            table.Load(reader);

            datumBindingSource.DataSource = table;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string pattern = Regex.Replace(txtSearch.Text, "([\\[\\]%])", "[$1]");
            pattern = Regex.Replace(pattern, "([\\*])", "[$1]");
            pattern = Regex.Replace(pattern, "(')", "''");
            datumBindingSource.Filter = "queueName like '%" + pattern + "%'";
        }
    }
}
