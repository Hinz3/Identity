namespace RoleSystem.Core.Interfaces;

public interface IPolicy<T>
{
    void ApplyPolicy(T value);
}
