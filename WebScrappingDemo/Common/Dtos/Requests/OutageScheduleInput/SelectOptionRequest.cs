namespace WebScrappingDemo.Common.Dtos.Requests.OutageScheduleInput;
using FastEndpoints;

public record SelectOptionRequest
{
    public string SessionId { get; set; }

    [FromBody] 
    public string OptionIndex { get; set; }
}
