using CommandLine;
using log4net;
using log4net.Config;
using SolaceManagement;
using SolaceSystems.Solclient.Messaging;
using System.Text.RegularExpressions;

namespace SolaceCliClient
{
    public class Runner
    {
        static ILog logger = LogManager.GetLogger(typeof(Runner));

        public static ConnectionInfoList ConnInfoList { get; set; }
        public IContext Context { get; set; }

        public static void Main(String[] args)
        {
            
            Parser.Default.ParseArguments<CommandLineOptions>(args)
                            .WithParsed(Execute)
                            .WithNotParsed(HandleParseError);
        }

        private static void HandleParseError(IEnumerable<Error> enumerable)
        {
            return;
;        }

        public static void Execute(CommandLineOptions options)
        {
            XmlConfigurator.Configure(new FileInfo("log4net.config"));
            logger.Info("Initialize");

            ConnectionInfo connInfo = null;

            var instance = new Runner();
            instance.Initialize();

            if (string.IsNullOrEmpty(options.Alias) == false)
            {
                var selectedList = ConnInfoList.ConnInfoList.Where(r => string.Equals(r.Alias, options.Alias)).ToList();
                
                if (selectedList == null || selectedList.Count <= 0)
                {
                    Console.Error.WriteLine(string.Format("Alias [{0}] is not defined", options.Alias));
                    return;
                } else
                {
                    logger.Info(string.Format("Connecting Alias [{0}] ...", options.Alias));
                    connInfo = selectedList.FirstOrDefault();
                }
            }
            else
            {
                connInfo = ConnInfoList.ConnInfoList.FirstOrDefault();
            }

            if(string.IsNullOrEmpty(connInfo.TimeKeyVariable))
            {
                connInfo.TimeKeyVariable = ConnInfoList.TimeKeyVariable;
            }

            if(string.IsNullOrEmpty(connInfo.QueueName))
            {
                var message = "QueueName is not defined";
                logger.Error(message);
                return;
            }

            var queueProducer = new QueueProducer(connInfo);
            queueProducer.Run(instance.Context);

            logger.Info(string.Format("Load JSON File [{0}]", options.FileName));
            var messages = Messages.LoadConfiguration(options.FileName);

            logger.Info(String.Format("{0} message(s) loaded", messages?.Message.Count));

            foreach(var message in messages.Message)
            {
                string content = message;

                var inputArgs = options.Inputs.ToArray<string>();

                for(int i = 0; i< inputArgs.Length; i++)
                {
                    content = content.Replace("${" + (i+1) + "}", inputArgs[i]);
                }

                if (string.IsNullOrEmpty(connInfo.TimeKeyVariable) == false)
                {
                    content = content.Replace("${" + connInfo.TimeKeyVariable + "}", DateTime.Now.ToString("yyyyMMddHHmmssfffffff"));
                }

                var regex = new Regex(@"\$\[((,|[0-9])+)\]");

                var rand = new Random();
                foreach(Match match in regex.Matches(content))
                {
                    string[] values = match.Groups[1].Value.Split(',');
                    string replaceValue = values[rand.Next(values.Length)];
                    content = content.Replace(match.Value, replaceValue);
                }

                var log = "Prepared Message : " + Environment.NewLine + content;
                logger.Info(log);
                ConsoleKey inputKey;

                do
                {
                    logger.Info("Press 'Enter' to send or 'K' to skip.......");
                }
                while ((inputKey = Console.ReadKey().Key) != ConsoleKey.Enter && inputKey != ConsoleKey.K);
               
                if(inputKey == ConsoleKey.K)
                {
                    logger.Info("Skipped ...");
                }else
                {
                    queueProducer.SendMessage(content, null);
                }
            }
        }

        private void Initialize()
        {
            InitializeContext();
            Context = ContextFactory.Instance.CreateContext(new ContextProperties(), null);
            ConnInfoList =  ConnectionInfoList.LoadConfiguration("connectioninfo.json");
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
    }
}
