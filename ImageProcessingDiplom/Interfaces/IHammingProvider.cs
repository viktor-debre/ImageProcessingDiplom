using Emgu.CV;

namespace ImageProcessingDiplom.Interfaces
{
    public interface IHammingProvider
    {
        public int[,] FindHammingDistance(Mat descriptors1, Mat descriptors2);
    }
}
