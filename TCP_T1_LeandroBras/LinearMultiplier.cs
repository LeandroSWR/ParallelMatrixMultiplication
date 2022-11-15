namespace TCP_T1_LeandroBras
{
    class LinearMultiplier : BaseMultiplier
    {
        public LinearMultiplier(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        
        public override float[,] DoubleIndexMultiply(float[,] matA, float[,] matB)
        {
            // Make sure the matrix we're creating to return has enough indexes
            float[,] retMat = new float[x, y];

            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    retMat[i, j] = 0;
                    for (int k = 0; k < y; k++)
                    {
                        retMat[i, j] += matA[i, k] * matB[k, j];
                    }
                }
            }

            // Check if the matrix is correct
            //for (int i = 0; i < y; i++)
            //{
            //    for (int j = 0; j < x; j++)
            //    {
            //        Console.Write($"|{retMat[j, i]}|");
            //    }
            //}

            return retMat;
        }

        public override float[] LinearizedIndexMultiply(float[] matA, float[] matB)
        {
            // Make sure the matrix we're creating to return has enough indexes
            float[] retMat = new float[x * y];

            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    float temp = 0;
                    
                    for (int k = 0; k < x; k++)
                    {
                        temp += matA[i * x + k] * matB[k * x + j];
                    }

                    retMat[j * x + i] = temp;
                }
            }

            // Check if the matrix is correct
            //for (int i = 0; i < y * x; i++)
            //{
            //    Console.Write($"|{retMat[i]}|");
            //}

            return retMat;
        }

        public override float[] TransposedMultiply(float[] matA, float[] matB)
        {
            // Make sure the matrix we're creating to return has enough indexes
            float[] retMat = new float[x * y];

            // Transpose B matrix
            matB = Transpose(matB);

            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    float temp = 0;
                    
                    for (int k = 0; k < x; k++)
                    {
                        temp += matA[i * x + k] * matB[j * x + k];
                    }

                    retMat[j * x + i] = temp;
                }
            }

            // Check if the matrix is correct
            //for (int i = 0; i < y * x; i++)
            //{
            //    Console.Write($"|{retMat[i]}|");
            //}

            return retMat;
        }
    }
}
