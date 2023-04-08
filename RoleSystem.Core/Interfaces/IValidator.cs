using Common.DTOs;

namespace RoleSystem.Core.Interfaces;

public interface IValidator<T>
{
    Task<List<ErrorDTO>> Validate(T value);
}
