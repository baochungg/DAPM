using System.Linq;
using System.Data;
using System;
using LINQ;

public class MDStaffBussines
{
    private ManagementDispatchDataContext MDData;
	public MDStaffBussines()
	{
        MDData = new ManagementDispatchDataContext(); 
	}
    public void LoadStaff(string departmentID, ref DataTable dtStaff, int idxS, int idxE, ref int count)
    {
        var deparments = from p in MDData.DepartmentOfDepartments where p.DepartmentParentID.ToString() == departmentID select p;
        var staff = from s in MDData.Staffs
                    where s.DepartmentID.ToString() == departmentID && !s.IsDeleted && !s.Acount.IsAdminAcount
                    select s;
        foreach (Staff s in staff)
        {
            count = count + 1;
            dtStaff.Rows.Add(new object[] { count, s.StaffID.ToString(), s.DepartmentID.ToString(), s.Department.DepartmentName, s.IsCharge, s.StaffName, GetDate(s.Birthday), 
                (s.Address != null ? s.Address : ""), (s.PhoneNumber != null ? s.PhoneNumber : ""), s.AcountID.ToString(), s.Acount.UserName, s.Acount.Password, s.Acount.AcountIsBlocked, s.EmailAddress });
        }
        foreach (DepartmentOfDepartment did in deparments)
        {
            NoopStaffOfDepartment(ref dtStaff, (from d in MDData.Departments where d.DepartmentID == did.DepartmentID select d), ref count);
        }
        DataTable result = dtStaff.Clone();
        for (int i = idxS - 1; i < idxE; i++)
        {
            if (i == dtStaff.Rows.Count) break;
            DataRow dr = result.NewRow();
            dr.ItemArray = dtStaff.Rows[i].ItemArray;
            result.Rows.Add(dr);
        }
        dtStaff = result;
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
    private void NoopStaffOfDepartment(ref DataTable dtStaff, IQueryable departments, ref int count)
    {
        foreach (Department d in departments)
        {
            var staff = from s in MDData.Staffs
                        where s.DepartmentID == d.DepartmentID && !s.IsDeleted && !s.Acount.IsAdminAcount
                        select s;
            foreach (Staff s in staff)
            {
                count = count + 1;
                dtStaff.Rows.Add(new object[] { count, s.StaffID.ToString(), s.DepartmentID.ToString(), s.Department.DepartmentName, s.IsCharge, s.StaffName, GetDate(s.Birthday), 
                    (s.Address != null ? s.Address : ""), (s.PhoneNumber != null ? s.PhoneNumber : ""), s.AcountID.ToString(), s.Acount.UserName, s.Acount.Password, s.Acount.AcountIsBlocked, s.EmailAddress });
            }
            var dids = from p in MDData.DepartmentOfDepartments where p.DepartmentParentID == d.DepartmentID select p;
            foreach (DepartmentOfDepartment did in dids)
            {
                NoopStaffOfDepartment(ref dtStaff, (from p in MDData.Departments where p.DepartmentID == did.DepartmentID select p), ref count);
            }
        }
    }
    public Staff GetStaffInfo(string staffID)
    {
        return MDData.Staffs.SingleOrDefault(s => s.StaffID.ToString() == staffID);
    }
    public void InsertStaff(string departmentID, bool isCharge, string staffName, DateTime? birthDay, string phoneNumber, string address, string username, string password, string email, bool isAcountBlocked)
    {
        Staff s = new Staff();
        s.DepartmentID = new Guid(departmentID);
        s.IsCharge = isCharge;
        s.StaffID = Guid.NewGuid();
        s.StaffName = staffName;
        s.Birthday = birthDay;
        s.PhoneNumber = phoneNumber;
        s.Address = address;
        s.EmailAddress = email;
        s.IsDeleted = false;
        LINQ.Acount ac = new LINQ.Acount();
        ac.AcountID = Guid.NewGuid();
        ac.UserName = username;
        ac.Password = password;
        ac.IsAdminAcount = false;
        ac.AcountIsBlocked = isAcountBlocked;
        MDData.Acounts.InsertOnSubmit(ac);
        s.Acount = ac;
        MDData.Staffs.InsertOnSubmit(s);
        StaffGroupUser sgu = new StaffGroupUser();
        sgu.Staff = s;
        sgu.GroupUserID = new Guid("6177cda0-084e-4b1a-8494-00b68bb98af5");
        MDData.SubmitChanges();
    }
    public void UpdateStaff(string staffID, string departmentID, bool isCharge, string staffName, DateTime? birthDay, string phoneNumber, string address, string username, string password, string email, bool isAcountBlocked)
    {
        Staff s = MDData.Staffs.SingleOrDefault(p => p.StaffID.ToString() == staffID);
        if (s != null)
        {
            if (departmentID != null)
                s.DepartmentID = new Guid(departmentID);
            s.IsCharge = isCharge;
            s.StaffName = staffName;
            s.Birthday = birthDay;
            s.PhoneNumber = phoneNumber;
            s.Address = address;
            s.Acount.UserName = username;
            if (password != "" && password != null)
                s.Acount.Password = password;
            s.EmailAddress = email;
            s.Acount.AcountIsBlocked = isAcountBlocked;
            MDData.SubmitChanges();
        }
    }
    public void DeleteStaff(string staffID,out string staffName, out int cTxtOut, out int cTxtTo, out int cEInbox, out int cESent, out int cCWork)
    {
        cTxtOut = 0; cTxtTo = 0; cEInbox = 0; cESent = 0; cCWork = 0;
        staffName = "";
        Staff s = MDData.Staffs.SingleOrDefault(p => p.StaffID.ToString() == staffID);
        if (s != null)
        {
            staffName = s.StaffName;
            cTxtOut = MDData.TextOuts.Count(p => p.StaffID.ToString() == staffID);
            cTxtTo = MDData.TextTos.Count(p => p.StaffToID.ToString() == staffID);
            cEInbox = MDData.EmailInboxes.Count(p => p.Acount == s.Acount);
            cESent = MDData.AcountEmailSentIDs.Count(p => p.Acount == s.Acount);
            cCWork = MDData.CalendarWorkings.Count(p => p.StaffOrDepartmentID.ToString() == staffID);
            if (cTxtOut == 0 && cTxtTo == 0 && cEInbox == 0 && cESent == 0 && cCWork == 0)
            {
                MDData.StaffGroupUsers.DeleteOnSubmit(s.StaffGroupUser);
                MDData.Acounts.DeleteOnSubmit(s.Acount);
                MDData.Staffs.DeleteOnSubmit(s);
            }
            else
            {
                s.IsDeleted = true;
                s.Acount.AcountIsBlocked = true;
            }
            MDData.SubmitChanges();
        }
    }
}
