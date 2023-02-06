using Emgu.CV;

namespace ImageProcessingDiplom.OpenCvServices
{
    public class HammingProvider
    {
        private const int descSize = 500;

        public int[,] FindHammingDistance(Mat descriptors1, Mat descriptors2)
        {
            var distances = new int[descSize, descSize];
            for (int i = 0; i < descSize; i++)
            {
                for (int j = 0; j < descSize; j++)
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
    }
}