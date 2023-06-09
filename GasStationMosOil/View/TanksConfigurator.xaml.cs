using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GasolineMosOil.Domain;
using GasolineMosOil.Model;
using GasStationMosOil;
using MySqlConnector;

namespace GasolineMosOil.View;

public partial class TanksConfigurator
{
    public TanksConfigurator()
    {
        InitializeComponent();
        DB db = new DB();

        DataTable table = new DataTable();

        MySqlDataAdapter adapter = new MySqlDataAdapter();
        MySqlCommand command = new MySqlCommand("SELECT `storage`.id as storage, typefuel.nameTypeFuel, `storage`.count, typefuel.price FROM `storage` INNER JOIN typefuel ON  `storage`.idTypeFuel = typefuel.idTypeFuel WHERE `storage`.idTypeFuel = typefuel.idTypeFuel", db.getConnection());
        adapter.SelectCommand = command;
        adapter.Fill(table);


        TanksConfigurationGrid.ItemsSource = table.DefaultView;

    }

    private void AcceptChangesButton_Click(object sender, RoutedEventArgs e)
    {
        // Get the DataTable from the DataGrid's ItemsSource
        var dataTable = (TanksConfigurationGrid.ItemsSource as DataView)?.Table;
        if (dataTable == null) return;

        // Check for empty rows and cells and delete empty rows
        for (var i = dataTable.Rows.Count - 1; i >= 0; i--)
        {
            var isEmptyRow = true;
            for (var j = 0; j < dataTable.Columns.Count; j++)
                if (dataTable.Rows[i][j] != DBNull.Value && !string.IsNullOrWhiteSpace(dataTable.Rows[i][j].ToString()))
                {
                    isEmptyRow = false;
                    break;
                }

            if (isEmptyRow) dataTable.Rows.RemoveAt(i);
        }

        // Check that at least one row exists in the data table
        if (dataTable.Rows.Count == 0)
        {
            var errorOutput = new string(string.Concat("Вы не можете не иметь ни одного вида топлива.",
                "\nЕсли у вас кончились резервы, то установите их в нулевое значение, если вы не можете",
                " использовать ни одного поста, удалите их через Конфигуратор системы."));
            MessageBox.Show(errorOutput, "Как сапожник без сапог", MessageBoxButton.OK,
                MessageBoxImage.Error);
            return;
        }

        foreach (DataRow row in dataTable.Rows)
            if (row["Name"] == DBNull.Value || string.IsNullOrWhiteSpace(row["Name"].ToString()))
            {
                MessageBox.Show("Некорректное значение в поле Название", "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

        // Check if the values in the Reserve and Price columns are valid Doubles
        foreach (DataRow row in dataTable.Rows)
        {
            if (row["Reserve"] == DBNull.Value || !double.TryParse(row["Reserve"].ToString(),
                    NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out _))
            {
                MessageBox.Show("Некорректное значение в поле Резерв", "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            if (row["Price"] == DBNull.Value || !double.TryParse(row["Price"].ToString(),
                    NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out _))
            {
                MessageBox.Show("Некорректное значение в поле Цена", "Ошибка", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
        }

        // Serialize the DataTable to the CSV file
        Serialize.WriteDataTableToCsvWithAutoGeneratedId(dataTable, "Tanks.csv");
        CurrentSession.CreateNewReceipt(0);
        CurrentSession.HistoryOfReceipts = new List<Receipt>();
        Close();
    }



    private void DeleteRowMenuItem_Click(object sender, RoutedEventArgs e)
    {
        if (TanksConfigurationGrid.SelectedItem != null)
            try
            {
                var selectedItem = TanksConfigurationGrid.SelectedItem;
                var itemsSource = TanksConfigurationGrid.ItemsSource as IList;
                itemsSource?.Remove(selectedItem);
            }
            catch
            {
                MessageBox.Show(
                    "Наведите курсор на строку с данными и повторите действие, чтобы удалить её",
                    "Ошибка удаления строки", MessageBoxButton.OK, MessageBoxImage.Error);
            }
    }


    private void DataGrid_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
    {
        var dataGrid = (DataGrid)sender;
        if (dataGrid.SelectedCells.Count > 0)
        {
            var selectedRow = dataGrid.SelectedCells[0].Item;
            dataGrid.SelectedItem = selectedRow;
        }
    }
}