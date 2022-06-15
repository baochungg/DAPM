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
using LINQ;
using System.Text;

/// <summary>
/// Summary description for MDNewWork
/// </summary>
public class MDNewWork
{
    private ManagementDispatchDataContext MDData;
	public MDNewWork()
	{
        MDData = new ManagementDispatchDataContext();
	}
    public System.Collections.IEnumerable LoadWork(int indexS,int indexE,out int count)
    {
        //var staff = from b in MDData.Staffs where b.StaffID.ToString() == staffID select b;
        //var calendar = from q in MDData.CalendarWorkings where q.StaffOrDepartmentID.ToString()== staffID select q;
        var query = from cv in MDData.Works
                    select cv;
        count = query.Count();
        System.Collections.IEnumerable work = query.AsEnumerable()
            .Select((p, index) => new
            {
                STT = index + 1,
                WorkID = p.WorkID,
                WorkName = p.WorkName,
                StartDate = p.DateWorkStart,
                StartEnd = p.DateWorkEnd

            }).Skip(indexS).Take(indexE - indexS);
        return work;
    }
}
