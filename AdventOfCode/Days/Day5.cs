using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Days
{
    public class Day5
    {
        private Dictionary<int, Stack<char>> boxes = new Dictionary<int, Stack<char>>();
        public string Solve(string input)
        {
            using StringReader sr = new StringReader(input);
            ParseBoxes(sr);
            foreach (var move in GetMoves(sr))
            {
                DoMove(move.quantity, move.source - 1, move.dest - 1);
            }

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < boxes.Count; i++)
                sb.Append(boxes[i].Peek());

            return sb.ToString();
        }

        private Stack<char> tempStack = new Stack<char>();
        private void DoMove(int quantity, int source, int dest)
        {
            tempStack.Clear();
            for (int i = 0; i < quantity; i++)
            {
                tempStack.Push(boxes[source].Pop());
            }

            for (int i = 0; i < quantity; i++)
            {
                boxes[dest].Push(tempStack.Pop());
            }
        }

        public Dictionary<int, Stack<char>> ParseBoxes(StringReader sr)
        {
            // Parse boxes until we get to the first blank line.
            string? line;
            while ((line = sr.ReadLine()) != null && !string.IsNullOrEmpty(line))
            {
                for (int cursor = 0; cursor < line.Length; cursor += 4)
                {
                    for (int j = cursor; j < cursor + 4 && j < line.Length; j++)
                    {
                        if (char.IsLetter(line[j]))
                        {
                            int columnNumber = cursor / 4;
                            if (!boxes.ContainsKey(columnNumber))
                                boxes[columnNumber] = new Stack<char>();

                            boxes[columnNumber].Push(line[j]);
                        }
                        else if (char.IsNumber(line[j]))
                        {
                            if (!boxes.ContainsKey(line[j] - '1'))
                                boxes.Add(line[j] - '1', new Stack<char>());
                        }
                    }
                }
            }

            // We pushed all these in reverse order, reverse them all.
            foreach (var s in boxes)
            {
                var reversed = boxes[s.Key];
                var tmp = new Stack<char>();
                while (reversed.Count > 0)
                    tmp.Push(reversed.Pop());

                boxes[s.Key] = tmp;
            }

            return boxes;
        }

        public IEnumerable<(int quantity, int source, int dest)> GetMoves(StringReader sr)
        {
            string? line = null;
            while ((line = sr.ReadLine()) != null)
            {
                // move 1 from 2 to 1
                string[] parts = line.Split(' ');
                yield return (int.Parse(parts[1]), int.Parse(parts[3]), int.Parse(parts[5]));
            }
        }
    }
}

