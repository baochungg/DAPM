<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CHƯƠNG TRÌNH QUẢN LÝ CÔNG VĂN - MANAGEMENT DISPATCH</title>
    <style type="text/css">
    *{margin:0px 0px;}
    body{font-family:Tahoma, Arial, Verdana, sans-serif;font-size:11px;background:url('/ManagerDispatch/Images/Icon/bg_body.jpg') repeat-x top left;}
    #header{line-height:80px;font-weight:bold;font-size:x-large; text-align:LEFT;padding-left:100px;color:White;background-color:#3B5999;}
    #main_left{float:left;margin-left:65px;margin-top:45px;}
    #main_right{float:left;margin-top:45px;margin-left:10px;}
    div.login-form{ background:#f4f8fe; border:1px solid #ccd7e6; padding:14px; color:#666;}
    p.login-form-tit{ color:#333; font-size:18px; font-weight:bold; margin-bottom:10px;}
    .login-form .linebt{ border-bottom:1px solid #b9c9e2; padding:5px 0;}
    .login-form .form{ margin:12px 15px 5px 15px; color:#333; line-height:22px;}
    .login-form label{ margin-bottom:6px; font-weight:bold; font-size:12px;}
    .login-form input.text{margin-bottom:7px; width:208px;}
    .login-form .checked{ width:15px; margin:0;height:17px;}
    .link{text-decoration:none;color:#617DA4}
    .link:hover{ text-decoration:underline; }
    a{text-decoration:none; color:#3B5999;}
    a:hover{text-decoration:underline;}
    .skLinkButtom
    {
       color: #FFF; text-decoration: none; 
       font-weight: bold; 
       background-image: url('/ManagerDispatch/Images/Icon/bgbuttomLogin.png');
       background-repeat: repeat-x; 
       width: 100px; display: block;
       padding-top:3px;
       padding-bottom : 7px; text-align: center; border-left: solid 1px #3B6E22; 
       border-right: solid 1px #3B6E22;
    }
    .ul_tab{width:100%;display:block;float:left;margin-bottom:10px;}
    .ul_tab li
    {
	    display:block;
	    float:left;
	    background:url('/ManagerDispatch/Images/Icon/point_left.png') no-repeat center left;
	    padding-left:20px;
	    list-style:none;
	    margin-right:20px;
    }
    .ul_tab li a.normal
    {
	    text-transform:uppercase;
	    font-weight:bold;
	    color:#5378c8;
	    font-size:11px;
	    text-decoration:none;
    }
    .ul_tab li a.active
    {
	    text-transform:uppercase;
	    font-weight:bold;
	    color:#5378c8;
	    font-size:11px;
	    text-decoration:underline;
    }
    .ul_type_doc
    {
	    display:block;
	    float:left;
	    width:100%;
    }
    .ul_type_doc li
    {
	    width:33%;
	    display:block;
	    float:left;
	    line-height:20px;
	    list-style:none;
	    background:url('/ManagerDispatch/Images/Icon/point.png') no-repeat center left;
	    text-indent:15px;
    }
    .ul_type_doc li:hover
    {
	    background-color:#F4F4F4;
    }
    .div_doc_receive,.div_doc_send
    {
	    width:100%;
	    display:block;
	    float:left;
    }
    .header_grid
    {
        height: 20px;
        line-height: 20px;
        text-align: center;
        color: #222;
        font-size: 11px;
        background-color: #f0f1f6;
    }
    .row_grid
    {
        color: #222;
        font-size: 11px;
        text-align: center;
        line-height: 18px;
        border-bottom: solid 1px #f0f1f6;
    }
    .row_grid a
    {
    	color:#222;
    }
    .row_grid a:hover
    {
    	color:Blue;
    	text-decoration:underline;
    }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="wrapper">
        <div id="header">PHẦN MỀM QUẢN LÝ CÔNG VĂN</div>
        <div id="main_left">
            <div class="login-form">
                <p class="login-form-tit">Đăng nhập</p>
                <p class="linebt">Đăng nhập với tài khoản được cấp phát cho nhân viên. </p>
                <div class="form">
                    <p><label>Tài khoản:</label></p>
                    <p><asp:TextBox ID="txtUserName" CssClass="text" onkeyup="txtKeyPress(event);" runat="server"></asp:TextBox></p>
                    <p><label>Mật khẩu:</label></p>
                    <p><asp:TextBox ID="txtPassword" CssClass="text" onkeyup="txtKeyPress(event);" runat="server" TextMode="Password"></asp:TextBox></p>
                    <p><asp:CheckBox ID="chkRememberPwd" runat="server" />
                        <span style="margin-left:5px;font-size:12px;"><a class="link" href="#">Lưu mật khẩu</a></span>
                        <span>|</span>
                        <span style="font-size:12px;"><a class="link" href="#">Quên mật khẩu</a></span>
                    </p>
                    <div style="color:Red;text-align:center;">
                        <asp:Label ID="lbMsg" runat="server" Text=""></asp:Label></div>
                    <div style="width:100px;margin-left:auto;margin-right:auto;height:31px;margin-top:10px;">
                     <asp:LinkButton ID="lbLogin" CssClass="skLinkButtom" runat="server" 
                            onclick="lbLogin_Click" OnClientClick="return CheckInput();">Đăng nhập</asp:LinkButton>
                    </div>
                    <script type="text/javascript">
                    function txtKeyPress(evt)
                    {
                        if(evt.keyCode == 13)
                            document.getElementById('<%= lbLogin.ClientID %>').click();
                    }
                    function CheckInput()
                    {
                        var us = document.getElementById('<%= txtUserName.ClientID %>');
                        if(us.value == '')
                        {
                            alert('Tên tài khoản không thể để trống!');
                            us.focus();
                            return false;
                        }
                        var pwd = document.getElementById('<%= txtPassword.ClientID %>');
                        if(pwd.value == '')
                        {
                            alert('Mật khẩu không thể để trống!');
                            pwd.focus();
                            return false;
                        }
                        return true;
                    }
                    </script>
                </div>
            </div>
        </div>
        <div id="main_right">
        <ul class="ul_tab">
            <li><a href="#" class="active">Văn bản đến</a> </li>
            <li><a href="#" class="normal">Văn bản đi</a> </li>
        </ul>
        <div class="div_doc_receive">
        <div>
            <ul class="ul_type_doc">
            <li>
                <a href="#">
                    Quyết định - nghị định
                </a>
                <em style="font-size:10px; color:#A0A0A0;">
                    <span>(0)</span>
                </em>
                </li>
                <li><a href="#">
                    Hồ sơ sinh viên
                </a>
                <em style="font-size:10px; color:#A0A0A0;">
                    <span id="rpListFieldDoc_ctl02_lbSumDoc">(1)</span>
                </em>
                </li>
                <li><a href="#">
                    Quyết định khen thưởng
                </a>
                <em style="font-size:10px; color:#A0A0A0;">
                    <span>(0)</span>
                </em>
                </li>
                <li><a href="#">
                    Hồ sơ lưu trữ 2011
                </a>
                <em style="font-size:10px; color:#A0A0A0;">
                    <span>(2)</span>
                </em>
                </li>
            </ul>
            </div>
            <div style="width: 99%; display: block;margin-top: 10px;clear:both;margin-left:50px;">
                <div style="height:100%;width:100%;">
                <table cellspacing="0" border="0" style="width:100%;border-collapse:collapse;">
                    <tr class="header_grid">
					    <th scope="col">Số VB</th><th scope="col">Thời gian</th><th scope="col">Nội dung trích yếu</th>
				    </tr>
				    <tr class="row_grid">
					    <td align="left" style="width:100px;">
                            <span style="padding-left:5px;">
                            QDUB2323
                            </span>
                            </td>
                         <td style="width:100px;">01/09/2011</td><td align="left">
                             <a href="#">
                                Nội dung trích yếu văn bản
                            </a>
                        </td>
				    </tr>
				    <tr class="row_grid">
					    <td align="left" style="width:100px;">
                            <span style="padding-left:5px;">
                            HS_000001
                            </span>
                            </td>
                        <td style="width:100px;">01/09/2011</td><td align="left">
                            <a href="#">
                                Nội dung trích yếu văn bản
                            </a>
                         </td>
				    </tr>
				    <tr class="row_grid">
					    <td align="left" style="width:100px;">
                            <span style="padding-left:5px;">
                            5753VBF
                            </span>
                        </td>
                        <td style="width:100px;">31/08/2011</td>
                        <td align="left">
                            <a href="#">
                                Nội dung trích yếu văn bản
                            </a>
                        </td>
				    </tr>
                </table>
                </div>
            </div>
        </div>
        </div>
    </div>
    </form>
</body>
</html>
