// CoordinatesTests.cs

private const double Tolerance = 0.001; 
    
[TestCaseSource(nameof(TestData))]
public void CanMeasureDistance(
  Coordinates a, Coordinates b, Distance expected)
{
  var distance = a.Distance(b);

  Assert.True(distance.Meters - expected.Meters < Tolerance);
}

public static IEnumerable<TestCaseData> TestData
{
  get
  {     
    // Sheffield Hallam libraries
    yield return new TestCaseData(
        new Coordinates(53.3717553, -1.4928765),
        new Coordinates(53.380264, -1.4675081),
        Distance.FromKilometers(1.9305366899397525));
  }
}