using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;

[Route("api/c/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly ICommandRepo _commandRepo;
    private readonly IMapper _mapper;

    public PlatformsController(ICommandRepo commandRepo,
        IMapper mapper)
    {
        _commandRepo = commandRepo;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetPlatforms(CancellationToken cancellationToken)
    {
        Console.WriteLine("--> Getting platforms from CommandService");
        var platformItems = await _commandRepo.GetAllPlatforms(cancellationToken);
        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
    }
    
    [HttpPost]
    public Task<IActionResult> TestInboundConnection(CancellationToken cancellationToken)
    {
        Console.WriteLine("--> Inboud POST # Command service");
        return Task.FromResult((IActionResult)Ok("Inbound test of from Platforms Controller"));
    }
}