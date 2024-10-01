using FastEndpoints;
using WebScrappingDemo.Services;

namespace WebScrappingDemo.Controllers.OutageScheduleSessionEndpoints.Base;

public abstract class OutageScheduleSessionEndpointBase<T, R> : Endpoint<T, R>
{
    protected readonly OutageScheduleService _outageScheduleService;

    public override void Configure()
    {
        Group<OutageScheduleSessionRouteGroup>();
        AllowAnonymous();
    }

    public OutageScheduleSessionEndpointBase(OutageScheduleService outageScheduleService)
    {
        _outageScheduleService = outageScheduleService;
    }
}

public abstract class OutageScheduleSessionEndpointBase<T> : Endpoint<T>
{
    protected readonly OutageScheduleService _outageScheduleService;

    public override void Configure()
    {
        Group<OutageScheduleSessionRouteGroup>();
        AllowAnonymous();
    }

    public OutageScheduleSessionEndpointBase(OutageScheduleService outageScheduleService)
    {
        _outageScheduleService = outageScheduleService;
    }
}

public abstract class OutageScheduleSessionEndpointWithoutRequestBase<TResponse> : EndpointWithoutRequest<TResponse>
{
    protected readonly OutageScheduleService _outageScheduleService;

    public override void Configure()
    {
        Group<OutageScheduleSessionRouteGroup>();
        AllowAnonymous();
    }

    public OutageScheduleSessionEndpointWithoutRequestBase(OutageScheduleService outageScheduleService)
    {
        _outageScheduleService = outageScheduleService;
    }
}