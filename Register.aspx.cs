using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Register : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        IncludeJS(this, "Register.aspx.js");
    }

    private class SQLResTable
    {
        public String Login { get; set; }
    }
    private static SQLResTable[] GetLoginsFromSQL(String query, SqlConnection connection)
    {
        SQLResTable[] results;
        using (var command = new SqlCommand(query, connection))
        {
            connection.Open();
            using (var reader = command.ExecuteReader())
            {
                var list = new List<SQLResTable>();
                while (reader.Read())
                    list.Add(new SQLResTable { Login = reader.GetString(0) });
                results = list.ToArray();
            }
			connection.Close();
        }
        return results;
    }


	private static void IncludeJS(Page page, string jsfile)
	{
		 System.Web.UI.HtmlControls.HtmlGenericControl child = new System.Web.UI.HtmlControls.HtmlGenericControl("script");
		 child.Attributes.Add("type", "text/javascript");
		 child.Attributes.Add("src", jsfile);
		 page.Header.Controls.Add(child);
	}
	
    protected void RadioButton2_CheckedChanged(object sender, EventArgs e) { } //niepotrzebne
    protected void RadioButton1_CheckedChanged(object sender, EventArgs e) { } //niepotrzebne
    protected void Button1_Click(object sender, EventArgs e)
    {
        if( LoginValidate(TextBox3.Text) && PassValidate(TextBox4.Text, TextBox5.Text) && EmailValidate(TextBox6.Text) && NameAndSurnameValidate(TextBox1.Text, TextBox2.Text) ) {
            PasswordSaltAndHash SaltAndHash = new PasswordSaltAndHash(TextBox4.Text);
            Phone Phone = PhoneGen(TextBox7.Text, TextBox8.Text, TextBox9.Text, TextBox18.Text, TextBox12.Text);
            Address Address = AddressGen(TextBox13.Text, TextBox14.Text, TextBox15.Text, TextBox16.Text, TextBox17.Text);

            // wrzuta do bazy

            String query = GenerateQuery(Phone, Address, SaltAndHash, TextBox1.Text, TextBox2.Text, TextBox3.Text, TextBox6.Text);

			try {
                Label1.Text = ProcessQuery(query, "Konto założone pomyślnie!");
			}
			catch (System.Data.SqlClient.SqlException ex)
			{
			Label1.Text = "Baza wygenerowała errora: " + ex.Message;
			}
        }
        else {
            Label1.Text = "Błąd walidacji danych";
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

    private String GenerateQuery(Phone Phone, Address Address, PasswordSaltAndHash SaltAndHash, String TextBox1, String TextBox2, String TextBox3, String TextBox6)
    {
        String PasswordSalt = SaltAndHash.getPasswordSalt();
        String PasswordHash = SaltAndHash.getPasswordHash();
		
        if (Phone == null && Address == null)
        {
            return "INSERT INTO Users (CzyAdmin, Imie, Nazwisko, Login, \"E-mail\", PasswordSalt, PasswordHash) VALUES (3, '" + TextBox1 + "', '" + TextBox2 + "', '" + TextBox3 + "', '" + TextBox6 + "', '" + PasswordSalt + "', '" + PasswordHash + "');";
        }
		
		else if( ( Phone == null ) && ( Address != null ) ) {
			if (Address.FlatNumber == null)
			{
				return "INSERT INTO Users (CzyAdmin, Imie, Nazwisko, Login, \"E-mail\", PasswordSalt, PasswordHash, Street, StreetNumber, PostCode, City) VALUES (3, '" + TextBox1 + "', '" + TextBox2 + "', '" + TextBox3 + "', '" + TextBox6 + "', '" + PasswordSalt + "', '" + PasswordHash + "', '" + Address.Street + "', " + Address.StreetNumber + ", " + Address.PostCode + "', '" + Address.City + "');";
			}
			else {
				return "INSERT INTO Users (CzyAdmin, Imie, Nazwisko, Login, \"E-mail\", PasswordSalt, PasswordHash, Street, StreetNumber, FlatNumber, PostCode, City) VALUES (3, '" + TextBox1 + "', '" + TextBox2 + "', '" + TextBox3 + "', '" + TextBox6 + "', '" + PasswordSalt + "', '" + PasswordHash + "', '" + Address.Street + "', " + Address.StreetNumber + ", " + Address.FlatNumber + ", '" + Address.PostCode + "', '" + Address.City + "');";
			}
		}
		else if( ( Phone != null ) && (Address == null ) ) {
			if (Phone.AreaCode == null)
			{
				return "INSERT INTO Users (CzyAdmin, Imie, Nazwisko, Login, \"E-mail\", PasswordSalt, PasswordHash, PhoneNumber, PhoneCountryCode) VALUES (3, '" + TextBox1 + "', '" + TextBox2 + "', '" + TextBox3 + "', '" + TextBox6 + "', '" + PasswordSalt + "', '" + PasswordHash + "', '" + Phone.Number + "', '" + Phone.CountryCode + "');";
			}
			else
			{
				return "INSERT INTO Users (CzyAdmin, Imie, Nazwisko, Login, \"E-mail\", PasswordSalt, PasswordHash, PhoneNumber, PhoneCountryCode, PhoneAreaCode) VALUES (3, '" + TextBox1 + "', '" + TextBox2 + "', '" + TextBox3 + "', '" + TextBox6 + "', '" + PasswordSalt + "', '" + PasswordHash + "', '" + Phone.Number + "', '" + Phone.CountryCode + "', '" + Phone.AreaCode + "');";
			}
		}
		else {
			if ( ( Phone.AreaCode == null ) && ( Address.FlatNumber == null ) )
			{
				return "INSERT INTO Users (CzyAdmin, Imie, Nazwisko, Login, \"E-mail\", PasswordSalt, PasswordHash, PhoneNumber, PhoneCountryCode, Street, StreetNumber, PostCode, City) VALUES (3, '" + TextBox1 + "', '" + TextBox2 + "', '" + TextBox3 + "', '" + TextBox6 + "', '" + PasswordSalt + "', '" + PasswordHash + "', '" + Phone.Number + "', '" + Phone.CountryCode + "', '" + Address.Street + "', " + Address.StreetNumber + ", " + Address.PostCode + "', '" + Address.City + "');";
			}
			else if ( Address.FlatNumber == null )
			{
				return "INSERT INTO Users (CzyAdmin, Imie, Nazwisko, Login, \"E-mail\", PasswordSalt, PasswordHash, PhoneNumber, PhoneCountryCode, PhoneAreaCode, Street, StreetNumber, PostCode, City) VALUES (3, '" + TextBox1 + "', '" + TextBox2 + "', '" + TextBox3 + "', '" + TextBox6 + "', '" + PasswordSalt + "', '" + PasswordHash + "', '" + Phone.Number + "', '" + Phone.CountryCode + "', '" + Phone.AreaCode + "', '" + Address.Street + "', " + Address.StreetNumber + ", " + Address.PostCode + "', '" + Address.City + "');";
			}
			else if ( Phone.AreaCode == null )
			{
				return "INSERT INTO Users (CzyAdmin, Imie, Nazwisko, Login, \"E-mail\", PasswordSalt, PasswordHash, PhoneNumber, PhoneCountryCode, Street, StreetNumber, FlatNumber, PostCode, City) VALUES (3, '" + TextBox1 + "', '" + TextBox2 + "', '" + TextBox3 + "', '" + TextBox6 + "', '" + PasswordSalt + "', '" + PasswordHash + "', '" + Phone.Number + "', '" + Phone.CountryCode + "', '" + Address.Street + "', " + Address.StreetNumber + ", " + Address.FlatNumber + ", '" + Address.PostCode + "', '" + Address.City + "');";
			}
			else
			{
				return "INSERT INTO Users (CzyAdmin, Imie, Nazwisko, Login, \"E-mail\", PasswordSalt, PasswordHash, PhoneNumber, PhoneCountryCode, PhoneAreaCode, Street, StreetNumber, FlatNumber, PostCode, City) VALUES (3, '" + TextBox1 + "', '" + TextBox2 + "', '" + TextBox3 + "', '" + TextBox6 + "', '" + PasswordSalt + "', '" + PasswordHash + "', '" + Phone.Number + "', '" + Phone.CountryCode + "', '" + Phone.AreaCode + "', '" + Address.Street + "', " + Address.StreetNumber + ", " + Address.FlatNumber + ", '" + Address.PostCode + "', '" + Address.City + "');";
			}
		}
    }

    private static bool LoginValidate(String login)
    {
        if (login.Length <= 0) // pusty
        {
            return false;
        }
        else
        {
            SQLResTable[] SQLResults = GetLoginsFromSQL("SELECT Login FROM Users WHERE Login = '" + login + "'", SQLConnector.GetSQLConnection());
            if ((SQLResults != null) && (SQLResults.Length > 0))
            {
                return false;  // login zajęty
            }
            else return true;
        }
    }
    private static bool PassValidate(String pass1, String pass2)
    {
        if ((pass1.Length <= 0) || (pass2.Length <= 0)) // pusty
        {
            return false;
        }
        else
        {
            if (pass1.Equals(pass2) == false) // nie zgadzają się
            {
                return false;
            }
            else
            {
                if (pass1.Length < 5) // za krótkie
                {
                    return false;
                }
                else return true;
            }
        }
    }
    private static bool EmailValidate(String email)
    {
        //if ((email.IndexOf("@") == -1) || (email.Count(x => x == '@')))
        if (email.Count(x => x == '@') != 1)
        {
            return false; // brak @ lub kilkukrotnie w adresie
        }
        if (email.IndexOf(".", email.IndexOf('@') + 1) == -1)
        {
            return false; // brak kropki po małpie
        }
        else return true;
    }
    private static bool NameAndSurnameValidate(String name, String surname) {
        if ( ( name.Length <= 0 ) || ( surname.Length <= 0 ) )
        {
            return false;
        }
        else return true;
    }
	
    private class Phone {
        public String CountryCode;
        public String AreaCode;
        public String Number;
        public Phone(String CountryCode, String AreaCode, String Number)
        {
            this.CountryCode = CountryCode;
            this.AreaCode = AreaCode;
            this.Number = Number;
        }
        public Phone(String AreaOrCountryCode, String Number, bool S)
        {
            if (S) // numer stacjonarny
            {
                this.CountryCode = "48"; // Polska
                this.AreaCode = AreaOrCountryCode;
                this.Number = Number;
            }
            else
            {
                this.CountryCode = AreaOrCountryCode;
                this.AreaCode = null;
                this.Number = Number;
            }
        }
        public Phone( String Number ) {
            this.CountryCode = "48"; // Polska
            this.AreaCode = null;
            this.Number = Number;
        }
    }
    private static Phone PhoneGen(String CountryStativeCode, String AreaCode, String StativeNumber, String MobileCountryCode, String MobileNumber) {
        if (MobileNumber.Length > 0) // domyślnie komórkowy
        {
            if (MobileCountryCode.Length == 2 )
            {
                return new Phone(MobileCountryCode, MobileNumber, false);
            }
            else
            {
                return new Phone(MobileNumber);
            }
        }
        else if( StativeNumber.Length > 0 ) {
            if( AreaCode.Length != 2 ) {
                return null; // błąd - numery stacjonarne muszą mieć nr kierunkowy
            }
            if( CountryStativeCode.Length == 2 ) {
                return new Phone(CountryStativeCode, AreaCode, StativeNumber);
            }
            else {
                return new Phone(AreaCode, StativeNumber, true);
            }
        }
        else return null; // brak numeru
    }
	
	private class Address {
	    public String Street;
		public String StreetNumber;
        public String FlatNumber;
        public String PostCode;
        public String City;
		public Address(String Street, String StreetNumber, String FlatNumber, String PostCode, String City) {
			this.Street = (Street.Length > 0) ? Street : null;
			this.StreetNumber = (StreetNumber.Length > 0) ? StreetNumber : null;
			this.FlatNumber = (FlatNumber.Length > 0) ? FlatNumber : null;
			this.PostCode = (PostCode.Length > 0) ? PostCode : null;
			this.City = (City.Length > 0) ? City : null;
		}
	}
	private static Address AddressGen (String Street, String StreetNumber, String FlatNumber, String PostCode, String City) {
        if( ( Street.Length <= 0 ) && ( StreetNumber.Length <= 0 ) && ( FlatNumber.Length <= 0 ) && ( PostCode.Length <= 0 ) && ( City.Length <= 0 ) ) {
            return null;
        }
		else {
            return new Address(Street, StreetNumber, FlatNumber, PostCode, City);
        }
	}
}