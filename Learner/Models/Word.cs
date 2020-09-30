using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learner.Models
{
    public class Word
    {
        public Guid Id { get; set; }

        public String Text { get; set; }

        public String Transcription { get; set; }

        public String Translation { get; set; }

        public String Language { get; set; }

        //public Guid CollectionId { get; set; }

        //[ForeignKey("CollectionId")]
        //public List<Collection> Collections { get; set; }
    }
}
