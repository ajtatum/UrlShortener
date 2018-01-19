using System;
using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models
{
    public class ShortenedUrl
    {
        [Key]
        [MaxLength(20)]
        public string Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string LongUrl { get; set; }

        public int ClickCount { get; set; }

        [Required]
        [MaxLength(50)]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }
    }
}
