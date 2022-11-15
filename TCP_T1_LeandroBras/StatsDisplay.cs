using System;
using System.Collections.Generic;
using System.Text;

namespace TCP_T1_LeandroBras
{
    class StatsDisplay
    {
        private class AverageStats
        {
            public int Num { get; set; }
            public double DiAvg { get; set; }
            public double LAvg { get; set; }
            public double TAvg { get; set; }
            public bool IsTaskBased { get; }

            public AverageStats(bool isTask)
            {
                Num = 0;
                DiAvg = 0;
                LAvg = 0;
                TAvg = 0;
                IsTaskBased = isTask;
            }

            public override string ToString()
            {
                string diSpace = "";
                for (int i = 0; i < ((13 - (DiAvg.ToString("0.00") + " ms").Length) / 2); i++)
                    diSpace += " ";

                string lSpace = "";
                for (int i = 0; i < ((13 - (LAvg.ToString("0.00") + " ms").Length) / 2); i++)
                    lSpace += " ";

                string tSpace = "";
                for (int i = 0; i < ((13 - (TAvg.ToString("0.00") + " ms").Length) / 2); i++)
                    tSpace += " ";

                return String.Format($"| 2000x2000 |" +
                    $"{diSpace}{DiAvg.ToString("0.00")} ms{diSpace}|" +
                    $"{lSpace}{LAvg.ToString("0.00")} ms{lSpace}|" +
                    $"{tSpace}{TAvg.ToString("0.00")} ms{tSpace}");
            }
        }

        private List<MultiplicationStats> mStats;

        AverageStats avgStatsTask;
        AverageStats avgStatsLinear;

        private string[] averageTables =
        {
            "|-----------------------------------------------------|",
            "|                                                     |",
            "|-----------------------------------------------------|",
            "|    Size   | DoubleIndex | Linearized  | Transposed  |",
            "|-----------------------------------------------------|",
            "|           |             |             |             |",
            "|-----------------------------------------------------|"
            
        };

        public StatsDisplay(List<MultiplicationStats> mStats)
        {
            this.mStats = mStats;
        }

        public void DisplayStats()
        {
            Console.Clear();

            if (mStats == null)
            {
                Console.WriteLine("There are no stats to be displayed...\n");
                Console.WriteLine("Press Any Key to go back...");
                Console.ReadKey();
                return;
            }
            else
            {
                SortStats();
                PrintLinearTable();
                PrintTaskTable();
                Console.SetCursorPosition(0, 9);
                Console.WriteLine("All multiplications done so far:");
                Console.WriteLine();

                for (int i = 0; i < mStats.Count; i++)
                {
                    Console.WriteLine(" - " + mStats[i].ToString());
                }
            }
        }

        private void PrintLinearTable()
        {
            // Print Empty Table
            for (int i = 0; i < averageTables.Length; i++)
            {
                Console.SetCursorPosition(2, 1 + i);
                Console.Write(averageTables[i]);
            }
            string title = "Average Time for Linear Multiplications";
            string ms = avgStatsLinear.DiAvg.ToString("0.00") + " ms";

            Console.SetCursorPosition(3 + ((53 - title.Length) / 2), 2);
            Console.Write(title);
            Console.SetCursorPosition(2, 6);
            Console.Write(avgStatsLinear.ToString());
        }

        private void PrintTaskTable()
        {
            // Print Empty Table
            for (int i = 0; i < averageTables.Length; i++)
            {
                Console.SetCursorPosition(60, 1 + i);
                Console.Write(averageTables[i]);
            }
            string title = "Average Time for Task Multiplications";
            string ms = avgStatsTask.DiAvg.ToString("0.00") + " ms";

            Console.SetCursorPosition(61 + ((53 - title.Length) / 2), 2);
            Console.Write(title);
            Console.SetCursorPosition(60, 6);
            Console.Write(avgStatsTask.ToString());
        }

        private void SortStats()
        {
            avgStatsTask = new AverageStats(true);
            avgStatsLinear = new AverageStats(false);

            foreach (MultiplicationStats stats in mStats)
            {
                if (stats.IsTaskBased)
                {
                    avgStatsTask.Num++;
                    CheckStatToAdd(stats, avgStatsTask);
                }
                else
                {
                    avgStatsLinear.Num++;
                    CheckStatToAdd(stats, avgStatsLinear);
                }
            }

            avgStatsTask.Num /= 3;
            avgStatsTask.DiAvg /= avgStatsTask.Num;
            avgStatsTask.LAvg /= avgStatsTask.Num;
            avgStatsTask.TAvg /= avgStatsTask.Num;
            avgStatsLinear.Num /= 3;
            avgStatsLinear.DiAvg /= avgStatsLinear.Num;
            avgStatsLinear.LAvg /= avgStatsLinear.Num;
            avgStatsLinear.TAvg /= avgStatsLinear.Num;
        }

        private void CheckStatToAdd(MultiplicationStats stats, AverageStats avgStats)
        {
            MultiplicationType type = (MultiplicationType)Enum.Parse(typeof(MultiplicationType), stats.MType);

            switch (type)
            {
                case MultiplicationType.DoubleIndex:
                    avgStats.DiAvg += stats.ETime;
                    break;
                case MultiplicationType.Linearized:
                    avgStats.LAvg += stats.ETime;
                    break;
                case MultiplicationType.Transposed:
                    avgStats.TAvg += stats.ETime;
                    break;
            }
        }
    }
}
