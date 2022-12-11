using System.Diagnostics;
using System.Numerics;
using System.Text;

namespace AdventOfCode.Day11
{
    public class Day11Part2
    {
        private IEnumerable<Monkey> monkeys = new List<Monkey>();
        public long Solve(string input, int numRounds = 20, bool fancyMath = false)
        {
            using var sr = new StringReader(input);
            monkeys = ParseInput(sr).Values.OrderBy(m => m.Id).ToList();

            for (int i = 0; i < numRounds; i++)
            {
                Debug.WriteLine($"-- Round {i + 1} --");
                foreach (var monkey in monkeys)
                {
                    if (fancyMath)
                        monkey.TakeTurnFancyMath();
                    else
                        monkey.TakeTurn();
                }
            }

            var top2 = monkeys
                .OrderByDescending(m => m.InspectCount)
                .Take(2)
                .ToList();

            return (long)top2[0].InspectCount * (long)top2[1].InspectCount;
        }

        public override string ToString() => PrintMonkeys().ToString();

        private StringBuilder PrintMonkeys()
        {
            var sb = new StringBuilder();
            foreach (var m in monkeys.OrderBy(m => m.Id))
            {
                sb.AppendLine($"Monkey {m.Id} inspected items {m.InspectCount} times");
                sb.Append($"Monkey {m.Id} has {m.Items.Count} in its queue:");
                while (m.Items.Any())
                    sb.Append(m.Items.Dequeue().OriginalWorryLevel).Append(", ");
                sb.AppendLine();

            }
            return sb;
        }

        private Dictionary<int, Monkey> ParseInput(StringReader input)
        {
            var monkeys = new Dictionary<int, Monkey>();
            Monkey? currentMonkey = null;
            List<Item> allItems = new List<Item>();
            do
            {
                currentMonkey = ParseMonkey(input, allItems);
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

            allItems.ForEach(i => i.Monkeys = monkeys.Values);

            return monkeys;
        }

        private Monkey? ParseMonkey(StringReader input, List<Item> allItems)
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

            List<Item> startingItems = itemWorryLevels.Select(i => new Item() { OriginalWorryLevel = i, RollingWorryLevel = i }).ToList();
            allItems.AddRange(startingItems);

            return new Monkey()
            {
                Divisor = divisor,
                Id = id,
                WorryOperand = worryOperand,
                WorryOperator = op,
                TrueMonkeyId = trueMonkeyId,
                FalseMonkeyId = falseMonkeyId,
                Items = new Queue<Item>(startingItems)
            };
        }

        private class Item
        {
            public int RollingWorryLevel { get; set; }
            public int OriginalWorryLevel { get; set; }
            private IEnumerable<Monkey> monkeys = null!;
            public IEnumerable<Monkey> Monkeys
            {
                get => monkeys;
                set
                {
                    monkeys = value;
                    CurrentRemainderOfWorryLevel = value.ToDictionary(m => m, m => OriginalWorryLevel % m.Divisor);
                }
            }

            private Dictionary<Monkey, int> CurrentRemainderOfWorryLevel { get; set; } = new Dictionary<Monkey, int>();

            // When some monkey operates on this item's worry level, we have to tell all monkeys to update their remainder levels
            // of this item with that same operation.
            public void MonkeyDo(char op, int operand)
            {
                foreach (Monkey m in Monkeys)
                {
                    CurrentRemainderOfWorryLevel[m] = UpdateRemainder(CurrentRemainderOfWorryLevel[m], op, operand) % m.Divisor;
                }
            }

            public int RemainderForMonkey(Monkey m) => CurrentRemainderOfWorryLevel[m];

            public int UpdateRemainder(int remainder, char op, int operand)
            {
                checked
                {
                    if (operand == int.MinValue) // old * old
                        remainder *= remainder;
                    else if (op == '*')
                        remainder *= operand;
                    else
                        remainder += operand;
                }
                return remainder;
            }
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

            public void TakeTurnFancyMath()
            {
                while (Items.Count > 0)
                {
                    InspectFancyMath();
                    GetBoredFancyMath();
                }
            }

            public void InspectFancyMath()
            {
                InspectCount++;
                var item = Items.Peek();
                item.MonkeyDo(WorryOperator, WorryOperand);
            }

            public void GetBoredFancyMath()
            {
                Item nextItem = Items.Dequeue();
                if (nextItem.RemainderForMonkey(this) == 0)
                {
                    Debug.WriteLine($"Monkey {Id} is throwing {nextItem.OriginalWorryLevel} to Monkey {TrueMonkey.Id} because worry level is {nextItem.RemainderForMonkey(this)}");
                    TrueMonkey.CatchItem(nextItem);
                }
                else
                {
                    Debug.WriteLine($"Monkey {Id} is throwing {nextItem.OriginalWorryLevel} to Monkey {FalseMonkey.Id} because worry level is {nextItem.RemainderForMonkey(this)}");
                    FalseMonkey.CatchItem(nextItem);
                }
            }

            public void CatchItem(Item item) => Items.Enqueue(item);

            #region Old way with overflow
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
                        item.RollingWorryLevel *= item.RollingWorryLevel;
                    else if (WorryOperator == '*')
                        item.RollingWorryLevel *= WorryOperand;
                    else
                        item.RollingWorryLevel += WorryOperand;
                }
            }

            public void GetBored()
            {
                Item nextItem = Items.Dequeue();
                //nextItem.WorryLevel = nextItem.WorryLevel / 3;
                if (nextItem.RollingWorryLevel % Divisor == 0)
                {
                    Debug.WriteLine($"Monkey {Id} is throwing {nextItem.OriginalWorryLevel} to Monkey {TrueMonkey.Id} because worry level is {nextItem.RollingWorryLevel}");
                    TrueMonkey.CatchItem(nextItem);
                }
                else
                {
                    Debug.WriteLine($"Monkey {Id} is throwing {nextItem.OriginalWorryLevel} to Monkey {FalseMonkey.Id} because worry level is {nextItem.RollingWorryLevel}");
                    FalseMonkey.CatchItem(nextItem);
                }
            }
            #endregion
        }
    }
}

