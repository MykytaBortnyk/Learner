using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RestAPI.ViewModels;

namespace RestAPI.Models
{
    /// <summary>
    /// Модель слова
    /// </summary>
    public class Word
    {
        public Word() { Collections = new List<Collection>(); }
        public Word(WordViewModel value, string userId)
        {
            Id = Guid.NewGuid();
            Text = value.Text;
            Transcription = value.Transcription;
            Translation = value.Transcription;
            Language = value.Language;
            AppUserId = userId;
            Collections = value.Collections ?? new List<Collection>();
        }

        public Guid Id { get; set; }

        /// <summary>
        /// Слово by itself
        /// </summary>
        public String Text { get; set; }

        /// <summary>
        /// Временный вариант трнаскрипции, будет зависеть от языка
        /// </summary>
        public String Transcription { get; set; }

        /// <summary>
        /// Перевод слова
        /// </summary>
        public String Translation { get; set; }

        /// <summary>
        /// Временный вариант записи языка, в будущем будет реализован отдельной моделью
        /// </summary>
        public String Language { get; set; }

        /// <summary>
        /// Навигационное свойство
        /// </summary>
        public String AppUserId { get; set; }

        /// <summary>
        /// Навигационное свойство
        /// </summary>
        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }

        //[ForeignKey("CollectionId")]
        //public List<Collection> Collections { get; set; }
        public ICollection<Collection> Collections { get; set; }
    }
}