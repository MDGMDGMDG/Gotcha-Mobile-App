using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gotcha_Mobile_App.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WaitPage : ContentPage
    {
        private Timer _timer;

        public WaitPage()
        {
            InitializeComponent();

            var startTimeSpan = TimeSpan.Zero;
            var periodTimeSpan = TimeSpan.FromSeconds(5);

            _timer = new System.Threading.Timer(async(e) =>
            {
                try
                {
                    App.CurrentGame = await FirebaseService.Get(App.CurrentGame);
                }
                catch (Exception ex)
                {
                    await App.Current.MainPage.DisplayAlert("Oops!", $"There's an issue with retrieving your data. Ex: '{ex.Message}'", "OK");
                    return;
                }

                Device.BeginInvokeOnMainThread(() =>
                {
                    string playerText = App.CurrentGame.NumberOfPlayers == 1 ? "player" : "players";
                    numberOfPlayersLabel.Text = $"{App.CurrentGame.NumberOfPlayers} {playerText} connected...";
                    if (App.CurrentGame.NumberOfPlayers > 1)
                        readyButton.IsEnabled = true;
                });
            }, null, startTimeSpan, periodTimeSpan);
        }

        private async void Ready_Clicked(object sender, EventArgs e)
        {
            _timer = null;
            await Navigation.PushAsync(new GamePage());
        }
    }
}