using System;
namespace RestAPI.ViewModels
{
    public class WordViewModel
    {
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
    }
}
