using WebScrappingDemo.Common.Dtos.Requests.OutageScheduleInput;
using WebScrappingDemo.Controllers.OutageScheduleInputEndpoints.Base;
using WebScrappingDemo.Services;

namespace WebScrappingDemo.Controllers.OutageScheduleInputEndpoints;

public class SelectRegionEndpoint : OutageScheduleInputEndpointBase<SelectRegionRequest>
{
    public override void Configure()
    {
        Post("{sessionId}/step-1-select-region");
        base.Configure();
    }

    public SelectRegionEndpoint(OutageScheduleService outageScheduleService) : base(outageScheduleService) { }

    public override async Task HandleAsync(SelectRegionRequest r, CancellationToken c)
    {
        await _outageScheduleService.SelectRegion(r.SessionId, r.UserInput);

        await SendOkAsync(c);
    }
}

public class InputCityEndpoint : OutageScheduleInputEndpointBase<InputCityRequest, object>
{
    public override void Configure()
    {
        Post("{sessionId}/step-2-input-city");
        base.Configure();
    }
    public InputCityEndpoint(OutageScheduleService outageScheduleService) : base(outageScheduleService) { }

    public override async Task HandleAsync(InputCityRequest r, CancellationToken c)
    {
       await SendAsync( await _outageScheduleService.InputCity(r.SessionId, r.UserInput));
    }
}

public class InputStreetEndpoint : OutageScheduleInputEndpointBase<InputCityRequest, object>
{
    public override void Configure()
    {
        Post("{sessionId}/step-4-input-street");
        base.Configure();
    }

    public InputStreetEndpoint(OutageScheduleService outageScheduleService) : base(outageScheduleService) { }

    public override async Task HandleAsync(InputCityRequest r, CancellationToken c)
    {
        await SendAsync(await _outageScheduleService.InputStreet(r.SessionId, r.UserInput));
    }
}

public class InputHouseNumberEndpoint : OutageScheduleInputEndpointBase<InputHouseNumberRequest, object>
{
    public override void Configure()
    {
        Post("{sessionId}/step-6-input-house-number");
        base.Configure();
    }

    public InputHouseNumberEndpoint(OutageScheduleService outageScheduleService) : base(outageScheduleService) { }

    public override async Task HandleAsync(InputHouseNumberRequest r, CancellationToken c)
    {
        await SendAsync(await _outageScheduleService.InputHouseNumber(r.SessionId, r.UserInput));
    }
}

public class SelectOptionEndpoint : OutageScheduleInputEndpointBase<SelectOptionRequest, object>
{
    public override void Configure()
    {
        Post("{sessionId}/step-3-5-7-select-option");
        base.Configure();
    }

    public SelectOptionEndpoint(OutageScheduleService outageScheduleService) : base(outageScheduleService) { }

    public override async Task HandleAsync(SelectOptionRequest r, CancellationToken c)
    {
        await SendAsync(await _outageScheduleService.SelectOption(r.SessionId, r.OptionIndex));
    }
}