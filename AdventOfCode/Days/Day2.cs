using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Days
{
    public class Day2
    {
        enum Play : int { Rock = 'A', Paper = 'B', Scissors = 'C' }
        public int Solve(string input)
        {
            return input.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)
            .Select(i => new { Opponent = (Play)i[0], Outcome = i[2] })
            .Sum(i => GetTotalScore(i.Opponent, i.Outcome));
        }

        int GetTotalScore(Play opponent, char outcome) => GetScore(outcome) + (int)GetMine(opponent, outcome) - 'A' + 1;
        int GetScore(char outcome) => outcome == 'Y' ? 3 : outcome == 'X' ? 0 : 6;
        Play GetMine(Play opponent, char outcome)
        {
            if (outcome == 'Y')
                return opponent;

            if (opponent == Play.Paper)
                return outcome == 'Z' ? Play.Scissors : Play.Rock;

            if (opponent == Play.Rock)
                return outcome == 'Z' ? Play.Paper : Play.Scissors;

            return outcome == 'Z' ? Play.Rock : Play.Paper;
        }
    }
}
