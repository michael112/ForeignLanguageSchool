using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class InsertCourse : System.Web.UI.Page
{
    internal bool addedInformation;
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private class Jezyki
    {
        internal int JezykID { get; set; }
        internal String Nazwa { get; set; }
    }
    private Jezyki[] GetLanguagesFromSQL(String query, SqlConnection connection)
    {
        Jezyki[] results;
        using (var command = new SqlCommand(query, connection))
        {
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                var list = new List<Jezyki>();
                while (reader.Read())
                    list.Add(new Jezyki { JezykID = reader.GetInt32(0), Nazwa = reader.GetString(1) });
                results = list.ToArray();
            }
            connection.Close();
        }
        return results;
    }

    internal String ListOfLanguages()
    {
        Jezyki[] languages = GetLanguagesFromSQL("SELECT * FROM Jezyki;", SQLConnector.GetSQLConnection());
        String HTMLTable = "<table>";
        int i = 0;
        while (i < languages.Length)
        {
            HTMLTable += "<tr><td>" + languages[i].JezykID + "</td><td>" + languages[i].Nazwa + "</td><td><a href=\"?content=EditLanguage&ID=" + languages[i].JezykID + "\">edycja</a></td><td><a href=\"?content=DeleteLanguage&ID=" + languages[i].JezykID + "\">usuń</a></td></tr>";
            i++;
        }
        return HTMLTable + "</table>";
    }

    internal String EditLanguage(int ID)
    {
        String result = "<form id=\"editLanguage\" name=\"editLanguage\" action=\"#\" method=\"post\"><ul>";
        DataClassesDataContext d = new DataClassesDataContext();
        var query = from j in d.Jezyki where j.JezykID == ID select j;
        result += "<li class=\"PAMenuItems\">Nazwa języka: <input name=\"nameOfLanguage\" type=\"text\" value=\"" + query.First().Nazwa + "\" /></li>";
        result += "<input name=\"id\" type=\"hidden\" value=\"" + query.First().JezykID + "\" />";
        result += "<input type=\"submit\" value=\"Zapisz\" onClick=\"document.getElementById(\"editLanguage\").submit();\" />";
        result += "</ul></form>";
        d.Dispose();
        return result;
    }

    internal String saveLanguageEdition(HttpRequest f)
    {
        int ID = Int32.Parse(f.Form["id"]);
        DataClassesDataContext d = new DataClassesDataContext();
        var query = from j in d.Jezyki where j.JezykID == ID select j;
        var jz = query.First();
        
        jz.Nazwa = f.Form["nameOfLanguage"];
        d.SubmitChanges();
        d.Dispose();

        String result = "Zapisano zmiany.";
        result += "<p><a href=\"?content=ManageLanguages\">Powrót</a></p>";
        return result;
    }

    protected String DeleteLanguage(int ID)
    {
        String query = "DELETE FROM Jezyki WHERE JezykID = " + ID + ";";
        String result = "";
        try
        {
            result += ProcessQuery(query, "Język usunięty pomyślnie!");
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
            result += "Baza wygenerowała errora: " + ex.Message;
        }
        finally
        {
            result += "<p><a href=\"?content=ManageLanguages\">Powrót</a></p>";
        }
        return result;
    }

    protected void InsertLanguage(object sender, EventArgs e)
    {
        addedInformation = true;
        String query = "INSERT INTO Jezyki (Nazwa) VALUES ('" + TextBox1.Text + "');";
        try
        {
            Label2.Text = ProcessQuery(query, "Język dodany pomyślnie!");
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
            Label2.Text = "Baza wygenerowała errora: " + ex.Message;
        }
    }

    internal String ProcessQuery(String query, String SuccessMessage)
    {
        try
        {
            SQLConnector.ProcessQuery(query);
            return SuccessMessage;
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
            throw;
        }
    }

    internal String ManageCourses(bool CzyAdmin)
    {
        String query = "SELECT k.KursID AS [IDkursu], k.Typ AS [TypKursu], j.Nazwa AS [Jezyk], p.Nazwa AS [Poziom], u.Imie AS [ImieProwadzacego], u.Nazwisko AS [NazwiskoProwadzacego], u.\"E-mail\" AS [E-mail prowadzacego] FROM Kursy k JOIN Users u ON (u.UserID = k.IDprowadzacego) JOIN Jezyki j ON (j.JezykID = k.Jezyk) JOIN Poziomy p ON (p.PoziomID = k.Poziom);";
        Courses[] cours = GetCoursesFromSQL(query, SQLConnector.GetSQLConnection());
        String result = "<h2>Lista kursów w bazie:</h2><table>";
        result += "<tr><td>ID kursu</td><td>Typ kursu</td><td>Język</td><td>Poziom</td><td>Imię Prowadzącego</td><td>Nazwisko Prowadzącego</td><td>E-mail Prowadzącego</td></tr>";
        int i = 0;
        while (i < cours.Length)
        {
            result += "<tr><td>" + cours[i].IDkursu + "</td><td>" + cours[i].TypKursu + "</td><td>" + cours[i].Jezyk + "</td><td>" + cours[i].Poziom + "</td><td>" + cours[i].ImieProwadzacego + "</td><td>" + cours[i].NazwiskoProwadzacego + "</td><td>" + cours[i].EmailProwadzacego + "</td>";
            AdminRightsChecker rights = new AdminRightsChecker(Request.Cookies["user"], Session);
            if (rights.CzySek())
            {
                result += "<td><a href=\"?content=ManageMaterials&ID=" + cours[i].IDkursu + "\">Zarządzaj materiałami do kursu</a></td><td><a href=\"?content=EditCourses&ID=" + cours[i].IDkursu + "\">edycja</a>" + "</td></tr>";
            }
            /*
            if (rights.CzyAdmin())
            {
                result += "<td><a href=\"?content=EditCourses&ID=" + cours[i].IDkursu + "\">edycja</a>" + "</td></tr>";
            }
            */
            else
            {
                result += "</tr>";
            }
            i++;
        }
        result += "</table>";
        // Dodaj nowy kurs:
        if (CzyAdmin)
        {
            result += "<p><a href=\"?content=AddCourse\" class=\"blackHyperlink\">Dodaj nowy kurs</a></p>";
            result += "<p><a href=\"?content=\">Powrót</a></p>";
        }
        return result;
    }

    private class Courses
    {
        internal int IDkursu { get; set; }
        internal int TypKursu { get; set; }
        internal String Jezyk { get; set; }
        internal String Poziom { get; set; }
        internal String ImieProwadzacego { get; set; }
        internal String NazwiskoProwadzacego { get; set; }
        internal String EmailProwadzacego { get; set; }
    }
    private Courses[] GetCoursesFromSQL(String query, SqlConnection connection)
    {
        Courses[] results;
        using (var command = new SqlCommand(query, connection))
        {
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                var list = new List<Courses>();
                while (reader.Read())
                    list.Add(new Courses { IDkursu = reader.GetInt32(0), TypKursu = reader.GetInt32(1), Jezyk = reader.GetString(2), Poziom = reader.GetString(3), ImieProwadzacego = reader.GetString(4), NazwiskoProwadzacego = reader.GetString(5), EmailProwadzacego = reader.GetString(6) });
                results = list.ToArray();
            }
            connection.Close();
        }
        return results;
    }


    internal String ShowUsers()
    {
        AdminRightsChecker rights = new AdminRightsChecker(Request.Cookies["user"], Session);
        String result = "<table><tr><td>Login</td><td>E-mail</td><td>Imie</td><td>Nazwisko</td><td>Numer kierunkowy (państwa)</td><td>Numer kierunkowy (województwo)</td><td>Numer telefonu</td><td>Ulica</td><td>Numer ulicy</td><td>Numer domu</td><td>Kod pocztowy</td><td>Miasto</td><td></td><td></td></tr>";
        DataClassesDataContext d = new DataClassesDataContext();
        List<Users> l = d.Users.ToList();
        foreach (var k in l)
        {
            result += "<tr><td>" + k.Login + "</td><td>" + k.E_mail + "</td><td>" + k.Imie + "</td><td>" + k.Nazwisko + "</td><td>" + k.PhoneCountryCode + "</td><td>" + k.PhoneAreaCode + "</td><td>" + k.PhoneNumber + "</td><td>" + k.Street + "</td><td>" + k.StreetNumber + "</td><td>" + k.FlatNumber + "</td><td>" + k.PostCode + "</td><td>" + k.City + "</td>";
            if (rights.CzyAdmin())
            {
                result += "<td><a href=\"?content=EditUsers&ID=" + k.UserID + "\">edycja</a></td><td><a href=\"?content=DeleteUsers&ID=" + k.UserID + "\">usuń</a></td></tr>";
            }
            else
            {
                result += "<td></td><td></td></tr>";
            }
        }
        result += "</table>";
        result += "<p><a href=\"?content=\">Powrót</a></p>";
        d.Dispose();
        return result;
    }

    internal String editUser(int ID)
    {
        AdminRightsChecker userRights = new AdminRightsChecker(ID);
        String result = "<form id=\"editUser\" name=\"editUser\" action=\"#\" method=\"post\"><ul>";
        DataClassesDataContext d = new DataClassesDataContext();
        var query = from u in d.Users where u.UserID == ID select u;
        result += "<li class=\"PAMenuItems\">Login: <input name=\"login\" type=\"text\" value=\"" + query.First().Login + "\" /></li>";
        result += "<li class=\"PAMenuItems\">Imię: <input name=\"imie\" type=\"text\" value=\"" + query.First().Imie + "\" /></li>";
        result += "<li class=\"PAMenuItems\">Nazwisko: <input name=\"nazwisko\" type=\"text\" value=\"" + query.First().Nazwisko + "\" /></li>";
        result += "<li class=\"PAMenuItems\">Numer telefonu: +<input name=\"numerPanstwa\" type=\"text\" value=\"" + query.First().PhoneCountryCode + "\" />(<input name=\"numerWojewodztwa\" type=\"text\" value=\"" + query.First().PhoneAreaCode + "\" />)<input name=\"numerTelefonu\" type=\"text\" value=\"" + query.First().PhoneNumber + "\" /></li>";
        result += "<li class=\"PAMenuItems\">Adres:</li><li class=\"PAMenuItems\">Ulica: <input name=\"ulica\" type=\"text\" value=\"" + query.First().Street + "\" /><input name=\"numerDomu\" type=\"text\" value=\"" + query.First().StreetNumber + "\" />, mieszkania <input name=\"numerMieszkania\" type=\"text\" value=\"" + query.First().FlatNumber + "\" /></li>";
        result += "<li class=\"PAMenuItems\">Kod pocztowy: <input name=\"kodPocztowy\" type=\"text\" value=\"" + query.First().PostCode + "\" /> Miasto: <input name=\"miasto\" type=\"text\" value=\"" + query.First().City + "\" /></li>";
            result += "<li class=\"PAMenuItems\">Uprawnienia użytkownika: <select name=\"rights\"><option value=\"1\"";
            if (userRights.CzyAdmin()) result += " selected=\"selected\"";
            result += ">Administrator</option><option value=\"2\"";
            if (!(userRights.CzyAdmin()) && (userRights.CzySek())) result += " selected=\"selected\"";
            result += ">Sekretariat</option><option value=\"3\"";
            if (!(userRights.CzyAdmin()) && !(userRights.CzySek())) result += " selected=\"selected\"";
            result += ">Użytkownik</option></select></li>";
        result += "<input name=\"id\" type=\"hidden\" value=\"" + query.First().UserID + "\" />";
        result += "<input type=\"submit\" value=\"Zapisz\" onClick=\"document.getElementById(\"editUser\").submit();\" />";
        result += "</ul></form>";
        result += "<p><a href=\"?content=ShowUsers\">Powrót do listy użytkowników</a><br /><a href=\"?content=\">Powrót do panelu administracyjnego</a></p>";
        d.Dispose();
        return result;
    }

    internal String saveUserEdition(HttpRequest f)
    {
        int ID = Int32.Parse(f.Form["id"]);
        DataClassesDataContext d = new DataClassesDataContext();
        var query = from u in d.Users where u.UserID == ID select u;
        Users us = query.First();
        us.Login = f.Form["login"];
        us.Imie = f.Form["imie"];
        us.Nazwisko = f.Form["nazwisko"];
        us.PhoneCountryCode = f.Form["numerPanstwa"];
        us.PhoneAreaCode = f.Form["numerWojewodztwa"];
        us.PhoneNumber = f.Form["numerTelefonu"];
        us.Street = f.Form["ulica"];
        if( f.Form["rights"] != "" ) us.CzyAdmin = int.Parse(f.Form["rights"]);
        if( f.Form["numerDomu"] != "" ) us.StreetNumber = int.Parse(f.Form["numerDomu"]);
        if( f.Form["numerMieszkania"] != "" ) us.FlatNumber = int.Parse(f.Form["numerMieszkania"]);
        us.PostCode = f.Form["kodPocztowy"];
        us.City = f.Form["miasto"];
        d.SubmitChanges();
        d.Dispose();
        
        String result = "Zapisano zmiany." + editUser(ID);
        return result;
    }

    internal String editCourse(int ID)
    {
        String result = "<form id=\"editCourse\" name=\"editCourse\" action=\"#\" method=\"post\">";
        DataClassesDataContext d = new DataClassesDataContext();

        var query = from k in d.Kursy
                    join u in d.Users on k.IDprowadzacego equals u.UserID
                    join j in d.Jezyki on k.Jezyk equals j.JezykID
                    join p in d.Poziomy on k.Poziom equals p.PoziomID
                    where k.KursID == ID
                    select new {
                        IDkursu = k.KursID,
                        TypKursu = k.Typ,
                        Jezyk = j.Nazwa,
                        Poziom = p.Nazwa,
                        ImieProwadzacego = u.Imie,
                        NazwiskoProwadzacego = u.Nazwisko,
                        EMailProwadzacego = u.E_mail,

                        IDprowadzacego = k.IDprowadzacego,
                        IDjezyka = k.Jezyk,
                        IDpoziomu = k.Poziom
                    };

        result += "<ul>";

        // JĘZYK

        result += "<li class=\"PAMenuItems\">Język kursu: <select name=\"language\">";
        var languageQuery = from j in d.Jezyki select j;
        foreach (var r in languageQuery)
        {
            if (r.JezykID == query.First().IDjezyka)
            {
                result += "<option value=\"" + r.JezykID + "\" selected=\"selected\">" + r.Nazwa + "</option>";
            }
            else
            {
                result += "<option value=\"" + r.JezykID + "\">" + r.Nazwa + "</option>";
            }
        }
        result += "</select></li>";

        // POZIOM

        result += "<li class=\"PAMenuItems\">Poziom kursu: <select name=\"level\">";
        var levelQuery = from p in d.Poziomy select p;
        foreach (var r in levelQuery)
        {
            if (r.PoziomID == query.First().IDpoziomu)
            {
                result += "<option value=\"" + r.PoziomID + "\" selected=\"selected\">" + r.Nazwa + "</option>";
            }
            else
            {
                result += "<option value=\"" + r.PoziomID + "\">" + r.Nazwa + "</option>";
            }
        }
        result += "</select></li>";

        // PROWADZĄCY

        result += "<li class=\"PAMenuItems\">Prowadzący: <select name=\"teacher\">";
        var teacherQuery = from u in d.Users select u;
        foreach (var r in teacherQuery)
        {
            if (r.UserID == query.First().IDprowadzacego)
            {
                result += "<option value=\"" + r.UserID + "\" selected=\"selected\">" + r.Imie + " " + r.Nazwisko + "</option>";
            }
            else
            {
                result += "<option value=\"" + r.UserID + "\">" + r.Imie + " " + r.Nazwisko + "</option>";
            }
        }
        result += "</select></li>";

        // POZOZSTAŁE WARTOŚCI

        result += "<input name=\"idKursu\" type=\"hidden\" value=\"" + query.First().IDkursu + "\" />";
        result += "<li class=\"PAMenuItems\">Typ kursu: <input name=\"type\" type=\"text\" value=\"" + query.First().TypKursu + "\" /></li>";

        result += "<input type=\"submit\" value=\"Zapisz\" onClick=\"document.getElementById(\"editCourse\").submit();\" />";
        result += "</ul></form>";
        d.Dispose();

        return result;
    }

    internal String saveCourseEdition(HttpRequest f)
    {
        String courseID = f.Form["idKursu"];
        String courseType = f.Form["type"];
        String languageID = f.Form["language"];
        String levelID = f.Form["level"];
        String teacherID = f.Form["teacher"];

        String courseQuery = "UPDATE Kursy SET Jezyk=" + languageID + ",Poziom=" + levelID + ",Typ=" + courseType + ",IDprowadzacego=" + teacherID + " WHERE KursID = " + courseID + ";";

        String result = ProcessQuery(courseQuery, "Zapisano zmiany.") + "<br />" + editCourse(int.Parse(courseID));

        return result;
    }
    internal String insertCourse()
    {
        String result = "<form id=\"insertCourse\" name=\"insertCourse\" action=\"#\" method=\"post\">";
        DataClassesDataContext d = new DataClassesDataContext();
        
        result += "<ul>";

        // JĘZYK

        result += "<li class=\"PAMenuItems\">Język kursu: <select name=\"language\">";
        var languageQuery = from j in d.Jezyki select j;
        foreach (var r in languageQuery)
        {
            result += "<option value=\"" + r.JezykID + "\">" + r.Nazwa + "</option>";
        }
        result += "</select></li>";

        // POZIOM

        result += "<li class=\"PAMenuItems\">Poziom kursu: <select name=\"level\">";
        var levelQuery = from p in d.Poziomy select p;
        foreach (var r in levelQuery)
        {
            result += "<option value=\"" + r.PoziomID + "\">" + r.Nazwa + "</option>";
        }
        result += "</select></li>";

        // PROWADZĄCY

        result += "<li class=\"PAMenuItems\">Prowadzący: <select name=\"teacher\">";
        var teacherQuery = from u in d.Users select u;
        foreach (var r in teacherQuery)
        {
            result += "<option value=\"" + r.UserID + "\">" + r.Imie + " " + r.Nazwisko + "</option>";
        }
        result += "</select></li>";

        result += "<input name=\"type\" type=\"hidden\" value=\"1\" />";
        result += "<input name=\"inserting\" type=\"hidden\" value=\"true\" />";
        result += "<input type=\"submit\" value=\"Zapisz\" onClick=\"document.getElementById(\"insertCourse\").submit();\" />";
        result += "</ul></form>";
        d.Dispose();

        return result;
    }
    internal String saveCourseInsertion(HttpRequest f)
    {
        String courseType = f.Form["type"];
        String languageID = f.Form["language"];
        String levelID = f.Form["level"];
        String teacherID = f.Form["teacher"];

        String courseQuery = "INSERT INTO Kursy (Jezyk, Poziom, Typ, IDprowadzacego) VALUES (" + languageID + ", " + levelID + ", " + courseType + ", " + teacherID + ");";
        String result = "";

        try
        {
            result += ProcessQuery(courseQuery, "Zapisano zmiany.");
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
            result += "Baza wygenerowała errora: " + ex.Message;
        }

        return result;
    }

    internal String ManageMaterials(int ID)
    {
        String result = "";
        DataClassesDataContext d = new DataClassesDataContext();
        var materialsQuery = from m in d.Materialy
			join k in d.Kursy on m.CourseID equals k.KursID
			join j in d.Jezyki on k.Jezyk equals j.JezykID
			join p in d.Poziomy on k.Poziom equals p.PoziomID
			join u in d.Users on k.IDprowadzacego equals u.UserID
			where m.CourseID == ID
			select new {
				IDmaterialu = m.ID,
				Kurs = "j. " + j.Nazwa + " poziom " + p.Nazwa + '(' + u.Imie + ' ' + u.Nazwisko + ')',
                Title = m.Title,
                Type = m.Type,
                FilePath = m.FilePath
			};

        result += "<table>";
        result += "<tr><td>Opis kursu</td><td>Tytuł materiału</td><td>Odnośnik</td>";
        foreach (var m in materialsQuery)
        {
            if( ( m.Type == 0 ) || ( m.Type == 1 ) )
            /*
                m.type:
                0 - plik na serwerze lokalnym lub zewnętrznym;
                1 - plik na serwisie Youtube
                Kolejne numery mogą oznaczać inne serwisy, ale ta funkcjonalność nie jest obecnie zaimplmentowana
            */
            {
                result += "<tr>";
                result += "<td>" + m.Kurs + "</td>";
                result += "<td>" + m.Title + "</td>";
                if (m.Type == 0)
                {
                    result += "<td><a href=\"" + m.FilePath + "\">Pobierz plik</a></td>";
                }
                else if (m.Type == 1)
                {
                    result += "<td><object type=\"application/x-shockwave-flash\" data=\"http://www.youtube.com/v/" + m.FilePath + "\" width=\"560\" height=\"315\"><param name=\"movie\" value=\"http://www.youtube.com/v/" + m.FilePath + "\" /></object></td>";
                }
                result += "</tr>";
            }
        }
        result += "</table>";
        result += "<p><a id=\"addNewMaterial\" name=\"addNewMaterial\" href=\"#\">Dodaj nowy materiał do tego kursu</a></p><div id=\"courseID\" style=\"display:none;\">" + ID + "</div><div id=\"newMaterial\"></div>";

        return result;
    }
    internal String saveMaterialEdition(HttpRequest f)
    {
        String result = "";

        return result;
    }
}