<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Department.aspx.cs" Inherits="Default3" Title="MD2011 - Quản lý phòng ban đơn vị" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript">
function CheckOwner()
{
    var dpID = $('#' + '<%= hdDepartmentID.ClientID %>');
    if(dpID.val() == 'NotOwner')
    {
        alert('Bạn không có quyền để thực hiện thao tác này!');
        return false;
    }
    return true;
}
function CheckInputDepartment()
{
    if(!CheckOwner()) return false;
    var dp = $('#' + '<%= txtDepartment.ClientID %>');
    if(dp.val() == '')
    {
        alert('Tên phòng ban không thể để trống!');
        dp.focus();
        return false;
    }
    else
    {
        $(this).attr('disabled', 'disabled');
        return true;
    }
}
</script>
    <div class="content_tabView_Doc">
    <div class="title_group">
     Danh sách phòng ban đơn vị hiện tại
    </div>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdateProgress ID="uprDeparment" runat="server" AssociatedUpdatePanelID="uplDepartment">
    <ProgressTemplate>
        <div style="text-align:center;color:#8497bf;height:20px;font-weight:bold;">Loading...</div>
    </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="uplDepartment" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <div style="text-align:center;height:20px;">
        <span id="lbMsg" style="color:Red;font-weight:bold;" runat="server"></span>
    </div>
    <div id="deparment_data" style="width:100%;height:100%;">
        <asp:GridView ID="grDepartment" runat="server" AutoGenerateColumns="False" OnRowDataBound="grDepartment_RowDataBound"
         GridLines="None" Width="100%" SelectedRowStyle-BackColor="#FFFFCC">
         <HeaderStyle CssClass="header_row_column_right" />
         <RowStyle CssClass="row_column_right" />
         <Columns>
             <asp:TemplateField HeaderText="TT">
                <ItemTemplate>
                    <span style="padding: 5px 0;display:block;"><%# DataBinder.Eval(Container.DataItem, "STT") %></span>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" Width="30px" />
             </asp:TemplateField>
             <asp:TemplateField HeaderText="Tên đơn vị">
                <ItemTemplate>
                 <%# DataBinder.Eval(Container.DataItem, "DepartmentName") %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
             </asp:TemplateField>
             <asp:TemplateField HeaderText="Nhân viên">
                <ItemTemplate>
                 <%# DataBinder.Eval(Container.DataItem, "StaffCount")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" Width="70px" />
             </asp:TemplateField>
             <asp:TemplateField HeaderText="Sổ văn bản">
                <ItemTemplate>
                 <%# DataBinder.Eval(Container.DataItem, "BookCount")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" Width="70px" />
             </asp:TemplateField>
             <asp:TemplateField HeaderText="Tùy chọn">
                <ItemTemplate>
                    <asp:ImageButton ID="imgBtEdit" runat="server" AlternateText="Chọn" ImageUrl="/ManagerDispatch/Images/Icon/edit.png" BorderWidth="0" OnClick="imgBtEdit_Click" CommandName='<%# Eval("STT") %>' CommandArgument='<%# Eval("DepartmentID") %>' ToolTip="Chọn đơn vị để sửa" />
                    <asp:ImageButton ID="imgBtDelete" runat="server" AlternateText="Xóa" ImageUrl="/ManagerDispatch/Images/Icon/delete.png" OnClientClick="return confirm('Bạn thật sự muốn xóa đơn vị được chọn và toàn bộ đơn vị thuộc phòng ban này?');" OnClick="imgBtDelete_Click" CommandArgument='<%# Eval("DepartmentID") %>' BorderWidth="0" ToolTip="Xóa đơn vị" />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" Width="80px" />
             </asp:TemplateField>
            </Columns>    
            <AlternatingRowStyle BackColor="#FCFCFC" />
         </asp:GridView>
        <asp:HiddenField ID="hdDepartmentID" runat="server" Value="0" />
        <div style="margin-top:7px;margin-left:6px;font-weight:bold;">
              Trang:
            <asp:DropDownList ID="drPage" runat="server" style="width:40px;" 
                  AutoPostBack="true" onselectedindexchanged="drPage_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
        </div>
        <div class="title_group">
          Thêm phòng ban đơn vị
        </div>
        <table class="tblGroupUser">
            <tr>
                <td align="right"> Tên đơn vị :</td>
                <td><asp:TextBox ID="txtDepartment" runat="server" class="TextBox"></asp:TextBox></td>
                <td align="right">Đơn vị trực thuộc:</td>
                <td align="left">
                <asp:DropDownList ID="drParentDepartment" runat="server" >
                </asp:DropDownList>   
                </td>
                <td>
                    <asp:ImageButton ID="btAdd" runat="server" AlternateText="Thêm, sửa" OnClientClick="return CheckInputDepartment();"
                        ImageUrl="~/Images/Icon/bt_add_new.png" BorderWidth="0" 
                        ToolTip="Thêm mới, cập nhật!" onclick="btAdd_Click" />
                    <asp:ImageButton ID="btCancel" runat="server" AlternateText="Hủy"
                        ImageUrl="~/Images/Icon/bt_cancel.png" BorderWidth="0" 
                        ToolTip="Bỏ qua đơn vị vừa chọn" onclick="btCancel_Click" OnClientClick="return CheckOwner();" />
                </td>
            </tr>
        </table>
    </ContentTemplate>
    </asp:UpdatePanel>
</div>
</asp:Content>

