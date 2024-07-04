using lyricboxd.Data;
using lyricboxd.Models;
using lyricboxd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace lyricboxd.Controllers
{
    public class HomeController : Controller
    {
        private readonly LyricboxdDbContext _context;
        private readonly SpotifyService _spotifyService;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;

        public HomeController(LyricboxdDbContext context, SpotifyService spotifyService, IConfiguration configuration, UserManager<User> userManager)
        {
            _context = context;
            _spotifyService = spotifyService;
            _configuration = configuration;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var reviews = await _context.Reviews
                .Include(r => r.User)
                .ToListAsync();

            var clientId = _configuration["Spotify:ClientId"];
            var clientSecret = _configuration["Spotify:ClientSecret"];
            var accessToken = await _spotifyService.GetAccessToken(clientId, clientSecret);

            var reviewDetails = new List<ReviewDetailsViewModel>();

            foreach (var review in reviews)
            {
                var track = await _spotifyService.GetTrack(accessToken, review.SongId);
                var trackViewModel = new TrackViewModel
                {
                    Id = track["id"].ToString(),
                    Name = track["name"].ToString(),
                    Artist = track["artists"][0]["name"].ToString(),
                    Album = track["album"]["name"].ToString(),
                    ReleaseYear = track["album"]["release_date"].ToString().Split('-')[0],
                    AlbumCoverUrl = track["album"]["images"][0]["url"].ToString()
                };

                reviewDetails.Add(new ReviewDetailsViewModel
                {
                    Review = review,
                    Track = trackViewModel
                });
            }

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                ViewBag.UserName = user.UserName;
            }

            return View(reviewDetails);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
