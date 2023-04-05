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
using System.Xml.Linq;

namespace GasStationMosOil.View
{
    /// <summary>
    /// Логика взаимодействия для AddGood.xaml
    /// </summary>
    public partial class AddGood : Window
    {
        public AddGood()
        {
            InitializeComponent();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ChangeGoodButton_Click(object sender, RoutedEventArgs e)
        {
            DB db = new DB();
            db.insertProduct(idName.Text, Convert.ToSingle(priceGood.Text), Convert.ToInt32(countGood.Text));
            Close();
        }
    }
}
