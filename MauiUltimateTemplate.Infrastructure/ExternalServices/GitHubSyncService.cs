using System.Net.Http.Json;

using Application.Domain.Entities;

using MauiUltimateTemplate.Domain.Interfaces;

namespace MauiUltimateTemplate.Infrastructure.ExternalServices
{
    public class GitHubSyncService : ICloudSyncService
    {
        private readonly HttpClient _httpClient;

        public GitHubSyncService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> PushToCloudAsync(Note note)
        {
            var response = await _httpClient.PostAsJsonAsync("https://api.github.com", note);
            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<Note>> PullFromCloudAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<Note>>("https://api.github.com");
        }
    }
}