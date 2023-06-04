using Emgu.CV;

namespace ImageProcessingDiplom.OpenCvServices
{
    public struct VoteResult
    {
        public List<int> results;
    }

    public class Distance
    {
        private const int DESC_NUMBER = 500;
        private const int DESC_SIZE = 512;
        private const int THREADHOLD_DESC = DESC_SIZE / 4;

        public List<int[,]> FindUniqueMathesForAllEtalon(List<Mat> etalons)
        {
            var result = new List<int[,]>();
            for (int i = 0; i < etalons.Count; i++)
            {
                result.Add(FindUniqueValue(etalons[i], etalons));
            }

            return result;
        }

        private int[,] FindUniqueValue(Mat comparingEtalon, List<Mat> etalons)
        {
            var result = new int[etalons.Count, DESC_NUMBER];


            for (int i = 0; i < DESC_NUMBER; i++)
            {
                for (int j = 0; j < etalons.Count; j++)
                {
                    result[j, i] = FindCountOfRepeadByHammingLenght(etalons[j].GetRawData(i), comparingEtalon);
                }
            }

            return result;
        }

        private int FindCountOfRepeadByHammingLenght(byte[] descriptor, Mat etalon)
        {
            int result = 0;
            for (int i = 0; i < DESC_NUMBER; ++i)
            {
                var distance = FindHammingLenghtForDescriptors(descriptor, etalon.GetRawData(i));
                if (distance < THREADHOLD_DESC)
                {
                    result += 1;
                }
            }

            return result;
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

        public int[,] FindHammingDistanceWithIndexes(Mat descriptors1, Mat descriptors2)
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
        public int[,] FindHammingDistanceWithIndexes(Mat descriptors1, Mat descriptors2, int[] indexes1, int[] indexes2)
        {
            int numIndexes1 = indexes1.Length;
            int numIndexes2 = indexes2.Length;

            var distances = new int[numIndexes1, numIndexes2];

            for (int i = 0; i < numIndexes1; i++)
            {
                for (int j = 0; j < numIndexes2; j++)
                {
                    int index1 = indexes1[i];
                    int index2 = indexes2[j];

                    distances[i, j] = FindHammingLenghtForDescriptors(descriptors1.GetRawData(index1), descriptors2.GetRawData(index2));
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

        public int[] GetIndexesOfTop100LeastElements(int[] inputArray)
        {
            if (inputArray.Length < 100)
            {
                throw new ArgumentException("Input array must have at least 100 elements.");
            }

            return inputArray
                .Select((value, index) => new { value, index })
                .OrderBy(x => x.value)
                .Take(100)
                .Select(x => x.index)
                .ToArray();
        }

        public int[] TakeOnlyRow(int indexNumber, int[,] array)
        {
            int[] ints = new int[500];
            for (int k = 0; k < 500; k++)
            {
                ints[k] = array[indexNumber, k];
            }

            return ints;
        }
    }
}