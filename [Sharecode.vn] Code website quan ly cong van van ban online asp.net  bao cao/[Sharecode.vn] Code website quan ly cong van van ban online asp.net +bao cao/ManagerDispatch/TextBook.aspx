<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="TextBook.aspx.cs" Inherits="TextBook" Title="Quản lý loại văn bản và sổ văn bản - Management Dispatch" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript">
function CheckInputBook()
{
    var bName = document.getElementById('<%= txtBookName.ClientID %>');
    var bPrefix = document.getElementById('<%= txtBookPrefix.ClientID %>');
    if(bName.value == '')
    {
        alert('Tên sổ không thể để trống!');
        bName.focus();
        return false;
    }
    if(bPrefix.value == '')
    {
        alert('Tiền tố không thể để trống!');
        bPrefix.focus();
        return false;
    }
    $(this).attr('disabled', 'disabled');
    return true;
}
function CheckOwner()
{
    var ttID = $('#' + '<%= hdTypeTextID.ClientID %>');
    if(ttID.val() == 'NotOwner')
    {
        alert('Bạn không có quyền để thực hiện thao tác này!');
        return false;
    }
    return true;
}
function CheckInputTypeText()
{
    if(!CheckOwner()) return false;
    var ttName = document.getElementById('<%= txtTypeTextName.ClientID %>');
    if(ttName.value == '')
    {
        alert('Tên loại văn bản không thể để trống!');
        ttName.focus();
        return false;
    }
    return true;
}
</script>
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<div class="content_tabView_Doc">
   <table cellspacing="10" style="width:100%">
    <tr>
        <!-- Book -->
        <td valign="top" width="50%">
         <div class="title_group">
          Sổ văn bản
         </div>
         <asp:UpdatePanel ID="updatepanel_Book" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            <div class="wrapperField">
             <table class="addField" cellspacing="3px">
                <tr>
                 <td align="right" width="20%"> Phòng ban :</td>
                 <td width="80%">
                     <asp:DropDownList ID="drDepartment" runat="server" style="padding:3px;width:90%;">
                     </asp:DropDownList>
                 </td>
                </tr>
                <tr>
                 <td align="right">Tên sổ :</td>
                 <td><asp:TextBox ID="txtBookName" runat="server" style="width:90%;height:20px;line-height: 20px;"></asp:TextBox></td>
                </tr>
                <tr>
                 <td align="right">Tiền tố :</td>
                 <td><asp:TextBox ID="txtBookPrefix" runat="server" style="width:90%;height:20px;line-height: 20px;"></asp:TextBox></td>
                </tr>
                <tr>
                 <td align="right">Mô tả :</td>
                 <td><asp:TextBox ID="txtDescriptionBook" runat="server" style="width:90%;height:20px;line-height: 20px;"></asp:TextBox></td>
                </tr>
                <tr>
                 <td align="right">&nbsp</td>
                 <td>
                 <asp:ImageButton ID="btAddBook" runat="server" AlternateText="Thêm, sửa"
                        ImageUrl="~/Images/Icon/bt_add_new.png" BorderWidth="0" ToolTip="Thêm mới, cập nhật!" onclick="btAddBook_Click" OnClientClick="return CheckInputBook();"  />
                 <asp:ImageButton ID="btCancelBook" runat="server" AlternateText="Hủy"
                        ImageUrl="~/Images/Icon/bt_cancel.png" BorderWidth="0" ToolTip="Bỏ qua sổ vừa chọn" onclick="btCancelBook_Click" />
                 </td>
                </tr>
                <tr>
                 <td colspan="2" align="center">
                     <asp:Label ID="lbMsg_Book" runat="server" Text=""></asp:Label>
                 </td>
                </tr>
                <tr>
                 <td colspan="2" align="center">
                    <asp:UpdateProgress ID="uprDeparment" runat="server" AssociatedUpdatePanelID="updatepanel_Book">
                    <ProgressTemplate>
                        <div style="text-align:center;color:#8497bf;height:30px;font-weight:bold;">Loading...</div>
                    </ProgressTemplate>
                    </asp:UpdateProgress>
                 </td>
                </tr>
             </table>
             <div style="width: 100%; display: block; float: left;">
                 <asp:GridView ID="grBook" runat="server" GridLines="None" AutoGenerateColumns="False" DataKeyNames="BookDescription"
                     Width="100%" SelectedRowStyle-BackColor="#FFFFCC" OnRowDataBound="grBook_RowDataBound">
                     <HeaderStyle CssClass="header_row_column_right" />
                     <RowStyle CssClass="row_column_right" />
                     <Columns>
                        <asp:TemplateField HeaderText="TT">
                           <ItemTemplate>
                           <span style="padding: 5px 0;display:block;"><%# DataBinder.Eval(Container.DataItem, "STT") %></span>
                           </ItemTemplate>
                           <ItemStyle HorizontalAlign="Center" Width="30px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tên sổ">
                            <ItemTemplate>
                              <span> <%# DataBinder.Eval(Container.DataItem, "BookName") %></span>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tiền tố">
                        <ItemTemplate>
                           <span style="padding: 5px 0;display:block;"><%# DataBinder.Eval(Container.DataItem, "BookPrefix") %></span>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="70px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Văn bản">
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container.DataItem, "TextCount")%>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="50px" />
                        </asp:TemplateField>    
                        <asp:TemplateField HeaderText="Tùy chọn">
                            <ItemTemplate>
                               <asp:ImageButton ID="imgBtEdit" runat="server" AlternateText="Chọn" ImageUrl="/ManagerDispatch/Images/Icon/edit.png" OnClick="imgBtEdit_Book_Click" BorderWidth="0" CommandName='<%# Eval("STT") %>' CommandArgument='<%# Eval("BookID") %>' ToolTip="Chọn sổ để sửa" />
                               <asp:ImageButton ID="imgBtDelete_Book" runat="server" AlternateText="Xóa" ImageUrl="/ManagerDispatch/Images/Icon/delete.png" OnClientClick="if(confirm('Bạn thật sự muốn xóa đối tượng được chọn?')){return confirm('Xóa sổ văn bản này sẽ xóa toàn bộ văn bản của sổ bạn có chắc muốn xóa không?')};return true;"
                               OnClick="imgBtDelete_Book_Click" CommandArgument='<%# Eval("BookID") %>' BorderWidth="0" ToolTip="Xóa sổ" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="60px" />
                        </asp:TemplateField>
                     </Columns>
                     <AlternatingRowStyle BackColor="#FCFCFC" />
                 </asp:GridView>
                 <asp:HiddenField ID="hdBookID" runat="server" Value="0" />
                 <div style="margin-top:7px;margin-left:6px;margin-bottom:10px;width:100%;text-align:center;font-weight:bold;">
                       Trang:
                     <asp:DropDownList ID="drPage_Book" runat="server" style="width:40px;" 
                          AutoPostBack="true" onselectedindexchanged="drPage_Book_SelectedIndexChanged">
                     </asp:DropDownList>
                 </div>
             </div>
            </div>
            </ContentTemplate>
            </asp:UpdatePanel>
        </td>
        <!-- End Book -->
        <!-- TypeText -->
        <td valign="top">
         <div class="title_group">
          Loại văn bản
         </div>
         <asp:UpdatePanel ID="updatepanel_TypeText" runat="server" UpdateMode="Conditional">
         <ContentTemplate>
            <div class="wrapperType">
                <table class="addField" cellspacing="3px">
                 <tr>
                  <td align="right" width="35%">Tên loại văn bản: </td>
                  <td width="65%"><asp:TextBox ID="txtTypeTextName" runat="server" style="width:99%;height:20px;line-height:20px;"></asp:TextBox></td>
                 </tr>
                 <tr>
                  <td align="right">Mô tả cho loại văn bản này: </td>
                  <td><asp:TextBox ID="txtTypeTextDescription" runat="server" style="width:99%;height:20px;line-height:20px;"></asp:TextBox></td>
                 </tr>
                 <tr>
                 <td align="right">&nbsp</td>
                 <td>
                     <asp:ImageButton ID="btAddTypeText" runat="server" AlternateText="Thêm, sửa"
                            ImageUrl="/ManagerDispatch/Images/Icon/bt_add_new.png" BorderWidth="0" ToolTip="Thêm mới, cập nhật!" onclick="btAddTypeText_Click" OnClientClick="return CheckInputTypeText();" />
                     <asp:ImageButton ID="btCancelTypeText" runat="server" AlternateText="Hủy"
                            ImageUrl="/ManagerDispatch/Images/Icon/bt_cancel.png" BorderWidth="0" ToolTip="Bỏ qua loại văn bản vừa chọn" onclick="btCancelTypeText_Click" OnClientClick="return CheckOwner();" />
                 </td>
                </tr>
                <tr>
                 <td colspan="2" align="center">
                     <asp:Label ID="lbMsg_TypeText" runat="server" Text=""></asp:Label>
                 </td>
                </tr>
                <tr>
                 <td colspan="2" align="center">
                    <asp:UpdateProgress ID="updateprocess_TypeText" runat="server" AssociatedUpdatePanelID="updatepanel_TypeText">
                    <ProgressTemplate>
                        <div style="text-align:center;color:#8497bf;height:30px;font-weight:bold;">Loading...</div>
                    </ProgressTemplate>
                    </asp:UpdateProgress>
                 </td>
                </tr>
                </table>
                <div style="display:block;float:left;width:100%;">
                <asp:GridView ID="grTypeText" runat="server" GridLines="None" AutoGenerateColumns="False" DataKeyNames="TypeTextDescription"
                     Width="100%" SelectedRowStyle-BackColor="#FFFFCC" OnRowDataBound="grTypeText_RowDataBound">
                     <HeaderStyle CssClass="header_row_column_right" />
                     <RowStyle CssClass="row_column_right" />
                     <Columns>
                     <asp:TemplateField HeaderText="TT">
                           <ItemTemplate>
                           <span style="padding: 5px 0;display:block;"><%# DataBinder.Eval(Container.DataItem, "STT") %></span>
                           </ItemTemplate>
                           <ItemStyle HorizontalAlign="Center" Width="30px" />
                     </asp:TemplateField>
                     <asp:TemplateField HeaderText="Tên loại văn bản">
                            <ItemTemplate>
                              <span> <%# DataBinder.Eval(Container.DataItem, "TypeTextName") %></span>
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
                           <asp:ImageButton ID="imgBtEdit" runat="server" AlternateText="Chọn" ImageUrl="/ManagerDispatch/Images/Icon/edit.png" OnClick="imgBtEdit_Text_Click" BorderWidth="0" CommandName='<%# Eval("STT") %>' CommandArgument='<%# Eval("TypeTextID") %>' ToolTip="Chọn loại văn bản để sửa" />
                           <asp:ImageButton ID="imgBtDelete_TypeText" runat="server" AlternateText="Xóa" ImageUrl="/ManagerDispatch/Images/Icon/delete.png" OnClientClick="if(confirm('Bạn thật sự muốn xóa đối tượng được chọn?')){return confirm('Xóa loại văn bản này sẽ ảnh hưởng tới toàn bộ văn bản của hệ thống bạn có chắc muốn xóa không?')};return true;"
                           OnClick="imgBtDelete_TypeText_Click" CommandArgument='<%# Eval("TypeTextID") %>' BorderWidth="0" ToolTip="Xóa loại văn bản" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                     </asp:TemplateField>
                    </Columns>
                    <AlternatingRowStyle BackColor="#FCFCFC" />
                </asp:GridView>
                <asp:HiddenField ID="hdTypeTextID" runat="server" Value="0" />
                <div style="margin-top:7px;margin-left:6px;margin-bottom:10px;width:100%;text-align:center;font-weight:bold;">
                       Trang:
                     <asp:DropDownList ID="drPage_TypeText" runat="server" style="width:40px;" 
                          AutoPostBack="true" onselectedindexchanged="drPage_TypeText_SelectedIndexChanged">
                     </asp:DropDownList>
                 </div>
                </div>
            </div>
         </ContentTemplate>
         </asp:UpdatePanel>
        </td>
        <!-- End TypeText -->
    </tr>
   </table>
 </div>
</asp:Content>

