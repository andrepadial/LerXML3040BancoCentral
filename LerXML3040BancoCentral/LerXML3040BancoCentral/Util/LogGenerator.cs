using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LerXML3040BancoCentral.Util
{
    public class LogGenerator
    {
        public static void WriteLog(string strLog)
        {
            //string logFilePath = @"C:\Logs\Log-" + System.DateTime.Today.ToString("MM-dd-yyyy") + "." + "txt";
            string logFilePath = ConfigurationManager.AppSettings["diretorioLog"];
            FileInfo logFileInfo = new FileInfo(logFilePath);
            DirectoryInfo logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists) logDirInfo.Create();
            using (FileStream fileStream = new FileStream(logFilePath, FileMode.Append))
            {
                using (StreamWriter log = new StreamWriter(fileStream))
                {
                    log.WriteLine(strLog);
                }
            }
        }
    }
}
