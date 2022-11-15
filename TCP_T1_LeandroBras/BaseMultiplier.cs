namespace TCP_T1_LeandroBras
{
    public abstract class BaseMultiplier
    {
        protected int x, y;

        public abstract float[,] DoubleIndexMultiply(float[,] matA, float[,] matB);

        public abstract float[] LinearizedIndexMultiply(float[] matA, float[] matB);

        public abstract float[] TransposedMultiply(float[] matA, float[] matB);

        protected float[] Transpose(float[] mat)
        {
            float[] transposed = new float[mat.Length];

            for (int i = 0; i < x; i++)
            {

                for (int j = 0; j < y; j++)
                {

                    transposed[i + j * y] = mat[j + i * x];
                }
            }

            return transposed;
        }
    }
}
