using WebScrappingDemo.Common.Dtos.Requests.OutageScheduleSession;
using WebScrappingDemo.Controllers.OutageScheduleSessionEndpoints.Base;
using WebScrappingDemo.Services;

namespace WebScrappingDemo.Controllers.OutageScheduleSessionEndpoints;

public class InitSessionEndpoint : OutageScheduleSessionEndpointWithoutRequestBase<object>
{
    public override void Configure()
    {
        Get("init-session");
        base.Configure();
    }

    public InitSessionEndpoint(OutageScheduleService outageScheduleService) : base(outageScheduleService) { }

    public override async Task HandleAsync(CancellationToken c)
    {
        await SendAsync(await _outageScheduleService.InitSession());
    }
}

public class GetSessionEndpoint : OutageScheduleSessionEndpointBase<GetSessionRequest>
{
    public override void Configure()
    {
        Get("{sessionId}");
        base.Configure();
    }

    public GetSessionEndpoint(OutageScheduleService outageScheduleService) : base(outageScheduleService) { }

    public override async Task HandleAsync(GetSessionRequest r, CancellationToken c)
    {
        await SendAsync(await _outageScheduleService.TryGetSession(r.SessionId));
    }
}
