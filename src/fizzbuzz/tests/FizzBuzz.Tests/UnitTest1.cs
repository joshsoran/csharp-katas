using Xunit;

public static class FizzBuzz
{
    public static string Of(int n)
        => (n % 15 == 0) ? "FizzBuzz"
         : (n % 3 == 0)  ? "Fizz"
         : (n % 5 == 0)  ? "Buzz"
         : n.ToString();
}

public class FizzBuzzTests
{
    [Theory]
    [InlineData(1, "1")]
    [InlineData(3, "Fizz")]
    [InlineData(5, "Buzz")]
    [InlineData(15, "FizzBuzz")]
    public void FizzBuzz_Works(int n, string expected)
    {
        Assert.Equal(expected, FizzBuzz.Of(n));
    }
}
