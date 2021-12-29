using Gotcha_Mobile_App.Model;
using Gotcha_Mobile_App.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gotcha_Mobile_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JoinPage : ContentPage
    {
        public JoinPage()
        {
            InitializeComponent();

            Refresh_Clicked(null, null);
        }

        private async void Refresh_Clicked(object sender, EventArgs e)
        {
            var games = await FirebaseService.GetGames();
            listOfGames.ItemsSource = games.OrderByDescending(x => x.StartDate).ToList();
        }

        private async void listOfGames_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            busyView.IsVisible = true;
            if (e.SelectedItem is Game selectedGame)
            {
                if (onTheRunSwitch.IsToggled)
                    selectedGame.CriminalName = nameOfUser.Text;
                if (!onTheRunSwitch.IsToggled)
                    selectedGame.PoliceName = nameOfUser.Text;

                // set global variables
                App.CurrentGame = selectedGame;
                App.OnTheRun = onTheRunSwitch.IsToggled;
                App.CurrentUserName = App.CurrentUserName;

                await FirebaseService.AddToNumPlayers(selectedGame);
                await Navigation.PushAsync(new WaitPage());
            }
            busyView.IsVisible = false; 
        }
    }
}