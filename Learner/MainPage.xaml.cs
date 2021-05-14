using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Learner.Models;
using Xamarin.Forms;

namespace Learner
{
    public partial class MainPage : ContentPage
    {
        private List<Word> source;

        public MainPage()
        {
            InitializeComponent();

            source = App._words;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            source = App._words.OrderBy(c => c.Text).ToList();

            collectionView.ItemsSource = source;

            if (source.Count == 0)
                label.IsVisible = true;
            else
                label.IsVisible = false;

            collectionView.SelectedItem = null;
            searchBar.Text = string.Empty;
        }

        async void OnWordAddClicked(object sender, EventArgs e) => await Navigation.PushAsync(new EntryPage());

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
                    collectionView.ItemsSource = source.OrderBy(x => x.Text);
                    break;

                case "Word (Z-A)":
                    collectionView.ItemsSource = source.OrderByDescending(x => x.Text);
                    break;

                case "Transcription (A-Z)":
                    collectionView.ItemsSource = source.OrderBy(x => x.Transcription);
                    break;

                case "Transcription (Z-A)":
                    collectionView.ItemsSource = source.OrderByDescending(x => x.Transcription);
                    break;

                case "Translation (A-Z)":
                    collectionView.ItemsSource = source.OrderBy(x => x.Translation);
                    break;

                case "Translation (Z-A)":
                    collectionView.ItemsSource = source.OrderByDescending(x => x.Translation);
                    break;

                case "Language (A-Z)":
                    collectionView.ItemsSource = source.OrderBy(x => x.Language).ThenBy(x => x.Text);
                    break;

                case "Language (Z-A)":
                    collectionView.ItemsSource = source.OrderByDescending(x => x.Language).ThenBy(x => x.Text);
                    break;
#if DEBUG
                case "Id (A-Z)":
                    collectionView.ItemsSource = source.OrderBy(x => x.Id);
                    break;

                case "Id (Z-A)":
                    collectionView.ItemsSource = source.OrderByDescending(x => x.Id).ThenBy(x => x.Text);
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
                source.Where(x => x.Text.ToLower().Contains(key.ToLower())
                    || x.Transcription != null && x.Transcription.ToLower().Contains(key.ToLower())
                    || x.Translation.ToLower().Contains(key.ToLower()));

            collectionView.ItemsSource = suggestions.OrderBy(x => x.Text);
        }
    }
}