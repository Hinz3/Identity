namespace RoleSystem.Events
{
    /// <summary>
    /// Published when a function is removed from a role
    /// </summary>
    public class RoleFunctionRemovedEvent
    {
        public int RoleId { get; set; }
        public int FunctionId { get; set; }
    }
}
