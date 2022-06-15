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

public partial class CreateText : System.Web.UI.Page
{
    private MDTypeTextBussines md_TTBus;
    private MDTextLevelBussines md_TLBus;
    private MDTextSecurityBussines md_TSBus;
    private MDCreateTextBussines md_CTBus;
    private MDDepartmentBussines md_DBus;
    private MDFileAttachment md_FABus;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadData();
            if (Session["FileAttachment"] == null) Session["FileAttachment"] = new System.Collections.Generic.List<FileAttSession>();
            FileAttSession fa = new FileAttSession();
            Session["CurrentFileAttachment"] = fa;
            System.Collections.Generic.List<FileAttSession> fas = Session["FileAttachment"] as System.Collections.Generic.List<FileAttSession>;
            if (fas != null)
            {
                fas.Add(fa);
            }
        }
    }
    private DateTime? DateCreate
    {
        get
        {
            if (this.txtDocCreateDate.Text == "") return null;
            string[] d = this.txtDocCreateDate.Text.Split('/');
            return new DateTime(int.Parse(d[2]), int.Parse(d[1]), int.Parse(d[0]));
        }
        set
        {
            if (value.HasValue)
            {
                string d = (value.Value.Day > 9 ? value.Value.Day.ToString() : "0" + value.Value.Day) + "/" +
                    (value.Value.Month > 9 ? value.Value.Month.ToString() : "0" + value.Value.Month) + "/"
                    + value.Value.Year;
                this.txtDocCreateDate.Text = d;
            }
            else this.txtDocCreateDate.Text = "";
        }
    }
    private DateTime? DateIssued
    {
        get
        {
            if (this.txtDateIssued.Text == "") return null;
            string[] d = this.txtDateIssued.Text.Split('/');
            return new DateTime(int.Parse(d[2]), int.Parse(d[1]), int.Parse(d[0]));
        }
        set
        {
            if (value.HasValue)
            {
                string d = (value.Value.Day > 9 ? value.Value.Day.ToString() : "0" + value.Value.Day) + "/" +
                    (value.Value.Month > 9 ? value.Value.Month.ToString() : "0" + value.Value.Month) + "/"
                    + value.Value.Year;
                this.txtDateIssued.Text = d;
            }
            else this.txtDateIssued.Text = "";
        }
    }
    private void LoadData()
    {
        if (md_TTBus == null) md_TTBus = new MDTypeTextBussines();
        if (md_TLBus == null) md_TLBus = new MDTextLevelBussines();
        if (md_TSBus == null) md_TSBus = new MDTextSecurityBussines();
        if (md_CTBus == null) md_CTBus = new MDCreateTextBussines();
        if (md_DBus == null) md_DBus = new MDDepartmentBussines();
        md_TTBus.TypeTextToDropdown(ref this.drTypeText, false);
        md_TLBus.TextLevelToDowndown(ref this.drTextLevel, false);
        md_TSBus.TextSecurityToDowndown(ref this.drTextSecurity, false);
        md_CTBus.DepartmentAddressToDropdown(ref this.drDepartmentAddress);
        md_CTBus.GroupUserToDropdown(ref this.drGroupUser);
        DataTable dtDeparment = new DataTable();
        int count = 0;
        LOGIN.Acount acount = Session["ACOUNT"] as LOGIN.Acount;
        if (acount.IsAdminAcount)
            md_DBus.LoadDepartment(ref drDepartment, ref dtDeparment, 0, 0, ref count);
        else
            md_DBus.LoadDepartment(acount.StaffLogged.DepartmentID.ToString(), ref drDepartment, ref dtDeparment, 0, 0, ref count);
    }
    private void LoadAttachmentFile()
    {
        FileAttSession fa = Session["CurrentFileAttachment"] as FileAttSession;
        if (fa != null)
        {
            DataTable dtFA = new DataTable();
            dtFA.Columns.Add("FileID");
            dtFA.Columns.Add("FileName");
            foreach (string key in fa.FileAttIDs.Keys)
            {
                dtFA.Rows.Add(new object[] { key, fa.FileAttIDs[key] });
            }
            this.grFileAttachment.DataSource = dtFA;
            this.grFileAttachment.DataBind();
        }
    }
    protected void lbUploadFile_Click(object sender, EventArgs e)
    {
        if (fuTextAttachment.HasFile && fuTextAttachment.PostedFile.ContentLength > 0)
        {
            if (fuTextAttachment.PostedFile.ContentLength > Math.Pow(10, 7))
            {
                lbFile.Text = "Dung lượng tệp phải < 10MB.";
            }
            else
            {
                byte[] fData = fuTextAttachment.FileBytes;
                md_FABus = new MDFileAttachment();
                string fID;
                md_FABus.InsertFileToDb(fuTextAttachment.FileName, fuTextAttachment.PostedFile.ContentLength, fuTextAttachment.PostedFile.ContentType, fData, out fID);
                FileAttSession fa = Session["CurrentFileAttachment"] as FileAttSession;
                fa.Add(fID, fuTextAttachment.FileName);
                LoadAttachmentFile();
            }
        }
    }
    protected void lbDeleteAtt_Click(object sender, EventArgs e)
    {
        string fID = ((LinkButton)sender).CommandArgument;
        md_FABus = new MDFileAttachment();
        md_FABus.DeleteFile(fID);
        FileAttSession fa = Session["CurrentFileAttachment"] as FileAttSession;
        fa.Remove(fID);
        LoadAttachmentFile();
    }
    protected void btnCancel_Click(object sender, EventArgs e)
    {
        md_FABus = new MDFileAttachment();
        FileAttSession fa = Session["CurrentFileAttachment"] as FileAttSession;
        foreach (string key in fa.FileAttIDs.Keys)
        {
            md_FABus.DeleteFile(key);
        }
        System.Collections.Generic.List<FileAttSession> fas = Session["FileAttachment"] as System.Collections.Generic.List<FileAttSession>;
        if (fas != null)
        {
            fas.Remove(fa);
        }
        Session["CurrentFileAttachment"] = null;
        Response.Redirect("/ManagerDispatch/Default.aspx");
    }
    protected void btnSend_Click(object sender, EventArgs e)
    {
        LOGIN.Acount acount = Session["ACOUNT"] as LOGIN.Acount;
        md_CTBus = new MDCreateTextBussines();
        bool toStaff = false, toGroupUser = false, toDepartment = false;
        string staffToID = "", groupUserID = "", departmentID = "";
        switch (this.drAddressTo.SelectedValue)
        {
            case "Staff":
                {
                    toStaff = true;
                    if (MDAcountBussines.Exists(this.t_staff.Text.Trim(), out staffToID))
                    {
                        toStaff = true;
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", "<script type=\"text/javascript\">alert('Tên tài khoản không đúng!');</script>");
                        return;
                    }
                    break;
                }
            case "GroupUser":
                {
                    toGroupUser = true;
                    groupUserID = this.drGroupUser.SelectedValue;
                    break;
                }
            case "Department":
                {
                    toDepartment = true;
                    departmentID = this.drDepartment.SelectedValue;
                    break;
                }
        }
        FileAttSession fa = Session["CurrentFileAttachment"] as FileAttSession;
        md_CTBus.SendText(this.drTypeText.SelectedValue, this.chkInternalDocument.Checked, (this.chkInternalDocument.Checked ? this.drDepartmentAddress.SelectedValue : null), this.txtTextNoCode.Text, DateCreate.Value,
            this.txtSigner.Text, this.drTextLevel.SelectedValue, this.drTextSecurity.SelectedValue, Convert.ToBoolean(int.Parse(this.drTextTreated.SelectedValue)), this.editorContent.Text, DateTime.Now, DateIssued.Value, null, null,
            toStaff, toGroupUser, toDepartment, acount.StaffLogged.StaffID.ToString(), staffToID, groupUserID, departmentID, fa);
        Page.ClientScript.RegisterStartupScript(this.GetType(), "PopupScript", "<script type=\"text/javascript\">alert('Đã gửi văn bản đi!');window.location.href='/ManagerDispatch/Default.aspx';</script>");
    }
}
