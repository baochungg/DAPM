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

namespace LOGIN
{
    public class Acount
    {
        private bool isAdminAcount;
        private LINQ.Acount acount;
        private OwnerAdmin ownerAdmin;
        private OwnerText ownerText;
        public LINQ.Staff StaffLogged
        {
            get { return this.acount.Staffs[0]; }
            set { this.acount.Staffs[0] = value; }
        }
        public bool IsAdminAcount
        {
            get { return this.isAdminAcount; }
            set { this.isAdminAcount = value; }
        }
        public LINQ.Acount AcountLogged
        {
            get { return this.acount; }
            set { this.acount = value; }
        }
        public LINQ.StaffGroupUser StaffGroupUserLogged
        {
            get { return this.StaffLogged.StaffGroupUser; }
        }
        public LINQ.GroupUser GroupUserLogged
        {
            get { return this.acount.Staffs[0].StaffGroupUser.GroupUser; }
        }
        public OwnerAdmin OwnerAdminLogged
        {
            get { return this.ownerAdmin; }
            set { this.ownerAdmin = value; }
        }
        public OwnerText OwnerTextLogged
        {
            get { return this.ownerText; }
            set { this.ownerText = value; }
        }
        public Acount(LINQ.Acount ac)
        {
            this.acount = ac;
            this.isAdminAcount = this.acount.IsAdminAcount;
            if (!isAdminAcount)
            {
                this.ownerAdmin = new OwnerAdmin(this.acount.Staffs[0].StaffGroupUser.GroupUser.OwnerGroupUsers[0].OwnerAdmin);
                this.ownerText = new OwnerText(this.acount.Staffs[0].StaffGroupUser.GroupUser.OwnerGroupUsers[0].OwnerText);
            }
        }
    }
    public class OwnerAdmin
    {
        //private string defaultOwnerAdminID = "7a76efc0-2ee7-41e6-a141-186a0c601c7b";
        private LINQ.OwnerAdmin ownerAdmin;
        public LINQ.OwnerAdmin OwnerAdminLogged
        {
            get { return this.ownerAdmin; }
            set { this.ownerAdmin = value; }
        }
        public bool AllOwner
        {
            get { return this.ownerAdmin.All; }
        }
        public bool NotOwnerAdmin
        {
            get
            {
                if (NotOwnerDepartment && !this.ownerAdmin.GroupUserAdmin && NotOwnerStaff && !this.ownerAdmin.SystemLogDelete
                    && !this.ownerAdmin.SystemLogView && !this.NotOwnerAdmin)
                {
                    return true;
                }
                return false;
            }
        }
        public bool AllOwnerDepartment
        {
            get
            {
                if (this.ownerAdmin.DepartmentCreate && this.ownerAdmin.DepartmentDelete && this.ownerAdmin.DepartmentModify && this.ownerAdmin.DepartmentReadOnly)
                    return true;
                return false;
            }
        }
        public bool NotOwnerDepartment
        {
            get
            {
                if (!this.ownerAdmin.DepartmentCreate && !this.ownerAdmin.DepartmentDelete && !this.ownerAdmin.DepartmentModify && !this.ownerAdmin.DepartmentReadOnly)
                    return true;
                return false;
            }
        }
        public bool AllOwnerStaff
        {
            get
            {
                if (this.ownerAdmin.StaffCreate && this.ownerAdmin.StaffDelete && this.ownerAdmin.StaffModify && this.ownerAdmin.StaffReadOnly)
                    return true;
                return false;
            }
        }
        public bool NotOwnerStaff
        {
            get
            {
                if (!this.ownerAdmin.StaffCreate && !this.ownerAdmin.StaffDelete && !this.ownerAdmin.StaffModify && !this.ownerAdmin.StaffReadOnly)
                    return true;
                return false;
            }
        }
        public OwnerAdmin(LINQ.OwnerAdmin oa)
        {
            this.ownerAdmin = oa;
        }
    }
    public class OwnerText
    {
        //private string defaultOwnerTextID = "1aa7944b-1869-4425-9dd5-1aa425d95224";
        private LINQ.OwnerText ownerText;
        public LINQ.OwnerText OwnerTextLogged
        {
            get { return this.ownerText; }
        }
        public bool AllOwner
        {
            get { return this.ownerText.All; }
        }
        public bool AllOwnerBook
        {
            get
            {
                if (this.ownerText.BookCreate && this.ownerText.BookDelete && this.ownerText.BookModify && this.ownerText.BookReadOnly)
                    return true;
                return false;
            }
        }
        public bool NotOwnerBook
        {
            get
            {
                if (!this.ownerText.BookCreate && !this.ownerText.BookDelete && !this.ownerText.BookModify && !this.ownerText.BookReadOnly)
                    return true;
                return false;
            }
        }
        public OwnerText(LINQ.OwnerText ot)
        {
            this.ownerText = ot;
        }
    }
}