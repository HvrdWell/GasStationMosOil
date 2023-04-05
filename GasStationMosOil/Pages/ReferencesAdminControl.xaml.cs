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
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GasStationMosOil.Pages
{
    /// <summary>
    /// Логика взаимодействия для ReferencesAdminControl.xaml
    /// </summary>
    public partial class ReferencesAdminControl : UserControl
    {
        DB db = new DB();
        public ReferencesAdminControl()
        {
            InitializeComponent();
            
        }
        #region Properties
        string comBoxTextDefence;
        #endregion
  
        #region Func
        #region FormFunc
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            setData();
        }

        private void btnChangeReferences_Click(object sender, RoutedEventArgs e)
        {
            setPropertyEnabledAndVisibiliry(false, Visibility.Visible);
            btnAcceptChanges.Visibility = Visibility.Visible;
            if (comBoxTextDefence == "Пол")
            {
                TextBlock x = dataGridViewReferences.Columns[1].GetCellContent(dataGridViewReferences.Items[dataGridViewReferences.SelectedIndex]) as TextBlock;
                txtBoxNameRef.Text = x?.Text;
            }
            if (comBoxTextDefence == "Роль")
            {
                TextBlock x = dataGridViewReferences.Columns[1].GetCellContent(dataGridViewReferences.Items[dataGridViewReferences.SelectedIndex]) as TextBlock;
                txtBoxNameRef.Text = x?.Text;
            } 
            if (comBoxTextDefence == "Пользовательские статусы")
            {
                TextBlock x = dataGridViewReferences.Columns[1].GetCellContent(dataGridViewReferences.Items[dataGridViewReferences.SelectedIndex]) as TextBlock;
                txtBoxNameRef.Text = x?.Text;
            }
            if (comBoxTextDefence == "Статусы заказа")
            {
                TextBlock x = dataGridViewReferences.Columns[1].GetCellContent(dataGridViewReferences.Items[dataGridViewReferences.SelectedIndex]) as TextBlock;
                txtBoxNameRef.Text = x?.Text;
            }
            if (comBoxTextDefence == "Бонусные карты")
            {

                    TextBlock x = dataGridViewReferences.Columns[2].GetCellContent(dataGridViewReferences.Items[dataGridViewReferences.SelectedIndex]) as TextBlock;
                    txtBoxNameRef.Text = x?.Text;


            }



        }
        private void btnAddNew_Click(object sender, RoutedEventArgs e)
        {
            setPropertyEnabledAndVisibiliry(true, Visibility.Visible);
            btnMakeNew.Visibility = Visibility.Visible;
            txtBoxNameRef.Text = "";
        }

        private void btnMakeNew_Click(object sender, RoutedEventArgs e)
        {
            setPropertyEnabledAndVisibiliry(true, Visibility.Collapsed);
            btnMakeNew.Visibility = Visibility.Collapsed;

            if (comBoxTextDefence == "Пол")
            {
                db.insertGender(txtBoxNameRef.Text);
                loadGender();
            }
            if (comBoxTextDefence == "Роль")
            {
                db.insertUserRole(txtBoxNameRef.Text);
                loadRole();
            };
            if (comBoxTextDefence == "Пользовательские статусы")
            {
                db.insertUserStatus(txtBoxNameRef.Text);
                loadUsersStatus();
            }
            if (comBoxTextDefence == "Бонусные карты")
            {
                if (txtBoxNameRef.Text.Any(char.IsDigit))
                {
                    db.insertbonusCards(Convert.ToInt32(txtBoxNameRef.Text));
                    bonusCards();
                }
                else
                {
                    MessageBox.Show("Введите буквы");
                    txtBoxNameRef.Text = string.Empty;
                }
            }


        }
        private void btnAcceptChanges_Click(object sender, RoutedEventArgs e)
        {
            setPropertyEnabledAndVisibiliry(false, Visibility.Visible);
            btnAcceptChanges.Visibility = Visibility.Collapsed;

            if (comBoxTextDefence == "Пол")
            {
                if (txtBoxNameRef.Text.Any(char.IsDigit))
                {
                    MessageBox.Show("Введите буквы");
                    txtBoxNameRef.Text = string.Empty;
                    
                }
                else
                {
                    db.UpdateGender(dataGridViewReferences.SelectedIndex + 1, txtBoxNameRef.Text);
                    loadGender();
                }
            }
            if (comBoxTextDefence == "Роль")
            {
                if (txtBoxNameRef.Text.Any(char.IsDigit))
                {
                    MessageBox.Show("Введите буквы");
                    txtBoxNameRef.Text = string.Empty;
                }
                else
                {
                    db.UpdateRole(dataGridViewReferences.SelectedIndex + 1, txtBoxNameRef.Text);
                    loadRole();
                }
            }
            if (comBoxTextDefence == "Пользовательские статусы")
            {
                if (txtBoxNameRef.Text.Any(char.IsDigit))
                {
                    MessageBox.Show("Введите буквы");
                    txtBoxNameRef.Text = string.Empty;
                }
                else
                {
                    db.UpdateUserStatus(dataGridViewReferences.SelectedIndex + 1, txtBoxNameRef.Text);
                    loadUsersStatus();
                }
            }
            if (comBoxTextDefence == "Бонусные карты")
            {
                if (txtBoxNameRef.Text.Any(char.IsDigit))
                {
                    db.UpdatebonusCards(dataGridViewReferences.SelectedIndex + 1, txtBoxNameRef.Text);
                    bonusCards();
                }
                else
                {
                    MessageBox.Show("Введите число");
                    txtBoxNameRef.Text = string.Empty;
                }
            }

            setPropertyEnabledAndVisibiliry(true, Visibility.Collapsed);
            
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (comBoxReferences.Text == "Бонусные карты")
            {
                TextColumnScores.Visibility = Visibility.Visible;
                TextColumnName.Visibility = Visibility.Collapsed;
                bonusCards();
            }
            else if (comBoxReferences.Text == "Пол")
            {
                TextColumnScores.Visibility = Visibility.Collapsed;
                TextColumnName.Visibility = Visibility.Visible;
                loadGender();
            }
            else if (comBoxReferences.Text == "Роль")
            {
                TextColumnScores.Visibility = Visibility.Collapsed;
                TextColumnName.Visibility = Visibility.Visible;
                loadRole();
            }
            else if (comBoxReferences.Text == "Пользовательские статусы")
            {
                TextColumnScores.Visibility = Visibility.Collapsed;
                TextColumnName.Visibility = Visibility.Visible;
                loadUsersStatus();
            }
            else
            {
                MessageBox.Show("Вы не выбрали параметры для построения таблицы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            setPropertyEnabledAndVisibiliry(true, Visibility.Collapsed);
            btnMakeNew.Visibility = Visibility.Collapsed;
            btnAcceptChanges.Visibility = Visibility.Collapsed;
            comBoxTextDefence = comBoxReferences.Text;
        }
        #endregion

        #region MyFunc
        private void setData()
        {
            comBoxReferences.Items.Clear();
            comBoxReferences.Items.Add("Бонусные карты");
            comBoxReferences.Items.Add("Пол");
            comBoxReferences.Items.Add("Роль");
            comBoxReferences.Items.Add("Пользовательские статусы");
        }

      
        private void loadGender()
        {
            TextColumnName.Visibility = Visibility.Visible;
            DB db = new DB();

            dataGridViewReferences.ItemsSource = db.genderAll().DefaultView;

        }
        private void loadRole()
        {
            TextColumnName.Visibility = Visibility.Visible;

            DB db = new DB();

            dataGridViewReferences.ItemsSource = db.roleAll().DefaultView;
        }
        private void loadUsersStatus()
        {
            TextColumnName.Visibility = Visibility.Visible;
            DB db = new DB();

            dataGridViewReferences.ItemsSource = db.StatusAll().DefaultView;
        }
        private void bonusCards()
        {
            TextColumnName.Visibility = Visibility.Collapsed;
            TextColumnScores.Visibility = Visibility.Visible;
            dataGridViewReferences.ItemsSource = db.bonusCards().DefaultView;
        }

        private void setPropertyEnabledAndVisibiliry(bool prop, Visibility visibility)
        {
            txtBlockNameRef.Visibility = visibility;
            txtBoxNameRef.Visibility = visibility;
            btnAddNew.IsEnabled = prop;
        }
        #endregion

        #endregion
    }

}
