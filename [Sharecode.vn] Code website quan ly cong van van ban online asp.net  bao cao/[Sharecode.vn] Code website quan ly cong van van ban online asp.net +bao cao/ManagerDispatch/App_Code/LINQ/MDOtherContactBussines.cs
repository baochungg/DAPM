using System.Linq;
using System;
using LINQ;


public class MDOtherContactBussines
{
    private ManagementDispatchDataContext MDData;
    public MDOtherContactBussines()
    {
        MDData = new ManagementDispatchDataContext();
    }
    public System.Collections.IEnumerable LoadOtherContact(int idxS, int idxE, out int count)
    {
        var query = from oc in MDData.DepartmentAddresses
                    select oc;
        count = query.Count();
        System.Collections.IEnumerable daddr = query.AsEnumerable()
            .Select((da, index) => new
            {
                STT = index + 1,
                DepartmentAddressID = da.DepartmentAddressID,
                DepartmentAddresName = da.DepartmentAddressName,
                Note = da.Note,
                TextCount = da.Texts.Count
            }).Skip(idxS).Take(idxE - idxS);
        return daddr;
    }
    public DepartmentAddress GetOtherContactInfo(string departmentAddressID)
    {
        return MDData.DepartmentAddresses.SingleOrDefault(p => p.DepartmentAddressID.ToString() == departmentAddressID);
    }
    public void InsertDepartmentAddress(string departmentAddressName, string Note)
    {
        DepartmentAddress dAdd = new DepartmentAddress();
        dAdd.DepartmentAddressID = Guid.NewGuid();
        dAdd.DepartmentAddressName = departmentAddressName;
        dAdd.IsDeleted = false;
        dAdd.Note = Note; ;
        MDData.DepartmentAddresses.InsertOnSubmit(dAdd);
        MDData.SubmitChanges();
    }
    public void UpdateDepartmentAddress(string departmentAddressID, string departmentAddressName, string Note)
    {
        DepartmentAddress dAdd = MDData.DepartmentAddresses.SingleOrDefault(p => p.DepartmentAddressID.ToString() == departmentAddressID);
        if (dAdd != null)
        {
            dAdd.DepartmentAddressName = departmentAddressName;
            dAdd.Note = Note;
            MDData.SubmitChanges();
        }
    }
    public void DeleteDepartmentAddress(string departmentAddressID, out int cText)
    {
        cText = 0;
        DepartmentAddress dAdd = MDData.DepartmentAddresses.SingleOrDefault(p => p.DepartmentAddressID.ToString() == departmentAddressID);
        if (dAdd != null)
        {
            var text = from p in MDData.Texts where p.DepartmentAddressID.ToString() == departmentAddressID select p;
            cText = text.Count();
            if (cText == 0)
            {
                MDData.DepartmentAddresses.DeleteOnSubmit(dAdd);
            }
            else
            {
                foreach (Text t in text)
                {
                    t.IsDeleted = true;
                }
                dAdd.IsDeleted = true;
            }
            MDData.SubmitChanges();
        }
    }
}
