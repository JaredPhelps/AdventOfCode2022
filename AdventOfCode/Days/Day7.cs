using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Days
{
    public class Day7
    {
        public int Solve(string input)
        {
            Directory root = ParseInput(input);
            List<Directory> list = new List<Directory>();
            GetDirectories(root, list);
            return list.Where(l => l.CalculateSize() <= 100000).Sum(l => l.CalculateSize());
        }

        public int SolvePart2(string input)
        {
            Directory root = ParseInput(input);
            List<Directory> list = new List<Directory>();
            GetDirectories(root, list);

            int freeSpace = 70000000 - root.CalculateSize();
            int neededSpace = 30000000 - freeSpace;
            return list.Where(l => l.CalculateSize() - neededSpace >= 0).OrderBy(l => l.CalculateSize() - neededSpace).First().CalculateSize();
        }

        public void GetDirectories(Directory parent, List<Directory> collector)
        {
            foreach (Directory subdir in parent.Subnodes.Values.Where(s => s is Directory))
            {
                collector.Add(subdir);
                GetDirectories(subdir, collector);
            }
        }

        public Directory ParseInput(string input)
        {
            var reader = new StringReader(input);
            string? line = reader.ReadLine(); // Skip cd /
            Directory root = new Directory(null, "/");
            Directory currentDir = root;

            while ((line = reader.ReadLine()) != null)
            {
                if (line.StartsWith("$ cd "))
                {
                    string dirName = line.Substring(5);
                    if (dirName == "..")
                    {
                        if (currentDir.Parent == null)
                            throw new Exception("Can't go higher than the top.");

                        currentDir = currentDir.Parent;
                    }
                    else
                    {
                        if (currentDir.Subnodes.ContainsKey(dirName))
                        {
                            currentDir = (Directory)currentDir.Subnodes[dirName];
                        }
                        else
                        {
                            throw new Exception("Can't navigate to a directory that isn't listed");
                        }
                    }
                }
                else if (line.StartsWith("$ ls"))
                {
                    // Anything to do here?
                    continue;
                }
                else if (line.StartsWith("dir "))
                {
                    currentDir.Subnodes.Add(line.Substring(4), new Directory(currentDir, line.Substring(4)));
                }
                else
                {
                    string[] parts = line.Split(' ');
                    currentDir.Subnodes.Add(parts[1], new File(int.Parse(parts[0]), parts[1]));
                }
            }

            return root;
        }
    }

    public abstract class Node
    {
        public string Name { get; set; }
        abstract public int CalculateSize();

        protected Node(string name)
        {
            Name = name;
        }
    }

    public class File : Node 
    {
        public File(int size, string name) : base(name)
        {
            Size = size;
        }
        public int Size { get; set; }
        override public int CalculateSize() => Size;
    }


    public class Directory : Node
    {
        public Directory? Parent { get; set; }
        public Dictionary<string, Node> Subnodes { get; set; }
        public Directory(Directory? parent, string name) : base(name)
        {
            Subnodes = new Dictionary<string, Node>();
            Parent = parent;
        }

        private int? cachedSize = null;
        override public int CalculateSize()
        {
            return cachedSize ?? (cachedSize = Subnodes.Values.Sum(n => n.CalculateSize())).Value;
        }
    }
}

