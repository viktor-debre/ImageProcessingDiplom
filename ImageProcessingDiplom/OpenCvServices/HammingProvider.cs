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
    }
}