using System.Linq;
using LINQ;

public class MDMenuBussines
{
    private ManagementDispatchDataContext MDData;
	public MDMenuBussines()
	{
        MDData = new ManagementDispatchDataContext();
	}
    public IQueryable LoadMainTab(string activeMenuID)
    {
        if (activeMenuID != null)
        {
            if (MDData.Menus.SingleOrDefault(p => p.MainMenuID.ToString() == activeMenuID) == null)
            {
                activeMenuID = "57e3987c-9b82-4d3c-a1fa-4147c31dd152";
            }
        }
        else activeMenuID = "57e3987c-9b82-4d3c-a1fa-4147c31dd152";
        var mainmenus = from p in MDData.Menus
                        where p.IsParentMenu && p.Enable
                        orderby p.MenuIndex
                        select new
                        {
                            menuID = p.MainMenuID,
                            menuName = p.MainMenuName,
                            description = p.MenuDescription,
                            active = ((p.MainMenuID.ToString() == activeMenuID && activeMenuID != null) ? "1" : "0")
                        };
        return mainmenus;
    }
    public IQueryable LoadSubMenuView(string parentMenuID)
    {
        if (parentMenuID != null)
        {
            if (MDData.Menus.SingleOrDefault(p => p.MainMenuID.ToString() == parentMenuID) == null)
            {
                parentMenuID = "57e3987c-9b82-4d3c-a1fa-4147c31dd152";
            }
        }
        else parentMenuID = "57e3987c-9b82-4d3c-a1fa-4147c31dd152";
        var submenu = from p in MDData.Menus
                      where p.ParentMenu.ToString() == parentMenuID && p.Enable
                      orderby p.MenuIndex
                      select new
                      {
                          menuID = p.MainMenuID,
                          menuName = p.MainMenuName,
                          Description = p.MenuDescription,
                          menuUrl = p.MenuUrl,
                          imgIcon = p.MenuIcon
                      };
        return submenu;
    }
}
