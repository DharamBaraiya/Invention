using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESSAM_ACCOUNTANT.BLL.Helpers
{
    public class Logs
    {

        public static void writeLogPriority(string content)
        {
            try
            {
                string logfilePath = AppDomain.CurrentDomain.BaseDirectory + "Logs\\ErrorS\\Error" + DateTime.Now.ToString("ddMMMyyyy") + ".log";
                Directory.CreateDirectory(Path.GetDirectoryName(logfilePath));
                FileStream fs = new FileStream(logfilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
                StreamWriter sw = new StreamWriter(fs);
                sw.BaseStream.Seek(0, SeekOrigin.End);
                sw.WriteLine(DateTime.Now.ToString() + ": " + content);
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                ex.InnerException.ToString();
            }
        }
        public static void ErrorMsg(string ContorllerName,string method, string Error)
        {
            writeLogPriority("Controller :" + ContorllerName + "Method Name:-" + method + " : " + Error);
        }
    }
}
