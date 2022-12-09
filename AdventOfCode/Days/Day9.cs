namespace AdventOfCode.Days
{
    public class Day9
    {
        public int Solve(string input, int size)
        {
            var head = new Knot(null, size - 1);
            using var sr = new StringReader(input);
            string? line = null;
            while ((line = sr.ReadLine()) != null)
            {
                char direction = line[0];
                int amount = int.Parse(line.Substring(2));
                head.Move(direction, amount);
            }
            return head.Tail.VisitedPositions.Count;
        }

        public int SolvePart2(string input)
        {
            return 0;
        }

        private struct Position
        {
            public int X { get; private set; }
            public int Y { get; private set; }
            public Position(int x, int y)
            {
                X = x;
                Y = y;
            }

            public Position Move(char direction, int amount)
            {
                int x = X;
                int y = Y;
                if (direction == 'U')
                    y += amount;
                else if (direction == 'D')
                    y -= amount;
                else if (direction == 'R')
                    x += amount;
                else if (direction == 'L')
                    x -= amount;

                return new Position(x, y);
            }
        }

        private class Knot
        {
            public Position Position { get; set; } = new Position(0, 0);
            public HashSet<Position> VisitedPositions = new();
            public Knot? Follower { get; private set; }
            public Knot? Head { get; private set; }
            public Knot Tail => Follower == null ? this : Follower.Tail;
            public Knot(Knot? head, int followerCount)
            {
                VisitedPositions.Add(Position);
                if (followerCount > 0)
                {
                    Follower = new Knot(this, followerCount - 1);
                }
                Head = head;
            }

            public void Move(char direction, int amount)
            {
                for (int i = 0; i < amount; i++)
                {
                    Position = Position.Move(direction, 1);
                    VisitedPositions.Add(Position);
                    Follower?.FollowHead();
                }
            }

            public void FollowHead()
            {
                if (IsAdjacent || Head == null)
                    return;

                int x = Position.X;
                int y = Position.Y;

                // If it's diagonal, X may be only 1 away but it still moves horizontall.
                int xDelta = Math.Abs(Position.X - Head.Position.X);
                int yDelta = Math.Abs(Position.Y - Head.Position.Y);

                if (xDelta > 1 || (xDelta == 1 && yDelta > 1))
                    x += (x > Head.Position.X ? -1 : 1);

                if (yDelta > 1 || (yDelta == 1 && xDelta > 1))
                    y += (y > Head.Position.Y ? -1 : 1);

                Position = new Position(x, y);
                VisitedPositions.Add(Position);
                Follower?.FollowHead();
            }

            private bool IsAdjacent => Head == null ? true :
                Math.Abs(Position.X - Head.Position.X) <= 1 && Math.Abs(Position.Y - Head.Position.Y) <= 1;
        }
    }
}

