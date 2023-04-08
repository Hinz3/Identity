namespace RoleSystem.Events
{
    /// <summary>
    /// Published when a function is deleted
    /// </summary>
    public class FunctionDeletedEvent
    {
        public int FunctionId { get; set; }
    }
}
