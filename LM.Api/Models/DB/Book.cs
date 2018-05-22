using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LM.Api.Filters;
using LM.Api.Models.EF;

namespace LM.Api.Models.DB
{
    public class Book : BaseEntity
    {
        public Book()
        {
            Authors = new HashSet<Author>();
        }

        [Required]
        [StringLength(20)]
        public string Title { get; set; }

        [Required]
        [Range(1, 10000)]
        public int? CountOfPage { get; set; }

        [Required]
        [StringLength(20)]
        public string Publisher { get; set; }

        [Required]
        [Range(1800, 2016)]
        public int? Year { get; set; }

        [Required]
        [StringLength(13, MinimumLength = 10)]
        [ISBNValidation]
        public string ISBN { get; set; }

        [StringLength(250)]
        public string PicturePath { get; set; }

        public virtual ICollection<Author> Authors { get; set; }
 
    }
}
