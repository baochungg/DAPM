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

public partial class NatureText : System.Web.UI.Page
{
    private MDTextSecurityBussines md_TSBus;
    private MDTextLevelBussines md_TLBus;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["ACOUNT"] != null)
        {
            LOGIN.Acount acount = Session["ACOUNT"] as LOGIN.Acount;
            if (acount != null)
            {
                if (!acount.IsAdminAcount)
                {
                    if (!acount.OwnerTextLogged.OwnerTextLogged.TextLevelAdmin)
                    {
                        lbMsg_TextLevel.Text = "<span style=\"color:red;font-weight:bold;\">Bạn không có quyền sử dụng chức năng này! Có thể nhấn " +
                            "<a href=\"javascript:history.back();\">back</a> để quay lại.</span>";
                        hdTextLevelID.Value = "NotOwner";
                    }
                    else if (!IsPostBack)
                        LoadTextLevelData();
                    if (!acount.OwnerTextLogged.OwnerTextLogged.TextSecurityAdmin)
                    {
                        lbMsg_TextSecurity.Text = "<span style=\"color:red;font-weight:bold;\">Bạn không có quyền sử dụng chức năng này! Có thể nhấn " +
                            "<a href=\"javascript:history.back();\">back</a> để quay lại.</span>";
                        hdTextSecurityID.Value = "NotOwner";
                    }
                    else if (!IsPostBack)
                        LoadTextSecurityData();
                }
                else if (!IsPostBack)
                {
                    LoadTextLevelData();
                    LoadTextSecurityData();
                }
            }
        }
    }
    #region TextLevel
    private void LoadTextLevelData()
    {
        if (md_TLBus == null) md_TLBus = new MDTextLevelBussines();
        int count;
        var tl = md_TLBus.LoadTextLevel(0, 10, out count);
        int numPage = count / 10;
        if (count % 10 != 0) numPage += 1;
        for (int i = 1; i <= numPage; i++)
            this.drPage_TextLevel.Items.Add(i.ToString());
        this.grTextLevel.DataSource = tl;
        this.grTextLevel.DataBind();
    }
    private void UpdateGridviewTextLevel()
    {
        if (md_TLBus == null) md_TLBus = new MDTextLevelBussines();
        int count;
        int cPage = int.Parse(drPage_TextLevel.Text);
        var tl = md_TLBus.LoadTextLevel(cPage * 10 - 10, cPage * 10, out count);
        this.grTextLevel.DataSource = tl;
        this.grTextLevel.DataBind();
        this.updatepanel_TextLevel.Update();
    }
    private void ResetControlTextLevel()
    {
        this.txtTextLevelName.Text = "";
        this.txtTextLevelPoint.Text = "";
        this.txtTextLevelDescription.Text = "";
        this.grTextLevel.SelectedIndex = -1;
        this.hdTextLevelID.Value = "0";
        this.btAddTextLevel.ImageUrl = "/ManagerDispatch/Images/Icon/bt_add_new.png";
    }
    protected void btAddTextLevel_Click(object sender, EventArgs e)
    {
        if (md_TLBus == null) md_TLBus = new MDTextLevelBussines();
        if (this.hdTextLevelID.Value == "0")
        {
            md_TLBus.InsertTextLevel(this.txtTextLevelName.Text, int.Parse(this.txtTextLevelPoint.Text), this.txtTextLevelDescription.Text);
            lbMsg_TextLevel.Text = "<span style=\"color:blue;\">Đã thêm mức độ văn bản!</span>";
        }
        else
        {
            md_TLBus.UpdateTextLevel(this.hdTextLevelID.Value, this.txtTextLevelName.Text, int.Parse(this.txtTextLevelPoint.Text), this.txtTextLevelDescription.Text);
            lbMsg_TextLevel.Text = "<span style=\"color:blue;\">Đã chỉnh sửa thông tin mức độ văn bản!</span>";
        }
        ResetControlTextLevel();
        UpdateGridviewTextLevel();
    }
    protected void btCancelTextLevel_Click(object sender, EventArgs e)
    {
        ResetControlTextLevel();
        this.lbMsg_TextLevel.Text = "";
    }
    protected void grTextLevel_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "this.originalstyle = this.style.backgroundColor;if(this.originalstyle != 'rgb(204, 255, 153)'){this.style.backgroundColor='#F4F4F4'}");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor = this.originalstyle");
            e.Row.ToolTip = grTextLevel.DataKeys[e.Row.RowIndex].Value.ToString();
        }
    }
    protected void imgBtEdit_TextLevel_Click(object sender, EventArgs e)
    {
        if (md_TLBus == null) this.md_TLBus = new MDTextLevelBussines();
        string textLevelID = ((ImageButton)sender).CommandArgument;
        this.hdTextLevelID.Value = textLevelID;
        this.btAddTextLevel.ImageUrl = "/ManagerDispatch/Images/Icon/bt_update.png";
        int rowidx = int.Parse(((ImageButton)sender).CommandName);
        this.grTextLevel.SelectedIndex = rowidx - (int.Parse(drPage_TextLevel.Text) - 1) * 10 - 1;
        LINQ.TextLevel tl = md_TLBus.GetTextLevelInfo(textLevelID);
        this.txtTextLevelName.Text = tl.TextLevelName;
        this.txtTextLevelPoint.Text = tl.TextLevelPoint.ToString();
        this.txtTextLevelDescription.Text = tl.TextLevelDescription;
        lbMsg_TextLevel.Text = "";
    }
    protected void imgBtDelete_TextLevel_Click(object sender, EventArgs e)
    {
        if (md_TLBus == null) this.md_TLBus = new MDTextLevelBussines();
        string textLevelID = ((ImageButton)sender).CommandArgument;
        int textCount;
        md_TLBus.DeleteTextLevel(textLevelID, out textCount);
        lbMsg_TextLevel.Text = "<span style=\"color:blue;\">Đã xóa mức độ văn bản cộng với <font style=\"color:red;\">" + textCount + "</font> văn bản liên quan!</span>";
        ResetControlTextLevel();
        UpdateGridviewTextLevel();
    }
    protected void drPage_TextLevel_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateGridviewTextLevel();
        ResetControlTextLevel();
        this.lbMsg_TextLevel.Text = "";
    }
    #endregion
    #region TextSecurity
    private void LoadTextSecurityData()
    {
        if (md_TSBus == null) md_TSBus = new MDTextSecurityBussines();
        int count;
        var ts = md_TSBus.LoadTextSecurity(0, 10, out count);
        int numPage = count / 10;
        if (count % 10 != 0) numPage += 1;
        for (int i = 1; i <= numPage; i++)
            this.drPage_TextSecurity.Items.Add(i.ToString());
        this.grTextSecurity.DataSource = ts;
        this.grTextSecurity.DataBind();
    }
    private void UpdateGridviewTextSecurity()
    {
        if (md_TSBus == null) md_TSBus = new MDTextSecurityBussines();
        int count;
        int cPage = int.Parse(drPage_TextSecurity.Text);
        var ts = md_TSBus.LoadTextSecurity(cPage * 10 - 10, cPage * 10, out count);
        this.grTextSecurity.DataSource = ts;
        this.grTextSecurity.DataBind();
        this.updatepanel_TextSecurity.Update();
    }
    private void ResetControlTextSecurity()
    {
        this.txtTextSecurityName.Text = "";
        this.txtTextSecurityNote.Text = "";
        this.grTextSecurity.SelectedIndex = -1;
        this.hdTextSecurityID.Value = "0";
        this.btAddTextSecurity.ImageUrl = "/ManagerDispatch/Images/Icon/bt_add_new.png";
    }
    protected void btAddTextSecurity_Click(object sender, EventArgs e)
    {
        if (md_TSBus == null) md_TSBus = new MDTextSecurityBussines();
        if (this.hdTextSecurityID.Value == "0")
        {
            md_TSBus.InsertTextSecurity(this.txtTextSecurityName.Text, this.txtTextSecurityNote.Text);
            lbMsg_TextSecurity.Text = "<span style=\"color:blue;\">Đã thêm độ bảo mật văn bản!</span>";
        }
        else
        {
            md_TSBus.UpdateTextSecurity(this.hdTextSecurityID.Value, this.txtTextSecurityName.Text, this.txtTextSecurityNote.Text);
            lbMsg_TextSecurity.Text = "<span style=\"color:blue;\">Đã chỉnh sửa thông tin độ bảo mật văn bản!</span>";
        }
        ResetControlTextSecurity();
        UpdateGridviewTextSecurity();
    }
    protected void btCancelTextSecurity_Click(object sender, EventArgs e)
    {
        ResetControlTextSecurity();
        this.lbMsg_TextSecurity.Text = "";
    }
    protected void grTextSecurity_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "this.originalstyle = this.style.backgroundColor;if(this.originalstyle != 'rgb(204, 255, 153)'){this.style.backgroundColor='#F4F4F4'}");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor = this.originalstyle");
            e.Row.ToolTip = grTextSecurity.DataKeys[e.Row.RowIndex].Value.ToString();
        }
    }
    protected void imgBtEdit_TextSecurity_Click(object sender, EventArgs e)
    {
        if (md_TSBus == null)
            this.md_TSBus = new MDTextSecurityBussines();
        string textSecurityID = ((ImageButton)sender).CommandArgument;
        this.hdTextSecurityID.Value = textSecurityID;
        this.btAddTextSecurity.ImageUrl = "/ManagerDispatch/Images/Icon/bt_update.png";
        int rowidx = int.Parse(((ImageButton)sender).CommandName);
        this.grTextSecurity.SelectedIndex = rowidx - (int.Parse(drPage_TextSecurity.Text) - 1) * 10 - 1;
        LINQ.TextSecurity ts = md_TSBus.GetTextSecurityInfo(textSecurityID);
        this.txtTextSecurityName.Text = ts.TextSecurityName;
        this.txtTextSecurityNote.Text = ts.Note;
        lbMsg_TextSecurity.Text = "";
    }
    protected void imgBtDelete_TextSecurity_Click(object sender, EventArgs e)
    {
        if (md_TSBus == null) this.md_TSBus = new MDTextSecurityBussines();
        string textSecurityID = ((ImageButton)sender).CommandArgument;
        int textCount;
        md_TSBus.DeleteTextSecurity(textSecurityID, out textCount);
        lbMsg_TextSecurity.Text = "<span style=\"color:blue;\">Đã xóa bảo mật văn bản cộng với <font style=\"color:red;\">" + textCount + "</font> văn bản liên quan!</span>";
        ResetControlTextSecurity();
        UpdateGridviewTextSecurity();
    }
    protected void drPage_TextSecurity_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateGridviewTextSecurity();
        ResetControlTextSecurity();
        this.lbMsg_TextSecurity.Text = "";
    }
    #endregion
}
