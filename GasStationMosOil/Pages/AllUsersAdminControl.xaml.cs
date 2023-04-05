using GasStationMosOil;
using GasStationMosOil.Pages;
using GasStationMosOil.View;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MessageBox = System.Windows.MessageBox;
using UserControl = System.Windows.Controls.UserControl;

namespace GasStationMosOil.Pages
{
    /// <summary>
    /// Логика взаимодействия для AllUsersAdminControl.xaml
    /// </summary>
    public partial class AllUsersAdminControl : UserControl
    {
        #region Properties
        AdminPanel adminPage;
        #endregion
        public AllUsersAdminControl(AdminPanel adminPage)
        {
            InitializeComponent();
            this.adminPage = adminPage;
        }
        #region Func
        #region FormFunc
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            setData();
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if(comBoxStaffClient.Text == "Управляющий")
            {
                loadClients();
            }
            else if(comBoxStaffClient.Text == "Кассир")
            {
                loadStaff();
            }
            else
            {
                MessageBox.Show("Вы не выбрали параметры для построения таблицы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void btnChangeUser_Click(object sender, RoutedEventArgs e)
        {
            TextBlock? x = dataGridViewUsers.Columns[0].GetCellContent(dataGridViewUsers.Items[dataGridViewUsers.SelectedIndex]) as TextBlock;

            if (x != null)
            {
                int s = Convert.ToInt32(x?.Text);
                adminPage.contentControl.Content = new ChangeStaffAdminControl(s, this, adminPage);


            }
        }
        #endregion

        #region MyFunc
        private void setData()
        {
            comBoxStaffClient.Items.Clear();
            comBoxStaffClient.Items.Add("Управляющий");
            comBoxStaffClient.Items.Add("Кассир");
        }

        internal void loadStaff()
        {
            DB db = new DB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT idUser, surname, name, patronymic, phoneNumber, data FROM user WHERE `user`.idRole = 3", db.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);


            dataGridViewUsers.ItemsSource = table.DefaultView;
        }

        internal void loadClients()
        {
            DB db = new DB();

            DataTable table = new DataTable();

            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = new MySqlCommand("SELECT idUser, surname, name, patronymic, phoneNumber, data FROM user WHERE `user`.idRole = 4", db.getConnection());
            adapter.SelectCommand = command;
            adapter.Fill(table);

            dataGridViewUsers.ItemsSource = table.DefaultView;
        }
        #endregion

        #endregion
    }
}
