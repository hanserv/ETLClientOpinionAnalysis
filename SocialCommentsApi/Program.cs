using SocialCommentsApi.Extensions;
using SocialCommentsApi.Interfaces;
using SocialCommentsApi.Middleware;
using SocialCommentsApi.Repositories;
using SocialCommentsApi.Services;

var builder = WebApplication.CreateBuilder(args);

if (string.IsNullOrWhiteSpace(builder.Configuration["ApiKey"]))
{
    throw new InvalidOperationException("Falta configurar 'ApiKey' (user-secrets).");
}

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithApiKey();

builder.Services.AddScoped<ISocialCommentRepository, SocialCommentRepository>();
builder.Services.AddScoped<ISocialCommentService, SocialCommentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<ApiKeyMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
