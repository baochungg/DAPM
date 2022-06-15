using System.Linq;
using System.Data;
using System.Web.UI.WebControls;
using System;
using LINQ;

public class MDCreateTextBussines
{
    private ManagementDispatchDataContext MDData;
	public MDCreateTextBussines()
	{
        MDData = new ManagementDispatchDataContext();
	}
    public void DepartmentAddressToDropdown(ref DropDownList drDepartmentAddress)
    {
        drDepartmentAddress.Items.Clear();
        drDepartmentAddress.DataSource = (from da in MDData.DepartmentAddresses select new { da.DepartmentAddressID, da.DepartmentAddressName });
        drDepartmentAddress.DataTextField = "DepartmentAddressName";
        drDepartmentAddress.DataValueField = "DepartmentAddressID";
        drDepartmentAddress.DataBind();
    }
    public void GroupUserToDropdown(ref DropDownList drGroupUser)
    {
        drGroupUser.Items.Clear();
        drGroupUser.DataValueField = "GroupUserID";
        drGroupUser.DataTextField = "GroupUserName";
        drGroupUser.DataSource = (from gu in MDData.GroupUsers select new { gu.GroupUserID, gu.GroupUserName });
        drGroupUser.DataBind();
    }
    public void InsertText(string typeTextID, bool isInternalDocument,string departmentAddressID, string textNoCode, DateTime dateCreate, string signer, string textLevelID, string textSecurityID, bool needTreated, bool isSubmitChange, out string textID)
    {
        Text t = new Text();
        t.TextID = Guid.NewGuid();
        textID = t.TextID.ToString();
        t.TypeTextID = new Guid(typeTextID);
        t.IsInternalDocuments = isInternalDocument;
        if (isInternalDocument) t.DepartmentAddressID = new Guid(departmentAddressID);
        t.TextNoCode = textNoCode;
        t.DateCreate = dateCreate;
        t.Signer = signer;
        t.TextLevelID = new Guid(textLevelID);
        t.TextSecurityID = new Guid(textSecurityID);
        t.NeedTreated = needTreated;
        MDData.Texts.InsertOnSubmit(t);
        if (isSubmitChange)
            MDData.SubmitChanges();
    }
    public void InsertTextInbox(string textID, string textContent, DateTime dateTo, DateTime dateIssued, DateTime? treatedDate, DateTime? editedDate, bool isSubmitChange, out string textInbox)
    {
        TextInbox ti = new TextInbox();
        ti.TextInboxID = Guid.NewGuid();
        textInbox = ti.TextInboxID.ToString();
        ti.TextID = new Guid(textID);
        ti.TextContent = textContent;
        ti.DateTo = dateTo;
        ti.DateIssued = dateIssued;
        ti.TreatedDate = treatedDate;
        ti.EditedDate = editedDate;
        MDData.TextInboxes.InsertOnSubmit(ti);
        if (isSubmitChange)
            MDData.SubmitChanges();
    }
    public void InsertTextOut(string textInboxID, bool toStaff, bool toGroupUser, bool toDepartment, string staffID, string groupUserID, string departmentID, bool isSubmitChange)
    {
        TextOut to = new TextOut();
        to.TextOutID = Guid.NewGuid();
        to.TextInboxID = new Guid(textInboxID);
        to.ToStaff = toStaff;
        to.ToGroupUser = toGroupUser;
        to.ToDepartment = toDepartment;
        if (toStaff) to.StaffID = new Guid(staffID);
        else to.StaffID = null;
        if (toGroupUser) to.GroupUserID = new Guid(groupUserID);
        else to.GroupUserID = null;
        if (toDepartment) to.DepartmentID = new Guid(departmentID);
        else to.DepartmentID = null;
        MDData.TextOuts.InsertOnSubmit(to);
        if (isSubmitChange)
            MDData.SubmitChanges();
    }
    // insert record to database
    public void InsertTextTo(string textInboxID, string staffFromID, string staffToID, bool isNew, string stateTextToID, bool isSubmitChange)
    {
        TextTo to = new TextTo();
        to.TextToID = Guid.NewGuid();
        to.TextInboxID = new Guid(textInboxID);
        to.StaffToID = new Guid(staffToID);
        to.StaffFromID = new Guid(staffFromID);
        to.IsNew = isNew;
        if (stateTextToID != null)
            to.StateTextToID = new Guid(stateTextToID);
        else
            to.StateTextToID = new Guid("9e33e31e-92fe-44d7-9fca-c2e9348110d2");
        MDData.TextTos.InsertOnSubmit(to);
        if (isSubmitChange)
            MDData.SubmitChanges();
    }
    // insert to groupuser
    public void InsertTextTo(string textInboxID, string staffFromID, string groupUserID, string stateTextToID, bool isNew, bool isSubmitChange)
    {
        var sgrs = from p in MDData.StaffGroupUsers where p.GroupUserID.ToString() == groupUserID && !p.Staff.IsDeleted select p;
        foreach (StaffGroupUser sgr in sgrs)
        {
            InsertTextTo(textInboxID, staffFromID, sgr.StaffID.ToString(), true, stateTextToID, false);
        }
        if (isSubmitChange)
            MDData.SubmitChanges();
    }
    //insert to department
    public void InsertTextTo(string textInboxID, string staffFromID, string departmentID, bool isNew, string stateTextToID)
    {
        var deparments = from p in MDData.DepartmentOfDepartments where p.DepartmentParentID.ToString() == departmentID select p;
        var staff = from s in MDData.Staffs
                    where s.DepartmentID.ToString() == departmentID && !s.IsDeleted && !s.Acount.IsAdminAcount
                    select s;
        foreach (Staff s in staff)
        {
            InsertTextTo(textInboxID, staffFromID, s.StaffID.ToString(), true, stateTextToID, false);
        }
        foreach (DepartmentOfDepartment did in deparments)
        {
            NoopDepartment(textInboxID, staffFromID, true, stateTextToID, (from p in MDData.Departments where p.DepartmentID == did.DepartmentID select p));
        }
        MDData.SubmitChanges();
    }
    public void InsertFileAttachment(string fileAttachmentID, string textID, bool isSubmitChange)
    {
        TextAttachment ta = new TextAttachment();
        ta.TextAttachmentID = Guid.NewGuid();
        ta.FileAttachmentID = new Guid(fileAttachmentID);
        ta.TextID = new Guid(textID);
        MDData.TextAttachments.InsertOnSubmit(ta);
        if (isSubmitChange) MDData.SubmitChanges();
    }
    private void NoopDepartment(string textInboxID, string staffFromID, bool isNew, string stateTextToID, IQueryable departments)
    {
        foreach (Department d in departments)
        {
            var staff = from s in MDData.Staffs
                        where s.DepartmentID == d.DepartmentID && !s.IsDeleted && !s.Acount.IsAdminAcount
                        select s;
            foreach (Staff s in staff)
            {
                InsertTextTo(textInboxID, staffFromID, s.StaffID.ToString(), true, stateTextToID, false);
            }
            var dids = from p in MDData.DepartmentOfDepartments where p.DepartmentParentID == d.DepartmentID select p;
            foreach (DepartmentOfDepartment did in dids)
            {
                NoopDepartment(textInboxID, staffFromID, true, stateTextToID, (from p in MDData.Departments where p.DepartmentID == did.DepartmentID select p));
            }
        }
    }
    public void SendText(string typeTextID, bool isInternalDocument, string departmentAddressID, string textNoCode, DateTime dateCreate, string signer, string textLevelID, string textSecurityID, bool needTreated
        , string textContent, DateTime dateTo, DateTime dateIssued, DateTime? treatedDate, DateTime? editedDate,
        bool toStaff, bool toGroupUser, bool toDepartment, string staffFromID, string stafTofID, string groupUserID, string departmentID, FileAttSession fa)
    {
        string textID;
        string textInboxID;
        InsertText(typeTextID, isInternalDocument, departmentAddressID, textNoCode, dateCreate, signer, textLevelID, textSecurityID, needTreated, false, out textID);
        InsertTextInbox(textID, textContent, dateTo, dateIssued, treatedDate, editedDate, false, out textInboxID);
        string stateTextTo;
        if (needTreated)
            stateTextTo = "9e33e31e-92fe-44d7-9fca-c2e9348110d2";
        else stateTextTo = "f7bea5fc-47e8-44fe-abd8-b50b991a8224";
        if (toStaff)
        {
            InsertTextTo(textInboxID, staffFromID, stafTofID, true, stateTextTo, false);
        }
        else if (toDepartment)
        {
            InsertTextTo(textInboxID, staffFromID, departmentID, true, stateTextTo);
        }
        else if (toGroupUser)
        {
            InsertTextTo(textInboxID, staffFromID, groupUserID, stateTextTo, false, false);
        }
        foreach (string key in fa.FileAttIDs.Keys)
        {
            InsertFileAttachment(key, textID, false);
        }
        InsertTextOut(textInboxID, toStaff, toGroupUser, toDepartment, stafTofID, groupUserID, departmentID, false);
        MDData.SubmitChanges();
    }
}
