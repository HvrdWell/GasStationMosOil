using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using GalaSoft.MvvmLight.Messaging;
using GasolineMosOil.Data;
using GasolineMosOil.Domain;
using GasolineMosOil.Model;


namespace GasolineMosOil.ViewModel;

// ViewModel interface
public interface IMainWindowViewModel
{
    int NozzlePostCount
    {
        get
        {
            var configurationData = new ConfigurationData();
            if (configurationData == null) throw new ArgumentNullException(nameof(configurationData));
            return configurationData.NozzlePostCount;
        }
    }
}

public class ConcreteMainWindowViewModel : IConfigurationDataProvider
{
    private readonly ConfigurationData _configurationData;

    public int NozzlePostCount => _configurationData.NozzlePostCount;

    public ConcreteMainWindowViewModel()
    {
        _configurationData = new ConfigurationData();
    }
}

// Data interface
public interface IConfigurationDataProvider
{
    int NozzlePostCount { get; }
}

// ViewModel class
public sealed class MainWindowViewModel : INotifyPropertyChanged, IMainWindowViewModel
{
    private ConfigurationData _configurationData;

    public MainWindowViewModel()
    {
        var dataProvider = Deserialize.DeserializeConfiguration();
        NozzlePostViewModel.SelectedIdChanged += OnNozzlePostUserControlActive;
        _configurationData = dataProvider;
        ReceiptItems = new ObservableCollection<ShoppingCartItem>();
        IsWindowFree = true;
        IsPaymentReady = true;
        Messenger.Default.Register<FillUpChangedMessage>(this, OnFillUpChangedMessageReceived);
        Messenger.Default.Register<FillUpEndedMessage>(this, OnFillUpEndedMessageReceived);
    }



    private void OnFillUpEndedMessageReceived(FillUpEndedMessage message)
    {
        IsPaymentReady = true;
        OnPropertyChanged(nameof(IsPaymentReady));
        OnPropertyChanged(nameof(TotalCostText));
        OnPropertyChanged(nameof(FinishPaymentType));
    }

    private void OnFillUpChangedMessageReceived(FillUpChangedMessage message)
    {
        OnPropertyChanged(nameof(TotalCostText));
        OnPropertyChanged(nameof(FinishPaymentType));
    }

    public DataTable NozzlePostCount
    {
        get
        {
            var nozzlePostData = new NozzlePostData(1);
            return nozzlePostData.NozzlePostCount;
        }
    }

    public List<string?>? PaymentTypeNames =>
        _configurationData.PaymentTypes?.Where(pt => pt.IsActive).Select(pt => pt.Name).ToList();

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        var handler = PropertyChanged;
        handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void UpdateNozzlePostCount()
    {
        _configurationData = Deserialize.DeserializeConfiguration();

        OnPropertyChanged(nameof(NozzlePostCount));
        OnPropertyChanged(nameof(PaymentTypeNames));
    }


    private NozzlePostViewModel? _selectedNozzlePostInstance;

    public NozzlePostViewModel? SelectedNozzlePostInstance
    {
        get => _selectedNozzlePostInstance;
        set
        {
            if (_selectedNozzlePostInstance == value) return;
            if (value is { IsNozzlePostBusy: true }) return;
            _selectedNozzlePostInstance = value;
            CurrentSession.CurrentReceipt.RelateNozzlePost = value;

            IsPaymentReady = true;
            OnPropertyChanged(nameof(IsPaymentReady));

            OnPropertyChanged(nameof(TotalCostText));
        }
    }

    private void OnNozzlePostUserControlActive(object? sender, NozzlePostViewModel? e)
    {
        SelectedNozzlePostInstance = e;
        OnPropertyChanged(nameof(SelectedNozzlePostInstance));

        OnPropertyChanged(nameof(ReceiptItems));
        OnPropertyChanged(nameof(TotalCostText));
        OnPropertyChanged(nameof(FinishPaymentType));
    }

    public string TotalCostText
    {
        get
        {
            if (SelectedNozzlePostInstance != null)
                if (!SelectedNozzlePostInstance.FillUp)
                    return (SelectedNozzlePostInstance.Summary + GoodsSummary).ToString("C2");
                else
                    return "Н/Д";
            return GoodsSummary.ToString("C2");
        }
    }

    public double TotalCost
    {
        get
        {
            if (SelectedNozzlePostInstance != null)
                return SelectedNozzlePostInstance.Summary + GoodsSummary;
            return GoodsSummary;
        }
    }

    public void SetGoodsSummary(double getGoodsSummary)
    {
        GoodsSummary = getGoodsSummary;
        IsPaymentReady = true;
        OnPropertyChanged(nameof(IsPaymentReady));
    }

    private double _goodsSummary;

    public double GoodsSummary
    {
        get => _goodsSummary;

        set
        {
            if (Math.Abs(_goodsSummary - value) < 0.001) return;
            _goodsSummary = value;
            OnPropertyChanged(nameof(TextGoodsSummary));
            OnPropertyChanged(nameof(ReceiptItems));
            OnPropertyChanged(nameof(TotalCostText));
            OnPropertyChanged(nameof(FinishPaymentType));
        }
    }

    public bool FinishPaymentType
    {
        get
        {
            if (SelectedNozzlePostInstance == null) return true;
            return !SelectedNozzlePostInstance.FillUp;
            // 1 - ОПЛАТА ; 0 - ПУСК
        }
    }

    public string TextGoodsSummary => _goodsSummary.ToString("C2");

    public bool IsWindowFree { get; set; }
    public bool IsPaymentReady { get; set; }

    public void UpdateReceiptItems(Receipt receipt)
    {
        ReceiptItems = new ObservableCollection<ShoppingCartItem>(ShoppingCartItem.UpdateCart(receipt));
        OnPropertyChanged(nameof(ReceiptItems));
        OnPropertyChanged(nameof(GoodsSummary));
        OnPropertyChanged(nameof(TextGoodsSummary));
        OnPropertyChanged(nameof(TotalCostText));
        OnPropertyChanged(nameof(FinishPaymentType));
    }

    public ObservableCollection<ShoppingCartItem> ReceiptItems { get; set; }

    public void FinishPayment(int UserId)
    {
        
        if (FinishPaymentType || SelectedNozzlePostInstance == null)
        {
            GasStationMosOil.DB db = new GasStationMosOil.DB();
            //GasStationMosOil.DB db = new GasStationMosOil.DB();
            //if (SelectedNozzlePostInstance.LiterCount != null) { 
            //    if (Convert.ToDecimal(SelectedNozzlePostInstance.LiterCount) == 0 || Convert.ToDecimal(SelectedNozzlePostInstance.Summary) != 0)
            //    {
            //        db.NewOrder(SelectedNozzlePostInstance.NozzlelId, UserId, Convert.ToDecimal(SelectedNozzlePostInstance.LiterCount), Convert.ToDecimal(Math.Round(Convert.ToDecimal(SelectedNozzlePostInstance.Summary))));
            //        db.MinusCount(SelectedNozzlePostInstance.SelectedFuelId, Convert.ToDecimal(SelectedNozzlePostInstance.LiterCount));
            //    }

            //}

            

            

            if (SelectedNozzlePostInstance != null)
            {
                int idOfCurrentOrder = db.NewOrder(SelectedNozzlePostInstance.NozzlelId, UserId, Convert.ToDecimal(SelectedNozzlePostInstance.LiterCount), Convert.ToDecimal(Math.Round(Convert.ToDecimal(TotalCost))));
                db.MinusCount(SelectedNozzlePostInstance.SelectedFuelId, Convert.ToDecimal(SelectedNozzlePostInstance.LiterCount));
                Serialize.UpdateGoodsFile(ReceiptItems, idOfCurrentOrder);
                OnPropertyChanged(nameof(TotalCost));
                CurrentSession.CreateNewReceipt(TotalCost);
                UpdateReceiptItems(CurrentSession.CurrentReceipt);
                SetGoodsSummary(CurrentSession.CurrentReceipt.GetGoodsSummary());
                if (!SelectedNozzlePostInstance.IsAlreadyFilledOut)
                {
                    SelectedNozzlePostInstance.StartFueling();
                    SelectedNozzlePostInstance.IsNozzlePostBusy = true;
                    SelectedNozzlePostInstance = null;
                    
                }
                else
                {
                    SelectedNozzlePostInstance.IsAlreadyFilledOut = false;
                    SelectedNozzlePostInstance = null;

                }
            }

            IsWindowFree = true;
            OnPropertyChanged(nameof(IsWindowFree));
            IsPaymentReady = false;
            OnPropertyChanged(nameof(IsPaymentReady));
        }
        else
        {
            Serialize.UpdateTanksFile(SelectedNozzlePostInstance);
            SelectedNozzlePostInstance.StartFueling();
            SelectedNozzlePostInstance.IsNozzlePostBusy = true;
            IsWindowFree = false;
            OnPropertyChanged(nameof(IsWindowFree));
            IsPaymentReady = false;
            OnPropertyChanged(nameof(IsPaymentReady));
        }

        OnPropertyChanged(nameof(SelectedNozzlePostInstance));
    }

    public string? UserLoginName
    {
        get => User.FullName;
        set
        {
            if (User.FullName == value) return;
            User.FullName = value;
            OnPropertyChanged(nameof(UserLoginName));
        }
    }

    public bool UserAccessLevel
    {
        get => User.IsAdmin;
        set
        {
            if (User.IsAdmin == value) return;
            User.IsAdmin = value;
            OnPropertyChanged(nameof(UserAccessLevel));
        }
    }

    public void UpdateTanksReservesInfo()
    {
        OnPropertyChanged(nameof(TanksReservesInfo));
    }

    public DataView TanksReservesInfo => Deserialize.GetDataTableFromDB().DefaultView;
}

public class FillUpChangedMessage
{
}

public class FillUpEndedMessage
{
}

public class UpdateUserMessage
{
}