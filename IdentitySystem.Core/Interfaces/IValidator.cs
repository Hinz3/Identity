using Common.DTOs;
using IdentitySystem.Contracts.DTOs;

namespace IdentitySystem.Core.Interfaces;

public interface IValidator<T>
{
    Task<List<ErrorDTO>> Validate(T value);
}
