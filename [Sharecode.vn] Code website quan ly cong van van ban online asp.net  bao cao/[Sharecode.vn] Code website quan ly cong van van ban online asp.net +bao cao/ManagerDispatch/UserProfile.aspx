<%@ Page Language="C#" AutoEventWireup="true" CodeFile="UserProfile.aspx.cs" Inherits="UserProfile" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Thay đổi thông tin cá nhân! - Management Dispatch</title>
    <link href="StyleSheet/User.css" rel="Stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            font-family: Times New Roman Arial Sans-Serif;
            font-size: 12pt;
            color: #39c;
            text-align: right;
            line-height: 35px;
            margin-bottom: 2px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <center>
        <div class="inform">
            <div class="title_inform">
                THÔNG TIN CÁ NHÂN
            </div>
            <table cellspacing="0" cellpadding="0" width="80%">
                <tr>
                    <td class="style1" align="right">
                        Họ tên :
                    </td>
                    <td style="width:2px;">
                    </td>
                    <td class="right" align="left">
                        <asp:TextBox runat="server" ID="txtName" Width="235px" Height="18px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style1" align="right">
                        Ngày sinh :
                    </td>
                    <td style="width:2px;">
                    </td>
                    <td class="right" align="left">
                        <asp:TextBox runat="server" ID="txtBirthday" Width="150px" Height="18px"></asp:TextBox>
                        <img src="Images/Calendar/cal.gif"
                                onclick="javascript:NewCssCal('<%=txtBirthday.ClientID %>');" style="margin-left: 10px" />
                    </td>
                </tr>
                <tr>
                    <td class="style1" align="right">
                        Địa chỉ :
                    </td>
                    <td style="width: 2px;" >
                    </td>
                    <td class="right" align="left">
                        <asp:TextBox runat="server" ID="txtAddress" Width="235px" Height="18px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style1" align="right">
                        Điện thoại :
                    </td>
                    <td style="width: 2px;">
                    </td>
                    <td class="right" align="left">
                        <asp:TextBox runat="server" ID="txtPhoneNumber" Width="235px" Height="18px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style1" align="right">
                        Email :
                    </td>
                    <td style="width:2px;">
                    </td>
                    <td class="right" align="left">
                        <asp:TextBox runat="server" ID="txtEmail" Width="235px" Height="18px"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <div style="text-align: center; font-family: Times New Roman Arial Sans-Serif; font-size: 12pt;
                margin-top: 10px">
                <asp:Button runat="server" ID="btSaveChange" Text="Lưu lại" Width="95px" />
            </div>
        </div>
    </center>
    </form>
</body>
</html>
