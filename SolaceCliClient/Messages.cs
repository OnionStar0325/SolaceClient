using System.Text.Json;

namespace SolaceCliClient;

public class Messages
{
    public List<string> Message { get; set; }

    public static Messages LoadConfiguration(string filePath)
    {
        string content = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<Messages>(content);
    }
}