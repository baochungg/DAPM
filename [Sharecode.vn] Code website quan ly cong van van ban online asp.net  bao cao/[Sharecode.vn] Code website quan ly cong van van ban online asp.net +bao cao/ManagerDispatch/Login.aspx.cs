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

public partial class Login : System.Web.UI.Page
{
    private MDLoginBussines md_LBus;
    protected void Page_Load(object sender, EventArgs e)
    {
        Page.SetFocus(txtUserName);
    }
    protected void lbLogin_Click(object sender, EventArgs e)
    {
        if (md_LBus == null) md_LBus = new MDLoginBussines();
        string message;
        LINQ.Acount ac;
        if (!md_LBus.Login(this.txtUserName.Text, this.txtPassword.Text, out message, out ac))
        {
            lbMsg.Text = message;
        }
        else
        {
            Session["ACOUNT"] = new LOGIN.Acount(ac);
            Response.Redirect("/ManagerDispatch/Default.aspx");
        }
    }
}
