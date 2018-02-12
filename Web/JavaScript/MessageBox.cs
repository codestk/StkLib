using System;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.UI;

namespace StkLib.Web.JavaScript
{
    /// <summary>
    /// MessageBox is only to be used in Web Applications! Does not have to be instantiated!
    /// </summary>
    /// <remarks>
    /// Enables the use of a web message box by simply typing WilsToolbox.MessageBox.Show("message")
    /// </remarks>
    public class MessageBox
    {

        //  public MessageBox() { }


        /// <summary>
        /// This will contains the PageName[QueuedMessage]
        /// </summary>
        private static readonly Hashtable MExecutingPages = new Hashtable();

        /// <summary>
        /// Pass a string through this method to display a javascript popup
        /// </summary>
        /// <remarks>
        /// Identify the calling Page, use it as the Hashtable Key, and put the Queue in as the value       
        /// For use with Web Applications Only
        /// </remarks>
        public static void Show(string sMessage)
        {
            // If this is the first time a page has called this method then
            if (!MExecutingPages.Contains(HttpContext.Current.Handler))
            {
                // Attempt to cast HttpHandler as a Page.
                // Simply put, this sets the current page as the executing page
                var executingPage = HttpContext.Current.Handler as Page;
                // error correction; ensure the page was successfully identified
                if (executingPage != null)
                {
                    // Create a Queue to hold one or more messages.
                    var messageQueue = new Queue();

                    // Add our message to the Queue
                    messageQueue.Enqueue(sMessage);

                    // Add our message queue to the hash table. Use our page reference
                    // (IHttpHandler) as the key.
                    // this will ensure that all messages put here will be displayed
                    // in order of placement, to the page that called it.
                    MExecutingPages.Add(HttpContext.Current.Handler, messageQueue);

                    // Wire up Unload event so that we can inject 
                    // some JavaScript for the alerts.
                    // simply put; set the method we have as the event handler
                    // for the calling pages Unload event (after page has rendered)
                    //executingPage.Unload += new EventHandler(ExecutingPage_Unload);
                    executingPage.Unload += ExecutingPage_Unload;
                }
            }
            else
            {
                // We have already been here previously, and the Show() method has already been 
                // called from this particular Executing Page.
                // Therefore we have already created a message queue in memory and have already
                // stored a reference to it in our hashtable. 
                // So, lets add a new entry for this executing page
                var queue = (Queue) MExecutingPages[HttpContext.Current.Handler];

                // And add our latest message to the Queue
                queue.Enqueue(sMessage);
            }
        }

        /// <summary>
        /// Pass a string through this method to display a javascript popup
        /// </summary>
        /// <remarks>
        /// Our page has finished rendering so lets output the JavaScript to produce the alert's
        /// </remarks>
        private static void ExecutingPage_Unload(object sender, EventArgs e)
        {
            // Get our message queue from the hashtable
            var queue = (Queue) MExecutingPages[HttpContext.Current.Handler];

            if (queue != null)
            {
                var sb = new StringBuilder();

                // How many messages have been registered?
                int iMsgCount = queue.Count;

                // Use StringBuilder to build up our client slide JavaScript.
                sb.Append("&lt;script language='javascript'>");

                // Loop round registered messages
                while (iMsgCount-- > 0)
                {
                    string sMsg = (string) queue.Dequeue();
                    sMsg = sMsg.Replace("\n", "\\n");
                    sMsg = sMsg.Replace("\"", "'");
                    sb.Append(@"alert( """ + sMsg + @""" );");
                }

                // Close our JS
                sb.Append(@"</" + "script>");


                // Were done, so remove our page reference from the hashtable
                MExecutingPages.Remove(HttpContext.Current.Handler);

                // Write the JavaScript to the end of the response stream.
                HttpContext.Current.Response.Write(sb.ToString());
            }
        }



        /// <summary>
        /// Shows a client-side JavaScript alert in the browser.
        /// </summary>
        /// <param name="message">The message to appear in the alert.</param>
        public static void Alert(string message)
        {
            // Cleans the message to allow single quotation marks
            string cleanMessage = message.Replace("'", "\\'");
            string script = "<script type=\"text/javascript\">$(document).ready(function(){alert('" + cleanMessage + "');});</script>";
            // Gets the executing web page
            var page = HttpContext.Current.CurrentHandler as Page;
            // Checks if the handler is a Page and that the script isn't all ready on the Page
            if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
            {
                page.ClientScript.RegisterClientScriptBlock(typeof(MessageBox), "MessageBox", script);
            }


        }
    }
}