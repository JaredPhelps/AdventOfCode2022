using AdventOfCode.Days;

namespace AdventOfCode.Tests
{
    public class Day2Test
    {
        [Fact]
        public void Test()
        {
            Assert.Equal(12, new Day2().Solve(@"A Y
B X
C Z"));
        }
    }
}