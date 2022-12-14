using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace AdventOfCode.Days
{
    public class Day14Part1
    {
        private Map map = new Map(new HashSet<Position>());
        public int Solve(string input)
        {
            using var sr = new StringReader(input);
            string? line = null;
            while ((line = sr.ReadLine()) != null)
            {
                foreach (var rock in ParsePosition(line))
                {
                    map.Add(rock);
                }
            }

            int fallCount = 0;
            var startPosition = new Position(500, 0);
            while (true)
            {
                var (newPos, moved, done) = map.Fall(startPosition);

                if (done)
                    break;
                
                fallCount++;
            }
            return fallCount;
        }

        private IEnumerable<Position> ParsePosition(string line)
        {
            List<int> parts = line
                .Split(new[] { ',', '-' })
                .Select(s => s.Trim(' ', '>', '-'))
                .Select(p => int.Parse(p))
                .ToList();

            Position startPosition = new Position(parts[0], parts[1]);
            yield return startPosition;
            for (int i = 2; i < parts.Count; i+=2)
            {
                Position nextPosition = new Position(parts[i], parts[i + 1]);
                foreach (var between in startPosition.GetPositionsBetween(nextPosition))
                {
                    yield return between;
                }
                yield return nextPosition;
                startPosition = nextPosition;
            }
        }

        private class Map
        {
            private HashSet<Position> takenSpots = new HashSet<Position>();
            private int minX;
            private int maxY;
            private int maxX;
            public Map(HashSet<Position> takenSpots)
            {
                minX = int.MaxValue;
                maxY = int.MinValue;
                maxX = int.MinValue;
            }

            public void Add(Position rock)
            {
                takenSpots.Add(rock);
                if (rock.X < minX) minX = rock.X;
                if (rock.X > maxX) maxX = rock.X;
                if (rock.Y > maxY) maxY = rock.Y;
            }

            public (Position nextPosition, bool Moved, bool Done) Fall(Position startPosition)
            {
                Position lastPosition = startPosition;
                Position? nextPosition;
                while ((nextPosition = MoveOne(lastPosition)) != null && !IsInAbyss(nextPosition))
                    lastPosition = nextPosition;

                // If nextPosition is null here, it means it can't move or it's in the resting place.
                // if it can't move, the previous position we tested is the resting place.
                // If it can move but is in the abyss, restingPlace is null and we're done.
                // If the resting place is already taken, we probably filled up the whole thing and we're done.
                Position restingPlace = nextPosition != null ? nextPosition : lastPosition;
                bool done = takenSpots.Contains(restingPlace) || IsInAbyss(restingPlace);
                if (!done)
                    takenSpots.Add(restingPlace);
                return (restingPlace, startPosition.Equals(restingPlace), done);
            }

            private Position? MoveOne(Position startPosition) => !takenSpots.Contains(startPosition.Down) ? startPosition.Down :
                    !takenSpots.Contains(startPosition.DownLeft) ? startPosition.DownLeft :
                    !takenSpots.Contains(startPosition.DownRight) ? startPosition.DownRight :
                    null;

            private bool IsInAbyss(Position p) => p.Y > maxY || p.X < minX || p.X > maxX;
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
            public Position Down => new Position(X, Y + 1);
            public Position Left => new Position(X - 1, Y);
            public Position DownLeft => new Position(X - 1, Y + 1);
            public Position Right => new Position(X + 1, Y);
            public Position DownRight => new Position(X + 1, Y + 1);

            public IEnumerable<Position> GetPositionsBetween(Position other)
            {
                if (other.X != X && other.Y != Y)
                    throw new Exception("Lines are not in a horizontal or vertical line");

                int start = Math.Min(X, other.X);
                int end = Math.Max(X, other.X);
                for (int i = start + 1; i < end; i++)
                {
                    yield return new Position(i, this.Y);
                }

                start = Math.Min(Y, other.Y);
                end = Math.Max(Y, other.Y);
                for (int i = start + 1; i < end; i++)
                {
                    yield return new Position(this.X, i);
                }
            }
        }
    }

}

