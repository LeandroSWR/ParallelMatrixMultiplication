using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace TCP_T1_LeandroBras
{
    class MatrixMultiplier
    {
        private const int x = 2000;
        private const int y = 2000;

        private float[] matA;
        private float[] matB;
        private float[,] mdMatA;
        private float[,] mdMatB;

        public List<MultiplicationStats> MStats { get; set; }

        public MatrixMultiplier()
        {
            InitializeMatrix();
            CheckForPreviousStats();
        }

        private void InitializeMatrix()
        {
            mdMatA = new float[x, y];
            mdMatB = new float[x, y];

            float value = 1f;

            for (int i = 0; i < y; i++)
                for (int j = 0; j < x; j++)
                {
                    mdMatA[j, i] = value;
                    mdMatB[j, i] = value++;
                }

            matA = new float[mdMatA.Length];
            matB = new float[mdMatB.Length];

            // Linearize the arrays for Linear multiplication
            Buffer.BlockCopy(mdMatA, 0, matA, 0, count: sizeof(float) * mdMatA.Length);
            Buffer.BlockCopy(mdMatB, 0, matB, 0, count: sizeof(float) * mdMatB.Length);
        }

        private void CheckForPreviousStats()
        {
            // Check to update stats
            if (File.Exists("mStats.json"))
            {
                UpdateMultiplicationStats();
            }
            else
            {
                MStats = new List<MultiplicationStats>();
            }
        }

        private void UpdateMultiplicationStats()
        {
            using (StreamReader f = new StreamReader("mStats.json"))
            {
                string jsonString = f.ReadToEnd();
                MStats = JsonConvert.DeserializeObject<List<MultiplicationStats>>(jsonString);
            }
        }

        private void SaveNewData()
        {
            string jsonString = JsonConvert.SerializeObject(MStats);
            File.WriteAllText("mStats.json", jsonString);
        }

        public void DoTaskMultiplications()
        {
            BaseMultiplier matMultiplier = new TaskBasedMultiplier(x, y);
            DoMultiplications(matMultiplier);
        }

        public void DoLinearMultiplications()
        {
            BaseMultiplier matMultiplier = new LinearMultiplier(x, y);
            DoMultiplications(matMultiplier);
        }

        public void DoAllMultiplications()
        {
            BaseMultiplier matMultiplier = new LinearMultiplier(x, y);
            Console.WriteLine("Starting Linear Multiplications:\n");
            DoMultiplications(matMultiplier);
            matMultiplier = new TaskBasedMultiplier(x, y);
            Console.WriteLine("Starting Task Based Multiplications:\n");
            DoMultiplications(matMultiplier);
        }

        private void DoMultiplications(BaseMultiplier matMultiplier)
        {
            double standard_time;

            // Start Stopwatch
            Stopwatch sw = Stopwatch.StartNew();

            //// Standard Multiplication
            Console.WriteLine("Performing standard multiplication...\n");
            float[,] mdMatC = matMultiplier.DoubleIndexMultiply(mdMatA, mdMatB);

            // Display Stats
            standard_time = sw.Elapsed.TotalMilliseconds;
            Console.WriteLine("Standard multiplication took {0} ms", standard_time.ToString("0.00"));
            Console.WriteLine("First element: {0} Last element: {1}\n", mdMatC[0, 0], mdMatC[x - 1, y - 1]);

            // Save Data
            MStats.Add(
                new MultiplicationStats(
                    standard_time, 
                    string.Format($"{x}x{y}"), 
                    MultiplicationType.DoubleIndex, 
                    matMultiplier is TaskBasedMultiplier));

            // Restart the Stopwatch
            sw = Stopwatch.StartNew();

            // Simple Linearized Index Multiplication
            Console.WriteLine("Performing Linearized Index Multiplication...\n");
            float[] matC = matMultiplier.LinearizedIndexMultiply(matA, matB);

            // Display Stats
            standard_time = sw.Elapsed.TotalMilliseconds;
            Console.WriteLine("Linearized Index Multiplication took {0} ms", standard_time.ToString("0.00"));
            Console.WriteLine("First element: {0} Last element: {1}\n", matC[0], matC[x * y - 1]);

            // Save Data
            MStats.Add(
                new MultiplicationStats(
                    standard_time,
                    string.Format($"{x}x{y}"),
                    MultiplicationType.Linearized,
                    matMultiplier is TaskBasedMultiplier));

            // Restart the Stopwatch
            sw = Stopwatch.StartNew();

            // Transposed Matrix Multiplication
            Console.WriteLine("Performing Transposed Multiplication...\n");
            float[] matD = matMultiplier.TransposedMultiply(matA, matB);

            // Display Stats
            standard_time = sw.Elapsed.TotalMilliseconds;
            Console.WriteLine("Transposed Multiplication took {0} ms", standard_time.ToString("0.00"));
            Console.WriteLine("First element: {0} Last element: {1}\n\n", matD[0], matD[x * y - 1]);
            sw.Stop();

            // Save Data
            MStats.Add(
                new MultiplicationStats(
                    standard_time,
                    string.Format($"{x}x{y}"),
                    MultiplicationType.Transposed,
                    matMultiplier is TaskBasedMultiplier));

            SaveNewData();
        }
    }
}
