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

public partial class GroupUser : System.Web.UI.Page
{
    private MDDepartmentBussines md_DBus;
    private MDGroupUserBussines md_GBus;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["ACOUNT"] != null)
        {
            LOGIN.Acount acount = Session["ACOUNT"] as LOGIN.Acount;
            if (acount != null)
            {
                if (!acount.IsAdminAcount)
                {
                    if (!acount.OwnerAdminLogged.OwnerAdminLogged.GroupUserAdmin)
                    {
                        lbMsg_GroupUser.Text = "<span style=\"color:red;font-weight:bold;\">Bạn không có quyền truy cập vào trang này! Vui lòng nhấn " +
                            "<a href=\"javascript:history.back();\">back</a> để quay lại.</span>";
                        hdStaffID.Value = "NotOwner";
                    }
                    else
                        if (!IsPostBack) LoadGroupUserData();
                }
                else
                    if (!IsPostBack) LoadGroupUserData();
            }
        }
    }
    private void LoadGroupUserData()
    {
        LOGIN.Acount acount = Session["ACOUNT"] as LOGIN.Acount;
        if (md_DBus == null) md_DBus = new MDDepartmentBussines();
        if (md_GBus == null) md_GBus = new MDGroupUserBussines();
        DataTable dtDeparment = new DataTable();
        int count = 0;
        if (acount.IsAdminAcount)
            md_DBus.LoadDepartment(ref drDepartment, ref dtDeparment, 0, 0, ref count);
        else
            md_DBus.LoadDepartment(acount.StaffLogged.DepartmentID.ToString(), ref drDepartment, ref dtDeparment, 0, 0, ref count);
        md_GBus.GroupUserToDropdown(ref this.drGroupUser);
        count = 0;
        DataTable dtGroupUser = new DataTable();
        dtGroupUser.Columns.Add("STT");
        dtGroupUser.Columns.Add("StaffID");
        dtGroupUser.Columns.Add("DepartmentName");
        dtGroupUser.Columns.Add("StaffName");
        dtGroupUser.Columns.Add("Birthday");
        dtGroupUser.Columns.Add("UserName");
        dtGroupUser.Columns.Add("AcountIsBlocked");
        dtGroupUser.Columns.Add("GroupUserName");
        if (acount.IsAdminAcount)
            md_GBus.LoadGroupUser("4e4fbaa3-442c-4cb2-84a8-03f1472da230", "00000000-0000-0000-0000-000000000000", ref dtGroupUser, 1, 10, ref count);
        else
            md_GBus.LoadGroupUser(acount.StaffLogged.DepartmentID.ToString(), "00000000-0000-0000-0000-000000000000", ref dtGroupUser, 1, 10, ref count);
        if (count == 0)
        {
            lbMsg_GroupUser.Text = "<span style=\"color:blue;\">Không có nhân viên nào thuộc phòng ban và nhóm người dùng tương ứng!</span>";
        }
        else
        {
            grGroupUser.DataSource = dtGroupUser;
            grGroupUser.DataBind();
            int numPage = count / 10;
            if (count % 10 != 0) numPage += 1;
            for (int i = 1; i <= numPage; i++)
                this.drPage.Items.Add(i.ToString());
        }
    }
    private void UpdateGridView(int cPage)
    {
        if (md_GBus == null) md_GBus = new MDGroupUserBussines();
        int count = 0;
        DataTable dtGroupUser = new DataTable();
        dtGroupUser.Columns.Add("STT");
        dtGroupUser.Columns.Add("StaffID");
        dtGroupUser.Columns.Add("DepartmentName");
        dtGroupUser.Columns.Add("StaffName");
        dtGroupUser.Columns.Add("Birthday");
        dtGroupUser.Columns.Add("UserName");
        dtGroupUser.Columns.Add("AcountIsBlocked");
        dtGroupUser.Columns.Add("GroupUserName");
        if (cPage == -1)
        {
            md_GBus.LoadGroupUser(this.drDepartment.SelectedValue, this.drGroupUser.SelectedValue, ref dtGroupUser, 1, 10, ref count);
            drPage.Items.Clear();
            if (count > 0)
            {
                int numPage = count / 10;
                if (count % 10 != 0) numPage += 1;
                for (int i = 1; i <= numPage; i++)
                    this.drPage.Items.Add(i.ToString());
            }
            else lbMsg_GroupUser.Text = "<span style=\"color:blue;\">Không có nhân viên nào thuộc phòng ban và nhóm người dùng tương ứng!</span>";
        }
        else
        {
            md_GBus.LoadGroupUser(this.drDepartment.SelectedValue, this.drGroupUser.SelectedValue, ref dtGroupUser, 10 * cPage - 9, 10 * cPage, ref count);
        }
        grGroupUser.DataSource = dtGroupUser;
        grGroupUser.DataBind();
        updatepanel_GroupUser.Update();
    }
    private void ResetControlGroupUser()
    {
        this.txtStaff.Text = "";
        this.hdStaffID.Value = "0";
        this.grGroupUser.SelectedIndex = -1;
    }
    protected void grGroupUser_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "this.originalstyle = this.style.backgroundColor;if(this.originalstyle != 'rgb(204, 255, 153)'){this.style.backgroundColor='#F4F4F4'}");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor = this.originalstyle");
        }
    }
    protected void imgBtEdit_Click(object sender, EventArgs e)
    {
        MDStaffBussines md_SBus = new MDStaffBussines();
        string staffID = ((ImageButton)sender).CommandArgument;
        this.hdStaffID.Value = staffID;
        LINQ.Staff s = md_SBus.GetStaffInfo(staffID);
        this.txtStaff.Text = s.StaffName;
        int rowidx = int.Parse(((ImageButton)sender).CommandName);
        this.grGroupUser.SelectedIndex = rowidx - (int.Parse(drPage.Text) - 1) * 10 - 1;
        lbMsg_GroupUser.Text = "";
    }
    protected void drPage_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateGridView(int.Parse(drPage.Text));
    }
    protected void drDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        lbMsg_GroupUser.Text = "";
        if (this.hdStaffID.Value == "0")
        {
            UpdateGridView(-1);
        }
    }
    protected void drGroupUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        lbMsg_GroupUser.Text = "";
        if (this.hdStaffID.Value == "0")
        {
            UpdateGridView(-1);
        }
    }
    protected void btSave_Click(object sender, EventArgs e)
    {
        if (hdStaffID.Value != "0" && hdStaffID.Value != "NotOwner")
        {
            if (md_GBus == null) md_GBus = new MDGroupUserBussines();
            md_GBus.Save(hdStaffID.Value, drGroupUser.SelectedValue);
            lbMsg_GroupUser.Text = "<span style=\"color:blue;font-weight:bold;\">Đã lưu thiết lập!</span>";
            ResetControlGroupUser();
            UpdateGridView(-1);
        }
    }
    protected void btCancel_Click(object sender, EventArgs e)
    {
        if (hdStaffID.Value != "0")
        {
            ResetControlGroupUser();
            lbMsg_GroupUser.Text = "";
        }
    }
}
