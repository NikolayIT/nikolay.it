namespace BlogSystem.Services
{
    using System;

    public interface IBlogUrlGenerator
    {
        string GenerateUrl(int id, string title, DateTime createdOn);
    }
}
