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

public partial class TextBook : System.Web.UI.Page
{
    private MDTypeTextBussines md_TTBus;
    private MDBookBussines md_BBus;
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
                    if (acount.OwnerTextLogged.NotOwnerBook)
                    {
                        lbMsg_Book.Text = "<span style=\"color:red;font-weight:bold;\">Bạn không có quyền sử dụng chức năng này! Có thể nhấn " +
                            "<a href=\"javascript:history.back();\">back</a> để quay lại.</span>";
                        hdBookID.Value = "NotOwner";
                    }
                    else if (!IsPostBack)
                        LoadBookData();
                    if (!acount.OwnerTextLogged.OwnerTextLogged.TypeTextAdmin)
                    {
                        lbMsg_TypeText.Text = "<span style=\"color:red;font-weight:bold;\">Bạn không có quyền sử dụng chức năng này! Có thể nhấn " +
                            "<a href=\"javascript:history.back();\">back</a> để quay lại.</span>";
                        hdTypeTextID.Value = "NotOwner";
                    }
                    else if (!IsPostBack)
                        LoadTypeTextData();
                }
                else if (!IsPostBack)
                {
                    LoadBookData();
                    LoadTypeTextData();
                }
            }
        }
        
    }
    #region Book
    private void LoadBookData()
    {
        LOGIN.Acount acount = Session["ACOUNT"] as LOGIN.Acount;
        if (md_BBus == null) md_BBus = new MDBookBussines();
        if (md_DBus == null) md_DBus = new MDDepartmentBussines();
        DataTable dtDeparment = new DataTable();
        int count = 0;
        if (acount.IsAdminAcount)
            md_DBus.LoadDepartment(ref drDepartment, ref dtDeparment, 0, 0, ref count);
        else
            md_DBus.LoadDepartment(acount.StaffLogged.DepartmentID.ToString(), ref drDepartment, ref dtDeparment, 0, 0, ref count);
        DataTable dtBook = new DataTable();
        dtBook.Columns.Add("STT");
        dtBook.Columns.Add("BookID");
        dtBook.Columns.Add("BookName");
        dtBook.Columns.Add("BookPrefix");
        dtBook.Columns.Add("BookDescription");
        dtBook.Columns.Add("TextCount");
        count = 0;
        if (acount.IsAdminAcount)
            md_BBus.LoadBook("4e4fbaa3-442c-4cb2-84a8-03f1472da230", ref dtBook, 1, 10, ref count);
        else
            md_BBus.LoadBook(acount.StaffLogged.DepartmentID.ToString(), ref dtBook, 1, 10, ref count);
        if (count == 0)
        {
            lbMsg_Book.Text = "<span style=\"color:blue;\">Không có sổ nào của phòng ban này!</span>";
        }
        else
        {
            grBook.DataSource = dtBook;
            grBook.DataBind();
            int numPage = count / 10;
            if (count % 10 != 0) numPage += 1;
            for (int i = 1; i <= numPage; i++)
                this.drPage_Book.Items.Add(i.ToString());
        }
    }
    private void UpdateGridViewBook(int cPage)
    {
        LOGIN.Acount acount = Session["ACOUNT"] as LOGIN.Acount;
        int count = 0;
        md_BBus = new MDBookBussines();
        DataTable dtBook = new DataTable();
        dtBook.Columns.Add("STT");
        dtBook.Columns.Add("BookID");
        dtBook.Columns.Add("BookName");
        dtBook.Columns.Add("BookPrefix");
        dtBook.Columns.Add("BookDescription");
        dtBook.Columns.Add("TextCount");
        if (cPage == -1)
        {
            md_BBus.LoadBook(this.drDepartment.SelectedValue, ref dtBook, 1, 10, ref count);
            drPage_Book.Items.Clear();
            if (count > 0)
            {
                int numPage = count / 10;
                if (count % 10 != 0) numPage += 1;
                for (int i = 1; i <= numPage; i++)
                    this.drPage_Book.Items.Add(i.ToString());
            }
            else lbMsg_Book.Text = "<span style=\"color:blue;\">Không có nhân viên nào thuộc phòng ban này!</span>";
        }
        else
            md_BBus.LoadBook(this.drDepartment.SelectedValue, ref dtBook, 10 * cPage - 9, 10 * cPage, ref count);
        this.grBook.DataSource = dtBook;
        this.grBook.DataBind();
        this.updatepanel_Book.Update();
    }
    private void ResetControlBook()
    {
        this.txtDescriptionBook.Text = "";
        this.txtBookPrefix.Text = "";
        this.txtBookName.Text = "";
        this.hdBookID.Value = "0";
        this.btAddBook.ImageUrl = "/ManagerDispatch/Images/Icon/bt_add_new.png";
        this.grBook.SelectedIndex = -1;
    }
    protected void btAddBook_Click(object sender, EventArgs e)
    {
        LOGIN.Acount acount = Session["ACOUNT"] as LOGIN.Acount;
        if (acount != null)
        {
            if (md_BBus == null) md_BBus = new MDBookBussines();
            if (this.hdBookID.Value == "0")
            {
                if (acount.IsAdminAcount || acount.OwnerAdminLogged.OwnerAdminLogged.StaffCreate)
                {
                    md_BBus.InsertBook(this.drDepartment.SelectedValue, this.txtBookName.Text, this.txtBookPrefix.Text, this.txtDescriptionBook.Text);
                    lbMsg_Book.Text = "<span style=\"color:blue;font-weight:bold;\">Đã thêm sổ văn bản!</span>";
                    ResetControlBook();
                    UpdateGridViewBook(-1);
                }
                else
                    lbMsg_Book.Text = "<span style=\"color:red;font-weight:bold;\">Tài khoản của bạn không có quyền thực hiện thao tác này!</span>";
            }
            else
            {
                md_BBus.UpdateBook(this.hdBookID.Value, this.drDepartment.SelectedValue, this.txtBookName.Text, this.txtBookPrefix.Text, this.txtDescriptionBook.Text);
                lbMsg_Book.Text = "<span style=\"color:blue;font-weight:bold;\">Đã sửa sổ văn bản!</span>";
                ResetControlBook();
                UpdateGridViewBook(-1);
            }
        }
        else Response.Redirect("/ManagerDispatch/Login.aspx");
    }
    protected void btCancelBook_Click(object sender, EventArgs e)
    {
        ResetControlBook();
        lbMsg_Book.Text = "";
    }
    protected void grBook_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "this.originalstyle = this.style.backgroundColor;if(this.originalstyle != 'rgb(204, 255, 153)'){this.style.backgroundColor='#F4F4F4'}");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor = this.originalstyle");
            e.Row.ToolTip = grBook.DataKeys[e.Row.RowIndex].Value.ToString();
        }
    }
    protected void drPage_Book_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateGridViewBook(int.Parse(drPage_Book.SelectedValue));
        ResetControlBook();
        lbMsg_Book.Text = "";
    }
    protected void imgBtEdit_Book_Click(object sender, EventArgs e)
    {
        LOGIN.Acount acount = Session["ACOUNT"] as LOGIN.Acount;
        if (acount != null)
        {
            if (acount.IsAdminAcount || acount.OwnerAdminLogged.OwnerAdminLogged.StaffModify)
            {
                if (md_BBus == null)
                    this.md_BBus = new MDBookBussines();
                string bookID = ((ImageButton)sender).CommandArgument;
                this.hdBookID.Value = bookID;
                this.btAddBook.ImageUrl = "/ManagerDispatch/Images/Icon/bt_update.png";
                LINQ.Book b = md_BBus.GetBookInfo(bookID);
                this.txtBookName.Text = b.BookName;
                this.txtBookPrefix.Text = b.BookPrefix;
                this.txtDescriptionBook.Text = b.BookDescription;
                int rowidx = int.Parse(((ImageButton)sender).CommandName);
                this.grBook.SelectedIndex = rowidx - (int.Parse(drPage_Book.Text) - 1) * 10 - 1;
                lbMsg_Book.Text = "";
            }
            else
                lbMsg_Book.Text = "<span style=\"color:red;font-weight:bold;\">Tài khoản của bạn không có quyền thực hiện thao tác này!</span>";
        }
        else Response.Redirect("/ManagerDispatch/Login.aspx");
    }
    protected void imgBtDelete_Book_Click(object sender, EventArgs e)
    {
        LOGIN.Acount acount = Session["ACOUNT"] as LOGIN.Acount;
        if (acount != null)
        {
            if (md_BBus == null) md_BBus = new MDBookBussines();
            string bookID = ((ImageButton)sender).CommandArgument;
            int cText;
            md_BBus.DeleteBook(bookID, out cText);
            lbMsg_Book.Text = "<span style=\"color:blue;font-weight:bold;\">Đã xóa sổ văn bản kèm theo <span style=\"color:red;\">" + cText + "</span> văn bản liên quan </span>";
            ResetControlBook();
            UpdateGridViewBook(-1);
        }
        else Response.Redirect("/ManagerDispatch/Login.aspx");
    }
    #endregion
    #region TypeText
    private void LoadTypeTextData()
    {
        if (md_TTBus == null) md_TTBus = new MDTypeTextBussines();
        int count;
        var tt = md_TTBus.LoadTypeText(0, 10, out count);
        int numPage = count / 10;
        if (count % 10 != 0) numPage += 1;
        for (int i = 1; i <= numPage; i++)
            this.drPage_TypeText.Items.Add(i.ToString());
        this.grTypeText.DataSource = tt;
        this.grTypeText.DataBind();
    }
    private void UpdateGridViewTypeText()
    {
        if (md_TTBus == null) md_TTBus = new MDTypeTextBussines();
        int count;  
        int cPage = int.Parse(drPage_TypeText.Text);
        var tt = md_TTBus.LoadTypeText(cPage * 10 - 10, cPage * 10, out count);
        this.grTypeText.DataSource = tt;
        this.grTypeText.DataBind();
        this.updatepanel_TypeText.Update();
    }
    private void ResetControlTypeText()
    {
        this.txtTypeTextName.Text = "";
        this.txtTypeTextDescription.Text = "";
        this.grTypeText.SelectedIndex = -1;
        this.hdTypeTextID.Value = "0";
        this.btAddTypeText.ImageUrl = "/ManagerDispatch/Images/Icon/bt_add_new.png";
    }
    protected void grTypeText_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes.Add("onmouseover", "this.originalstyle = this.style.backgroundColor;if(this.originalstyle != 'rgb(204, 255, 153)'){this.style.backgroundColor='#F4F4F4'}");
            e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor = this.originalstyle");
            e.Row.ToolTip = grTypeText.DataKeys[e.Row.RowIndex].Value.ToString();
        }
    }
    protected void drPage_TypeText_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateGridViewTypeText();
        ResetControlTypeText();
        this.lbMsg_TypeText.Text = "";
    }
    protected void imgBtEdit_Text_Click(object sender, EventArgs e)
    {
        if (md_TTBus == null)
            this.md_TTBus = new MDTypeTextBussines();
        string typeTextID = ((ImageButton)sender).CommandArgument;
        this.hdTypeTextID.Value = typeTextID;
        this.btAddTypeText.ImageUrl = "/ManagerDispatch/Images/Icon/bt_update.png";
        int rowidx = int.Parse(((ImageButton)sender).CommandName);
        this.grTypeText.SelectedIndex = rowidx - (int.Parse(drPage_TypeText.Text) - 1) * 10 - 1;
        LINQ.TypeText tt = md_TTBus.GetTypeTextInfo(typeTextID);
        this.txtTypeTextName.Text = tt.TypeTextName;
        this.txtTypeTextDescription.Text = tt.TypeTextDescription;
    }
    protected void imgBtDelete_TypeText_Click(object sender, EventArgs e)
    {
        if (md_TTBus == null) md_TTBus = new MDTypeTextBussines();
        string typeTextID = ((ImageButton)sender).CommandArgument;
        int cText;
        md_TTBus.DeleteTypeText(typeTextID, out cText);
        lbMsg_TypeText.Text = "<span style=\"color:blue;font-weight:bold;\">Đã xóa loại văn bản kèm theo <span style=\"color:red;\">" + cText + "</span> văn bản liên quan </span>";
        ResetControlTypeText();
        UpdateGridViewTypeText();
    }
    protected void btAddTypeText_Click(object sender, EventArgs e)
    {
        if (md_TTBus == null) md_TTBus = new MDTypeTextBussines();
        if (this.hdTypeTextID.Value == "0")
        {
            md_TTBus.InsertTypeText(this.txtTypeTextName.Text, this.txtTypeTextDescription.Text);
            lbMsg_TypeText.Text = "<span style=\"color:blue;font-weight:bold;\">Đã thêm loại văn bản!</span>";
        }
        else
        {
            md_TTBus.UpdateTypeText(this.hdTypeTextID.Value, this.txtTypeTextName.Text, this.txtTypeTextDescription.Text);
            lbMsg_TypeText.Text = "<span style=\"color:blue;font-weight:bold;\">Đã sửa loại văn bản!</span>";
        }
        ResetControlTypeText();
        UpdateGridViewTypeText();
    }
    protected void btCancelTypeText_Click(object sender, EventArgs e)
    {
        ResetControlTypeText();
        lbMsg_TypeText.Text = "";
    }
    #endregion
}
