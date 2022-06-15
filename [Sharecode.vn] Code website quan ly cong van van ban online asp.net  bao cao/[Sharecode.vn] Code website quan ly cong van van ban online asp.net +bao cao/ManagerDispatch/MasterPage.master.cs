using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using LINQ;


public partial class MasterPage : System.Web.UI.MasterPage
{
    private MDMenuBussines md_MBus;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDataTest();
            if (Session["ACOUNT"] != null)
            {
                LOGIN.Acount acount = Session["ACOUNT"] as LOGIN.Acount;
                lbUserInfo.Text = acount.StaffLogged.StaffName;
            }
            else
            {
                Response.Redirect("/ManagerDispatch/Login.aspx");
            }
        }
        switch (Request.QueryString.Count)
        {
            case 1:
                {
                    switch (Request.QueryString.Keys[0].ToLower())
                    {
                        case "mainmenuid":
                        case "parentmenuid":
                            {
                                string menuID = Request.QueryString[0];
                                LoadMainTab(menuID);
                                break;
                            }
                    }
                    break;
                }
            default:
                {
                    LoadMainTab(null);
                    break;
                }
        }
    }
    protected void LoadDataTest()
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("STT");
        dt.Columns.Add("docID");
        dt.Columns.Add("date");
        DataRow dr;
        for (int i = 1; i <= 9; i++)
        {
            dr = dt.NewRow();
            dr[0] = i;
            dr[1] = "VB00" + i;
            dr[2] = "0" + 9 + "/10/2011";
            dt.Rows.Add(dr);
        }
        this.grNewDocTo.DataSource = dt;
        this.grNewDocTo.DataBind();
        this.grNewDocFrom.DataSource = dt;
        this.grNewDocFrom.DataBind();
        DataTable dtNewWork = new DataTable();
        dtNewWork.Columns.Add("STT");
        dtNewWork.Columns.Add("nameWork");
        dr = dtNewWork.NewRow();
        dr[0] = "1";
        dr[1] = "Họp - (21/10/2011)";
        dtNewWork.Rows.Add(dr);
        dr = dtNewWork.NewRow();
        dr[0] = "2";
        dr[1] = "Hoàn thành MTP - (21/10/2011)";
        dtNewWork.Rows.Add(dr);
        this.grNewWork.DataSource = dtNewWork;
        this.grNewWork.DataBind();
    }
    protected void LoadMainTab(string menuID)
    {
        if (md_MBus == null)
            md_MBus = new MDMenuBussines();
        this.rpTabMenu.DataSource = md_MBus.LoadMainTab(menuID);
        this.rpTabMenu.DataBind();
    }
    protected void lbTabView_Click(object sender, EventArgs e)
    {
        LinkButton lb = sender as LinkButton;
        if (lb != null)
        {
            Response.Redirect("/ManagerDispatch/Default.aspx?mainmenuid=" + lb.CommandArgument.ToString());
        }
    }
    protected void imgAcount_Click(object sender, EventArgs e)
    {
        Session["ACOUNT"] = null;
        Response.Redirect("/ManagerDispatch/Login.aspx");
    }
}
