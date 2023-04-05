
using GasolineMosOil.View;
using GasStationMosOil.View;
using MySqlConnector;
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

namespace GasStationMosOil
{
    /// <summary>
    /// Логика взаимодействия для Auth.xaml
    /// </summary>
    public partial class Auth : Window
    {
        public Auth()
        {
            InitializeComponent();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }



        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (loginTextBox.Text.Length > 0) // проверяем введён ли логин     
                {
                    if (PasswordTextBox.Password.Length > 0) // проверяем введён ли пароль         
                    {             // ищем в базе данных пользователя с такими данными   

                        DB db = new DB();

                        DataTable table = new DataTable();

                        MySqlDataAdapter adapter = new MySqlDataAdapter();

                        MySqlCommand command = new MySqlCommand("Select * FROM `user` WHERE `login` = @uL AND `password` = @uP", db.getConnection());
                        command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = loginTextBox.Text;
                        command.Parameters.Add("@uP", MySqlDbType.VarChar).Value = PasswordTextBox.Password;

                        adapter.SelectCommand = command;
                        adapter.Fill(table);

                        if (table.Rows.Count > 0)
                        {
                            if (Convert.ToInt32(table.Rows[0][10]) == 3)
                            {
                                GasolineMosOil.View.MainWindow Casher = new GasolineMosOil.View.MainWindow(table);
                                Casher.Show();
                                this.Hide();
                            }
                            if(Convert.ToInt32(table.Rows[0][10]) == 4)
                            {

                                GasolineMosOil.View.MainWindow Casher = new GasolineMosOil.View.MainWindow(table);
                                Casher.Show();
                                this.Hide();
                                

                            }
                            if (Convert.ToInt32(table.Rows[0][10]) == 1)
                            {

                                AdminPanel adminPanel = new AdminPanel();
                                adminPanel.Show();
                                this.Hide();


                            }
                        }
                        else MessageBox.Show("Пользователь не найден");
                    }
                    else MessageBox.Show("Введите пароль");
                }
                else MessageBox.Show("Введите логин");
            }
            catch
            {
                MessageBox.Show("Отсуствует подключение к базе данных!");
            }
        }

   
    }
}
