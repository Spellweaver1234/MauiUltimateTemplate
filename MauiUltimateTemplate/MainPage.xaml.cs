using MauiUltimateTemplate.Application.UI.ViewModels;

namespace MauiUltimateTemplate
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            (BindingContext as MainViewModel)?.LoadNotesCommand.Execute(null);
        }
    }
}
