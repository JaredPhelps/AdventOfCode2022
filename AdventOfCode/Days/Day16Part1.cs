using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace AdventOfCode.Days
{
    public class Day16Part1
    {
        public int Solve(string input, int minutesLeft)
        {
            Dictionary<string, Node> nodes = ParseInput(input);
            int i = 0;
            foreach (var n in nodes)
            {
                nodeIndexes.Add(n.Value.Name, 1L << i++);
            }

            Node startingNode = nodes["AA"];
            int score = startingNode.OptimalPath(minutesLeft, new HashSet<string>());
            return score;
        }

        private Dictionary<string, Node> ParseInput(string input)
        {
            var nodes = new Dictionary<string, Node>();
            string[] lines = input.Split("\r\n");

            // Just get a dictionary of all the nodes for now.
            foreach (var line in lines)
            {
                string s = new string(line.Skip(1).Where(c => char.IsUpper(c)).ToArray());
                for (int i = 0; i < s.Length; i+=2)
                {
                    string nodeName = s.Substring(i, 2);
                    if (!nodes.ContainsKey(nodeName))
                        nodes.Add(nodeName, new Node(nodeName));
                }
            }

            // fill in flow rate and children
            foreach (var line in lines)
            {
                string s = new string(line.Skip(1).Where(c => char.IsUpper(c)).ToArray());
                Node n = nodes[s.Substring(0, 2)];
                n.FlowRate = int.Parse(line.Split('=', ';')[1]);
                for (int i = 2; i < s.Length; i += 2)
                {
                    string nodeName = s.Substring(i, 2);
                    n.Children.Add(nodes[nodeName]);
                }
            }

            return nodes;
        }


        private static Dictionary<string, long> nodeIndexes = new Dictionary<string, long>();
        private static Dictionary<(string, int, long), int> cache = new Dictionary<(string, int, long), int>();

        private class Node
        {
            public List<Node> Children = new List<Node>();
            public string Name { get; set; }
            public int FlowRate { get; set; }
            public Node(string name)
            {
                Name = name;
            }

            public int OptimalPath(int minutesLeft, HashSet<string> openValves)
            {
                if (minutesLeft <= 1)
                    return 0;

                int? cachedValue = OptimalPathCache(minutesLeft, openValves);
                if (cachedValue != null)
                    return cachedValue.Value;

                long openValveSetHash = OpenValveSetHash(openValves);


                // I can spend a minute opening my current valve then going to each of my children, or I can spend a minute going to each of my children.
                // But if my valve is open already, all I can do is travel to my children again.

                int openScore = 0;
                // Even if we were here before, we might want to open it now instead of earlier, because we might have been chasing a very high value valve.
                if (!openValves.Contains(Name) && FlowRate > 0)
                {
                    openValves.Add(Name);
                    openScore = (FlowRate * (minutesLeft - 1)) + Children.Max(c => c.OptimalPath(minutesLeft - 2, openValves));
                    openValves.Remove(Name);
                }

                int skipThisValveScore = Children.Max(c => c.OptimalPath(minutesLeft - 1, openValves));

                cache.Add((Name, minutesLeft, openValveSetHash), Math.Max(skipThisValveScore, openScore));
                return Math.Max(skipThisValveScore, openScore);
            }

            // We can cache values that indicate that you're at this node with this many minutes left and this set of open valves.
            public int? OptimalPathCache(int minutesLeft, HashSet<string> openValves)
            {
                long openValvesHash = OpenValveSetHash(openValves);
                if (cache.ContainsKey((Name, minutesLeft, openValvesHash)))
                    return cache[(Name, minutesLeft, openValvesHash)];

                return default;
            }

            public long OpenValveSetHash(HashSet<string> openValves)
            {
                long openValvesHash = 0;
                foreach (string v in openValves)
                {
                    openValvesHash ^= nodeIndexes[v];
                }
                return openValvesHash;
            }
        }
    }

}

