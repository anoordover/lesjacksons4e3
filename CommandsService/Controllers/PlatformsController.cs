using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;

[Route("api/c/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    public PlatformsController()
    {
    }

    [HttpPost]
    public Task<IActionResult> TestInboundConnection(CancellationToken cancellationToken)
    {
        Console.WriteLine("--> Inboud POST # Command service");
        return Task.FromResult((IActionResult)Ok("Inbound test of from Platforms Controller"));
    }
}