using Microsoft.EntityFrameworkCore;
using MovieApi.Models;

namespace MovieApi.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static void SeedData(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<MovieContext>();

            context.Database.Migrate(); 

            if (!context.Movies.Any())
            {
                var movie1 = new Movie
                {
                    Title = "The Matrix",
                    Genre = "Sci-Fi",
                    Year = 1999,
                    Duration = 136,
                    MovieDetails = new MovieDetails
                    {
                        Synopsis = "Neo discovers the truth...",
                        Language = "English",
                        Budget = 63000000
                    },
                    Reviews = new List<Review>
                    {
                        new Review { ReviewerName = "Alice", Comment = "Amazing", Rating = 5 },
                        new Review { ReviewerName = "Bob", Comment = "Classic", Rating = 4 }
                    },
                    MovieActors = new List<MovieActor>
                    {
                        new MovieActor
                        {
                            Actor = new Actor 
                            { 
                                Name = "Keanu Reeves", 
                                BirthYear = 1964,
                                MovieActors = new List<MovieActor>()
                            }
                        }
                    }
                };

                context.Movies.Add(movie1);
                context.SaveChanges();
            }
        }
    }
}
