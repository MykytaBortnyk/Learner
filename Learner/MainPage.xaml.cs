using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Learner.Models;
using Microsoft.EntityFrameworkCore;
using Xamarin.Forms;

namespace Learner
{
    public partial class MainPage : ContentPage
    {
        Guid collectionId;

        private readonly List<Word> source;

        public MainPage()
        {
            InitializeComponent();

            if (App._words.Count == 0)
                label.IsVisible = true;
            else
                label.IsVisible = false;

            source = App._words.OrderBy(c => c.Text).ToList();

            collectionView.ItemsSource = source;

#if DEBUG
            ToolbarItem item = new ToolbarItem { Text = "Remove Db", Order = ToolbarItemOrder.Secondary };
            this.ToolbarItems.Add(item);
            item.Clicked += OnRemoveDbClicked;
#endif
        }

        public MainPage(Collection collection)
        {
            InitializeComponent();

            source = collection.Words.ToList();

            if (collection.Words.Count == 0)
                label.IsVisible = true;
            else
                label.IsVisible = false;

            collectionView.ItemsSource = source;

            collectionId = collection.Id;

            Title = collection.Name;

            toolbarItem.Clicked += async (sender, e) =>
            {
                await Navigation.PushAsync(new CollectionEntryPage(collection));
            };

            toolbarItem.Clicked -= OnWordAddClicked;

            toolbarItem.Text = "Edit collection";
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

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
                source.Where(x => x.Text.ToLower().Contains(key.ToLower()) //this part is only needed for collId == null when we don't looking for words from some collection
                    || x.Transcription != null && x.Transcription.ToLower().Contains(key.ToLower()) //same as for collection words
                    || x.Translation.ToLower().Contains(key.ToLower()));

            collectionView.ItemsSource = suggestions.OrderBy(x => x.Text);
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
    }
}