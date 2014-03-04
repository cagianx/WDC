using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Windows.Forms;
using CsQuery;
using SelfishHttp;
using WDC.Project;

namespace WDC
{
 
    class ProjectRunner
    {
        protected WdcProject Project { get; set; }
        private SelfishHttp.Server _server;
        private int ServerPort { get; set; }
        public List<SiteRunner> LsSiteRunners = new List<SiteRunner>();
       
        public void StartServer()
        {
            ServerPort = Utility.ChooseRandomUnusedPort();
            Console.WriteLine("Starting WDC server....");
            if (this._server != null)
            {
                this._server.Stop();
            }
            this._server=new SelfishHttp
                        .Server(ServerPort);
            Console.WriteLine("Server listening on port " + ServerPort);
            this._server.OnPost("/next").Respond((req, res) => {

                var pageContent=req.BodyAs<string>();

                var ajR=System.Web.Helpers.Json.Decode<AjRequestNext>(pageContent);

                

                 var webReq = WebRequest.Create(ajR.Url);

                 webReq.Proxy = null;
                webReq.Method = ajR.IsPostRequest ? "POST" : "GET";

                webReq.ContentType = "application/x-www-form-urlencoded";

           
                var reqData = Encoding.UTF8.GetBytes(ajR.PostData);

                webReq.ContentLength = reqData.Length;

                using (var reqStream = webReq.GetRequestStream())
                    reqStream.Write(reqData, 0, reqData.Length);

                using (var wres = webReq.GetResponse())
                using (var resSteam = wres.GetResponseStream())
                using (var sr = new StreamReader(resSteam))
                {
                    LsSiteRunners.Add( new SiteRunner(sr.ReadToEnd(),Project.SiteList.First(x=>x.Id==ajR.TemplateId),this.ServerPort,Project.ScriptsPath));
                   
                    
                }
            });
        }


    }
   
    class SiteRunner
    {
        public string Identity = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);
        public FileInfo File2Parse;
        private WebBrowser Browser;
        private SiteTemplate Site;
        private List<Uri> Scripts;
        public bool IsRunning = true;
        public int ServerPort { get; set; }
        public Thread th { get; set; }
        public SiteRunner(string fileContent,
                        SiteTemplate st,
                        int serverPort,
                        List<Uri> scripts)
        {
            Scripts = scripts;
            this.Site = st;
            this.ServerPort = serverPort;

            File2Parse = new FileInfo(Path.GetTempFileName());
            File.WriteAllText(File2Parse.FullName, fileContent);

           th=new Thread(this.LauchSite);
            th.Start();
          
           
        }

        public void StopThread()
        {
            if (File2Parse!=null && File2Parse.Exists)
                File2Parse.Delete();
            IsRunning = false;
        }
        public void LauchSite()
        {
            var doc = CQ.CreateDocumentFromFile(File2Parse.FullName);

            doc.Find("style").Remove();
            doc.Find("link").Remove();

            foreach (var script in this.Scripts)
            {
                doc.Find("body").Append(script.AbsoluteUri);
            }

            doc.Find("body").Append(
                            CQ.CreateFragment("<script/>")
                                .Attr("type","text/javascript")
                                .Text(@"
                                    var window.WDC="+(Json.Encode(new Dictionary<string,string>
                                {
                                    {"ServiceUrl","http://localhost:"+this.ServerPort+"/"},
                                    {"Identity",this.Identity}

                                }))+";")

                                ).Append(CQ.CreateFragment("<script/>")
                                .Attr("type", "text/javascript")
                                .Text(File.ReadAllText("WDC.js")))

                                .Append(CQ.CreateFragment("<script/>")
                                .Attr("type", "text/javascript")
                                .Text(" window."+this.Site.CallBackName+"();"));


            doc.Save(File2Parse.FullName);
            Browser=new WebBrowser();
            Browser.Navigate(File2Parse.FullName);
            while(IsRunning)Thread.Sleep(100);
        }
    }
}
