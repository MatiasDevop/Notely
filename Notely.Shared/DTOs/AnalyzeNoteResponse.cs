namespace Notely.Shared.DTOs;

public record AnalyzeNoteResponse(Guid NoteId, List<TagResponse> Tags);
