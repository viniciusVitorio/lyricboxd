using lyricboxd.Data;
using lyricboxd.Models;
using lyricboxd.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace lyricboxd.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly LyricboxdDbContext _context;
        private readonly SpotifyService _spotifyService;
        private readonly IConfiguration _configuration;

        public ReviewsController(LyricboxdDbContext context, SpotifyService spotifyService, IConfiguration configuration)
        {
            _context = context;
            _spotifyService = spotifyService;
            _configuration = configuration;
        }

        // GET: Reviews
        public async Task<IActionResult> Index()
        {
            var lyricboxdDbContext = _context.Reviews.Include(r => r.User);
            return View(await lyricboxdDbContext.ToListAsync());
        }

        // GET: Reviews/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // GET: Reviews/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email");
            return View();
        }

        // POST: Reviews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,SongId,Rating,ReviewText,CreatedAt")] Review review)
        {
            if (ModelState.IsValid)
            {
                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", review.UserId);
            return View(review);
        }

        // GET: Reviews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", review.UserId);
            return View(review);
        }

        // POST: Reviews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,SongId,Rating,ReviewText,CreatedAt")] Review review)
        {
            if (id != review.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(review);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReviewExists(review.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", review.UserId);
            return View(review);
        }

        // GET: Reviews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var review = await _context.Reviews
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (review == null)
            {
                return NotFound();
            }

            return View(review);
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }

        // GET: Reviews/CreateWithSpotify
        [HttpGet]
        [HttpGet]
        public async Task<IActionResult> CreateWithSpotify(string trackName)
        {
            ViewBag.ClientId = _configuration["Spotify:ClientId"];
            ViewBag.ClientSecret = _configuration["Spotify:ClientSecret"];

            if (string.IsNullOrEmpty(trackName))
            {
                ViewData["UserId"] = new SelectList(_context.User, "Id", "Email");
                return View();
            }

            var clientId = _configuration["Spotify:ClientId"];
            var clientSecret = _configuration["Spotify:ClientSecret"];
            var accessToken = await _spotifyService.GetAccessToken(clientId, clientSecret);
            var searchResult = await _spotifyService.SearchTracks(accessToken, trackName);

            // Extrair informações relevantes para a View
            var tracks = searchResult["tracks"]["items"].ToList();
            var model = tracks.Select(track => new TrackViewModel
            {
                Id = track["id"].ToString(),
                Name = track["name"].ToString(),
                Artist = track["artists"][0]["name"].ToString(),
                Album = track["album"]["name"].ToString(),
                ReleaseYear = track["album"]["release_date"].ToString().Split('-')[0],
                AlbumCoverUrl = track["album"]["images"][0]["url"].ToString()
            }).ToList();

            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email");
            return View(model);
        }

        // POST: Reviews/CreateWithSpotify
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateWithSpotify([Bind("Id,UserId,SongId,Rating,ReviewText,CreatedAt")] Review review)
        {
            if (ModelState.IsValid)
            {
                _context.Add(review);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", review.UserId);
            return View(review);
        }

        [HttpGet]
        public async Task<IActionResult> SearchTracks(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                return BadRequest("Query is required.");
            }

            var clientId = _configuration["Spotify:ClientId"];
            var clientSecret = _configuration["Spotify:ClientSecret"];
            var accessToken = await _spotifyService.GetAccessToken(clientId, clientSecret);
            var searchResult = await _spotifyService.SearchTracks(accessToken, query);

            // Extrair informações relevantes para a View
            var tracks = searchResult["tracks"]["items"].Select(track => new TrackViewModel
            {
                Id = track["id"].ToString(),
                Name = track["name"].ToString(),
                Artist = track["artists"][0]["name"].ToString()
            }).ToList();

            return Json(tracks);
        }
    }
}
