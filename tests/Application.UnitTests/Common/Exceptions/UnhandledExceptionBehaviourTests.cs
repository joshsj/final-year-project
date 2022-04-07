using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RendezVous.Application.Common.Behaviours;
using RendezVous.Application.Jobs.Queries.GetJobs;

namespace RendezVous.Application.UnitTests.Common.Exceptions;

public class UnhandledExceptionBehaviourTests
{
    [Test]
    public void ThrowsTheOriginalException()
    {
        // Arrange
        var request = new GetJobsQuery();
        var exception = new Exception();
        
        var logger = Mock.Of<ILogger<GetJobsQuery>>();
        var sut = new UnhandledExceptionBehaviour<GetJobsQuery, Unit>(logger);
        
        // Act
        var result = Assert.ThrowsAsync<Exception>(
            () => sut.Handle(request, CancellationToken.None, () => throw exception));

        // Assert
        Assert.That(result, Is.SameAs(exception));
    }

    [Test]
    public void LogsTheRequest()
    {
        // Arrange
        var request = new GetJobsQuery();

        var logger = new Mock<ILogger<GetJobsQuery>>();
        var sut = new UnhandledExceptionBehaviour<GetJobsQuery, Unit>(logger.Object);
        
        // Act
        Assert.ThrowsAsync<Exception>(
            () => sut.Handle(request, CancellationToken.None, () => throw new Exception()));

        // Assert
        logger.Verify(x => x.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()));
    }
}
