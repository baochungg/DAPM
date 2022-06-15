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
using System;
using LINQ;

public partial class CalendarWork : System.Web.UI.Page
{
    private MDCalendarWorkBussines md_CWbus;

    private DateTime? DateStart
    {
        get
        {
            if (this.txtTimeStart.Text == "") return null;
            string[] d = this.txtTimeStart.Text.Split('/');
            return new DateTime(int.Parse(d[2]), int.Parse(d[1]), int.Parse(d[0]));
        }
        set
        {
            if (value.HasValue)
            {
                string d = (value.Value.Day > 9 ? value.Value.Day.ToString() : "0" + value.Value.Day) + "/" +
                    (value.Value.Month > 9 ? value.Value.Month.ToString() : "0" + value.Value.Month) + "/"
                    + value.Value.Year;
                this.txtTimeStart.Text = d;
            }
            else
                this.txtTimeStart.Text = "";
        }
    }
    private DateTime? DateEnd
    {
        get
        {//Download source code tai Sharecode.vn
            if (this.txtTimeStart.Text == "") return null;
            string[] d = this.txtTimeEnd.Text.Split('/');
            return new DateTime(int.Parse(d[2]), int.Parse(d[1]), int.Parse(d[0]));
        }
        set
        {
            if (value.HasValue)
            {
                string d = (value.Value.Day > 9 ? value.Value.Day.ToString() : "0" + value.Value.Day) + "/" +
                    (value.Value.Month > 9 ? value.Value.Month.ToString() : "0" + value.Value.Month) + "/"
                    + value.Value.Year;
                this.txtTimeEnd.Text = d;
            }
            else
                this.txtTimeEnd.Text = "";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //this.ShowCombo();
            txtTimeStart.Text = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
            txtTimeEnd.Text = DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString();
        }
        md_CWbus = new MDCalendarWorkBussines();
        md_CWbus.LoadCombo(ref drTypeCalendar);
        LOGIN.Acount acount = Session["ACOUNT"] as LOGIN.Acount;
        txtUser.Text = acount.StaffLogged.StaffName.ToString();
        txtUser.Enabled = false;
        imbtEditCalendar.Visible = false;
        //txtUser.BackColor = System.Drawing.Color.DimGray;
    }
    public void ResetTexbox()
    {
        txtWorkName.Text = "";
        txtWorkDetail.Text = "";
        txtNote.Text = "";
    }
    protected void imbtThemLich_Click(object sender, ImageClickEventArgs e)
    {
        md_CWbus = new MDCalendarWorkBussines();
        LOGIN.Acount acount = Session["ACOUNT"] as LOGIN.Acount;
        if (acount != null)
        {
            md_CWbus.InsertCalendarStaff(drTypeCalendar.SelectedValue, acount.StaffLogged.StaffID.ToString(), txtUser.Text, txtWorkName.Text, txtWorkDetail.Text, txtNote.Text, DateStart.Value, DateEnd.Value);
        }
        ResetTexbox();
        lbMess.Style["color"] = "green";
        lbMess.Text = "Đã thêm một công việc!";

    }
    //protected void calendar1_SelectionChanged(object sender, EventArgs e)
    //{
    //    DateTime t = calendar1.SelectedDate;
    //    txtTimeStart.Text = t.Day + "/" + t.Month + "/" + t.Year;
    //}
    //protected void calendar2_SelectionChanged(object sender, EventArgs e)
    //{
    //    if (calendar1.SelectedDate >= calendar2.SelectedDate)
    //    {
    //        lbMess.Text = "gia tri khong phu hop voi thoi gian bat dau.";
    //        txtTimeEnd.Text = " ";
    //    }
    //    else
    //    {
    //        DateTime t2 = calendar2.SelectedDate;
    //        txtTimeEnd.Text = t2.Day + "/" + t2.Month + "/" + t2.Year;
    //    }

    //}
    protected void imbtcancel_Click(object sender, ImageClickEventArgs e)
    {
        ResetTexbox();
    }
    protected void imbtEditCalendar_Click(object sender, ImageClickEventArgs e)
    {

    }
}
