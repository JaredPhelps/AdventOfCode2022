using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Days
{
    public class Day8
    {
        public int Solve(string input)
        {
            TreeNode[,] map = ParseInput(input);
            int count = 0;
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(0); j++)
                {
                    TreeNode tree = map[i, j];

                    count += tree.IsVisible ? 1 : 0;
                }
            }

            return count;
        }
        public int SolvePart2(string input)
        {
            TreeNode[,] map = ParseInput(input);
            int max = 0;
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(0); j++)
                {
                    max = Math.Max(map[i, j].TreeCount, max);
                }
            }

            return max;
        }

        private TreeNode[,] ParseInput(string input)
        {
            int gridSize = input.IndexOf('\r');
            TreeNode[,] map = new TreeNode[gridSize, gridSize];
            string? line = null;
            int rowNum = 0;
            using var sr = new StringReader(input);
            while ((line = sr.ReadLine()) != null) 
            {
                for (int i = 0; i < gridSize; i++)
                    map[rowNum,i] = new TreeNode(map, i, rowNum, line[i] - '0');

                rowNum++;
            }
            return map;
        }

        private class TreeNode
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Height { get; set; }
            private TreeNode[,] map;
            public TreeNode(TreeNode[,] map, int x, int y, int height)
            {
                // It's advent of code and I can do this if I want to.
                this.map = map;
                this.X = x;
                this.Y = y;
                this.Height = height;
            }

            public TreeNode? Left => X > 0 ? map[Y, X - 1] : null;
            public TreeNode? Up => Y > 0 ? map[Y - 1, X] : null;
            public TreeNode? Right => X < map.GetLength(0) - 1 ? map[Y, X + 1] : null;
            public TreeNode? Down => Y < map.GetLength(0) - 1 ? map[Y + 1, X] : null;


            private int? biggestLeftCache, biggestRightCache, biggestUpCache, biggestDownCache;
            public int BiggestLeft => biggestLeftCache ?? (biggestLeftCache = Math.Max(Left?.BiggestLeft ?? -1, Left?.Height ?? -1)).Value;
            public int BiggestRight => biggestRightCache ?? (biggestRightCache = Math.Max(Right?.BiggestRight ?? -1, Right?.Height ?? -1)).Value;
            public int BiggestUp => biggestUpCache ?? (biggestUpCache = Math.Max(Up?.BiggestUp ?? -1, Up?.Height ?? -1)).Value;
            public int BiggestDown => biggestDownCache ?? (biggestDownCache = Math.Max(Down?.BiggestDown ?? -1, Down?.Height ?? -1)).Value;

            public bool IsVisibleLeft => BiggestLeft < Height;
            public bool IsVisibleRight => BiggestRight < Height;
            public bool IsVisibleUp => BiggestUp < Height;
            public bool IsVisibleDown => BiggestDown < Height;

            public bool IsVisible => IsVisibleLeft || IsVisibleRight || IsVisibleDown || IsVisibleUp;

            // treeCountXCache[i] represents how many trees to the X direction one encounters until meeting a tree of at least height i.
            private int?[] treeCountLeftCache = new int?[10], treeCountRightCache = new int?[10], treeCountUpCache = new int?[10], treeCountDownCache = new int?[10];

            public int TreeCountLeft(int height) => treeCountLeftCache[height] ??
                (treeCountLeftCache[height] = Left == null ? 0 :
                Left.Height >= height ? 1 :
                Left.TreeCountLeft(height) + 1).Value;

            public int TreeCountRight(int height) => treeCountRightCache[height] ??
                (treeCountRightCache[height] = Right == null ? 0 :
                Right.Height >= height ? 1 :
                Right.TreeCountRight(height) + 1).Value;

            public int TreeCountUp(int height) => treeCountUpCache[height] ??
                (treeCountUpCache[height] = Up == null ? 0 :
                Up.Height >= height ? 1 :
                Up.TreeCountUp(height) + 1).Value;

            public int TreeCountDown(int height) => treeCountDownCache[height] ??
                (treeCountDownCache[height] = Down == null ? 0 :
                Down.Height >= height ? 1 :
                Down.TreeCountDown(height) + 1).Value;

            public int TreeCount => TreeCountLeft(Height) * TreeCountRight(Height) * TreeCountDown(Height) * TreeCountUp(Height);
        }
    }
}

