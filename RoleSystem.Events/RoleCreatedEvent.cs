namespace RoleSystem.Events
{
    /// <summary>
    /// Published when a role has been created
    /// </summary>
    public class RoleCreatedEvent
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
