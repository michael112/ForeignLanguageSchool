/*
    BEFORE using routes, install package:
    Install-Package Microsoft.AspNet.WebApi.WebHost
	It's best to use:
	update-Package -reinstall Microsoft.AspNet.WebApi.WebHost
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

[RoutePrefix("api/material")]
public class MaterialController : ApiController
{
    // GET api/<controller>
    [Route("")]
    [HttpGet]
    public IEnumerable<Material> Get()
    {
        return MaterialInput.Read();
    }

    // GET api/<controller>/5
    [Route("{id}")]
    [HttpGet]
    public Material Get(int id)
    {
        try
        {
            Material material = MaterialInput.Read(id);
            return material;
        }
        catch (HttpResponseException ex)
        {
            return null;;
        }
    }

    internal Material JSONToObject(string JSONString)
    {
        // przy użyciu biblioteki Newtonsoft Json .NET
        return Newtonsoft.Json.JsonConvert.DeserializeObject<Material>(JSONString);
    }

    // POST api/<controller>
    [Route("")]
    [HttpPost]
    public async void Post()
    {
        try
        {
            String JSONString = await Request.Content.ReadAsStringAsync();
            Material material = JSONToObject(JSONString);
            MaterialInput.Create(material);
        }
        catch ( HttpResponseException ex ) {}
    }

    // PUT api/<controller>/5
    [Route("{id}")]
    [HttpPut]
    public async void Put(int id)
    {
        try
        {
            String JSONString = await Request.Content.ReadAsStringAsync();
            Material material = JSONToObject(JSONString);
            MaterialInput.Update(id, material);
        }
        catch ( HttpResponseException ex ) {}
    }

    // DELETE api/<controller>/5
    [Route("{id}")]
    [HttpDelete]
    public void Delete(int id)
    {
        try {
            MaterialInput.Delete(id);
        }
        catch ( HttpResponseException ex ) {}
    }


    /*
    [Route("new/{userid}")]
    [HttpGet, HttpPost]
    public HttpResponseMessage GetMaterialAdditionHTML( int UserID )
    {
	    string result = "";
        DataClassesDataContext d = new DataClassesDataContext();
	    var CourseQuery = from k in d.Kursy
	    join j in d.Jezyki on k.Jezyk equals j.JezykID
	    join p in d.Poziomy on k.Poziom equals p.PoziomID
	    join u in d.Users on k.IDprowadzacego equals u.UserID
	    where k.IDprowadzacego == UserID
	    select new {
            IDKursu = k.KursID,
		    Kurs = "j. " + j.Nazwa + " poziom " + p.Nazwa + " (" + u.Imie + ' ' + u.Nazwisko + ')'
	    };

        result += "<ul>";
        result += "<form id=\"addNewMaterial\" name=\"addNewMaterial\" action=\"#\" method=\"post\">";
        result += "<li class=\"PAMenuItems\">";
        if (CourseQuery.Count() > 1)
        {
            result += "Kurs: ";
            result += "<select name=\"course\">";
            foreach (var c in CourseQuery)
            {
                result += "<option value=\"" + c.IDKursu + "\">" + c.Kurs + "</option>";
            }
            result += "</select>";
        }
        else if( CourseQuery.Count() == 1 )
        {
            result += "Jedyny prowadzony przez Ciebie kurs: " + CourseQuery.First().Kurs + " <input type=\"hidden\" value=\"" + CourseQuery.First().IDKursu + "\" />";
        }
        else
        {
            result += "Brak kursów prowadzonych przez użytkownika!";
        }
        d.Dispose();

        result += "</li>";
        result += "<li class=\"PAMenuItems\">Tytuł: <input name=\"title\" type=\"text\" /></li>";
        result += "<li class=\"PAMenuItems\">Forma materiału:</li>";
        result += "<li class=\"PAMenuItems\"><input type=\"radio\" name=\"type\" value=\"0\" />Plik w serwisie lub na serwerze zewnętrznym&nbsp;&nbsp;&nbsp;<input type=\"radio\" name=\"type\" value=\"1\" />Film w serwisie YouTube</li>";
        result += "<li class=\"PAMenuItems\"><input type=\"text\" name=\"filePath\" />Ścieżka do pliku lub wartość atrybutu &quot;v&quot; w przypadku filmu YouTube</li>";
        result += "<input type=\"hidden\" name=\"courseID\" value=\"" + CourseID + "\" />";
        result += "<input type=\"hidden\" name=\"inserting\" value=\"true\" />";
        result += "<input type=\"submit\" value=\"Dodaj\" />";
        result += "</form>";
        result += "</ul>";

	    return GetResponseMessage(result);
    }
    */

    [Route("new/{courseid}")]
    [HttpGet, HttpPost]
    public HttpResponseMessage GetMaterialAdditionHTML(int CourseID)
    {
        string result = "";

        result += "<ul>";
        result += "<form id=\"newMaterialForm\" name=\"newMaterialForm\" action=\"/api/material\" method=\"post\">";
        result += "<li class=\"PAMenuItems\">Tytuł: <input name=\"title\" type=\"text\" /></li>";
        result += "<li class=\"PAMenuItems\">Forma materiału:</li>";
        result += "<li class=\"PAMenuItems\"><input type=\"radio\" name=\"type\" value=\"0\" />Plik w serwisie lub na serwerze zewnętrznym&nbsp;&nbsp;&nbsp;<input type=\"radio\" name=\"type\" value=\"1\" />Film w serwisie YouTube</li>";
        result += "<li class=\"PAMenuItems\"><input type=\"text\" name=\"filePath\" />Ścieżka do pliku lub wartość atrybutu &quot;v&quot; w przypadku filmu YouTube</li>";
        result += "<input type=\"hidden\" name=\"courseID\" value=\"" + CourseID + "\" />";
        //result += "<input type=\"hidden\" name=\"inserting\" value=\"true\" />";
        //result += "<input type=\"button\" id=\"jQuerySubmit\" value=\"Dodaj\" />";
        //result += "<input type=\"submit\" value=\"Dodaj\" />";
        result += "</form>";
        result += "<input type=\"button\" id=\"jQuerySubmit\" value=\"Dodaj\" />";
        result += "</ul>";

        return GetResponseMessage(result);
    }

    public HttpResponseMessage GetResponseMessage(String result)
    {
        var resp = new HttpResponseMessage(HttpStatusCode.OK);
        resp.Content = new StringContent(result, System.Text.Encoding.UTF8, "text/html");
        //resp.Content = new StringContent(result, System.Text.Encoding.UTF8, "text/plain");
        return resp;
    }
}
