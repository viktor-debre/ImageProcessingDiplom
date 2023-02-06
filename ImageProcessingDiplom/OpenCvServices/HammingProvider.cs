using Emgu.CV;
using ImageProcessingDiplom.Interfaces;

namespace ImageProcessingDiplom.OpenCvServices
{
    public class HammingProvider : IHammingProvider
    {
        private const int DESC_SIZE = 500;

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

        private int FindHammingLenghtForDescriptors(byte[] descriptor1, byte[] descriptor2)
        {
            int distance = 0;

            for (int i = 0; i < descriptor1.Length; ++i)
            {
                byte xorResult = (byte)(descriptor1[i] ^ descriptor2[i]);

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
    }
}