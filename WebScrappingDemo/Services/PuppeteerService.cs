using System.Security.Cryptography;
using PuppeteerSharp;
using WebScrappingDemo.Common.Constants;
using WebScrappingDemo.Common.Utilities;
using WebScrappingDemo.Domain.Entities;
using WebScrappingDemo.Domain.Enums;
using WebScrappingDemo.Domain.PuppeteerModels.OutageModels;

namespace WebScrappingDemo.Services;

public class PuppeteerService
{
    static PuppeteerService()
    {
        new BrowserFetcher().DownloadAsync().Wait();
    }

    public async Task<BrowserSession> InitBrowserSession()
    {
        IBrowser browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Args = new string[] { "--no-sandbox" },
            Headless = true,
            DefaultViewport = new ViewPortOptions() { Height = 1024, Width = 1366 }
        });

        IPage page = await browser.NewPageAsync();
        await page.GoToAsync("https://yasno.com.ua/schedule-turn-off-electricity");

        BrowserSession session = new BrowserSession
        {
            SessionId = Guid.NewGuid().ToString(),
            Browser = browser,
            Page = page
        };

        return session;
    }

    public static async Task SelectRegion(IPage page, string userInput)
    {
        await page.WaitForSelectorAsync("button.region-card");
        IElementHandle[] regions = await page.QuerySelectorAllAsync("button.region-card");

        foreach (IElementHandle? region in regions)
        {
            string innerText = await region.EvaluateFunctionAsync<string>("element => element.innerText");

            if (innerText == userInput)
            {
                await region.ClickAsync();
            }
        }
    }

    public static async Task<List<DropdownOption>> InputCityAndGetOptions(IPage page, string userCityInput)
    {
        await page.WaitForSelectorAsync("#vs6__combobox");
        await page.ReloadAsync();
        await page.WaitForSelectorAsync("#vs6__combobox");
        await page.FocusAsync("#vs6__combobox > div.vs__selected-options > input");
        await Task.Delay(RandomNumberGenerator.GetInt32(500, 1000));
        await page.ClickAsync("#vs6__combobox > div.vs__selected-options > input");

        //input city
        await page.Keyboard.SendCharacterAsync(userCityInput);
        await Task.Delay(RandomNumberGenerator.GetInt32(500, 1000));
        List<DropdownOption> cityOptions = await GetDropdownOptionsAsync(page, "#vs6__listbox");
        return cityOptions;
    }

    public static async Task<List<DropdownOption>> InputStreetAndGetOptions(IPage page, string userStreetInput)
    {
        await page.WaitForSelectorAsync("#vs7__combobox");
        await page.FocusAsync("#vs7__combobox > div.vs__selected-options > input");
        await Task.Delay(RandomNumberGenerator.GetInt32(500, 1000));
        await page.ClickAsync("#vs7__combobox > div.vs__selected-options > input");
        //input Street
        await page.Keyboard.SendCharacterAsync(userStreetInput);
        await Task.Delay(RandomNumberGenerator.GetInt32(500, 1000));
        List<DropdownOption> streetOptions = await GetDropdownOptionsAsync(page, "#vs7__listbox");
        return streetOptions;
    }

    public static async Task<List<DropdownOption>> InputHouseNumberAndGetOptions(IPage page, string userHouseNumberInput)
    {
        await page.WaitForSelectorAsync("#vs8__combobox");
        await page.FocusAsync("#vs8__combobox > div.vs__selected-options > input");
        await Task.Delay(RandomNumberGenerator.GetInt32(500, 1000));
        await page.ClickAsync("#vs8__combobox > div.vs__selected-options > input");
        //input HouseNumber
        await page.Keyboard.SendCharacterAsync(userHouseNumberInput);
        await Task.Delay(RandomNumberGenerator.GetInt32(500, 1000));
        List<DropdownOption> houseNumberOptions = await GetDropdownOptionsAsync(page, "#vs8__listbox");
        return houseNumberOptions;
    }

    public static async Task<List<OutageScheduleDay>> GetOutageScheduleAsync(IPage page)
    {
        await page.WaitForSelectorAsync("div.schedule");

        IElementHandle scheduleGrid = await page.QuerySelectorAsync("div.form-wr.s-meter.electricity-outages-schedule > div > div > div.right-side > div.schedule");
        //IElementHandle scheduleGrid = await page.QuerySelectorAsync("div.schedule");
        IElementHandle[] scheduleGridCols = await scheduleGrid.QuerySelectorAllAsync(".col");

        int hoursCol = 24;
        int colStartIndex = 26;

        Task<OutageScheduleDay> monday = Task.Run(() => GetOutageSchedulePerDay(26, 26 + hoursCol, scheduleGridCols, OutageScheduleConstants.WeekDays.MONDAY));
        Task<OutageScheduleDay> tuesday = Task.Run(() => GetOutageSchedulePerDay(colStartIndex * 2 - 1, colStartIndex * 2 - 1 + hoursCol, scheduleGridCols, OutageScheduleConstants.WeekDays.TUESDAY));
        Task<OutageScheduleDay> wednesday = Task.Run(() => GetOutageSchedulePerDay(colStartIndex * 3 - 2, colStartIndex * 3 - 2 + hoursCol, scheduleGridCols, OutageScheduleConstants.WeekDays.WEDNESDAY));
        Task<OutageScheduleDay> thursday = Task.Run(() => GetOutageSchedulePerDay(colStartIndex * 4 - 3, colStartIndex * 4 - 3 + hoursCol, scheduleGridCols, OutageScheduleConstants.WeekDays.THURSDAY));
        Task<OutageScheduleDay> friday = Task.Run(() => GetOutageSchedulePerDay(colStartIndex * 5 - 4, colStartIndex * 5 - 4 + hoursCol, scheduleGridCols, OutageScheduleConstants.WeekDays.FRIDAY));
        Task<OutageScheduleDay> saturday = Task.Run(() => GetOutageSchedulePerDay(colStartIndex * 6 - 5, colStartIndex * 6 - 5 + hoursCol, scheduleGridCols, OutageScheduleConstants.WeekDays.SATURDAY));
        Task<OutageScheduleDay> sunday = Task.Run(() => GetOutageSchedulePerDay(colStartIndex * 7 - 6, colStartIndex * 7 - 6 + hoursCol, scheduleGridCols, OutageScheduleConstants.WeekDays.SUNDAY));

        OutageScheduleDay[] scheduleDays = await Task.WhenAll(monday, tuesday, wednesday, thursday, friday, saturday, sunday);
        List<OutageScheduleDay> result = scheduleDays.OrderBy(x => x.NumberWeekDay).ToList();

        return result;
    }

    public static async Task<byte[]> GetScreenshotOfScheduleWithAddressTitle(IPage page, string userCityInput)
    {
        IElementHandle? scheduleTitle;
        //Get Title
        var isScheduleTitle = await page.WaitForSelectorAsync("div.form-wr.s-meter.electricity-outages-schedule > div > div > div.right-side > div.address-line", options: new WaitForSelectorOptions { Hidden = true });
        if (isScheduleTitle is not null)
        {
            scheduleTitle = await page.QuerySelectorAsync("div.form-wr.s-meter.electricity-outages-schedule > div > div > div.right-side > div.address-line");

            //Change Attrs and Styles
            await scheduleTitle.EvaluateFunctionAsync("element => element.setAttribute('style','text-align: center;')");
            await scheduleTitle.EvaluateFunctionAsync($"element => element.innerText = '{userCityInput}'+', '+element.innerText");
            await scheduleTitle.EvaluateFunctionAsync("element => element.setAttribute('class','description-title')");

            await RemoveActiveStatusFromScheduleCols(page);

            // Copy Schedule into Title
            await MoveElementIntoAnother(page,
                sourceSelector: "div.form-wr.s-meter.electricity-outages-schedule > div > div > div.right-side > div.schedule",
                targetSelector: "div.form-wr.s-meter.electricity-outages-schedule > div > div > div.right-side > div.description-title");
        }
        else
        {
            scheduleTitle = await page.QuerySelectorAsync("div.form-wr.s-meter.electricity-outages-schedule > div > div > div.right-side > div.description-title");
        }

        // Take screenshot
        byte[] imageBytes = await GetByteScreenshot(scheduleTitle);
        return imageBytes;
    }

    /// <summary>
    /// Select cols outage of specific day and convert to system object
    /// </summary>
    /// <param name="indexFrom">Start scheduleGridCols index of cols</param>
    /// <param name="indexTo">End scheduleGridCols index of cols</param>
    /// <param name="scheduleGridCols">All cols of schedule</param>
    /// <param name="day">Convert parsed to system day of Schedule</param>
    /// <exception cref="NotImplementedException"></exception>
    private static OutageScheduleDay GetOutageSchedulePerDay(int indexFrom, int indexTo, IElementHandle[] scheduleGridCols, string dayName)
    {
        OutageScheduleDay schedulePerDay = new OutageScheduleDay(dayName, WeekDayConverter.GetDayOfWeekInt(dayName));
        int indexOfTime = 0;

        for (int i = indexFrom; i < indexTo; i++)
        {
            OutageStatus status = scheduleGridCols[i].RemoteObject.Description switch
            {
                var powerOff when powerOff.Contains(OutageScheduleConstants.HTML_Tags.OutageStatus.DEFINITE_OUTAGE) => OutageStatus.PowerOff,
                var powerPossibleOn when powerPossibleOn.Contains(OutageScheduleConstants.HTML_Tags.OutageStatus.POSSIBLE_OUTAGE) => OutageStatus.PowerPossibleOn,
                var powerOn when !powerOn.Contains(OutageScheduleConstants.HTML_Tags.OutageStatus.DEFINITE_OUTAGE) &&
                        !powerOn.Contains(OutageScheduleConstants.HTML_Tags.OutageStatus.DEFINITE_OUTAGE) => OutageStatus.PowerOn,
                _ => throw new NotImplementedException(),
            };

            schedulePerDay.OutageHours[indexOfTime].Status = status;
            indexOfTime++;
        }

        return schedulePerDay;
    }

    private static async Task RemoveActiveStatusFromScheduleCols(IPage page)
    {
        IElementHandle scheduleGrid = await page.QuerySelectorAsync("div.form-wr.s-meter.electricity-outages-schedule > div > div > div.right-side > div.schedule");
        IElementHandle[] todayActiveElements = await scheduleGrid.QuerySelectorAllAsync(".col.active");
        foreach (IElementHandle? element in todayActiveElements)
        {
            await element.EvaluateFunctionAsync("element => element.classList.remove('active')");
        }
    }

    private static async Task<byte[]> GetByteScreenshot(IElementHandle scheduleTitle, int quality = 100, ScreenshotType type = ScreenshotType.Jpeg)
    {
        return await scheduleTitle.ScreenshotDataAsync(new ElementScreenshotOptions { Quality = quality, Type = type });
    }

    private static async Task MoveElementIntoAnother(IPage page, string sourceSelector, string targetSelector)
    {
        // Define the JavaScript code to move one div into another
        string script = $$"""
                var sourceDiv = document.querySelector('{{sourceSelector}}');
                    var targetDiv = document.querySelector('{{targetSelector}}');
                    if (sourceDiv && targetDiv) {
                        targetDiv.appendChild(sourceDiv);
                    }
                """;
        // Execute the script
        await page.EvaluateExpressionAsync(script);
    }

    private static async Task<List<DropdownOption>> GetDropdownOptionsAsync(IPage page, string optionNodeId)
    {
        List<DropdownOption> result = new List<DropdownOption>();

        IElementHandle elementList = await page.QuerySelectorAsync(optionNodeId);

        IElementHandle[] listOptions = await elementList.QuerySelectorAllAsync("[role='option']");

        foreach (IElementHandle? option in listOptions)
        {
            //# vs2__option-0 > span > span
            string optionTextValue = await option.EvaluateFunctionAsync<string>("element => element.textContent");
            result.Add(new DropdownOption
            {
                Text = optionTextValue,
                Node = option,
            });
        }

        return result;
    }
}
