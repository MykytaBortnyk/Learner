using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learner.Models
{
    public class ManyToMany
    {
        [Required]
        public string CollectionId { get; set; }

        [ForeignKey("CollectionId")]
        public Collection Collection { get; set; }

        [Required]
        public string WordId { get; set; }

        [ForeignKey("WordId")]
        public Word Word { get; set; }
    }
}
