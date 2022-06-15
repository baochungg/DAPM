using System;
using System.Data;
using System.Configuration;
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

/// <summary>
/// Summary description for CalendarWorkBussines
/// </summary>
public class MDCalendarWorkBussines
{
    private ManagementDispatchDataContext MDData;
	public MDCalendarWorkBussines()
	{
        MDData = new ManagementDispatchDataContext();
	}
    public void LoadCombo(ref System.Web.UI.WebControls.DropDownList drtypeCalendar)
    {
        var typeCalendar = from p in MDData.TypeCalendars
                           select new
                           {
                               p.TypeCalendarID,
                               p.TypeCalendarName
                           };
        drtypeCalendar.DataSource = typeCalendar;
        drtypeCalendar.DataTextField = "TypeCalendarName";
        drtypeCalendar.DataValueField = "TypeCalendarID";
        drtypeCalendar.DataBind();
        //var nhanvien = from p in MDData.CalendarWorkings
        //               select new
        //               {
        //                   p.
        //               };
    }

    public void InsertCalendarStaff(string typeCalendar,string staffID,string staffName,string workName, string workDetails, string Note, DateTime dateStart, DateTime dateEnd)
    {
        Work cv = new Work();
        cv.WorkID = Guid.NewGuid();
        cv.WorkName = workName;
        cv.WorkDetails = workDetails;
        cv.Note = Note;
        cv.DateWorkStart = dateStart;
        cv.DateWorkEnd = dateEnd;
        MDData.Works.InsertOnSubmit(cv);
        LINQ.CalendarWorking cw = new LINQ.CalendarWorking();
        cw.CalendarWorkID = Guid.NewGuid();
        cw.TypeCalendarID = new Guid(typeCalendar);
        cw.StaffOrDepartmentID = new Guid(staffID);
        cw.WorkID = cv.WorkID;
        cw.ChargePerson = staffName;
        string stateID = "2bff3c99-d0eb-458e-b499-c16ef08d1337";
        cw.WorkingStateID = new Guid(stateID);
        MDData.CalendarWorkings.InsertOnSubmit(cw);
        MDData.SubmitChanges();
    }
    public void UpdateCalendarWork(string workID,string workName,string workDetails,DateTime dateStart,DateTime dateEnd,string note,string calendarWorkID, string typeCalendarID,string staffID,string staffName,string stateWork)
    {
        Work cv = MDData.Works.SingleOrDefault(p => p.WorkID.ToString() == workID);
        if (cv != null)
        {
            cv.WorkName = workName;
            cv.WorkDetails = workDetails;
            cv.DateWorkStart = dateStart;
            cv.DateWorkEnd = dateEnd;
            cv.Note = note;
            MDData.SubmitChanges();
        }
        CalendarWorking cw = MDData.CalendarWorkings.SingleOrDefault(p => p.CalendarWorkID.ToString() == calendarWorkID);
        if (cw != null)
        {
            cw.TypeCalendarID =new Guid(typeCalendarID);
            cw.StaffOrDepartmentID =new Guid(staffID);
            cw.WorkID =new Guid(workID);
            cw.ChargePerson = staffName;
            cw.WorkingStateID = new Guid(stateWork);
        }
        MDData.SubmitChanges();
    }
    public void DeleteWork(string workID, string workName)
    {
 
    }
}
