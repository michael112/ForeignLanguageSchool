<%@ Page Title="" Language="C#" MasterPageFile="~/Template.master" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="Register" %>
    


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h1 class="LoginTitle">Rejestracja użytkownika:</h1>    

    <asp:Label ID="Label1" runat="server" Text=""></asp:Label><br />
    <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
&nbsp;Login (*)<br />
    <asp:TextBox ID="TextBox4" runat="server" TextMode="Password"></asp:TextBox>
&nbsp;Hasło(*) - min. 5 znaków<br />
    <asp:TextBox ID="TextBox5" runat="server" TextMode="Password"></asp:TextBox>
&nbsp;Powtórz hasło (*)<br />
    <asp:TextBox ID="TextBox6" runat="server" TextMode="Email" AutoCompleteType="Email"></asp:TextBox>
&nbsp;E-mail (*)<br />
    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
&nbsp;Imię (*)<br />
    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
&nbsp;Nazwisko (*)<br />
    Numer telefonu (<asp:RadioButton ID="RadioButton1" runat="server" GroupName="RadioButtonList1" Checked="True" Text="komórkowy" OnCheckedChanged="RadioButton1_CheckedChanged" />
    <asp:RadioButton ID="RadioButton2" runat="server" GroupName="RadioButtonList1" Text="stacjonarny" OnCheckedChanged="RadioButton2_CheckedChanged" />
&nbsp;)
    <div id="tel_stac" runat="server" visible="True">
    +<asp:TextBox ID="TextBox7" runat="server" Width="25px">48</asp:TextBox>
    (<asp:TextBox ID="TextBox8" runat="server" Width="25px"></asp:TextBox>
    )<asp:TextBox ID="TextBox9" runat="server"></asp:TextBox>
    </div>
    <div id="tel_kom" runat="server" visible="True">
    <asp:TextBox ID="TextBox18" runat="server" Width="25px"></asp:TextBox>
    <asp:TextBox ID="TextBox12" runat="server"></asp:TextBox>
    </div>
    <asp:RadioButtonList ID="RadioButtonList1" runat="server">
    </asp:RadioButtonList>
    Adres:<br />
    ul.
    <asp:TextBox ID="TextBox13" runat="server"></asp:TextBox>
&nbsp;nr domu
    <asp:TextBox ID="TextBox14" runat="server" Width="16px"></asp:TextBox>
&nbsp;nr mieszk.<asp:TextBox ID="TextBox15" runat="server" Width="16px"></asp:TextBox>
    <br />
    kod pocztowy:
    <asp:TextBox ID="TextBox16" runat="server"></asp:TextBox>
    , miasto:
    <asp:TextBox ID="TextBox17" runat="server"></asp:TextBox>
    <br />
    <asp:Button ID="Button1" runat="server" Text="Wyślij" OnClick="Button1_Click" />
    <br />

</asp:Content>

