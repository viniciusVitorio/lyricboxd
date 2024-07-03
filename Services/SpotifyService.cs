using lyricboxd.Models;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace lyricboxd.Services
{
    public class SpotifyService
    {
        private readonly HttpClient _httpClient;

        public SpotifyService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetAccessToken(string clientId, string clientSecret)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");
            var clientCredentials = System.Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}"));
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", clientCredentials);
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string> {
                {"grant_type", "client_credentials"}
            });

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(responseContent);
            return json["access_token"].ToString();
        }

        public async Task<JObject> SearchTracks(string accessToken, string query)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.spotify.com/v1/search?q={query}&type=track");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JObject.Parse(responseContent);
        }

        public async Task<JObject> GetTrack(string accessToken, string trackId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://api.spotify.com/v1/tracks/{trackId}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JObject.Parse(responseContent);
        }

        public async Task<List<TrackViewModel>> SearchTracksAsync(string query, string clientId, string clientSecret)
        {
            var accessToken = await GetAccessToken(clientId, clientSecret);
            var searchResults = await SearchTracks(accessToken, query);

            var tracks = new List<TrackViewModel>();

            foreach (var item in searchResults["tracks"]["items"])
            {
                tracks.Add(new TrackViewModel
                {
                    Id = item["id"].ToString(),
                    Name = item["name"].ToString(),
                    Artist = string.Join(", ", item["artists"].Select(a => a["name"].ToString())),
                    Album = item["album"]["name"].ToString(),
                    ReleaseYear = item["album"]["release_date"].ToString().Split('-')[0],
                    AlbumCoverUrl = item["album"]["images"].First["url"].ToString()
                });
            }

            return tracks;
        }
    }
}
