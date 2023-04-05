using System;
using System.Globalization;
using System.Windows.Data;
using GasolineMosOil.Data;

namespace GasolineMosOil.Domain;

public static class GoodsModel
{
    public static (string?, double) GetNameAndPriceById(int id)
    {
        // Create an instance of the GoodsData class
        var goodsData = new GoodsData();

        // Find the row in the GoodsDataTable with the corresponding Id
        var row = goodsData.GoodsDataTable.Rows.Find(id);

        // Return the goods data from the row, or null if no matching row was found
        return row != null
            ? (row["name"].ToString(),
                double.Parse(row["price"].ToString() ?? throw new InvalidOperationException(), 
                    NumberStyles.AllowDecimalPoint, 
                    CultureInfo.InvariantCulture))
            : throw new ValueUnavailableException($"Не удалось найти {id}");
    }

    public static double GetPriceById(int id)
    {
        // Create an instance of the GoodsData class
        var goodsData = new GoodsData();
        
        // Find the row in the DataTable with the matching Id
        var row = goodsData.GoodsDataTable.Rows.Find(id);

        // Return the goods goodsData from the row, or null if no matching row was found
        return row != null
            ? (double.Parse(row["price"].ToString() ?? throw new InvalidOperationException(), 
                NumberStyles.AllowDecimalPoint, 
                CultureInfo.InvariantCulture))
            : throw new ValueUnavailableException($"Не удалось найти {id}");
    }
    
    public static int GetRemainingById(int id)
    {
        // Create an instance of the GoodsData class
        var goodsData = new GoodsData();
        
        // Find the row in the DataTable with the matching Id
        var row = goodsData.GoodsDataTable.Rows.Find(id);

        // Return the goods goodsData from the row, or null if no matching row was found
        return row != null
            ? int.Parse(row["count"].ToString() ?? throw new InvalidOperationException())
            : throw new ValueUnavailableException($"Не удалось найти {id}");
    }
}