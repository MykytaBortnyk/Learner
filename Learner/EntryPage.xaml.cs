using System;
using System.Linq;
using System.Collections.Generic;
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
            this.ToolbarItems.Add(item);
        }

        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            if (wordText.Text == null || translation.Text == null)
            {
                await DisplayAlert("Alert", "Word may not be empty!", "Ok");
                return;
            }

            var word = new Word
            {
                Id = isEditing == false ? Guid.NewGuid() : _word.Id,
                Text = wordText.Text,
                Transcription = transcription.Text,
                Translation = translation.Text
            };

            using var db = new Infrastruction.ApplicationContext(App._dbPath);

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
    }
}
