using System;
using System.IO;

namespace TestSite.SC91.XConnect.DeployTool
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length <= 0)
            {
                throw new ArgumentException($"Target folder not specified");
            }

            DirectoryInfo di = new DirectoryInfo(args[0].TrimEnd('\\', '\"'));
            if (!di.Exists)
            {
                throw new ArgumentException($"Target folder does not exist: {args[0]}");
            }

            Tasks.XConnectModel.Export(di);
        }
    }
}
