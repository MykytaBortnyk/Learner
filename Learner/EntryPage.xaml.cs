using System;
using System.Linq;
using Learner.Models;
using Xamarin.Forms;

namespace Learner
{
    public partial class EntryPage : ContentPage
    {
        private Word _word;

        private bool isEditing = false;

        public EntryPage()
        {
            InitializeComponent();

            if (App._collections != null && App._collections.Count > 0)
            {
                var control = new Picker
                {
                    Title = "Collection",
                    ItemsSource = App._collections.Select(x => x.Name).ToList(),
                    ClassId = "picker2"
                };
                stackLayout.Children.Insert(4, control);
            }
        }

        public EntryPage(Word word)
        {
            InitializeComponent();

            _word = word;
            wordText.Text = word.Text;
            transcription.Text = word.Transcription;
            translation.Text = word.Translation;
            isEditing = true;
            var item = new ToolbarItem { Text = "🗑" };
            item.Clicked += OnDeleteClicked;
            ToolbarItems.Add(item);
            picker1.SelectedItem = picker1.Items.FirstOrDefault(x => x == word.Language);
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            if (wordText.Text == null || translation.Text == null || picker1.SelectedIndex == -1)
            {
                await DisplayAlert("Alert!", "Fill all the fields!", "Ok");
                return;
            }


            //looks strange 
            var word = new Word
            {
                Id = isEditing == false ? Guid.NewGuid() : _word.Id,
                Text = wordText.Text,
                Transcription = transcription.Text,
                Translation = translation.Text,
                Language = picker1.SelectedItem.ToString()
            };

            /*if (!isEditing)
            {

            }*/

            using var db = new Infrastruction.ApplicationContext(App._dbPath);

            var picker = stackLayout.Children.FirstOrDefault(x => x.ClassId == "picker2") as Picker;
            if (picker.SelectedIndex != -1)
            {
                var col = App._collections.FirstOrDefault(x =>
                    x.Name == picker.ItemsSource[picker.SelectedIndex].ToString());

                col.Words.Add(word);
                var result = db.Collections.Update(col);
            }

            if (isEditing)
            {
                db.Words.Update(word);
            }
            else
            {
                db.Add(word);
            }

            await db.SaveChangesAsync();

            await Navigation.PopAsync();
        }

        async void OnCancelButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        async void OnDeleteClicked(object sender, EventArgs e)
        {
            using var db = new Infrastruction.ApplicationContext(App._dbPath);

            db.Words.Remove(_word);

            await db.SaveChangesAsync();

            await Navigation.PopAsync();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            wordText.Focus();
        }
    }
}
