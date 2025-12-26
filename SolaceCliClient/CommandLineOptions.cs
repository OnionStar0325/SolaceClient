using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SolaceCliClient
{
    public class CommandLineOptions
    {
        [Option('a', "alias", Required = false, HelpText = "Solace Connection Alias")]
        public string Alias { get; set; }

        [Option('f', "file", Required = true, HelpText = "Message Template File Name")]
        public string FileName { get; set; }

        [Value(0, MetaName = "Inputs", HelpText = "Input Parameter for messages")]
        public IEnumerable<string> Inputs { get; set; }
    }
}
