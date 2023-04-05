using System.Collections.Generic;
using System.Data;
using GasolineMosOil.Domain;

namespace GasolineMosOil.Model;

public class ShoppingCartItem
{
    public string? Name { get; set; }
    public int Id { get; set; }
    public int Count { get; set; }
    public double TotalCost { get; set; }

    public static DataTable? Update(Receipt receipt)
    {
        var commodityItem = receipt.CommodityItem;
        var table = new DataTable();
        table.Columns.Add("id", typeof(int));
        table.Columns.Add("Name", typeof(string));
        table.Columns.Add("Count", typeof(int));
        table.Columns.Add("TotalCost", typeof(double));

        // Add each item in the List to the DataTable
        foreach (var item in commodityItem)
        {
            var (goodsName, goodsPrice) = GoodsModel.GetNameAndPriceById(item.id);

            table.Rows.Add(item.id, goodsName, item.Count, item.TotalCost);
        }

        return table;
    }

    public static IEnumerable<ShoppingCartItem> UpdateCart(Receipt receipt)
    {
        var commodityItem = receipt.CommodityItem;
        var shoppingCartItems = new List<ShoppingCartItem>();

        foreach (var item in commodityItem)
        {
            var (goodsName, goodsPrice) = GoodsModel.GetNameAndPriceById(item.id);

            var shoppingCartItem = new ShoppingCartItem
            {
                Name = goodsName,
                Id = item.id,
                Count = item.Count,
                TotalCost = item.TotalCost
            };

            shoppingCartItems.Add(shoppingCartItem);
        }

        return shoppingCartItems;
    }
}