using FastEndpoints;
using WebScrappingDemo.Common.Dtos.Requests.OutageSchedule;
using WebScrappingDemo.Controllers.OutageScheduleEndpoints.Group;
using WebScrappingDemo.Services;

namespace WebScrappingDemo.Controllers.OutageScheduleEndpoints;

[HttpGet("{sessionId}/parsed-table")]
[Group<OutageScheduleRouteGroup>]
public class GetParsedTableEndpoint : Endpoint<GetParsedTableRequest, List<Domain.Entities.OutageScheduleDay>>
{
    private readonly OutageScheduleService _outageScheduleService;

    public GetParsedTableEndpoint(OutageScheduleService outageScheduleService)
    {
        _outageScheduleService = outageScheduleService;
    }

    public override async Task HandleAsync(GetParsedTableRequest r, CancellationToken c)
    {
        List<Domain.Entities.OutageScheduleDay> result = await _outageScheduleService.GetOutageScheduleAsync(r.SessionId);

        await SendAsync(result);
    }
}
