using System;
using System.Collections.Generic;
using System.Data;
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
using MySqlConnector;

namespace GasStationMosOil.View
{
    /// <summary>
    /// Логика взаимодействия для ChangeTanks.xaml
    /// </summary>
    public partial class ChangeTanks : Window
    {
        DB db = new DB();
        public delegate void UpdateWindow<in T>(T obj);
        public ChangeTanks()
        {
            InitializeComponent();
            loadData();
        }
        public void loadData()
        {

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT typefuel.idTypeFuel, typefuel.nameTypeFuel, typefuel.price, `storage`.count FROM typefuel INNER JOIN `storage` ON typefuel.idTypeFuel = `storage`.idTypeFuel WHERE typefuel.idTypeFuel = `storage`.idTypeFuel", db.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);


            var tanksDataTable = table;
            TanksConfigurationGrid.ItemsSource = tanksDataTable.DefaultView;
        }


        private void DataGrid_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
        }
        private void DeleteRowMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

   

        private void btnChangeUser_Click(object sender, RoutedEventArgs e)
        {
            TextBlock? x = TanksConfigurationGrid.Columns[0].GetCellContent(TanksConfigurationGrid.Items[TanksConfigurationGrid.SelectedIndex]) as TextBlock;
            Action UpdateWindow = delegate { loadData(); };

            if (x != null)
            {
                int s = Convert.ToInt32(x?.Text);
                var ChangeTanksOpen = new ChangeTanksOpen(s, UpdateWindow)
                {
                    Topmost = true,
                    WindowStartupLocation = WindowStartupLocation.CenterScreen
                };
                ChangeTanksOpen.ShowDialog();
            }
        }


        private void CancelChangesButtonTanks_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
