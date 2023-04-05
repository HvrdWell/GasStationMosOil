using System;
using System.Data;
using System.Globalization;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Effects;
using System.Windows.Threading;
using GasolineMosOil.Model;
using GasolineMosOil.ViewModel;
using GasStationMosOil;
using GasStationMosOil.Model;
using GasStationMosOil.ViewModel;

namespace GasolineMosOil.View;


public partial class MainWindow
{
 
    private DateTime _sessionTime = new(0, 0);
    public delegate void Action<in T>(T obj);
    
    private readonly MainWindowViewModel _viewModel;
    private readonly BlurEffect _blur;
    DB db = new DB();
    BarCode BarCode = new BarCode();
    SerialPort serialPort = new SerialPort();
    DataTable UserData = new DataTable();
    CodeScannerViewModel codeScannerViewModel = new CodeScannerViewModel();
    public MainWindow(DataTable table)
    {
        InitializeComponent();
        UserData = table;
        _viewModel = new MainWindowViewModel();
        serialPort = BarCode.CreateNewScanner();
        serialPort.Open(); 
        serialPort.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(serialPort_DataRecieved);
        DataContext = _viewModel;
        UserNameLabel.Content = table.Rows[0][4]+" "+ table.Rows[0][5]+" "+ table.Rows[0][6];
        roleOfUserTextBlock.Text = db.nameOfUserRole(Convert.ToInt32(table.Rows[0][10]));

        CreateNozzlePosts(_viewModel.NozzlePostCount);
      
        var timer = new DispatcherTimer();
        timer.Tick += _timer_Tick;
        timer.Interval = new TimeSpan(0, 0, 1);
        timer.Start();
    }
    private void serialPort_DataRecieved(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
    {
        try {
            codeScannerViewModel.ScanProduct(serialPort.ReadExisting());
            _viewModel.UpdateReceiptItems(CurrentSession.CurrentReceipt);
            _viewModel.SetGoodsSummary(CurrentSession.CurrentReceipt.GetGoodsSummary());
        }
        catch (Exception ex)
        {

        }
    }
    private void grid_Loaded(object sender, RoutedEventArgs e)
    {
        ShoppingCartItem.Update(CurrentSession.CurrentReceipt);
    }
    public void UpdateAfterChange()
    {
        CreateNozzlePosts(_viewModel.NozzlePostCount);
        ShoppingCartItem.Update(CurrentSession.CurrentReceipt);

    }
    public void CreateNozzlePosts(DataTable count)
    {
        NozzleList.Children.Clear();

  
        for (var i = 1; i <= count.Rows.Count; i++)
        {
            var dataProvider = new ConcreteNozzlePostViewModel(i);
            var viewModel = new NozzlePostViewModel(i, dataProvider);

            var nozzlePostControl = new NozzlePost(i, viewModel)
            {
                DataContext = viewModel
            };

            NozzleList.Children.Add(nozzlePostControl);
        }
    }

    private void _timer_Tick(object? sender, EventArgs e)
    {
        DateTimeFoot.Text = DateTime.Now.ToString("D");
        CurrentTime.Text = DateTime.Now.ToString("H:mm:ss");

        _sessionTime = _sessionTime.AddSeconds(1);
        ShiftTime.Text = $"Смена открыта: {_sessionTime:H:mm}";

        CommandManager.InvalidateRequerySuggested();
    }

    private void AddGoodsButton_OnClick(object sender, RoutedEventArgs e)
    {
        var dataProvider = new ConcreteGoodsSelectorViewModel();
        var goodsSelectorViewModel = new GoodsSelectorViewModel(dataProvider);

        var goodsSelector = new GoodsSelector(goodsSelectorViewModel, _viewModel)
        {
            Topmost = true,
            WindowStartupLocation = WindowStartupLocation.CenterScreen
        };
        goodsSelector.Show();
    }

    private void MainWindow_OnClosed(object? sender, EventArgs e)
    {
        Environment.Exit(0);
    }

    private void FinishPaymentButton_OnClick(object sender, RoutedEventArgs e)
    {
        _viewModel.FinishPayment(Convert.ToInt32(UserData.Rows[0][0]));

    }

    private void FuelInfoButton_OnClick(object sender, RoutedEventArgs e)
    {
            _viewModel.UpdateTanksReservesInfo();
            ReservesUserOnlyPopup.IsOpen = true;
  
    }

   

    private void ShiftButton_OnClick(object sender, RoutedEventArgs e)
    {
        var shiftInfo = new ShiftInfo(Convert.ToString(UserNameLabel.Content))
        {
            Topmost = true,
            WindowStartupLocation = WindowStartupLocation.CenterScreen
        };

        Effect = _blur;
        shiftInfo.ShowDialog();

        Effect = null;
    }

    private void SystemButton_OnClick(object sender, RoutedEventArgs e)
    {
        if (roleOfUserTextBlock.Text == "Управляющий")
        {
            Action UpdateWindowAfterChange = delegate { UpdateAfterChange(); };
            var systemConfigurator = new SystemConfigurator(UpdateWindowAfterChange)
            {
                Topmost = true,
                WindowStartupLocation = WindowStartupLocation.CenterScreen
            };
            Effect = _blur;
            systemConfigurator.ShowDialog();

            Effect = null;

  

            _viewModel.UpdateReceiptItems(CurrentSession.CurrentReceipt);
        }
        else
        {
            AccessDeniedPopup.IsOpen = true;
        }
    }

    private void DepositedCash_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        string input = DepositedCash.Text;

        if (Regex.IsMatch(input, @"^\d+(\.\d{0,2})?$"))
        {
            UpdateFinalCost(double.Parse(input));
        }
        else
        {
            DepositedCash.Text = null;
        }

    }
    private void UpdateFinalCost(Double amountGiven)
    {
        ChangeCash.Text = (amountGiven - _viewModel.TotalCost > 0) ? (amountGiven - _viewModel.TotalCost).ToString() : "";
    }
}

public class FinishPaymentConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
            return boolValue ? "ОПЛАТА" : "ПУСК";
        throw new ArgumentException();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

public class UserAccessLevelConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
            return boolValue ? "Управляющий" : "Кассир";
        throw new ArgumentException();
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}