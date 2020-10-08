using System;
using System.Linq;
using Learner.Models;
using Microsoft.EntityFrameworkCore;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Learner
{
    public partial class EntryPage : ContentPage
    {
        private Word _word;

        private bool isEditing;

        public EntryPage()
        {
            InitializeComponent();

            Title = "Word adding";

            if (App._collections == null && App._collections.Count <= 0)
            {
                return;
            }
            Picker control = new Picker
            {
                Title = "Collection",
                ItemsSource = App._collections.Select(x => x.Name).ToList(),
                ClassId = "picker2"
            };
            stackLayout.Children.Insert(4, control);
        }

        public EntryPage(Word word)
        {
            InitializeComponent();

            Title = "Word editing";

            _word = word;
            wordText.Text = word.Text;
            transcription.Text = word.Transcription;
            translation.Text = word.Translation;
            isEditing = true;
            var item = new ToolbarItem { Text = "🗑" };
            item.Clicked += OnDeleteClicked;
            ToolbarItems.Add(item);
            var item1 = new ToolbarItem { IconImageSource = "baseline_music_note_white_18dp.png" };
            item1.Clicked += OnSpeachClicked;
            ToolbarItems.Add(item1);
            picker1.SelectedItem = picker1.Items.FirstOrDefault(x => x == word.Language);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (isEditing)
                return;
            wordText.Focus();
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            if (wordText.Text == null || translation.Text == null || picker1.SelectedIndex == -1)
            {
                await DisplayAlert("Alert!", "Fill all the fields!", "Ok");
                return;
            }

            var picker = stackLayout.Children.FirstOrDefault(x => x.ClassId == "picker2") as Picker;

            if (_word == null)
                _word = new Word()
                {
                    Id = Guid.NewGuid(),
                };

            _word.Text = wordText.Text;
            _word.Transcription = transcription.Text;
            _word.Translation = translation.Text;
            _word.Language = picker1.SelectedItem.ToString();

            if (isEditing)
            {
                App.Context.Words.Update(_word);
            }
            else
            {
                App.Context.Add(_word);
            }

            if (picker != null && picker.SelectedIndex != -1)
            {
                Collection collection = await App.Context.Collections.FirstOrDefaultAsync(x =>
                    x.Name == picker.ItemsSource[picker.SelectedIndex].ToString());

                if (collection != null)
                {
                    collection.Words.Add(_word);

                    App.Context.Collections.Update(collection);
                }
            }

            await App.Context.SaveChangesAsync();

            await Navigation.PopAsync();
        }

        async void OnCancelButtonClicked(object sender, EventArgs e) => await Navigation.PopAsync();

        async void OnDeleteClicked(object sender, EventArgs e)
        {
            var result = await DisplayAlert("Delete this item?", "This is permanent and cannot be undone.", "Delete", "Cancel");

            if (!result)
                return;

            App.Context.Words.Remove(_word);

            await App.Context.SaveChangesAsync();

            await Navigation.PopAsync();
        }

        async void OnSpeachClicked(object sender, EventArgs e)
        {
            var locales = await TextToSpeech.GetLocalesAsync();

            // Grab the first locale
            var locale = locales.FirstOrDefault(x => x.Name.ToLower().Contains(_word.Language.ToLower()));

            var settings = new SpeechOptions
            {
                Volume = 1
            };

            if (locale != null)
                settings.Locale = locale;

            await TextToSpeech.SpeakAsync(_word.Text, settings);
        }
    }
}
