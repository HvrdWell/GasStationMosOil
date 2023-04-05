using System.Data;
using GasolineMosOil.Domain;
using GasolineMosOil.ViewModel;
using GasStationMosOil;
using MySqlConnector;
namespace GasolineMosOil.Data;

public class GoodsData : IGoodsDataProvider
{
    public DataTable GoodsDataTable { get; }

    public GoodsData()
    {
        DB db = new DB();

        DataTable table = new DataTable();

        MySqlDataAdapter adapter = new MySqlDataAdapter();
        MySqlCommand command = new MySqlCommand("SELECT id, name, count, price FROM product", db.getConnection());
        adapter.SelectCommand = command;
        adapter.Fill(table);


        GoodsDataTable = table;

        GoodsDataTable.AcceptChanges();

        DataColumn?[] primaryKeyColumns = { GoodsDataTable.Columns["id"] };
        GoodsDataTable.PrimaryKey = primaryKeyColumns!;
    }
}