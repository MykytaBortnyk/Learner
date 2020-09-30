using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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

            _words = CollectionWordViewModel.CastCollection(App._words.OrderBy(t => t.Text));

            colView.ItemsSource = _words;

            InitializeCheckboxes();
        }

        /*
         * Короче, Меченый, я тебя спас и в благородство играть не буду: выполнишь для меня пару заданий — и мы в расчете. 
         * Заодно посмотрим, как быстро у тебя башка после амнезии прояснится. А по твоей теме постараюсь разузнать. 
         * Хрен его знает, на кой ляд тебе этот Чеклист сдался, но я в чужие дела не лезу, хочешь сделать, значит есть зачем...
         */

        public CollectionEntryPage(Collection collection)
        {
            InitializeComponent();

            Title = "Collection editing";

            //works slow!
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

            _words.AddRange(CollectionWordViewModel.CastCollection(App._words.Except(collection.Words)));

            colView.ItemsSource = _words.OrderBy(t => t.Item.Text);

            InitializeCheckboxes();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!isEditing)
                collectionName.Focus();
        }

        void InitializeCheckboxes()
        {
            colView.ItemTemplate = new DataTemplate(() =>
            {
                Grid grid = new Grid();
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(10, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(45, GridUnitType.Star) });
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(45, GridUnitType.Star) });

                var c = new CheckBox { ClassId = "", Color = Color.FromHex("#1976D2") };
                c.CheckedChanged += CheckBox_CheckedChanged;
                c.SetBinding(CheckBox.IsCheckedProperty, "IsSelected");
                c.SetBinding(CheckBox.ClassIdProperty, "Item.Id");
                grid.Children.Add(c);

                var l = new Label { VerticalOptions = LayoutOptions.Center};
                var l1 = new Label { VerticalOptions = LayoutOptions.Center };

                l.SetBinding(Label.TextProperty, "Item.Text");
                grid.Children.Add(l, 1, 0);

                l1.SetBinding(Label.TextProperty, "Item.Translation");
                grid.Children.Add(l1, 2, 0);

                return grid;
            });
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
                };
            }


            _collection.Name = collectionName.Text;
            _collection.Language = picker1.SelectedItem.ToString();
            _collection.Words = CollectionWordViewModel.CastCollection(_words.Where(x => x.IsSelected));


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
            var result = await DisplayAlert("Delete this item?", "This is permanent and cannot be undone.", "Delete", "Cancel");

            if (!result)
                return;

            App.Context.Collections.Remove(_collection);

            await App.Context.SaveChangesAsync();

            await Navigation.PopAsync();
        }

        async void OnCancelButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        void CheckBox_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var id = (sender as CheckBox).ClassId;

            var result = _words.FirstOrDefault(x => x.Item.Id.ToString() == id);

            if (result != null)
                result.IsSelected = (sender as CheckBox).IsChecked;
        }
    }
}