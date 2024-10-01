namespace WebScrappingDemo.Common.Dtos.Requests.OutageScheduleInput;
using FastEndpoints;

public abstract record OutageScheduleInputBaseRequest
{
    public string SessionId { get; set; }

    [FromBody]
    public string UserInput { get; set; }
}
