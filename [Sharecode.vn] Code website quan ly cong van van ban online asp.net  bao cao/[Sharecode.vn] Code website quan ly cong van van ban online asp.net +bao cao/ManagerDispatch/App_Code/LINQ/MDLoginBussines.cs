using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using LINQ;

public class MDLoginBussines
{
    private ManagementDispatchDataContext MDData;
	public MDLoginBussines()
	{
        MDData = new ManagementDispatchDataContext();
	}
    public bool Login(string userName, string passWord, out string message, out LINQ.Acount ac)
    {
        message = "";
        ac = MDData.Acounts.SingleOrDefault(p => p.UserName == userName && p.Password == passWord);
        if (ac != null)
        {
            if (ac.AcountIsBlocked)
            {
                message = "Tài khoản của bạn đã bị khóa!";
                return false;
            }
            else
            {
                if (ac.Staffs[0] != null)
                {
                    if (ac.Staffs[0].IsDeleted)
                    {
                        message = "Có vấn đề xảy ra khi đăng nhập!";
                        return false;
                    }
                    else
                    {
                        message = "OK";
                        return true;
                    }
                }
                else
                {
                    message = "Có vấn đề xảy ra khi đăng nhập!";
                    return false;
                }
            }
        }
        else
        {
            message = "Tài khoản hoặc mật khẩu không đúng!";
            return false;
        }
        return false;
    }
}
