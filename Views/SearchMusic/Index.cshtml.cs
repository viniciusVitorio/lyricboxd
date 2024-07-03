using lyricboxd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace lyricboxd.Pages.SearchMusic
{
    public class SearchMusicModel : PageModel
    {
        private readonly SpotifyService _spotifyService;
        private readonly IConfiguration _configuration;

        public SearchMusicModel(SpotifyService spotifyService, IConfiguration configuration)
        {
            _spotifyService = spotifyService;
            _configuration = configuration;
        }

        public async Task<IActionResult> OnGetResults(string query)
        {
            var clientId = _configuration["Spotify:ClientId"];
            var clientSecret = _configuration["Spotify:ClientSecret"];
            var accessToken = await _spotifyService.GetAccessToken(clientId, clientSecret);
            var results = await _spotifyService.SearchTracks(accessToken, query);

            return new JsonResult(results);
        }
    }
}
