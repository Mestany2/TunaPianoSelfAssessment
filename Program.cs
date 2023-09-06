using TunaPiano.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// allows our api endpoints to access the database through Entity Framework Core
builder.Services.AddNpgsql<TunaPianoDbContext>(builder.Configuration["TunaPianoDbConnectionString"]);

// Set the JSON serializer options
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}




app.UseAuthorization();

app.MapControllers();

//Create a song
app.MapPost ("/api/Song", (TunaPianoDbContext db, Song song) =>
{
    db.Songs.Add(song);
    db.SaveChanges();
    return Results.Created($"/api/Song/{song.Id}", song);
});

//Assign a Genre to a song
app.MapPost("/api/SongGenre", (TunaPianoDbContext db, int songId, int genreId) =>
{
    var getSong = db.Songs.FirstOrDefault(s => s.Id == songId);
    var getGenre = db.Genres.FirstOrDefault(g => g.Id == genreId);
    if (getSong.Genres == null)
    {
        getSong.Genres = new List<Genre>();
    }

    getSong.Genres.Add(getGenre);
    db.SaveChanges();
    return getSong;


});

//Delete a song
app.MapDelete("/api/song/{songId}", (TunaPianoDbContext db, int songId) =>
{
    Song song = db.Songs.FirstOrDefault(s => s.Id == songId);
    if (song == null)
    {
        return Results.NotFound();
    }
    db.Songs.Remove(song);
    db.SaveChanges();
    return Results.NoContent();
});

//Update a song
app.MapPut("/api/Song/{id}", (TunaPianoDbContext db, int id, Song song) =>
{
    Song SongToUpdate = db.Songs.SingleOrDefault(song => song.Id == id);
    if (SongToUpdate == null)
    {
        return Results.NotFound();
    }
    SongToUpdate.ArtistId = song.ArtistId;
    SongToUpdate.Album = song.Album;
    SongToUpdate.Title = song.Title;
    SongToUpdate.Length = song.Length;

    db.SaveChanges();
    return Results.NoContent();
});

//View a list of songs
app.MapGet("/api/allSongs", (TunaPianoDbContext db) =>
{
   return db.Songs
    .Include(s => s.Artist)
    .ToList();

});

//Details view of a single Song and its associated genres and artist details

app.MapGet("/api/SongsDetails", (TunaPianoDbContext db, int id) =>
{
    var song = db.Songs.Where(s => s.Id == id)
    .Include(x => x.Genres)
    .Include(s => s.Artist).ToList();    

    return song;
}
);

//Create an Artist
app.MapPost("/api/Artist", (TunaPianoDbContext db, Artist artist) =>
{
    db.Artists.Add(artist);
    db.SaveChanges();
    return Results.Created($"/api/Artist/{artist.Id}", artist);
});


//Delete an Artist
app.MapDelete("/api/song/{songId}", (TunaPianoDbContext db, int artistId) =>
{
    Artist artist = db.Artists.FirstOrDefault(a => a.Id == artistId);
    if (artist == null)
    {
        return Results.NotFound();
    }
    db.Artists.Remove(artist);
    db.SaveChanges();
    return Results.NoContent();
});

app.Run();
