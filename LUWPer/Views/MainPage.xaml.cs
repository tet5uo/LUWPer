using LUWPer.Models;
using LUWPer.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace LUWPer.Views
{
    public sealed partial class MainPage : Page
    {
        private MainViewModel ViewModel
        {
            get { return DataContext as MainViewModel; }
        }

        public MainPage()
        {
            InitializeComponent();
        }

    }
}
