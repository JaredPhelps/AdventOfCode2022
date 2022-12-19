using AdventOfCode.Days;

namespace AdventOfCode.Tests
{
    public class Day16Test
    {
        [Fact]
        public void TestPart1()
        {
            Assert.Equal(1651, new Day16Part1().Solve(testInput, 30));
        }

        [Fact]
        public void TestPart1_2Minutes()
        {
            Assert.Equal(0, new Day16Part1().Solve(testInput, 2));
        }

        [Fact]
        public void TestPart1_3Minutes()
        {
            Assert.Equal(20, new Day16Part1().Solve(testInput, 3));
        }

        [Fact]
        public void TestPart1Real()
        {
            Assert.Equal(1896, new Day16Part1().Solve(input, 30));
        }

        [Fact]
        public void TestPart2()
        {
            Assert.Equal(1707, new Day16Part2().Solve(testInput, 26));
        }

        [Fact]
        public void TestPart2Real()
        {
            // yeah this was pretty dang slow but fast enough to come up with the answer.
            // Assert.Equal(2576, new Day16Part2().Solve(input, 26));
        }

        private static string testInput = @"Valve AA has flow rate=0; tunnels lead to valves DD, II, BB
Valve BB has flow rate=13; tunnels lead to valves CC, AA
Valve CC has flow rate=2; tunnels lead to valves DD, BB
Valve DD has flow rate=20; tunnels lead to valves CC, AA, EE
Valve EE has flow rate=3; tunnels lead to valves FF, DD
Valve FF has flow rate=0; tunnels lead to valves EE, GG
Valve GG has flow rate=0; tunnels lead to valves FF, HH
Valve HH has flow rate=22; tunnel leads to valve GG
Valve II has flow rate=0; tunnels lead to valves AA, JJ
Valve JJ has flow rate=21; tunnel leads to valve II";

        private static string input = @"Valve XG has flow rate=0; tunnels lead to valves CR, OH
Valve ZF has flow rate=7; tunnels lead to valves SC, BY, PM, LW, CJ
Valve RD has flow rate=13; tunnels lead to valves JS, VM
Valve XJ has flow rate=0; tunnels lead to valves JO, YO
Valve CJ has flow rate=0; tunnels lead to valves UA, ZF
Valve UA has flow rate=0; tunnels lead to valves ZP, CJ
Valve EQ has flow rate=0; tunnels lead to valves ZP, RP
Valve IU has flow rate=0; tunnels lead to valves EV, CN
Valve QM has flow rate=0; tunnels lead to valves XA, CN
Valve WC has flow rate=0; tunnels lead to valves JS, OH
Valve MU has flow rate=0; tunnels lead to valves AA, ZP
Valve MW has flow rate=11; tunnels lead to valves BM, AG, RG, NL
Valve XA has flow rate=0; tunnels lead to valves JO, QM
Valve OH has flow rate=12; tunnels lead to valves WC, YS, XG, KO
Valve QD has flow rate=20; tunnels lead to valves BY, KY, CR, RP
Valve OE has flow rate=0; tunnels lead to valves FB, BU
Valve CB has flow rate=0; tunnels lead to valves AA, FX
Valve TB has flow rate=23; tunnel leads to valve VM
Valve PM has flow rate=0; tunnels lead to valves ZF, AA
Valve YS has flow rate=0; tunnels lead to valves OH, RG
Valve KO has flow rate=0; tunnels lead to valves OH, VT
Valve AA has flow rate=0; tunnels lead to valves PM, MU, BM, AW, CB
Valve RG has flow rate=0; tunnels lead to valves YS, MW
Valve BU has flow rate=0; tunnels lead to valves OE, EV
Valve RK has flow rate=0; tunnels lead to valves KY, FX
Valve JO has flow rate=18; tunnels lead to valves NL, SX, XA, XJ
Valve AG has flow rate=0; tunnels lead to valves IS, MW
Valve AW has flow rate=0; tunnels lead to valves BS, AA
Valve ZP has flow rate=9; tunnels lead to valves UA, NG, DU, MU, EQ
Valve KY has flow rate=0; tunnels lead to valves QD, RK
Valve EV has flow rate=19; tunnels lead to valves VT, BU, IU, SX
Valve FB has flow rate=24; tunnel leads to valve OE
Valve DU has flow rate=0; tunnels lead to valves IS, ZP
Valve NG has flow rate=0; tunnels lead to valves FX, ZP
Valve HC has flow rate=0; tunnels lead to valves CN, HB
Valve SC has flow rate=0; tunnels lead to valves IS, ZF
Valve HB has flow rate=22; tunnel leads to valve HC
Valve VM has flow rate=0; tunnels lead to valves RD, TB
Valve LW has flow rate=0; tunnels lead to valves ZF, FX
Valve SX has flow rate=0; tunnels lead to valves EV, JO
Valve FX has flow rate=6; tunnels lead to valves FA, NG, RK, LW, CB
Valve JS has flow rate=0; tunnels lead to valves WC, RD
Valve BM has flow rate=0; tunnels lead to valves MW, AA
Valve FA has flow rate=0; tunnels lead to valves IS, FX
Valve RP has flow rate=0; tunnels lead to valves QD, EQ
Valve NL has flow rate=0; tunnels lead to valves MW, JO
Valve CN has flow rate=15; tunnels lead to valves HC, QM, IU
Valve BS has flow rate=0; tunnels lead to valves IS, AW
Valve KP has flow rate=25; tunnel leads to valve YO
Valve YO has flow rate=0; tunnels lead to valves XJ, KP
Valve CR has flow rate=0; tunnels lead to valves XG, QD
Valve BY has flow rate=0; tunnels lead to valves QD, ZF
Valve IS has flow rate=5; tunnels lead to valves DU, SC, AG, FA, BS
Valve VT has flow rate=0; tunnels lead to valves KO, EV";
    }
}