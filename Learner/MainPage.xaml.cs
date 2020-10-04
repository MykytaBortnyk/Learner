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

        public MainPage()
        {
            InitializeComponent();
            toolbarItem.Clicked += OnWordAddClicked;

#if DEBUG
            ToolbarItem item = new ToolbarItem { Text = "Remove Db", Order = ToolbarItemOrder.Secondary };
            this.ToolbarItems.Add(item);
            item.Clicked += OnRemoveDbClicked;
#endif
        }

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

            toolbarItem.Text = "Edit collection";
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var coll = await App.Context.Collections.FirstOrDefaultAsync(c => c.Id == collectionId);

            if (coll == null)
            {
                if (App._words.Count == 0)
                    label.IsVisible = true;
                else
                    label.IsVisible = false;

                collectionView.ItemsSource = App._words.OrderBy(c => c.Text);
            }
            else
            {
                if (coll.Words.Count == 0)
                    label.IsVisible = true;
                else
                    label.IsVisible = false;

                collectionView.ItemsSource = coll.Words;
            }

            collectionView.SelectedItem = null;
        }

        async void OnWordAddClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EntryPage());
        }

        //rework this tomorrow 
        async void OnSortClicked(object sender, EventArgs e)
        {
            var result = await DisplayActionSheet(
                "Sort by:", "Cancel", null,
                "Word (A-Z)", "Word (Z-A)",
                "Transcription (A-Z)", "Transcription (Z-A)",
                "Translation (A-Z)", "Translation (Z-A)",
#if DEBUG
                "Id (A-Z)", "Id (Z-A)",
#endif
                "Language (A-Z)", "Language (Z-A)");

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
            if (collectionView.SelectedItem != null) await Navigation.PushAsync(new EntryPage(selectedItem));
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

        async void OnSearchClicked(object sender, EventArgs e)
        {
            /*
             * TODO:
             * 1) Add some anim to show finded item
             * 2) Extend search by adding more options to search
             * 3) I'm not sure about search, it wasn't tested
             */

            var result = await DisplayPromptAsync("Search", "Type the word to search", "Find", "Cancel", keyboard: Keyboard.Default);

            if (result == null)
                return;

            if (result == string.Empty)
            {
                await DisplayAlert("Alert!", "Word may not be empty!", "Ok");
                return;
            }
            result = result.ToLower();
            var word = App._words.Find(x => x.Text.ToLower().Contains(result));

            //not sure about this
            word ??= App._words.Find(x => x.Transcription.ToLower().Contains(result));
            word ??= App._words.Find(x => x.Translation.ToLower().Contains(result));

            if (word == null)
            {
                await DisplayAlert("Alert!", "Word not found!", "Ok");
                return;
            }

            /*
            There should be animation here
            Alert is temporary solution
            */

            await DisplayAlert("", "Found!", "Ok");
            collectionView.ScrollTo(word);
        }
    }
}