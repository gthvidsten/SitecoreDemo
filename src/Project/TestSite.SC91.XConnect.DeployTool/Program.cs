using System;
using System.IO;

namespace TestSite.SC91.XConnect.DeployTool
{
    internal class Program
    {
        private const string XCONNECT_PATH_FILENAME = "XConnectPathArgs.txt";

        private static void Main(string[] args)
        {
            string xConnectPath;

            if (args.Length > 0)
            {
                xConnectPath = args[0];
            }
            else
            {


                if (!File.Exists(XCONNECT_PATH_FILENAME))
                {
                    throw new Exception($"File with path to XConnect project folder does not exist: {XCONNECT_PATH_FILENAME}");
                }

                xConnectPath = File.ReadAllText(XCONNECT_PATH_FILENAME);
            }


            DirectoryInfo di = new DirectoryInfo(xConnectPath);
            if (!di.Exists)
            {
                throw new ArgumentException($"XConnect path does not exist: {xConnectPath}");
            }

            // Export XConnect Model
            Tasks.XConnectModel.Export(di);

            // Export Automation rules
            Tasks.AutomationRules.Export(di);
        }
    }
}
