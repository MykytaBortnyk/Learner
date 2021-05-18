using System;
using System.Collections.Generic;
using Learner.ViewModels;
using Xamarin.Forms;

namespace Learner
{
    public partial class FlyoutMenuPage : ContentPage
    {
        public FlyoutMenuPage()
        {
            InitializeComponent();

            var masterPageItems = new List<FlyoutPageItem>();
            masterPageItems.Add(new FlyoutPageItem
            {
                Title = "Dictionary",
                IconSource = "baseline_book_black_18dp.png",
                TargetType = typeof(MainPage)
            });
            masterPageItems.Add(new FlyoutPageItem
            {
                Title = "Quiz",
                IconSource = "baseline_school_black_18dp.png",
                TargetType = typeof(QuizPage)
            });
            masterPageItems.Add(new FlyoutPageItem
            {
                Title = "Collections",
                IconSource = "baseline_style_black_18dp.png",
                TargetType = typeof(CollectionsPage)
            });
            //masterPageItems.Add(new MasterPageItem
            //{
            //    Title = "Settings",
            //    TargetType = typeof(SettingsPage)
            //});

            MenuItemsListView.ItemsSource = masterPageItems;

            IconImageSource = "baseline_menu_white_18dp.png";
            Title = "Learner";
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new AuthPage());
        }
    }
}
