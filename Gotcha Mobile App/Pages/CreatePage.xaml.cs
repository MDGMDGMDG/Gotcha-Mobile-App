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
    public partial class CreatePage : ContentPage
    {
        public CreatePage()
        {
            InitializeComponent();
        }

        private async void Create_Clicked(object sender, EventArgs e)
        {
            busyView.IsVisible = true;  

            var game = new Game();
            game.NameOfGame = nameOfGame.Text;
            game.StartDate = DateTime.Now;
            if (onTheRunSwitch.IsToggled)
             game.CriminalName = nameOfUser.Text;
            if (!onTheRunSwitch.IsToggled)
                game.PoliceName = nameOfUser.Text;

            // set global variables
            App.CurrentGame = game;
            App.OnTheRun = onTheRunSwitch.IsToggled;
            App.CurrentUserName = App.CurrentUserName;

            await FirebaseService.Create(game);
            await Navigation.PushAsync(new WaitPage());

            busyView.IsVisible = false;
        }
    }
}