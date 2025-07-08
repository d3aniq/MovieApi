namespace MovieApi.DTOs
{
    public class MovieDetailDto
    {
    public int Id { get; set; }
    public required string Title { get; set; }
    public int ReleaseYear { get; set; }

    public required MovieDetailsDto MovieDetails { get; set; }

    public required List<ReviewDto> Reviews { get; set; }
    public required List<ActorDto> Actors { get; set; }
    }
}
