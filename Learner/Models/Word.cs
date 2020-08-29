using System;
namespace Learner.Models
{
    public class Word
    {
        public Guid Id { get; set; }

        public String Text { get; set; }

        public String Transcription { get; set; }

        public String Translation { get; set; }
    }
}
