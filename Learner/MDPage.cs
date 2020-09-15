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

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
                masterPage.MenuItemsListView.SelectedItem = null;
                IsPresented = false;
            }
        }
    }
}

