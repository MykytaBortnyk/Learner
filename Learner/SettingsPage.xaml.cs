using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Essentials;
using Learner.Models;

namespace Learner
{
    public partial class SettingsPage : ContentPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            EthStatus.Text = Connectivity.NetworkAccess == NetworkAccess.Internet ? "Eth is Enable" : "Eth is Disable";
            if (Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                var rest = new Learner.Services.RestService<Word>("word");
                var result = await rest.GetAsync();
                if (result != null)
                {
                    sync.Text = "Sync is Enable";
                }
                else
                {
                    sync.Text = "Sync is Disable";
                }
            }
        }

        async void Button_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AuthPage());
        }
    }
}
