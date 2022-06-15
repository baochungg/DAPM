<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="NatureText.aspx.cs" Inherits="NatureText" Title="Độ bảo mật và tính cấp thiết của văn bản - Management Dispatch" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript">
function CheckInputTextLevel()
{
    var tlName = $('#' + '<%= txtTextLevelName.ClientID %>');
    if(tlName.val() == '')
    {
        alert('Tên gọi không thể để trống!');
        tlName.focus();
        return false;
    }
    var tlPoint = $('#' + '<%= txtTextLevelPoint.ClientID %>');
    if(tlPoint.val() == '')
    {
        alert('Độ ưu tiên không thể để trống!');
        tlPoint.focus();
        return false;
    }
    return true;
}
function CheckInputTextSecurity()
{
    var tsName = $('#' + '<%= txtTextSecurityName.ClientID %>');
    if(tsName.val() == '')
    {
        alert('Tên độ bản mật văn bản không thể để trống!');
        tsName.focus();
        return false;
    }
}
</script>
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<div class="content_tabView_Doc">
    <table cellspacing="10" style="width:100%">
     <!-- Text Level -->
     <tr>
      <td valign="top" width="50%">
       <div class="title_group">
          Mức độ của văn bản
       </div>
       <asp:UpdatePanel ID="updatepanel_TextLevel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
         <div class="wrapperField">
          <table class="addField" cellspacing="3px">
          <tr>
              <td align="right" width="20%">Tên gọi: </td>
              <td width="80%"><asp:TextBox ID="txtTextLevelName" runat="server" style="width:90%;height:20px;line-height:20px;"></asp:TextBox></td>
          </tr>
          <tr>
              <td align="right">Độ ưu tiên: </td>
              <td><asp:TextBox ID="txtTextLevelPoint" runat="server" style="width:90%;height:20px;line-height:20px;"></asp:TextBox></td>
          </tr>
          <tr>
              <td align="right">Mô tả: </td>
              <td><asp:TextBox ID="txtTextLevelDescription" runat="server" style="width:90%;height:20px;line-height:20px;"></asp:TextBox></td>
          </tr>
          <tr>
             <td align="right">&nbsp</td>
             <td>
                 <asp:ImageButton ID="btAddTextLevel" runat="server" OnClientClick="return CheckInputTextLevel();"
                        ImageUrl="~/Images/Icon/bt_add_new.png" BorderWidth="0" ToolTip="Thêm mới, cập nhật!" onclick="btAddTextLevel_Click"  />
                 <asp:ImageButton ID="btCancelTextLevel" runat="server" 
                        ImageUrl="~/Images/Icon/bt_cancel.png" BorderWidth="0" ToolTip="Bỏ qua loại văn bản vừa chọn" onclick="btCancelTextLevel_Click" />
             </td>
          </tr>
          <tr>
             <td colspan="2" align="center">
                <asp:UpdateProgress ID="updateprocess_TextLevel" runat="server" AssociatedUpdatePanelID="updatepanel_TextLevel">
                <ProgressTemplate>
                    <div style="text-align:center;color:#8497bf;height:20px;font-weight:bold;">Loading...</div>
                </ProgressTemplate>
                </asp:UpdateProgress>
             </td>
          </tr>
          <tr>
             <td colspan="2" align="center">
                 <div><asp:Label ID="lbMsg_TextLevel" runat="server" Text=""></asp:Label></div>
             </td>
          </tr>
          </table>
          <div style="display:block;float:left;width:100%;">
          <asp:GridView ID="grTextLevel" runat="server" GridLines="None" AutoGenerateColumns="False" DataKeyNames="TextLevelDesciption"
                     Width="100%" SelectedRowStyle-BackColor="#FFFFCC" OnRowDataBound="grTextLevel_RowDataBound">
             <HeaderStyle CssClass="header_row_column_right" />
             <RowStyle CssClass="row_column_right" />
             <Columns>
             <asp:TemplateField HeaderText="TT">
                   <ItemTemplate>
                   <span style="padding: 5px 0;display:block;"><%# DataBinder.Eval(Container.DataItem, "STT") %></span>
                   </ItemTemplate>
                   <ItemStyle HorizontalAlign="Center" Width="30px" />
             </asp:TemplateField>
             <asp:TemplateField HeaderText="Tên độ ưu tiên">
                    <ItemTemplate>
                      <span> <%# DataBinder.Eval(Container.DataItem, "TextLevelName")%></span>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" />
             </asp:TemplateField>
             <asp:TemplateField HeaderText="Cấp độ">
                <ItemTemplate>
                    <%# DataBinder.Eval(Container.DataItem, "TextLevelPoint")%>
                    </ItemTemplate>
                 <ItemStyle HorizontalAlign="Center" Width="60px" />
             </asp:TemplateField> 
             <asp:TemplateField HeaderText="Tùy chọn">
                <ItemTemplate>
                   <asp:ImageButton ID="imgBtEdit" runat="server" AlternateText="Chọn" ImageUrl="/ManagerDispatch/Images/Icon/edit.png" OnClick="imgBtEdit_TextLevel_Click" BorderWidth="0" CommandName='<%# Eval("STT") %>' CommandArgument='<%# Eval("TextLevelID") %>' ToolTip="Chọn loại văn bản để sửa" />
                   <asp:ImageButton ID="imgBtDelete" runat="server" AlternateText="Xóa" ImageUrl="/ManagerDispatch/Images/Icon/delete.png" OnClientClick="if(confirm('Bạn thật sự muốn xóa đối tượng được chọn?')){return confirm('Xóa mức độ văn bản này sẽ ảnh hưởng tới toàn bộ văn bản của hệ thống bạn có chắc muốn xóa không?')};return true;" OnClick="imgBtDelete_TextLevel_Click"
                    CommandArgument='<%# Eval("TextLevelID") %>' BorderWidth="0" ToolTip="Xóa loại văn bản" />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" Width="80px" />
             </asp:TemplateField>
             </Columns>
             <AlternatingRowStyle BackColor="#FCFCFC" />
          </asp:GridView>
          <asp:HiddenField ID="hdTextLevelID" runat="server" Value="0" />
           <div style="margin-top:7px;margin-left:6px;margin-bottom:10px;width:100%;text-align:center;font-weight:bold;">
                 Trang:
               <asp:DropDownList ID="drPage_TextLevel" runat="server" style="width:40px;" 
                  AutoPostBack="true" onselectedindexchanged="drPage_TextLevel_SelectedIndexChanged">
               </asp:DropDownList>
            </div>
          </div>
         </div>
        </ContentTemplate>
       </asp:UpdatePanel>
      </td>
     <!-- End Text Level -->
     <!-- TextSecurity -->
     <td valign="top">
      <div class="title_group">
          Độ bảo mật của văn bản
      </div>
      <asp:UpdatePanel ID="updatepanel_TextSecurity" runat="server" UpdateMode="Conditional">
      <ContentTemplate>
        <div class="wrapperType">
        <table class="addField" cellspacing="3px">
        <tr>
          <td align="right" width="20%">Tên gọi: </td>
          <td width="80%"><asp:TextBox ID="txtTextSecurityName" runat="server" style="width:90%;height:20px;line-height:20px;"></asp:TextBox></td>
        </tr>
        <tr>
          <td align="right">Ghi chú: </td>
          <td><asp:TextBox ID="txtTextSecurityNote" runat="server" style="width:90%;height:20px;line-height:20px;"></asp:TextBox></td>
        </tr>
        <tr>
         <td align="right">&nbsp</td>
         <td>
             <asp:ImageButton ID="btAddTextSecurity" runat="server" AlternateText="Thêm, sửa" OnClientClick="return CheckInputTextSecurity();"
                    ImageUrl="/ManagerDispatch/Images/Icon/bt_add_new.png" BorderWidth="0" ToolTip="Thêm mới, cập nhật!" onclick="btAddTextSecurity_Click"  />
             <asp:ImageButton ID="btCancelTextSecurity" runat="server" AlternateText="Hủy"
                    ImageUrl="/ManagerDispatch/Images/Icon/bt_cancel.png" BorderWidth="0" ToolTip="Bỏ qua loại văn bản vừa chọn" onclick="btCancelTextSecurity_Click" />
         </td>
        </tr>
        <tr>
         <td colspan="2" align="center">
             <asp:Label ID="lbMsg_TextSecurity" runat="server" Text=""></asp:Label>
         </td>
        </tr>
        <tr>
         <td colspan="2" align="center">
            <asp:UpdateProgress ID="updateprocess_TextSecurity" runat="server" AssociatedUpdatePanelID="updatepanel_TextSecurity">
            <ProgressTemplate>
                <div style="text-align:center;color:#8497bf;height:20px;font-weight:bold;">Loading...</div>
            </ProgressTemplate>
            </asp:UpdateProgress>
         </td>
        </tr>
        </table>
        <div style="display:block;float:left;width:100%;">
        <asp:GridView ID="grTextSecurity" runat="server" GridLines="None" AutoGenerateColumns="False" DataKeyNames="Note"
             Width="100%" SelectedRowStyle-BackColor="#FFFFCC" OnRowDataBound="grTextSecurity_RowDataBound">
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
                  <span> <%# DataBinder.Eval(Container.DataItem, "TextSecurityName") %></span>
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Left" />
             </asp:TemplateField>
             <asp:TemplateField HeaderText="Tùy chọn">
                <ItemTemplate>
                   <asp:ImageButton ID="imgBtEdit" runat="server" AlternateText="Chọn" ImageUrl="/ManagerDispatch/Images/Icon/edit.png" OnClick="imgBtEdit_TextSecurity_Click" BorderWidth="0" CommandName='<%# Eval("STT") %>' CommandArgument='<%# Eval("TextSecurityID") %>' ToolTip="Chọn loại văn bản để sửa" />
                   <asp:ImageButton ID="imgBtDelete" runat="server" AlternateText="Xóa" ImageUrl="/ManagerDispatch/Images/Icon/delete.png" OnClientClick="if(confirm('Bạn thật sự muốn xóa đối tượng được chọn?')){return confirm('Xóa độ bảo mật bản này sẽ ảnh hưởng tới toàn bộ văn bản của hệ thống bạn có chắc muốn xóa không?')};return true;" OnClick="imgBtDelete_TextSecurity_Click" CommandArgument='<%# Eval("TextSecurityID") %>' BorderWidth="0" ToolTip="Xóa loại văn bản" />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" Width="80px" />
             </asp:TemplateField>
             </Columns>
             <AlternatingRowStyle BackColor="#FCFCFC" />
        </asp:GridView>
        <asp:HiddenField ID="hdTextSecurityID" runat="server" Value="0" />
        <div style="margin-top:7px;margin-left:6px;margin-bottom:10px;width:100%;text-align:center;font-weight:bold;">
               Trang:
             <asp:DropDownList ID="drPage_TextSecurity" runat="server" style="width:40px;" 
                  AutoPostBack="true" onselectedindexchanged="drPage_TextSecurity_SelectedIndexChanged">
             </asp:DropDownList>
         </div>
        </div>
        </div>
      </ContentTemplate>
      </asp:UpdatePanel>
     </td>
     </tr>
     <!-- End TextSecurity -->
     <tr>
     </tr>
    </table>
</div>
</asp:Content>

