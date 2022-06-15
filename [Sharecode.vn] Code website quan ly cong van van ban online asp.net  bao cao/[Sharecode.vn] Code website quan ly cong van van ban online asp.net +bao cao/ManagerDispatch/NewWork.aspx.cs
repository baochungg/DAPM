using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using LINQ;

public partial class NewWork : System.Web.UI.Page
{
    private MDCalendarWorkBussines md_CalendarBus;
    private MDNewWork md_newWorkBus;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
            LoadlistWork();
    }
    public void LoadlistWork()
    {
        int count;
        md_newWorkBus = new MDNewWork();
        LOGIN.Acount acount = Session["ACCOUNT"] as LOGIN.Acount;
        //if(acount.
        //{
            var work = md_newWorkBus.LoadWork( 0, 10, out count);
            this.grCalendar.DataSource = work;
            this.grCalendar.DataBind();
        //}
    }
    public void imbSua_calendar_Click()
    {
        md_CalendarBus = new MDCalendarWorkBussines();
        Response.Redirect("/ManagerDispatch/CalendarWork.aspx");
    }
}
