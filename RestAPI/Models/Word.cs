using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestAPI.Models
{
    /// <summary>
    /// Модель слова
    /// </summary>
    public class Word
    {
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
        public Guid UserId { get; set; }

        /// <summary>
        /// Навигационное свойство
        /// </summary>
        public AppUser AppUser { get; set; }
    }
}
