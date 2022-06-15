using System.Linq;
using System;
using LINQ;

public class MDTextLevelBussines
{
    private ManagementDispatchDataContext MDData;
	public MDTextLevelBussines()
	{
        MDData = new ManagementDispatchDataContext();
	}
    public System.Collections.IEnumerable LoadTextLevel(int idxS, int idxE, out int count)
    {
        var query = from tl in MDData.TextLevels
                    where !tl.IsDeleted
                    orderby tl.TextLevelPoint
                    select tl;
        count = query.Count();
        System.Collections.IEnumerable textlevel = query.AsEnumerable()
            .Select((tl, index) => new
            {
                STT = index + 1,
                TextLevelID = tl.TextLevelID,
                TextLevelName = tl.TextLevelName,
                TextLevelPoint = tl.TextLevelPoint,
                TextLevelDesciption = (tl.TextLevelDescription != null ? tl.TextLevelDescription : "")
            }).Skip(idxS).Take(idxE - idxS);
        return textlevel;
    }
    public void TextLevelToDowndown(ref System.Web.UI.WebControls.DropDownList drTextLevel, bool hasFirstRow)
    {
        drTextLevel.Items.Clear();
        drTextLevel.DataTextField = "TextLevelName";
        drTextLevel.DataValueField = "TextLevelID";
        var query = from tl in MDData.TextLevels orderby tl.TextLevelName select new { tl.TextLevelID, tl.TextLevelName };
        if (hasFirstRow)
            drTextLevel.DataSource = (new[] { new { TextLevelID = new Guid("00000000-0000-0000-0000-000000000000"), TextLevelName = "-- Tất cả --" } }).Concat(query);
        else
            drTextLevel.DataSource = query;
        drTextLevel.DataBind();
    }
    public TextLevel GetTextLevelInfo(string textLevelID)
    {
        return MDData.TextLevels.SingleOrDefault(p => p.TextLevelID.ToString() == textLevelID);
    }
    public void InsertTextLevel(string textLevelName, int textLevelPoint, string textLevelDescription)
    {
        TextLevel tl = new TextLevel();
        tl.TextLevelID = Guid.NewGuid();
        tl.TextLevelName = textLevelName;
        tl.TextLevelPoint = textLevelPoint;
        tl.TextLevelDescription = textLevelDescription;
        tl.IconUrl = "/ManagerDispatch/Images/Icon/waring_normal.png";
        tl.IsDeleted = false;
        MDData.TextLevels.InsertOnSubmit(tl);
        MDData.SubmitChanges();
    }
    public void DeleteTextLevel(string textLevelID, out int textCount)
    {
        TextLevel tl = MDData.TextLevels.SingleOrDefault(p => p.TextLevelID.ToString() == textLevelID);
        var text = from t in MDData.Texts where t.TextLevelID.ToString() == textLevelID && !t.IsDeleted select t;
        textCount = text.Count();
        if (textCount > 0)
        {
            foreach (Text t in text)
            {
                t.IsDeleted = true;
            }
            tl.IsDeleted = true;
        }
        else MDData.TextLevels.DeleteOnSubmit(tl);
        MDData.SubmitChanges();
    }
    public void UpdateTextLevel(string textLevelID, string textLevelName, int textLevelPoint, string textLevelDescription)
    {
        TextLevel tl = MDData.TextLevels.SingleOrDefault(p => p.TextLevelID.ToString() == textLevelID);
        tl.TextLevelName = textLevelName;
        tl.TextLevelPoint = textLevelPoint;
        tl.TextLevelDescription = textLevelDescription;
        MDData.SubmitChanges();
    }
}
