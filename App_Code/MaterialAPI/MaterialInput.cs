using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net;

public class MaterialInput : IMaterialInput
{
    public static void Create(Material material)
    {
        String query = "INSERT INTO Materialy (CourseID, Title, FilePath, Type) VALUES (" + material.getCourseID() + ", '" + material.getTitle() + "', '" + material.getFilePath() + "', " + material.getType() + ");";
        try
        {
            SQLConnector.ProcessQuery(query);
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
            throw new HttpResponseException(HttpStatusCode.InternalServerError);
        }
    }
    public static IEnumerable<Material> Read()
    {
        DataClassesDataContext d = new DataClassesDataContext();
        List<Materialy> MaterialCollection = d.Materialy.ToList();
        List<Material> Result = new List<Material>();
        foreach ( var material in MaterialCollection )
        {
            Result.Add( new Material( material.ID, material.CourseID, material.Title, material.Type, material.FilePath ) );
        }
        return Result;
    }
    public static Material Read(int id)
    {
        DataClassesDataContext d = new DataClassesDataContext();
        var MaterialCollection = from m in d.Materialy where m.ID == id select m;
        if ( MaterialCollection.Count() == 1 )
        {
            var Material = MaterialCollection.First();
            Material ReturnObject = new Material(Material.ID, Material.CourseID, Material.Title, Material.Type, Material.FilePath);
            d.Dispose();
            return ReturnObject;
        }
        else
        {
            throw new HttpResponseException(HttpStatusCode.NotFound);
        }
    }

    internal static int GetDatabaseID(int id)
    {
        List<Material> ListOfMaterials = (List<Material>)Read();
        int i = 0;
        while ( i < ListOfMaterials.Count )
        {
            if ( ListOfMaterials[i].getID() == id )
            {
                return i;
            }
            else
            {
                i++;
            }
        }
        throw new IndexOutOfRangeException("Cannot find requested ID!");
    }

    public static void Update(int id, Material material)
    {
        try
        {
            String query = "UPDATE Materialy SET CourseID=" + material.getCourseID() + ", Title='" + material.getTitle() + "', FilePath='" + material.getFilePath() + "', Type=" + material.getType() + " WHERE ID=" + id + ";";
            SQLConnector.ProcessQuery(query);
        }
        catch (Exception ex)
        {
            throw new HttpResponseException(HttpStatusCode.InternalServerError);
        }
    }
    public static void Delete(int id)
    {
        try
        {
            String query = "DELETE FROM Materialy WHERE ID = " + id + ";";
            SQLConnector.ProcessQuery(query);
        }
        catch (Exception ex)
        {
            throw new HttpResponseException(HttpStatusCode.InternalServerError);
        }
    }
}