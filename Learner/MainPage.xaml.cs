using System;
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
        public MainPage()
        {
            InitializeComponent();
        }

        async void OnWordAddClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EntryPage());
        }

        async void OnCollectionViewSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (Word)collectionView.SelectedItem;
            if (collectionView.SelectedItem != null) await Navigation.PushAsync(new EntryPage(selectedItem));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            collectionView.ItemsSource = App._words;
            collectionView.SelectedItem = null;
        }
    }
}