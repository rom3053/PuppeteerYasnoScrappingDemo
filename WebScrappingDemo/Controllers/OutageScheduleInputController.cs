﻿using Microsoft.AspNetCore.Mvc;
using WebScrappingDemo.Services;

namespace WebScrappingDemo.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OutageScheduleInputController : ControllerBase
{
    private readonly OutageScheduleService _outageScheduleService;

    public OutageScheduleInputController(OutageScheduleService outageScheduleService)
    {
        _outageScheduleService = outageScheduleService;
    }

    [HttpPost("{sessionId}/step-1-select-region")]
    public async Task<IActionResult> SelectRegion([FromRoute] string sessionId, string userInput)
    {
        await _outageScheduleService.SelectRegion(sessionId, userInput);
        return Ok();
    }

    [HttpPost("{sessionId}/step-2-input-city")]
    public async Task<object> InputCity([FromRoute] string sessionId, string userInput)
    {
        return await _outageScheduleService.InputCity(sessionId, userInput);
    }

    [HttpPost("{sessionId}/step-4-input-street")]
    public async Task<object> InputStreet([FromRoute] string sessionId, string userInput)
    {
        return await _outageScheduleService.InputStreet(sessionId, userInput);
    }

    [HttpPost("{sessionId}/step-6-input-house-number")]
    public async Task<object> InputHouseNumber([FromRoute] string sessionId, string userInput)
    {
        return await _outageScheduleService.InputHouseNumber(sessionId, userInput);
    }

    [HttpPost("{sessionId}/step-3-5-7-select-option")]
    public async Task<IActionResult> SelectOption([FromRoute] string sessionId, string optionIndex)
    {
        await _outageScheduleService.SelectOption(sessionId, optionIndex);
        return Ok();
    }
}