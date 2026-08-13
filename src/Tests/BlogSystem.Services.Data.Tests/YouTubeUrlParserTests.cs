namespace BlogSystem.Services.Data.Tests
{
    using BlogSystem.Services;

    using Xunit;

    public class YouTubeUrlParserTests
    {
        private readonly YouTubeUrlParser parser = new YouTubeUrlParser();

        [Theory]
        [InlineData("https://www.youtube.com/embed/LYWD_PXvFHw", "LYWD_PXvFHw")]
        [InlineData("https://www.youtube.com/watch?v=m-dlCFYRS28", "m-dlCFYRS28")]
        [InlineData("https://youtu.be/y8zG6yY29gk", "y8zG6yY29gk")]
        [InlineData("https://www.youtube.com/watch?list=PL9&v=3GuOfIyFAu0", "3GuOfIyFAu0")]
        public void GetVideoIdShouldExtractTheIdFromEveryYouTubeUrlShape(string url, string expected)
        {
            Assert.Equal(expected, this.parser.GetVideoId(url));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("/img/2011-08/book-cover.png")]
        [InlineData("https://nikolay.it/img/some-image.png")]
        public void GetVideoIdShouldReturnNullForAnythingThatIsNotAYouTubeUrl(string url)
        {
            Assert.Null(this.parser.GetVideoId(url));
        }

        [Fact]
        public void GetThumbnailUrlShouldBuildTheYouTubeThumbnailAddress()
        {
            Assert.Equal(
                "https://img.youtube.com/vi/LYWD_PXvFHw/maxresdefault.jpg",
                this.parser.GetThumbnailUrl("https://www.youtube.com/embed/LYWD_PXvFHw"));
        }

        [Fact]
        public void ToAbsoluteImageUrlShouldReturnTheThumbnailForAYouTubeUrl()
        {
            Assert.Equal(
                "https://img.youtube.com/vi/m-dlCFYRS28/maxresdefault.jpg",
                this.parser.ToAbsoluteImageUrl("https://www.youtube.com/embed/m-dlCFYRS28", "https://nikolay.it"));
        }

        [Fact]
        public void ToAbsoluteImageUrlShouldMakeRootRelativeImagesAbsolute()
        {
            Assert.Equal(
                "https://nikolay.it/img/2011-08/book-cover.png",
                this.parser.ToAbsoluteImageUrl("/img/2011-08/book-cover.png", "https://nikolay.it"));
        }

        [Fact]
        public void ToAbsoluteImageUrlShouldLeaveAlreadyAbsoluteImagesUnchanged()
        {
            Assert.Equal(
                "https://nikolay.it/img/2021-02/DatabaseNormalization.png",
                this.parser.ToAbsoluteImageUrl("https://nikolay.it/img/2021-02/DatabaseNormalization.png", "https://nikolay.it"));
        }

        [Fact]
        public void ToAbsoluteImageUrlShouldReturnNullWhenThereIsNoImage()
        {
            Assert.Null(this.parser.ToAbsoluteImageUrl(null, "https://nikolay.it"));
        }
    }
}
