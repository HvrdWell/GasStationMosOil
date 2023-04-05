using System.IO;

namespace GasolineMosOil.Domain;

public class DataSecure
{
    public void EnsureAssetsExists()
    {
        EnsureConfigurationFileExists();
        EnsureGoodsFileExists();
        EnsureTanksFileExists();
        EnsureUsersFileExists();
    }
    
    public void EnsureConfigurationFileExists()
    {
        string fileName = "Configuration.xml";
        string filePath = "Assets\\" + fileName;
        string defaultContents = @"<?xml version=""1.0"" encoding=""utf-8""?>
            <Configuration>
            <NozzlePostCount>0</NozzlePostCount>
            <PaymentTypes>
            <PaymentType>
            <Name>Пластиковая карта</Name>
            <IsActive>true</IsActive>
            <IsCash>false</IsCash>
            </PaymentType>
            </PaymentTypes>
            </Configuration>";

        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, defaultContents);
        }
    }
    
    public void EnsureGoodsFileExists()
    {
        string fileName = "Goods.csv";
        string filePath = "Assets\\" + fileName;
        string defaultContents = "Id;Name;Count;Price\n1000;null;0;1";

        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, defaultContents);
        }
    }

    public void EnsureTanksFileExists()
    {
        string fileName = "Tanks.csv";
        string filePath = "Assets\\" + fileName;
        string defaultContents = "Id;Name;Reserve;Price\n0;null;0;1";

        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, defaultContents);
        }
    }

    public void EnsureUsersFileExists()
    {
        string fileName = "Users.xml";
        string filePath = "Assets\\" + fileName;
        string defaultContents = @"<Users>
    <User>
        <Login>admin</Login>
        <Password>admin</Password>
        <FullName>admin</FullName>
        <AccessLevel>admin</AccessLevel>
    </User>
</Users>";

        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, defaultContents);
        }
    }


}