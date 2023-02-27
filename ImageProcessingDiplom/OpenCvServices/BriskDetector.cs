using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Drawing;

namespace ImageProcessingDiplom.OpenCvServices
{
    public class BriskDetector
    {
        public VectorOfKeyPoint Keypoints { get; set; }

        public Mat Descriptors { get; set; }

        private readonly Brisk _detector;

        public BriskDetector(string filePath)
        {
            Mat image = CvInvoke.Imread(filePath + ".png", ImreadModes.Grayscale);

            _detector = new Brisk(10,4);

            VectorOfKeyPoint keypoints = new VectorOfKeyPoint();
            _detector.DetectRaw(image, keypoints);

            Mat descriptors = new Mat();
            _detector.Compute(image, keypoints, descriptors);

            Keypoints = keypoints;
            Descriptors = descriptors;

            Image<Bgr, byte> outputImage = new Image<Bgr, byte>(image.Size);
            Features2DToolbox.DrawKeypoints(image, keypoints, outputImage, new Bgr(Color.Green), Features2DToolbox.KeypointDrawType.Default);

            // Save the output image to a file
            CvInvoke.Imwrite(filePath + "_result.png", outputImage.Mat);
        }
    }
}
