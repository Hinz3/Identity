namespace RoleSystem.Events
{
    /// <summary>
    /// Published when a user is added to role
    /// </summary>
    public class RoleUserAddedEvent
    {
        public int RoleId { get; set; }
        public string UserId { get; set; }
    }
}
