using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Days
{
    public class Day3Part2
    {
        public int Solve(string input)
        {
            return input.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select((l, index) => new { Line = l, Group = index / 3 })
                .GroupBy(l => l.Group)
                .Select(g => g.First().Line.First(c => g.All(l => l.Line.Contains(c))))
                .Sum(c => GetPriority(c));
        }

        public int GetPriority(char c)
        {
            return Char.IsUpper(c) ? (c - 'A' + 27) : (c - 'a' + 1);
        }
    }
}
