namespace BlogSystem.Web.Infrastructure.Identity
{
    using System.Security.Principal;

    using BlogSystem.Data;

    using Data.Models;

    using Microsoft.AspNet.Identity;

    public class CurrentUser : ICurrentUser
    {
        private readonly IIdentity currentIdentity;
        private readonly IApplicationDbContext currentDbContext;

        private ApplicationUser user;

        public CurrentUser(IIdentity identity, IApplicationDbContext context)
        {
            this.currentIdentity = identity;
            this.currentDbContext = context;
        }

        public ApplicationUser Get()
        {
            return this.user ?? (this.user = this.currentDbContext.Users.Find(this.currentIdentity.GetUserId()));
        }
    }
}