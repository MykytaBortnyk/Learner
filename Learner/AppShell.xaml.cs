using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Learner
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //Routing.RegisterRoute(nameof(), typeof());
        }

        protected override async void OnNavigated(ShellNavigatedEventArgs args)
        {
            if (args.Current.Location.OriginalString == "//Quiz")
                if(App._words.Count > 1)
                    await Shell.Current.GoToAsync("//QuizPage");
                else
                    await DisplayAlert("", "Add more than 10 words to activate the quiz!", "Ok"); //who stole the toast?                
        }
    }
}
