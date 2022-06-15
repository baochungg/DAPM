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

public partial class _Default : System.Web.UI.Page
{
    private MDMenuBussines md_MBus;
    protected void Page_Load(object sender, EventArgs e)
    {
        switch (Request.QueryString.Count)
        {
            case 1:
                {
                    if (Request.QueryString.Keys[0].ToLower() == "mainmenuid")
                    {
                        string menuID = Request.QueryString[0];
                        LoadMenu(menuID);
                    }
                    break;
                }
            default:
                {
                    LoadMenu(null);
                    break;
                }
        }
    }
    protected void LoadMenu(string parentMenuID)
    {
        if (md_MBus == null) md_MBus = new MDMenuBussines();
        this.rpTabView_Menu.DataSource = md_MBus.LoadSubMenuView(parentMenuID);
        this.rpTabView_Menu.DataBind();
    }
    protected void lbMenuName_Click(object sender, EventArgs e)
    {
        Response.Redirect(((LinkButton)sender).CommandArgument);
    }
}
