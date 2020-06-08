using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BulkyBook.Models
{
    public class ProductImg
    {
        [Key]
        public int Id { get; set; }

        public string ImageUrl { get; set; }

        [ForeignKey("ProdId")]
        public virtual Product Product { get; set; }

    }
}
