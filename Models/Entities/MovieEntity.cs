﻿using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace MovieCatalogApi.Models.Entities;

public class MovieEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Year { get; set; }
    public string Country { get; set; }
    public int Time { get; set; }
    public string Tagline { get; set; }
    public string Director { get; set; }
    public int Budget { get; set; }
    public int Fees { get; set; }
    public int AgeLimit { get; set; }
    public GenreEntity Genre { get; set; }
    public List<UserEntity> LikedUsers { get; set; } = new();
}