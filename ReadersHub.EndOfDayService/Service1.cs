using Amazon;
using Amazon.SQS;
using NLog;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Timers;

namespace ReadersHub.EndOfDayService
{
    public partial class Service1 : ServiceBase
    {
        private readonly string SQS_URL;
        private readonly string SELLER_ID;
        private readonly AmazonSQSClient sqsClient;
        private static Logger logger;
        private Dictionary<string, string> Settings;

        private readonly int INTERVAL;


        public Service1()
        {
            InitializeComponent();
            InitializeSettings();

            SQS_URL = Settings["SQS_URL"];
            SELLER_ID = Settings["SELLER_ID"];
            INTERVAL = int.Parse(ConfigurationManager.AppSettings["HeartBeatIntervalSecond"]) * 1000;

            sqsClient = InitializeSQSClient();
            logger = LogManager.GetCurrentClassLogger();

        }
        private void InitializeSettings()
        {
            string path = ConfigurationManager.AppSettings["SettingsPath"];
            Settings = File
            .ReadAllLines(path)
            .Select(x => x.Split(','))
            .Where(x => x.Length > 1)
            .ToDictionary(x => x[0].Trim(), x => x[1].Trim());
        }
        public void OnDebug()
        {
            logger.Log(LogLevel.Info, "SERVICE START (ondebug)");
            timer.Interval = INTERVAL;
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
            timer.Start();
        }

        protected override void OnStart(string[] args)
        {
            logger.Log(LogLevel.Info, "SERVICE START (onstart)");
            timer.Interval = INTERVAL;
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
            timer.Start();
        }

        protected override void OnStop()
        {
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            logger.Log(LogLevel.Info, "PURGE QUEUE");
            try
            {
                var response = sqsClient.PurgeQueue(SQS_URL);
                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {

                }
            }
            catch (System.Exception ex)
            {
                /// TODO: Bu durumda ne yapacağız?
                /// 
                LogEventInfo logInfo = new LogEventInfo()
                {
                    Exception = ex,
                    Level = LogLevel.Error
                };

                logger.Log(logInfo);
            }
        }


        private AmazonSQSClient InitializeSQSClient()
        {
            string accessKey = Settings["ACCESS_KEY_ID"];
            string secretKey = Settings["SECRET_KEY"];
            return new AmazonSQSClient(accessKey, secretKey, new AmazonSQSConfig()
            {
                AuthenticationServiceName = "sqs",
                RegionEndpoint = RegionEndpoint.USWest2,
                SignatureMethod = Amazon.Runtime.SigningAlgorithm.HmacSHA256
            });
        }
    }
}
