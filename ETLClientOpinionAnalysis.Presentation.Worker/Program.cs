using ETLClientOpinionAnalysis.Core.Application;
using ETLClientOpinionAnalysis.Infrastructure;
using ETLClientOpinionAnalysis.Presentation.Worker;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();
builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer(builder.Configuration);

var host = builder.Build();
host.Run();