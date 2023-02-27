using Emgu.CV;

namespace ImageProcessingDiplom.OpenCvServices
{
    public class MedoidFinder
    {
        private readonly HammingProvider _hamming;
        public MedoidFinder(HammingProvider hamming)
        {
            _hamming = hamming;
        }

        public int FindIndexOfMinDistance(int[,] distances, int medoidIndex)
        {
            int minDistance = 512;
            int minDictanceIndex = 0;

            for (int j = 0; j < distances.GetLength(1); j++)
            {
                if (distances[medoidIndex, j] < minDistance)
                {
                    minDistance = distances[medoidIndex, j];
                    minDictanceIndex = j;
                }
            }

            return minDictanceIndex;
        }

        public int FindIndexOfMaxDistance(int[,] distances, int medoidIndex)
        {
            int minDistance = 0;
            int maxDictanceIndex = 0;

            for (int j = 0; j < distances.GetLength(1); j++)
            {
                if (distances[medoidIndex, j] > minDistance)
                {
                    minDistance = distances[medoidIndex, j];
                    maxDictanceIndex = j;
                }
            }

            return maxDictanceIndex;
        }

        //From distance matrix calculated by HammingProvider find descriptor that index is same as min sum of columns
        public int FindMedoidIndex(int[,] distances)
        {
            int minValue = int.MaxValue;
            int minIndex = 0;

            for (int i = 0; i < distances.GetLength(0); i++)
            {
                int summ = 0;

                for (int j = 0; j < distances.GetLength(1); j++)
                {
                    summ += distances[i, j];
                }

                if (summ < minValue)
                {
                    minValue = summ;
                    minIndex = i;
                }
            }

            return minIndex;
        }

        public int FindMinDistanceForMedoid(/*byte[] medoid*/int[,] distances, int medoidIndex)
        {
            int minValue = int.MaxValue;
            for (int i = 0; i < distances.GetLength(1); i++)
            {
                if (distances[medoidIndex, i] < minValue && distances[medoidIndex, i] != 0)
                {
                    minValue = distances[medoidIndex, i];
                }
            }
            //for (int i = 0; i < medoid.Length; i++)
            //{
            //    if (medoid[i] < minValue && medoid[i] != 0)
            //    {
            //        minValue = medoid[i];
            //    }
            //}

            return minValue;
        }

        public int FindMaxDistanceForMedoid(/*byte[] medoid*/int[,] distances, int medoidIndex)
        {
            int maxValue = int.MinValue;
            for (int i = 0; i < distances.GetLength(1); i++)
            {
                if (distances[medoidIndex, i] > maxValue && distances[medoidIndex, i] != 0)
                {
                    maxValue = distances[medoidIndex, i];
                }
            }
            //for (int i = 0; i < medoid.Length; i++)
            //{
            //    if (medoid[i] < minValue && medoid[i] != 0)
            //    {
            //        minValue = medoid[i];
            //    }
            //}

            return maxValue;
        }

        public VoteResult TriangleMethodMin(Mat descriptors, List<byte[]> medoids, List<int> medoidMinElements)
        {
            VoteResult results;
            results.results = new List<int>() { 0, 0, 0 };

            for (int j = 0; j < 500; j++)
            {
                var descriptor = descriptors.GetRawData(j);

                int minVote = int.MaxValue;
                int minIndex = 0;
                for (int i = 0; i < medoids.Count; i++)
                {
                    int Ci = medoidMinElements[i];
                    int Bi = _hamming.FindHammingLenghtForDescriptors(medoids[i], descriptor);

                    int value = Ci + Bi;
                    if (value < minVote)
                    {
                        minVote = value;
                        minIndex = i;
                    }
                }

                results.results[minIndex] += 1;
            }

            return results;
        }

        public VoteResult TriangleMethodMax(Mat descriptors, List<byte[]> medoids, List<int> medoidMaxElements)
        {
            VoteResult results;
            results.results = new List<int>() { 0, 0, 0 };

            for (int j = 0; j < 500; j++)
            {
                var descriptor = descriptors.GetRawData(j);

                int maxVote = int.MinValue;
                int maxIndex = 0;
                for (int i = 0; i < medoids.Count; i++)
                {
                    int Ci = medoidMaxElements[i];
                    int Bi = _hamming.FindHammingLenghtForDescriptors(medoids[i], descriptor);

                    int value = Ci + Bi;
                    if (value > maxVote)
                    {
                        maxVote = value;
                        maxIndex = i;
                    }
                }

                results.results[maxIndex] += 1;
            }

            return results;
        }
    }
}
