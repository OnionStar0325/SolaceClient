using System.Text.Json;

namespace SolaceManagement;

public class ConnectionInfo
{
    public string HostName { get; set; }
    public string VPNName { get; set; }
    public string QueueName { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
    public string SEMPHostName { get; set; }
    public string ReplyQueueName { get; set; }
    public string TimeKeyVariable { get; set; }

    public List<string> ExcludePatterns { get; set; }

    public static ConnectionInfo LoadConfiguration(string filePath)
    {
        string configContent = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<ConnectionInfo>(configContent);
    }
}