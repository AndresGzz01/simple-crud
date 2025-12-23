using System.ComponentModel.DataAnnotations;

namespace simple_crud.Api.Models;

public class Argon2Options
{
    [Required]
    [Range(1, int.MaxValue)]
    public int MemorySize { get; init; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Iterations { get; init; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Parallelism { get; init; }

    [Required]
    [Range(1, int.MaxValue)]
    public int SaltSize { get; init; }

    [Required]
    [Range(1, int.MaxValue)]
    public int HashSize { get; init; }
}
