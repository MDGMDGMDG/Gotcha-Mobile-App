using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Gotcha_Mobile_App
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void Create_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreatePage());
        }

        private async void Join_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new JoinPage());
        }
    }
}
