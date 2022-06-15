using System.Linq;
using System;
using System.Web.UI.WebControls;
using System.Data;
using LINQ;

public class MDTextToBussines
{
    private ManagementDispatchDataContext MDData;
	public MDTextToBussines()
	{
        MDData = new ManagementDispatchDataContext();
	}
    public void StateTextToDropdown(ref DropDownList drStateText, bool hasFirstRow)
    {
        drStateText.Items.Clear();
        drStateText.DataTextField = "StateTextToName";
        drStateText.DataValueField = "StateTextToID";
        var query = from p in MDData.StateTextTos orderby p.StateTextToName select new { p.StateTextToID, p.StateTextToName };
        if (hasFirstRow)
            drStateText.DataSource = (new[] { new { StateTextToID = new Guid("00000000-0000-0000-0000-000000000000"), StateTextToName = "-- Tất cả --" } }).Concat(query);
        else
            drStateText.DataSource = query;
        drStateText.DataBind();
    }
    // Staff >> From Staff1 > To
    public void LoadTextTo(string staffID, 
        string typeTextID, string textLevelID, string textSecurityID, string stateTextToID, string fromUserName, string textNoCode,
        DateTime? dateTo, DateTime? dateIssued, DateTime? dateTreated,
        out IQueryable qTextTo, int idxS, int idxE, out int count)
    {
        var query = from p in MDData.TextTos
                   where !p.TextInbox.Text.IsDeleted && p.StaffToID.ToString() == staffID
                   select p;
        // Filter
        if (typeTextID != null && typeTextID != "00000000-0000-0000-0000-000000000000")
        {
            query = query.Where(p => p.TextInbox.Text.TypeTextID.ToString() == typeTextID);
        }
        if (textLevelID != null && textLevelID != "00000000-0000-0000-0000-000000000000")
        {
            query = query.Where(p => p.TextInbox.Text.TextLevelID.ToString() == textLevelID);
        }
        if (textSecurityID != null && textSecurityID != "00000000-0000-0000-0000-000000000000")
        {
            query = query.Where(p => p.TextInbox.Text.TextSecurityID.ToString() == textSecurityID);
        }
        if (stateTextToID != null && stateTextToID != "00000000-0000-0000-0000-000000000000")
        {
            query = query.Where(p => p.StateTextToID.ToString() == stateTextToID);
        }
        if (fromUserName != null && fromUserName != "")
        {
            query = query.Where(p => p.Staff.Acount.UserName.ToLower() == fromUserName.ToLower());
        }
        if (textNoCode != null && textNoCode != "")
        {
            query = query.Where(p => p.TextInbox.Text.TextNoCode.ToLower() == textNoCode.ToLower());
        }
        if (dateTo.HasValue)
        {
            query = query.Where(p => p.TextInbox.DateTo.Day == dateTo.Value.Day && p.TextInbox.DateTo.Month == dateTo.Value.Month && p.TextInbox.DateTo.Year == dateTo.Value.Year);
        }
        if (dateIssued.HasValue)
        {
            query = query.Where(p => p.TextInbox.DateIssued.Day == dateIssued.Value.Day && p.TextInbox.DateIssued.Month == dateIssued.Value.Month && p.TextInbox.DateIssued.Year == dateIssued.Value.Year);
        }
        if (dateTreated.HasValue)
        {
            query = query.Where(p => p.TextInbox.TreatedDate.HasValue && p.TextInbox.TreatedDate.Value.Day == dateTreated.Value.Day && p.TextInbox.TreatedDate.Value.Month == dateTreated.Value.Month && p.TextInbox.TreatedDate.Value.Year == dateTreated.Value.Year);
        }
        count = query.Count();
        var textTos = query.OrderByDescending(p => p.TextInbox.DateTo).Skip(idxS).Take(idxE - idxS);
        qTextTo = textTos.Select(p => new
        {
            TextToID = p.TextToID.ToString(),
            TextLevelIcon = p.TextInbox.Text.TextLevel.IconUrl,
            TextLevelDescription = p.TextInbox.Text.TextLevel.TextLevelDescription,
            StateTextIcon = p.StateTextTo.IconUrl,
            StateTextDescription = p.StateTextTo.StateTextToName,
            TextSecurityIcon = p.TextInbox.Text.TextSecurity.IconUrl,
            TextSecurityDescription = p.TextInbox.Text.TextSecurity.TextSecurityName,
            StaffFromID = p.StaffFromID.ToString(),
            StaffName = p.Staff.StaffName,
            TextContent = GetTitleTextTo(p.TextInbox.Text.TextNoCode, p.TextInbox.TextContent),
            TextAttachment = p.TextInbox.Text.TextAttachments.Count() > 0 ? "/ManagerDispatch/Images/Icon/text_attachment.png" : "",
            TextAttachmentDescription = GetListTextAtt(p.TextInbox.Text.TextAttachments),
            DateTo = GetDateTo(p.TextInbox.DateTo),
            IsNew = p.IsNew
        });
    }
    private string GetListTextAtt(System.Data.Linq.EntitySet<TextAttachment> tas)
    {
        string result = "";
        foreach (TextAttachment ta in tas)
        {
            result += ta.FileAttachment.FileName + ",";
        }
        if (result != "") return result.Remove(result.Length - 1, 1);
        return result;
    }
    private string GetDateTo(DateTime dateTo)
    {
        DateTime toDay = DateTime.Now;
        if (dateTo.Day == toDay.Day && dateTo.Month == toDay.Month && dateTo.Year == toDay.Year)
        {
            return (dateTo.Hour > 9 ? dateTo.Hour.ToString() : "0" + dateTo.Hour) + ":" + (dateTo.Minute > 9 ? dateTo.Minute.ToString() : "0" + dateTo.Minute);
        }
        else if (dateTo.Year == toDay.Year)
            return (dateTo.Day > 9 ? dateTo.Day.ToString() : "0" + dateTo.Day) + "/" + (dateTo.Month > 9 ? dateTo.Month.ToString() : "0" + dateTo.Month);
        else return (dateTo.Day > 9 ? dateTo.Day.ToString() : "0" + dateTo.Day) + "/" + (dateTo.Month > 9 ? dateTo.Month.ToString() : "0" + dateTo.Month) + "/" + dateTo.Year;
    }
    private string GetTitleTextTo(string tNo, string value)
    {
        tNo = tNo.Trim();
        if (tNo != "") tNo = tNo + " - ";
        value = HtmlRemoval.StripTagsCharArray(value);
        value = value.TrimStart();
        if (value.Length > 502) return tNo + value.Substring(0, 500);
        if (value == "") return "(không có nội dung)";
        return tNo + value;
    }
    public TextTo GetTextToInfomation(string textToID)
    {
        return MDData.TextTos.SingleOrDefault(p => p.TextToID.ToString() == textToID);
    }
}
