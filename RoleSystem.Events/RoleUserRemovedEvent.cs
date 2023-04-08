namespace RoleSystem.Events
{
    /// <summary>
    /// Published when a user has been removed from role
    /// </summary>
    public class RoleUserRemovedEvent
    {
        public int RoleId { get; set; }
        public string UserId { get; set; }
    }
}
