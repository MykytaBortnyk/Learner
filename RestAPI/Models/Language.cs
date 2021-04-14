using System;
namespace RestAPI.Models
{
    /// <summary>
    /// Модель языка, будет использована в будущем
    /// </summary>
    public class Language
    {
        public enum WritingSystem
        {
            Latin,
            Cyrillic,
            Greek,
            Hanzi,
            Kanji,
            Kana,
            Hanja,
            Arabic,
            Hebrew
        }

        public enum Category
        {
            Alphabetical,
            Syllabic,
            Abjad
        }

        public Guid Id { get; set; }

        public String Name { get; set; }

        public WritingSystem Script { get; set; }

        public Category ScriptCategory { get; set; }
    }
}
