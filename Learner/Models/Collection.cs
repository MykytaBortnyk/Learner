using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        public List<Word> Words { get; set; }
    }
}
