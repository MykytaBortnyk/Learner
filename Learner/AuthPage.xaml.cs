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
        public AuthPage(bool isRegister)
        {
            InitializeComponent();
            if (!isRegister)
            {
                name.IsVisible = false;
                passwordC.IsVisible = false;
                button1.Clicked -= Register;
                button1.Clicked += LogIn;
                button1.Text = "Log In";
            }

            identity = DependencyService.Get<IdentityService>();
        }

        async void Register(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("", "No internet connection!", "Ok");
                return;
            }

            if (string.IsNullOrWhiteSpace(name.Text) ||
                string.IsNullOrWhiteSpace(mail.Text) ||
                string.IsNullOrWhiteSpace(password.Text) ||
                string.IsNullOrWhiteSpace(passwordC.Text))
            {
                await DisplayAlert("", "Fill all the fields!", "Ok");
                return;
            }

            if (!password.Text.Equals(passwordC.Text))
            {
                await DisplayAlert("", "Passwords doesn't match!", "Ok");
                return;
            }
            var vm = new SignUpViewModel();
            vm.Email = mail.Text;
            vm.UserName = name.Text;
            vm.Password = password.Text;
            vm.ConfirmPassword = passwordC.Text;
            vm.StayLoggedIn = checkBox1.IsChecked;

            if (await identity.SignUp(vm))
            {
                await DisplayAlert("", "Success!", "Ok");

                await Navigation.PopAsync();
            }
            else await DisplayAlert("", "Try again!", "Ok");
        }

        async void LogIn (object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await DisplayAlert("", "No internet connection!", "Ok");
                return;
            }

            if (string.IsNullOrWhiteSpace(mail.Text) ||
                string.IsNullOrWhiteSpace(password.Text))
            {
                await DisplayAlert("", "Fill all the fields!", "Ok");
                return;
            }

            var vm = new SignInViewModel();
            vm.Email = mail.Text;
            vm.Password = password.Text;
            vm.RememberMe = checkBox1.IsChecked;

            if (await identity.SignIn(vm))
            {
                await DisplayAlert("", "Success!", "Ok");
                await Navigation.PopAsync();
            }
            else await DisplayAlert("", "Try again!", "Ok");
        }

        async void OnCancelButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
