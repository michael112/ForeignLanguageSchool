using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;

public class SQLConnector
{
	public SQLConnector()
	{

	}
    public static SqlConnection setConnection(String server, String database)
    {
        return new SqlConnection("Server=" + server + ";Database=" + database + ";Trusted_Connection=True;");
    }
    public static SqlConnection setConnection(String server, String database, String user, String password)
    {
        return new SqlConnection("Server=" + server + ";Database=" + database + ";User Id=" + user + ";Password=" + password + ";");
    }
	
	//public static Object[] PushQuery( SqlConnection connection, String query ) {
    public static SqlConnection GetSQLConnection()
    {
        return setConnection("localhost", "ELrn"); // INFORMACJE O NAZWIE BAZY
    }

    public static void ProcessQuery(String query)
    {
        SqlConnection connection = GetSQLConnection();
        try
        {
            connection.Open();
            SqlCommand command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();
        }
        catch (System.Data.SqlClient.SqlException ex)
        {
            throw;
        }
        finally
        {
            connection.Close();
        }
    }

    public static Object[] PushQuery(String query)
    {
        SqlConnection connection = setConnection("localhost", "ELrn"); // INFORMACJE O NAZWIE BAZY
        connection.Open();
		Object[] table = executeQuery(connection, query);
		connection.Close();
		return table;
	}
		private static Object[] executeQuery(SqlConnection connection, String query)
		{
			SqlCommand sqlc = new SqlCommand(query);
			sqlc.Connection = connection;
			DataTable res = new DataTable();
			res.Load(sqlc.ExecuteReader());
			return res.Rows[0].ItemArray;
		}	
}