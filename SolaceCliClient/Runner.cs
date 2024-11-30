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

        public ConnectionInfo ConnInfo { get; set; }
        public IContext Context { get; set; }

        public static void Main(String[] args)
        {
            XmlConfigurator.Configure(new FileInfo("log4net.config"));
            Execute(args);
        }

        public static void Execute(String[] args)
        {
            if (args.Length <= 0)
            {
                Console.Out.WriteLine("SolaceCliClient [JSON FILE LOCATION] [PARAM1...]");
                return;
            }
            logger.Info("Initialize");

            var instance = new Runner();
            instance.Initialize();

            if(string.IsNullOrEmpty(instance.ConnInfo.QueueName))
            {
                var message = "QueueName is not defined";
                logger.Error(message);
                return;
            }

            var queueProducer = new QueueProducer(instance.ConnInfo);
            queueProducer.Run(instance.Context);

            logger.Info(string.Format("Load JSON File [{0}]", args[0]));
            var messages = Messages.LoadConfiguration(args[0]);

            logger.Info(String.Format("{0} message(s) loaded", messages?.Message.Count));

            foreach(var message in messages.Message)
            {
                string content = message;

                for(int i = 1; i< args.Length; i++)
                {
                    content = content.Replace("${" + i + "}", args[i]);
                }

                if (string.IsNullOrEmpty(instance.ConnInfo.TimeKeyVariable) == false)
                {
                    content = content.Replace("${" + instance.ConnInfo.TimeKeyVariable + "}", DateTime.Now.ToString("yyyyMMddHHmmssfffffff"));
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
            ConnInfo = ConnectionInfo.LoadConfiguration("connectioninfo.json");
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
