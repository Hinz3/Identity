using System.Collections.Generic;

namespace RoleSystem.Events
{
    public class UserFunctionsEvent
    {
        public string UserId { get; set; }
        public List<int> AssignedFunctions { get; set; }
    }
}
