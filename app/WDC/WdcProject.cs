using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WDC.Project
{
    public class WdcProject
    {
        public string Name { get; set; }

        public List<Uri> ScriptsPath = new List<Uri>();

        public string MainScriptSource { get; set; }

        public List<SiteTemplate> SiteList=new List<SiteTemplate>();
        
    }
    public class SiteTemplate
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public string CallBackName { get; set; }
        

    }
}
