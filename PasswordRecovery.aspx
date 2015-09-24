<%@ Page Title="" Language="C#" MasterPageFile="~/Template.master" AutoEventWireup="true" CodeFile="PasswordRecovery.aspx.cs" Inherits="PasswordRecovery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:Panel ID="Panel1" runat="server">
        <h1 class="LoginTitle">Przywracanie zapomnianego hasła</h1>
        <p class="PasswordRecoveryInfo">Aby zmienić hasło, proszę podać e-mail oraz login.</p>
        <div>
            <ul>
                <li class="LoginForm">Login: <asp:TextBox ID="TextBox1" CssClass="ForgottenPasswordItem" runat="server"></asp:TextBox></li>
                <li class="LoginForm">Adres e-mail: <asp:TextBox ID="TextBox2" CssClass="ForgottenPasswordItem" runat="server"></asp:TextBox></li>
                <li class="LoginForm"><asp:Button ID="Button1" runat="server" CssClass="ForgottenPasswordItem" Text="Resetuj hasło" OnClick="Button1_Click" /></li>
            </ul>
        </div>
    </asp:Panel>
    <asp:Panel ID="Panel2" runat="server" Visible="False">
        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
        <p class="center"><a href="Default.aspx">Powrót na stronę główną.</a></p>
    </asp:Panel>
</asp:Content>

