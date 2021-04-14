using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestAPI.Models
{
    /// <summary>
    /// Модель коллекции слов пользователя
    /// </summary>
    public class Collection
    {
        public Collection()
        {
            Words = new List<Word>();
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
        public Guid UserId { get; set; }

        /// <summary>
        /// Навигационное свойство
        /// </summary>
        public AppUser AppUser { get; set; }

        /// <summary>
        /// Навигационное свойство
        /// </summary>
        public List<Word> Words { get; set; }
    }
}
