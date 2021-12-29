using Gotcha_Mobile_App.Model;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Gotcha_Mobile_App
{
    public partial class App : Application
    {
        public static Game CurrentGame { get; internal set; }
        public static string CurrentUserName { get; internal set; }
        public static bool OnTheRun { get; internal set; }
        public static Location LastLocation { get; internal set; }

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
