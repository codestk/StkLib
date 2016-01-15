using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StkLib.Web.Controls.StkGridView
{
    public class StkGvEvent : System.Web.UI.Page
    {


       virtual protected void BindData()
        {

        }


       virtual protected void BtnSearch_Click(object sender, EventArgs e)
       {
           BindData();
       }

       protected void Refresh_Click(object sender, EventArgs args)
       {
           //update the grids contents
           BindData();
       }



       protected string SortExpression
        {
            get
            {
                return ViewState["SortExpression"] as string;
            }
            set
            {
                ViewState["SortExpression"] = value;
            }
        }

       protected SortDirection SortDirection
        {
            get
            {
                object o = ViewState["SortDirection"];
                if (o == null)
                    return SortDirection.Ascending;
                return (SortDirection)o;
            }
            set
            {
                ViewState["SortDirection"] = value;
            }
        }

        protected virtual  void GvCustomers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            var gridView = (GridView)sender;

         

            if (e.Row.RowType == DataControlRowType.Header)
            {
                int cellIndex = -1;
                foreach (DataControlField field in gridView.Columns)
                {

                    if (field.SortExpression != "")
                    {
                        e.Row.Cells[gridView.Columns.IndexOf(field)].CssClass = "headerstyle";
                    }
                    //if (field.SortExpression == gridView.SortExpression)
                    if (field.SortExpression == SortExpression)
                    {
                        cellIndex = gridView.Columns.IndexOf(field);
                        break;
                        
                    }
                }

                if (cellIndex > -1)
                {
                    //  this is a header row,
                    //  set the sort style
                    e.Row.Cells[cellIndex].CssClass =
                        SortDirection == SortDirection.Ascending
                         ? "sortdescheaderstyle" : "sortascheaderstyle";
                        //? "sortascheaderstyle" : "sortdescheaderstyle";
                         
                }
            }

       


        }


        protected void gvCustomers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            var gv = (GridView)sender;
            gv.PageIndex = e.NewPageIndex;
            gv.EditIndex = -1;
            gv.SelectedIndex = -1;

            //gvCustomers.PageIndex = e.NewPageIndex;
            //gvCustomers.EditIndex = -1;
            //gvCustomers.SelectedIndex = -1;
        }

        protected void gvCustomers_PageIndexChanged(object sender, EventArgs e)
        {
            BindData();
        }


        protected virtual void gvCustomers_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (SortExpression == e.SortExpression)
            {
                SortDirection = SortDirection == SortDirection.Ascending ?
                    SortDirection.Descending : SortDirection.Ascending;

            }
            else
            {
                SortDirection = SortDirection.Ascending;
            }
            SortExpression = e.SortExpression;

            var gv = (GridView)sender;
            gv.EditIndex = -1;
            gv.SelectedIndex = -1;
        }

        protected virtual void gvCustomers_Sorted(object sender, EventArgs e)
        {
            BindData();
        }


        protected virtual void SetTableBootsStab(GridView gvView)
        {
            gvView.UseAccessibleHeader = true;
            gvView.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

    

        //For PRint
        protected virtual void GridviewAllPages(GridView gvView, Button btnNameForRefresh)
        {

            gvView.AllowPaging = false;

            BindData();

            string buttonname = btnNameForRefresh.ClientID;
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "PrintPanel(); $('#" + buttonname + "').click();", true);

        }

    }
}