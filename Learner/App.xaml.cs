using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using Learner.Infrastruction;
using Learner.Models;
using Learner.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Learner
{
    public partial class App : Application
    {
        public static String _dbPath;

        public static int Scores;

        public static List<Word> _words = new List<Word>();

        public static List<Collection> _collections = new List<Collection>();

        public static readonly ApplicationContext Context;

        public static bool IsAuthenticated;

        public static readonly HttpClient httpClient;

        public static string Uri = "https://4e50689810eb.ngrok.io/api/";

        static App()
        {
            _dbPath = Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.LocalApplicationData), "Words.db3");

            httpClient = new HttpClient();

            Context = new ApplicationContext(_dbPath);
        }


        /// 
        /// TODO:TinyIoC
        /// 
        public App() //shit code bellow, it's should be in some fabric, hard IO in ctor is bad practice, but i'll fix this later 
        {
            InitializeComponent();
            DependencyService.Register<IdentityService>();
            DependencyService.Register<RestService<Word>>();
#if DEBUG
            File.Delete(_dbPath);
            Context.Database.EnsureDeleted();
            Console.WriteLine(File.Exists(_dbPath));
#endif
            // Ensure database is created
            Context.Database.EnsureCreated();
//#if DEBUG

            if (!Context.Words.Any())
            {
                // Insert Data
                Context.Words.AddRange(addWords());

                Context.SaveChanges();
            }
//#endif

            _words = Context.Words.ToList();
            _collections = Context.Collections.ToList();

            //MainPage = new MDPage();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            base.OnStart();

            if (!Properties.ContainsKey("scores"))
                return;
            Scores = (int)Properties["scores"];

            if (!Properties.ContainsKey("IsAuthenticated"))
                return;
            IsAuthenticated = (bool)Properties["IsAuthenticated"];
        }

        protected override void OnSleep()
        {
            base.OnSleep();
            Properties["scores"] = Scores;
            Properties["IsAuthenticated"] = IsAuthenticated;
        }

        public static List<Word> addWords()
        {
            var list = new List<Word>();

            list.Add(new Word() { Id = Guid.NewGuid(), Text = "ありがとう", Transcription = "arigato", Translation = "Thank you", Language = "Japanese" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "こんにちは", Transcription = "konnichiwa", Translation = "Hello", Language = "Japanese" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "おはよう", Transcription = "ohayou", Translation = "Good morning", Language = "Japanese" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "おやすみなさい", Transcription = "oyasuminasai", Translation = "Good night", Language = "Japanese" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "水", Transcription = "みず", Translation = "Water", Language = "Japanese" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "犬", Transcription = "いぬ", Translation = "Dog", Language = "Japanese" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "よろしく", Transcription = "yoroshku", Translation = "Nice to meet you", Language = "Japanese" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "雲", Transcription = "くも", Translation = "Cloud", Language = "Japanese" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "風", Transcription = "かぜ", Translation = "Wind", Language = "Japanese" });

            list.Add(new Word() { Id = Guid.NewGuid(), Text = "一", Transcription = "いち", Translation = "One", Language = "Japanese" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "二", Transcription = "に", Translation = "Two", Language = "Japanese" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "三", Transcription = "さん", Translation = "Three", Language = "Japanese" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "四", Transcription = "よん", Translation = "Four", Language = "Japanese" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "五", Transcription = "ご", Translation = "Five", Language = "Japanese" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "六", Transcription = "ろく", Translation = "Six", Language = "Japanese" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "七", Transcription = "なな", Translation = "Seven", Language = "Japanese" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "八", Transcription = "はち", Translation = "Eight", Language = "Japanese" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "九", Transcription = "きゅう", Translation = "Nine", Language = "Japanese" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "十", Transcription = "じゅう", Translation = "Ten", Language = "Japanese" });

            list.Add(new Word() { Id = Guid.NewGuid(), Text = "人", Transcription = "じん", Translation = "Man", Language = "Japanese" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "口", Transcription = "くち", Translation = "Mouth", Language = "Japanese" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "語", Transcription = "ご", Translation = "Word", Language = "Japanese" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "体", Transcription = "からだ", Translation = "Body", Language = "Japanese" });
            list.Add(new Word() { Id = Guid.NewGuid(), Text = "心", Transcription = "こころ", Translation = "Heart", Language = "Japanese" });
            //list.Add(new Word() { Id = Guid.NewGuid(), Text = "", Transcription = "", Translation = "" });

            return list;
        }
    }
}