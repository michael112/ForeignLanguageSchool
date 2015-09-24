<%@ Page Title="" Language="C#" MasterPageFile="~/Template.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <%
    if (Request.QueryString["page"] == "logout")
    {
        DestroySession();
        Response.Redirect(Request.Url.AbsolutePath);
    }
    else {
        String loggedAs = LoginInfoChecker.checkLoginInfo(Request.Cookies["user"], Session);
        if (loggedAs != null) {
            LogggedInfo.InnerHtml = "Już zalogowano jako: " + loggedAs + ". <br /><a href=\"?page=logout\">Aby wylogować, kliknij</a>.<br /><a href=\"Default.aspx\">Przejdź do strony głównej</a>.";
            %>
            <div id="LogggedInfo" runat="server"></div>
            <%
        }
        else if (Request.Form["send"] == null) {
            %>
            <div id="LoginForm1" runat="server"></div>
            <%
        }
        else {
		    String PassHash = CheckPass(Request.Form["login"], Request.Form["password"]);
            if (PassHash == null) {
                %>
                <h1 class="LoginFailed">Dane podane przy logowaniu są niepoprawne</h1>
                <div id="LoginForm2" runat="server"></div>
                <%        
            }
            else {
                setCookieAndSessionInfo(Request.Form["login"], PassHash);
                Response.Write("Cookie: " + Response.Cookies["user"]["login"] + ", Session: " + Session["userLogin"]);
                if (Request.Form["PrevSite"] != null) {
                    Response.Redirect(Request.Form["PrevSite"]);
                }
                else {
                    Response.Redirect("Default.aspx");
                }
            }
        }
    }
    %>

</asp:Content>

