using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace MosEisleyCantina.MosEisleyCantina.Common.Models
{
    public class Dish
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(18, 2)")] 
        public decimal Price { get; set; }

        [MaxLength(2083)]
        public string ImageUrl { get; set; } = string.Empty;

        public decimal? Rating { get; set; }
    }
}