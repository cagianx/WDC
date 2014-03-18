using System;
using System.IO;

namespace WDC
{
    public class Config
    {
        public static string MainPath = Directory.GetParent(System.Reflection.Assembly.GetEntryAssembly().Location).FullName+@"\";

        public static string CachePath
        {
            get { return Path.Combine(MainPath, @"Cache\"); }
        }

        public static string CurrentPath
        {
            get
            {
                return Directory.GetCurrentDirectory();
                
            }
        }

    }

    
}
