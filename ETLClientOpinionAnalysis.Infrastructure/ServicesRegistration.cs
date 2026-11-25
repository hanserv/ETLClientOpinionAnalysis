using ETLClientOpinionAnalysis.Core.Domain.Entities.Api;
using ETLClientOpinionAnalysis.Core.Domain.Entities.Csv;
using ETLClientOpinionAnalysis.Core.Domain.Entities.Db;
using ETLClientOpinionAnalysis.Core.Domain.Entities.Dwh.Dimensions;
using ETLClientOpinionAnalysis.Core.Domain.Interfaces;
using ETLClientOpinionAnalysis.Core.Domain.Settings;
using ETLClientOpinionAnalysis.Infrastructure.Extractors.Api;
using ETLClientOpinionAnalysis.Infrastructure.Extractors.Csv;
using ETLClientOpinionAnalysis.Infrastructure.Extractors.Db;
using ETLClientOpinionAnalysis.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ETLClientOpinionAnalysis.Infrastructure
{
    public static class ServicesRegistration
    {
        public static void AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            #region Configurations
            services.Configure<CsvSettings>(configuration.GetSection("Csv"));
            services.Configure<StagingSettings>(configuration.GetSection("Staging"));
            services.Configure<DatabaseSettings>(configuration.GetSection("Database"));
            services.Configure<ApiSettings>(configuration.GetSection("Api"));
            services.Configure<WarehouseSettings>(configuration.GetSection("Warehouse"));
            #endregion

            #region Http Clients
            services.AddHttpClient(ApiExtractor.HttpClientName, (serviceProvider, client) =>
            {
                var settings = serviceProvider.GetRequiredService<IOptions<ApiSettings>>().Value;

                client.BaseAddress = new Uri(settings.BaseUrl.TrimEnd('/') + "/");
                client.DefaultRequestHeaders.Add("X-Api-Key", settings.ApiKey);
            });
            #endregion

            #region Extractors
            services.AddTransient<IExtractor<Survey>, CsvExtractor>();
            services.AddTransient<IExtractor<WebReview>, WebReviewExtractor>();
            services.AddTransient<IExtractor<Client>, ClientExtractor>();
            services.AddTransient<IExtractor<Product>, ProductExtractor>();
            services.AddTransient<IExtractor<SocialComment>, ApiExtractor>();
            #endregion

            #region Repositories
            services.AddTransient<IStagingRepository, JsonStagingRepository>();
            services.AddTransient<IDimensionRepository<DimSentiment>, SentimentRepository>();
            services.AddTransient<IDimensionRepository<DimSource>, SourceRepository>();
            services.AddTransient<IDimensionRepository<DimDate>, DateRepository>();
            services.AddTransient<IDimensionRepository<DimProduct>, ProductRepository>();
            services.AddTransient<IDimensionRepository<DimClient>, ClientRepository>();
            #endregion
        }
    }
}
