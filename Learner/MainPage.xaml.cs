﻿using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Learner.Infrastruction;
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
#if DEBUG
            ToolbarItem item = new ToolbarItem { Text = "❌" };
            this.ToolbarItems.Add(item);
            item.Clicked += OnRemoveDbClicked;
#endif
        }

        async void OnWordAddClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EntryPage());
        }

        void OnSortClicked(object sender, EventArgs e)
        {
            if (SortByText)
            {
                SortByText = !SortByText;
                collectionView.ItemsSource = App._words.OrderBy(x => x.Translation);
            }
            else
            {
                SortByText = !SortByText;
                collectionView.ItemsSource = App._words.OrderBy(x => x.Text);
            }
        }

        async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (Word)collectionView.SelectedItem;
            if (collectionView.SelectedItem != null) await Navigation.PushAsync(new EntryPage(selectedItem));
        }

        async void ToolbarItemClicked(object sender, EventArgs e)
        {
            if (App._words.Count < 1)
            {
                await DisplayAlert("", "Add more than 10 words to activate the quiz!", "Ok"); //who stole the toast?
                return;
            }
            await Navigation.PushAsync(new QuizPage());
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
        protected override void OnAppearing()
        {
            base.OnAppearing();
            collectionView.ItemsSource = App._words;
            collectionView.SelectedItem = null;
        }
    }
}