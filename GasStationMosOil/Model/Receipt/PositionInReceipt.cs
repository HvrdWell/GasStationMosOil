using GasolineMosOil.Domain;

namespace GasolineMosOil.Model
{
    public class PositionInReceipt
    {
        public int id { get; init; }
        public int Count { get; set; }

        public string? Name
        {
            get
            {
                (string? name, _) = GoodsModel.GetNameAndPriceById(id);
                return name;
            }
        }
    
        public string Price
        {
            get
            {
                (_, double price) = GoodsModel.GetNameAndPriceById(id);
                return price.ToString("C2");
            }
        }
        

        public double TotalCost =>
            GoodsModel.GetPriceById(id) * Count;
    }
}