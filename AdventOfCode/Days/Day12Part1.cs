using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace AdventOfCode.Days
{
    public class Day12Part1
    {
        public int Solve(string input)
        {
            using var sr = new StringReader(input);
            var (startNode, endNode, map) = ParseInput(sr);
            Queue<Node> startQueue = new Queue<Node>();
            Queue<Node> endQueue = new Queue<Node>();
            startQueue.Enqueue(startNode);
            endQueue.Enqueue(endNode);

            startNode.IsTraversingFromStart = true;
            endNode.IsTraversingFromEnd = true;

            while (startQueue.Count > 0 && endQueue.Count > 0)
            {
                var sNode = startQueue.Dequeue();
                var eNode = endQueue.Dequeue();

                BfsStart(startQueue, sNode);
                BfsEnd(endQueue, eNode);

                Node? intersection = map.Values.FirstOrDefault(n => n.IsTraversingFromEnd && n.IsTraversingFromStart);
                if (intersection != null)
                {
                    int steps = 0;
                    Node cursor = intersection;
                    while (cursor.StartParent != null)
                    {
                        steps++;
                        cursor = cursor.StartParent;
                    }

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

            void BfsStart(Queue<Node> queue, Node parent)
            {
                foreach (var neighbor in parent.StartNeighbors.Where(n => !n.IsTraversingFromStart))
                {
                    neighbor.IsTraversingFromStart = true;
                    neighbor.StartParent = parent;
                    queue.Enqueue(neighbor);
                }
            }

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
            public Node? StartParent { get; set; }
            public Node? EndParent { get; set; }
            public Position Position { get; set; }
            public char Height { get; set; }
            public Dictionary<Position, Node> Map { get; set; }
            public bool IsTraversingFromStart { get; set; }
            public bool IsTraversingFromEnd { get; set; }
            public Node(int x, int y, char height, Dictionary<Position, Node> map)
            {
                Map = map;
                Height = height;
                Position = new Position(x, y);
            }

            public IEnumerable<Node> StartNeighbors => Position.Neighbors
                .Where(p => Map.ContainsKey(p))
                .Select(p => Map[p])
                .Where(n => n.Height - Height <= 1);

            public IEnumerable<Node> EndNeighbors => Position.Neighbors
                .Where(p => Map.ContainsKey(p))
                .Select(p => Map[p])
                .Where(n => Height - n.Height <= 1);


        }
    }
}

