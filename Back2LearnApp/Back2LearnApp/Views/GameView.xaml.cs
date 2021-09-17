using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Back2LearnApp.ViewModels;

namespace Back2LearnApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GameView : ContentPage
    {
        GameViewModel vm;

        public GameView()
        {
            InitializeComponent();

            vm = new GameViewModel();
            BindingContext = vm;
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            await Task.Run(() => vm.LoadDataCommand.Execute(null));
        }
    }
}