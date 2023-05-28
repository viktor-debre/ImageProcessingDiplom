using Emgu.CV;

namespace ImageProcessingDiplom.OpenCvServices
{
    public struct VoteResult
    {
        public List<int> results;
    }

    public class HammingProvider
    {
        private const int DESC_NUMBER = 500;
        private const int DESC_SIZE = 512;
        private const int THREADHOLD_DESC = DESC_SIZE / 4;

        public int[,] FindHammingDistance(Mat descriptors1, Mat descriptors2)
        {
            var distances = new int[DESC_NUMBER, DESC_NUMBER];
            for (int i = 0; i < DESC_NUMBER; i++)
            {
                for (int j = 0; j < DESC_NUMBER; j++)
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

            for (int i = 0; i < DESC_NUMBER; ++i)
            {
                var descriptor = descriptors.GetRawData(i);

                var minDistance = DESC_SIZE;
                var indexOfEtalonToVote = -1;
                for (int j = 0; j < etalons.Count; ++j)
                {
                    var distance = FindMinHammingLenght(descriptor, etalons[j]);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        //Threshold
                        if (minDistance < THREADHOLD_DESC)
                        {
                            indexOfEtalonToVote = j;
                        }
                        //indexOfEtalonToVote = j;
                    }
                }
                if (indexOfEtalonToVote != -1)
                {
                    results.results[indexOfEtalonToVote] += 1;
                }
            }

            return results;
        }


        public VoteResult VoteMedoid(List<byte[]> medoids, Mat descriptors)
        {
            VoteResult results;
            results.results = new List<int>() { 0, 0, 0 };

            for (int i = 0; i < DESC_NUMBER; ++i)
            {
                var descriptor = descriptors.GetRawData(i);

                var minDistance = DESC_SIZE;
                var indexOfEtalonToVote = -1;
                for (int j = 0; j < medoids.Count; ++j)
                {
                    var distance = FindHammingLenghtForDescriptors(descriptor, medoids[j]);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        indexOfEtalonToVote = j;
                    }
                }
                if (indexOfEtalonToVote != -1)
                {
                    results.results[indexOfEtalonToVote] += 1;
                }
            }

            return results;
        }

        private int FindMinHammingLenght(byte[] descriptor, Mat etalon)
        {
            int minDistance = DESC_SIZE;
            for (int k = 0; k < DESC_NUMBER; ++k)
            {
                var distance = FindHammingLenghtForDescriptors(descriptor, etalon.GetRawData(k));
                if (distance < minDistance)
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

        public VoteResult FindMedoidMethodVoting(List<byte[]> medoids, byte[] descriptor)
        {
            VoteResult results;
            results.results = new List<int>() { 0, 0, 0 };

            int minDistance = int.MaxValue;
            int indexOfEtalonToVote = 0;
            for (int j = 0; j < medoids.Count; ++j)
            {
                var distance = FindHammingLenghtForDescriptors(descriptor, medoids[j]);
                Console.WriteLine($"Distance to {j+1}: {distance}");
                if (distance < minDistance)
                {
                    minDistance = distance;
                    indexOfEtalonToVote = j;
                }
            }

            results.results[indexOfEtalonToVote] += 1;

            return results;
        }
    }
}