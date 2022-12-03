using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Days
{
    public class Day3
    {
        public int Solve(string input)
        {
            return input.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
            .Select(i => new { Compartment1 = i.Substring(0, i.Length / 2), Compartment2 = i.Substring(i.Length / 2) })
            .Select(rs => rs.Compartment1.First(c => rs.Compartment2.Contains(c)))
            .Sum(c => GetPriority(c));
        }

        public int GetPriority(char c)
        {
            return Char.IsUpper(c) ? (c - 'A' + 27) : (c - 'a' + 1);
        }
    }
}
