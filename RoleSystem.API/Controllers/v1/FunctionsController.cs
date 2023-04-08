using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoleSystem.Contracts.DTOs;
using RoleSystem.Core.Interfaces.Services;
using System.Net;

namespace RoleSystem.API.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class FunctionsController : ControllerBase
{
    private readonly IFunctionService service;

    public FunctionsController(IFunctionService service)
    {
        this.service = service;
    }

    [Authorize(Policy = "RoleSystem.Functions.Get")]
    [HttpGet]
    public async Task<IActionResult> GetFunctions()
    {
        var response = await service.GetFunctions();
        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [Authorize(Policy = "RoleSystem.Functions.Get")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFunction(int id)
    {
        var response = await service.GetFunction(id);
        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [Authorize(Policy = "RoleSystem.Functions.Create")]
    [HttpPost]
    public async Task<IActionResult> CreateFunction([FromBody] FunctionDTO function)
    {
        var response = await service.CreateFunction(function);
        if (!response.Success)
        {
            return BadRequest(response);
        }

        return StatusCode((int)HttpStatusCode.Created, response);
    }

    [Authorize(Policy = "RoleSystem.Functions.Update")]
    [HttpPut]
    public async Task<IActionResult> UpdateFunction([FromBody] FunctionDTO function)
    {
        var response = await service.UpdateFunction(function);
        if(!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [Authorize(Policy = "RoleSystem.Functions.Delete")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFunction(int id)
    {
        var response = await service.DeleteFunction(id);
        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }
}
