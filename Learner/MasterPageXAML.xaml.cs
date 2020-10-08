using System;
using System.Collections.Generic;
using Learner.ViewModels;
using Xamarin.Forms;

namespace Learner
{
    public partial class MasterPageXAML : ContentPage
    {
        public MasterPageXAML()
        {
            InitializeComponent();

            var masterPageItems = new List<MasterPageItem>();
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Dictionary",
                IconSource = "baseline_book_black_18dp.png",
                TargetType = typeof(MainPage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Quiz",
                IconSource = "baseline_school_black_18dp.png",
                TargetType = typeof(QuizPage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Collections",
                IconSource = "baseline_style_black_18dp.png",
                TargetType = typeof(CollectionsPage)
            });

            MenuItemsListView.ItemsSource = masterPageItems;

            IconImageSource = "baseline_menu_white_18dp.png";
            Title = "Learner";
        }
    }
}
