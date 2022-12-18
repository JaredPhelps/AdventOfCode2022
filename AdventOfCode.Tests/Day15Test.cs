using AdventOfCode.Days;

namespace AdventOfCode.Tests
{
    public class Day15Test
    {
        [Fact]
        public void TestPart1()
        {
            Assert.Equal(26, new Day15Part1().Solve(testInput, 10));
        }

        [Fact]
        public void TestPart1Real()
        {
            Assert.Equal(5040643, new Day15Part1().Solve(input, 2000000));
        }

        [Fact]
        public void TestPart2a()
        {
            Assert.Equal(56000011, new Day15Part2().Solve(testInput, 20));
        }

        [Fact]
        public void TestPart2aReal()
        {
            Assert.Equal(11016575214126, new Day15Part2().Solve(input, 4000000));
        }

        private static string testInput = @"Sensor at x=2, y=18: closest beacon is at x=-2, y=15
Sensor at x=9, y=16: closest beacon is at x=10, y=16
Sensor at x=13, y=2: closest beacon is at x=15, y=3
Sensor at x=12, y=14: closest beacon is at x=10, y=16
Sensor at x=10, y=20: closest beacon is at x=10, y=16
Sensor at x=14, y=17: closest beacon is at x=10, y=16
Sensor at x=8, y=7: closest beacon is at x=2, y=10
Sensor at x=2, y=0: closest beacon is at x=2, y=10
Sensor at x=0, y=11: closest beacon is at x=2, y=10
Sensor at x=20, y=14: closest beacon is at x=25, y=17
Sensor at x=17, y=20: closest beacon is at x=21, y=22
Sensor at x=16, y=7: closest beacon is at x=15, y=3
Sensor at x=14, y=3: closest beacon is at x=15, y=3
Sensor at x=20, y=1: closest beacon is at x=15, y=3";

        private static string input = @"Sensor at x=2924811, y=3544081: closest beacon is at x=3281893, y=3687621
Sensor at x=2719183, y=2520103: closest beacon is at x=2872326, y=2415450
Sensor at x=3754787, y=3322726: closest beacon is at x=3281893, y=3687621
Sensor at x=1727202, y=1485124: closest beacon is at x=1329230, y=1133797
Sensor at x=2517008, y=2991710: closest beacon is at x=2454257, y=2594911
Sensor at x=1472321, y=3123671: closest beacon is at x=2216279, y=3414523
Sensor at x=3456453, y=3959037: closest beacon is at x=3281893, y=3687621
Sensor at x=3997648, y=2624215: closest beacon is at x=4401794, y=2000000
Sensor at x=463281, y=967584: closest beacon is at x=1329230, y=1133797
Sensor at x=2395529, y=1897869: closest beacon is at x=2454257, y=2594911
Sensor at x=3094466, y=3888307: closest beacon is at x=3281893, y=3687621
Sensor at x=2737812, y=3928154: closest beacon is at x=2744537, y=4159197
Sensor at x=813538, y=3874308: closest beacon is at x=2216279, y=3414523
Sensor at x=822358, y=1997263: closest beacon is at x=1329230, y=1133797
Sensor at x=3993754, y=3951321: closest beacon is at x=3281893, y=3687621
Sensor at x=2585409, y=3541887: closest beacon is at x=2216279, y=3414523
Sensor at x=3269796, y=3730504: closest beacon is at x=3281893, y=3687621
Sensor at x=3075750, y=2873879: closest beacon is at x=2872326, y=2415450
Sensor at x=1357, y=2747918: closest beacon is at x=-1077481, y=3057204
Sensor at x=2256257, y=344800: closest beacon is at x=1854450, y=-900998
Sensor at x=2779742, y=2308087: closest beacon is at x=2872326, y=2415450
Sensor at x=867692, y=64146: closest beacon is at x=1329230, y=1133797
Sensor at x=3454465, y=966419: closest beacon is at x=4401794, y=2000000
Sensor at x=1902550, y=2398376: closest beacon is at x=2454257, y=2594911";
    }
}