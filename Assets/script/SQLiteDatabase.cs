using System;
using System.Data.SQLite;
using UnityEngine;

public static class SQLiteDatabase
{
    public static SQLiteConnection connection;
    private static string connection_string;
    private const string database_folder = "Assets";
    private const string database_file = "data.sqlite";

    static SQLiteDatabase()
    {
        connection = null;
        
        const string database_path = database_folder + "/" + database_file;
        const string data_source = "Data Source=" + database_path;

        DotNetEnv.Env.Load();
        string pwd = Environment.GetEnvironmentVariable("DB_PASSWORD");
        string password = "Password=" + pwd;
        
        connection_string = data_source + ";" + password;
    }

    public static void OpenConnection()
    {
        try {
			connection = new SQLiteConnection(connection_string);
            connection.Open();
        }
        catch(Exception e) {
            Debug.Log(e.ToString());
        }
    }

    public static void CloseConnection()
    {
        try {
            connection.Close();
        }
        catch(Exception e) {
            Debug.Log(e.ToString());
        }
    }

    public static void UpdateQuery(string query)
    {
        try {
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.ExecuteNonQuery();
        }
        catch (Exception e) {
            Debug.Log(e.ToString());
        }
    }
}