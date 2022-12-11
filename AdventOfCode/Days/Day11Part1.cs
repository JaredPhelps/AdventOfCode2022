using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace AdventOfCode.Days
{
    public class Day11Part1
    {
        private IEnumerable<Monkey> monkeys = new List<Monkey>();
        public int Solve(string input, int numRounds = 20)
        {
            using var sr = new StringReader(input);
            monkeys = ParseInput(sr).Values.OrderBy(m => m.Id).ToList();

            for (int i = 0; i < numRounds; i++)
            {
                foreach (var monkey in monkeys)
                {
                    monkey.TakeTurn();
                }
            }

            var top2 = monkeys
                .OrderByDescending(m => m.InspectCount)
                .Take(2)
                .ToList();

            return top2[0].InspectCount * top2[1].InspectCount;
        }

        private Dictionary<int, Monkey> ParseInput(StringReader input)
        {
            var monkeys = new Dictionary<int, Monkey>();
            Monkey? currentMonkey = null;
            do
            {
                currentMonkey = ParseMonkey(input);
                if (currentMonkey != null)
                {
                    monkeys.Add(currentMonkey.Id, currentMonkey);
                    _ = input.ReadLine();
                }
            } while (currentMonkey != null);

            foreach (var monkey in monkeys.Values)
            {
                monkey.TrueMonkey = monkeys[monkey.TrueMonkeyId];
                monkey.FalseMonkey = monkeys[monkey.FalseMonkeyId];
            }

            return monkeys;
        }

        private Monkey? ParseMonkey(StringReader input)
        {
            string line = input.ReadLine()!;
            if (line == null)
                return null;
            int id = int.Parse(line.Substring("Monkey ".Length, 1));
            var itemWorryLevels = input.ReadLine()!
                .Split(' ', ',')
                .Where(p => int.TryParse(p, out int _))
                .Select(int.Parse)
                .ToList();

            line = input.ReadLine()!.Replace("Operation: new = old ", "").Trim();
            char op = line[0];
            int worryOperand = int.TryParse(line[2..], out int x) ? x : int.MinValue;
            line = input.ReadLine()!;
            int divisor = int.Parse(line["  Test: divisible by ".Length..]);
            line = input.ReadLine()!;
            int trueMonkeyId = line.Last() - '0';
            line = input.ReadLine()!;
            int falseMonkeyId = line.Last() - '0';

            return new Monkey()
            {
                Divisor = divisor,
                Id = id,
                WorryOperand = worryOperand,
                WorryOperator = op,
                TrueMonkeyId = trueMonkeyId,
                FalseMonkeyId = falseMonkeyId,
                Items = new Queue<Item>(itemWorryLevels.Select(i => new Item() {  WorryLevel= i, OriginalMonkeyId = id }))
            };
        }

        private record Item
        {
            public int WorryLevel { get; set; }
            public int OriginalMonkeyId { get; set; }
        }

        private class Monkey
        {
            public Queue<Item> Items { get; set; } = new Queue<Item>();
            public int Id { get; set; }
            public int Divisor { get; set; }
            public int WorryOperand { get; set; }
            public int TrueMonkeyId { get; set; }
            public int FalseMonkeyId { get; set; }
            public Monkey TrueMonkey { get; set; } = null!;
            public Monkey FalseMonkey { get; set; } = null!;
            public int InspectCount { get; private set; }
            public char WorryOperator { get; set; }

            public void TakeTurnPart2()
            {
                while (Items.Count > 0)
                {
                    Inspect();
                    GetBored();
                }
            }

            public void TakeTurn()
            {
                while (Items.Count > 0)
                {
                    Inspect();
                    GetBored();
                }
            }

            public void Inspect()
            {
                InspectCount++;
                var item = Items.Peek();
                checked
                {
                    if (WorryOperand == int.MinValue) // old * old
                        item.WorryLevel *= item.WorryLevel;
                    else if (WorryOperator == '*')
                        item.WorryLevel *= WorryOperand;
                    else
                        item.WorryLevel += WorryOperand;
                }
            }

            public void GetBored()
            {
                Item nextItem = Items.Dequeue();
                nextItem.WorryLevel = nextItem.WorryLevel / 3;
                if (nextItem.WorryLevel % Divisor == 0)
                {
                    TrueMonkey.CatchItem(nextItem);
                }
                else
                {
                    FalseMonkey.CatchItem(nextItem);
                }
            }

            public void CatchItem(Item item) => Items.Enqueue(item);
        }
    }
}

