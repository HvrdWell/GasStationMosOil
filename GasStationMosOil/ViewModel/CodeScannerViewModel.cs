using GasolineMosOil.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace GasStationMosOil.ViewModel
{
    class CodeScannerViewModel
    {
        DB db = new DB();
      
        public void ScanProduct(string recievedData)
        {
            int idOfProduct = db.findProductByBarCode(recievedData);
           
            CurrentSession.CurrentReceipt.AddIdToCommodityItem(idOfProduct, db.howManyProductsCount(idOfProduct));
            
        }
       
    }
}
