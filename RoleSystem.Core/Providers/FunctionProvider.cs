using RoleSystem.Contracts.DTOs;
using RoleSystem.Core.Interfaces.Providers;
using RoleSystem.Core.Interfaces.Repositories;

namespace RoleSystem.Core.Providers;

public class FunctionProvider : IFunctionProvider
{
    private readonly IRoleUserRepository roleUserRepository;
    private readonly IRoleFunctionRepository roleFunctionRepository;
    private readonly IFunctionRepository functionRepository;

    public FunctionProvider(IRoleUserRepository roleUserRepository, IRoleFunctionRepository roleFunctionRepository, IFunctionRepository functionRepository)
    {
        this.roleUserRepository = roleUserRepository;
        this.roleFunctionRepository = roleFunctionRepository;
        this.functionRepository = functionRepository;
    }

    public async Task<List<FunctionDTO>> GetFunctions(string userId)
    {
        var roles = await roleUserRepository.GetRoleIds(userId);
        if (!roles.Any())
        {
            return new List<FunctionDTO>();
        }

        var allFunctions = await functionRepository.GetFunctions();
        var allRoleFunctions = await roleFunctionRepository.GetAllRoleFunctions();

        var functions = new List<int>();
        foreach (var role in roles)
        {
            if (!allRoleFunctions.ContainsKey(role))
            {
                continue;
            }

            functions.AddRange(allRoleFunctions[role]);
        }

        var assignedFunctions = new List<FunctionDTO>();
        foreach (var functionId in functions.Distinct().Order().ToList())
        {
            var function = allFunctions.Where(x => x.Id == functionId).FirstOrDefault();
            if (function == null)
            {
                continue;
            }

            assignedFunctions.Add(function);

            var childFunctions = allFunctions.Where(x => x.ParentFunctionId.HasValue && x.ParentFunctionId.Value == functionId).ToList();
            assignedFunctions.AddRange(childFunctions);
        }

        return assignedFunctions.Distinct().ToList();
    }
}
