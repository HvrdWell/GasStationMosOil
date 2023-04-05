using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using GasolineMosOil.Data;
using GasStationMosOil;
using Microsoft.VisualBasic.FileIO;
using MySqlConnector;

namespace GasolineMosOil.Domain;

public static class Deserialize
{
    public static readonly string CsvFileDefault =
        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Assets\";

    public static DataTable GetDataTableFromDB()
    {
        DB db = new DB();

        DataTable table = new DataTable();

        MySqlDataAdapter adapter = new MySqlDataAdapter();
        MySqlCommand command = new MySqlCommand("SELECT `storage`.id as id, typefuel.nameTypeFuel as name, `storage`.count as count, typefuel.price as price FROM `storage` INNER JOIN typefuel ON storage.idTypeFuel = typefuel.idTypeFuel WHERE `storage`.idTypeFuel = typefuel.idTypeFuel", db.getConnection());

        adapter.SelectCommand = command;
        adapter.Fill(table);
       
        return table;


    }

 

    public static UsersData.Users DeserializeUsersData()
    {
        var fileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Assets\Users.xml";
        var serializer = new XmlSerializer(typeof(UsersData.Users));

        using var fs = new FileStream(fileName, FileMode.Open);
        var users = (UsersData.Users)serializer.Deserialize(fs)!;

        return users;
    }

    public static ConfigurationData DeserializeConfiguration()
    {
        var fileName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\Assets\Configuration.xml";
        var xmlSerializer = new XmlSerializer(typeof(ConfigurationData));

        using (var stream = new StreamReader(fileName))
        {
            return (ConfigurationData)xmlSerializer.Deserialize(stream)! ?? throw new InvalidOperationException();
        }
    }
}