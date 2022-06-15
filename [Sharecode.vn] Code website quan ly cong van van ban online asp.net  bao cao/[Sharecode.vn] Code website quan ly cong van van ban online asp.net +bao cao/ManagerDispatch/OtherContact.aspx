<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="OtherContact.aspx.cs" Inherits="OtherContact" Title="Đơn vị gửi nhận văn bản - Management Dispatch" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript">
function CheckOwner()
{
    var departmentAddressID = $('#' + '<%= hdDepartmentAddressID.ClientID %>');
    if(departmentAddressID.val() == 'NotOwner')
    {
        alert('Bạn không có quyền để thực hiện thao tác này!');
        return false;
    }
    return true;
}
function CheckInputOtherContact()
{
    if(!CheckOwner()) return false;
    var dAdd = $('#' + '<%= txtDepartmentAddressName.ClientID %>');
    if(dAdd.val() == '')
    {
        alert('Tên đơn vị không thể để trống!');
        dAdd.focus();
        return false;
    }
    return true;
}
</script>
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
<div class="content_tabView_Doc">
<table cellspacing="10" style="width:100%">
    <tr>
    <!-- OtherContact -->
     <td valign="top" width="50%">
       <div class="title_group">
          Đơn vị gửi nhận văn bản
       </div>
       <asp:UpdatePanel ID="updatepanel_OtherContact" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
        <div class="wrapperField">
          <table class="addField" cellspacing="3px">
             <tr>
                  <td align="right" width="20%" style="font-weight:bold;">Tên đơn vị: </td>
                  <td width="80%"><asp:TextBox ID="txtDepartmentAddressName" runat="server" style="width:90%;height:20px;line-height:20px;"></asp:TextBox></td>
             </tr>
             <tr>
                  <td align="right" style="font-weight:bold;">Ghi chú: </td>
                  <td><asp:TextBox ID="txtNote" runat="server" style="width:90%;height:20px;line-height:20px;"></asp:TextBox></td>
             </tr>
             <tr>
             <td align="right">&nbsp</td>
                 <td>
                     <asp:ImageButton ID="btAddOtherContact" runat="server" OnClientClick="return CheckInputOtherContact();"
                            ImageUrl="~/Images/Icon/bt_add_new.png" BorderWidth="0" ToolTip="Thêm mới, cập nhật!" onclick="btAddOtherContact_Click"  />
                     <asp:ImageButton ID="btCancelOtherContact" runat="server" 
                            ImageUrl="~/Images/Icon/bt_cancel.png" BorderWidth="0" ToolTip="Bỏ qua loại văn bản vừa chọn" onclick="btCancelOtherContact_Click" OnClientClick="return CheckOwner();" />
                 </td>
             </tr>
             <tr>
                 <td colspan="2" align="center">
                    <asp:UpdateProgress ID="updateprocess_OtherContact" runat="server" AssociatedUpdatePanelID="updatepanel_OtherContact">
                    <ProgressTemplate>
                        <div style="text-align:center;color:#8497bf;height:20px;font-weight:bold;">Loading...</div>
                    </ProgressTemplate>
                    </asp:UpdateProgress>
                 </td>
             </tr>
             <tr>
                 <td colspan="2" align="center">
                     <div><asp:Label ID="lbMsg_OtherContact" runat="server" Text=""></asp:Label></div>
                 </td>
            </tr>
          </table>
         <div style="display:block;float:left;width:100%;">
         <asp:GridView ID="grOtherContact" runat="server" GridLines="None" AutoGenerateColumns="False" DataKeyNames="Note"
                     Width="100%" SelectedRowStyle-BackColor="#CCFF99" OnRowDataBound="grOtherContact_RowDataBound">
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
                      <span> <%# DataBinder.Eval(Container.DataItem, "DepartmentAddresName")%></span>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" />
             </asp:TemplateField>
             <asp:TemplateField HeaderText="Văn bản">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container.DataItem, "TextCount")%>
                            </ItemTemplate>
                         <ItemStyle HorizontalAlign="Center" Width="60px" />
             </asp:TemplateField> 
             <asp:TemplateField HeaderText="Tùy chọn">
                <ItemTemplate>
                   <asp:ImageButton ID="imgBtEdit" runat="server" AlternateText="Chọn" ImageUrl="/ManagerDispatch/Images/Icon/edit.png" OnClick="imgBtEdit_OtherContact_Click" BorderWidth="0" CommandName='<%# Eval("STT") %>' CommandArgument='<%# Eval("DepartmentAddressID") %>' ToolTip="Chọn đơn vị để sửa" />
                   <asp:ImageButton ID="imgBtDelete" runat="server" AlternateText="Xóa" ImageUrl="/ManagerDispatch/Images/Icon/delete.png" OnClientClick="if(confirm('Bạn thật sự muốn xóa đối tượng được chọn?')){return confirm('Xóa đơn vị này này sẽ ảnh hưởng tới toàn bộ văn bản của hệ thống bạn có chắc muốn xóa không?')};return true;" OnClick="imgBtDelete_OtherContact_Click"
                    CommandArgument='<%# Eval("DepartmentAddressID") %>' BorderWidth="0" ToolTip="Xóa loại văn bản" />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" Width="80px" />
             </asp:TemplateField>
             </Columns>
             <AlternatingRowStyle BackColor="#FCFCFC" />
         </asp:GridView>
         <asp:HiddenField ID="hdDepartmentAddressID" runat="server" Value="0" />
         <div style="margin-top:7px;margin-left:6px;margin-bottom:10px;width:100%;text-align:center;font-weight:bold;">
                 Trang:
               <asp:DropDownList ID="drPage_OtherContact" runat="server" style="width:40px;" 
                  AutoPostBack="true" onselectedindexchanged="drPage_OtherContact_SelectedIndexChanged">
               </asp:DropDownList>
         </div>
         </div>
        </div>
        </ContentTemplate>
       </asp:UpdatePanel>
     </td>
     <td valign="top" width="50%">
     </td>
    </tr>
</table>
</div>
</asp:Content>

