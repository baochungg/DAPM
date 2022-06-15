using System.Linq;
using LINQ;

public class MDAcountBussines
{
    private ManagementDispatchDataContext MDData;
	public MDAcountBussines()
	{
        MDData = new ManagementDispatchDataContext();
	}
    // Kiểm tra tên một tài khoản đã tồn tại hay chưa
    public static bool Exists(string userName)
    {
        using (ManagementDispatchDataContext mdd = new ManagementDispatchDataContext())
        {
            Acount ac = mdd.Acounts.SingleOrDefault(p => p.UserName == userName);
            if (ac == null) return false;
            else
            {
                if (ac.Staffs[0].IsDeleted) return false;
            }
        }
        return true;
    }
    //Kiểm tra tên tài khoản có tồn tại không có thì trả ra cả mã nhân viên
    public static bool Exists(string userName, out string staffID)
    {
        staffID = "";
        using (ManagementDispatchDataContext mdd = new ManagementDispatchDataContext())
        {
            Acount ac = mdd.Acounts.SingleOrDefault(p => p.UserName == userName);
            if (ac == null) return false;
            else
            {
                if (ac.Staffs[0].IsDeleted) return false;
                staffID = ac.Staffs[0].StaffID.ToString();
            }
        }
        return true;
    }
    //Kiểm tra tên một tài khoản có trùng với tên tài khoản nào khác trừ chính nó
    public static bool Exists(string staffID, string userName)
    {
        using (ManagementDispatchDataContext mdd = new ManagementDispatchDataContext())
        {
            Acount ac = mdd.Acounts.SingleOrDefault(p => p.UserName == userName);
            if (ac == null) return false;
            else
            {
                if (ac.Staffs[0].IsDeleted) return false;
                if (ac.Staffs[0].StaffID.ToString() != staffID) return true;
            }
        }
        return false;
    }
}
