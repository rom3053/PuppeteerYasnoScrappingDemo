using WebScrappingDemo.Common.Dtos;
using WebScrappingDemo.Common.Mapping.DropdownOption;
using WebScrappingDemo.Domain.Enums;
using WebScrappingDemo.Domain.PuppeteerModels.OutageModels;

namespace WebScrappingDemo.Services;

public class OutageScheduleService
{
    private readonly PuppeteerService _puppeteerService;
    private readonly BrowserSessionStorage _browserSessionStorage;

    public OutageScheduleService(PuppeteerService puppeteerService,
        BrowserSessionStorage browserSessionStorage)
    {
        _puppeteerService = puppeteerService;
        _browserSessionStorage = browserSessionStorage;
    }

    public async Task<object?> TryGetSession(string sessionId)
    {
        bool isExist = _browserSessionStorage.Sessions.TryGetValue(sessionId, out BrowserSession? session);

        if (isExist)
        {
            return new
            {
                session.SessionId,
                session.CurrentInputStep,
                session.CreatedAt,
                DropdownOptionDtos = session.DropdownOptions.MapToDto(),
            };
        }

        return default;
    }

    // Step 0
    public async Task<object> InitSession()
    {
        BrowserSession session = await _puppeteerService.InitBrowserSession();
        _browserSessionStorage.Sessions.TryAdd(session.SessionId, session);

        return new
        {
            sessionId = session.SessionId,
            regionOptions = new string[] { "Київ", "Дніпро" },
        };
    }

    // Step 1
    public async Task SelectRegion(string sessionId, string region)
    {
        var session = GetAndExtendSession(sessionId);

        await PuppeteerService.SelectRegion(session.Page, region);
        session.CurrentInputStep++;
        return;
    }

    // Step 2
    public async Task<List<DropdownOptionDto>> InputCity(string sessionId, string city)
    {
        var session = GetAndExtendSession(sessionId);

        List<DropdownOption> options = await PuppeteerService.InputCityAndGetOptions(session.Page, city);
        session.DropdownOptions = options;
        session.CurrentInputStep++;
        return options.MapToDto();
    }

    // Step 4
    public async Task<List<DropdownOptionDto>> InputStreet(string sessionId, string street)
    {
        var session = GetAndExtendSession(sessionId);

        List<DropdownOption> options = await PuppeteerService.InputStreetAndGetOptions(session.Page, street);
        session.DropdownOptions = options;
        session.CurrentInputStep++;
        return options.MapToDto();
    }

    // Step 6
    public async Task<List<DropdownOptionDto>> InputHouseNumber(string sessionId, string houseNumber)
    {
        var session = GetAndExtendSession(sessionId);

        List<DropdownOption> options = await PuppeteerService.InputHouseNumberAndGetOptions(session.Page, houseNumber);
        session.DropdownOptions = options;
        session.CurrentInputStep++;
        return options.MapToDto();
    }

    // Step 3,5,7
    public async Task<SelectedDropdownOptionDto> SelectOption(string sessionId, string userOption)
    {
        DropdownOption? optionNode = default;

        var session = GetAndExtendSession(sessionId);
        bool isIndex = int.TryParse(userOption, out int optionIndex);

        if (isIndex)
        {
            optionNode = session.DropdownOptions.ElementAtOrDefault(optionIndex);
            await optionNode.SelectAndClickAsync();
        }
        
        //need to set current step
        session.CurrentInputStep++;

        var selectedType = session.CurrentInputStep switch
        {
            OutageInputStep.Step_3 => SelectedOutageInputType.SelectedCity,
            OutageInputStep.Step_5 => SelectedOutageInputType.SelectedStreet,
            OutageInputStep.Step_7 => SelectedOutageInputType.SelectedHouseNumber,
        };


        //return data of current selected step
        return new SelectedDropdownOptionDto
        {
            Index = optionIndex,
            Text = optionNode?.Text,
            SelectedOutageInputType = selectedType,
        };
    }

    // Result 1
    public async Task<List<Domain.Entities.OutageScheduleDay>> GetOutageScheduleAsync(string sessionId)
    {
        var session = GetAndExtendSession(sessionId);

        List<Domain.Entities.OutageScheduleDay> result = await PuppeteerService.GetOutageScheduleAsync(session.Page);
        return result;
    }

    // Result 2
    public async Task<byte[]> GetScreenshotOutageScheduleAsync(string sessionId, string cityName)
    {
        var session = GetAndExtendSession(sessionId);

        byte[] result = await PuppeteerService.GetScreenshotOfScheduleWithAddressTitle(session.Page, cityName);
        return result;
    }

    private BrowserSession? GetAndExtendSession(string sessionId)
    {
        bool isExist = _browserSessionStorage.Sessions.TryGetValue(sessionId, out BrowserSession? session);

        if (isExist)
        {
            session!.ExtendSessionTime();
        }

        return session;
    }
}
