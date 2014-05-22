namespace BlogSystem.Web.Infrastructure.Identity
{
    using BlogSystem.Data.Models;

    public interface ICurrentUser
    {
        ApplicationUser Get();
    }
}