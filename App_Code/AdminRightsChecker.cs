using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Opis podsumowujący dla AdminRightsChecker
/// </summary>
public class AdminRightsChecker
{
    public AdminRightsChecker(HttpCookie username, System.Web.SessionState.HttpSessionState Session)
    {
        if ((username == null) && (((Session["userLogin"] == null) && (Session["userPassHash"] == null))))
        {
            this.rights = 4; // brak zalogowania - na pewno nie jest adminem :)
        }
        else
        {
            String login = (username != null) ? username.Values["login"] : (((Session["userLogin"] != null) && (Session["userPassHash"] != null)) ? Session["userLogin"].ToString() : null);
            String query = "SELECT CzyAdmin FROM Users WHERE Login = '" + login + "';";

            this.rights = GetRightsFromSQL(query, SQLConnector.GetSQLConnection());
        }
    }

    public int? GetUserID(HttpCookie username, System.Web.SessionState.HttpSessionState Session)
    {
        if ((username == null) && (((Session["userLogin"] == null) && (Session["userPassHash"] == null))))
        {
            return null;
        }
        else
        {
            String login = (username != null) ? username.Values["login"] : (((Session["userLogin"] != null) && (Session["userPassHash"] != null)) ? Session["userLogin"].ToString() : null);
            String query = "SELECT UserID FROM Users WHERE Login = '" + login + "';";

            return GetRightsFromSQL(query, SQLConnector.GetSQLConnection());
        }
    }

    public AdminRightsChecker(int userID)
    {
        String query = "SELECT CzyAdmin FROM Users WHERE UserID = '" + userID + "';";
        this.rights = GetRightsFromSQL(query, SQLConnector.GetSQLConnection());
    }

    private int rights;
    private static int GetRightsFromSQL(String query, System.Data.SqlClient.SqlConnection connection)
    {
        int[] results;
        using (var command = new System.Data.SqlClient.SqlCommand(query, connection))
        {
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                var list = new List<int>();
                while (reader.Read())
                    list.Add(reader.GetInt32(0));
                results = list.ToArray();
            }
            connection.Close();
        }
        return results[0];
    }

    public static AdminRightsChecker SetUserPrivileges(HttpCookie username, System.Web.SessionState.HttpSessionState Session)
    {
        return new AdminRightsChecker(username, Session);
    }

    public bool CzySek()
    {
        return this.rights < 3;
    }
    public bool CzyAdmin()
    {
        return this.rights < 2;
    }
}