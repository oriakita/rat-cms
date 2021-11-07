using System;

namespace Raci.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string UserName { get; set; }

        Guid UserGuidId { get; set; }

        bool IsAuthenticated { get; }
    }
}
