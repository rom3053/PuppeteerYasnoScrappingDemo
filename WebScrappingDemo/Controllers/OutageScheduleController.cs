using Microsoft.AspNetCore.Mvc;
using WebScrappingDemo.Services;

namespace WebScrappingDemo.Controllers;

[ApiController]
[Route("[controller]")]
public class OutageScheduleController : ControllerBase
{
    private readonly ILogger<OutageScheduleController> _logger;
    private readonly OutageScheduleService _outageScheduleService;

    public OutageScheduleController(ILogger<OutageScheduleController> logger,
        OutageScheduleService outageScheduleService)
    {
        _logger = logger;
        _outageScheduleService = outageScheduleService;
    }

    [HttpGet("{sessionId}/screenshot")]
    public async Task<IActionResult> GetScreenshot([FromRoute] string sessionId)
    {

        byte[] bytes = await _outageScheduleService.GetScreenshotOutageScheduleAsync(sessionId);
        return File(bytes, "image/jpeg", $"OutageSchedule_{DateTime.UtcNow:yyyy-dd-MM-HH-mm-ss}.jpeg");
    }

    [HttpGet("{sessionId}/parsed-table")]
    public async Task<IActionResult> GetParsedTable([FromRoute] string sessionId)
    {
        List<Domain.Entities.OutageScheduleDay> result = await _outageScheduleService.GetOutageScheduleAsync(sessionId);

        return Ok(result);
    }
}