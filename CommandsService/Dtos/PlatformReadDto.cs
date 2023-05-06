using System.ComponentModel.DataAnnotations;

namespace CommandsService.Dtos;

public class PlatformReadDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
}