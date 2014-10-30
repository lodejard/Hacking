using Microsoft.Build.BuildEngine;
using Microsoft.Build.Evaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var x in new[] {
                //@"..\..\..\LibraryBeta.PCL.Desktop\LibraryBeta.PCL.Desktop.csproj",
                @"..\..\..\LibraryGamma\LibraryGamma.csproj"})
            {
                Console.WriteLine("---- {0}", x);
                var e = new ProjectCollection(new Dictionary<string, string>()
                {
                    {"Configuration", "Release"},
                    {"OutDir", @"C:\Users\lodejard\Projects\Hacking\artifacts"},
                    {"DesignTimeBuild", "true"},
                    {"BuildProjectReferences", "false"}
                });
                var rootElement = new XElement("root");
                var projectFiles = new List<string> { x };
                for (var index = 0; index != projectFiles.Count; index++)
                {
                    var projectFile = projectFiles[index];
                    var project = e.LoadProject(projectFile);
                    var projectInstance = project.CreateProjectInstance();
                    var buildResult = projectInstance.Build("ResolveReferences", null);

                    var projectElement = new XElement("project");
                    projectElement.SetAttributeValue("projectFile", projectFile);
                    projectElement.SetAttributeValue("buildResult", buildResult);
                    rootElement.Add(projectElement);

                    foreach (var itemType in projectInstance.ItemTypes)
                    {
                        foreach (var item in projectInstance.GetItems(itemType))
                        {
                            if (item.ItemType == "ReferencePath" && item.HasMetadata("MSBuildSourceProjectFile"))
                            {
                                var referenceProjectFile = item.GetMetadata("MSBuildSourceProjectFile").EvaluatedValue;
                                if (!projectFiles.Contains(referenceProjectFile))
                                {
                                    projectFiles.Add(referenceProjectFile);
                                }
                            }

                            var itemElement = new XElement("item");
                            itemElement.SetAttributeValue("itemType", item.ItemType);
                            itemElement.SetAttributeValue("evaluated", item.EvaluatedInclude);
                            projectElement.Add(itemElement);

                            foreach (var metadata in item.Metadata)
                            {
                                var metadataElement = new XElement("metadata");
                                metadataElement.SetAttributeValue("name", metadata.Name);
                                metadataElement.SetAttributeValue("evaluated", metadata.EvaluatedValue);
                                itemElement.Add(metadataElement);
                            }
                        }
                    }

                    foreach (var property in projectInstance.Properties)
                    {
                        var propertyElement = new XElement("property");
                        propertyElement.SetAttributeValue("name", property.Name);
                        propertyElement.SetAttributeValue("evaluated", property.EvaluatedValue);
                        projectElement.Add(propertyElement);
                    }
                }
                rootElement.Save(Console.Out);
            }
        }
    }
}
