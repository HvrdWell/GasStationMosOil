using GasolineMosOil.View;
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

namespace GasStationMosOil.View
{
    /// <summary>
    /// Логика взаимодействия для ChangeTanksOpen.xaml
    /// </summary>
    public partial class ChangeTanksOpen : Window
    {
        private Action UpdateWindowAfterChange;
        private int idOfTanks;
        DB db = new DB();
        public ChangeTanksOpen(int id, Action UpdateWindowAfterChange)
        {
            InitializeComponent();
            this.UpdateWindowAfterChange = UpdateWindowAfterChange;
            this.idOfTanks = id;
            LoadData(idOfTanks);
        }
        private void LoadData(int id)
        {
            DataTable table = db.infoAboutTanks(id);
            idTanks.Text = Convert.ToString(table.Rows[0][0]);
            idName.Text = Convert.ToString(table.Rows[0][1]);
            priceTanks.Text = Convert.ToString(table.Rows[0][2]);
            countTanks.Text = Convert.ToString(table.Rows[0][3]);
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ChangeTanksButton_Click(object sender, RoutedEventArgs e)
        {

            db.UpdateTanks(idOfTanks, idName.Text, Convert.ToSingle(priceTanks.Text));
            db.UpdateStorage(idOfTanks, Convert.ToSingle(countTanks.Text));
            UpdateWindowAfterChange();
            Close();
        }
    }
}
