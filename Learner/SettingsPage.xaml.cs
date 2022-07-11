using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Essentials;
using Learner.Models;
using Learner.Services;

namespace Learner
{
    public partial class SettingsPage : ContentPage
    {
        RestService<Word> rest;

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
                rest = DependencyService.Get<RestService<Word>>();
                App.IsAuthenticated = await DependencyService.Get<IdentityService>().IsAuthenticated();
                AuthStatus.Text = $"Is authenticated = {App.IsAuthenticated}";
                //TODO:в проверке на аут парс вернёт нулл если сервер не вернёт статус = лок синка
                //заебись идея, но нет варианта вернуть так статус серверачерез бул
                sync.Text = await rest.GetAsync() != null ? "Sync is Enable" : "Sync is Disable";
                register.IsVisible = App.IsAuthenticated ? false : true;
                logIn.IsVisible = App.IsAuthenticated ? false : true;
                logOut.IsVisible = App.IsAuthenticated ? true : false;
            }
        }

        async void Button_Clicked(object sender, EventArgs e)
        {
            if(Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("", "No internet connection!", "Ok");
                return;
            }
            await Navigation.PushAsync(new AuthPage(true));
        }

        async void Button1_Clicked(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("", "No internet connection!", "Ok");
                return;
            }
            await Navigation.PushAsync(new AuthPage(false));
        }

        async void OnSyncClicked(object sender, EventArgs e)
        {
            await rest.PutMany(App._words);
        }

        async void OnLogOutClicked(System.Object sender, System.EventArgs e)
        {
            await DependencyService.Get<IdentityService>().LogOut();
        }
    }
}
