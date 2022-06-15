using System.Linq;
using System;
using LINQ;

public class MDTextSecurityBussines
{
    private ManagementDispatchDataContext MDData;
	public MDTextSecurityBussines()
	{
        MDData = new ManagementDispatchDataContext();
	}
    public System.Collections.IEnumerable LoadTextSecurity(int idxS, int idxE, out int count)
    {
        var query = from ts in MDData.TextSecurities
                    where !ts.IsDeleted
                    select ts;
        count = query.Count();
        System.Collections.IEnumerable textsecurity = query.AsEnumerable()
            .Select((ts, index) => new
            {
                STT = index + 1,
                TextSecurityID = ts.TextSecurityID,
                TextSecurityName = ts.TextSecurityName,
                Note = (ts.Note != null ? ts.Note : "")
            }).Skip(idxS).Take(idxE - idxS);
        return textsecurity;
    }
    public void TextSecurityToDowndown(ref System.Web.UI.WebControls.DropDownList drTextSecurity, bool hasFirstRow)
    {
        drTextSecurity.Items.Clear();
        drTextSecurity.DataValueField = "TextSecurityID";
        drTextSecurity.DataTextField = "TextSecurityName";
        var query = from ts in MDData.TextSecurities orderby ts.TextSecurityName select new { ts.TextSecurityID, ts.TextSecurityName };
        if (hasFirstRow)
            drTextSecurity.DataSource = (new[] { new { TextSecurityID = new Guid("00000000-0000-0000-0000-000000000000"), TextSecurityName = "-- Tất cả --" } }).Concat(query);
        else
            drTextSecurity.DataSource = query;
        drTextSecurity.DataBind();
    }
    public TextSecurity GetTextSecurityInfo(string textSecurityID)
    {
        return MDData.TextSecurities.SingleOrDefault(p => p.TextSecurityID.ToString() == textSecurityID);
    }
    public void InsertTextSecurity(string textSecurityName, string textSecurityNote)
    {
        TextSecurity ts = new TextSecurity();
        ts.TextSecurityID = Guid.NewGuid();
        ts.TextSecurityName = textSecurityName;
        ts.Note = textSecurityNote;
        ts.IconUrl = "/ManagerDispatch/Images/Icon/security_not.png";
        ts.IsDeleted = false;
        MDData.TextSecurities.InsertOnSubmit(ts);
        MDData.SubmitChanges();
    }
    public void DeleteTextSecurity(string textSecurityID, out int textCount)
    {
        TextSecurity ts = MDData.TextSecurities.SingleOrDefault(p => p.TextSecurityID.ToString() == textSecurityID);
        var text = from t in MDData.Texts where t.TextSecurityID.ToString() == textSecurityID && !t.IsDeleted select t;
        textCount = text.Count();
        if (textCount > 0)
        {
            foreach (Text t in text)
            {
                t.IsDeleted = true;
            }
            ts.IsDeleted = true;
        }
        else MDData.TextSecurities.DeleteOnSubmit(ts);
        MDData.SubmitChanges();
    }
    public void UpdateTextSecurity(string textSecurityID, string textSecurityName, string textSecurityNote)
    {
        TextSecurity ts = MDData.TextSecurities.SingleOrDefault(p => p.TextSecurityID.ToString() == textSecurityID);
        ts.TextSecurityName = textSecurityName;
        ts.Note = textSecurityNote;
        MDData.SubmitChanges();
    }
}
