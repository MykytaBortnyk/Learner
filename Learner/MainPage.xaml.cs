using System;
using System.IO;
using System.Linq;
using Learner.Models;
using Xamarin.Forms;

namespace Learner
{
    public partial class MainPage : ContentPage
    {
        bool SortByText = true;

        public MainPage()
        {
            InitializeComponent();
<<<<<<< Updated upstream

=======
>>>>>>> Stashed changes
#if DEBUG
            ToolbarItem item = new ToolbarItem { Text = "Remove Db", Order = ToolbarItemOrder.Secondary };
            this.ToolbarItems.Add(item);
            item.Clicked += OnRemoveDbClicked;
#endif
        }

<<<<<<< Updated upstream
        protected override void OnAppearing()
=======
        public MainPage(Collection collection)
        {
            InitializeComponent();

            collectionView.ItemsSource = collection.Words;

            collectionId = collection.Id;

            Title = collection.Name;

            toolbarItem.Clicked += async (sender, e) =>
            {
                await Navigation.PushAsync(new CollectionEntryPage(collection));
            };
            toolbarItem.Clicked -= OnWordAddClicked;

            toolbarItem.Text = "Edit collection";
        }

        protected override async void OnAppearing()
>>>>>>> Stashed changes
        {
            base.OnAppearing();

            if (App._words.Count == 0)
                label.IsVisible = true;
            else
                label.IsVisible = false;

            collectionView.ItemsSource = App._words.OrderBy(c => c.Text);

            collectionView.SelectedItem = null;
            searchBar.Text = string.Empty;
        }

        async void OnWordAddClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EntryPage());
        }

        //rework this tomorrow 
        async void OnSortClicked(object sender, EventArgs e)
        {
            searchBar.Text = string.Empty;

            var result = await DisplayActionSheet(
                "Sort by:", "Cancel", null,
                "Word (A-Z)", "Word (Z-A)",
                "Transcription (A-Z)", "Transcription (Z-A)",
                "Translation (A-Z)", "Translation (Z-A)",
#if DEBUG
                "Id (A-Z)", "Id (Z-A)",
#endif
                "Language (A-Z)", "Language (Z-A)"

);

            switch (result)
            {
                default:
                    collectionView.ItemsSource = App._words.OrderBy(x => x.Text);
                    break;

                case "Word (Z-A)":
                    collectionView.ItemsSource = App._words.OrderByDescending(x => x.Text);
                    break;

                case "Transcription (A-Z)":
                    collectionView.ItemsSource = App._words.OrderBy(x => x.Transcription);
                    break;

                case "Transcription (Z-A)":
                    collectionView.ItemsSource = App._words.OrderByDescending(x => x.Transcription);
                    break;

                case "Translation (A-Z)":
                    collectionView.ItemsSource = App._words.OrderBy(x => x.Translation);
                    break;

                case "Translation (Z-A)":
                    collectionView.ItemsSource = App._words.OrderByDescending(x => x.Translation);
                    break;

                case "Language (A-Z)":
                    collectionView.ItemsSource = App._words.OrderBy(x => x.Language).ThenBy(x => x.Text);
                    break;

                case "Language (Z-A)":
                    collectionView.ItemsSource = App._words.OrderByDescending(x => x.Language).ThenBy(x => x.Text);
                    break;
#if DEBUG
                case "Id (A-Z)":
                    collectionView.ItemsSource = App._words.OrderBy(x => x.Id);
                    break;

                case "Id (Z-A)":
                    collectionView.ItemsSource = App._words.OrderByDescending(x => x.Id).ThenBy(x => x.Text);
                    break;
#endif
            }
        }

        async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (Word)collectionView.SelectedItem;
            if (selectedItem != null) await Navigation.PushAsync(new EntryPage(selectedItem));
            searchBar.Text = string.Empty;
        }

#if DEBUG
        async void OnRemoveDbClicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert("", "Remove Db?", "Yes", "No");

            if (result && File.Exists(App._dbPath))
            {
                File.Delete(App._dbPath);
            }
        }
#endif

        void searchBar_TextChanged(object sender, EventArgs e)
        {
            var key = searchBar.Text;

            var suggestions =
                App._collections.FirstOrDefault(c => c.Id == collectionId)?
                .Words.Where(x => x.Text.ToLower().Contains(key.ToLower())) ??
                App._words.Where(x => x.Text.ToLower().Contains(key.ToLower()));

            collectionView.ItemsSource = suggestions.OrderBy(x => x.Text);
        }
    }
}