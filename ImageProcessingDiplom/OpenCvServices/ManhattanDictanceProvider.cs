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

        private int FindMinimumsDistanceToElementFrom(int index, int[,] distances2, int[,] distances3)
        {
            int min = BRISK_BYTE_COUNT;
            for (int j = 0; j < distances2.GetLength(1); j++)
            {
                if (distances2[index, j] < min)
                {
                    min = distances2[index, j];
                }
                if (distances3[index, j] < min)
                {
                    min = distances3[index, j];
                }
            }

            return min;
        }


        private int FindMinimumsDistanceToElementFromSameEtalon(int index, int[,] distances1)
        {
            int min = BRISK_BYTE_COUNT;
            for (int j = 0; j < distances1.GetLength(1); j++)
            {
                if (j == index) continue;
                if (distances1[index, j] < min)
                {
                    min = distances1[index, j];
                }
            }

            return min;
        }

        public int[] FindMinimumV(int[,] distances1, int[,] distances2, int[,] distances3)
        {
            int[] minimums = new int[distances2.GetLength(0)];

            for (int i = 0; i < distances2.GetLength(0); i++)
            {
                int pz_E = FindMinimumsDistanceToElementFrom(i, distances2, distances3);
                int pz_S = FindMinimumsDistanceToElementFromSameEtalon(i, distances1);
                minimums[i] = pz_E - pz_S;
            }

            return minimums;
        }

        public int[] GetTopIndices(int[] arr, int max)
        {
            // sort the array in descending order
            int[] sortedArr = arr.OrderByDescending(x => x).ToArray();

            // create an array to store the indices of the top values
            int[] topIndices = new int[max];
            int count = 0;
            for (int i = 0; i < arr.Length; i++)
            {
                if (sortedArr.Contains(arr[i]) && count < max)
                {
                    topIndices[count] = i;
                    count++;
                }
            }

            return topIndices;
        }

        public int[,] TakeTopElementsFromIndexes(int[,] arr, int[] indexes)
        {
            int[,] newArr = new int[indexes.Count(), indexes.Count()];
            for (int i = 0; i < indexes.Count(); i++)
            {
                for (int j = 0; j < indexes.Count(); j++)
                {
                    newArr[i,j] = arr[indexes[i], indexes[j]];
                }
            }

            return newArr;
        }

        public int[,] TakeTopElementsFrom2Indexes(int[,] arr, int[] indexes1, int[] indexes2)
        {
            int[,] newArr = new int[indexes1.Count(), indexes2.Count()];
            for (int i = 0; i < indexes1.Count(); i++)
            {
                for (int j = 0; j < indexes2.Count(); j++)
                {
                    newArr[i, j] = arr[indexes1[i], indexes2[j]];
                }
            }

            return newArr;
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
