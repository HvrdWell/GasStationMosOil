using System;
using System.Data;
using GasolineMosOil.Data;
using GasolineMosOil.Model;

namespace GasolineMosOil.ViewModel;

// ViewModel interface
public interface IGoodsSelectorViewModel
{
    DataTable GoodsDataTable { get; }
}

public class ConcreteGoodsSelectorViewModel : IGoodsDataProvider
{
    private readonly GoodsData _goodsData;

    public DataTable GoodsDataTable => _goodsData.GoodsDataTable;

    public ConcreteGoodsSelectorViewModel()
    {
        _goodsData = new GoodsData();
    }
}


public interface IGoodsDataProvider
{
    DataTable GoodsDataTable { get; }
}


public class GoodsSelectorViewModel : IGoodsSelectorViewModel
{
    private readonly IGoodsDataProvider _goodsData;

    public GoodsSelectorViewModel(IGoodsDataProvider goodsData)
    {
        _goodsData = goodsData;
    }

    private DataTable? ShoppingCartDataTable { get; } = new();

    public DataTable GoodsDataTable => _goodsData.GoodsDataTable;


    public void UpdateShoppingCartData(int index, int count)
    {
        var row = ShoppingCartDataTable?.Rows[index];
        var values = row?.ItemArray;
        var id = (int)(values?[0] ?? 0);

        try
        {
            CurrentSession.CurrentReceipt.ChangeCountById(id, count);
        }
        catch (ArgumentException)
        {
           
        }
    }


    public DataTable GetFilteredGoodsDataTable(string filterText)
    {
        if (string.IsNullOrEmpty(filterText)) return GoodsDataTable;

        try
        {
            var dataTableFiltered = GoodsDataTable.AsEnumerable()
                .Where(row => row.Field<string>("name")!.ToLower().Contains(filterText.ToLower()))
                .OrderByDescending(row => row.Field<string>("id"))
                .CopyToDataTable();

            return dataTableFiltered;
        }
        catch
        {
            return new DataTable();
        }
    }
}