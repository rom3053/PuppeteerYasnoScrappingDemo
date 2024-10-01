using FastEndpoints;
using WebScrappingDemo.Common.Dtos;
using WebScrappingDemo.Common.Dtos.Requests.OutageSchedule;
using WebScrappingDemo.Controllers.OutageScheduleEndpoints.Group;
using WebScrappingDemo.Services;


namespace WebScrappingDemo.Controllers.OutageScheduleEndpoints;

[HttpGet("{sessionId}/screenshot")]
[Group<OutageScheduleRouteGroup>]
public class GetScreenshotEndpoint : Endpoint<GetScreenshotRequest, FileDto>
{
    private readonly OutageScheduleService _outageScheduleService;

    public GetScreenshotEndpoint(OutageScheduleService outageScheduleService)
    {
        _outageScheduleService = outageScheduleService;
    }

    public override async Task HandleAsync(GetScreenshotRequest r, CancellationToken c)
    {
        byte[] bytes = await _outageScheduleService.GetScreenshotOutageScheduleAsync(r.SessionId, r.CityName);

        await SendAsync(new FileDto
        {
            Bytes = bytes,
            Extension = "jpeg",
            MediaType = "image/jpeg",
            Name = $"OutageSchedule_{DateTime.UtcNow:yyyy-dd-MM-HH-mm-ss}.jpeg",
        });
    }
}
