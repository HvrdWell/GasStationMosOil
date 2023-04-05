using System;
using System.Data;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using GasolineMosOil.Model;
using GasolineMosOil.ViewModel;

namespace GasolineMosOil.View;

public partial class GoodsSelector
{
    private void MovingWindow(object sender, RoutedEventArgs e)
    {
        DragMove();
    }

    private DataTable? _shoppingCartGoodsTable;

    private readonly GoodsSelectorViewModel _viewModel;
    private readonly MainWindowViewModel _mainWindowViewModel;

    public GoodsSelector(GoodsSelectorViewModel viewModel, MainWindowViewModel mainWindowViewModel)
    {
        _viewModel = viewModel;
        _mainWindowViewModel = mainWindowViewModel;

        InitializeComponent();

        GoodsGrid.ItemsSource = _viewModel.GoodsDataTable.DefaultView;
        ShowShoppingCartChanges();
        GoodsGrid.GridLinesVisibility = DataGridGridLinesVisibility.All;
    }

    private void Row_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        try
        {
            if (GoodsGrid.SelectedItem == null)
                return;
            var dr = GoodsGrid.SelectedItem as DataRowView;
            var dr1 = dr?.Row;

            var itemId = int.Parse(Convert.ToString(dr1?.ItemArray[0]) ?? throw new InvalidOperationException());
            var quantity = double.Parse(Convert.ToString(dr1?.ItemArray[2]) ?? throw new InvalidOperationException());


            CurrentSession.CurrentReceipt.AddIdToCommodityItem(itemId, quantity);
            ShowShoppingCartChanges();
        }
        catch
        {
        }
    }

    private void SearchGoodsTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        var t = (TextBox)sender;
        var filterText = t.Text.ToLower();

        var filteredDataTable = _viewModel.GetFilteredGoodsDataTable(filterText);

        if (filteredDataTable.Rows.Count == 0)
            return;

        GoodsGrid.ItemsSource = filteredDataTable.DefaultView;
    }

    private void SearchGoodsTextBox_OnGotFocus(object sender, RoutedEventArgs e)
    {
        var t = (TextBox)sender;
        t.Text = "";
        SearchGoodsPlaceholder.Visibility = Visibility.Hidden;
    }

    private void ShoppingCartGrid_OnCellEditEnding(object? sender, DataGridCellEditEndingEventArgs e)
    {
        var dataGrid = sender as DataGrid;
        int count;
        try
        {
            var value = (e.EditingElement as TextBox)?.Text;
            Debug.Assert(value != null, nameof(value) + " != null");
            count = int.Parse(value);
        }
        catch (FormatException)
        {
            MessageTextPopUp.Text = "Ошибка: неверное значение";
            MessagePopup.IsOpen = true;
            ShowShoppingCartChanges();
            return;
        }
        catch (OverflowException)
        {
            MessageTextPopUp.Text = "Ошибка: Вы пытаетесь добавить товара гораздо больше,\n чем имеется в наличии";
            MessagePopup.IsOpen = true;
            ShowShoppingCartChanges();
            return;
        }

        Debug.Assert(dataGrid != null, nameof(dataGrid) + " != null");
        var index = dataGrid.ItemContainerGenerator.IndexFromContainer(e.Row);

        _shoppingCartGoodsTable = ShoppingCartItem.Update(CurrentSession.CurrentReceipt);

        var row = _shoppingCartGoodsTable?.Rows[index];
        var values = row?.ItemArray;
        var id = (int)(values?[0] ?? 0);

        try
        {
            CurrentSession.CurrentReceipt.ChangeCountById(id, count);
        }
        catch (ArgumentException)
        {
            MessageTextPopUp.Text = "Ошибка: вы пытаетесь добавить товара больше,\n чем имеется в наличии";
            MessagePopup.IsOpen = true;
        }

        ShowShoppingCartChanges();
    }

    private void ShoppingCartGrid_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        var cell = ShoppingCartGrid.CurrentCell.Column;

        // Check if the double-click did not occur in the Count column
        if (cell != null && cell != ShoppingCartGrid.Columns[2])
            try
            {
                if (ShoppingCartGrid.SelectedItem == null)
                    return;
                var dr = ShoppingCartGrid.SelectedItem as DataRowView;
                var dr1 = dr?.Row;

                var itemId = int.Parse(Convert.ToString(dr1?.ItemArray[0]) ?? throw new InvalidOperationException());

                CurrentSession.CurrentReceipt.RemoveIdFromCommodityItem(itemId);
                ShowShoppingCartChanges();
            }
            catch
            {
                // Ignored
            }
    }

    private void ShowShoppingCartChanges()
    {
        _shoppingCartGoodsTable = ShoppingCartItem.Update(CurrentSession.CurrentReceipt);
        ShoppingCartGrid.ItemsSource = _shoppingCartGoodsTable?.DefaultView;
        _mainWindowViewModel.UpdateReceiptItems(CurrentSession.CurrentReceipt);
        _mainWindowViewModel.SetGoodsSummary(CurrentSession.CurrentReceipt.GetGoodsSummary());
    }

    private void ClearShoppingCart(object sender, RoutedEventArgs e)
    {
        CurrentSession.CurrentReceipt.ClearCommodityItem();
        ShowShoppingCartChanges();
    }

    private void WindowClose(object? sender, EventArgs e)
    {
        try
        {
            Close();
        }
        catch (InvalidOperationException)
        {
            // Ignored
        }
    }
}