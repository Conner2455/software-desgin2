using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Porter_ChinookMusicApp.Data;
using Porter_ChinookMusicApp.Models;
using Porter_ChinookMusicApp.Models.Dtos;
using Porter_ChinookMusicApp.Models.Entities;
using System.Security;

namespace Porter_ChinookMusicApp.Services;

public class MusicQueryService
{
    private readonly ApplicationDbContext _context;
    public MusicQueryService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Artist>> GetAllArtistsWithAlbums()
    {
        return await _context.Artist
            .Where(artist => artist.Albums.Count > 0)
            .Include(artist => artist.Albums)
            .ToListAsync();
    }

    public async Task<List<Artist>> GetAllArtistsWithMoreThanOneAlbum()
    {
        return await _context.Artist
            .Where(artist => artist.Albums.Count > 1)
            .ToListAsync();
    }

    public async Task<Artist?> GetArtistByNameWithAlbums(string artistName)
    {
        return await _context.Artist
            .Where(artist => artist.Name == artistName)
            .Include(artist => artist.Albums)
            .SingleOrDefaultAsync();
    }

    public async Task<List<Track>> GetTracksByAlbumId(int albumId)
    {
        return await _context.Track
            .Where(track => track.Album.AlbumId == albumId)
            .ToListAsync();
    }

    public async Task<List<Genre>> GetAllGenresWithTracks()
    {
        return await _context.Genre
            .Include(genre => genre.Tracks)
            .ToListAsync();
    }

    public async Task<List<Track>> GetTracksByGenreId(int genreId)
    {
        return await _context.Track
            .Where(track => track.GenreId == genreId)
            .ToListAsync();
    }

    public async Task<List<Statistic>> GetTotalTracksByAlbum()
    {
        return await _context.Album
            .Select(album => new Statistic
            {
                Label = album.Title,
                Value = album.Tracks.Count()
            })
            .ToListAsync();
    }

    public async Task<List<Album>> GetAlbumsByArtistId(int artistId)
    {
        return await _context.Album
            .Where(album => album.ArtistId == artistId)
            .ToListAsync();
    }

    public async Task<List<Playlist>> GetAllPlaylistsWithTracks()
    {
        return await _context.Playlist
            .Where(playlist => playlist.Tracks.Count > 0)
            .Include(playlist => playlist.Tracks)
            .ToListAsync();
    }

    public async Task<List<Statistic>> GetAverageDurationByGenre()
    {
        return await _context.Genre
            .Select(genre => new Statistic
            {
                Label = genre.Name,
                Value = (decimal) genre.Tracks.Average(track => track.Milliseconds) / 1000,
                ValueMetric = "Seconds"
            })
            .ToListAsync();
    }

    // #11
    public async Task<List<Artist>> GetArtistsWithoutAlbums()
    {
        return await _context.Artist
            .Where(artist => artist.Albums.Count == 0)
            .ToListAsync();
    }

    public async Task<List<Track>> GetTracksWithGenreAndAlbum()
    {
        return await _context.Track
            .Include(track => track.Genre)
            .Include(track => track.Album)
            .ToListAsync();
    }

    public async Task<List<TrackDetails>> GetTrackDetails()
    {
        return await _context.Track
            .Include(track => track.Album)
            .ThenInclude(track => track.Artist)
            .Select(track => new TrackDetails
            {

                Track = track.Name,
                Album = track.Album.Title,
                Artist = track.Album.Artist.Name
            })
            .ToListAsync();
    }

    public async Task<List<Statistic>> GetAlbumsWithTrackDuration()
    {
        return await _context.Album
            .Select(album => new Statistic
            {
                Label = album.Title,
                Value = album.Tracks.Sum(track => track.Milliseconds) / 1000,
                ValueMetric = "Seconds"
            })
            .ToListAsync();
    }

    public async Task<List<Statistic>> GetGenreTrackCounts()
    {
        return await _context.Genre
            .Select(genre => new Statistic
            {
                Label = genre.Name,
                Value = genre.Tracks.Count(),
                ValueMetric = "Count"
            })
            .ToListAsync();
    }

    public async Task<List<Statistic>> GetPlaylistsWithTrackCount()
    {
        return await _context.Playlist
            .Select(playlist => new Statistic
            {
                Label = playlist.Name,
                Value = playlist.Tracks.Count(),
                ValueMetric = "Count"
            })
            .ToListAsync();
    }

     public async Task<List<Track>> GetTracksByPlaylistId(int playlistId)
    {
        return await _context.Playlist
            .Where(p => p.PlaylistId == playlistId)
            .SelectMany(p => p.Tracks)
            .ToListAsync();
    }

    public async Task<Playlist?> GetPlaylistWithMostTracks()
    {
        return await _context.Playlist
            .OrderByDescending(playlist => playlist.Tracks.Count)
            .FirstOrDefaultAsync();
    }

    public async Task<Playlist?> GetPlaylistWithLeastTracks()
    {
        return await _context.Playlist
            .OrderBy(playlist => playlist.Tracks.Count)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Statistic>> GetTopFivePlaylistsWithMostTracks()
    {
        return await _context.Playlist
            .OrderByDescending(playlist => playlist.Tracks.Count)
            .Take(5)
            .Select(playlist => new Statistic
            {
                Label = playlist.Name,
                Value = playlist.Tracks.Count
            })
            .ToListAsync();
    }

    public async Task<List<Statistic>> GetBottomFivePlaylistsWithLeastTracks()
    {
        return await _context.Playlist
            .OrderBy(playlist => playlist.Tracks.Count)
            .Take(5)
            .Select(playlist => new Statistic
            {
                Label = playlist.Name,
                Value = playlist.Tracks.Count
            })
            .ToListAsync();
    }
}