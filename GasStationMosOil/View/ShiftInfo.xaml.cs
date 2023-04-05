using System.Diagnostics;
using System.Text;
using System.Windows;
using GasolineMosOil.Model;

namespace GasolineMosOil.View;

public partial class ShiftInfo
{
    string UserName;
    public ShiftInfo(string UserName)
    {
        InitializeComponent();
        this.UserName = UserName;
        Revenue.Text = CurrentSession.Revenue.ToString("C2");
        ShowHistoryOfReceipts();
    }

    private void ShowHistoryOfReceipts()
    {
        var separator = "\n--------------------\n\n";
        var outputText = new StringBuilder();
        outputText.Append(string.Concat("Продавец: ", UserName, "\n"));
        outputText.Append(separator);
        foreach (var receipt in CurrentSession.HistoryOfReceipts)
        {
            if (receipt.RelateNozzlePost != null)
            {
                var post = receipt.RelateNozzlePost;
                outputText.Append(string.Concat("ПОСТ: ", post.NozzlelId, "\n"));
                outputText.Append(string.Concat("Топливо: ", post.SelectedFuelName, "\n"));
                outputText.Append(string.Concat("Количество: ", post.LiterCount.ToString("N2"), " л.\n"));
                outputText.Append(string.Concat("Цена: ", post.Price, " за л.\n"));
                outputText.Append(string.Concat("ИТОГО: ", post.Summary.ToString("C2"), "\n"));
            }

            if (receipt.GoodsSummary > 0)
            {
                outputText.Append("\nПродуктовая корзина:\n");
                outputText.Append("Арт.\tНазвание\tКол-во\tЦена\tИтого\n");
                foreach (var position in receipt.CommodityItem)
                {
                    outputText.Append(string.Join("\t", position.id, position.Name, position.Count, position.Price,
                        position.TotalCost));
                    outputText.Append('\n');
                }

                outputText.Append(string.Concat("\n", "ИТОГО: ", receipt.TextGoodsSummary, "\n"));
            }

            string totalCost;
            if (receipt.RelateNozzlePost != null)
                totalCost = (receipt.RelateNozzlePost.Summary + receipt.GoodsSummary).ToString("C2");
            else totalCost = receipt.GoodsSummary.ToString("C2");
            outputText.Append(string.Concat("\n", "ИТОГОВАЯ СУММА: ", totalCost));
            outputText.Append(separator);
        }

        HistoryReceiptsInfo.Text = outputText.ToString();
    }

    private void EndShift_Click(object sender, RoutedEventArgs e)
    {
        EndShiftPopup.IsOpen = true;
    }

    private void ContinueShift_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void SureEndShift_Click(object sender, RoutedEventArgs e)
    {
        Process.Start(Application.ResourceAssembly.Location);
        Application.Current.Shutdown();
    }
}