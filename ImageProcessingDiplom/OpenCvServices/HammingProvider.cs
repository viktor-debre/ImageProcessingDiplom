using Emgu.CV;

namespace ImageProcessingDiplom.OpenCvServices
{
    public class HammingProvider
    {
        private const int DESC_SIZE = 500;
        private const double THRESHOLD_MATHES = 0.25f;
        private const int BRISK_BYTE_COUNT = 512;

        public int[,] FindHammingDistance(Mat descriptors1, Mat descriptors2)
        {
            var distances = new int[DESC_SIZE, DESC_SIZE];
            for (int i = 0; i < DESC_SIZE; i++)
            {
                for (int j = 0; j < DESC_SIZE; j++)
                {
                    distances[i, j] = FindHammingLenghtForDescriptors(descriptors1.GetRawData(i), (descriptors2.GetRawData(j)));
                }
            }

            return distances;
        }

        private int FindHammingLenghtForDescriptors(byte[] descriptors1, byte[] descriptors2)
        {
            int distance = 0;

            for (int i = 0; i < descriptors1.Length; ++i)
            {
                byte xorResult = (byte)(descriptors1[i] ^ descriptors2[i]);

                for (int j = 0; j < 8; j++)
                {
                    if ((xorResult & (1 << j)) != 0)
                    {
                        distance++;
                    }
                }
            }

            return distance;
        }

        public int CountThresholdMathes(int[,] distances)
        {
            int count = 0;

            for (int i = 0; i < DESC_SIZE; i++)
            {
                for (int j = 0; j < DESC_SIZE; j++)
                {
                    if (distances[i,j] < BRISK_BYTE_COUNT * THRESHOLD_MATHES)
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