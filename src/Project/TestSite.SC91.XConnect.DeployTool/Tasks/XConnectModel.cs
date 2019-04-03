using System.IO;
using Sitecore.XConnect.Serialization;
using TestSite.SC91.Foundation.XConnect;

namespace TestSite.SC91.XConnect.DeployTool.Tasks
{
    internal class XConnectModel
    {
        public static void Export(FileSystemInfo targetDirectory)
        {
            // File name will be Sitecore.XConnect.Collection.Model, 9.0.json
            string json = XdbModelWriter.Serialize(CustomModel.Model);

            DirectoryInfo[] paths =
            {
                new DirectoryInfo(Path.Combine(targetDirectory.FullName, "Project\\TestSite.SC91.XConnect\\App_data\\Models")),
                new DirectoryInfo(Path.Combine(targetDirectory.FullName, "Project\\TestSite.SC91.XConnect\\App_data\\jobs\\continuous\\IndexWorker\\App_data\\Models"))
            };

            foreach (DirectoryInfo di in paths)
            {
                if (!di.Exists)
                {
                    di.Create();
                }

                // Copy JSON file to XConnect
                File.WriteAllText(Path.Combine(di.FullName, CustomModel.Model.FullName + ".json"), json);
            }
        }
    }
}
