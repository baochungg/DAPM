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

public partial class ManagementDispatchFileAttachment : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        bool flag = false;
        if (Request.QueryString.Keys.Count == 1)
        {
            if (Request.QueryString.Keys[0].ToLower() == "fid")
            {
                flag = true;
                MDFileAttachment md_FABus = new MDFileAttachment();
                LINQ.FileAttachment f = md_FABus.GetFile(Request.QueryString[0]);
                Response.Clear();
                Response.ContentType = f.ContentType;
                Response.AddHeader("Content-Disposition", "attachment; filename=" + f.FileName);
                byte[] fData = f.FileData.ToArray();
                System.IO.BinaryWriter bw = new System.IO.BinaryWriter(Response.OutputStream);
                bw.Write(fData);
                bw.Close();
                Response.OutputStream.Write(fData, 0, (int)f.FileSize);
                Response.End();
            }
        }
        if (!flag) Response.Write("Error Page!");
    }
}
