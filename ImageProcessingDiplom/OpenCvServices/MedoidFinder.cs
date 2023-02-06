namespace ImageProcessingDiplom.OpenCvServices
{
    public class MedoidFinder
    {
        public int FindIndexOfMinDistance(int[,] distances, int medoidIndex)
        {
            int minDistance = 512;
            int minDictanceIndex = 0;

            for (int j = 0; j < distances.GetLength(1); j++)
            {
                if(distances[medoidIndex, j] < minDistance)
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

                if(summ < minValue)
                {
                    minValue = summ;
                    minIndex = i;
                }
            }

            return minIndex;
        }
    }
}
