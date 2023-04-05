using System.Data;
using GasolineMosOil.Domain;
using GasolineMosOil.ViewModel;
using GasStationMosOil;
using MySqlConnector;

namespace GasolineMosOil.Data;

public class NozzlePostData : INozzlePostDataProvider
{
    public DataTable NozzlePostDataTable { get; }

    public NozzlePostData(int number)
    {
        DB db = new DB();
        NozzlePostDataTable = db.selectNozzlePosts(number); ;

        DataColumn?[] primaryKeyColumns = { NozzlePostDataTable.Columns["id"] };
        NozzlePostDataTable.PrimaryKey = primaryKeyColumns!;
    }


    public DataTable NozzlePostCount
    {
        get
        {
            DB db = new DB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT DISTINCT `column`.number FROM `column`", db.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);
            return table;
        }
    }
}