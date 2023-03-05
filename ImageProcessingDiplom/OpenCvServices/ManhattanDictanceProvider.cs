namespace ImageProcessingDiplom.OpenCvServices
{
    public class ManhattanDictanceProvider
    {
        private const double THRESHOLD_MATHES = 0.25f;
        private const int BRISK_BYTE_COUNT = 512;

        public int CountThresholdMathes(int[,] distances)
        {
            var minimums = FindMinimumsOfDistances(distances);
            int count = 0;

            for (int i = 0; i < minimums.Length; i++)
            {
                if (minimums[i] < BRISK_BYTE_COUNT * THRESHOLD_MATHES)
                {
                    count++;
                }
            }

            return count;
        }

        public bool MatrixIsSymmetric(int[,] distances)
        {
            bool result = true;
            for (int i = 0; i < distances.GetLength(0); i++)
            {
                for (int j = 0; j < distances.GetLength(0); j++)
                {
                    if (distances[i, j] != distances[j, i])
                    {
                        result = false;
                    }
                }
            }

            return result;
        }

        public int[] FindMinimumsOfDistances(int[,] distances)
        {
            int[] minimums = new int[distances.GetLength(0)];

            for (int i = 0; i < distances.GetLength(0); i++)
            {
                minimums[i] = BRISK_BYTE_COUNT;
                for (int j = 0; j < distances.GetLength(1); j++)
                {
                    if (distances[i, j] < minimums[i])
                    {
                        minimums[i] = distances[i, j];
                    }
                }
            }

            return minimums;
        }
    }
}
