﻿namespace NewsBlog.Models
{
    public class Page
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Summary { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? Slug { get; set; }
    }
}
