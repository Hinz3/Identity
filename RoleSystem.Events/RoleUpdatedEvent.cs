﻿namespace RoleSystem.Events
{
    /// <summary>
    /// Published when a role has been updated
    /// </summary>
    public class RoleUpdatedEvent
    {
        public int RoleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
