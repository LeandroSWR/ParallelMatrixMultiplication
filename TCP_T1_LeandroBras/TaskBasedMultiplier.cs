using System.Threading.Tasks;

namespace TCP_T1_LeandroBras
{
    class TaskBasedMultiplier : BaseMultiplier
    {
        private float[,] retMdMat;
        private float[] retMat;

        public TaskBasedMultiplier(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        struct MatLimit
        {
            public int fLine, lLine;
            public int fCol, lCol;

            public MatLimit(int fCol, int lCol, int fLine, int lLine)
            {
                this.fLine = fLine;
                this.lLine = lLine;
                this.fCol = fCol;
                this.lCol = lCol;
            }
        }

        public override float[,] DoubleIndexMultiply(float[,] matA, float[,] matB)
        {
            // Make sure the matrix we're creating to return has enough indexes
            retMdMat = new float[x, y];

            // Create the necessary tasks
            Task[] tasks = new Task[4];

            MatLimit matLim1 = new MatLimit(0, x / 2, 0, y / 2);
            MatLimit matLim2 = new MatLimit(0, x / 2, y / 2, y);
            MatLimit matLim3 = new MatLimit(x / 2, x, 0, y / 2);
            MatLimit matLim4 = new MatLimit(x / 2, x, y / 2, y);

            tasks[0] = Task.Factory.StartNew(() => MultiplyDoubleIndex(matLim1, matA, matB));
            tasks[1] = Task.Factory.StartNew(() => MultiplyDoubleIndex(matLim2, matA, matB));
            tasks[2] = Task.Factory.StartNew(() => MultiplyDoubleIndex(matLim3, matA, matB));
            tasks[3] = Task.Factory.StartNew(() => MultiplyDoubleIndex(matLim4, matA, matB));

            Task.WaitAll(tasks);

            // Check if the matrix is correct
            //for (int i = 0; i < y; i++)
            //{
            //    for (int j = 0; j < x; j++)
            //    {
            //        Console.Write($"|{retMdMat[j, i]}|");
            //    }
            //}

            return retMdMat;
        }

        private void MultiplyDoubleIndex(MatLimit data, float[,] matA, float[,] matB)
        {
            Parallel.For(data.fLine, data.lLine, i =>
            {
                for (int j = data.fCol; j < data.lCol; j++)
                {
                    float temp = 0;
                    for (int k = 0; k < x; k++)
                    {
                        temp += matA[i, k] * matB[k, j];
                    }
                    retMdMat[i, j] = temp;
                }
            });
        }

        public override float[] LinearizedIndexMultiply(float[] matA, float[] matB)
        {
            // Make sure the matrix we're creating to return has enough indexes
            retMat = new float[matA.Length];

            // Create the necessary tasks
            Task[] tasks = new Task[4];

            MatLimit matLim1 = new MatLimit(0, x / 2, 0, y / 2);
            MatLimit matLim2 = new MatLimit(0, x / 2, y / 2, y);
            MatLimit matLim3 = new MatLimit(x / 2, x, 0, y / 2);
            MatLimit matLim4 = new MatLimit(x / 2, x, y / 2, y);

            tasks[0] = Task.Factory.StartNew(() => MultiplyLinearized(matLim1, matA, matB));
            tasks[1] = Task.Factory.StartNew(() => MultiplyLinearized(matLim2, matA, matB));
            tasks[2] = Task.Factory.StartNew(() => MultiplyLinearized(matLim3, matA, matB));
            tasks[3] = Task.Factory.StartNew(() => MultiplyLinearized(matLim4, matA, matB));

            Task.WaitAll(tasks);

            // Check if the matrix is correct
            //for (int i = 0; i < y * x; i++)
            //{
            //    Console.Write($"|{retMat[i]}|");
            //}

            return retMat;
        }

        private void MultiplyLinearized(MatLimit data, float[] matA, float[] matB)
        {
            Parallel.For(data.fLine, data.lLine, i =>
            {
                for (int j = data.fCol; j < data.lCol; j++)
                {
                    float temp = 0;
                    for (int k = 0; k < x; k++)
                    {
                        temp += matA[i * x + k] * matB[k * x + j];
                    }
                    retMat[j * x + i] = temp;
                }
            });
        }

        public override float[] TransposedMultiply(float[] matA, float[] matB)
        {
            // Make sure the matrix we're creating to return has enough indexes
            retMat = new float[matA.Length];

            // Transpose B matrix
            matB = Transpose(matB);

            // Create the necessary tasks
            Task[] tasks = new Task[4];

            MatLimit matLim1 = new MatLimit(0, x / 2, 0, y / 2);
            MatLimit matLim2 = new MatLimit(0, x / 2, y / 2, y);
            MatLimit matLim3 = new MatLimit(x / 2, x, 0, y / 2);
            MatLimit matLim4 = new MatLimit(x / 2, x, y / 2, y);

            tasks[0] = Task.Factory.StartNew(() => MultiplyTransposed(matLim1, matA, matB));
            tasks[1] = Task.Factory.StartNew(() => MultiplyTransposed(matLim2, matA, matB));
            tasks[2] = Task.Factory.StartNew(() => MultiplyTransposed(matLim3, matA, matB));
            tasks[3] = Task.Factory.StartNew(() => MultiplyTransposed(matLim4, matA, matB));

            Task.WaitAll(tasks);

            // Check if the matrix is correct
            //for (int i = 0; i < y * x; i++)
            //{
            //    Console.Write($"|{retMat[i]}|");
            //}

            return retMat;
        }

        private void MultiplyTransposed(MatLimit data, float[] matA, float[] matB)
        {
            Parallel.For(data.fLine, data.lLine, i =>
            {
                for (int j = data.fCol; j < data.lCol; j++)
                {
                    float temp = 0;
                    for (int k = 0; k < x; k++)
                    {
                        temp += matA[i * x + k] * matB[j * x + k];
                    }
                    retMat[j * x + i] = temp;
                }
            });
        }
    }
}
