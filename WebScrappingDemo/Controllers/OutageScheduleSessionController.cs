using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebScrappingDemo.Services;

namespace WebScrappingDemo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OutageScheduleSessionController : ControllerBase
{
    private readonly OutageScheduleService _outageScheduleService;

    public OutageScheduleSessionController(OutageScheduleService outageScheduleService)
    {
        _outageScheduleService = outageScheduleService;
    }

    [HttpGet("init-session")]
    public async Task<object> InitSession()
    {
        return await _outageScheduleService.InitSession();
    }

    [HttpGet("{sessionId}")]
    public async Task<object?> GetSession([FromRoute] string sessionId)
    {
        return await _outageScheduleService.TryGetSession(sessionId);
    }
}
