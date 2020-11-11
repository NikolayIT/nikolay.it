namespace BlogSystem.Services.Data
{
    using System.Threading.Tasks;

    public interface ILatestVideosProvider
    {
        Task FetchLatestVideosAsync(string youtubeChannelId, bool useFilter);
    }
}
