using Microsoft.AspNetCore.Mvc;
using WebScrappingDemo.Common.Dtos;
using WebScrappingDemo.Services;

namespace WebScrappingDemo.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    public async Task<FileDto> GetScreenshot([FromRoute] string sessionId, [FromQuery] string cityName)
    {
        byte[] bytes = await _outageScheduleService.GetScreenshotOutageScheduleAsync(sessionId, cityName);

        return new FileDto
        {
            Bytes = bytes,
            Extension = "jpeg",
            MediaType = "image/jpeg",
            Name = $"OutageSchedule_{DateTime.UtcNow:yyyy-dd-MM-HH-mm-ss}.jpeg",
        };
    }

    [HttpGet("{sessionId}/parsed-table")]
    public async Task<IActionResult> GetParsedTable([FromRoute] string sessionId)
    {
        List<Domain.Entities.OutageScheduleDay> result = await _outageScheduleService.GetOutageScheduleAsync(sessionId);

        return Ok(result);
    }
}