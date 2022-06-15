<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="Staff.aspx.cs" Inherits="Staff" Title="Quản lý nhân viên phòng ban, đơn vị" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentHead" runat="Server">
    <script src="/ManagerDispatch/JS/datetimepicker_css.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<script type="text/javascript">
$(function(){
    var staff_br = $('#' + '<%= txtBirthday.ClientID %>');
    staff_br.focus(function(){
        $('#imgbSelectCal').click();
    });
});
function showInfoDel(staffName, cTxtOut, cTxtTo, cEInbox, cESent, cCWork)
{
    if($('#staffInfoDel').html().trim() == '')
    {
        $('#staffInfoDel').html(
            "<div style=\"width:100%;text-align:center;font-weight:bold;color:blue;margin-top:3px;font-style:italic;\">" + staffName + "</div>" + 
            "<table style=\"width:auto;margin-left:auto;margin-right:auto;font-size:12px;\">" +
            "<tr>" +
                "<th style=\"width:60px;color:#917f7f;\">Sổ VB đến</th>" +
                "<th style=\"width:60px;color:#917f7f;\">Sổ VB di</th>" +
                "<th style=\"width:55px;color:#917f7f;\">Thư đến</th>" +
                "<th style=\"width:55px;color:#917f7f;\">Thư đi</th>" +
                "<th style=\"width:60px;color:#917f7f;\">Lịch làm việc</th>" +
            "</tr>" + 
            "<tr>" + 
                "<td style=\"color:red;text-align:center;font-weight:bold;\">" + cTxtTo + "</td>" +
                "<td style=\"color:red;text-align:center;font-weight:bold;\">" + cTxtOut + "</td>" +
                "<td style=\"color:red;text-align:center;font-weight:bold;\">" + cEInbox + "</td>" +
                "<td style=\"color:red;text-align:center;font-weight:bold;\">" + cESent +"</td>" +
                "<td style=\"color:red;text-align:center;font-weight:bold;\">" + cCWork + "</td>" +
            "</tr>" + 
            "</table>"
            );
            $('#staffInfoDel').slideDown('slow');
    }
    else
    {
        if($('#staffInfoDel').is(":hidden"))
        {
            $('#staffInfoDel').slideDown('slow');
        }
        else $('#staffInfoDel').slideUp('slow');
    }
}
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
function CheckInputStaff()
{
    if(!CheckOwner()) return false;
    var department = $('#' + '<%= drDepartment.ClientID %>');
    if(department.val() == '4e4fbaa3-442c-4cb2-84a8-03f1472da230')
    {
        alert('Bạn chưa chọn phòng ban cho nhân viên này!');
        department.focus();
        return false;
    }
    var staffName = $('#' + '<%= txtStaffName.ClientID %>');
    if(staffName.val() == '')
    {
        alert('Tên nhân viên không thể để trống!');
        staffName.focus();
        return false;
    }
    var staff_UserName = $('#' + '<%= txtUserName.ClientID %>');
    if(staff_UserName.val() == '')
    {
        alert('Tên tài khoản không thể để trống!');
        staff_UserName.focus();
        return false;
    }
    var staff_Pwd = $('#' + '<%= txtPassword.ClientID %>');
    if(staff_Pwd.val() == '' && $('#' + '<%= hdStaffID.ClientID %>').val() == "0")
    {
        alert('Mật khẩu không thể để trống!');
        staff_Pwd.focus();
        return false;
    }
    var staff_Email = $('#' + '<%= txtEmailAdress.ClientID %>');
    if(staff_Email.val() == '')
    {
        alert('Email không thể để trống!');
        staff_Email.focus();
        return false;
    }
    if(!(/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(staff_Email.val())))
    {
        alert('Địa chỉ Email không hợp lệ!');
        staff_Email.focus();
        return false;
    }
    return true;
}
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="content_tabView_Doc">
        <div class="title_group">
            Thêm thông tin nhân viên
        </div>
        <div id="dateTimePicker" style="display: none; position: absolute; top: 30px; left: 0px;">
            <asp:Calendar ID="clBirthday" runat="server" BackColor="White" Width="200px"></asp:Calendar>
        </div>
        <asp:UpdatePanel ID="updatepanel_Staff" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table class="tbUser" cellpadding="5" cellspacing="5" style="width: 100%;">
                    <tr>
                        <td class="tdULeft" style="text-align: right; width: 15%;">
                            Lựa chọn đơn vị(<font style="color: Red;">*</font>) :
                        </td>
                        <td class="tdURight" style="width: 35%;">
                            <asp:DropDownList ID="drDepartment" runat="server" Style="width: 90%; height: 20px;
                                line-height: 20px;" OnSelectedIndexChanged="drDepartment_SelectedIndexChanged"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </td>
                        <td style="width: 10%;">
                            &nbsp;
                        </td>
                        <td style="width: 40%;">
                            <asp:CheckBox ID="chkIsCharge" runat="server"  TabIndex="4" />
                            Là trưởng phòng ban này
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 15%;">
                            Tên đầy đủ(<font style="color: Red;">*</font>) :
                        </td>
                        <td>
                            <asp:TextBox ID="txtStaffName" runat="server" Style="width: 90%; height: 20px; line-height: 20px;"
                                MaxLength="100" TabIndex="1"></asp:TextBox>
                        </td>
                        <td style="text-align: right; width: 10%;">
                            Ngày sinh :
                        </td>
                        <td style="position: relative;">
                            <asp:TextBox ID="txtBirthday" runat="server" Style="width: 70%; height: 20px; line-height: 20px;"  TabIndex="5"
                                MaxLength="10"></asp:TextBox>
                            <img id="imgbSelectCal" src="/ManagerDispatch/Images/Calendar/cal.gif" onclick="javascript:NewCssCal('<%= txtBirthday.ClientID %>');"
                                style="cursor: pointer" alt="Chọn ngày" />
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 15%;">
                            Tên tài khoản(<font style="color: Red;">*</font>) :
                        </td>
                        <td>
                            <asp:TextBox ID="txtUserName" runat="server" AutoPostBack="true" Style="width: 90%;
                                height: 20px; line-height: 20px;" MaxLength="50" TabIndex="1"></asp:TextBox>
                        </td>
                        <td style="text-align: right; width: 10%;">
                            Số điện thoại :
                        </td>
                        <td>
                            <asp:TextBox ID="txtPhoneNumber" runat="server" Style="width: 70%; height: 20px;
                                line-height: 20px;" MaxLength="20" TabIndex="6"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 10%;">
                            Mật khẩu(<font style="color: Red;">*</font>) :
                        </td>
                        <td>
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Style="width: 90%;
                                height: 20px; line-height: 20px;" MaxLength="40" TabIndex="2"></asp:TextBox>
                        </td>
                        <td style="text-align: right; width: 15%;">
                            Địa chỉ :
                        </td>
                        <td>
                            <asp:TextBox ID="txtAddress" runat="server" Style="width: 70%; height: 20px; line-height: 20px;"
                                MaxLength="150" TabIndex="7"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: right; width: 15%;">
                            Địa chỉ Email(<font style="color: Red;">*</font>) :
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmailAdress" runat="server" Style="width: 90%; height: 20px;
                                line-height: 20px;" MaxLength="50"  TabIndex="3"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:CheckBox ID="chkBlockedAcount" runat="server" TabIndex="8" />
                            Khóa tài khoản
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:ImageButton ID="btAddStaff" runat="server" TabIndex="9" AlternateText="Thêm, sửa" ImageUrl="/ManagerDispatch/Images/Icon/bt_add_new.png"
                                BorderWidth="0" ToolTip="Thêm mới, cập nhật!" OnClick="btAddStaff_Click" OnClientClick="return CheckInputStaff();" />
                            <asp:ImageButton ID="btCancelStaff" runat="server" TabIndex="10" AlternateText="Hủy" ImageUrl="/ManagerDispatch/Images/Icon/bt_cancel.png"
                                BorderWidth="0" ToolTip="Bỏ qua nhân viên vừa chọn" OnClick="btCancelStaff_Click" OnClientClick="return CheckOwner();" />
                        </td>
                    </tr>
                </table>
                <div class="title_group">
                    Danh sách nhân viên
                </div>
                <asp:UpdateProgress ID="uprStaff" runat="server" AssociatedUpdatePanelID="updatepanel_Staff">
                    <ProgressTemplate>
                        <div style="text-align: center; color: #8497bf; height: 20px; font-weight: bold;">
                            Loading...</div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
                <div style="text-align: center; min-height: 20px;">
                    <asp:Label ID="lbMsg_Staff" runat="server" Text=""></asp:Label>
                    <div id="staffInfoDel" style="text-align: center; display: block; height: auto;">
                    </div>
                </div>
                <div id="staff_data" style="width: 100%; height: 100%;">
                    <asp:GridView ID="grStaff" runat="server" AutoGenerateColumns="False" OnRowDataBound="grStaff_RowDataBound"
                        GridLines="None" Width="100%" SelectedRowStyle-BackColor="#CCFF99">
                        <SelectedRowStyle BackColor="#FFFFCC" />
                        <HeaderStyle CssClass="header_row_column_right" />
                        <RowStyle CssClass="row_column_right" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox runat="server" ID="chkSelectAll" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkRowSelect" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="30px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TT">
                                <ItemTemplate>
                                    <span style="padding: 5px 0; display: block;">
                                        <%# DataBinder.Eval(Container.DataItem, "STT") %></span>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="30px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tên đầy đủ">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "StaffName")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ngày sinh">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "Birthday")%>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="70px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Địa chỉ">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "Address")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Điện thoại">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "PhoneNumber")%>
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="80px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email">
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container.DataItem, "EmailAddress")%>
                                </ItemTemplate>
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
                            <asp:TemplateField HeaderText="Tùy chọn">
                                <ItemTemplate>
                                    <asp:ImageButton ID="imgBtEdit" runat="server" AlternateText="Chọn" ImageUrl="/ManagerDispatch/Images/Icon/edit.png"
                                        BorderWidth="0" OnClick="imgBtEdit_Click" CommandName='<%# Eval("STT") %>' CommandArgument='<%# Eval("StaffID") %>'
                                        ToolTip="Chọn nhân viên để sửa" />
                                    <asp:ImageButton ID="imgBtDelete" runat="server" AlternateText="Xóa" ImageUrl="/ManagerDispatch/Images/Icon/delete.png"
                                        OnClientClick="return confirm('Bạn thật sự muốn xóa nhân viên này?');" OnClick="imgBtDelete_Click"
                                        CommandArgument='<%# Eval("StaffID") %>' BorderWidth="0" ToolTip="Xóa nhân viên" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="70px" />
                            </asp:TemplateField>
                        </Columns>
                        <AlternatingRowStyle BackColor="#FCFCFC" />
                    </asp:GridView>
                    <asp:HiddenField ID="hdStaffID" runat="server" Value="0" />
                    <div style="margin-top: 7px; margin-left: 6px; font-weight: bold;">
                        Trang:
                        <asp:DropDownList ID="drPage" runat="server" Style="width: 40px;" AutoPostBack="true"
                            OnSelectedIndexChanged="drPage_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    <center style="width: 99%;">
                        <asp:ImageButton ID="imgBtDeleteStaff" ImageUrl="/ManagerDispatch/Images/Icon/xoataikhoan.png"
                            runat="server" AlternateText="Xóa tài khoản" />
                    </center>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
