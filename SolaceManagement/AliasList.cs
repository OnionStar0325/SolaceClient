using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace SolaceManagement
{
    public class AliasList
    {
        private static List<AliasItem> _aliasList = new List<AliasItem>();

        public class AliasItem
        {
            public string Alias { get; set; }
            public string Keyword { get; set; }
        }

        public static void LoadAliases()
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "aliases.yaml");
                if (File.Exists(path))
                {
                    var deserializer = new DeserializerBuilder()
                        .WithNamingConvention(PascalCaseNamingConvention.Instance)
                        .Build();

                    var yaml = File.ReadAllText(path);
                    _aliasList = deserializer.Deserialize<List<AliasItem>>(yaml) ?? new List<AliasItem>();
                }
                else
                {
                    // Fallback or verify if file is in project root during dev
                    if (File.Exists("aliases.yaml"))
                    {
                        var deserializer = new DeserializerBuilder()
                       .WithNamingConvention(PascalCaseNamingConvention.Instance)
                       .Build();
                        var yaml = File.ReadAllText("aliases.yaml");
                        _aliasList = deserializer.Deserialize<List<AliasItem>>(yaml) ?? new List<AliasItem>();
                    }
                }
            }
            catch (Exception ex)
            {
                // Silently fail or log to debug
                System.Diagnostics.Debug.WriteLine($"Failed to load aliases: {ex.Message}");
            }
        }

        public static List<AliasItem> Aliases { get { return _aliasList; } }
    }
}
