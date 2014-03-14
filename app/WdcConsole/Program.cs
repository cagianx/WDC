using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text.RegularExpressions;
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
            var startWithNoParameters = !args.Any();

            do
            {
                if (startWithNoParameters)
                {
                    string command = "";
                    while (command.Trim().Length== 0)
                    {
                        Console.WriteLine("");
                        Console.WriteLine("Type a command:");
                        Console.Write("> ");
                        command = Console.ReadLine();
                    }
                    
                  
                    Regex argReg = new Regex(@"\w+|""[\\\/\.\w\s]*""");
                    args = new string[argReg.Matches(command).Count];
                    int i = 0;
                    foreach (var enumer in argReg.Matches(command))
                    {
                        args[i] = enumer.ToString();
                        i++;
                    }
                }

                switch (args[0])
                {
                    case "create-sample-project":
                    case "csp":

                        if (args.Length < 2 || String.IsNullOrEmpty(args[1])) 
                            Console.WriteLine("Argument 2 must be a file path");
                        else
                            CreateSampleProject(args[1]);
                        break;

                    case "build-project-file":
                    case "bpf":
                        BuildProjectFile();
                    
                        break;
                    case "exit":
                    case "q":
                        startWithNoParameters = false;
                        break;
                    default:
                        Console.WriteLine("Unrecognized command "+args[0]);
                        break;
                }
            } while (startWithNoParameters);
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
            Console.WriteLine("sample project created");
        }

        static void BuildProjectFile()
        {
            
        }

    }
}
