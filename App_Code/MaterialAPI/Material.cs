using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[System.Runtime.Serialization.DataContract]
public class Material
{
    [System.Runtime.Serialization.DataMember]
    private int? ID;
    [System.Runtime.Serialization.DataMember]
    private int? CourseID;
    [System.Runtime.Serialization.DataMember]
    private String Title;
    [System.Runtime.Serialization.DataMember]
    private int? Type;
    [System.Runtime.Serialization.DataMember]
    private String FilePath;

    [Newtonsoft.Json.JsonConstructor]
    public Material( int? ID, int? CourseID, String Title, int? Type, String FilePath)
    {
        this.ID = ID;
        this.CourseID = CourseID;
        this.Title = Title;
        this.Type = Type;
        this.FilePath = FilePath;
    }

    public int? getID()
    {
        return this.ID;
    }

    public int? getCourseID()
    {
        return this.CourseID;
    }
    public void setCourseID( int? CourseID )
    {
        this.CourseID = CourseID;
    }

    public String getTitle()
    {
        return this.Title;
    }
    public void setTilte( String Title )
    {
        this.Title = Title;
    }

    public int? getType()
    {
        return this.Type;
    }
    public void setType( int? Type )
    {
        this.Type = Type;
    }

    public String getFilePath()
    {
        return this.FilePath;
    }
    public void setFilePath(String FilePath)
    {
        this.FilePath = FilePath;
    }
}