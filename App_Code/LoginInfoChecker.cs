using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class LoginInfoChecker
{
    public static String checkLoginInfo(HttpCookie Cookie, System.Web.SessionState.HttpSessionState Session)
    {
        if( (Session["userLogin"] != null) && (Session["userPassHash"] != null) ) {
            return Session["userLogin"].ToString();
        }
        else if (Cookie != null)
        {
            return Cookie.Values["login"];
        }
        else return null;
    }
}