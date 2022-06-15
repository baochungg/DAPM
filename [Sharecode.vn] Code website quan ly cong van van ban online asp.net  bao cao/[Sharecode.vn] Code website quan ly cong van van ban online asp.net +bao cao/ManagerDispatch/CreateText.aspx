<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CreateText.aspx.cs" Inherits="CreateText" Title="Tạo và triển khai văn bản - Management Dispatch" %>
<%@ Register Assembly="Winthusiasm.HtmlEditor" Namespace="Winthusiasm.HtmlEditor" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentHead" Runat="Server">
    <link href="/ManagerDispatch/StyleSheet/createText.css" rel="stylesheet" type="text/css" />
    <script src="/ManagerDispatch/JS/datetimepicker_css.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript">
window.onbeforeunload = function(){
//    var nfa = <%= ((FileAttSession)Session["CurrentFileAttachment"]).FileAttIDs.Keys.Count %>;
//    if(nfa > 0)
//    {
//        if(confirm('Bạn có chắc chắn muốn rời khỏi trang trong khi có tệp đính kèm?'))
//        {
//            return true;
//        }
//        return false;
//    }
};
$(function(){
    var doc_cr = $('#' + '<%= txtDocCreateDate.ClientID %>');
    doc_cr.focus(function(){
        $('#imgbSelectCal').click();
    });
    var chkID = $('#' + '<%= chkInternalDocument.ClientID %>');
    chkID.click(function(){
        var drDA = $('#' + '<%= drDepartmentAddress.ClientID %>'); 
        if($(this).attr('checked'))
        {
            $('#spDepartmentAddr').hide();
            drDA.slideUp();
        }
        else
        {
            $('#spDepartmentAddr').show();
            drDA.slideDown();
        }
    });
    $('#' + '<%= drAddressTo.ClientID %>').change(function(){
        $('#sp_Staff').hide();
        $('#sp_GroupUser').hide();
        $('#sp_Department').hide();
        $('#sp_' + this[this.selectedIndex].value).show();
        $('#addressTo').html(this[this.selectedIndex].title);
    });
});
function CheckInput()
{
    var dateCreate = $('#' + '<%= txtDocCreateDate.ClientID %>');
    if(dateCreate.val() == '')
    {
        alert('Ngày ra văn bản không thể để trống!');
        dateCreate.focus();
        return false;
    }
    var date = $('#' + '<%= txtDateIssued.ClientID %>');
    if(date.val() == '')
    {
        alert('Ngày ban hành văn bản không thể để trống!');
        date.focus();
        return false;
    }
    if($('#' + '<%= drAddressTo.ClientID %>').val() == 'Staff')
    {
        var acountID = $('#' + '<%= t_staff.ClientID %>');
        if(acountID.val() == '')
        {
            alert('Bạn chưa nhập tên tài khoản!!');
            acountID.focus();
            return false;
        }
    }
    var strCreateDate = dateCreate.val();
    var dCr = new Date(parseInt(strCreateDate.substring(6, 10), 10), parseInt(strCreateDate.substring(3, 5), 10), parseInt(strCreateDate.substring(0, 2), 10));
    var strDate = date.val();
    var d = new Date(parseInt(strDate.substring(6, 10), 10), parseInt(strDate.substring(3, 5), 10), parseInt(strDate.substring(0, 2), 10));
    if(d < dCr)
    {
        alert('Ngày ban hành văn bản không thể trước ngày ra văn bản!');
        return false;
    }
    var strDateTreated = $('#' + '<%= txtDateTreated.ClientID %>').val();
    if(strDateTreated != '')
    {
        var dTr = new Date(parseInt(strDateTreated.substring(6, 10), 10), parseInt(strDateTreated.substring(3, 5), 10), parseInt(strDateTreated.substring(0, 2), 10));
        if(dTr < d)
        {
            alert('Ngày hết hiệu lực không thể trước ngày ban hành văn bản!');
            return false;
        }
    }
    if($find('<%= editorContent.ClientID %>').GetText() == '')
    {
        if(!confirm('Bạn có chắc chắn muốn gửi văn bản đi mà k có nội dung trích yếu?'))
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
        <!-- General Information -->
        <td valign="top" width="50%">
         <div class="title_group">
          Thông tin chung của văn bản
         </div>
         <table class="tbl_VB">
            <tr>
             <td class="tdVBLeft">
                &nbsp;
             </td>
             <td>
                <span><asp:CheckBox ID="chkInternalDocument" Checked="true" runat="server"/> Văn bản nội bộ</span>
             </td>
            </tr>
            <tr>
             <td class="tdVBLeft">
                <span id="spDepartmentAddr" style="display:none;">Đơn vị: </span>
             </td>
             <td style="height:auto;">
                <span>
                    <asp:DropDownList ID="drDepartmentAddress" runat="server" style="width:60%;display:none;">
                    </asp:DropDownList>
                </span>
             </td>
            </tr>
            <tr>
             <td class="tdVBLeft">
                Loại văn bản :
             </td>
             <td class="tdVBRight">
                 <asp:DropDownList ID="drTypeText" runat="server"></asp:DropDownList>
             </td>
            </tr>
            <tr>
             <td class="tdVBLeft">
                Số văn bản :
             </td>
             <td class="tdVBRight">
                 <asp:TextBox ID="txtTextNoCode" runat="server"></asp:TextBox>
             </td>
            </tr>
            <tr>
             <td class="tdVBLeft">
                Ngày ra văn bản :
             </td>
             <td class="tdVBRight">
                 <asp:TextBox ID="txtDocCreateDate" runat="server" Style="width: 70%; height: 20px; line-height: 20px;"
                                MaxLength="10"></asp:TextBox>
                <img id="imgbSelectCal" src="/ManagerDispatch/Images/Calendar/cal.gif" onclick="javascript:NewCssCal('<%= txtDocCreateDate.ClientID %>');"
                    style="cursor: pointer" alt="Chọn ngày" />
             </td>
            </tr>
            <tr>
             <td class="tdVBLeft">
                Người ký văn bản :
             </td>
             <td class="tdVBRight">
                 <asp:TextBox ID="txtSigner" runat="server"></asp:TextBox>
             </td>
            </tr>
          <tr>
             <td class="tdVBLeft">
                Tệp đính kèm :
             </td>
             <td class="tdVBRight">
                 <asp:GridView ID="grFileAttachment" runat="server" GridLines="None" AutoGenerateColumns="False" ShowHeader="false" Width="80%" style="margin-bottom:3px;">
                    <Columns>
                        <asp:TemplateField>
                               <ItemTemplate>
                                   <span style="font-style:italic;font-size:11px;"><a href="#" style="color:#008000;text-decoration:none;"
                                    onmouseover="$(this).css('text-decoration', 'underline');" 
                                    onmouseout="$(this).css('text-decoration', 'none');">
                                    <%# Eval("FileName").ToString() %></a>
                                   </span>
                               </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                               <ItemTemplate>
                                <asp:LinkButton ID="lbDeleteAtt" runat="server" style="color:Red;font-style:italic;" CommandArgument='<%# Eval("FileID") %>' OnClick="lbDeleteAtt_Click">Xóa</asp:LinkButton>
                               </ItemTemplate>
                               <ItemStyle HorizontalAlign="Center" Width="30px" />
                        </asp:TemplateField>
                    </Columns>
                 </asp:GridView>
                <span style="color:Red;"><asp:Label ID="lbFile" runat="server" Text=""></asp:Label></span>
                <span><asp:FileUpload type="file" ID="fuTextAttachment" runat="server" /></span><br />
                <span>
                    <asp:LinkButton  ID="lbUploadFile" runat="server" 
                     style="text-decoration:none;" 
                     onmouseover="$(this).css('text-decoration', 'underline');" 
                     onmouseout="$(this).css('text-decoration', 'none');" 
                     onclick="lbUploadFile_Click">Tải văn file đính kèm lên
                    </asp:LinkButton>
                </span>
             </td>
            </tr>   
         </table>
        </td>
        <!-- End General Information -->
        <td valign="top">
         <div class="title_group">
          Thông tin cho văn bản đi
         </div>
         <table class="tbl_VB">
        <tr>
         <td class="tdVBLeft">
            Độ cấp thiết của VB :
         </td>
         <td class="tdVBRight">
             <asp:DropDownList ID="drTextLevel" runat="server"></asp:DropDownList>
         </td>
        </tr>
        <tr>
         <td class="tdVBLeft">
            Yêu cầu bảo mật VB :
         </td>
         <td class="tdVBRight">
             <asp:DropDownList ID="drTextSecurity" runat="server"></asp:DropDownList>
         </td>
        </tr>
        <tr>
         <td class="tdVBLeft">
            Tính chất :
         </td>
         <td class="tdVBRight">
             <asp:DropDownList ID="drTextTreated" runat="server">
             <asp:ListItem Text="Văn bản cần xử lý" Value="1" Selected="True"></asp:ListItem>
             <asp:ListItem Text="Văn bản quy phạm" Value="0"></asp:ListItem>
             </asp:DropDownList>
         </td>
        </tr>
         <tr>
             <td class="tdVBLeft">
                <span> Gửi tới :</span>
             </td>
             <td class="tdVBRight">
                <asp:DropDownList ID="drAddressTo" runat="server">
                    <asp:ListItem Text="Nhân viên" Value="Staff" Selected="True" title="Tên tài khoản"></asp:ListItem>
                    <asp:ListItem Text="Nhóm người dùng" Value="GroupUser" title="Chọn nhóm người dùng"></asp:ListItem>
                    <asp:ListItem Text="Phòng ban" Value="Department" title="Chọn phòng ban"></asp:ListItem>
                </asp:DropDownList>
             </td>
          </tr>
         <tr>
             <td class="tdVBLeft">
                 <span id="addressTo">Tên tài khoản: </span>
             </td>
             <td class="tdVBRight">
                 <span id="sp_Staff"><asp:TextBox ID="t_staff" runat="server"></asp:TextBox></span>
                 <span id="sp_GroupUser" style="display:none;"><asp:DropDownList ID="drGroupUser" runat="server"></asp:DropDownList></span>
                 <span id="sp_Department" style="display:none;">
                     <asp:DropDownList ID="drDepartment" runat="server"></asp:DropDownList>
                 </span>
             </td>
          </tr>
         <tr>
             <td class="tdVBLeft">
                Ngày ban hành :
             </td>
             <td class="tdVBRight">
                 <asp:TextBox ID="txtDateIssued" runat="server" Style="width: 70%; height: 20px; line-height: 20px;"
                                MaxLength="10"></asp:TextBox>
                <img id="img1" src="/ManagerDispatch/Images/Calendar/cal.gif" onclick="javascript:NewCssCal('<%= txtDateIssued.ClientID %>');"
                    style="cursor: pointer" alt="Chọn ngày" />
             </td>
          </tr>
         <tr>
             <td class="tdVBLeft">
                Ngày hết hiệu lực :
             </td>
             <td class="tdVBRight">
                 <asp:TextBox ID="txtDateTreated" runat="server" Style="width: 70%; height: 20px; line-height: 20px;"
                                MaxLength="10"></asp:TextBox>
                <img id="img2" src="/ManagerDispatch/Images/Calendar/cal.gif" onclick="javascript:NewCssCal('<%= txtDateTreated.ClientID %>');"
                    style="cursor: pointer" alt="Chọn ngày" />
             </td>
          </tr>
         </table>
        </td>
    </tr>
    </table>
    <div style="margin-bottom:20px;width:90%;margin-top:-15px;margin-left:auto;margin-right:auto;position:relative;" id="txtEditor">
            Nội dung văn bản đi:
        <div style="margin-top:3px;">
            <cc1:HtmlEditor ID="editorContent" AutoSave="true" runat="server" Height="250px" Width="950px" />
        </div>
        <div style="position:absolute;right:0px;bottom:-8px;">
            <button id="btnSend" runat="server" class="minimal" style="width:100px;height:30px;" onclick="if(!CheckInput()) return false;" onserverclick="btnSend_Click">Gửi VB</button>
            <button id="btnCancel" runat="server" class="minimal" style="width:100px;height:30px;" onserverclick="btnCancel_Click">Hủy</button>
        </div>
    </div>
</div>
</asp:Content>
