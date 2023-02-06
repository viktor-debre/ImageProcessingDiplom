using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Util;

namespace ImageProcessingDiplom.OpenCvServices
{
    public class BriskDetector
    {
        public VectorOfKeyPoint Keypoints { get; set; }

        public Mat Descriptors { get; set; }

        private readonly Brisk _detector;

        public BriskDetector(string filePath)
        {
            Mat image = CvInvoke.Imread(filePath, ImreadModes.Grayscale);

            _detector = new Brisk();

            VectorOfKeyPoint keypoints = new VectorOfKeyPoint();
            _detector.DetectRaw(image, keypoints);

            Mat descriptors = new Mat();
            _detector.Compute(image, keypoints, descriptors);

            Keypoints = keypoints;
            Descriptors = descriptors;
        }
    }
}
