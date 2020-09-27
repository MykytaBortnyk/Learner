using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Learner.Models;
using Learner.ViewModels;
using Xamarin.Forms;

namespace Learner
{
    //https://media.indiedb.com/images/members/4/3824/3823973/profile/bycicle_of_crutches.jpg
    public partial class CollectionEntryPage : ContentPage
    {
        private Collection _collection;

        private bool isEditing = false;

        private List<CollectionWordViewModel> _words = new List<CollectionWordViewModel>();

        public CollectionEntryPage()
        {
            InitializeComponent();

            Title = "Collection adding";

            _words = CastCollection(App._words);

            _words[0].IsSelected = true;

            colView.ItemsSource = _words;
        }

        public CollectionEntryPage(Collection collection)
        {
            InitializeComponent();

            Title = "Collection editing";

            _collection = App.Context.Collections.FirstOrDefault(x => x.Id == collection.Id); //look at param source

            isEditing = true;

            var item = new ToolbarItem { Text = "🗑" };

            item.Clicked += OnDeleteClicked;


            ToolbarItems.Add(item);

            picker1.SelectedItem = picker1.Items.FirstOrDefault(
                x => x == collection.Language);

            collectionName.Text = collection.Name;

            foreach (var i in collection.Words)
            {
                _words.Add(new CollectionWordViewModel { IsSelected = true, Item = i });
            }

            _words.AddRange(CastCollection(App._words.Except(collection.Words).ToList()));

            colView.ItemsSource = _words;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //collectionName.Focus();
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            if (collectionName.Text == null || picker1.SelectedIndex == -1) //TODO: collection size checking //ok, which one? 
            {
                await DisplayAlert("Alert!", "Fill all the fields!", "Ok");
                return;
            }

            if (_collection == null)
            {
                _collection = new Collection
                {
                    Id = Guid.NewGuid(),
                    Name = collectionName.Text,
                    Language = picker1.SelectedItem.ToString()
                };
            }

            _collection.Words = CastCollection(_words.Where(x => x.IsSelected).ToList());


            if (isEditing)
            {
                App.Context.Collections.Update(_collection);
            }
            else
            {
                App.Context.Add(_collection);
            }

            await App.Context.SaveChangesAsync();

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

        void LabelCheckBox_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var checkBox = sender as CheckBox;

            /*var item = App._words.FirstOrDefault(x => x.Text == checkBox.Text);
            colView.SelectedItem = 
            if (checkBox.IsChecked && item != null && _collection.Words.Any(x => x.Text == checkBox.Text))
            */
        }

        void colView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var item = sender as CheckBox;
        }

        List<CollectionWordViewModel> CastCollection(List<Word> words)
        {
            var list = new List<CollectionWordViewModel>();

            foreach (var word in words)
            {
                list.Add(
                    new CollectionWordViewModel { Item = word });
            }

            return list;
        }

        List<Word> CastCollection(List<CollectionWordViewModel> words)
        {
            var list = new List<Word>();

            foreach (var word in words)
            {
                list.Add(word.Item);
            }

            return list;
        }
    }


    public class CollectionWordViewModel : INotifyPropertyChanged
    {
        public Word Item { get; set; }

        public bool IsSelected { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
