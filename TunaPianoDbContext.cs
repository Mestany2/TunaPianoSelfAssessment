using Microsoft.EntityFrameworkCore;
using TunaPiano.Models;
using System.Runtime.CompilerServices;

public class TunaPianoDbContext : DbContext
    {
        public DbSet<Song> Songs { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Genre> Genres { get; set; }

        public TunaPianoDbContext(DbContextOptions<TunaPianoDbContext> context) : base(context)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          
            modelBuilder.Entity<Artist>().HasData(new Artist[]
            {
        new Artist {Id = 1, Name = "Steve Butler", Age = 32, Bio = "Grow up in NSS"},
        new Artist {Id = 2, Name = "Cameron Dorris", Age = 31, Bio = "The man with the plan"},
        new Artist {Id = 3, Name = "Mads Cook", Age = 22, Bio = "Ahead of the game"},

            });            
            modelBuilder.Entity<Song>().HasData(new Song[]
            {
        new Song {Id = 1, Title = "Song One", ArtistId = 2, Album = "Album One", Length=12},
        new Song {Id = 2, Title = "Song Two", ArtistId = 1, Album = "Album Two", Length=24},
        new Song {Id = 3, Title = "Song Three", ArtistId = 3, Album = "Album Three", Length=32},

            });           
            modelBuilder.Entity<Genre>().HasData(new Genre[]
            {
        new Genre {Id = 1, Description="Rock"},
        new Genre {Id = 2, Description="Jazz"},
        new Genre {Id = 3, Description="Pop"},


            });
        }


    }

