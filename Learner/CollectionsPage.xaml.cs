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
            collectionView.ItemsSource = App._collections;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (App._collections.Count == 0)
                label.IsVisible = true;
            else
                label.IsVisible = false;

            collectionView.SelectedItem = null;
            searchBar.Text = string.Empty;
        }

        async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (Collection)collectionView.SelectedItem;

            if (selectedItem != null) await Navigation.PushAsync(new CollectionMainPage(selectedItem));
            searchBar.Text = string.Empty;
        }

        async void OnCollectionAddClicked(object sender, EventArgs e) => await Navigation.PushAsync(new CollectionEntryPage());

        async void OnSortClicked(object sender, EventArgs e)
        {
            searchBar.Text = string.Empty;

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

        void searchBar_TextChanged(object sender, System.EventArgs e)
        {
            var key = searchBar.Text;

            var suggestions = App._collections
                .Where(x => x.Name.ToLower()
                .Contains(key.ToLower()));

            collectionView.ItemsSource = suggestions.OrderBy(x => x.Name);
        }
    }
}
