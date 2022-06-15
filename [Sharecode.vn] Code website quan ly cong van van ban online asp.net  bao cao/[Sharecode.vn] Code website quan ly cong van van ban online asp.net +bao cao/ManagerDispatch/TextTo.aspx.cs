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

public partial class TextTo : System.Web.UI.Page
{
    private MDTypeTextBussines md_TTBus;
    private MDTextLevelBussines md_TLBus;
    private MDTextSecurityBussines md_TSBus;
    private MDTextToBussines md_TToBus;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) LoadData();
    }
    private DateTime? DateTo
    {
        get
        {
            if (this.txtDateTo.Text == "") return null;
            string[] d = this.txtDateTo.Text.Split('/');
            return new DateTime(int.Parse(d[2]), int.Parse(d[1]), int.Parse(d[0]));
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
    }
    private DateTime? DateTreated
    {
        get
        {
            //Download source code tai Sharecode.vn
            if (this.txtDateTreated.Text == "") return null;
            string[] d = this.txtDateTreated.Text.Split('/');
            return new DateTime(int.Parse(d[2]), int.Parse(d[1]), int.Parse(d[0]));
        }
    }
    private void LoadData()
    {
        LOGIN.Acount acount = Session["ACOUNT"] as LOGIN.Acount;
        if (acount != null)
        {
            if (md_TTBus == null) md_TTBus = new MDTypeTextBussines();
            if (md_TLBus == null) md_TLBus = new MDTextLevelBussines();
            if (md_TSBus == null) md_TSBus = new MDTextSecurityBussines();
            if (md_TToBus == null) md_TToBus = new MDTextToBussines();
            md_TTBus.TypeTextToDropdown(ref this.drTypeText, true);
            md_TLBus.TextLevelToDowndown(ref this.drTextLevel, true);
            md_TSBus.TextSecurityToDowndown(ref this.drTextSecurity, true);
            md_TToBus.StateTextToDropdown(ref this.drStateText, true);
            //
            IQueryable qTextTo;
            int count;
            md_TToBus.LoadTextTo(acount.StaffLogged.StaffID.ToString(), null, null, null, null, null, null, null, null, null, out qTextTo, 0, 20, out count);
            lbPageCount.Text = count.ToString();
            if (count < 20) lbPageTo.Text = "1-" + count;
            else lbPageTo.Text = "1-20";
            grTextTo.DataSource = qTextTo;
            grTextTo.DataBind();
        }
        else Response.Redirect("/ManagerDispatch/Login.aspx");
    }
    private void UpdateGridView()
    {
        LOGIN.Acount acount = Session["ACOUNT"] as LOGIN.Acount;
        if (acount != null)
        {
            if (md_TToBus == null) md_TToBus = new MDTextToBussines();
            IQueryable qTextTo;
            int count;
            md_TToBus.LoadTextTo(acount.StaffLogged.StaffID.ToString(), this.drTypeText.SelectedValue, this.drTextLevel.SelectedValue, this.drTextSecurity.SelectedValue,
                this.drStateText.SelectedValue, this.txtUserName.Text, this.txtTextNoCode.Text, DateTo, DateIssued, DateTreated, out qTextTo, 0, 20, out count);
            lbPageCount.Text = count.ToString();
            if (count < 20)
            {
                if (count == 0) lbPageTo.Text = "0-0";
                else lbPageTo.Text = "1-" + count;
            }
            else lbPageTo.Text = "1-20";
            grTextTo.DataSource = qTextTo;
            grTextTo.DataBind();
            updatepanel_TextTo.Update();
        }
        else Response.Redirect("/ManagerDispatch/Login.aspx");
    }
    protected void grTextTo_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            CheckBox chk = e.Row.FindControl("chkRowSelect") as CheckBox;
            chk.Attributes.Add("textToID", DataBinder.Eval(e.Row.DataItem, "TextToID").ToString());
            chk.Attributes.Add("onclick", "chkSelectTextTo_CheckedChange('" + chk.ClientID + "');");
            for (int i = 1; i < e.Row.Cells.Count; i++)
            {
                e.Row.Cells[i].Attributes.Add("onclick", "document.getElementById('" + e.Row.FindControl("btRow").ClientID + "').click();");    
            }
            bool isNew = (bool)DataBinder.Eval(e.Row.DataItem, "IsNew");
            if (isNew)
            {
                e.Row.Attributes.Add("class", "rowTextTo rowNewTextTo");
            }
            else e.Row.Attributes.Add("class", "rowTextTo");
        }
    }
    protected void btSearch_Click(object sender, ImageClickEventArgs e)
    {
        UpdateGridView();
    }
    protected void btRow_Click(object sender, EventArgs e)
    {
        lbTitle.Text = " Nội dung văn bản ";
        topSearch.Visible = false;
        lTitle.Visible = false;
        controlTextTo1.Visible = false;
        controlTextTo2.Visible = true;
        grTextTo.Visible = false;
        showTextTo.Visible = true;
        
        //
        lbtDateIssued.Text = "";
        lbtNo.Text = "";
        if (md_TToBus == null) md_TToBus = new MDTextToBussines();
        LINQ.TextTo to = md_TToBus.GetTextToInfomation((sender as Button).CommandArgument);
        if (to != null)
        {
            if (to.TextInbox.Text.TextNoCode.Trim() != "")
            {
                lbtNo.Text = to.TextInbox.Text.TextNoCode;
                lbTextNoCode.Text = to.TextInbox.Text.TextNoCode;
                lbtDateIssued.Text = "(" + MDDateTime.DateToString(to.TextInbox.DateIssued) +
                    (to.TextInbox.TreatedDate.HasValue ? " - " + MDDateTime.DateToString(to.TextInbox.TreatedDate) : " - ???") + ")";
            }
            else
            {
                lbtNo.Text = ".....";
                lbTextNoCode.Text = "(không có)";
            }
            lbTextToFrom.Text = to.Staff.StaffName + " (" + to.Staff.Department.DepartmentName + ")";
            lbTypeText.Text = to.TextInbox.Text.TypeText.TypeTextName;
            lbDateTo.Text = MDDateTime.DateToString(to.TextInbox.DateTo);
        }
    }
    protected void btBack_Click(object sender, EventArgs e)
    {
        lbTitle.Text = " Lựa chọn điều kiện tìm kiếm ";
        topSearch.Visible = true;
        lTitle.Visible = true;
        controlTextTo1.Visible = true;
        controlTextTo2.Visible = false;
        grTextTo.Visible = true;
        UpdateGridView();
    }
}
