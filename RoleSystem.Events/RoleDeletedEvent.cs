namespace RoleSystem.Events
{
    /// <summary>
    /// Published when a role has been deleted
    /// </summary>
    public class RoleDeletedEvent
    {
        public int RoleId { get; set; }
    }
}
