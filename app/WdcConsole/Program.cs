using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Web.Helpers;
using WDC.Project;

namespace WdcConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to wdc console app!");
            Console.WriteLine("Current Arguments "+Json.Encode(args));
            if (!args.Any())
            {
             
                Exit("parameter needed");
            }

            switch (args[0])
            {
                case "create-sample-project":
                case "csp":

                    if (args.Length<2||String.IsNullOrEmpty(args[1])) Exit("Argument 2 must be a file path");
                    CreateSampleProject(args[1]);
                    break;

                case "build-project-file":
                case "bpf":
                    BuildProjectFile();

                    break;
            }
        }

       

        static void Exit(string messagge,int code = 0)
        {
            Console.WriteLine(messagge);
            Console.WriteLine("Exit");
            Environment.Exit(code);
        }
        static void CreateSampleProject(string path)
        {

            var p = new WDC.Project.WdcProject
            {
                Name = "Project to extract data from example.com",
                MainScriptSource = "here the Javascritp code",
                ScriptsPath = new List<Uri>
                        {
                            new Uri("http://code.jquery.com/jquery-latest.min.js")
                        }
            };
            p.SiteList.Add(new SiteTemplate
            {
                CallBackName = "HomePageCb",
                Description = "This is the description for the homepage",
                Id = "HomePage"
            });
            p.SiteList.Add(new SiteTemplate
            {
                CallBackName = "ResultPageCb",
                Description = "This is the description for the result page",
                Id = "ResultPage"
            });
            File.WriteAllText(path+"SampleProject.wdc",Json.Encode(p));
            Exit("sample project created");
        }

        static void BuildProjectFile()
        {
            
        }

    }
}
