using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Learner.Models;
using Xamarin.Forms;

namespace Learner
{
    public partial class QuizPage : ContentPage
    {
        Guid rightButtonId;

        int scores = 0;

        String hint;

        public QuizPage()
        {
            InitializeComponent();

            button1.Clicked += SomeButtonClicked;
            button2.Clicked += SomeButtonClicked;
            button3.Clicked += SomeButtonClicked;
            button4.Clicked += SomeButtonClicked;

            StartQuiz();
        }

        void StartQuiz()
        {
            answersLabel.Text = scores.ToString();

            Button[] buttons = { button1, button2, button3, button4 };

            Random rnd = new Random();
            List<Word> words = new List<Word>();

            //while instead normal roll
            while (words.Count < 4)
            {
                var temp = App._words[rnd.Next(App._words.Count)];

                if (!words.Contains(temp))
                {
                    buttons[words.Count].Text = temp.Translation;
                    words.Add(temp);
                }
            }

            var luckyButton = rnd.Next(buttons.Length);
            rightButtonId = buttons[luckyButton].Id;
            label1.Text = words[luckyButton].Text;
            hint = words[luckyButton].Transcription;
        }

        //cast sender to button and compare the Id property with Id of "right" button
        void SomeButtonClicked(object sender, EventArgs e)
        {
            if (((Button)sender).Id == rightButtonId)
            {
                label1.Text = "Success!";
                scores++;
            }
            else
                label1.Text = "Not success";

            StartQuiz();
        }

        async void OnHelpClicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("", "Use 10 scores to show hint?", "Use", "Cancel");
            if (answer)
            {
                if (scores > 10)
                {
                    scores -= 10;
                    await DisplayAlert("", hint, "Close");
                    answersLabel.Text = scores.ToString();
                }
                else
                    await DisplayAlert("", "Not enough scores!", "Close");
            }
        }
    }
}
