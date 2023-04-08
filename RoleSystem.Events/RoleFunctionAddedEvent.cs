namespace RoleSystem.Events
{
    /// <summary>
    /// Published when a function is added to role
    /// </summary>
    public class RoleFunctionAddedEvent
    {
        public int RoleId { get; set; }
        public int FunctionId { get; set; }
    }
}
