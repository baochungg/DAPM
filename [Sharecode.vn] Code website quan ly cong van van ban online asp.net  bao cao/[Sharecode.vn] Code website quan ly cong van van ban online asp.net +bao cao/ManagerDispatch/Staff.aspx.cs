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

public partial class Staff : System.Web.UI.Page
{
    private MDDepartmentBussines md_DBus;
    private MDStaffBussines md_SBus;
    private DateTime? StaffBirthday
    {
        get
        {
            if (this.txtBirthday.Text == "") return null;
            string[] d = this.txtBirthday.Text.Split('/');
            return new DateTime(int.Parse(d[2]), int.Parse(d[1]), int.Parse(d[0]));
        }
        set
        {
            if (value.HasValue)
            {
                string d = (value.Value.Day > 9 ? value.Value.Day.ToString() : "0" + value.Value.Day) + "/" +
                    (value.Value.Month > 9 ? value.Value.Month.ToString() : "0" + value.Value.Month) + "/"
                    + value.Value.Year;
                this.txtBirthday.Text = d;
            }
            else this.txtBirthday.Text = "";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["ACOUNT"] != null)
        {
            //Download source code tai Sharecode.vn
            LOGIN.Acount acount = Session["ACOUNT"] as LOGIN.Acount;
            if (acount != null)
            {
                if (!acount.IsAdminAcount)
                {
                    if (acount.OwnerAdminLogged.NotOwnerStaff)
                    {
                        lbMsg_Staff.Text = "<span style=\"color:red;font-weight:bold;\">Bạn không có quyền truy cập vào trang này! Vui lòng nhấn " +
                            "<a href=\"javascript:history.back();\">back</a> để quay lại.</span>";
                        hdStaffID.Value = "NotOwner";
                    }
                    else
                        if (!IsPostBack) LoadStaffData();
                }
                else
                    if (!IsPostBack) LoadStaffData();
                if (!IsPostBack) Page.SetFocus(this.txtStaffName);
            }
        }
    }
    private void LoadStaffData()
    {
        LOGIN.Acount acount = Session["ACOUNT"] as LOGIN.Acount;
        if (md_DBus == null) md_DBus = new MDDepartmentBussines();
        if (md_SBus == null) md_SBus = new MDStaffBussines();
        DataTable dtDeparment = new DataTable();
        int count = 0;
        if (acount.IsAdminAcount)
            md_DBus.LoadDepartment(ref drDepartment, ref dtDeparment, 0, 0, ref count);
        else
            md_DBus.LoadDepartment(acount.StaffLogged.DepartmentID.ToString(), ref drDepartment, ref dtDeparment, 0, 0, ref count);
        count = 0;
        DataTable dtStaff = new DataTable();
        dtStaff.Columns.Add("STT");
        dtStaff.Columns.Add("StaffID");
        dtStaff.Columns.Add("DepartmentID");
        dtStaff.Columns.Add("DepartmentName");
        dtStaff.Columns.Add("IsCharge");
        dtStaff.Columns.Add("StaffName");
        dtStaff.Columns.Add("Birthday");
        dtStaff.Columns.Add("Address");
        dtStaff.Columns.Add("PhoneNumber");
        dtStaff.Columns.Add("AcountID");
        dtStaff.Columns.Add("UserName");
        dtStaff.Columns.Add("Password");
        dtStaff.Columns.Add("AcountIsBlocked");
        dtStaff.Columns.Add("EmailAddress");
        if (acount.IsAdminAcount)
            md_SBus.LoadStaff("4e4fbaa3-442c-4cb2-84a8-03f1472da230", ref dtStaff, 1, 10, ref count);
        else
            md_SBus.LoadStaff(acount.StaffLogged.DepartmentID.ToString(), ref dtStaff, 1, 10, ref count);
        if (count == 0)
        {
            lbMsg_Staff.Text = "<span style=\"color:blue;\">Không có nhân viên nào thuộc phòng ban này!</span>";
        }
        else
        {
            grStaff.DataSource = dtStaff;
            grStaff.DataBind();
            int numPage = count / 10;
            if (count % 10 != 0) numPage += 1;
            for (int i = 1; i <= numPage; i++)
                this.drPage.Items.Add(i.ToString());
        }
    }
    private void UpdateGridviewStaff(int cPage)
    {
        LOGIN.Acount acount = Session["ACOUNT"] as LOGIN.Acount;
        if (md_SBus == null) md_SBus = new MDStaffBussines();
        int count = 0;
        DataTable dtStaff = new DataTable();
        dtStaff.Columns.Add("STT");
        dtStaff.Columns.Add("StaffID");
        dtStaff.Columns.Add("DepartmentID");
        dtStaff.Columns.Add("DepartmentName");
        dtStaff.Columns.Add("IsCharge");
        dtStaff.Columns.Add("StaffName");
        dtStaff.Columns.Add("Birthday");
        dtStaff.Columns.Add("Address");
        dtStaff.Columns.Add("PhoneNumber");
        dtStaff.Columns.Add("AcountID");
        dtStaff.Columns.Add("UserName");
        dtStaff.Columns.Add("Password");
        dtStaff.Columns.Add("AcountIsBlocked");
        dtStaff.Columns.Add("EmailAddress");
        if (cPage == -1)
        {
            md_SBus.LoadStaff(this.drDepartment.SelectedValue, ref dtStaff, 1, 10, ref count);
            drPage.Items.Clear();
            if (count > 0)
            {
                int numPage = count / 10;
                if (count % 10 != 0) numPage += 1;
                for (int i = 1; i <= numPage; i++)
                    this.drPage.Items.Add(i.ToString());
            }
            else lbMsg_Staff.Text = "<span style=\"color:blue;\">Không có nhân viên nào thuộc phòng ban này!</span>";
        }
        else
        {
            md_SBus.LoadStaff(this.drDepartment.SelectedValue, ref dtStaff, 10 * cPage - 9, 10 * cPage, ref count);
        }
        grStaff.DataSource = dtStaff;
        grStaff.DataBind();
        updatepanel_Staff.Update();
    }
    private void ResetControlStaff()
    {
        this.txtAddress.Text = "";
        this.txtBirthday.Text = "";
        this.txtEmailAdress.Text = "";
        this.txtPassword.Text = "";
        this.txtPhoneNumber.Text = "";
        this.txtStaffName.Text = "";
        this.txtUserName.Text = "";
        chkBlockedAcount.Checked = false;
        chkIsCharge.Checked = false;
        this.hdStaffID.Value = "0";
        this.grStaff.SelectedIndex = -1;
        this.btAddStaff.ImageUrl = "/ManagerDispatch/Images/Icon/bt_add_new.png";
    }
    protected void grStaff_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "this.originalstyle = this.style.backgroundColor;if(this.originalstyle != 'rgb(204, 255, 153)'){this.style.backgroundColor='#F4F4F4'}");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor = this.originalstyle");
        }
    }
    protected void imgBtEdit_Click(object sender, EventArgs e)
    {
        LOGIN.Acount acount = Session["ACOUNT"] as LOGIN.Acount;
        if (acount != null)
        {
            if (acount.IsAdminAcount || acount.OwnerAdminLogged.OwnerAdminLogged.StaffModify)
            {
                if (md_SBus == null) md_SBus = new MDStaffBussines();
                string staffID = ((ImageButton)sender).CommandArgument;
                this.hdStaffID.Value = staffID;
                LINQ.Staff s = md_SBus.GetStaffInfo(staffID);
                this.drDepartment.SelectedValue = s.DepartmentID.ToString();
                this.btAddStaff.ImageUrl = "/ManagerDispatch/Images/Icon/bt_update.png";
                this.txtAddress.Text = s.Address;
                StaffBirthday = s.Birthday;
                this.txtEmailAdress.Text = s.EmailAddress;
                this.txtPassword.Text = s.Acount.Password;
                this.txtPhoneNumber.Text = s.PhoneNumber;
                this.txtStaffName.Text = s.StaffName;
                this.txtUserName.Text = s.Acount.UserName;
                chkBlockedAcount.Checked = s.Acount.AcountIsBlocked;
                chkIsCharge.Checked = s.IsCharge;
                int rowidx = int.Parse(((ImageButton)sender).CommandName);
                this.grStaff.SelectedIndex = rowidx - (int.Parse(drPage.Text) - 1) * 10 - 1;
                lbMsg_Staff.Text = "";
            }
            else
                lbMsg_Staff.Text = "<span style=\"color:red;font-weight:bold;\">Tài khoản của bạn không có quyền thực hiện thao tác này!</span>";
        }
        else Response.Redirect("/ManagerDispatch/Login.aspx");
    }
    protected void imgBtDelete_Click(object sender, EventArgs e)
    {
        LOGIN.Acount acount = Session["ACOUNT"] as LOGIN.Acount;
        if (acount != null)
        {
            if (acount.IsAdminAcount || acount.OwnerAdminLogged.OwnerAdminLogged.StaffDelete)
            {
                if (md_SBus == null) md_SBus = new MDStaffBussines();
                string staffID = ((ImageButton)sender).CommandArgument;
                int cTxtOut, cTxtTo, cEInbox, cESent, cCWork;
                string staffName;
                if (acount.StaffLogged.StaffID.ToString() == staffID)
                {
                    lbMsg_Staff.Text = "<span style=\"color:red;font-weight:bold;\">Không thể thực hiện thao tác này!</span>";
                    return;
                }
                md_SBus.DeleteStaff(staffID, out staffName, out cTxtOut, out cTxtTo, out cEInbox, out cESent, out cCWork);
                lbMsg_Staff.Text = "<span style=\"color:blue;font-weight:bold;\">Đã xóa <a style=\"font-style:italic;color:red;\" href=\"javascript:;\" onclick=\"showInfoDel('" + staffName + "', " + cTxtOut + ", " +
                    cTxtTo + ", " + cEInbox + ", " + cESent + ", " + cCWork + ");\">thông tin</a> nhân viên!</span>";
                ResetControlStaff();
                UpdateGridviewStaff(-1);
            }
            else
                lbMsg_Staff.Text = "<span style=\"color:red;font-weight:bold;\">Tài khoản của bạn không có quyền thực hiện thao tác này!</span>";
        }
        else Response.Redirect("/ManagerDispatch/Login.aspx");
    }
    protected void btAddStaff_Click(object sender, EventArgs e)
    {
        LOGIN.Acount acount = Session["ACOUNT"] as LOGIN.Acount;
        if (acount != null)
        {
            if (md_DBus == null) md_DBus = new MDDepartmentBussines();
            if (md_SBus == null) md_SBus = new MDStaffBussines();
            if (this.hdStaffID.Value == "0")
            {
                bool flag = false;
                if (acount.IsAdminAcount) flag = true;
                else if (acount.OwnerAdminLogged.OwnerAdminLogged.StaffCreate) flag = true;
                if (flag)
                {
                    if (MDAcountBussines.Exists(txtUserName.Text))
                    {
                        lbMsg_Staff.Text = "<span style=\"color:red;font-weight:bold;\">Tên tài khoản đã tồn tại!</span>";
                        return;
                    }
                    md_SBus.InsertStaff(this.drDepartment.SelectedValue, this.chkIsCharge.Checked, this.txtStaffName.Text,
                        StaffBirthday, this.txtPhoneNumber.Text, this.txtAddress.Text,
                        this.txtUserName.Text, this.txtPassword.Text, this.txtEmailAdress.Text, this.chkBlockedAcount.Checked);
                    lbMsg_Staff.Text = "<span style=\"color:blue;font-weight:bold;\">Đã thêm nhân viên!</span>";
                    ResetControlStaff();
                    UpdateGridviewStaff(-1);
                }
                else
                    lbMsg_Staff.Text = "<span style=\"color:red;font-weight:bold;\">Tài khoản của bạn không có quyền thực hiện thao tác này!</span>";
            }
            else
            {
                if (MDAcountBussines.Exists(this.hdStaffID.Value, txtUserName.Text))
                {
                    lbMsg_Staff.Text = "<span style=\"color:red;font-weight:bold;\">Tên tài khoản đã tồn tại!</span>";
                    return;
                }
                md_SBus.UpdateStaff(hdStaffID.Value, this.drDepartment.SelectedValue != "4e4fbaa3-442c-4cb2-84a8-03f1472da230" ? this.drDepartment.SelectedValue : null, 
                    this.chkIsCharge.Checked, this.txtStaffName.Text, StaffBirthday, this.txtPhoneNumber.Text, this.txtAddress.Text, this.txtUserName.Text, this.txtPassword.Text,
                    this.txtEmailAdress.Text, this.chkBlockedAcount.Checked);
                lbMsg_Staff.Text = "<span style=\"color:blue;font-weight:bold;\">Đã sửa thông tin nhân viên!</span>";
                ResetControlStaff();
                UpdateGridviewStaff(-1);
            }
        }
        else Response.Redirect("/ManagerDispatch/Login.aspx");
    }
    protected void btCancelStaff_Click(object sender, EventArgs e)
    {
        ResetControlStaff();
        lbMsg_Staff.Text = "";
    }
    protected void drPage_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateGridviewStaff(int.Parse(drPage.Text));
    }
    protected void drDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        lbMsg_Staff.Text = "";
        if (this.hdStaffID.Value == "0")
        {
            UpdateGridviewStaff(-1);
        }
    }
}
