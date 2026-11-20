using System.Net.Http.Json;
using ETLClientOpinionAnalysis.Core.Domain.Entities.Api;
using ETLClientOpinionAnalysis.Core.Domain.Interfaces;
using ETLClientOpinionAnalysis.Core.Domain.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ETLClientOpinionAnalysis.Infrastructure.Extractors.Api
{
    public class ApiExtractor : IExtractor<SocialComment>
    {
        public const string HttpClientName = "SocialCommentsApi";

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ApiSettings _settings;
        private readonly ILogger<ApiExtractor> _logger;

        public ApiExtractor(IHttpClientFactory httpClientFactory, IOptions<ApiSettings> settings,
            ILogger<ApiExtractor> logger)
        {
            _httpClientFactory = httpClientFactory;
            _settings = settings.Value;
            _logger = logger;
        }

        public string SourceName => "social_comments";

        public async Task<IReadOnlyList<SocialComment>> ExtractAsync(CancellationToken cancellationToken = default)
        {
            var client = _httpClientFactory.CreateClient(HttpClientName);
            var comments = new List<SocialComment>();

            var page = 1;
            var totalPages = 1;

            while (page <= totalPages)
            {
                var url = $"api/comments?page={page}&pageSize={_settings.PageSize}";

                var response = await client.GetFromJsonAsync<ApiPagedResponse>(url, cancellationToken)
                    ?? throw new InvalidOperationException("La API devolvio una respuesta vacia.");

                comments.AddRange(response.Items);
                totalPages = response.TotalPages;

                _logger.LogInformation("API: pagina {Page} de {TotalPages} leida", page, totalPages);

                page++;
            }

            return comments;
        }
    }
}
