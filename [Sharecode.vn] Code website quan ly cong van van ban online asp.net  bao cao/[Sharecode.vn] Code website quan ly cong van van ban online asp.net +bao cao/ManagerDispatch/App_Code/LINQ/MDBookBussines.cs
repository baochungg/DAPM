using System.Linq;
using System.Data;
using System.Web.UI.WebControls;
using System;
using LINQ;

public class MDBookBussines
{
    private ManagementDispatchDataContext MDData;
	public MDBookBussines()
	{
        MDData = new ManagementDispatchDataContext();
	}
    public void LoadBook(string departmentID, ref DataTable dtBook, int idxS, int idxE, ref int count)
    {
        var deparments = from p in MDData.DepartmentOfDepartments where p.DepartmentParentID.ToString() == departmentID select p;
        var book = from b in MDData.Books
                    where b.DepartmentID.ToString() == departmentID && !b.IsDeleted
                    select b;
        foreach (Book b in book)
        {
            count = count + 1;
            dtBook.Rows.Add(new object[] { count, b.BookID, b.BookName, b.BookPrefix, (b.BookDescription != null ? b.BookDescription : ""), MDData.TextInBooks.Count(p => p.BookID == b.BookID) });
        }
        foreach (DepartmentOfDepartment did in deparments)
        {
            NoopBook(ref dtBook, (from p in MDData.Departments where p.DepartmentID == did.DepartmentID select p), ref count);
        }
        DataTable result = dtBook.Clone();
        for (int i = idxS - 1; i < idxE; i++)
        {
            if (i == dtBook.Rows.Count) break;
            DataRow dr = result.NewRow();
            dr.ItemArray = dtBook.Rows[i].ItemArray;
            result.Rows.Add(dr);
        }
        dtBook = result;
    }
    private void NoopBook(ref DataTable dtBook, IQueryable departments, ref int count)
    {
        foreach (Department d in departments)
        {
            var book = from b in MDData.Books
                       where b.DepartmentID == d.DepartmentID && !b.IsDeleted
                       select b;
            foreach (Book b in book)
            {
                count = count + 1;
                dtBook.Rows.Add(new object[] { count, b.BookID, b.BookName, b.BookPrefix, (b.BookDescription != null ? b.BookDescription : ""), MDData.TextInBooks.Count(p => p.BookID == b.BookID) });
            }
            var dids = from p in MDData.DepartmentOfDepartments where p.DepartmentParentID == d.DepartmentID select p;
            foreach (DepartmentOfDepartment did in dids)
            {
                NoopBook(ref dtBook, (from p in MDData.Departments where p.DepartmentID == did.DepartmentID select p), ref count);
            }
        }
    }
    public Book GetBookInfo(string bookID)
    {
        return MDData.Books.SingleOrDefault(p => p.BookID.ToString() == bookID);
    }
    public void InsertBook(string departmentID, string bookName, string bookPrefix, string bookDescription)
    {
        Book b = new Book();
        b.BookID = Guid.NewGuid();
        b.DepartmentID = new Guid(departmentID);
        b.BookName = bookName;
        b.BookPrefix = bookPrefix;
        b.BookDescription = bookDescription;
        MDData.Books.InsertOnSubmit(b);
        MDData.SubmitChanges();
    }
    public void UpdateBook(string bookID, string departmentID, string bookName, string bookPrefix, string bookDescription)
    {
        Book b = MDData.Books.SingleOrDefault(p => p.BookID.ToString() == bookID);
        if (b != null)
        {
            b.DepartmentID = new Guid(departmentID);
            b.BookName = bookName;
            b.BookPrefix = bookPrefix;
            b.BookDescription = bookDescription;
            MDData.SubmitChanges();
        }
    }
    public void DeleteBook(string bookID, out int cText)
    {
        cText = 0;
        Book book = MDData.Books.Single(p => p.BookID.ToString() == bookID);
        if (book != null)
        {
            cText = book.TextInBooks.Count();
            if (cText != 0)
            {
                foreach (TextInBook tib in book.TextInBooks)
                {
                    MDData.TextInBooks.DeleteOnSubmit(tib);
                }
            }
            MDData.Books.DeleteOnSubmit(book);
            MDData.SubmitChanges();
        }
        
    }
}
