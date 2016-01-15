using System.Web;
using System.Web.UI;

namespace StkLib.Web.Controls.Form
{
    public class StkAlert
    {


        public static void Show(string message)
        {
            // Cleans the message to allow single quotation marks
            string cleanMessage = message.Replace("'", "\\'");
            string script = "<script type=\"text/javascript\">alert('" + cleanMessage + "');</script>";

            // Gets the executing web page
            var page = HttpContext.Current.CurrentHandler as Page;

            // Checks if the handler is a Page and that the script isn't allready on the Page
            if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
            {
                page.ClientScript.RegisterClientScriptBlock(typeof(StkAlert), "alert", script,true);
            }
        }


        public static void ShowAjax(string message,Control ct)
        {
            // Checks if the handler is a Page and that the script isn't allready on the Page
            //if (page != null && !page.ClientScript.IsClientScriptBlockRegistered("alert"))
            //{
                ScriptManager.RegisterClientScriptBlock(ct, typeof(StkAlert), "alert", "alert('" + message + "');", true);
            //}

        }



     
  

    }
}