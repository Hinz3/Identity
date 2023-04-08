using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RoleSystem.Contracts.DTOs;
using RoleSystem.Core.Interfaces.Services;
using System.Net;

namespace RoleSystem.API.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IRoleService service;
    private readonly IRoleFunctionService functionService;
    private readonly IRoleUserService roleUserService;

    public RolesController(IRoleService service, IRoleFunctionService functionService, IRoleUserService roleUserService)
    {
        this.service = service;
        this.functionService = functionService;
        this.roleUserService = roleUserService;
    }

    [Authorize(Policy = "RoleSystem.Roles.Get")]
    [HttpGet]
    public async Task<IActionResult> GetRoles()
    {
        var response = await service.GetRoles();
        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [Authorize(Policy = "RoleSystem.Roles.Get")]
    [HttpGet("{roleId}")]
    public async Task<IActionResult> GetRole(int roleId)
    {
        var response = await service.GetRole(roleId);
        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [Authorize(Policy = "RoleSystem.Roles.Get")]
    [HttpGet("{roleId}/Users")]
    public async Task<IActionResult> GetRoleUsers(int roleId)
    {
        var response = await roleUserService.GetRoleUsers(roleId);
        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [Authorize(Policy = "RoleSystem.Roles.Create")]
    [HttpPost]
    public async Task<IActionResult> CreateRole([FromBody] RoleDTO role)
    {
        var response = await service.CreateRole(role);
        if (!response.Success)
        {
            return BadRequest(response);
        }

        return StatusCode((int)HttpStatusCode.Created, response);
    }

    [Authorize(Policy = "RoleSystem.Roles.Update")]
    [HttpPut]
    public async Task<IActionResult> UpdateRole([FromBody] RoleDTO role)
    {
        var response = await service.UpdateRole(role);
        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [Authorize(Policy = "RoleSystem.Roles.Delete")]
    [HttpDelete("{roleId}")]
    public async Task<IActionResult> DeleteRole(int roleId)
    {
        var response = await service.DeleteRole(roleId);
        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [Authorize(Policy = "RoleSystem.Roles.Functions.Add")]
    [HttpPut("{roleId}/Functions/{functionId}")]
    public async Task<IActionResult> AddRoleFunction(int roleId, int functionId)
    {
        var response = await functionService.AddFunction(roleId, functionId);
        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [Authorize(Policy = "RoleSystem.Roles.Functions.Remove")]
    [HttpDelete("{roleId}/Functions/{functionId}")]
    public async Task<IActionResult> RemoveRoleFunction(int roleId, int functionId)
    {
        var response = await functionService.RemoveFunction(roleId, functionId);
        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [Authorize(Policy = "RoleSystem.Roles.Users.Add")]
    [HttpPut("{roleId}/Users/{userId}")]
    public async Task<IActionResult> AddRoleUser(int roleId, string userId)
    {
        var response = await roleUserService.AddUser(roleId, userId);
        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }

    [Authorize(Policy = "RoleSystem.Roles.Users.Remove")]
    [HttpDelete("{roleId}/Users/{userId}")]
    public async Task<IActionResult> RemvoeRoleUser(int roleId, string userId)
    {
        var response = await roleUserService.RemoveUser(roleId, userId);
        if (!response.Success)
        {
            return BadRequest(response);
        }

        return Ok(response);
    }
}
