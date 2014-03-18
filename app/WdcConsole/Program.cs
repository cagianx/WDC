using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Helpers;
using WDC;
using WDC.Project;

namespace WdcConsole
{
    class Program
    {
        public static string AppPath;
        

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
                    case "run":
                        var projectFile = Path.Combine(Config.CurrentPath, args[1]);

                      var runner=  new ProjectRunner(Json.Decode<WdcProject>(File.ReadAllText(projectFile)),args[2]);

                        runner.StartServer();
                        break;
                    case "sc":
                      Console.WriteLine("Main Path: "+Config.MainPath);
                      Console.WriteLine("Current Path: " + Config.CurrentPath);
                      Console.WriteLine("Cache path: " + Config.CachePath);
                       
                        break;
                    case "create-sample-project":
                    case "csp":
                        var path = args.Length>=2&&!String.IsNullOrEmpty(args[1])
                            ? Path.Combine(Config.MainPath, args[1])
                            : Config.CurrentPath;


                        CreateSampleProject(path);
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

            var p = new WdcProject
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
