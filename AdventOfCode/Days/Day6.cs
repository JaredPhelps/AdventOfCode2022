using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Days
{
    public class Day6
    {
        public int Solve(string input, int distinctSetSize = 4)
        {
            // Find the position of the first character after the first set of unique characters of variable size.

            // Holds counts of the last distinctSetSize characters from i - distinctSetSize + 1 to i
            Dictionary<char, int> last4Counts= new Dictionary<char, int>(distinctSetSize);
            char? beginChar = null;
            int i;
            for (i = 0; i < distinctSetSize; i++)
            {
                AddChar(input[i]);
            }

            do
            {
                if (last4Counts.Count == distinctSetSize)
                    return i;

                beginChar = input[i - distinctSetSize];
                AddChar(input[i++]);

            } while (i < input.Length);
            return i;

            // seed with first distinctSetSize characters.
            void AddChar(char c)
            {
                if (!last4Counts.ContainsKey(c))
                    last4Counts.Add(c, 0);

                last4Counts[c]++;
                if (beginChar != null)
                {
                    last4Counts[beginChar.Value]--;

                    if (last4Counts[beginChar.Value] == 0)
                        last4Counts.Remove(beginChar.Value);
                }    
            }
        }
    }
}

