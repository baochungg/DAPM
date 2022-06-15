using System.Linq;
using System;
using System.Web.UI.WebControls;
using System.Data;
using LINQ;

public class MDGroupUserBussines
{
    private ManagementDispatchDataContext MDData;
	public MDGroupUserBussines()
	{
        MDData = new ManagementDispatchDataContext();
	}
    public void GroupUserToDropdown(ref DropDownList drGroupUser)
    {
        var query = from gs in MDData.GroupUsers select new { gs.GroupUserID, gs.GroupUserName };
        drGroupUser.DataTextField = "GroupUserName";
        drGroupUser.DataValueField = "GroupUserID";
        drGroupUser.DataSource = (new[] { new { GroupUserID = new Guid("00000000-0000-0000-0000-000000000000"), GroupUserName = "---" } }).Concat(query);
        drGroupUser.DataBind();
    }
    public void LoadGroupUser(string departmentID, string groupUserID, ref DataTable dtGroupUser, int idxS, int idxE, ref int count)
    {
        var deparments = from p in MDData.DepartmentOfDepartments where p.DepartmentParentID.ToString() == departmentID select p;
        bool groupUserFlag = false;
        if (groupUserID != "00000000-0000-0000-0000-000000000000") groupUserFlag = true;
        var staff = from s in MDData.Staffs
                    where s.DepartmentID.ToString() == departmentID && !s.IsDeleted && !s.Acount.IsAdminAcount
                    select s;
        if (groupUserFlag)
            staff = staff.Where(p => p.StaffGroupUser.GroupUserID.ToString() == groupUserID);
        foreach (Staff s in staff)
        {
            count = count + 1;
            dtGroupUser.Rows.Add(new object[] { count, s.StaffID.ToString(), s.Department.DepartmentName, s.StaffName, GetDate(s.Birthday), 
                s.Acount.UserName, s.Acount.AcountIsBlocked, s.StaffGroupUser.GroupUser.GroupUserName });
        }
        foreach (DepartmentOfDepartment did in deparments)
        {
            NoopGroupUserStaff(ref dtGroupUser, (from d in MDData.Departments where d.DepartmentID == did.DepartmentID select d), groupUserID, ref count);
        }
        DataTable result = dtGroupUser.Clone();
        for (int i = idxS - 1; i < idxE; i++)
        {
            if (i == dtGroupUser.Rows.Count) break;
            DataRow dr = result.NewRow();
            dr.ItemArray = dtGroupUser.Rows[i].ItemArray;
            result.Rows.Add(dr);
        }
        dtGroupUser = result;
    }
    private void NoopGroupUserStaff(ref DataTable dtGroupUser, IQueryable departments, string groupUserID, ref int count)
    {
        foreach (Department d in departments)
        {
            bool groupUserFlag = false;
            if (groupUserID != "00000000-0000-0000-0000-000000000000") groupUserFlag = true;
            var staff = from s in MDData.Staffs
                        where s.DepartmentID == d.DepartmentID && !s.IsDeleted && !s.Acount.IsAdminAcount
                        select s;
            if (groupUserFlag)
                staff = staff.Where(p => p.StaffGroupUser.GroupUserID.ToString() == groupUserID);
            foreach (Staff s in staff)
            {
                count = count + 1;
                dtGroupUser.Rows.Add(new object[] { count, s.StaffID.ToString(), s.Department.DepartmentName, s.StaffName, GetDate(s.Birthday), 
                s.Acount.UserName, s.Acount.AcountIsBlocked, s.StaffGroupUser.GroupUser.GroupUserName });
            }
            var dids = from p in MDData.DepartmentOfDepartments where p.DepartmentParentID == d.DepartmentID select p;
            foreach (DepartmentOfDepartment did in dids)
            {
                NoopGroupUserStaff(ref dtGroupUser, (from p in MDData.Departments where p.DepartmentID == did.DepartmentID select p), groupUserID, ref count);
            }
        }
    }
    private string GetDate(DateTime? value)
    {
        if (value.HasValue)
        {
            string d = (value.Value.Day > 9 ? value.Value.Day.ToString() : "0" + value.Value.Day) + "/" +
                    (value.Value.Month > 9 ? value.Value.Month.ToString() : "0" + value.Value.Month) + "/"
                    + value.Value.Year;
            return d;
        }
        return "";
    }
    public void Save(string staffID, string groupUserID)
    {
        Staff staff = MDData.Staffs.SingleOrDefault(p => p.StaffID.ToString() == staffID);
        if (staff != null)
        {
            staff.StaffGroupUser.GroupUserID = new Guid(groupUserID);
            MDData.SubmitChanges();
        }
    }
}
