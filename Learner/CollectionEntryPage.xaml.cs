using System;
using System.Collections.Generic;
using Learner.Models;
using Xamarin.Forms;

namespace Learner
{
    public partial class CollectionEntryPage : ContentPage
    {
        private Collection _collection;

        private bool isEditing = false;

        public CollectionEntryPage()
        {
            InitializeComponent();
        }

        public CollectionEntryPage(Collection collection)
        {
            InitializeComponent();
        }
        
        protected override void OnAppearing()
        {
            base.OnAppearing();
            collectionName.Focus();
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            if (collectionName.Text == null || picker1.SelectedIndex == -1) //TODO: collection size checking
            {
                await DisplayAlert("Alert!", "Fill all the fields!", "Ok");
                return;
            }

            var collection = new Collection
            {
                Id = isEditing == false ? Guid.NewGuid() : _collection.Id,
                Name = collectionName.Text,
                /*Text = wordText.Text,
                Transcription = transcription.Text,
                Translation = translation.Text,*/
                Language = picker1.SelectedItem.ToString()
            };

            using var db = new Infrastruction.ApplicationContext(App._dbPath);

            if (isEditing)
            {
                db.Collections.Update(collection);
            }
            else
            {
                db.Add(collection);
            }

            await db.SaveChangesAsync();

            await Navigation.PopAsync();
        }

        async void OnDeleteClicked(object sender, EventArgs e)
        {
            using var db = new Infrastruction.ApplicationContext(App._dbPath);

            db.Collections.Remove(_collection);

            await db.SaveChangesAsync();

            await Navigation.PopAsync();
        }

        async void OnCancelButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}
