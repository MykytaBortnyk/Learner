using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Learner.Models;

namespace Learner.ViewModels
{
    public class CollectionWordViewModel
    {
        public Word Item { get; set; }

        public bool IsSelected { get; set; }
      
        public static List<CollectionWordViewModel>CastCollection(IEnumerable<Word> words)
        {
            var list = new List<CollectionWordViewModel>();

            foreach (var word in words)
            {
                list.Add(
                    new CollectionWordViewModel { Item = word });
            }

            return list;
        }

        public static List<Word> CastCollection(IEnumerable<CollectionWordViewModel> words)
        {
            var list = new List<Word>();

            foreach (var word in words)
            {
                list.Add(word.Item);
            }

            return list;
        }
    }
}
