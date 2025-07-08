using System.ComponentModel.DataAnnotations;

namespace MovieApi.DTOs
{
    public class MovieCreateDto
    {
        [Required]
        public required string Title { get; set; }

        [Range(1900, 2100)]
        public int ReleaseYear { get; set; }

        public int MovieDetailsId { get; set; }
    }
}
