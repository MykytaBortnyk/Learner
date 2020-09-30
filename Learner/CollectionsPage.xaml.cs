using System;
using System.Linq;
using Learner.Models;
using Xamarin.Forms;

namespace Learner
{
    public partial class CollectionsPage : ContentPage
    {
        public CollectionsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            collectionView.ItemsSource = App._collections;

            if (App._collections.Count == 0)
                label.IsVisible = true;
            else
                label.IsVisible = false;

            collectionView.SelectedItem = null;
        }

        async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (Collection)collectionView.SelectedItem;
            if (collectionView.SelectedItem != null) await Navigation.PushAsync(new CollectionEntryPage(selectedItem));
        }

        async void OnCollectionAddClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CollectionEntryPage());
        }

        async void OnSortClicked(object sender, EventArgs e)
        {
            var result = await DisplayActionSheet(
                "Sort by:", "Cancel", null,
                "Name (A-Z)", "Name (Z-A)",
#if DEBUG
                "Id (A-Z)", "Id (Z-A)",
#endif
                "Language (A-Z)", "Language (Z-A)"
);

            switch (result)
            {
                default:
                    collectionView.ItemsSource = App._collections.OrderBy(x => x.Name);
                    break;

                case "Name (Z-A)":
                    collectionView.ItemsSource = App._collections.OrderByDescending(x => x.Name);
                    break;

                case "Language (A-Z)":
                    collectionView.ItemsSource = App._collections.OrderBy(x => x.Language).ThenBy(x => x.Name);
                    break;

                case "Language (Z-A)":
                    collectionView.ItemsSource = App._collections.OrderByDescending(x => x.Language).ThenBy(x => x.Name);
                    break;
#if DEBUG
                case "Id (A-Z)":
                    collectionView.ItemsSource = App._collections.OrderBy(x => x.Id);
                    break;

                case "Id (Z-A)":
                    collectionView.ItemsSource = App._collections.OrderByDescending(x => x.Id);
                    break;
#endif
            }
        }

        async void OnSearchClicked(object sender, EventArgs e)
        {
            var result = await DisplayPromptAsync("Search", "Type the word to search", "Find", "Cancel", keyboard: Keyboard.Default);

            if (result == null)
                return;

            if (result == string.Empty)
            {
                await DisplayAlert("Alert!", "Word may not be empty!", "Ok");
                return;
            }

            result = result.ToLower();

            var collection = App._collections.Find(c => c.Name.ToLower().Contains(result)); //App._collections.FirstOrDefault(x => x.Name.Contains(result));

            //not sure about this
            collection ??= App._collections.Find(x => x.Language.ToLower().Contains(result));

            if (collection == null)
            {
                await DisplayAlert("Alert!", "Collection not found!", "Ok");
                return;
            }

            /*
            There should be animation here
            Alert is temporary solution
            */

            await DisplayAlert("", "Found!", "Ok");
            collectionView.ScrollTo(collection);
        }
    }
}
