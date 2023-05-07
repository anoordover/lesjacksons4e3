using System.Text.Json;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;

namespace CommandsService.EventProcessing;

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMapper _mapper;

    public EventProcessor(IServiceScopeFactory scopeFactory,
        IMapper mapper)
    {
        _scopeFactory = scopeFactory;
        _mapper = mapper;
    }
    
    public void ProcessEvent(string message)
    {
        var eventType = DetermineEventType(message);
        switch (eventType)
        {
            case EventType.PlatformPublished:
                Console.WriteLine("-->");
                break;
            default:
                break;
        }
    }

    private void AddPlatform(string platformPublishedMessage)
    {
        using var scope = _scopeFactory.CreateScope();
        var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();
    }
    private static EventType DetermineEventType(string notificationMessage)
    {
        Console.WriteLine("--> Determining Event");
        var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);
        switch (eventType.Event)
        {
            case "Platform_Published":
                Console.WriteLine("--> Platform Published Event Detected");
                return EventType.PlatformPublished;
            default:
                Console.WriteLine("--> Could not detemine event type");
                return EventType.Undetermined;
        }
    }
}

enum EventType
{
    PlatformPublished,
    Undetermined
}
