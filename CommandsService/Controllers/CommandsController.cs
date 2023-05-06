using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;

[Route("api/c/platforms/{platformId:int}/[controller]")]
[ApiController]
public class CommandsController : ControllerBase
{
    private readonly ICommandRepo _commandRepo;
    private readonly IMapper _mapper;

    public CommandsController(ICommandRepo commandRepo,
        IMapper mapper)
    {
        _commandRepo = commandRepo;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> GetCommandsForPlatform(int platformId,
        CancellationToken cancellationToken)
    {
        Console.WriteLine($"--> Hit GetCommandsForPlatform: {platformId}");
        if (!await _commandRepo.PlatformExists(platformId, cancellationToken))
        {
            return NotFound();
        }
        var commandItems = await _commandRepo.GetCommandsForPlatform(platformId,
            cancellationToken);
        return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commandItems));
    }

    [HttpGet("{commandId:int}", Name="GetCommandForPlatform")]
    public async Task<IActionResult> GetCommandForPlatform(int platformId,
        int commandId,
        CancellationToken cancellationToken)
    {
        Console.WriteLine($"--> Hit GetCommand: {platformId} / {commandId}");
        if (!await _commandRepo.PlatformExists(platformId,
                cancellationToken))
        {
            return NotFound();
        }
        var command = await _commandRepo.GetCommand(
            platformId,
            commandId,
            cancellationToken);
        if (command == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<CommandReadDto>(command));    
    }

    [HttpPost]
    public async Task<IActionResult> CreateCommandForPlatform(
        int platformId,
        CommandCreateDto command,
        CancellationToken cancellationToken)
    {
        Console.WriteLine($"--> Hit CreateCommandForPlatform: {platformId}");
        if (!await _commandRepo.PlatformExists(platformId,
                cancellationToken))
        {
            return NotFound();
        }
        var commandItem = _mapper.Map<Command>(command);
        _commandRepo.CreateCommand(platformId, commandItem);
        await _commandRepo.SaveChanges(cancellationToken);
        return CreatedAtRoute(nameof(GetCommandForPlatform), 
            new {platformId, commandId = commandItem.Id},
            _mapper.Map<CommandReadDto>(commandItem));
    }
}