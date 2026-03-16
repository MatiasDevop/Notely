namespace Notely.Shared.DTOs;

public record TagResponse(
    Guid Id,
    string Name,
    string Color,
    DateTime CreatedAt
);
