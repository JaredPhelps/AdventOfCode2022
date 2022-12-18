using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace AdventOfCode.Days
{
    public class Day15Part2
    {
        public long Solve(string input, int maxY)
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

            List<RangeBoundary> ranges = sensors.SelectMany(s => s.GetBoundaryLines()).ToList();
            foreach (var rangeLinePlus in ranges.Where(r => r.Slope == 1))
            {
                foreach (var rangeLineMinus in ranges.Where(r => r.Slope == -1))
                {
                    foreach (var potentialSolution in rangeLinePlus.PointsNearIntersection(rangeLineMinus))
                    {
                        if (potentialSolution.X < 0 || potentialSolution.Y < 0 || potentialSolution.X > maxY || potentialSolution.Y > maxY)
                            continue;

                        bool isCovered = sensors.Any(s => s.IsInRange(potentialSolution));
                        if (!isCovered)
                        {
                            checked
                            {
                                return potentialSolution.X * 4000000L + potentialSolution.Y;
                            }
                        }
                    }
                }
            }

            throw new Exception("No solution found.");
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

            public bool IsInRange(Position p) => Math.Abs(this.position - p) <= Range;

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

            public IEnumerable<RangeBoundary> GetBoundaryLines()
            {
                Position left = new Position(position.X - Range, position.Y);
                Position right = new Position(position.X + Range, position.Y);
                yield return new RangeBoundary(-1, left);
                yield return new RangeBoundary(1, left);
                yield return new RangeBoundary(-1, right);
                yield return new RangeBoundary(1, right);
            }
        }

        private record RangeBoundary
        {
            // Always -1 or 1 for this problem.
            public int Slope { get; set; }
            public Position Point { get; set; }
            // Point at 17,7, m = 1, b = -1
            // Point at -1, 7 m = -1, b = 8
            // y = mx + b, b = y - mx => b = 7 - 17 = -10

            public int YIntercept => Point.Y - Slope * Point.X;

            public RangeBoundary(int slope, Position point)
            {
                Slope = slope;
                Point = point;
            }

            // The y-intercept of the line 
            // Something like mx + b = nx + c => (m - n)x = c - b, x = (c - b)/(m - n)

            private int IntersectionPointX(RangeBoundary other) => (other.YIntercept - YIntercept) / (Slope - other.Slope);
            private int Y(int x) => Slope * x + YIntercept;
            public Position IntersectionPoint(RangeBoundary other) => new Position(IntersectionPointX(other), Y(IntersectionPointX(other)));

            public IEnumerable<Position> PointsNearIntersection(RangeBoundary other)
            {
                if (other.Slope == Slope)
                    return Enumerable.Empty<Position>();

                Position intersectionPoint = IntersectionPoint(other);

                return intersectionPoint.Neighbors.AsEnumerable();
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

            public IEnumerable<Position> Neighbors => new[] { new Position(X + 1, Y), new Position(X, Y + 1), new Position(X - 1, Y), new Position(X, Y - 1) };
        }
    }

}

