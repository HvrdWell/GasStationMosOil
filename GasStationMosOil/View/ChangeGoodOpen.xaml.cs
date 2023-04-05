using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;

namespace GasStationMosOil.View
{

    public partial class ChangeGoodOpen : Window
    {
        private Action UpdateWindowAfterChange;
        private int idOfGood;
        DB db = new DB();
        public ChangeGoodOpen(int id, Action UpdateWindowAfterChange)
        {
            InitializeComponent();
            this.UpdateWindowAfterChange = UpdateWindowAfterChange;
            this.idOfGood = id;
            LoadData(idOfGood);
        }

        private void LoadData(int id)
        {
            DataTable table = db.infoAboutGood(id);
            idGood.Text = Convert.ToString(table.Rows[0][0]);
            idName.Text = Convert.ToString(table.Rows[0][1]);
            priceGood.Text = Convert.ToString(table.Rows[0][2]);
            countGood.Text = Convert.ToString(table.Rows[0][3]);
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ChangeGoodButton_Click(object sender, RoutedEventArgs e)
        {
            
            db.UpdateGoods(idOfGood, idName.Text, Convert.ToSingle(priceGood.Text), Convert.ToInt32(countGood.Text));
            UpdateWindowAfterChange();
            Close();
        }
    }
}
