using System;
using System.Configuration;
using System.IO;
using System.ServiceProcess;
using System.Threading;

namespace TestMachine
{
    public partial class TestMachine : ServiceBase
    {
        public TestMachine()
        {
            InitializeComponent();
            DiagnosticOut("Hello World!", 1);                                                    //say hello to the world
        }

        protected override void OnStart(string[] args)
        {
            CallOnStart();
            DiagnosticOut("Started up.", 3);                                                    // report making it through startup
        }

        /// <summary>
        ///  Called on service stop
        /// </summary>
        protected override void OnStop()
        {
            base.OnStop();
            CallOnStop();
            DiagnosticOut("Stopped", 3);                                                        // report making it through stoping
        }

        public static void DiagnosticOut(string message, int LoggingLevel)                             //report status mesages to a file
        {
            try
            {
                string DiagnosticMessage = "";                                                  //message to be output to the file
                for (int x = 0; x < message.Length; x++)                                        //foreach character in the message
                {
                    if (message[x] != ',')                                                      //if it is not a comma
                        DiagnosticMessage += message[x];                                        //carry it over
                }                                                                               //add all sorts of diagnostic datas
                DiagnosticMessage += ", TimeStamp:" + DateTime.Now.ToString() + ", LoggingLevelNeeded: " + LoggingLevel.ToString() + ", LoggingLevelSelected" + LogggingLevel.ToString();
                if (LoggingLevel <= LogggingLevel)                                              //if we are asking to see this data
                {
                    using (StreamWriter DiagnosticWriter = File.AppendText(ConfigurationManager.AppSettings["DiagnosticFile"]))// @"C:\Users\d.paddock\Desktop\Diagnostic.csv")) defualt
                    {
                        DiagnosticWriter.WriteLine(DiagnosticMessage);                          //output it to file
                    }
                }
            }
            catch (Exception ex)                                                                //catch any errors and cry becouse we cant log them.
            {
                try//try a simplified log
                {
                    using (StreamWriter DiagnosticWriter = File.AppendText(ConfigurationManager.AppSettings["DiagnosticFile"]))// @"C:\Users\d.paddock\Desktop\Diagnostic.csv")) defualt
                    {
                        DiagnosticWriter.WriteLine(ex.ToString());                          //output it to file
                    }
                }
                catch (Exception Ex)//sadly giveup
                {
                }
            }
        }

        private int good = 0;                                               //contains a count of good product
        private int bad = 0;                                                //contains a count of bad product
        private int index = 0;                                              //contains a count of true machine indexes
        private int empty = 0;                                              //contains a count of indexes that were empty
        private string MachineName = "HIL-XS-FIM";
        public static int LogggingLevel;                                    //what logging level the service has selected
        private Random random;                                              //used to randomize data
        private string TopicName;                                           //Topic the Main Subscriber is subbed to
        private string Broker;                                              //IP of the broker we are connecting to
        private TopicPublisher Publisher;
        private Timer IndexTimer;                                           //used to index every set interval
        private int Index = 0;

        private void CallOnStop()
        {
        }

        /// <summary>
        /// Call On start of service
        /// </summary>
        private void CallOnStart()
        {
            TopicName = ConfigurationManager.AppSettings["MainTopicName"];               //load everything from the app settings
            Broker = ConfigurationManager.AppSettings["BrokerIP"];
            random = new Random();
            try
            {
                Publisher = new TopicPublisher(TopicName, Broker);
                IndexTimer = new System.Threading.Timer(OnElapsedTime, null, 1000, Convert.ToInt32(ConfigurationManager.AppSettings["indexrate"]));
            }
            catch
            {
            }
        }

        private void OnElapsedTime(object source)
        {
            Index++;
            if (Index < 120)
            {
                switch (random.Next(0, 7))
                {
                    case 0:
                        switch (random.Next(0, 3))
                        {
                            case 0:
                                Publisher.SendMessage("     {\"Machine\": \"" + MachineName + "\", \"Good\":\"0\", \"Bad\":\"1\",\"Empty\":\"0\",\"Attempt\":\"1\",\"Other\":\"1\",\"HeadNumber\":\"" + (Index % 4).ToString() + "\"}");
                                break;

                            case 1:
                                Publisher.SendMessage("     {\"Machine\": \"" + MachineName + "\", \"Good\":\"0\", \"Bad\":\"1\",\"Empty\":\"0\",\"Attempt\":\"1\",\"Other\":\"0\",\"HeadNumber\":\"" + (Index % 4).ToString() + "\",\"Error1\":\"1\"}");
                                break;

                            case 2:
                                Publisher.SendMessage("     {\"Machine\": \"" + MachineName + "\", \"Good\":\"0\", \"Bad\":\"1\",\"Empty\":\"0\",\"Attempt\":\"1\",\"Other\":\"0\",\"HeadNumber\":\"" + (Index % 4).ToString() + "\",\"Error2\":\"1\"}");
                                break;
                        }
                        bad++;
                        break;

                    case 1:
                        Publisher.SendMessage("     {\"Machine\": \"" + MachineName + "\", \"Good\":\"1\", \"Bad\":\"0\",\"Empty\":\"0\",\"Attempt\":\"1\",\"Other\":\"0\",\"HeadNumber\":\"" + (Index % 4).ToString() + "\"}");
                        good++;
                        break;

                    case 2:
                        Publisher.SendMessage("     {\"Machine\": \"" + MachineName + "\", \"Good\":\"1\", \"Bad\":\"0\",\"Empty\":\"0\",\"Attempt\":\"1\",\"Other\":\"0\",\"HeadNumber\":\"" + (Index % 4).ToString() + "\"}");
                        good++;
                        break;

                    case 3:
                        Publisher.SendMessage("     {\"Machine\": \"" + MachineName + "\", \"Good\":\"1\", \"Bad\":\"0\",\"Empty\":\"0\",\"Attempt\":\"1\",\"Other\":\"0\",\"HeadNumber\":\"" + (Index % 4).ToString() + "\"}");
                        good++;
                        break;

                    case 4:
                        Publisher.SendMessage("     {\"Machine\": \"" + MachineName + "\", \"Good\":\"1\", \"Bad\":\"0\",\"Empty\":\"0\",\"Attempt\":\"1\",\"Other\":\"0\",\"HeadNumber\":\"" + (Index % 4).ToString() + "\"}");
                        good++;
                        break;

                    case 5:
                        Publisher.SendMessage("     {\"Machine\": \"" + MachineName + "\", \"Good\":\"1\", \"Bad\":\"0\",\"Empty\":\"0\",\"Attempt\":\"1\",\"Other\":\"0\",\"HeadNumber\":\"" + (Index % 4).ToString() + "\"}");
                        good++;
                        break;

                    case 6:
                        Publisher.SendMessage("     {\"Machine\": \"" + MachineName + "\", \"Good\":\"0\", \"Bad\":\"0\",\"Empty\":\"1\",\"Attempt\":\"0\",\"Other\":\"0\",\"HeadNumber\":\"" + (Index % 4).ToString() + "\"}");
                        empty++;
                        break;
                }
                index++;
            }
            if (Index == 120)
            {
                Publisher.SendMessage("     {\"Machine\": \"" + MachineName + "\", \"Good\":\"" + good.ToString() + "\" , \"Bad\":\"" + bad.ToString() + "\", \"Empty\":\"" + empty.ToString() + "\", \"Indexes\":\"" + index.ToString() + "\", \"UOM\":\"EA\", \"NAED\":\"31474\"}");
                good = 0;
                bad = 0;
                index = 0;
                empty = 0;
            }
            if (Index == 121)
            {
                Publisher.SendMessage("     {\"Machine\": \"" + MachineName + "\", \"StatusCode\":\"0\" , \"MReason\":\"Preventing Wrenching\", \"UReason\":\"oiling up wrench launcher\", \"NAED\":\"31474\",\"Code\": \"1300\"}");
            }
            if (Index == 150)
            {
                Publisher.SendMessage("     {\"Machine\": \"" + MachineName + "\", \"StatusCode\":\"2\" , \"MReason\":\"Preventing Wrenching\", \"UReason\":\"oiling up wrench launcher\", \"NAED\":\"31474\",\"Code\": \"1300\"}");
                Index = 0;
            }
        }
    }
}