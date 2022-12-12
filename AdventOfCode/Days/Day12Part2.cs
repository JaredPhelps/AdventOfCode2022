using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace AdventOfCode.Days
{
    public class Day12Part2
    {
        public int Solve(string input)
        {
            using var sr = new StringReader(input);
            var (startNode, endNode, map) = ParseInput(sr);
            Queue<Node> endQueue = new Queue<Node>();
            endQueue.Enqueue(endNode);

            endNode.IsTraversingFromEnd = true;

            while (endQueue.Count > 0)
            {
                var eNode = endQueue.Dequeue();

                BfsEnd(endQueue, eNode);

                Node? intersection = map.Values.FirstOrDefault(n => n.IsTraversingFromEnd && n.Height == 'a');
                if (intersection != null)
                {
                    int steps = 0;
                    Node cursor = intersection;
                    cursor = intersection;
                    while (cursor.EndParent != null)
                    {
                        steps++;
                        cursor = cursor.EndParent;
                    }
                    // return # steps.
                    return steps;
                }
            }
            throw new Exception("Not found.");
            void BfsEnd(Queue<Node> queue, Node parent)
            {
                foreach (var neighbor in parent.EndNeighbors.Where(n => !n.IsTraversingFromEnd))
                {
                    neighbor.IsTraversingFromEnd = true;
                    neighbor.EndParent = parent;
                    queue.Enqueue(neighbor);
                }
            }
        }

        private (Node start, Node end, Dictionary<Position, Node>) ParseInput(StringReader input)
        {
            int y = 0;
            Node? start = null;
            Node? end = null;
            string? line = null;
            var map = new Dictionary<Position, Node>();
            while ((line = input.ReadLine()) != null)
            {
                for (int x = 0; x < line.Length; x++)
                {
                    var newNode = new Node(x, y, line[x], map);
                    if (newNode.Height == 'S')
                    {
                        start = newNode;
                        newNode.Height = 'a';
                    }
                    else if (newNode.Height == 'E')
                    {
                        end = newNode;
                        newNode.Height = 'z';
                    }

                    map.Add(newNode.Position, newNode);
                }
                y++;
            }
            return (start!, end!, map);
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

            public Position Up => new Position(X, Y - 1);
            public Position Down => new Position(X, Y + 1);
            public Position Left => new Position(X - 1, Y);
            public Position Right => new Position(X + 1, Y);

            public IEnumerable<Position> Neighbors
            {
                get
                {
                    yield return Up;
                    yield return Down;
                    yield return Left;
                    yield return Right;
                }
            }


        }

        private record Node
        {
            public Node? EndParent { get; set; }
            public Position Position { get; set; }
            public char Height { get; set; }
            public Dictionary<Position, Node> Map { get; set; }
            public bool IsTraversingFromEnd { get; set; }
            public Node(int x, int y, char height, Dictionary<Position, Node> map)
            {
                Map = map;
                Height = height;
                Position = new Position(x, y);
            }

            public IEnumerable<Node> EndNeighbors => Position.Neighbors
                .Where(p => Map.ContainsKey(p))
                .Select(p => Map[p])
                .Where(n => Height - n.Height <= 1);


        }
    }
}

