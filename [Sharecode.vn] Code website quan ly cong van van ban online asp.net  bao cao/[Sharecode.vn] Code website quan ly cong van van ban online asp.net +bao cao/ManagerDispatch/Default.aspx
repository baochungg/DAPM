<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentHead" Runat="Server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="content_tabView">
    <div class="title_group">
     Các danh mục chính
    </div>
    <div style="display: block; float: left; width: 100%;">
     <!-- TabView Content -->
     <ul class="TabView_Menu">
         <asp:Repeater ID="rpTabView_Menu" runat="server">
         <ItemTemplate>
            <li class="normal">
                <div class="group">
                    <asp:Image ID="img_Icon" class="menu_TabView_Icon" runat="server" ImageUrl='<%# Eval("imgIcon") %>' style="border-width: 0px;" />
                    <asp:LinkButton ID="lbMenuName" class="menu_Tabview_MenuName" runat="server" style="font-weight: bold;" OnClick="lbMenuName_Click" CommandArgument='<%# Eval("menuUrl") %>' ><%# Eval("menuName") %></asp:LinkButton>
                    <asp:LinkButton ID="lbDescription" class="menu_Tabview_Description" runat="server" OnClick="lbMenuName_Click" CommandArgument='<%# Eval("menuUrl") %>' ><%# Eval("Description") %></asp:LinkButton>
                </div>
            </li>
         </ItemTemplate>
         </asp:Repeater>
     </ul>
     <!-- End TabView Content -->
    </div>
</div>
</asp:Content>

