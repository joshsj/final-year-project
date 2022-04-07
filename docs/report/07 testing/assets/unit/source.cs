// CoordinatesTests.cs

private const double Tolerance = 0.001; 
    
[TestCaseSource(nameof(TestData))]
public void CanMeasureDistance(
  Coordinates a, Coordinates b, Distance expected)
{
  var distance = a.Distance(b);

  Assert.True(distance.Meters - expected.Meters < Tolerance);
}
