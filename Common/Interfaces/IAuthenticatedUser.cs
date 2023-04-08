using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Interfaces
{
    public interface IAuthenticatedUser
    {
        string UserId { get; }
        string UserName { get; }
        List<int> Functions { get; }

        bool HasFunction(int functionId);
    }
}
