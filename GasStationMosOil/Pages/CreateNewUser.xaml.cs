using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GasStationMosOil.Pages
{
    /// <summary>
    /// Логика взаимодействия для CreateNewUser.xaml
    /// </summary>
    public partial class CreateNewUser : UserControl
    {
        public CreateNewUser()
        {
            InitializeComponent();
            setData();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            setData();
        }


        private void btnChangeAccept_Click(object sender, RoutedEventArgs e)
        {
            invalidateData();
        }


        #region MyFunc


        private void invalidateData()
        {


            DateTime dateResult;
            Regex regexEmail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Regex regexPassprot = new Regex(@"\d{4}\s\d{6}");
            Match matchEmail = regexEmail.Match(txtBoxEmail.Text);
            if (String.IsNullOrEmpty(txtBoxName.Text) || String.IsNullOrEmpty(txtBoxSurname.Text) ||
            String.IsNullOrEmpty(txtBoxEmail.Text) || String.IsNullOrEmpty(txtBoxPassword.Text) ||
            String.IsNullOrEmpty(txtBoxPhoneNumber.Text) || String.IsNullOrEmpty(txtBoxBirthday.Text))
            {
                MessageBox.Show("Некорректно введены данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            //if (!DateTime.TryParse(txtBoxBirthday.Text, out dateResult) || !matchEmail.Success || DateTime.Parse(txtBoxBirthday.Text) > DateTime.Now || DateTime.Parse(txtBoxBirthday.Text) < DateTime.Now.AddYears(-100))
            //{
            //    MessageBox.Show("Некорректно введены данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}
            if (txtBoxPhoneNumber.Text.Any(char.IsLetter) || txtBoxPhoneNumber.Text.Any(char.IsPunctuation))
            {
                MessageBox.Show("Некорректно введены данные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                updateData();
            }

        }

        private void setData()
        {

            DB db = new DB();
            comBoxGender.ItemsSource = db.genderAll().DefaultView;
            comBoxRole.ItemsSource = db.roleAll().DefaultView;
            comBoxStatus.ItemsSource = db.StatusAll().DefaultView;


        }





        private void updateData()
        {
            DB db = new DB();
            string sql = "insert user SET login = @loginbox, password = @passwordbox, surname = @surnamebox, name = @namebox, patronymic = @patronymicbox, data = @databox, phoneNumber = @phoneNumber, email = @emailbox, idRole = @idrole, idStatus = @idstatus, idGender = @idgender ";
            MySqlCommand cmd = new MySqlCommand(sql, db.getConnection());
            cmd.CommandType = CommandType.Text;
            cmd.Parameters.Add("@loginbox", MySqlDbType.VarChar).Value = txtBoxLogin.Text;
            cmd.Parameters.Add("@passwordbox", MySqlDbType.VarChar).Value = txtBoxPassword.Text;
            cmd.Parameters.Add("@surnamebox", MySqlDbType.VarChar).Value = txtBoxSurname.Text;
            cmd.Parameters.Add("@namebox", MySqlDbType.VarChar).Value = txtBoxName.Text;
            if (txtBoxPatronymic.Text == string.Empty)
            {   
                cmd.Parameters.Add("@patronymicbox", MySqlDbType.VarChar).Value = string.Empty;
            }
            else
            {
                cmd.Parameters.Add("@patronymicbox", MySqlDbType.VarChar).Value = txtBoxPatronymic.Text;
            }
            cmd.Parameters.Add("@databox", MySqlDbType.Date).Value = Convert.ToDateTime(txtBoxBirthday.Text);
            cmd.Parameters.Add("@phoneNumber", MySqlDbType.VarChar).Value = txtBoxPhoneNumber.Text;
            cmd.Parameters.Add("@emailbox", MySqlDbType.VarChar).Value = txtBoxEmail.Text;
            cmd.Parameters.Add("@idrole", MySqlDbType.VarChar).Value = comBoxRole.SelectedIndex;
            cmd.Parameters.Add("@idstatus", MySqlDbType.VarChar).Value = comBoxStatus.SelectedIndex;
            cmd.Parameters.Add("@idgender", MySqlDbType.VarChar).Value = comBoxGender.SelectedIndex;
            MySqlDataReader MyReader2;
            db.openConnection();
            MyReader2 = cmd.ExecuteReader();
            db.closeConnection();
        }

        private void setPropertyEnabledAndVisibiliry(bool prop, Visibility visibility)
        {
            txtBoxName.IsEnabled = prop;
            txtBoxSurname.IsEnabled = prop;
            txtBoxPatronymic.IsEnabled = prop;
            txtBoxEmail.IsEnabled = prop;
            txtBoxPassword.IsEnabled = prop;
            txtBoxPhoneNumber.IsEnabled = prop;
            txtBoxBirthday.IsEnabled = prop;
            comBoxGender.IsEnabled = prop;
            comBoxRole.IsEnabled = prop;
            comBoxStatus.IsEnabled = prop;
            btnChangeAccept.Visibility = visibility;
        }
        #endregion


    

    }
}