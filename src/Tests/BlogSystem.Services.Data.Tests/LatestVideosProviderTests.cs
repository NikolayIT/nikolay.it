namespace BlogSystem.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using BlogSystem.Data.Common.Repositories;
    using BlogSystem.Data.Models;
    using BlogSystem.Services.YouTube;

    using Moq;

    using Xunit;

    public class LatestVideosProviderTests
    {
        [Fact]
        public async Task FetchLatestVideosAsyncAddsAllNewVideos()
        {
            var items = new List<Item>
                        {
                            NewItem("video1", "Title 1", "Description 1", new DateTime(2024, 1, 1)),
                            NewItem("video2", "Title 2", "Description 2", new DateTime(2024, 2, 2)),
                        };

            var fetcherMock = new Mock<IYouTubeChannelVideosFetcher>();
            fetcherMock.Setup(x => x.GetAllVideosFromChannel(It.IsAny<string>(), It.IsAny<Func<Item, bool>>()))
                .ReturnsAsync(items);

            var addedVideos = new List<Video>();
            var repositoryMock = new Mock<IDeletableEntityRepository<Video>>();
            repositoryMock.Setup(x => x.AllWithDeleted()).Returns(new List<Video>().AsQueryable());
            repositoryMock.Setup(x => x.AddAsync(It.IsAny<Video>()))
                .Callback<Video>(addedVideos.Add)
                .Returns(Task.CompletedTask);

            var provider = new LatestVideosProvider(repositoryMock.Object, fetcherMock.Object);

            await provider.FetchLatestVideosAsync("channel", false);

            Assert.Equal(2, addedVideos.Count);
            Assert.Equal("video1", addedVideos[0].VideoId);
            Assert.Equal("Title 1", addedVideos[0].Title);
            Assert.Equal("Description 1", addedVideos[0].Description);
            Assert.Equal(new DateTime(2024, 1, 1), addedVideos[0].CreatedOn);
            repositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task FetchLatestVideosAsyncSkipsVideosThatAlreadyExist()
        {
            var items = new List<Item>
                        {
                            NewItem("existing", "Existing", "Already in the database", new DateTime(2024, 1, 1)),
                        };

            var fetcherMock = new Mock<IYouTubeChannelVideosFetcher>();
            fetcherMock.Setup(x => x.GetAllVideosFromChannel(It.IsAny<string>(), It.IsAny<Func<Item, bool>>()))
                .ReturnsAsync(items);

            var existingVideos = new List<Video> { new Video { VideoId = "existing" } };
            var repositoryMock = new Mock<IDeletableEntityRepository<Video>>();
            repositoryMock.Setup(x => x.AllWithDeleted()).Returns(existingVideos.AsQueryable());

            var provider = new LatestVideosProvider(repositoryMock.Object, fetcherMock.Object);

            await provider.FetchLatestVideosAsync("channel", false);

            repositoryMock.Verify(x => x.AddAsync(It.IsAny<Video>()), Times.Never);
            repositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task FetchLatestVideosAsyncAppliesNameFilterWhenRequested()
        {
            Func<Item, bool> capturedFilter = null;
            var fetcherMock = new Mock<IYouTubeChannelVideosFetcher>();
            fetcherMock.Setup(x => x.GetAllVideosFromChannel(It.IsAny<string>(), It.IsAny<Func<Item, bool>>()))
                .Callback<string, Func<Item, bool>>((_, filter) => capturedFilter = filter)
                .ReturnsAsync(new List<Item>());

            var repositoryMock = new Mock<IDeletableEntityRepository<Video>>();
            repositoryMock.Setup(x => x.AllWithDeleted()).Returns(new List<Video>().AsQueryable());

            var provider = new LatestVideosProvider(repositoryMock.Object, fetcherMock.Object);

            await provider.FetchLatestVideosAsync("channel", true);

            Assert.NotNull(capturedFilter);
            Assert.True(capturedFilter(NewItem("id1", "Лекция", "Лекция на Николай Костов", new DateTime(2024, 1, 1))));
            Assert.False(capturedFilter(NewItem("id2", "Other", "Some unrelated video", new DateTime(2024, 1, 1))));
        }

        [Fact]
        public async Task FetchLatestVideosAsyncAcceptsAllVideosWhenFilterIsNotRequested()
        {
            Func<Item, bool> capturedFilter = null;
            var fetcherMock = new Mock<IYouTubeChannelVideosFetcher>();
            fetcherMock.Setup(x => x.GetAllVideosFromChannel(It.IsAny<string>(), It.IsAny<Func<Item, bool>>()))
                .Callback<string, Func<Item, bool>>((_, filter) => capturedFilter = filter)
                .ReturnsAsync(new List<Item>());

            var repositoryMock = new Mock<IDeletableEntityRepository<Video>>();
            repositoryMock.Setup(x => x.AllWithDeleted()).Returns(new List<Video>().AsQueryable());

            var provider = new LatestVideosProvider(repositoryMock.Object, fetcherMock.Object);

            await provider.FetchLatestVideosAsync("channel", false);

            Assert.NotNull(capturedFilter);
            Assert.True(capturedFilter(NewItem("id1", "Other", "Some unrelated video", new DateTime(2024, 1, 1))));
        }

        private static Item NewItem(string videoId, string title, string description, DateTime publishedAt) =>
            new Item
            {
                Id = new Id { VideoId = videoId },
                Snippet = new Snippet { Title = title, Description = description, PublishedAt = publishedAt },
            };
    }
}
