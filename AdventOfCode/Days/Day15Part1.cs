using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace AdventOfCode.Days
{
    public class Day15Part1
    {
        public int Solve(string input, int y = 10)
        {
            // For each sensor, check to see if its range even hits y=2000000 by computing how far away from its beacon it is.
            // if a sensor is at (sx, sy) and its beacon is at (bx, by), then the sensor's range is |(bx - sx)| + |(by - sy)|
            // For a point (px, py) to be in range, |(px - sx)| + |(py - sy)| < |(bx - sx)| + |(by - sy)|
            using var sr = new StringReader(input);
            string? line = null;
            List<Sensor> sensors = new List<Sensor>();
            var beacons = new HashSet<Position>();
            while ((line = sr.ReadLine()) != null)
            {
                sensors.Add(ParsePosition(line, beacons));
            }

            List<Range> ranges = sensors
                .Select(s => s.CoverageAt(y))
                .Where(r => r != null)
                .Select(r => r!)
                .ToList();

            ranges.Sort((r1, r2) => r1.Start - r2.Start);
            Range cursor = ranges.First();
            int rangeCount = 0;
            foreach (Range r in ranges.Skip(1))
            {
                if (cursor.OverlapsWith(r))
                {
                    cursor = cursor + r;
                }
                else
                {
                    rangeCount += cursor.End - cursor.Start;
                    cursor = r;
                }
            }

            return rangeCount + (cursor.End - cursor.Start + 1) - beacons.Where(b => b.Y == y).Count();
        }

        private Sensor ParsePosition(string line, HashSet<Position> beacons)
        {
            List<int> parts = line
                .Split(new[] { '=', ',', ':' })
                .Where(p => p.Any(c => char.IsDigit(c)))
                .Select(p => int.Parse(p))
                .ToList();

            var tmpBeacon = new Position(parts[2], parts[3]);
            if (!beacons.TryGetValue(tmpBeacon, out Position? beacon))
            {
                beacon = tmpBeacon;
                beacons.Add(beacon);
            }
            return new Sensor(new Position(parts[0], parts[1]), beacon!);
        }

        private class Sensor
        {
            private Position position;
            private Position beacon;
            public Sensor(Position position, Position beacon)
            {
                this.position = position;
                this.beacon = beacon;
            }

            public int Range => this.position - beacon;

            public Range? CoverageAt(int y)
            {
                int distanceToY = Math.Abs(position.Y - y);

                // This is how much of our range is "leftover" to use on the horizontal line
                int wiggleRoomX = Range - distanceToY;
                if (wiggleRoomX < 0)
                    return null;

                // Coverage at y is position.X +/- (Range - distanceToY)?
                return new Range(position.X + wiggleRoomX, position.X - wiggleRoomX);
            }
        }


        private record Range
        {
            public int Start { get; set; }
            public int End { get; set; }
            public Range(int start, int end)
            {
                Start = Math.Min(start, end);
                End = Math.Max(start, end);
            }

            public bool OverlapsWith(Range other) => other.Start >= Start && other.Start <= End;

            public static Range operator +(Range range1, Range range2) => new Range(Math.Min(range1.Start, range2.Start), Math.Max(range1.End, range2.End));
        }

        private record Position
        {
            public int X { get; set; }
            public int Y { get; set; }
            public Position(int x, int y)
            {
                X = x;
                Y = y;
            }

            public static int  operator -(Position a, Position b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
        }
    }

}

