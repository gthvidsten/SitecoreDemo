using System.IO;
using System.Xml;
using Sitecore.XConnect.Schema;
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
                new DirectoryInfo(Path.Combine(targetDirectory.FullName, "App_data\\Models")),
                new DirectoryInfo(Path.Combine(targetDirectory.FullName, "App_data\\jobs\\continuous\\IndexWorker\\App_data\\Models"))
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

            // Create XML definition file for Automation Engine
            CreateModelXmlDefinition(Path.Combine(targetDirectory.FullName, "App_data\\jobs\\continuous\\AutomationEngine\\App_data\\Config\\sitecore", "sc." + typeof(CustomModel).FullName + ".xml"));

            // Create list of all custom facets to be automatically included when retrieving a contact in Automation Engine
            CreateModelXmlFacetDefinition(CustomModel.Model, Path.Combine(targetDirectory.FullName, "App_Data\\jobs\\continuous\\AutomationEngine\\App_Data\\Config\\sitecore\\MarketingAutomation\\sc.TestSite.XConnect.Model.xml"));
        }


        private static void CreateModelXmlFacetDefinition(XdbModel model, string xmlFilename)
        {
            FileInfo fi = new FileInfo(xmlFilename);
            if (fi.Directory != null && !fi.Directory.Exists)
            {
                fi.Directory.Create();
            }

            FileStream s = new FileStream(xmlFilename, FileMode.Create, FileAccess.Write);

            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                OmitXmlDeclaration = true
            };

            using (XmlWriter writer = XmlWriter.Create(s, settings))
            {
                writer.WriteStartElement("Settings");
                writer.WriteStartElement("Sitecore");
                writer.WriteStartElement("XConnect");
                writer.WriteStartElement("MarketingAutomation");
                writer.WriteStartElement("Engine");
                writer.WriteStartElement("Services");
                writer.WriteStartElement("MarketingAutomation.Loading.ContactFacetsConfigurator");
                writer.WriteStartElement("Options");
                writer.WriteStartElement("IncludeFacetNames");

                foreach (XdbFacetDefinition declaredFacet in model.DeclaredFacets)
                {
                    writer.WriteStartElement(declaredFacet.Name);
                    writer.WriteString(declaredFacet.Name);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.Flush();
            }
        }

        private static void CreateModelXmlDefinition(string xmlFilename)
        {
            FileInfo fi = new FileInfo(xmlFilename);
            if (fi.Directory != null && !fi.Directory.Exists)
            {
                fi.Directory.Create();
            }

            FileStream s = new FileStream(xmlFilename, FileMode.Create, FileAccess.Write);

            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                OmitXmlDeclaration = true
            };

            using (XmlWriter writer = XmlWriter.Create(s, settings))
            {
                writer.WriteStartElement("Settings");
                writer.WriteStartElement("Sitecore");
                writer.WriteStartElement("XConnect");
                writer.WriteStartElement("Services");
                writer.WriteStartElement("XConnect.Client.Configuration");
                writer.WriteStartElement("Options");
                writer.WriteStartElement("Models");
                writer.WriteStartElement("MyCustomModel");
                writer.WriteStartElement("TypeName");

                writer.WriteString($"{typeof(CustomModel).FullName}, {typeof(CustomModel).Assembly.GetName().Name}");

                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.Flush();
            }
        }
    }
}
