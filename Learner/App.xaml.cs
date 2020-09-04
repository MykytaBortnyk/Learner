using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Learner.Infrastruction;
using Learner.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Learner
{
    public partial class App : Application
    {
        public static String _dbPath;

        public static List<Word> _words;

        public App() //shit code bellow, it's should be in some fabric, hard IO in ctor is bad practice, but i'll fix this later 
        {
            InitializeComponent();

            _dbPath = Path.Combine(Environment.GetFolderPath(
                        Environment.SpecialFolder.LocalApplicationData), "Words.db3");

            using var db = new ApplicationContext(_dbPath);

#if DEBUG
            File.Delete(_dbPath);

            Console.WriteLine(File.Exists(_dbPath));
#endif
            // Ensure database is created
            db.Database.EnsureCreated();
//#if DEBUG

            if (!db.Words.Any())
            {
                // Insert Data
                db.AddRange(addWords());

                db.Words.OrderBy(x => x.Text);

                db.SaveChanges();
            }
            // Retreive Data
//#endif
            _words = db.Words.OrderBy(x => x.Text).ToList();

            MainPage = new NavigationPage(new MainPage());
        }


        List<Word> addWords()
        {
            var list = new List<Word>();

            list.Add(new Word() { Id = Guid.NewGuid(), Text = "ありがとう", Transcription = "arigato", Translation = "Thank you" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "こんにちは", Transcription = "konnichiwa", Translation = "Hello" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "おはよう", Transcription = "ohayou", Translation = "Good morning" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "おやすみなさい", Transcription = "oyasuminasai", Translation = "Good night" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "水", Transcription = "みず", Translation = "Water" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "犬", Transcription = "いぬ", Translation = "Dog" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "よろしく", Transcription = "yoroshku", Translation = "Nice to meet you" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "雲", Transcription = "くも", Translation = "Cloud" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "風", Transcription = "かぜ", Translation = "Wind" });

            list.Add(new Word() { Id = Guid.NewGuid(), Text = "一", Transcription = "いち", Translation = "One" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "二", Transcription = "に", Translation = "Two" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "三", Transcription = "さん", Translation = "Three" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "四", Transcription = "よん", Translation = "Four" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "五", Transcription = "ご", Translation = "Five" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "六", Transcription = "ろく", Translation = "Six" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "七", Transcription = "なな", Translation = "Seven" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "八", Transcription = "はち", Translation = "Eight" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "九", Transcription = "きゅう", Translation = "Nine" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "十", Transcription = "じゅう", Translation = "Ten" });

            list.Add(new Word() { Id = Guid.NewGuid(), Text = "人", Transcription = "じん", Translation = "Man" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "口", Transcription = "くち", Translation = "Mouth" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "語", Transcription = "ご", Translation = "Word" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "体", Transcription = "からだ", Translation = "Body" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "心", Transcription = "こころ", Translation = "Heart" });
            //list.Add(new Word() { Id = Guid.NewGuid(), Text = "", Transcription = "", Translation = "" });

            return list;
        }
    }
}
