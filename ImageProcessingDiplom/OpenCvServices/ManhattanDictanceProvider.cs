using ImageProcessingDiplom.Interfaces;

namespace ImageProcessingDiplom.OpenCvServices
{
    public class ManhattanDictanceProvider : IManhattanDictanceProvider
    {
        private const double THRESHOLD_MATHES = 0.25f;
        private const int BRISK_BYTE_COUNT = 512;

        public int CountThresholdMathes(int[,] distances)
        {
            int count = 0;

            for (int i = 0; i < distances.GetLength(0); i++)
            {
                for (int j = 0; j < distances.GetLength(1); j++)
                {
                    if (distances[i, j] < BRISK_BYTE_COUNT * THRESHOLD_MATHES)
                    {
                        count++;
                        break;
                    }
                }
            }

            return count;
        }
    }
}
