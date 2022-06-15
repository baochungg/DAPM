using System.Linq;
using System.Data;
using System.Web.UI.WebControls;
using System;
using LINQ;

public class MDFileAttachment
{
    private ManagementDispatchDataContext MDData;
	public MDFileAttachment()
	{
        MDData = new ManagementDispatchDataContext();
	}
    public void InsertFileToDb(string fileName, int fileSize, string contentType, byte[] fData, out string fID)
    {
        FileAttachment f = new FileAttachment();
        f.FileAttachmentID = Guid.NewGuid();
        fID = f.FileAttachmentID.ToString();
        f.FileName = fileName;
        f.FileSize = fileSize;
        f.ContentType = contentType;
        System.Data.Linq.Binary fBinary = new System.Data.Linq.Binary(fData);
        f.FileData = fBinary;
        MDData.FileAttachments.InsertOnSubmit(f);
        MDData.SubmitChanges();
    }
    public FileAttachment GetFile(string fID)
    {
        return MDData.FileAttachments.SingleOrDefault(p => p.FileAttachmentID.ToString() == fID);
    }
    public void DeleteFile(string fID)
    {
        FileAttachment f = MDData.FileAttachments.SingleOrDefault(p => p.FileAttachmentID.ToString() == fID);
        if (f != null)
        {
            MDData.FileAttachments.DeleteOnSubmit(f);
            MDData.SubmitChanges();
        }
    }
}

public class FileAttSession
{
    private System.Collections.Generic.Dictionary<string, string> fIDs;
    public System.Collections.Generic.Dictionary<string, string> FileAttIDs
    {
        get { return this.fIDs; }
        set { this.fIDs = value; }
    }
    public FileAttSession()
    {
        fIDs = new System.Collections.Generic.Dictionary<string, string>();
    }
    public void Add(string fID, string fName)
    {
        this.fIDs.Add(fID, fName);
    }
    public void Remove(string fID)
    {
        this.fIDs.Remove(fID);
    }
}
