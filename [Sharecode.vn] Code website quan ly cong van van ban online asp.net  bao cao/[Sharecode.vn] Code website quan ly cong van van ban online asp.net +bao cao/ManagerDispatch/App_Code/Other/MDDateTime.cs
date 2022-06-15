using System;
public class MDDateTime
{
	public MDDateTime()
	{
	}
    public static string DateToString(DateTime? value)
    {
        if (value.HasValue)
            return ((value.Value.Day > 9 ? value.Value.Day.ToString() : "0" + value.Value.Day) + "/" +
                (value.Value.Month > 9 ? value.Value.Month.ToString() : "0" + value.Value.Month) + "/" +
                    value.Value.Year);
        return "";
    }
}
