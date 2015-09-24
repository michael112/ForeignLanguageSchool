<%@ Application Language="C#" %>

<%@ Import Namespace="System.Web.Http" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Kod uruchamiany podczas uruchamiania aplikacji
        GlobalConfiguration.Configure(config =>
        {
            config.MapHttpAttributeRoutes();
            config.Formatters.Remove(config.Formatters.XmlFormatter); // ma być JSON, więc wywalamy XML
        });
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Kod uruchamiany podczas zamykania aplikacji

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        // Kod uruchamiany po wystąpieniu nieobsługiwanego błędu

    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Kod uruchamiany w momencie rozpoczynania nowej sesji

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Kod uruchamiany w momencie kończenia sesji. 
        // Uwaga: zdarzenie Session_End jest wywoływane tylko wtedy, gdy tryb stanu sesji
        // jest ustawiony na InProc w pliku Web.config. Jeśli tryb sesji jest ustawiony na StateServer 
        // lub SQLServer, zdarzenie nie jest wywoływane.

    }
       
</script>
