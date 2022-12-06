using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Days
{
    public class Day4
    {
        public int Solve(string input)
        {
            return input.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
            .Select(i =>
            {
                int[] sizes = i.Split(',').SelectMany(p => p.Split('-')).Select(x => int.Parse(x)).ToArray();
                return new { x0 = sizes[0], y0 = sizes[1], x1 = sizes[2], y1 = sizes[3] };
            })
            .Count(p => RangesPartiallyOverlap(p.x0, p.y0, p.x1, p.y1));
        }

        public static bool RangesOverlap(int x0, int y0, int x1, int y1)
        {
            return (x0 <= x1 && y0 >= y1) || (x1 <= x0 && y1 >= y0);
        }

        public static bool RangesPartiallyOverlap(int x0, int y0, int x1, int y1)
        {
            return (x0 <= x1 && y0 >= x1) || (x1 <= x0 && y1 >= x0);
        }
    }
}
