using System.ComponentModel.DataAnnotations;

namespace HsoPkipt.ViewModels.Project;

public class CreateProjectVM
{
    [Required]
    [MaxLength(200)]
    public string Title { get; init; } = default!;

    [MaxLength(500)]
    public string? ShortDescription { get; init; }

    [Required]
    public string Content { get; init; } = default!;

    public string? ImageUrl { get; init; }
}