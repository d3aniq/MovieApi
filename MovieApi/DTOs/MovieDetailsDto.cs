namespace MovieApi.DTOs
{
    public class MovieDetailsDto
    {
        public int Id { get; set; }
        public required string Genre { get; set; }
        public int Duration { get; set; }
    }
}
