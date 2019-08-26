using System;
using System.Configuration;
using System.IO;
using System.ServiceProcess;
using System.Threading;

namespace TestLine
{
    public partial class TestLine : ServiceBase
    {
        public TestLine()
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

        private int M1Good = 0;                                               //contains a count of good product
        private int M1bad = 0;                                                //contains a count of bad product
        private int M1index = 0;                                              //contains a count of true machine indexes
        private int M1empty = 0;                                              //contains a count of indexes that were empty
        private string M1MachineName = "HIL-XS-FIM";
        private int M1Index = 0;
        private int M2Good = 0;                                               //contains a count of good product
        private int M2bad = 0;                                                //contains a count of bad product
        private int M2index = 0;                                              //contains a count of true machine indexes
        private int M2empty = 0;                                              //contains a count of indexes that were empty
        private string M2MachineName = "HIL-XS-AF5";
        private int M2Index = 0;
        private int M3Good = 0;                                               //contains a count of good product
        private int M3bad = 0;                                                //contains a count of bad product
        private int M3index = 0;                                              //contains a count of true machine indexes
        private int M3empty = 0;                                              //contains a count of indexes that were empty
        private string M3MachineName = "HIL-XS-Cram";
        private int M3Index = 0;
        private int M4Good = 0;                                               //contains a count of good product
        private int M4bad = 0;                                                //contains a count of bad product
        private int M4index = 0;                                              //contains a count of true machine indexes
        private int M4empty = 0;                                              //contains a count of indexes that were empty
        private string M4MachineName = "HIL-XS-LugInserter";
        private int M4Index = 0;
        public static int LogggingLevel;                                    //what logging level the service has selected
        private Random random;                                              //used to randomize data
        private string TopicName;                                           //Topic the Main Subscriber is subbed to
        private string Broker;                                              //IP of the broker we are connecting to
        private TopicPublisher Publisher;
        private Timer M1Timer;                                              //used to index every set interval
        private Timer M2Timer;                                              //used to index every set interval
        private Timer M3Timer;                                              //used to index every set interval
        private Timer M4Timer;                                              //used to index every set interval

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
                M1Timer = new Timer(M1TimeElapsed, null, 1000, Convert.ToInt32(ConfigurationManager.AppSettings["indexrate"]));
                M2Timer = new Timer(M2TimeElapsed, null, 1000, Convert.ToInt32(ConfigurationManager.AppSettings["indexrate"]) + 100);
                M3Timer = new Timer(M3TimeElapsed, null, 1000, Convert.ToInt32(ConfigurationManager.AppSettings["indexrate"]) + 200);
                M4Timer = new Timer(M4TimeElapsed, null, 1000, Convert.ToInt32(ConfigurationManager.AppSettings["indexrate"]) + 300);
            }
            catch(Exception ex)
            {

            }
        }

        private void M1TimeElapsed(object source)
        {
            M1Index++;
            if (M1Index < 320)
            {
                int Number = random.Next(0, 100);

                if (Number < 3)
                {
                    switch (random.Next(0, 3))
                    {
                        case 0:
                            Publisher.SendMessage("     {\"Machine\": \"" + M1MachineName + "\", \"Good\":\"0\", \"Bad\":\"1\",\"Empty\":\"0\",\"Attempt\":\"1\",\"Other\":\"1\",\"Head_number\":\"" + (M1Index % 4).ToString() + "\"}");
                            break;

                        case 1:
                            Publisher.SendMessage("     {\"Machine\": \"" + M1MachineName + "\", \"Good\":\"0\", \"Bad\":\"1\",\"Empty\":\"0\",\"Attempt\":\"1\",\"Other\":\"0\",\"Head_number\":\"" + (M1Index % 4).ToString() + "\",\"Error1\":\"1\"}");
                            break;

                        case 2:
                            Publisher.SendMessage("     {\"Machine\": \"" + M1MachineName + "\", \"Good\":\"0\", \"Bad\":\"1\",\"Empty\":\"0\",\"Attempt\":\"1\",\"Other\":\"0\",\"Head_number\":\"" + (M1Index % 4).ToString() + "\",\"Error2\":\"1\"}");
                            break;
                    }
                    M1bad++;
                }
                if (Number > 3 && Number < 98)
                {
                    Publisher.SendMessage("     {\"Machine\": \"" + M1MachineName + "\", \"Good\":\"1\", \"Bad\":\"0\",\"Empty\":\"0\",\"Attempt\":\"1\",\"Other\":\"0\",\"Head_number\":\"" + (M1Index % 4).ToString() + "\"}");
                    M1Good++;
                }
                if (Number > 98)
                {
                    Publisher.SendMessage("     {\"Machine\": \"" + M1MachineName + "\", \"Good\":\"0\", \"Bad\":\"0\",\"Empty\":\"1\",\"Attempt\":\"0\",\"Other\":\"0\",\"Head_number\":\"" + (M1Index % 4).ToString() + "\"}");
                    M1empty++;
                }
            }
            if (M1Index == 160)
            {
                Publisher.SendMessage("     {\"Machine\": \"" + M1MachineName + "\", \"Good\":\"" + M1Good.ToString() + "\" , \"Bad\":\"" + M1bad.ToString() + "\", \"Empty\":\"" + M1empty.ToString() + "\", \"Indexes\":\"" + M1index.ToString() + "\", \"UOM\":\"EA\", \"NAED\":\"31518\"}");
                M1Good = 0;
                M1bad = 0;
                M1index = 0;
                M1empty = 0;
                M1Index = 0;
            }
        }

        private void M2TimeElapsed(object source)
        {
            M2Index++;
            if (M2Index < 320)
            {
                int Number = random.Next(0, 100);

                if (Number < 3)
                {
                    switch (random.Next(0, 3))
                    {
                        case 0:
                            Publisher.SendMessage("     {\"Machine\": \"" + M2MachineName + "\", \"Good\":\"0\", \"Bad\":\"1\",\"Empty\":\"0\",\"Attempt\":\"1\",\"Other\":\"1\",\"Head_number\":\"" + (M2Index % 4).ToString() + "\"}");
                            break;

                        case 1:
                            Publisher.SendMessage("     {\"Machine\": \"" + M2MachineName + "\", \"Good\":\"0\", \"Bad\":\"1\",\"Empty\":\"0\",\"Attempt\":\"1\",\"Other\":\"0\",\"Head_number\":\"" + (M2Index % 4).ToString() + "\",\"Error1\":\"1\"}");
                            break;

                        case 2:
                            Publisher.SendMessage("     {\"Machine\": \"" + M2MachineName + "\", \"Good\":\"0\", \"Bad\":\"1\",\"Empty\":\"0\",\"Attempt\":\"1\",\"Other\":\"0\",\"Head_number\":\"" + (M2Index % 4).ToString() + "\",\"Error2\":\"1\"}");
                            break;
                    }
                    M2bad++;
                }
                if (Number > 3 && Number < 98)
                {
                    Publisher.SendMessage("     {\"Machine\": \"" + M2MachineName + "\", \"Good\":\"1\", \"Bad\":\"0\",\"Empty\":\"0\",\"Attempt\":\"1\",\"Other\":\"0\",\"Head_number\":\"" + (M2Index % 4).ToString() + "\"}");
                    M2Good++;
                }
                if (Number > 98)
                {
                    Publisher.SendMessage("     {\"Machine\": \"" + M2MachineName + "\", \"Good\":\"0\", \"Bad\":\"0\",\"Empty\":\"1\",\"Attempt\":\"0\",\"Other\":\"0\",\"Head_number\":\"" + (M2Index % 4).ToString() + "\"}");
                    M2empty++;
                }
            }
            if (M2Index == 160)
            {
                Publisher.SendMessage("     {\"Machine\": \"" + M2MachineName + "\", \"Good\":\"" + M2Good.ToString() + "\" , \"Bad\":\"" + M2bad.ToString() + "\", \"Empty\":\"" + M2empty.ToString() + "\", \"Indexes\":\"" + M2index.ToString() + "\", \"UOM\":\"EA\", \"NAED\":\"31518\"}");
                M2Good = 0;
                M2bad = 0;
                M2index = 0;
                M2empty = 0;
                M2Index = 0;
            }
        }

        private void M3TimeElapsed(object source)
        {
            M3Index++;
            if (M3Index < 320)
            {
                int Number = random.Next(0, 100);

                if (Number < 3)
                {
                    switch (random.Next(0, 3))
                    {
                        case 0:
                            Publisher.SendMessage("     {\"Machine\": \"" + M3MachineName + "\", \"Good\":\"0\", \"Bad\":\"1\",\"Empty\":\"0\",\"Attempt\":\"1\",\"Other\":\"1\",\"Head_number\":\"" + (M3Index % 4).ToString() + "\"}");
                            break;

                        case 1:
                            Publisher.SendMessage("     {\"Machine\": \"" + M3MachineName + "\", \"Good\":\"0\", \"Bad\":\"1\",\"Empty\":\"0\",\"Attempt\":\"1\",\"Other\":\"0\",\"Head_number\":\"" + (M3Index % 4).ToString() + "\",\"Error1\":\"1\"}");
                            break;

                        case 2:
                            Publisher.SendMessage("     {\"Machine\": \"" + M3MachineName + "\", \"Good\":\"0\", \"Bad\":\"1\",\"Empty\":\"0\",\"Attempt\":\"1\",\"Other\":\"0\",\"Head_number\":\"" + (M3Index % 4).ToString() + "\",\"Error2\":\"1\"}");
                            break;
                    }
                    M3bad++;
                }
                if (Number > 3 && Number < 98)
                {
                    Publisher.SendMessage("     {\"Machine\": \"" + M3MachineName + "\", \"Good\":\"1\", \"Bad\":\"0\",\"Empty\":\"0\",\"Attempt\":\"1\",\"Other\":\"0\",\"Head_number\":\"" + (M3Index % 4).ToString() + "\"}");
                    M3Good++;
                }
                if (Number > 98)
                {
                    Publisher.SendMessage("     {\"Machine\": \"" + M3MachineName + "\", \"Good\":\"0\", \"Bad\":\"0\",\"Empty\":\"1\",\"Attempt\":\"0\",\"Other\":\"0\",\"Head_number\":\"" + (M3Index % 4).ToString() + "\"}");
                    M3empty++;
                }
            }
            if (M3Index == 160)
            {
                Publisher.SendMessage("     {\"Machine\": \"" + M3MachineName + "\", \"Good\":\"" + M3Good.ToString() + "\" , \"Bad\":\"" + M3bad.ToString() + "\", \"Empty\":\"" + M3empty.ToString() + "\", \"Indexes\":\"" + M3index.ToString() + "\", \"UOM\":\"EA\", \"NAED\":\"31518\"}");
                M3Good = 0;
                M3bad = 0;
                M3index = 0;
                M3empty = 0;
                M3Index = 0;
            }
        }

        private void M4TimeElapsed(object source)
        {
            M4Index++;
            if (M4Index < 320)
            {
                int Number = random.Next(0, 100);

                if (Number < 3)
                {
                    switch (random.Next(0, 3))
                    {
                        case 0:
                            Publisher.SendMessage("     {\"Machine\": \"" + M4MachineName + "\", \"Good\":\"0\", \"Bad\":\"1\",\"Empty\":\"0\",\"Attempt\":\"1\",\"Other\":\"1\",\"Head_number\":\"" + (M4Index % 4).ToString() + "\"}");
                            break;

                        case 1:
                            Publisher.SendMessage("     {\"Machine\": \"" + M4MachineName + "\", \"Good\":\"0\", \"Bad\":\"1\",\"Empty\":\"0\",\"Attempt\":\"1\",\"Other\":\"0\",\"Head_number\":\"" + (M4Index % 4).ToString() + "\",\"Error1\":\"1\"}");
                            break;

                        case 2:
                            Publisher.SendMessage("     {\"Machine\": \"" + M4MachineName + "\", \"Good\":\"0\", \"Bad\":\"1\",\"Empty\":\"0\",\"Attempt\":\"1\",\"Other\":\"0\",\"Head_number\":\"" + (M4Index % 4).ToString() + "\",\"Error2\":\"1\"}");
                            break;
                    }
                    M4bad++;
                }
                if (Number > 3 && Number < 98)
                {
                    Publisher.SendMessage("     {\"Machine\": \"" + M4MachineName + "\", \"Good\":\"1\", \"Bad\":\"0\",\"Empty\":\"0\",\"Attempt\":\"1\",\"Other\":\"0\",\"Head_number\":\"" + (M4Index % 4).ToString() + "\"}");
                    M4Good++;
                }
                if (Number > 98)
                {
                    Publisher.SendMessage("     {\"Machine\": \"" + M4MachineName + "\", \"Good\":\"0\", \"Bad\":\"0\",\"Empty\":\"1\",\"Attempt\":\"0\",\"Other\":\"0\",\"Head_number\":\"" + (M4Index % 4).ToString() + "\"}");
                    M4empty++;
                }
            }
            if (M4Index == 160)
            {
                Publisher.SendMessage("     {\"Machine\": \"" + M4MachineName + "\", \"Good\":\"" + M4Good.ToString() + "\" , \"Bad\":\"" + M4bad.ToString() + "\", \"Empty\":\"" + M4empty.ToString() + "\", \"Indexes\":\"" + M4index.ToString() + "\", \"UOM\":\"EA\", \"NAED\":\"31518\"}");
                M4Good = 0;
                M4bad = 0;
                M4index = 0;
                M4empty = 0;
                M4Index = 0;
            }
        }
    }
}