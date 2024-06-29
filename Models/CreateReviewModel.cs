using lyricboxd.Data;
using lyricboxd.Models;
using lyricboxd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;

namespace lyricboxd.Pages.Reviews
{
    public class CreateReviewModel : PageModel
    {
        private readonly SpotifyService _spotifyService;
        private readonly IConfiguration _configuration;
        private readonly LyricboxdDbContext _context;

        [BindProperty]
        public Review Review { get; set; } = new Review(); // Inicializando Review

        public string TrackName { get; set; }
        public string ArtistName { get; set; }
        public string AlbumName { get; set; }
        public string ReleaseYear { get; set; }
        public string AlbumCoverUrl { get; set; }

        public CreateReviewModel(SpotifyService spotifyService, IConfiguration configuration, LyricboxdDbContext context)
        {
            _spotifyService = spotifyService;
            _configuration = configuration;
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(string trackId)
        {
            if (string.IsNullOrEmpty(trackId))
            {
                return Page();
            }

            var clientId = _configuration["Spotify:ClientId"];
            var clientSecret = _configuration["Spotify:ClientSecret"];
            var accessToken = await _spotifyService.GetAccessToken(clientId, clientSecret);
            var track = await _spotifyService.GetTrack(accessToken, trackId);

            TrackName = track["name"].ToString();
            ArtistName = track["artists"][0]["name"].ToString();
            AlbumName = track["album"]["name"].ToString();
            ReleaseYear = track["album"]["release_date"].ToString().Split('-')[0];
            AlbumCoverUrl = track["album"]["images"][0]["url"].ToString();

            Review.SongId = trackId;
            Review.UserId = Guid.NewGuid(); // substitua isso com o ID real do usuário logado

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Reviews.Add(Review);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }
}
