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
                Title = "Colections",
                IconSource = "baseline_style_black_18dp.png",
                TargetType = typeof(MainPage)
            });

            /*MenuItemsListView = new ListView
            {
                ItemsSource = masterPageItems,
                ItemTemplate = new DataTemplate(() =>
                {
                    var grid = new Grid { Padding = new Thickness(5, 10) };
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

                    var image = new Image();
                    image.SetBinding(Image.SourceProperty, "IconSource");
                    var label = new Label { VerticalOptions = LayoutOptions.FillAndExpand };
                    label.SetBinding(Label.TextProperty, "Title");

                    grid.Children.Add(image);
                    grid.Children.Add(label, 1, 0);

                    return new ViewCell { View = grid };
                }),
                SeparatorVisibility = SeparatorVisibility.None
            };*/

            MenuItemsListView.ItemsSource = masterPageItems;

            IconImageSource = "baseline_menu_white_18dp.png";
            Title = "Learner";
        }
    }
}
