using ETLClientOpinionAnalysis.Core.Application.Interfaces;
using ETLClientOpinionAnalysis.Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ETLClientOpinionAnalysis.Core.Application
{
    public static class ServicesRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            #region Services
            services.AddTransient<IExtractionService, ExtractionService>();
            #endregion
        }
    }
}
