using System;
using System.Collections.Generic;
using System.Linq;
using GasolineMosOil.Domain;
using GasolineMosOil.ViewModel;

namespace GasolineMosOil.Model
{
    public class Receipt
    {
        public NozzlePostViewModel? RelateNozzlePost;
        public readonly List<PositionInReceipt> CommodityItem;

        public Receipt()
        {
            CommodityItem = new List<PositionInReceipt>();
        }

        public void AddIdToCommodityItem(int id, double quantity)
        {
            var position = CommodityItem.FirstOrDefault(x => x.id == id);
            if (position is null && quantity > 0)
                CommodityItem.Add(new PositionInReceipt { id = id, Count = 1 });
            else
            if (position != null && position.Count < quantity)
                position.Count++;
        }

        public void RemoveIdFromCommodityItem(int id)
        {
            var position = CommodityItem.FirstOrDefault(x => x.id == id);
            if (position != null)
                CommodityItem.Remove(position);
        }

        public void ClearCommodityItem()
        {
            CommodityItem.Clear();
        }

        public void ChangeCountById(int id, int count)
        {
            var position = CommodityItem.FirstOrDefault(x => x.id == id);
            if (position is null)
            {
                CommodityItem.Add(new PositionInReceipt { id = id, Count = 1 });
            }
            else
            {
                if (count == 0) RemoveIdFromCommodityItem(id);
                if (GoodsModel.GetRemainingById(id) >= count)
                    position.Count = count;
                else
                    throw new ArgumentException();
            }
        }

        public double GetGoodsSummary()
        {
            var sum = CommodityItem.Sum(x => x.TotalCost);
            return sum;
        }

        public string TextGoodsSummary => CommodityItem.Sum(x => x.TotalCost).ToString("C2");
        public double GoodsSummary => CommodityItem.Sum(x => x.TotalCost);
    }
}