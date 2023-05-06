using System.ComponentModel.DataAnnotations;

namespace CommandsService.Dtos;

public class CommandReadDto
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string HowTo { get; set; }
    [Required]
    public string CommandLine { get; set; }
    [Required]
    public int PlatformId { get; set; }
}