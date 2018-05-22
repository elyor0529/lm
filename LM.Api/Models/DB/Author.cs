using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LM.Api.Models.EF;

namespace LM.Api.Models.DB
{
    public class Author : BaseEntity
    {
       
        [Required]
        [StringLength(20)]
        public string FirstName { get; set; }
         
        [Required]
        [StringLength(20)]
        public string LastName { get; set; }

        public int? BookId { get; set; }

        [ForeignKey("BookId")]
        public virtual Book Book{ get; set; }
    }
}
