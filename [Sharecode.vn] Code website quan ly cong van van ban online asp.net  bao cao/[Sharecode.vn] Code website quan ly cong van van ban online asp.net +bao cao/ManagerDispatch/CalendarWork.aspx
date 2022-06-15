<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="CalendarWork.aspx.cs" Inherits="CalendarWork" Title="tạo công việc mới" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentHead" runat="Server">
    <!--<script type="text/javascript" language="javascript">
//function displayCalendar()
//{
//var datePicker=document.getElementById('datePicker');
//datePicker.style.display='block';
//}
//function display1()
//{
//var datPicker1=document.getElementById('datePicker1');
//datPicker1.style.display='block';
//datePicker1.style.display='block';
}</script>-->

    <script src="/ManagerDispatch/JS/datetimepicker_css.js" type="text/javascript"></script>

    <style type="text/css">
        .calendar
        {
            display: block;
            width: 65%;
            margin: 0px;
            padding: 30px 20px;
        }
        .title
        {
            width: 99.9%;
            border-bottom: solid 1px #c0c0c0;
            color: #222;
            font-size: 16px;
            margin-bottom: 10px;
            font-family: Verdana;
        }
        .content
        {
            display: block;
            width: 100%;
            float: left;
            font-size: 13px;
            font: tahoma;
        }
        .content .td_left
        {
            text-align: right;
            color: #333;
            width: 25%;
        }
        .contet .td_right
        {
            text-align: right;
        }
        .descripton
        {
            width: 100%;
            height: 25px;
            margin-left: 5px;
        }
        .select
        {
            margin-left: 5px;
        }
        #datePicker
        {
            display: none;
            position: absolute;
            border: 1px #666;
            background-color: white;
        }
        #datePicker1
        {
            display: none;
            position: absolute;
            border: 1px #666;
            background-color: white;
            width: 50px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="calendar">
        <div class="title">
            <span style="color: #666; font-size: 16px; font: tahoma; font-weight: bold; margin-left: 5px">
                Tạo mới lịch công tác</span><br />
        </div>
        <div class="content">
            <br />
            <table style="width: 100%; line-height: 25px" cellspacing="0">
                <tr>
                    <td class="td_left">
                        Loại lịch:
                    </td>
                    <td class="td_right">
                        <asp:DropDownList runat="server" ID="drTypeCalendar" Width="35%" CssClass="select">
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 10px">
                    </td>
                </tr>
                <tr>
                    <td class="td_left">
                        Người dùng lịch:
                    </td>
                    <td class="td_right">
                        <asp:TextBox runat="server" ID="txtUser" CssClass="descripton"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 10px">
                    </td>
                </tr>
                <tr>
                    <td class="td_left">
                        Tên công việc:
                    </td>
                    <td class="td_right">
                        <asp:TextBox runat="server" ID="txtWorkName" CssClass="descripton"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 10px">
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 10px">
                    </td>
                </tr>
                <tr>
                    <td class="td_left">
                        Nội dung công việc:
                    </td>
                    <td style="height: 135px">
                        <asp:TextBox runat="server" ID="txtWorkDetail" Height="100%" Width="100%" CssClass="select"
                            TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 10px">
                    </td>
                </tr>
                <tr>
                    <td class="td_left">
                        Ghi chú :
                    </td>
                    <td class="td_right">
                        <asp:TextBox runat="server" ID="txtNote" Style="width: 100%; height: 80px; margin-left: 5px"
                            TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 10px">
                    </td>
                </tr>
                <tr>
                    <td class="td_left">
                        Thời gian bắt đầu:
                    </td>
                    <td class="td_right">
                        <asp:TextBox runat="server" ID="txtTimeStart" Width="25%" Height="25px" CssClass="select"
                            BorderColor="#666666" BorderWidth="1px"></asp:TextBox><img src="Images/Calendar/cal.gif"
                                onclick="javascript:NewCssCal('<%=txtTimeStart.ClientID %>');" style="margin-left: 10px" />
                        <%--<div id="datePicker">
                                    <!--<asp:Calendar runat="server" ID="calendar1" OnSelectionChanged="calendar1_SelectionChanged"
                                        DayNameFormat="FirstLetter" Height="25px" Width="165px" CaptionAlign="Top" FirstDayOfWeek="Monday">
                                    </asp:Calendar>-->
                                </div>--%>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 6px">
                    </td>
                </tr>
                <tr>
                    <td class="td_left">
                        Thời gian kết thúc:
                    </td>
                    <td class="td_right">
                        <asp:TextBox runat="server" ID="txtTimeEnd" Height="25px" Width="25%" CssClass="select"
                            BorderColor="#666666" BorderWidth="1px"></asp:TextBox>
                        <img src="Images/Calendar/cal.gif" onclick="javascript:NewCssCal('<%=txtTimeEnd.ClientID %>');"
                            style="margin-left: 10px" />
                        <%--<div id="datePicker1">
                            <!--<asp:Calendar ID="calendar2" runat="server" DayNameFormat="FirstLetter" Height="25px"
                                OnSelectionChanged='calendar2_SelectionChanged' Width="165px">
                                <DayStyle Wrap="False" />
                            </asp:Calendar>-->
                            <asp:Label runat="server" ID="lbMess"></asp:Label>
                        </div>--%>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="height: 10px">
                    </td>
                </tr>
                <tr>
                    <td class="td_left">
                    </td>
                    <td class="td_right">
                        <asp:ImageButton runat="server" ID="imbtThemLich" ImageUrl="~/Images/Icon/bt_add_new.png"
                            OnClick="imbtThemLich_Click" />
                            &nbsp
                                <asp:ImageButton runat="server" ID="imbtEditCalendar" 
                            ImageUrl="~/Images/bt_update.png" onclick="imbtEditCalendar_Click" />
                            &nbsp<asp:ImageButton runat="server" ID="imbtcancel"
                                ImageUrl="~/Images/Icon/bt_cancel.png" OnClick="imbtcancel_Click" />
                                
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <center>
                            <asp:Label runat="server" ID="lbMess"></asp:Label></center>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
