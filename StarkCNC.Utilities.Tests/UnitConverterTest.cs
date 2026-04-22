using System.Globalization;

namespace StarkCNC.Utilities.Tests;

public class UnitConverterTest
{
    [Theory]
    [InlineData(50, "шт.")]
    [InlineData(24, "сек.")]
    public void ConvertIntTest(int value, string unit)
    {
        // Arrange
        var converter = new UnitConverter();
        // Act
        var result = converter.Convert(value, typeof(string), unit, CultureInfo.InvariantCulture);
        // Assert
        Assert.Equivalent($"{value} {unit}", result);
    }

    [Theory]
    [InlineData(50, "шт.")]
    [InlineData(25.7, "сек.")]
    [InlineData(99.979, "шт.")]
    public void ConvertFloatTest(float value, string unit)
    {
        // Arrange
        var converter = new UnitConverter();
        // Act
        var result = converter.Convert(value, typeof(string), unit, CultureInfo.InvariantCulture);
        // Assert
        Assert.Equivalent($"{value} {unit}", result);
    }

    [Theory]
    [InlineData("50 шт.", 50)]
    [InlineData("24 сек.", 24)]
    public void ConvertBackInt(string value, int numberExpected)
    {
        // Arrange
        var converter = new UnitConverter();
        // Act
        var result = (int)converter.ConvertBack(value, typeof(int), null, CultureInfo.InvariantCulture);
        // Assert
        Assert.Equal(numberExpected, result);
    }

    [Theory]
    [InlineData("50 шт.", 50)]
    [InlineData("25.7 сек.", 25.7)]
    [InlineData("99.979 шт.", 99.979)]
    public void ConvertBackFloatTest(string value, float numberExpected)
    {
        // Arrange
        var converter = new UnitConverter();
        // Act
        var result = (float)converter.ConvertBack(value, typeof(float), null, CultureInfo.InvariantCulture);
        // Assert
        Assert.Equal(numberExpected, result);
    }
}
