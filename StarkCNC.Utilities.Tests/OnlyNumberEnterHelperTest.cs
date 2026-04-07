namespace StarkCNC.Utilities.Tests;

public class OnlyNumberEnterHelperTest
{
    [Theory]
    [InlineData("127")]
    [InlineData("89.09")]
    [InlineData("65,235")]
    [InlineData("0,001")]
    [InlineData("0.0001")]
    public void PositiveNumberEnteredTest(string number)
    {
        Assert.True(OnlyNumberEnterHelper.IsTextAllowed(number));
    }

    [Theory]
    [InlineData("-127")]
    [InlineData("-89.09")]
    [InlineData("-65,235")]
    [InlineData("-0,001")]
    [InlineData("-0.0001")]
    public void NegativeNumberEnteredTest(string number)
    {
        Assert.False(OnlyNumberEnterHelper.IsTextAllowed(number));
    }

    [Theory]
    [InlineData("testString")]
    [InlineData("0.99f")]
    [InlineData("124.4d")]
    [InlineData("Str1ng C0nt41n5 number5")]
    public void StringEnteredTest(string number)
    {
        Assert.False(OnlyNumberEnterHelper.IsTextAllowed(number));
    }
}
