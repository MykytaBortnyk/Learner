using System;
using System.Collections.Generic;
using Learner.Services;
using Learner.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Learner
{
    public partial class AuthPage : ContentPage
    {
        private readonly IdentityService identity;
        //TODO: нужно больше инфы о юзере, залогинен он или нет, с сервака взять какие-то рофлы и закатать их в клиент
        public AuthPage()
        {
            InitializeComponent();
            identity = DependencyService.Get<IdentityService>();
        }

        async void Register(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
                return;

            var vm = new SignUpViewModel();
            vm.Email = mail.Text;
            vm.UserName = name.Text;
            vm.Password = password.Text;
            vm.ConfirmPassword = passwordC.Text;
            vm.StayLoggedIn = checkBox.IsChecked;

            if (await identity.SignUp(vm))
            {
                await DisplayAlert("", "Success!", "Ok");

                await Navigation.PopAsync();
            }
            else await DisplayAlert("", "Try again!", "Ok");
        }

        async void OnCancelButtonClicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
