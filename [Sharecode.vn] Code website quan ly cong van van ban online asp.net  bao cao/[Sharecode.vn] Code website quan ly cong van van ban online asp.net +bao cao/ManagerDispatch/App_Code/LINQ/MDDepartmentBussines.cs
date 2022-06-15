using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System;
using LINQ;

public class MDDepartmentBussines
{
    private ManagementDispatchDataContext MDData;
	public MDDepartmentBussines()
	{
        MDData = new ManagementDispatchDataContext();
	}
    public void LoadDepartment(string departmentID, ref DropDownList drDepartment, ref DataTable dtDepartment, int idxS, int idxE, ref int count)
    {
        drDepartment.Items.Clear();
        Department dm = MDData.Departments.SingleOrDefault(p => p.DepartmentID.ToString() == departmentID);
        drDepartment.Items.Add(new ListItem(dm.DepartmentName.ToLower() == "root" ? "---" : dm.DepartmentName, departmentID));
        var deparments = from p in MDData.DepartmentOfDepartments where p.DepartmentParentID.ToString() == departmentID select p;
        foreach (DepartmentOfDepartment did in deparments)
        {
            NoopDepartment(ref drDepartment, ref dtDepartment, (from d in MDData.Departments where d.DepartmentID == did.DepartmentID && !d.IsDeleted select d), "", ref count);
        }
        if (dtDepartment.Columns.Count > 0)
        {
            DataTable result = dtDepartment.Clone();
            for (int i = idxS - 1; i < idxE; i++)
            {
                if (i == dtDepartment.Rows.Count) break;
                DataRow dr = result.NewRow();
                dr.ItemArray = dtDepartment.Rows[i].ItemArray;
                result.Rows.Add(dr);
            }
            dtDepartment = result;
        }
    }
    public void LoadDepartment(ref DropDownList drDepartment, ref DataTable dtDepartment, int idxS, int idxE, ref int count)
    {
        LoadDepartment("4e4fbaa3-442c-4cb2-84a8-03f1472da230", ref drDepartment, ref dtDepartment, idxS, idxE, ref count);
    }
    private void NoopDepartment(ref DropDownList drDepartment, ref DataTable dtDepartment, IQueryable departments, string prefix, ref int count)
    {
        prefix = prefix + "---";
        foreach (Department d in departments)
        {
            int idx = count;
            count = count + 1;
            drDepartment.Items.Add(new ListItem(prefix + d.DepartmentName, d.DepartmentID.ToString()));
            if (dtDepartment.Columns.Count > 0)
                dtDepartment.Rows.Add(new object[] { count, d.DepartmentID.ToString(), prefix + " " + d.DepartmentName, d.Staffs.Count(p => !p.IsDeleted && !p.Acount.IsAdminAcount), d.Books.Count(p => !p.IsDeleted) });
            int staffcount = dtDepartment.Rows.Count;
            var dids = from p in MDData.DepartmentOfDepartments where p.DepartmentParentID == d.DepartmentID select p;
            foreach (DepartmentOfDepartment did in dids)
            {
                NoopDepartment(ref drDepartment, ref dtDepartment, (from p in MDData.Departments where p.DepartmentID == did.DepartmentID && !p.IsDeleted select p), prefix, ref count);
                if (dtDepartment.Rows.Count >= staffcount + 1)
                {
                    dtDepartment.Rows[idx]["StaffCount"] = int.Parse(dtDepartment.Rows[idx]["StaffCount"].ToString()) + int.Parse(dtDepartment.Rows[staffcount]["StaffCount"].ToString());
                    staffcount = dtDepartment.Rows.Count;
                }
            }
            if (dtDepartment.Columns.Count > 0)
            {
                int bookcount = 0;
                for (int i = idx + 1; i < dtDepartment.Rows.Count; i++)
                {
                    //staffcount += int.Parse(dtDepartment.Rows[i]["StaffCount"].ToString());
                    bookcount += int.Parse(dtDepartment.Rows[i]["BookCount"].ToString());
                }
                //dtDepartment.Rows[idx]["StaffCount"] = int.Parse(dtDepartment.Rows[idx]["StaffCount"].ToString()) + staffcount;
                dtDepartment.Rows[idx]["BookCount"] = int.Parse(dtDepartment.Rows[idx]["BookCount"].ToString()) + bookcount;
            }
        }
    }
    public void GetDepartmentInfo(string departmentID, out string departmentName, out string parentDepartmentID)
    {
        departmentName = "";
        parentDepartmentID = "4e4fbaa3-442c-4cb2-84a8-03f1472da230";
        Department d = MDData.Departments.SingleOrDefault(p => p.DepartmentID.ToString() == departmentID);
        if (d != null) departmentName = d.DepartmentName;
        DepartmentOfDepartment did = MDData.DepartmentOfDepartments.SingleOrDefault(p => p.DepartmentID.ToString() == departmentID);
        if (did != null) parentDepartmentID = did.DepartmentParentID.ToString();
    }
    public void InsertDepartment(string departmentName, string departmentParentID)
    {
        Department d = new Department();
        d.DepartmentID = Guid.NewGuid();
        d.DepartmentName = departmentName;
        DepartmentOfDepartment dod = new DepartmentOfDepartment();
        dod.DepartmentID = d.DepartmentID;
        dod.DepartmentParentID = new Guid(departmentParentID);
        MDData.Departments.InsertOnSubmit(d);
        MDData.DepartmentOfDepartments.InsertOnSubmit(dod);
        MDData.SubmitChanges();
    }
    public void UpdateDepartment(string departmentID, string departmentParentID, string departmentName)
    {
        DepartmentOfDepartment dod = MDData.DepartmentOfDepartments.SingleOrDefault(p => p.DepartmentID.ToString() == departmentID);
        dod.DepartmentParentID = new Guid(departmentParentID);
        Department d = MDData.Departments.SingleOrDefault(p => p.DepartmentID.ToString() == departmentID);
        d.DepartmentName = departmentName;
        MDData.SubmitChanges();
    }
    public bool DeleteDepartment(string departmentID)
    {
        if (NoopDeleteDepartment(departmentID))
        {
            MDData.SubmitChanges();
            return true;
        }
        return false;
    }
    private bool NoopDeleteDepartment(string departmentID)
    {
        var staff = from s in MDData.Staffs where s.DepartmentID.ToString() == departmentID select s;
        bool flagStaff = false;
        if (staff.Count() > 0)
        {
            if (staff.Count(p => !p.IsDeleted) > 0) return false;
            else flagStaff = true;
        }
        var book = from b in MDData.Books where b.DepartmentID.ToString() == departmentID select b;
        if (book.Count() > 0)
        {
            if (book.Count(p => !p.IsDeleted) > 0) return false;
        }
        Department d = MDData.Departments.SingleOrDefault(p => p.DepartmentID.ToString() == departmentID);
        DepartmentOfDepartment depodep = MDData.DepartmentOfDepartments.SingleOrDefault(p => p.DepartmentID.ToString() == departmentID);
        var dods = from p in MDData.DepartmentOfDepartments
                   where p.DepartmentParentID.ToString() == departmentID
                   select p;
        foreach (DepartmentOfDepartment dod in dods)
        {
            if (!NoopDeleteDepartment(dod.DepartmentID.ToString()))
            {
                return false;
            }
        }
        MDData.DepartmentOfDepartments.DeleteOnSubmit(depodep);
        if (!flagStaff)
            MDData.Departments.DeleteOnSubmit(d);
        else d.IsDeleted = true;
        return true;
    }
}
