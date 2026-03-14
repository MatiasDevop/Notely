using Microsoft.EntityFrameworkCore;
using Tags.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddDbContext<TagsDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("notely-tags")));
builder.EnrichNpgsqlDbContext<TagsDbContext>();

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    //Apply Ef Migrations
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<TagsDbContext>();
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();

app.Run();

