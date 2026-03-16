using Microsoft.AspNetCore.Mvc;
using Notely.Shared.DTOs;
using Tags.Api.Data;

namespace Tags.Api.Features.AnalyzeNote;

public static class AnalyzeNoteEndpoint
{

    public static async Task<IResult> AnalyzeNote(
        [FromBody] AnalyzeNoteRequest request,
        TagsDbContext context,
        ILogger<Program> logger)
    {
        try
        {
            //For noew implement simple tag analysis based on content keywords
            // Simulate tag analysis logic (replace with actual implementation)
            var tags = AnalyzeContentForTags(request.Title, request.Content);
            // Save tags to database
            var tagEntities = tags.Select(tag => new Tag
            {
                Id = Guid.NewGuid(),
                Name = tag.Name,
                Color = tag.Color,
                NoteId = request.NoteId,
                CreatedAtUtc = DateTime.UtcNow
            }).ToList();

            context.Tags.AddRange(tagEntities);
            await context.SaveChangesAsync();

            var response = new AnalyzeNoteResponse(request.NoteId, tags);

            return Results.Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error analyzing note for tags");
            return Results.Problem("An error occurred while analyzing the note for tags.");
        }
    }

    private static List<TagResponse> AnalyzeContentForTags(string title, string content)
    {
        var tags = new List<TagResponse>();

        var allText = $"{title} {content}".ToLowerInvariant();

        //Simple keyword-based tagging (replace with LLM in the future)
        var keywords = new Dictionary<string, (string name, string color)>
        {
            { "work", ("Work", "#FF5733") },
            { "personal", ("Personal", "#33FF57") },
            { "urgent", ("Urgent", "#FF3333") },
            { "idea", ("Idea", "#3357FF") },
            { "project", ("Project", "#FF33A1") },
            { "meeting", ("Meeting", "#33FFF5") },
            { "todo", ("To-Do", "#F5FF33") },
            { "important", ("Important", "#FF8C33") },
            { "reminder", ("Reminder", "#8C33FF") },
            { "note", ("Note", "#33FF8C") }

        };

        foreach (var keyword in keywords)
        {
            if (allText.Contains(keyword.Key))
            {
                tags.Add(new TagResponse(Guid.NewGuid(), keyword.Value.name, keyword.Value.color, DateTime.UtcNow));
            }
        }

        // If no tags are found, add a default "General" tag
        if (!tags.Any())
        {
            tags.Add(new TagResponse(Guid.NewGuid(), "General", "#CCCCCC", DateTime.UtcNow));
        }

        return tags;
    }
}
