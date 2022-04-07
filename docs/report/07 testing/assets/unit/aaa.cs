// UnhandledExceptionBehaviourTests.cs

// Arrange
var request = new GetJobsQuery();
var exception = new Exception();

var logger = Mock.Of<ILogger<GetJobsQuery>>();
var sut = new UnhandledExceptionBehaviour<GetJobsQuery, Unit>(logger);

// Act
var result = Assert.ThrowsAsync<Exception>(() => 
  sut.Handle(
    request, 
    CancellationToken.None, 
    () => throw exception));

// Assert
Assert.That(result, Is.SameAs(exception));