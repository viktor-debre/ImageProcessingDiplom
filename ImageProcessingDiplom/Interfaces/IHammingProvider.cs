using Emgu.CV;
using ImageProcessingDiplom.OpenCvServices;

namespace ImageProcessingDiplom.Interfaces
{
    public interface IHammingProvider
    {
        public int[,] FindHammingDistance(Mat descriptors1, Mat descriptors2);

        public VoteResult VoteEtalon(List<Mat> etalons, Mat descriptors);

        public int FindHammingLenghtForDescriptors(byte[] descriptor1, byte[] descriptor2);
    }
}
