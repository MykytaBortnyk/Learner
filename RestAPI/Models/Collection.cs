using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RestAPI.ViewModels;

namespace RestAPI.Models
{
    /// <summary>
    /// Модель коллекции слов пользователя
    /// </summary>
    public class Collection
    {
        public Collection() { Words = new List<Word>(); }

        public Collection(CollectionViewModel value, string userId)
        {
            Id = Guid.NewGuid();
            Name = value.Name;
            Language = value.Language;
            AppUserId = userId;
            Words = /*value.Words ?? */new List<Word>();
        }

        public Guid Id { get; set; }

        /// <summary>
        /// Название пользовательской колекции
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Временный вариант записи языка, в будущем будет реализован отдельной моделью
        /// </summary>
        public String Language { get; set; }

        /// <summary>
        /// Навигационное свойство
        /// </summary>
        public string AppUserId { get; set; }

        /// <summary>
        /// Навигационное свойство
        /// </summary>
        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }

        /// <summary>
        /// Навигационное свойство
        /// </summary>
        //[ForeignKey("WordId")]
        //public List<Word> Words { get; set; }
        public ICollection<Word> Words { get; set; }
    }
}