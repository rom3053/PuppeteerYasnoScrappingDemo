using FastEndpoints;
using WebScrappingDemo.Services;

namespace WebScrappingDemo.Controllers.OutageScheduleInputEndpoints.Base;

public abstract class OutageScheduleInputEndpointBase<T,R> : Endpoint<T, R>
{
    protected readonly OutageScheduleService _outageScheduleService;

    public override void Configure()
    {
        Group<OutageScheduleInputRouteGroup>();
        AllowAnonymous();
    }

    public OutageScheduleInputEndpointBase(OutageScheduleService outageScheduleService)
    {
        _outageScheduleService = outageScheduleService;
    }
}

public abstract class OutageScheduleInputEndpointBase<T> : Endpoint<T>
{
    protected readonly OutageScheduleService _outageScheduleService;

    public override void Configure()
    {
        Group<OutageScheduleInputRouteGroup>();
        AllowAnonymous();
    }

    public OutageScheduleInputEndpointBase(OutageScheduleService outageScheduleService)
    {
        _outageScheduleService = outageScheduleService;
    }
}
