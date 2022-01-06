using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AliDDNS
{
    class LogModule
    {
        static string path = AppDomain.CurrentDomain.BaseDirectory;

        public static void Log(string logs)
        {
            StreamWriter sw = new StreamWriter(path + "log.txt", true);
            sw.WriteLine(string.Format("[{0}]: {1}", DateTime.Now.ToLocalTime().ToString(), logs));
            sw.Flush();
            sw.Dispose();
            //Console.WriteLine("logged");
        }

    }
}
