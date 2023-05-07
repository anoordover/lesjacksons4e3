using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.AsyncDataServices;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.Http;

namespace PlatformService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepo _repository;
    private readonly IMapper _mapper;
    private readonly ICommandDataClient _commandDataClient;
    private readonly IMessageBusClient _messageBusClient;

    public PlatformsController(IPlatformRepo repository,
        IMapper mapper,
        ICommandDataClient commandDataClient,
        IMessageBusClient messageBusClient)
    {
        _repository = repository;
        _mapper = mapper;
        _commandDataClient = commandDataClient;
        _messageBusClient = messageBusClient;
    }

    [HttpGet]
    public async Task<IActionResult> GetPlatforms(CancellationToken cancellationToken)
    {
        Console.WriteLine("--> Getting Platforms...");
        var platformItems = await _repository.getAllPlatforms(cancellationToken);
        return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
    }

    [HttpGet("{id:int}", Name = "GetPlatformById")]
    public async Task<IActionResult> GetPlatformById(int id,
        CancellationToken cancellationToken)
    {
        Console.WriteLine("--> Get Platform");
        var platformItem = await _repository.GetPlatformById(id, cancellationToken);
        if (platformItem == null)
        {
            return NotFound();
        }
        return Ok(_mapper.Map<PlatformReadDto>(platformItem));
    }

    [HttpPost]
    public async Task<IActionResult> CreatePlatform(PlatformCreateDto platformCreateDto,
        CancellationToken cancellationToken)
    {
        var platform = _mapper.Map<Platform>(platformCreateDto);
        await _repository.CreatePlatform(platform, 
            cancellationToken);
        await _repository.SaveChanges(cancellationToken);
        var platformReadDto = _mapper.Map<PlatformReadDto>(platform);
        try
        {
            await _commandDataClient.SendPlatformToCommand(platformReadDto,
                cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not send synchronously: {e.Message}");
        }

        try
        {
            var platformPublishedDto = _mapper.Map<PlatformPublishedDto>(platformReadDto);
            platformPublishedDto.Event = "Platform_Published";
            _messageBusClient.PublishNewPlatform(platformPublishedDto);
        }
        catch (Exception e)
        {
            Console.WriteLine($"--> Could not send asynchronously: {e.Message}");
        }
        return CreatedAtAction(nameof(GetPlatformById), 
            new {platformReadDto.Id}, 
            platformReadDto);
    }
}