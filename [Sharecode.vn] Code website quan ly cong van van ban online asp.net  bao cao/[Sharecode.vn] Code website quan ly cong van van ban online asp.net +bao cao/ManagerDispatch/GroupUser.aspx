<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="GroupUser.aspx.cs" Inherits="GroupUser" Title="Quản lý nhóm người dùng - Management Dispatch" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript">
function CheckOwner()
{
    var staffID = $('#' + '<%= hdStaffID.ClientID %>');
    if(staffID.val() == 'NotOwner')
    {
        alert('Bạn không có quyền để thực hiện thao tác này!');
        return false;
    }
    return true;
}
function CheckInput()
{
    if(!CheckOwner()) return false;
    var staffID = $('#' + '<%= hdStaffID.ClientID %>');
    if(staffID.val() == '0')
    {
        alert('Bạn chưa chọn nhân viên!');
        return false;
    }
    return true;
}
</script>
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
<div class="content_tabView_Doc">
    <div class="title_group">
            Quản lý nhóm người dùng
    </div>
    <asp:UpdatePanel ID="updatepanel_GroupUser" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <table class="tbUser" cellpadding="5" cellspacing="5" style="width:100%;">
    <tr>
        <td class="tdULeft" style="text-align:right;width:10%;"><span class="sp_title_left">ĐƠN VỊ: </span></td>
        <td class="tdURight" style="width:20%;">
            <asp:DropDownList ID="drDepartment" runat="server" style="width:99%;height:20px;line-height:20px;" 
            OnSelectedIndexChanged="drDepartment_SelectedIndexChanged" AutoPostBack="true">
            </asp:DropDownList>
        </td>
        <td class="tdURight" style="width:30%">
            <span class="sp_title_left">NHÓM NGƯỜI DÙNG:
            <asp:DropDownList ID="drGroupUser" runat="server" style="width:70%;height:20px;line-height:20px;text-transform:none;" 
                OnSelectedIndexChanged="drGroupUser_SelectedIndexChanged" AutoPostBack="true">
            </asp:DropDownList>
            </span>
        </td>
    </tr>
    <tr>
        <td class="tdULeft" style="text-align:right;width:5%;"><span class="sp_title_left">NHÂN VIÊN: </span></td>
        <td class="tdURight">
            <span style="margin-bottom:2px;"><asp:TextBox ID="txtStaff" runat="server" ReadOnly="true" style="width:70%;height:20px;line-height:20px;"></asp:TextBox></span>
            <span style="padding-left:5px;position:relative;">
            <asp:ImageButton ID="btSave" runat="server" AlternateText="Lưu" style="position:absolute;margin-left:3px;" ImageUrl="/ManagerDispatch/Images/Icon/luulai.png"
             BorderWidth="0" ToolTip="Lưu lại thiết lập" onclick="btSave_Click" OnClientClick="return CheckInput();" />
            <asp:ImageButton ID="btCancel" runat="server" AlternateText="Hủy" ImageUrl="/ManagerDispatch/Images/Icon/bt_cancel.png"
             style="position:absolute;left:103px;" BorderWidth="0" ToolTip="Bỏ qua nhân viên vừa chọn" OnClick="btCancel_Click" OnClientClick="return CheckOwner();" />
            </span>
        </td>
    </tr>
    </table>
     <div class="title_group">
        Danh sách nhân viên 
    </div>
    <asp:UpdateProgress ID="updateprocess_GroupUser" runat="server" AssociatedUpdatePanelID="updatepanel_GroupUser">
    <ProgressTemplate>
        <div style="text-align:center;color:#8497bf;height:20px;font-weight:bold;">Loading...</div>
    </ProgressTemplate>
    </asp:UpdateProgress>
    <div style="text-align: center; min-height: 20px;">
        <asp:Label ID="lbMsg_GroupUser" runat="server" Text=""></asp:Label>
        <div id="staffInfoDel" style="text-align: center; display: block; height: auto;">
        </div>
    </div>
    <div id="groupuser_data" style="width:100%;height:100%;">
        <asp:GridView ID="grGroupUser" runat="server" AutoGenerateColumns="False" OnRowDataBound="grGroupUser_RowDataBound"
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
             <asp:TemplateField HeaderText="Tên nhân viên">
                <ItemTemplate>
                 <%# DataBinder.Eval(Container.DataItem, "StaffName") %>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
             </asp:TemplateField>
             <asp:TemplateField HeaderText="Ngày sinh">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "Birthday")%>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" Width="70px" />
             </asp:TemplateField>
             <asp:TemplateField HeaderText="Tài khoản">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "UserName")%>
                </ItemTemplate>
             </asp:TemplateField>
             <asp:TemplateField HeaderText="Khóa TK">
                <ItemTemplate>
                    <asp:CheckBox runat="server" ID="chkBlockedAcount" Enabled="false" Checked='<%# bool.Parse(DataBinder.Eval(Container.DataItem, "AcountIsBlocked").ToString()) %>' />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" Width="40px" />
             </asp:TemplateField>
             <asp:TemplateField HeaderText="Đơn vị hiện tại">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "DepartmentName")%>
                </ItemTemplate>
             </asp:TemplateField>
             <asp:TemplateField HeaderText="Nhóm người dùng">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "GroupUserName")%>
                </ItemTemplate>
             </asp:TemplateField>
             <asp:TemplateField HeaderText="Tùy chọn">
                <ItemTemplate>
                    <asp:ImageButton ID="imgBtEdit" runat="server" AlternateText="Chọn" ImageUrl="/ManagerDispatch/Images/Icon/edit.png"
                        BorderWidth="0" OnClick="imgBtEdit_Click" CommandName='<%# Eval("STT") %>' CommandArgument='<%# Eval("StaffID") %>'
                        ToolTip="Chọn nhân viên để sửa" />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" Width="70px" />
            </asp:TemplateField>
         </Columns>
         </asp:GridView>
         <asp:HiddenField ID="hdStaffID" runat="server" Value="0" />
         <div style="margin-top: 7px; margin-left: 6px; font-weight: bold;">
            Trang:
            <asp:DropDownList ID="drPage" runat="server" Style="width: 40px;" AutoPostBack="true"
                OnSelectedIndexChanged="drPage_SelectedIndexChanged">
            </asp:DropDownList>
         </div>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</div>
</asp:Content>