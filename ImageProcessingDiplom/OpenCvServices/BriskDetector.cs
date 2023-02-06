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
            // Load an image
            Mat image = CvInvoke.Imread("D:\\repos\\diplom\\image1.png", ImreadModes.Grayscale);

            // Create the BRISK detector
            _detector = new Brisk();

            // Detect keypoints
            VectorOfKeyPoint keypoints = new VectorOfKeyPoint();
            _detector.DetectRaw(image, keypoints);

            // Compute descriptors
            Mat descriptors = new Mat();

            _detector.Compute(image, keypoints, descriptors);

            Keypoints = keypoints;
            Descriptors = descriptors;
        }
    }
}
