<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="TextTo.aspx.cs" Inherits="TextTo" Title="Văn bản đến - Management Dispatch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentHead" Runat="Server">
    <link href="/ManagerDispatch/StyleSheet/text.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .selectedRow{background:none repeat scroll 0 0 #FFFFCC;}
    </style>
    <script src="/ManagerDispatch/JS/datetimepicker_css.js" type="text/javascript"></script>
    <script src="JS/EllipsisText.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<script type="text/javascript">
//$(function(){
//    var d_t = $('#' + '<%= txtDateTo.ClientID %>');
//    d_t.focus(function(){
//        $('#imgbSelectCal').click();
//    });
//});
$(".ellipsis").ellipsis();
$(function(){
    $('#' + '<%= chkSelectAll.ClientID %>').change(function(){
    });
});
function chkSelectTextTo_CheckedChange(chkID)
{
    $('#' + chkID).parent().parent().parent().toggleClass('selectedRow');
}
</script>
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
<div class="content_tabView_Doc">
    <asp:UpdatePanel ID="updatepanel_TextTo" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
    <div class="title_group">
        <asp:Label ID="lbTitle" runat="server" Text=" Lựa chọn điều kiện tìm kiếm "></asp:Label>
    </div>
    <div id="topSearch" runat="server">
    <table class="tablSearch" cellpadding="5" cellspacing="5">
        <tr>
             <td align="right" style="width:15%;">
               Loại văn bản : 
             </td>
             <td>
                <asp:DropDownList ID="drTypeText" runat="server"></asp:DropDownList>
             </td>
             <td align="right" style="width:15%;">
                Số văn bản : 
             </td>
             <td>
                 <asp:TextBox ID="txtTextNoCode" runat="server" style="width:60%;height:20px;line-height:20px;" placeholder="- Tìm tất cả -"></asp:TextBox>
             </td>
             <td>&nbsp;</td>
             <td>&nbsp;</td>
        </tr>
        <tr>
             <td align="right">
               Độ cấp thiết của VB : 
             </td>
             <td>
                <asp:DropDownList ID="drTextLevel" runat="server"></asp:DropDownList>
             </td>
             <td align="right">
                Thời gian đến : 
             </td>
             <td>
                 <asp:TextBox ID="txtDateTo" runat="server" style="width:60%;height:20px;line-height:20px;" placeholder="- Tìm tất cả -"></asp:TextBox>
                 <img id="imgbSelectCal" src="/ManagerDispatch/Images/Calendar/cal.gif" onclick="javascript:NewCssCal('<%= txtDateTo.ClientID %>');"
                    style="cursor: pointer" alt="Chọn ngày" />
             </td>
        </tr>
        <tr>
             <td align="right">
               Yêu cầu bảo mật : 
             </td>
             <td>
                <asp:DropDownList ID="drTextSecurity" runat="server"></asp:DropDownList>
             </td>
             <td align="right">
                Ngày ban hành : 
             </td>
             <td>
                 <asp:TextBox ID="txtDateIssued" runat="server" style="width:60%;height:20px;line-height:20px;" placeholder="- Tìm tất cả -"></asp:TextBox>
                 <img id="img1" src="/ManagerDispatch/Images/Calendar/cal.gif" onclick="javascript:NewCssCal('<%= txtDateIssued.ClientID %>');"
                    style="cursor: pointer" alt="Chọn ngày" />
             </td>
        </tr>
        <tr>
             <td align="right">
               Tình trạng xử lý : 
             </td>
             <td>
                <asp:DropDownList ID="drStateText" runat="server"></asp:DropDownList>
             </td>
             <td align="right">
                Ngày hết hiệu lực : 
             </td>
             <td>
                 <asp:TextBox ID="txtDateTreated" runat="server" style="width:60%;height:20px;line-height:20px;" placeholder="- Tìm tất cả -"></asp:TextBox>
                 <img id="img2" src="/ManagerDispatch/Images/Calendar/cal.gif" onclick="javascript:NewCssCal('<%= txtDateTreated.ClientID %>');"
                    style="cursor: pointer" alt="Chọn ngày" />
             </td>
        </tr>
        <tr>
            <td align="right">
               Tài khoản gửi tới : 
             </td>
             <td>
                <asp:TextBox ID="txtUserName" runat="server" style="width:95%;height:20px;line-height:20px;" placeholder="- Tìm tất cả -"></asp:TextBox>
             </td>
             <td>&nbsp;</td>
             <td>
                <asp:ImageButton ID="btSearch" 
                     ImageUrl="/ManagerDispatch/Images/Icon/btSearch.gif" runat="server" 
                     style="border-width:0px;" onclick="btSearch_Click" />
             </td>
        </tr>
    </table>
    </div>
    <div id="lTitle" runat="server" class="title_group" style="margin-bottom:7px;">
        Danh sách văn bản
    </div>
    <div>
        <div style="position:relative;height:36px;">
            <asp:UpdateProgress ID="uprTextTo" runat="server" AssociatedUpdatePanelID="updatepanel_TextTo">
             <ProgressTemplate>
                <div style="width:100%;text-align:center;display: inline-block;position:absolute;"><span style="font-weight:bold;color:#4C2222;background:#F9EDBE;padding:2px 10px 2px 10px;">Đang tải...</span></div>
             </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="controlTextTo1" runat="server" style="display: inline-block;">
               <div style="margin-bottom:10px;position:absolute;" >
                <div id="tagSelectAll" runat="server" class="btTextTo" style="-moz-user-select: none;" onmousedown="$(this).css('border', '1px solid #4D90FE');" onmouseup="$(this).css('border', '1px solid rgba(0, 0, 0, 0.1)');" title="Chọn toàn bộ">
                    <div style="display: inline-block;">
                        <span style="position:relative;top:7px;"><asp:CheckBox ID="chkSelectAll" runat="server" /></span>
                        <div>&nbsp;</div>
                    </div>
                </div>
               </div> 
               <div style="margin-bottom:10px;position:absolute;left:90px;" >
                <div id="tagRefresh" runat="server" class="btTextTo" onclick="btRefresh_Click();" style="-moz-user-select: none;" onmousedown="$(this).css('border', '1px solid #4D90FE');" onmouseup="$(this).css('border', '1px solid rgba(0, 0, 0, 0.1)');" title="Làm mới">
                    <div style="display: inline-block;">
                        <span style="width:0px;">&nbsp;</span>
                        <div class="btTextTo_icon" style="background: url('/ManagerDispatch/Images/Icon/button_icon.png') no-repeat scroll -63px -21px transparent;"></div>
                    </div>
                    <script type="text/javascript">
                        function btRefresh_Click()
                        {
                            document.getElementById('<%= btSearch.ClientID %>').click();
                        }
                    </script>
                </div>
              </div>
            </div>
            <div id="controlTextTo2" runat="server" visible="false" style="display: inline-block;">
                <div id="tagBack" class="btTextTo" style="position:relative;top:-6px; -moz-user-select: none;" onmousedown="$(this).css('border', '1px solid #4D90FE');" onmouseup="$(this).css('border', '1px solid rgba(0, 0, 0, 0.1)');" 
                onclick="document.getElementById('<%= btBack.ClientID %>').click();" title="Trở lại">
                    <div style="display: inline-block;">
                        <span style="width:0px;"></span>
                        <div><div class="btTextTo_icon" style="position:relative;top:5px; background: url('/ManagerDispatch/Images/Icon/button_icon.png') no-repeat scroll -0px -42px transparent;"></div></div>
                    </div>
                    <asp:Button ID="btBack" runat="server" Text="" style="display:none;width:0px;height:0px;" OnClick="btBack_Click" />
                </div>
                <div id="tagSaveTextTo" runat="server" visible="true" class="btTextTo" style="-moz-user-select: none;border-bottom-right-radius: 0;border-top-right-radius: 0;margin-right: 0;" onmousedown="$(this).css('border', '1px solid #4D90FE');" onmouseup="$(this).css('border', '1px solid rgba(0, 0, 0, 0.1)');" title="Lưu trữ">
                    <div style="display: inline-block;">
                        <span style="width:0px;">&nbsp;</span>
                        <div class="btTextTo_icon" style="background: url('/ManagerDispatch/Images/Icon/button_icon.png') no-repeat scroll -84px -21px transparent;"></div>
                    </div>
                </div>
                <div id="tagDeleteTextTo" runat="server" visible="True" class="btTextTo" style="-moz-user-select: none;border-bottom-left-radius: 0;border-top-left-radius: 0; border-left-width:0px;margin-left: -4px;" onmousedown="$(this).css('border', '1px solid #4D90FE');" onmouseup="$(this).css('border', '1px solid rgba(0, 0, 0, 0.1)');" title="Xóa văn bản">
                    <div style="display: inline-block;">
                        <span style="width:0px;">&nbsp;</span>
                        <div class="btTextTo_icon" style="background: url('/ManagerDispatch/Images/Icon/button_icon.png') no-repeat scroll -63px -42px transparent;"></div>
                    </div>
                </div>
                <div id="tagXLVB" visible="True" class="btTextTo" style="-moz-user-select: none;position:relative;top:-1px;margin-left:-2px;" onmousedown="$(this).css('border', '1px solid #4D90FE');" onmouseup="$(this).css('border', '1px solid rgba(0, 0, 0, 0.1)');" title="Xóa văn bản">
                    <div style="display: inline-block;">
                        <span style="font-family: arial,sans-serif;font-size: 11px;">Xử lý văn bản</span>
                        <div></div>
                    </div>
                </div>
            </div>
            <div id="Di" style="position:absolute;right:-5px;display:inline-block;">
                <div style="position:relative;display: inline-block;">
                    <span style="font-weight:bold;"><asp:Label ID="lbPageTo" runat="server" Text="1-100"></asp:Label></span>
                    <span> trong tổng số </span>
                    <span style="font-weight:bold;"><asp:Label ID="lbPageCount" runat="server" Text="100"></asp:Label></span>
                    <span style="width:2px;">&nbsp;</span>
                </div>
                <div id="btPreviousPage" class="btTextTo" onmousedown="$(this).css('border', '1px solid #4D90FE');" onmouseup="$(this).css('border', '1px solid rgba(0, 0, 0, 0.1)');" style="-moz-user-select: none;min-width:34px;padding:0;margin-right:-4px;border-bottom-right-radius: 0;border-top-right-radius: 0" title="Trang trước">
                    <div style="display: inline-block;">
                        <span style="width:0px;">&nbsp;</span>
                        <div class="btTextTo_icon" style="background: url('/ManagerDispatch/Images/Icon/button_icon.png') no-repeat scroll -21px -21px transparent;margin-left:-2px;"></div>
                    </div>
                </div>
                <div id="btNextPage" class="btTextTo" onmousedown="$(this).css('border', '1px solid #4D90FE');" onmouseup="$(this).css('border', '1px solid rgba(0, 0, 0, 0.1)');" style="-moz-user-select: none;min-width:34px;padding:0;border-bottom-left-radius: 0;border-top-left-radius: 0;border-left-width:0px;" title="Trang kế tiếp">
                    <div style="display: inline-block;">
                        <span style="width:0px;">&nbsp;</span>
                        <div class="btTextTo_icon" style="background: url('/ManagerDispatch/Images/Icon/button_icon.png') no-repeat scroll -42px -21px transparent;margin-left:-2px;"></div>
                    </div>
                </div>
                <div id="btSetting" class="btTextTo" onmousedown="$(this).css('border', '1px solid #4D90FE');" onmouseup="$(this).css('border', '1px solid rgba(0, 0, 0, 0.1)');" style="-moz-user-select: none;min-width:54px;" title="Cài đặt">
                    <div style="display: inline-block;">
                        <div class="btTextTo_icon" style="background: url('/ManagerDispatch/Images/Icon/button_icon.png') no-repeat scroll -0px -21px transparent;margin-left:10px;"></div>
                        <div class="btTextTo_icon" style=" background: url('/ManagerDispatch/Images/Icon/arrow_down.png') no-repeat scroll 0 1px transparent;margin-left:0px;top:8px;"></div>
                    </div>
                </div>
            </div>
        </div>
        <div style="border-bottom: 1px solid #E5E5E5;width:100%"></div>
    </div>
    <asp:GridView ID="grTextTo" runat="server" AutoGenerateColumns="False" OnRowDataBound="grTextTo_RowDataBound"
                        GridLines="None" Width="100%" SelectedRowStyle-BackColor="#FFFFCC" ShowHeader="false">
        <%--<RowStyle CssClass="rowTextTo" />--%>
        <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:CheckBox runat="server" ID="chkRowSelect" CssClass="chkSelectTextTo" />
            </ItemTemplate>
            <ItemStyle HorizontalAlign="Center" Width="26px" CssClass="tdRow" />
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <span><asp:Image ID="imgTextLevel" ImageUrl='<%# Eval("TextLevelIcon") %>' ToolTip='<%# Eval("TextLevelDescription") %>' runat="server" /></span>
            </ItemTemplate>
            <ItemStyle Width="26px" CssClass="tdRow icon" />
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <span><asp:Image ID="imgTextState" ImageUrl='<%# Eval("StateTextIcon") %>' ToolTip='<%# Eval("StateTextDescription") %>' runat="server" /></span>
            </ItemTemplate>
            <ItemStyle Width="26px" CssClass="tdRow icon"  />
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <span><asp:Image ID="imgTextSecurity" ImageUrl='<%# Eval("TextSecurityIcon") %>' ToolTip='<%# Eval("TextSecurityDescription") %>' runat="server" /></span>
            </ItemTemplate>
            <ItemStyle Width="26px" CssClass="tdRow icon"  />
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <%# DataBinder.Eval(Container.DataItem, "StaffName")%>
            </ItemTemplate>
            <ItemStyle Width="22ex" CssClass="tdRow tdRowStaffName"  />
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
            <div class="ellipsis" style="max-width:650px;white-space: nowrap;overflow: hidden;" >
                <%# DataBinder.Eval(Container.DataItem, "TextContent")%>
            </div>
            </ItemTemplate>
            <ItemStyle CssClass="tdRow" />
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <span><asp:Image ID="imgTextAttachment" ImageUrl='<%# Eval("TextAttachment") %>' ToolTip='<%# Eval("TextAttachmentDescription") %>' runat="server" /></span>
            </ItemTemplate>
            <ItemStyle Width="30px" CssClass="tdRow icon"  />
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <div style="text-align:right;width:60px;padding-right:20px;"><asp:Label ID="lbDateTo" runat="server" Text='<%# Eval("DateTo") %>'></asp:Label></div>
            </ItemTemplate>
            <ItemStyle Width="90px" CssClass="tdRow" />
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:Button ID="btRow" runat="server" Text="" style="display:none;" OnClick="btRow_Click" CommandArgument='<%# Eval("TextToID") %>' />
            </ItemTemplate>
            <ItemStyle Width="0px" />
        </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <div id="showTextTo" runat="server" visible="false" style="font-family: arial,sans-serif;">
        <div id="tTt" style="font-weight:bold;width:100%;font-size:13px;margin-top:10px;height:auto;">
            <span><asp:Label ID="lbtNo" runat="server" Text=""></asp:Label></span>
            <span style="margin-left:2px;font-style:italic;"><asp:Label ID="lbtDateIssued" runat="server" Text=""></asp:Label></span>
        </div>
        <div id="dTt" style="margin-top:10px;">
            <table class="addField" style="background:#F5F5F5">
             <tr>
                <td align="right" style="width:10%;padding-right:5px;"> Người gửi: </td>
                <td align="left"><span style="font-weight:normal;"><asp:Label ID="lbTextToFrom" runat="server" Text=""></asp:Label></span></td>
             </tr>
             <tr>
                <td align="right" style="width:10%;padding-right:5px;"> Loại văn bản: </td>
                <td align="left"> <span style="font-weight:normal;"><asp:Label ID="lbTypeText" runat="server" Text=""></asp:Label></span></td>
             </tr>
             <tr>
                <td align="right" style="width:10%;padding-right:5px;"> Số văn bản: </td>
                <td align="left"> <span style="font-weight:normal;"><asp:Label ID="lbTextNoCode" runat="server" Text=""></asp:Label></span></td>
             </tr>
             <tr>
                <td align="right" style="width:10%;padding-right:5px;"> Thời gian đến: </td>
                <td align="left"> <span style="font-weight:normal;"><asp:Label ID="lbDateTo" runat="server" Text=""></asp:Label></span></td>
             </tr>
            </table>
        </div>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
</div>
</asp:Content>

