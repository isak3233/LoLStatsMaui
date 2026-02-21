using LoLStatsMaui.ViewModels;
using Microsoft.Maui.Controls;
namespace LoLStatsMaui
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel(Navigation);

        }

    }
}
