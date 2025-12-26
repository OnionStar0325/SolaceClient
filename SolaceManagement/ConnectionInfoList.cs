using System.Text.Json;

namespace SolaceManagement;

public class ConnectionInfoList
{
    public List<ConnectionInfo> ConnInfoList { get; set; }
    public string TimeKeyVariable { get; set; }

    public List<string> ExcludePatterns { get; set; }

    public static ConnectionInfoList LoadConfiguration(string filePath)
    {
        string configContent = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<ConnectionInfoList>(configContent);
    }
}