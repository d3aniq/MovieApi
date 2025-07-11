﻿namespace MovieApi.Models
{
    public class Actor
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public int BirthYear { get; set; }

        public required ICollection<MovieActor> MovieActors { get; set; }
    }
}
