using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learner.Models
{
    public class Collection
    {
        public Collection()
        {
            Words = new List<Word>();
        }

        public Guid Id { get; set; }

        public String Name { get; set; }

        public String Language { get; set; }

        [NotMapped]
        public ICollection<Word> Words { get; set; }

        public List<WordCollection> WordCollections { get; set; }
    }
}
