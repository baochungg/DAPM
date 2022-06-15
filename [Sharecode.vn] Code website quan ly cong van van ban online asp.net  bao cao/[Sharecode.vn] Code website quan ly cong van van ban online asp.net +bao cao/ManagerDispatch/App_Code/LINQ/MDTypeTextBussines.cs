using System.Linq;
using System;
using LINQ;

public class MDTypeTextBussines
{
    private ManagementDispatchDataContext MDData;
	public MDTypeTextBussines()
	{
        MDData = new ManagementDispatchDataContext();
	}
    public System.Collections.IEnumerable LoadTypeText(int idxS, int idxE, out int count)
    {
        var query = from tt in MDData.TypeTexts
                    where !tt.IsDeleted
                    select tt;
        count = query.Count();
        System.Collections.IEnumerable typetext = query.AsEnumerable()
            .Select((tat, index) => new
            {
                STT = index + 1,
                TypeTextID = tat.TypeTextID,
                TypeTextName = tat.TypeTextName,
                TypeTextDescription = (tat.TypeTextDescription != null ? tat.TypeTextDescription : ""),
                TextCount = tat.Texts.Count
            }).Skip(idxS).Take(idxE - idxS);
        return typetext;
    }
    public void TypeTextToDropdown(ref System.Web.UI.WebControls.DropDownList drTypeText, bool hasFirstRow)
    {
        drTypeText.Items.Clear();
        drTypeText.DataTextField = "TypeTextName";
        drTypeText.DataValueField = "TypeTextID";
        var query = from tt in MDData.TypeTexts orderby tt.TypeTextName select new { tt.TypeTextID, tt.TypeTextName };
        if (hasFirstRow)
            drTypeText.DataSource = (new[] { new { TypeTextID = new Guid("00000000-0000-0000-0000-000000000000"), TypeTextName = "-- Tất cả --" } }).Concat(query);
        else
            drTypeText.DataSource = query;
        drTypeText.DataBind();
    }
    public TypeText GetTypeTextInfo(string typeTextID)
    {
        return MDData.TypeTexts.SingleOrDefault(p => p.TypeTextID.ToString() == typeTextID);
    }
    public void InsertTypeText(string typeTextName, string typeTextDescription)
    {
        TypeText tt = new TypeText();
        tt.TypeTextID = Guid.NewGuid();
        tt.IsDeleted = false;
        tt.TypeTextName = typeTextName;
        tt.TypeTextDescription = typeTextDescription;
        MDData.TypeTexts.InsertOnSubmit(tt);
        MDData.SubmitChanges();
    }
    public void UpdateTypeText(string typeTextID, string typeTextName, string typeTextDescription)
    {
        TypeText tt = MDData.TypeTexts.SingleOrDefault(p => p.TypeTextID.ToString() == typeTextID);
        if (tt != null)
        {
            tt.TypeTextName = typeTextName;
            tt.TypeTextDescription = typeTextDescription;
            MDData.SubmitChanges();
        }
    }
    public void DeleteTypeText(string typeTextID, out int cText)
    {
        cText = 0;
        TypeText tt = MDData.TypeTexts.SingleOrDefault(p => p.TypeTextID.ToString() == typeTextID);
        if (tt != null)
        {
            cText = MDData.Texts.Count(p => p.TypeText == tt);
            if (cText == 0)
            {
                MDData.TypeTexts.DeleteOnSubmit(tt);
            }
            else
            {
                tt.IsDeleted = true;
            }
            MDData.SubmitChanges();
        }
    }
}
