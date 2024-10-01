using FastEndpoints;
namespace WebScrappingDemo.Common.Dtos.Requests.OutageSchedule;

public class GetScreenshotRequest
{
    public string SessionId { get; set; }

    [QueryParam]
    public string CityName { get; set; }
}
