using System;
using System.Collections.Generic;
using System.Linq;
using Learner.Models;
using Xamarin.Forms;

namespace Learner
{
    public partial class SearchPage : ContentPage
    {
        public SearchPage()
        {
            InitializeComponent();
        }

        void searchBar_TextChanged(System.Object sender, System.EventArgs e)
        {
            var key = searchBar.Text;

            var suggestions = App._words.Where(x => x.Text.ToLower().Contains(key.ToLower()));

            сollectionView.ItemsSource = suggestions;
        }

        async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (Word)сollectionView.SelectedItem;

            сollectionView.SelectedItem = null;

            if (selectedItem != null) await Navigation.PushAsync(new EntryPage(selectedItem));
        }
    }
}
