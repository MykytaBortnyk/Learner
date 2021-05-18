using System;
using Learner.ViewModels;
using Xamarin.Forms;

namespace Learner
{
    public class MDPage : FlyoutPage
    {
        FlyoutMenuPage flyoutPage;

        public MDPage()
        {
            flyoutPage = new FlyoutMenuPage();
            Flyout = flyoutPage;
            Detail = new NavigationPage(new MainPage());

            flyoutPage.MenuItemsListView.ItemSelected += OnItemSelected;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as FlyoutPageItem;
            if (item != null)
            {
                if (item.Title == "Quiz" && App._words.Count < 1)
                    await DisplayAlert("", "Add more than 10 words to activate the quiz!", "Ok"); //who stole the toast?
                else
                    Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));

                flyoutPage.MenuItemsListView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}

