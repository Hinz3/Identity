namespace RoleSystem.Events
{
    /// <summary>
    /// Published when a function is created 
    /// </summary>
    public class FunctionUpdatedEvent
    {
        public int FunctionId { get; set; }
        public int? ParentFunctionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
