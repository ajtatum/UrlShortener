using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UrlShortener.Models
{
    public class ShortenedUrl
    {
        public ShortenedUrl()
        {
            ShortenedUrlClicks = new HashSet<ShortenedUrlClick>();
        }

        [Key]
        [MaxLength(20)]
        public string Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string LongUrl { get; set; }

        [Required]
        [MaxLength(50)]
        public string CreatedBy { get; set; }

        [Required]
        public DateTime CreatedOn { get; set; }

        public ICollection<ShortenedUrlClick> ShortenedUrlClicks { get; set; }
    }
}
