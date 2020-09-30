using System;
using Learner.ViewModels;
using Xamarin.Forms;

namespace Learner
{
    public class MDPage : MasterDetailPage
    {
        MasterPageXAML masterPage;

        public MDPage()
        {
            masterPage = new MasterPageXAML();
            Master = masterPage;
            Detail = new NavigationPage(new MainPage());

            masterPage.MenuItemsListView.ItemSelected += OnItemSelected;

            if (Device.RuntimePlatform == Device.UWP)
            {
                MasterBehavior = MasterBehavior.Popover;
            }
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                if (item.Title == "Quiz" && App._words.Count < 1)
                    await DisplayAlert("", "Add more than 10 words to activate the quiz!", "Ok"); //who stole the toast?
                else
                    Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));

                masterPage.MenuItemsListView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}

