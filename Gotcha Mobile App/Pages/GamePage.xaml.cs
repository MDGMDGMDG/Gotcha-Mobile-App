using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gotcha_Mobile_App.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamePage : ContentPage
    {
        private bool _isInitiated;
        private bool _shutDown;
        private CancellationTokenSource cts;
        private Task _task;
        private Location _oldLocation { get; set; }
        private Location _newLocation { get; set; }

        public GamePage()
        {
            InitializeComponent();
        }

        private async void QuitButton_Clicked(object sender, EventArgs e)
        {
            if (await DisplayAlert("Quit Game", "Are you sure you want to quit this current game?", "Yes", "No"))
            {
                await Navigation.PopToRootAsync();
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (!_isInitiated)
            {
                _isInitiated = true;
                _task = await Task.Factory.StartNew(async () =>
                {
                    do
                    {
                        System.Diagnostics.Debug.WriteLine($"task START before 5 sec");
                        GetCurrentLocation();
                        await Task.Delay(5000);
                        System.Diagnostics.Debug.WriteLine($"task END after 5 sec");
                    } while (!_shutDown);
                });
            }
        }

        void GetCurrentLocation()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    bool hasPermission = await CheckPermissions();
                    if (!hasPermission)
                        return;
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                System.Diagnostics.Debug.WriteLine($"GetCurrentLocation method");
                try
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(5));
                        cts = new CancellationTokenSource();
                        var location = await Geolocation.GetLocationAsync(request, cts.Token);

                        if (location != null)
                        {
                            App.LastLocation = location;

                            //get game criminal and police location
                            var game = await FirebaseService.Get(App.CurrentGame);
                            var targetLocation = App.OnTheRun ? new Location(game.PoliceLatitude, game.PoliceLongitude) : new Location(game.CriminalLatitude, game.CriminalLongitude);
                            double distance = Location.CalculateDistance(App.LastLocation, targetLocation, DistanceUnits.Kilometers);

                            //show distance in meter on screen
                            distanceLabel.Text = $"{(int)(distance * 1000)}m.";

                            //save last location to game
                            await FirebaseService.UpdateLocation(App.LastLocation, game);
                        }
                    });
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"EXCEPTION! {ex.Message}");
                }
            });
        }

        private async Task<bool> CheckPermissions()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }

            if (status != PermissionStatus.Granted)
            {
                await App.Current.MainPage.DisplayAlert("Oops!", "To use this app, go to settings and give this app full-blown location-always permision.", "OK");
                return false;
            }

            return true;
        }

        protected override async void OnDisappearing()
        {
            _task = null;
            _shutDown = true;
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();

            await Task.Delay(500); //wait for cancelation

            base.OnDisappearing();
        }
    }
}