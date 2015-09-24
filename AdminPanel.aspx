<%@ Page Title="" Language="C#" MasterPageFile="~/Template.master" AutoEventWireup="true" CodeFile="AdminPanel.aspx.cs" Inherits="InsertCourse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" src="//code.jquery.com/jquery-1.11.3.min.js"></script>
    <script type="text/javascript" src="jquery.json.js"></script>
    <script type="text/javascript" src="CourseAddition.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div id="APContent" runat="server"></div>
<%
if( (addedInformation != null) && (addedInformation) ) {
    %>
    <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
    <%
}
//else {
    String content;
    if (!AdminRightsChecker.SetUserPrivileges(Request.Cookies["user"], Session).CzySek())
    {
        content = "ManageCoursesNoAdmin"; // dla nie-admina dostępne jest wyświetlenie listy kursów bez prawa do kasowania
    }
    //else if( Request.Form[""] )
    else {
        content = "";
        content += Request.QueryString["content"];
    }

    switch (content) {
        case "ManageLanguages": {
            //Wyświetla listę języków z możliwością dodania nowych
            %>
            <h2>Lista dostępnych języków:</h2>
            <%
            String languageTableText = ListOfLanguages();
            if( languageTableText != "<table></table>" ) {
                languageTable.InnerHtml = languageTableText;                
                %>
                <div id="languageTable" runat="server"></div>
                <%
            }    
            %>
            <br /><br />Dodaj nowy język:
            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
            <asp:Button ID="Button1" runat="server" Text="Zapisz" OnClick="InsertLanguage" />
            <p><a href="?content=">Powrót</a></p>
            <%
            languageTable.InnerHtml = languageTableText;
            break;
        }
        case "EditLanguage": {
            if (Request.Form["id"] == null)
            {
                try
                {
                    Response.Write(EditLanguage(Int32.Parse(Request.QueryString["ID"])));
                }
                catch (System.ArgumentNullException ex)
                {
                    Response.Write("Nie podano ID języka");
                }
            }
            else
            {
                Response.Write(saveLanguageEdition(Request));
            }
            break;
        }
        case "DeleteLanguage": {
            try
            {
                Response.Write(DeleteLanguage(Int32.Parse(Request.QueryString["ID"])));
            }
            catch (System.ArgumentNullException ex)
            {
                Response.Write("Nie podano ID języka");
            }
            break;
        }
        case "AddCourse": {
            if ((Request.Form["inserting"] == null) || (Request.Form["inserting"] != "true"))
            {
                Response.Write(insertCourse());
            }
            else
            {
                Response.Write(saveCourseInsertion(Request));
            }
            break;
        }
        case "ManageCoursesAdmin": {
            Response.Write(ManageCourses(true));
            break;
        }
        case "ManageCoursesNoAdmin": {
            Response.Write(ManageCourses(false));
            break;
        }
        case "ShowUsers":
        {
            Response.Write(ShowUsers());
            break;
        }
        case "EditUsers":
        {
            if (Request.Form["id"] == null)
            {
				try {
					Response.Write(editUser(Int32.Parse(Request.QueryString["ID"])));
				}
				catch(System.ArgumentNullException ex) {
					Response.Write("Nie podano ID użytkownika");
				}
            }
            else
            {
                Response.Write(saveUserEdition(Request));
            }
            break;
        }
        case "DeleteUsers":
        {
            
            break;
        }
        case "EditCourses":
        {
            if (Request.Form["idKursu"] == null)
            {
                try
                {
                    Response.Write(editCourse(Int32.Parse(Request.QueryString["ID"])));
                }
                catch (System.ArgumentNullException ex)
                {
                    Response.Write("Nie podano ID kursu");
                }
            }
            else
            {
                Response.Write(saveCourseEdition(Request));
            }
            break;
        }
        case "ManageMaterials":
        {
            try
            {
                Response.Write(ManageMaterials(Int32.Parse(Request.QueryString["ID"])));
            }
            catch (System.ArgumentNullException ex)
            {
                Response.Write("Nie podano ID kursu");
            }
            break;
        }
        default: {
            %>
            <h1 class="PAMenuItems">Panel administracyjny</h1>
            <h2 class="PAMenuItems">Wybierz czynność:</h2>
            <ul class="PAMenuItems">
            <li><a href="?content=ManageLanguages" class="blackHyperlink">Zarządzaj językami</a></li>
            <li><a href="?content=ManageCoursesAdmin" class="blackHyperlink">Zarządzaj kursami</a></li>
			<li><a href="?content=ShowUsers" class="blackHyperlink">Zarządzaj użytkownikami</a></li>
            </ul>
            <%
            break;
        }
    }
%>
</asp:Content>