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

public partial class Default3 : System.Web.UI.Page
{
    private MDDepartmentBussines md_DBus;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["ACOUNT"] != null)
        {
            LOGIN.Acount acount = Session["ACOUNT"] as LOGIN.Acount;
            if (acount != null)
            {
                if (!acount.IsAdminAcount)
                {
                    if (acount.OwnerAdminLogged.NotOwnerDepartment)
                    {
                        lbMsg.InnerHtml = "<span style=\"color:red;font-weight:bold;\">Bạn không có quyền truy cập vào trang này! Vui lòng nhấn " +
                            "<a href=\"javascript:history.back();\">back</a> để quay lại.</span>";
                        hdDepartmentID.Value = "NotOwner";
                    }
                    else
                        if (!IsPostBack) LoadData();
                }
                else
                    if (!IsPostBack) LoadData();
            }
        }
    }
    protected void LoadData()
    {
        LOGIN.Acount acount = Session["ACOUNT"] as LOGIN.Acount;
        if (md_DBus == null) md_DBus = new MDDepartmentBussines();
        DataTable dtDeparment = new DataTable();
        dtDeparment.Columns.Add("STT");
        dtDeparment.Columns.Add("DepartmentID");
        dtDeparment.Columns.Add("DepartmentName");
        dtDeparment.Columns.Add("StaffCount");
        dtDeparment.Columns.Add("BookCount");
        int count = 0;
        if (acount.IsAdminAcount)
            md_DBus.LoadDepartment(ref this.drParentDepartment, ref dtDeparment, 1, 10, ref count);
        else
            md_DBus.LoadDepartment(acount.StaffLogged.DepartmentID.ToString(), ref this.drParentDepartment, ref dtDeparment, 1, 10, ref count);
        int numPage = count / 10;
        if (count % 10 != 0) numPage += 1;
        for (int i = 1; i <= numPage; i++)
            this.drPage.Items.Add(i.ToString());
        this.grDepartment.DataSource = dtDeparment;
        this.grDepartment.DataBind();
    }
    protected void UpdateGridView()
    {
        LOGIN.Acount acount = Session["ACOUNT"] as LOGIN.Acount;
        int count = 0;
        int cPage = int.Parse(drPage.Text);
        md_DBus = new MDDepartmentBussines();
        DataTable dtDeparment = new DataTable();
        dtDeparment.Columns.Add("STT");
        dtDeparment.Columns.Add("DepartmentID");
        dtDeparment.Columns.Add("DepartmentName");
        dtDeparment.Columns.Add("StaffCount");
        dtDeparment.Columns.Add("BookCount");
        if (acount.IsAdminAcount)
            md_DBus.LoadDepartment(ref this.drParentDepartment, ref dtDeparment, 10 * cPage - 9, 10 * cPage, ref count);
        else
            md_DBus.LoadDepartment(acount.StaffLogged.DepartmentID.ToString(), ref this.drParentDepartment, ref dtDeparment, 10 * cPage - 9, 10 * cPage, ref count);
        this.grDepartment.DataSource = dtDeparment;
        this.grDepartment.DataBind();
        this.uplDepartment.Update();
    }
    protected void ResetControl()
    {
        this.txtDepartment.Text = "";
        this.drParentDepartment.SelectedIndex = 0;
        this.hdDepartmentID.Value = "0";
        this.btAdd.ImageUrl = "/ManagerDispatch/Images/Icon/bt_add_new.png";
        this.grDepartment.SelectedIndex = -1;
    }
    protected void imgBtEdit_Click(object sender, EventArgs e)
    {
        LOGIN.Acount acount = Session["ACOUNT"] as LOGIN.Acount;
        if (acount != null)
        {
            if (acount.IsAdminAcount || acount.OwnerAdminLogged.OwnerAdminLogged.DepartmentModify)
            {
                this.md_DBus = new MDDepartmentBussines();
                string deparmentID = ((ImageButton)sender).CommandArgument;
                this.hdDepartmentID.Value = deparmentID;
                this.btAdd.ImageUrl = "/ManagerDispatch/Images/Icon/bt_update.png";
                string departmentName, departmentParentID;
                md_DBus.GetDepartmentInfo(deparmentID, out departmentName, out departmentParentID);
                this.drParentDepartment.SelectedValue = departmentParentID;
                this.txtDepartment.Text = departmentName;
                int rowidx = int.Parse(((ImageButton)sender).CommandName);
                this.grDepartment.SelectedIndex = rowidx - (int.Parse(drPage.Text) - 1) * 10 - 1;
                lbMsg.InnerHtml = "";
            }
            else
                lbMsg.InnerHtml = "<span style=\"color:red;font-weight:bold;\">Tài khoản của bạn không có quyền thực hiện thao tác này!</span>";
        }
        else Response.Redirect("/ManagerDispatch/Login.aspx");
    }
    protected void imgBtDelete_Click(object sender, EventArgs e)
    {
        LOGIN.Acount acount = Session["ACOUNT"] as LOGIN.Acount;
        if (acount != null)
        {
            if (acount.IsAdminAcount || acount.OwnerAdminLogged.OwnerAdminLogged.DepartmentDelete)
            {
                md_DBus = new MDDepartmentBussines();
                if (md_DBus.DeleteDepartment(((ImageButton)sender).CommandArgument))
                {
                    lbMsg.Style["color"] = "blue";
                    lbMsg.InnerHtml = "Đã xóa phòng ban!";
                    ResetControl();
                    UpdateGridView();
                }
                else
                {
                    lbMsg.Style["color"] = "red";
                    lbMsg.InnerHtml = "Bạn phải xóa hết nhân viên và sổ VB thuộc phòng ban này mới có thể xóa phòng ban!";
                    txtDepartment.Focus();
                }
            }
            else
                lbMsg.InnerHtml = "<span style=\"color:red;font-weight:bold;\">Tài khoản của bạn không có quyền thực hiện thao tác này!</span>";
        }
        else Response.Redirect("/ManagerDispatch/Login.aspx");
    }
    protected void grDepartment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "this.originalstyle = this.style.backgroundColor;if(this.originalstyle != 'rgb(204, 255, 153)'){this.style.backgroundColor='#F4F4F4'}");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor = this.originalstyle");  
        }
    }
    protected void drPage_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateGridView();
        ResetControl();
        lbMsg.InnerHtml = "";
    }
    protected void btCancel_Click(object sender, ImageClickEventArgs e)
    {
        ResetControl();
        lbMsg.InnerHtml = "";
    }
    protected void btAdd_Click(object sender, ImageClickEventArgs e)
    {
        LOGIN.Acount acount = Session["ACOUNT"] as LOGIN.Acount;
        if (acount != null)
        {
            md_DBus = new MDDepartmentBussines();
            if (hdDepartmentID.Value == "0")
            {
                if (acount.IsAdminAcount || acount.OwnerAdminLogged.OwnerAdminLogged.StaffCreate)
                {
                    md_DBus.InsertDepartment(this.txtDepartment.Text, this.drParentDepartment.SelectedValue);
                    lbMsg.Style["color"] = "blue";
                    lbMsg.InnerHtml = "Đã thêm phòng ban!";
                }
                else
                    lbMsg.InnerHtml = "<span style=\"color:red;font-weight:bold;\">Tài khoản của bạn không có quyền thực hiện thao tác này!</span>";
            }
            else
            {
                md_DBus.UpdateDepartment(hdDepartmentID.Value, this.drParentDepartment.SelectedValue, this.txtDepartment.Text);
                lbMsg.Style["color"] = "blue";
                lbMsg.InnerHtml = "Cập nhật thành công!";
            }
            ResetControl();
            UpdateGridView();
        }
        else Response.Redirect("/ManagerDispatch/Login.aspx");
    }
}
