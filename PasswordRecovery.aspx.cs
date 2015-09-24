using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class PasswordRecovery : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private class UserItemToRecover {
        private int id;
        internal UserItemToRecover(int id) {
            this.id = id;
        }

        internal int getId() {
            return this.id;
        }
    }

    private UserItemToRecover LoginAndMailValidation(String seekingLogin, String seekingMail)
    {
        DataClassesDataContext d = new DataClassesDataContext();
        var query = from u in d.Users where ((u.Login == seekingLogin) && (u.E_mail == seekingMail)) select u;
        bool notEmpty = query.Any();
        UserItemToRecover result;
        if( notEmpty ) {
            result = new UserItemToRecover(query.First().UserID);
        }
        else {
            result = null;
        }
        d.Dispose();
        return result;
    }

    private String SendPasswordToDatabase(String PasswordSalt, String PasswordHash, UserItemToRecover ValidationItem)
    {
        return "UPDATE Users SET PasswordSalt = '" + PasswordSalt + "', PasswordHash='" + PasswordHash + "' WHERE UserID = " + ValidationItem.getId() + ";";
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        UserItemToRecover ValidationItem = LoginAndMailValidation(TextBox1.Text, TextBox2.Text);
        if( ValidationItem != null ) {
            String newPassword = StringGen.GenString();
            PasswordSaltAndHash sah = new PasswordSaltAndHash(newPassword);
            String query = SendPasswordToDatabase(sah.getPasswordSalt(), sah.getPasswordHash(), ValidationItem);
            try
            {
                SQLConnector.ProcessQuery(query);
                Panel1.Visible = false;
                Panel2.Visible = true;
                Label1.Text = "<p class=\"center\">Hasło zostało zmienione pomyślnie. Nowe hasło brzmi: " + newPassword + "</p>";
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                Panel1.Visible = false;
                Panel2.Visible = true;
                Label1.Text = "<p class=\"center\">Baza wygenerowała następującego errora: " + ex.Message + "</p>";
            }
        }
    }
}