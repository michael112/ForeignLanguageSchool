using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;

public partial class Login : System.Web.UI.Page
{
    internal void DestroySession() {
        Session["userLogin"] = null;
        Session["userPassHash"] = null;
        
        Response.Cookies["user"].Expires = DateTime.Now.AddDays(-1);
    }
    private class LoginRes
    {
        public String Login { get; set; }
        public String PasswordSalt { get; set; }
        public String PasswordHash { get; set; }
    }
    private LoginRes[] GetLoginsFromSQL(String query, SqlConnection connection)
    {
	LoginRes[] results;
	using (var command = new SqlCommand(query, connection))
	{
		connection.Open();
		using (var reader = command.ExecuteReader())
		{
			var list = new List<LoginRes>();
			while (reader.Read())
				list.Add(new LoginRes { Login = reader.GetString(0), PasswordSalt = reader.GetString(1), PasswordHash = reader.GetString(2) });
			results = list.ToArray();
		}
		connection.Close();
	}
	return results;
	}
	
    protected void Page_Load(object sender, EventArgs e)
    {
        LoginForm1.InnerHtml = "<h1 class=\"LoginTitle\">Zaloguj się:</h1><form action=\"#\" method=\"post\"><ul class=\"LoginForm\"><li>Login: <input type=\"text\" id=\"login\" name=\"login\" /></li><li>Hasło: <input type=\"password\" id=\"password\" name=\"password\" /></li></ul><input type=\"hidden\" id=\"send\" name=\"send\" value=\"true\" /><input type=\"submit\" value=\"Wyślij\" /></form>";
        LoginForm2.InnerHtml = LoginForm1.InnerHtml;
    }
    private void setCookie(String login, String passHash)
    {
        HttpCookie cookie = new HttpCookie("user");
        cookie.Values["login"] = login;
        cookie.Values["passHash"] = passHash;
        cookie.Expires = DateTime.Now.AddHours(3);
        Response.Cookies.Add(cookie);
    }
    private void setSession(String login, String passHash)
    {
        Session["userLogin"] = login;
        Session["userPassHash"] = passHash;
    }
    public void setMsAuthTicket( String login, String passHash) {
        FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, login, System.DateTime.Now, DateTime.Now.AddMinutes(60), false, passHash);
        string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
        HttpCookie authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
        Response.Cookies.Add(authCookie);
	}
    internal void setCookieAndSessionInfo(String login, String passHash)
    {
        setCookie(login, passHash);
        setSession(login, passHash);
        setMsAuthTicket(login, passHash);
    }
    public String CheckPass(String login, String password)
    { // Zwraca hash hasła, jeśli prawidłowe lub null, jeśli nieprawidłowe
		LoginRes[] SQLResults = GetLoginsFromSQL("SELECT Login, PasswordSalt, PasswordHash FROM Users WHERE Login = '" + login + "'", SQLConnector.GetSQLConnection());
		// zakładamy, że może być tylko jedna osoba z danym loginem
        if ( ( SQLResults != null ) && (SQLResults.Length == 1) ) {
            String PasswordSalt = SQLResults[0].PasswordSalt;
            String PasswordHash = SQLResults[0].PasswordHash;
            if (HashGen.CreateSHAHash(password, PasswordSalt).Equals(PasswordHash))
            {
                return PasswordHash;
            }
            else return null;
        }
        else return null; // W zasadzie wyłapujemy tu przypadek braku loginu w bazie.
    }
    public HtmlString ReturnLoginForm()
    {
        return new HtmlString("<h1 class=\"LoginTitle\">Zaloguj się:</h1><form action=\"#\" method=\"post\"><ul class=\"LoginForm\"><li>Login: <input type=\"text\" id=\"login\" name=\"login\" /></li><li>Hasło: <input type=\"password\" id=\"password\" name=\"password\" /></li></ul><input type=\"hidden\" id=\"send\" name=\"send\" value=\"true\" /><input type=\"submit\" value=\"Wyślij\" /></form>");
    }
}