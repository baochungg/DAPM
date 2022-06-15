<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="NewWork.aspx.cs" Inherits="NewWork" Title="lịch làm việc" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentHead" runat="Server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="content_tabView_Doc">
        <div class="title">
            Danh sách lịch công tác
        </div>
        <ul class="ul_tab">
            <li style="padding-right: 10px;">
                <asp:DropDownList runat="server" ID="drCalendar" Width="150px">
                    <asp:ListItem Text="Lịch ngày"></asp:ListItem>
                    <asp:ListItem Text="Lịch tuần"></asp:ListItem>
                    <asp:ListItem Text="Lịch tháng"></asp:ListItem>
                </asp:DropDownList>
            </li>
            <li><a href="javascript:void(0)" class="active" id="a_person">Lịch cá nhân</a> </li>
            <!--<li><a href="javascript:void(0)" class="nomarl" id="a_depart">Lịch phòng</a> </li>
            <li><a href="javascript:void(0)" class="nomarl" id="a_company">Lịch cơ quan</a>
            </li>-->
        </ul>
        <div class="contentgrid">
            <asp:GridView runat="server" ID="grCalendar" CellSpacing="0" Width="100%" GridLines="None"
                AutoGenerateColumns="false" CssClass="gridvew">
                <HeaderStyle cssclass="header_row1"/>
                <Columns>
                    <asp:TemplateField HeaderText="STT">
                        <ItemTemplate>
                            <span style="padding: 5px 0; display: block; margin-right: 5px;">
                                <%# DataBinder.Eval(Container.DataItem,"STT") %></span>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="20px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tên công việc">
                        <ItemTemplate>
                            <span style="padding: 5px 0; display: block; margin-right: 5px;">
                                <%# DataBinder.Eval(Container.DataItem, "WorkName")%></span>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="20px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ngày bắt đầu">
                        <ItemTemplate>
                            <span style="padding: 5px 0; display: block; margin-right: 5px;">
                                <%# DataBinder.Eval(Container.DataItem, "StartDate")%></span>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="20px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ngày kết thúc">
                        <ItemTemplate>
                            <span style="padding: 5px 0; display: block; margin-right: 5px;">
                                <%# DataBinder.Eval(Container.DataItem, "StartEnd")%></span>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="20px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tùy chọn">
                        <ItemTemplate>
                            <asp:ImageButton runat="server" ID="imbSua_calendar" AlternateText="chọn" ToolTip="chọn để sửa"
                                ImageUrl="/ManagerDispatch/Images/Icon/edit.png" BorderWidth="0" CommandName='<%# Eval("STT") %>'
                                CommandArgument='<%# Eval("WorkID") %>'/>
                            <asp:ImageButton ID="imgBtDelete_calendar" runat="server" AlternateText="Xóa" ImageUrl="/ManagerDispatch/Images/Icon/delete.png"
                                OnClientClick="if(confirm('Bạn thật sự muốn xóa lịch được chọn?')){return confirm('bạn có chắc muốn xóa không?')};return true;"
                                CommandArgument='<%# Eval("WorkID") %>' BorderWidth="0" ToolTip="Xóa lịch" />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="20px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <ul class="ul_note">
                <li>
                    <img alt="" style="border-width: 0px; width: 16px; height: 16px;" src="Images/continue_flag.png">
                    Công việc chưa hoàn thành </li>
                <li>
                    <img alt="" style="border-width: 0px; width: 16px; height: 16px;" src="Images/new_flag.png">
                    Công việc mới </li>
                <li>
                    <img alt="" style="border-width: 0px; width: 16px; height: 16px;" src="Images/done_flag.png">
                    Công việc đã hoàn thành </li>
            </ul>
        </div>
    </div>
</asp:Content>
