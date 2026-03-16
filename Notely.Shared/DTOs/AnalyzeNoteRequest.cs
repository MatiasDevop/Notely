namespace Notely.Shared.DTOs;

public record AnalyzeNoteRequest(Guid NoteId, string Title, string Content);