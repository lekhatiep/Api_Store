using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DoAn3API.Helper
{
    public static class LogHelper
    {
        private static IConfiguration _configuration;

        static LogHelper()
        {
            var builder = new ConfigurationBuilder()
           .SetBasePath(AppContext.BaseDirectory)
           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            _configuration = builder.Build();
        }

        static public void WriteLog(string fileName, string Content)
        {
            try
            {
                
                fileName = fileName != "" ? fileName : _configuration["LogFile"];

                if (_configuration["WriteLog"] == "1")
                {
                    string strFunction = "";
                    FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + fileName, FileMode.Append, FileAccess.Write);
                    StreamWriter srLog = new StreamWriter(fs);
                    string strWrite = Environment.NewLine + "====================================================" + Environment.NewLine;
                    strWrite = strFunction + DateTime.Now.ToString() + "--" + DateTime.Now.ToShortTimeString();
                    strWrite += ": ";
                    strWrite += Content;
                    srLog.WriteLine(strWrite);
                    srLog.Flush();
                    srLog.Close();
                    fs.Close();
                }
            }
            catch { }
        }

        static public void WriteLog(string Content)
        {
            string fileName = _configuration["LogFile"];
            WriteLog(fileName, Content);

        }
    }
}
