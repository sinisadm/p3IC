using Microsoft.AspNetCore.Authorization;

namespace Ticketing.API.Security
{
    public class TestRequirement : IAuthorizationRequirement
    {
        public TestRequirement()
        {
        }
    }
}
