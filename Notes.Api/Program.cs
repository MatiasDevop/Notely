using Microsoft.EntityFrameworkCore;
using Notes.Api.Data;
using Notes.Api.Features.CreateNote;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddDbContext<NoteDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("notely-notes")));
builder.EnrichNpgsqlDbContext<NoteDbContext>();

builder.Services.AddHttpClient("TagsApi", client =>
{
    client.BaseAddress = new Uri("https+http://tags-api");
});

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    //Apply Ef Migrations
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<NoteDbContext>();
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();

app.MapPost("notes", CreateNoteEndpoint.CreateNote);

app.Run();

