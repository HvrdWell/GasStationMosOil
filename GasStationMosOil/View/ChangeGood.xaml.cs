using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySqlConnector;

namespace GasStationMosOil.View
{
    /// <summary>
    /// Логика взаимодействия для ChangeGood.xaml
    /// </summary>
    public partial class ChangeGood : Window
    {
        DB db = new DB();
        public delegate void UpdateWindow<in T>(T obj);
        public ChangeGood()
        {
            InitializeComponent();

            loadData();
        }

        public void loadData()
        {

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT id, `name`, count, price FROM product", db.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);


            var goodsDataTable = table;
            GoodsConfigurationGrid.ItemsSource = goodsDataTable.DefaultView;
        }


        private void DataGrid_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
        }
        private void DeleteRowMenuItem_Click(object sender, RoutedEventArgs e)
        {
            TextBlock? x = GoodsConfigurationGrid.Columns[0].GetCellContent(GoodsConfigurationGrid.Items[GoodsConfigurationGrid.SelectedIndex]) as TextBlock;
            if (x != null)
            {
                int s = Convert.ToInt32(x?.Text);
                db.deleteGood(s);
                loadData();
            }
            else
            {
                System.Windows.MessageBox.Show("Наведите курсор на строку с данными и повторите действие, чтобы удалить её", "Ошибка удаления", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void btnChangeUser_Click(object sender, RoutedEventArgs e)
        {
            TextBlock? x = GoodsConfigurationGrid.Columns[0].GetCellContent(GoodsConfigurationGrid.Items[GoodsConfigurationGrid.SelectedIndex]) as TextBlock;
            Action UpdateWindow = delegate { loadData(); };
            if (x != null)
            {
                int s = Convert.ToInt32(x?.Text);
                var ChangeGoodOpen = new ChangeGoodOpen(s, UpdateWindow)
                {
                    Topmost = true,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };
                ChangeGoodOpen.ShowDialog();
            }
          

        }

        private void ADdnewButton_Click(object sender, RoutedEventArgs e)
        {
            var AddGood = new AddGood()
            {
                Topmost = true,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            AddGood.ShowDialog();
        }

        private void CancelGoodsChangesButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
