namespace MovieApi.DTOs
{
    public class MovieUpdateDto
    {
        public required string Title { get; set; }
        public int ReleaseYear { get; set; }
        public int MovieDetailsId { get; set; }
    }
}
