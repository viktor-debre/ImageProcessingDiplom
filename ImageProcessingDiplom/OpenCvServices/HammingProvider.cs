using Emgu.CV;
using ImageProcessingDiplom.Interfaces;

namespace ImageProcessingDiplom.OpenCvServices
{
    public struct VoteResult
    {
        public List<int> results;
    }

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

        public VoteResult VoteEtalon(List<Mat> etalons, Mat descriptors)
        {
            VoteResult results;
            results.results = new List<int>() { 0, 0, 0 };

            for (int i = 0; i < DESC_SIZE; ++i)
            {
                var descriptor = descriptors.GetRawData(i);

                var minDistance = 512;
                var indexOfEtalonToVote = -1;
                for (int j = 0; j < etalons.Count; ++j)
                {
                    var distance = FindMinHammingLenght(descriptor, etalons[j]);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        //Threshold
                        //if (minDistance < 128)
                        //{
                        //    indexOfEtalonToVote = j;
                        //}
                        indexOfEtalonToVote = j;
                    }
                }
                if( indexOfEtalonToVote != -1)
                {
                    results.results[indexOfEtalonToVote] += 1;
                }
            }

            return results;
        }

        private int FindMinHammingLenght(byte[] descriptor, Mat etalon)
        {
            int minDistance = 512;
            for (int k = 0; k < DESC_SIZE; ++k)
            {
                var distance = FindHammingLenghtForDescriptors(descriptor, etalon.GetRawData(k));
                if(distance < minDistance)
                {
                    minDistance = distance;
                }
            }
            
            return minDistance;
        }

        public int FindHammingLenghtForDescriptors(byte[] descriptor1, byte[] descriptor2)
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