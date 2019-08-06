using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using TestSite.SC91.Feature.AutomationRules.Interfaces;

namespace TestSite.SC91.XConnect.DeployTool.Tasks
{
    internal class AutomationRules
    {
        public static void Export(FileSystemInfo targetDirectory)
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "  ",
                OmitXmlDeclaration = true
            };

            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            if (path != null)
            {
                foreach (string dll in Directory.GetFiles(path, "*.dll"))
                {
                    try
                    {
                        Assembly assembly = Assembly.LoadFrom(dll);

                        List<Type> assemblyTypes = assembly.GetTypes()
                            .Where(t => typeof(ICustomCondition).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
                            .ToList();

                        if (assemblyTypes.Any())
                        {
                            string xmlFilename = Path.Combine(targetDirectory.FullName, "App_data\\jobs\\continuous\\AutomationEngine\\App_Data\\Config\\sitecore\\Segmentation\\sc." + assembly.GetName().Name + ".xml");
                            FileInfo fi = new FileInfo(xmlFilename);
                            if (fi.Directory != null && !fi.Directory.Exists)
                            {
                                fi.Directory.Create();
                            }

                            FileStream s = new FileStream(xmlFilename, FileMode.Create, FileAccess.Write);

                            using (XmlWriter writer = XmlWriter.Create(s, settings))
                            {
                                writer.WriteStartElement("Settings");
                                writer.WriteStartElement("Sitecore");
                                writer.WriteStartElement("XConnect");
                                writer.WriteStartElement("Services");
                                writer.WriteStartElement("DescriptorLocator");
                                writer.WriteStartElement("Options");
                                writer.WriteStartElement("PredicateDescriptors");

                                foreach (Type interfaceType in assemblyTypes.OrderBy(t => t.FullName))
                                {
                                    ConstructorInfo[] ctors = interfaceType.GetConstructors();

                                    // invoke the first public constructor with no parameters.
                                    if (ctors[0].Invoke(new object[] { }) is ICustomCondition instance)
                                    {
                                        writer.WriteStartElement(interfaceType.Name);
                                        writer.WriteStartElement("id");
                                        writer.WriteString(instance.AssociatedItemId.ToString("B").ToUpper());
                                        writer.WriteEndElement();
                                        writer.WriteStartElement("type");
                                        writer.WriteString($"{interfaceType.FullName}, {assembly.GetName().Name}");
                                        writer.WriteEndElement();
                                        writer.WriteEndElement();
                                    }
                                }

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
                    catch (FileLoadException)
                    { } // The Assembly has already been loaded.
                    catch (BadImageFormatException)
                    { } // If a BadImageFormatException exception is thrown, the file is not an assembly.
                }
            }
        }
    }
}
