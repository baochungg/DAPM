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

public partial class OtherContact : System.Web.UI.Page
{
    private MDOtherContactBussines md_OCBus;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["ACOUNT"] != null)
        {
            LOGIN.Acount acount = Session["ACOUNT"] as LOGIN.Acount;
            if (acount != null)
            {
                if (!acount.IsAdminAcount)
                {
                    if (!acount.OwnerTextLogged.OwnerTextLogged.OtherContactAdmin)
                    {
                        lbMsg_OtherContact.Text = "<span style=\"color:red;font-weight:bold;\">Bạn không có quyền truy cập vào trang này! Vui lòng nhấn " +
                            "<a href=\"javascript:history.back();\">back</a> để quay lại.</span>";
                        hdDepartmentAddressID.Value = "NotOwner";
                    }
                    else
                        if (!IsPostBack) LoadDeparmentAddress();
                }
                else
                    if (!IsPostBack) LoadDeparmentAddress();
            }
        }
    }
    private void LoadDeparmentAddress()
    {
        if (md_OCBus == null) md_OCBus = new MDOtherContactBussines();
        int count;
        var oc = md_OCBus.LoadOtherContact(0, 10, out count);
        int numPage = count / 10;
        if (count % 10 != 0) numPage += 1;
        for (int i = 1; i <= numPage; i++)
            this.drPage_OtherContact.Items.Add(i.ToString());
        this.grOtherContact.DataSource = oc;
        this.grOtherContact.DataBind();
    }
    private void UpdateGridViewOtherContact()
    {
        if (md_OCBus == null) md_OCBus = new MDOtherContactBussines();
        int count;
        int cPage = int.Parse(drPage_OtherContact.Text);
        var tl = md_OCBus.LoadOtherContact(cPage * 10 - 10, cPage * 10, out count);
        this.grOtherContact.DataSource = tl;
        this.grOtherContact.DataBind();
        this.updatepanel_OtherContact.Update();
    }
    private void ResetControlOtherContact()
    {
        this.txtDepartmentAddressName.Text = "";
        this.txtNote.Text = "";
        this.grOtherContact.SelectedIndex = -1;
        this.hdDepartmentAddressID.Value = "0";
        this.btAddOtherContact.ImageUrl = "/ManagerDispatch/Images/Icon/bt_add_new.png";
    }
    protected void btAddOtherContact_Click(object sender, EventArgs e)
    {
        if (md_OCBus == null) md_OCBus = new MDOtherContactBussines();
        if (hdDepartmentAddressID.Value == "0")
        {
            md_OCBus.InsertDepartmentAddress(this.txtDepartmentAddressName.Text, this.txtNote.Text);
            lbMsg_OtherContact.Text = "<span style=\"color:blue;\">Đã thêm đơn vị!</span>";
        }
        else
        {
            md_OCBus.UpdateDepartmentAddress(this.hdDepartmentAddressID.Value, this.txtDepartmentAddressName.Text, this.txtNote.Text);
            lbMsg_OtherContact.Text = "<span style=\"color:blue;\">Đã chỉnh sửa thông tin đơn vị!</span>";
        }
        ResetControlOtherContact();
        UpdateGridViewOtherContact();
    }
    protected void btCancelOtherContact_Click(object sender, EventArgs e)
    {
        ResetControlOtherContact();
        this.lbMsg_OtherContact.Text = "";
    }
    protected void grOtherContact_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "this.originalstyle = this.style.backgroundColor;if(this.originalstyle != 'rgb(204, 255, 153)'){this.style.backgroundColor='#F4F4F4'}");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor = this.originalstyle");
            e.Row.ToolTip = grOtherContact.DataKeys[e.Row.RowIndex].Value.ToString();
        }
    }
    protected void imgBtEdit_OtherContact_Click(object sender, EventArgs e)
    {
        if (md_OCBus == null) this.md_OCBus = new MDOtherContactBussines();
        string dAddr = ((ImageButton)sender).CommandArgument;
        this.hdDepartmentAddressID.Value = dAddr;
        this.btAddOtherContact.ImageUrl = "/ManagerDispatch/Images/Icon/bt_update.png";
        int rowidx = int.Parse(((ImageButton)sender).CommandName);
        this.grOtherContact.SelectedIndex = rowidx - (int.Parse(drPage_OtherContact.Text) - 1) * 10 - 1;
        LINQ.DepartmentAddress da = md_OCBus.GetOtherContactInfo(dAddr);
        if (da != null)
        {
            this.txtDepartmentAddressName.Text = da.DepartmentAddressName;
            this.txtNote.Text = da.Note;
            lbMsg_OtherContact.Text = "";
        }
    }
    protected void imgBtDelete_OtherContact_Click(object sender, EventArgs e)
    {
        if (md_OCBus == null) md_OCBus = new MDOtherContactBussines();
        string departmentAddressID = ((ImageButton)sender).CommandArgument;
        int textCount;
        md_OCBus.DeleteDepartmentAddress(departmentAddressID, out textCount);
        lbMsg_OtherContact.Text = "<span style=\"color:blue;font-weight:bold;\">Đã xóa đơn vị với <font style=\"color:red;\">" + textCount + "</font> văn bản liên quan đến đơn vị này!</span>";
        ResetControlOtherContact();
        UpdateGridViewOtherContact();
    }
    protected void drPage_OtherContact_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateGridViewOtherContact();
        ResetControlOtherContact();
        this.lbMsg_OtherContact.Text = "";
    }
}
