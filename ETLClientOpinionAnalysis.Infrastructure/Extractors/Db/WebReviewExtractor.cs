using ETLClientOpinionAnalysis.Core.Domain.Entities.Db;
using ETLClientOpinionAnalysis.Core.Domain.Settings;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ETLClientOpinionAnalysis.Infrastructure.Extractors.Db
{
    public class WebReviewExtractor : SqlExtractorBase<WebReview>
    {
        public WebReviewExtractor(IOptions<DatabaseSettings> settings, ILogger<WebReviewExtractor> logger)
            : base(settings, logger) { }

        public override string SourceName => "web_reviews";

        protected override string Query => @"
            SELECT Id, ClientId, ProductId, [Date], Comment, Rating
            FROM dbo.WebReviews;";

        protected override WebReview Map(SqlDataReader reader) => new()
        {
            Id = reader.GetInt32(0),
            ClientId = reader.GetInt32(1),
            ProductId = reader.GetInt32(2),
            Date = reader.GetFieldValue<DateOnly>(3),
            Comment = reader.GetString(4),
            Rating = reader.GetInt32(5)
        };
    }
}
