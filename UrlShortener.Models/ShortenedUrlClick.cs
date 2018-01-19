using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UrlShortener.Models
{
    public class ShortenedUrlClick
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string ShortenedUrlId { get; set; }

        [Required]
        public DateTime ClickDate { get; set; }

        [MaxLength(500)]
        public string Referrer { get; set; }

        public virtual ShortenedUrl ShortenedUrl { get; set; }
    }
}
