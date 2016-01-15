using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
//How To Use
//protected void Application_Error(object sender, EventArgs e)
//      {
//          // find out the last error
//         System.Exception  ex = Server.GetLastError().GetBaseException();
//          // log the error now
//         ErrorLogging.LogError(ex, ""); 
//      }
using System.Web;

namespace StkLib.Errors
{
  
   

    public class ErrorLogging
    {

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="ee">The ee.</param>
        /// <param name="userFriendlyError">The user friendly error.</param>
        /// <returns></returns>
        public static string LogError(Exception ee, string userFriendlyError)
        {
            string logType = ConfigurationManager.AppSettings["ErrorLogType"];
            if (logType.Equals("1"))
            {
                return LogErrorToLogFile(ee, userFriendlyError);
            }
            return LogErrorInSystemEvent(ee, userFriendlyError);
        }

        /// <summary>
        /// Log the error and return
        /// </summary>
        /// <param name="ee">The ee.</param>
        /// <param name="userFriendlyError">The user friendly error.</param>
        /// <returns></returns>
        public static string LogErrorToLogFile(Exception ee, string userFriendlyError)
        {
            string path = HttpContext.Current.Server.MapPath("~/ErrorLogging/");
            // check if directory exists
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path = path + DateTime.Today.ToString("dd-MM-yy") + ".log";
            // check if file exist
            if (!File.Exists(path))
            {
                File.Create(path).Dispose();
            }
            // log the error now
            using (StreamWriter writer = File.AppendText(path))
            {
                string error = "\r\nLog written at : " + DateTime.Now +
                               "\r\nError occured on page : " + HttpContext.Current.Request.Url +
                               "\r\n\r\nHere is the actual error :\n" + ee;
                writer.WriteLine(error);
                writer.WriteLine("==========================================");
                writer.Flush();
                writer.Close();
            }
            return userFriendlyError;
        }

        /// <summary>
        /// Logs the error in system event.
        /// </summary>
        /// <param name="ee">The ee.</param>
        /// <param name="userFriendlyError">The user friendly error.</param>
        /// <returns></returns>
        public static string LogErrorInSystemEvent(Exception ee, string userFriendlyError)
        {
            const string eventLog = "Sample1Error1";
            const string eventSource = "ErrorLoggingSampleApp1";

            // check if source exists
            if (!EventLog.SourceExists(eventSource))
            {
                EventLog.CreateEventSource(eventSource, eventLog);
            }

            // create the instance of the EventLog and log the error
            using (var myLog = new EventLog(eventLog))
            {
                myLog.Source = eventSource;

                string error = "\r\nLog written at : " + DateTime.Now +
                               "\r\nError occured on page : " + HttpContext.Current.Request.Url +
                               "\r\n\nHere is the actual error :\n" + ee;
                myLog.WriteEntry(error, EventLogEntryType.Error);
            }

            return userFriendlyError;
        }
    }
}


