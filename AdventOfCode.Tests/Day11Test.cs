using AdventOfCode.Days;
using AdventOfCode.Day11;

namespace AdventOfCode.Tests
{
    public class Day11Test
    {
        [Fact]
        public void Test()
        {
            Assert.Equal(10605, new Day11Part1().Solve(testInput, 20));
        }

        [Fact]
        public void TestReal()
        {
            Assert.Equal(316888, new Day11Part1().Solve(input, 20));
        }

        [Fact]
        public void TestPart2Ex1()
        {
            Assert.Equal(new Day11Part2().Solve(testInput, 2, false), new Day11Part2().Solve(testInput, 2, true));
        }

        [Fact]
        public void TestPart2After2Rounds()
        {
            var day11part1 = new Day11Part2();
            var day11part2 = new Day11Part2();
            long part1Solution = day11part1.Solve(testInput, 2, false);
            long part2Solution = day11part2.Solve(testInput, 2, true);

            string part1Log = day11part1.ToString();
            string part2Log = day11part2.ToString();
            Assert.Equal(part1Log, part2Log);

            Assert.Equal(part1Solution, part2Solution);
        }

        [Fact]
        public void TestPart2Ex2()
        {
            Assert.Equal(103 * 99L, new Day11Part2().Solve(testInput, 20, true));
        }

        [Fact]
        public void TestPart2Ex3()
        {
            Assert.Equal(5204 * 5192L, new Day11Part2().Solve(testInput, 1000, true));
        }

        [Fact]
        public void TestPart2()
        {
            Assert.Equal(2713310158L, new Day11Part2().Solve(testInput, 10000, true));
        }

        [Fact]
        public void TestRealPart2()
        {
            Assert.Equal(35270398814, new Day11Part2().Solve(input, 10000, true));
        }

    private static string testInput = @"Monkey 0:
  Starting items: 79, 98
  Operation: new = old * 19
  Test: divisible by 23
    If true: throw to monkey 2
    If false: throw to monkey 3

Monkey 1:
  Starting items: 54, 65, 75, 74
  Operation: new = old + 6
  Test: divisible by 19
    If true: throw to monkey 2
    If false: throw to monkey 0

Monkey 2:
  Starting items: 79, 60, 97
  Operation: new = old * old
  Test: divisible by 13
    If true: throw to monkey 1
    If false: throw to monkey 3

Monkey 3:
  Starting items: 74
  Operation: new = old + 3
  Test: divisible by 17
    If true: throw to monkey 0
    If false: throw to monkey 1";

        private static string input = @"Monkey 0:
  Starting items: 99, 63, 76, 93, 54, 73
  Operation: new = old * 11
  Test: divisible by 2
    If true: throw to monkey 7
    If false: throw to monkey 1

Monkey 1:
  Starting items: 91, 60, 97, 54
  Operation: new = old + 1
  Test: divisible by 17
    If true: throw to monkey 3
    If false: throw to monkey 2

Monkey 2:
  Starting items: 65
  Operation: new = old + 7
  Test: divisible by 7
    If true: throw to monkey 6
    If false: throw to monkey 5

Monkey 3:
  Starting items: 84, 55
  Operation: new = old + 3
  Test: divisible by 11
    If true: throw to monkey 2
    If false: throw to monkey 6

Monkey 4:
  Starting items: 86, 63, 79, 54, 83
  Operation: new = old * old
  Test: divisible by 19
    If true: throw to monkey 7
    If false: throw to monkey 0

Monkey 5:
  Starting items: 96, 67, 56, 95, 64, 69, 96
  Operation: new = old + 4
  Test: divisible by 5
    If true: throw to monkey 4
    If false: throw to monkey 0

Monkey 6:
  Starting items: 66, 94, 70, 93, 72, 67, 88, 51
  Operation: new = old * 5
  Test: divisible by 13
    If true: throw to monkey 4
    If false: throw to monkey 5

Monkey 7:
  Starting items: 59, 59, 74
  Operation: new = old + 8
  Test: divisible by 3
    If true: throw to monkey 1
    If false: throw to monkey 3";
    }
}