using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ETLClientOpinionAnalysis.Core.Domain.Entities.Db;
using ETLClientOpinionAnalysis.Core.Domain.Settings;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ETLClientOpinionAnalysis.Infrastructure.Extractors.Db
{
    public class ProductExtractor : SqlExtractorBase<Product>
    {
        public ProductExtractor(IOptions<DatabaseSettings> settings, ILogger<ProductExtractor> logger)
            : base(settings, logger) { }

        public override string SourceName => "products";

        protected override string Query => @"
            SELECT p.Id, p.Name, c.Name AS CategoryName
            FROM dbo.Products p
            JOIN dbo.ProductCategories c ON c.Id = p.ProductCategoryId;";

        protected override Product Map(SqlDataReader reader) => new()
        {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            CategoryName = reader.GetString(2)
        };
    }
}
