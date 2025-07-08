namespace MovieApi.DTOs
{
    public class MovieDto
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public int ReleaseYear { get; set; }
    }
}
