using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Learner.Models;
using Xamarin.Forms;

namespace Learner
{
    public partial class CollectionMainPage : ContentPage
    {
        private Collection source;

        public CollectionMainPage(Collection collection)
        {
            InitializeComponent();

            source = collection;

            //collectionView.ItemsSource = source.Words;

            Title = collection.Name;

            toolbarItem.Clicked += async (sender, e) =>
            {
                await Navigation.PushAsync(new CollectionEntryPage(collection));
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            collectionView.ItemsSource = App._collections.FirstOrDefault(c => c.Id == source.Id).Words;

            if (source.Words.Count == 0)
                label.IsVisible = true;
            else
                label.IsVisible = false;

            collectionView.SelectedItem = null;
            searchBar.Text = string.Empty;
        }

        async void OnWordAddClicked(object sender, EventArgs e) => await Navigation.PushAsync(new EntryPage());

        async Task RefreshItemsAsync()
        {
            await Task.Factory.StartNew(() =>
            {
                collectionView.ItemsSource = App._collections.FirstOrDefault(c => c.Id == source.Id).Words;
            });         
        }

        //отета мусор, потом на енам перенести, сравнение по строкам временный костыль
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
                    collectionView.ItemsSource = source.Words.OrderBy(x => x.Text);
                    break;

                case "Word (Z-A)":
                    collectionView.ItemsSource = source.Words.OrderByDescending(x => x.Text);
                    break;

                case "Transcription (A-Z)":
                    collectionView.ItemsSource = source.Words.OrderBy(x => x.Transcription);
                    break;

                case "Transcription (Z-A)":
                    collectionView.ItemsSource = source.Words.OrderByDescending(x => x.Transcription);
                    break;

                case "Translation (A-Z)":
                    collectionView.ItemsSource = source.Words.OrderBy(x => x.Translation);
                    break;

                case "Translation (Z-A)":
                    collectionView.ItemsSource = source.Words.OrderByDescending(x => x.Translation);
                    break;

                case "Language (A-Z)":
                    collectionView.ItemsSource = source.Words.OrderBy(x => x.Language).ThenBy(x => x.Text);
                    break;

                case "Language (Z-A)":
                    collectionView.ItemsSource = source.Words.OrderByDescending(x => x.Language).ThenBy(x => x.Text);
                    break;
#if DEBUG
                case "Id (A-Z)":
                    collectionView.ItemsSource = source.Words.OrderBy(x => x.Id);
                    break;

                case "Id (Z-A)":
                    collectionView.ItemsSource = source.Words.OrderByDescending(x => x.Id).ThenBy(x => x.Text);
                    break;
#endif
            }
        }

        async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Word selectedItem = (Word)collectionView.SelectedItem;
            if (selectedItem != null) await Navigation.PushAsync(new EntryPage(selectedItem));
            searchBar.Text = string.Empty;
        }

        void searchBar_TextChanged(object sender, EventArgs e)
        {
            var key = searchBar.Text;

            IEnumerable<Word> suggestions =
                source.Words.Where(x => x.Text.ToLower().Contains(key.ToLower())
                    || x.Transcription != null && x.Transcription.ToLower().Contains(key.ToLower())
                    || x.Translation.ToLower().Contains(key.ToLower()));

            collectionView.ItemsSource = suggestions.OrderBy(x => x.Text);
        }
    }
}
