using System.Text;

namespace AdventOfCode.Days
{
    public class Day10
    {
        public (int, String) Solve(string input)
        {
            var cpu = new Cpu();
            int result = 0;
            cpu.CycleFound += Cpu_CycleFound;
            string? line = null;
            using (var sr = new StringReader(input))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split(' ');
                    if (parts[0] == "noop")
                        cpu.Noop();
                    else
                        cpu.Add(int.Parse(parts[1]));
                }
            }
            return (result, cpu.crtScreen.ToString());

            void Cpu_CycleFound(object? sender, EventArgs e) => result += (int)sender;
        }


        public int SolvePart2(string input)
        {
            return 0;
        }

        private class Cpu
        {
            public StringBuilder crtScreen { get; } = new StringBuilder();
            int cycleCounter = 0;
            int x = 1;
            public event EventHandler? CycleFound;

            public void Add(int val)
            {
                AddCycle1(val);
                AddCycle2(val);
            }

            public void Noop() => IncrementCycle();

            private void IncrementCycle()
            {
                cycleCounter++;
                if ((cycleCounter - 20) % 40 == 0 && CycleFound != null)
                    CycleFound(x * cycleCounter, new EventArgs());
                crtScreen.Append(IsSpriteOnScreen ? "#" : ".");
                if (cycleCounter % 40 == 0)
                    crtScreen.AppendLine();
            }

            // True if X is within 1 of Cycle % 40
            private bool IsSpriteOnScreen => Math.Abs(x - ((cycleCounter - 1) % 40)) <= 1;

            private void AddCycle1(int val)
            {
                IncrementCycle();
            }

            private void AddCycle2(int val)
            {
                IncrementCycle();
                x += val;
            }
        }
    }
}

