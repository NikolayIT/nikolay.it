namespace BlogSystem.Web.Infrastructure.Identity
{
    using BlogSystem.Data;
    using Data.Models;
    using System.Security.Principal;
    using Microsoft.AspNet.Identity;

    public class CurrentUser : ICurrentUser
    {
        private readonly IIdentity currentIdentity;
        private readonly IApplicationDbContext currentDbContext;

        private ApplicationUser user;

        public CurrentUser(IIdentity identity, IApplicationDbContext context)
        {
            currentIdentity = identity;
            currentDbContext = context;
        }

        public ApplicationUser Get()
        {
            return user ?? (user = currentDbContext.Users.Find(currentIdentity.GetUserId()));
        }
    }
}