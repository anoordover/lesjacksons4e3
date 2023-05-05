using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepo _repository;
    private readonly IMapper _mapper;

    public PlatformsController(IPlatformRepo repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
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
        return CreatedAtAction(nameof(GetPlatformById), 
            new {platformReadDto.Id}, 
            platformReadDto);
    }
}