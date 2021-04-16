using System;
using System.Collections.Generic;
using RestAPI.Models;

namespace RestAPI.ViewModels
{
    public class CollectionViewModel
    {
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
        public List<Word> Words { get; set; }
    }
}
