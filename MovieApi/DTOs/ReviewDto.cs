namespace MovieApi.DTOs
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public required string Text { get; set; }
        public int Rating { get; set; }
    }
}
