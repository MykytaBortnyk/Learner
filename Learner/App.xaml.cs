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

        public App()
        {
            InitializeComponent();

            _dbPath = Path.Combine(Environment.GetFolderPath(
                        Environment.SpecialFolder.LocalApplicationData), "Words.db3");

            using var db = new ApplicationContext(_dbPath);

            // Ensure database is created
            db.Database.EnsureCreated();
            if (!db.Words.Any())
            {
                // Insert Data
                db.Add(new Word() { Id = Guid.NewGuid(), Text = "Usaly", Translation = "Обычно", Transcription = "ˈyo͞oZH(o͞o)əlē" });

                db.SaveChanges();
            }
            // Retreive Data
            _words = db.Words.ToList();

            MainPage = new NavigationPage(new MainPage());
        }
    }
}
