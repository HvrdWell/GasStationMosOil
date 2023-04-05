using GasolineMosOil.Domain;

namespace GasolineMosOil.View;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App
{
    public App()
    {
        new DataSecure().EnsureAssetsExists();
    }
}