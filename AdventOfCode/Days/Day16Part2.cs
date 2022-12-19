using System.Diagnostics;
using System.Numerics;
using System.Text;
using System.Xml.Linq;

namespace AdventOfCode.Days
{
    public class Day16Part2
    {
        public int Solve(string input, int minutesLeft)
        {
            Dictionary<string, Node> nodes = ParseInput(input);
            int i = 0;
            foreach (var n in nodes)
            {
                nodeIndexes.Add(n.Value.Name, 1L << i++);
            }

            NodePair startingNode = allNodePairs["AA"]["AA"];
            int score = startingNode.OptimalPath(minutesLeft, new HashSet<string>());
            return score;
        }

        private Dictionary<string, Node> ParseInput(string input)
        {
            string[] lines = input.Split("\r\n");
            List<string> nodeNames = lines.Select(l => l.Substring("Valve ".Length, 2)).ToList();

            var nodes = nodeNames.ToDictionary(nn => nn, nn => new Node(nn));


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

            for (int i = 0; i < nodeNames.Count; i++)
            {
                Node n1 = nodes[nodeNames[i]];
                
                for (int j = i; j < nodeNames.Count; j++)
                {
                    Node n2 = nodes[nodeNames[j]];
                    string key1 = n1.Name.CompareTo(n2.Name) < 0 ? n1.Name : n2.Name;
                    string key2 = n1.Name.CompareTo(n2.Name) < 0 ? n2.Name : n1.Name;
                    if (!allNodePairs.ContainsKey(key1))
                        allNodePairs.Add(key1, new Dictionary<string, NodePair>());

                    allNodePairs[key1].Add(key2, new NodePair(key1.CompareTo(key2) < 0 ? n1 : n2, key1.CompareTo(key2) < 0 ? n2 : n1));
                }
            }

            return nodes;
        }

        private static long cacheHits = 0;
        private static long cacheMisses = 0;
        private static Dictionary<string, long> nodeIndexes = new Dictionary<string, long>();
        private static Dictionary<(string, string, int, long), int> cache = new Dictionary<(string, string, int, long), int>();

        private static Dictionary<string, Dictionary<string, NodePair>> allNodePairs = new Dictionary<string, Dictionary<string, NodePair>>();

        private class NodePair
        {
            public Node n1;
            public Node n2;

            public NodePair(Node n1, Node n2)
            {
                this.n1 = n1;
                this.n2 = n2;

                if (n1.Name.CompareTo(n2.Name) < 0)
                {
                    this.n1 = n2;
                    this.n2 = n1;
                }
            }

            public IEnumerable<NodePair> BothStayChildren => BothLeaveChildren;
            public IEnumerable<NodePair> BothLeaveChildren
            {
                get
                {
                    foreach (var n1Child in n1.Children)
                    {
                        foreach (var n2Child in n2.Children)
                        {
                            Node n1 = n1Child.Name.CompareTo(n2Child.Name) < 0 ? n1Child : n2Child;
                            Node n2 = n1Child.Name.CompareTo(n2Child.Name) < 0 ? n2Child : n1Child;
                            yield return allNodePairs[n1.Name][n2.Name];
                        }
                    }
                }
            }

            public IEnumerable<NodePair> N1LeavesChildren
            {
                get
                {
                    foreach (var n1Child in n1.Children)
                    {
                        Node n1 = n1Child.Name.CompareTo(this.n2.Name) < 0 ? n1Child : this.n2;
                        Node n2 = n1Child.Name.CompareTo(this.n2.Name) < 0 ? this.n2 : n1Child;
                        yield return allNodePairs[n1.Name][n2.Name];
                    }
                }
            }

            public IEnumerable<NodePair> N2LeavesChildren
            {
                get
                {
                    foreach (var n2Child in n2.Children)
                    {
                        Node n1 = n2Child.Name.CompareTo(this.n1.Name) < 0 ? n2Child : this.n1;
                        Node n2 = n2Child.Name.CompareTo(this.n1.Name) < 0 ? this.n1 : n2Child;
                        yield return allNodePairs[n1.Name][n2.Name];
                    }
                }
            }

            public int OptimalPath(int minutesLeft, HashSet<string> openValves)
            {
                if (minutesLeft <= 1)
                    return 0;

                int? cachedValue = OptimalPathCache(minutesLeft, openValves);
                if (cachedValue != null)
                {
                    cacheHits++;
                    return cachedValue.Value;
                }
                cacheMisses++;

                if ((cacheHits + cacheMisses) % 100000 == 0)
                {
                    Debug.WriteLine(((decimal)cacheHits / (cacheHits + cacheMisses) * 100M).ToString("F2"));
                }

                long openValveSetHash = OpenValveSetHash(openValves);


                int bothOpenScore = 0;
                int n1OpenScore = 0;
                int n2OpenScore = 0;
                int neitherOpenScore = 0;

                // I can spend a minute opening my current valve then going to each of my children, or I can spend a minute going to each of my children.
                // But if my valve is open already, all I can do is travel to my children again.

                // We can both turn our valves
                if (n1.Name != n2.Name && !openValves.Contains(n1.Name) && !openValves.Contains(n2.Name) && n1.FlowRate > 0 && n2.FlowRate > 0)
                {
                    openValves.Add(n1.Name);
                    openValves.Add(n2.Name);
                    bothOpenScore = (n1.FlowRate + n2.FlowRate) * (minutesLeft - 1) + BothStayChildren.Max(c => c.OptimalPath(minutesLeft - 2, openValves));
                    openValves.Remove(n1.Name);
                    openValves.Remove(n2.Name);
                }

                // n1 turns their valve, n2 leaves.
                if (!openValves.Contains(n1.Name) && n1.FlowRate > 0)
                {
                    openValves.Add(n1.Name);
                    n1OpenScore = n1.FlowRate * (minutesLeft - 1) + N2LeavesChildren.Max(c => c.OptimalPath(minutesLeft - 1, openValves));
                    openValves.Remove(n1.Name);
                }

                // n2 turns their valve, n1 leaves.
                if (!openValves.Contains(n2.Name) && n2.FlowRate > 0)
                {
                    openValves.Add(n2.Name);
                    n2OpenScore = n2.FlowRate * (minutesLeft - 1) + N1LeavesChildren.Max(c => c.OptimalPath(minutesLeft - 1, openValves));
                    openValves.Remove(n2.Name);
                }

                // both leave
                if (true)
                {
                    neitherOpenScore = BothLeaveChildren.Max(c => c.OptimalPath(minutesLeft - 1, openValves));
                }

                string key1 = n1.Name.CompareTo(n2.Name) < 0 ? n1.Name : n2.Name;
                string key2 = n1.Name.CompareTo(n2.Name) < 0 ? n2.Name : n1.Name;
                int score = new[] { bothOpenScore, n1OpenScore, n2OpenScore, neitherOpenScore }.Max();
                cache.Add((key1, key2, minutesLeft, openValveSetHash), score);
                return score;
            }

            // We can cache values that indicate that you're at this node with this many minutes left and this set of open valves.
            public int? OptimalPathCache(int minutesLeft, HashSet<string> openValves)
            {
                string key1 = n1.Name.CompareTo(n2.Name) < 0 ? n1.Name : n2.Name;
                string key2 = n1.Name.CompareTo(n2.Name) < 0 ? n2.Name : n1.Name;

                long openValvesHash = OpenValveSetHash(openValves);
                if (cache.ContainsKey((key1, key2, minutesLeft, openValvesHash)))
                    return cache[(key1, key2, minutesLeft, openValvesHash)];

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

        private class Node
        {
            public List<Node> Children = new List<Node>();
            public string Name { get; set; }
            public int FlowRate { get; set; }
            public Node(string name)
            {
                Name = name;
            }
        }
    }

}

