using System;
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

            var label = new Label { Text = "The word list is empty!", HorizontalOptions = LayoutOptions.Center, ClassId = "zeroWordsLabel" };

            if (App._collections == null || App._collections.Count == 0)
                stackLayout.Children.Insert(0, label);
            else
            {
                if (stackLayout.Children[0].ClassId == label.ClassId)
                {
                    stackLayout.Children.RemoveAt(0);
                }
            }

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
        }

        async void OnSearchClicked(object sender, EventArgs e)
        {
        }
    }
}
