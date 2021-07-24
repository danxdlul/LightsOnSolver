using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LightbulbSolver
{
    class Program
    {
        static void Main(string[] args)
        {

            List<int[,]> switches = readSwitchesFromFile(@"./InputFiles/Switches.txt");

            var initialState = readInitialStateFromFile(@"./InputFiles/InitialState.txt");

            if (initialState.Length == 0 || switches.Count == 0)
            {
                Console.WriteLine("You need to provide both initial state and different switches in the txt files that are located in InputFiles folder.");
                return;
            }

            var switchCombinations = Combinations(switches);   

            foreach(var combination in switchCombinations)
            {
                var actualState = initialState;
                foreach (var switchState in combination)
                {
                    applySwitch(switchState, ref actualState);
                }

                if (isSolved(actualState))
                {
                    Console.WriteLine(combination);
                    break;
                }
            }

        }
        public static IEnumerable<T[]> Combinations<T>(IEnumerable<T> source)
        {
            if (null == source)
                throw new ArgumentNullException(nameof(source));

            T[] data = source.ToArray();

            return Enumerable
              .Range(1, (1 << (data.Length)) - 1)
              .Select(index => data
                 .Where((v, i) => (index & (1 << i)) != 0)
                 .ToArray());
        }

        private static void applySwitch(int[,] switchState, ref int[,] actualState)
        {
            for(int i = 0;i < switchState.GetLength(1); i++)
            {
                for(int j = 0;j< switchState.GetLength(0); j++)
                {
                    if(switchState[i,j] == 1)
                    {
                        actualState[i, j] = actualState[i,j] == 1 ? 0 : 1;
                    }
                }
            }
        }

        private static bool isSolved(int[,] state)
        {
            foreach(var i in state)
            {
                if (i == 0)
                {
                    return false;
                }
            }
            return true;
        }

        private static int[,] readInitialStateFromFile(string file)
        {
            int i = 0, j = 0;
            string input = File.ReadAllText(file);
            int m = input.Split('\n')[0].Trim().Split(',').Length;
            int n = input.Split('\n').Length;
            int[,] result = new int[m, n];
            foreach (var row in input.Split('\n'))
            {
                j = 0;
                foreach (var col in row.Trim().Split(','))
                {
                    result[i, j] = int.Parse(col.Trim());
                    j++;
                }
                i++;
            }

            return result;
        }

        private static List<int[,]> readSwitchesFromFile(string file)
        {
            int i = 0, j = 0;
            List<int[,]> result = new List<int[,]>();
            string input = File.ReadAllText(file);

            List<string> switches = input.Split("\r\n\r\n").ToList();

            int m = switches[0].Split('\n')[0].Trim().Split(',').Length;
            int n = switches[0].Split('\n').Length;
            int[,] aSwitch = new int[m, n];

            foreach (var sw in switches)
            {
                i = 0;
                foreach (var row in sw.Split('\n'))
                {
                    j = 0;
                    foreach (var col in row.Trim().Split(','))
                    {
                        aSwitch[i, j] = int.Parse(col.Trim());
                        j++;
                    }
                    i++;
                }
                result.Add(aSwitch);
            }
            return result;
        }
    }
}
