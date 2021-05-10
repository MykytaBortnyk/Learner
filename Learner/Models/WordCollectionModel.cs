using System;
using System.Collections.Generic;

namespace Learner.Models
{
    public class WordCollection
    {
        public Guid WordId { get; set; }
        public Guid CollectionId { get; set; }

        public Collection Collection { get; set; }
        public Word Word { get; set; }
    }
}
