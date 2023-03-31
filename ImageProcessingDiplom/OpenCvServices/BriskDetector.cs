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

            _detector = new Brisk();

            VectorOfKeyPoint keypoints = new VectorOfKeyPoint();
            _detector.DetectRaw(image, keypoints);

            Mat descriptors = new Mat();
            _detector.Compute(image, keypoints, descriptors);

            MKeyPoint[] mKeyPointsArray = keypoints.ToArray();
            List<MKeyPoint> mKeyPointsList = new List<MKeyPoint>();

            //int numKeyPoints = Math.Min(700, mKeyPointsArray.Length);
            for (int i = 0; i < mKeyPointsArray.Length - 1; i += 2)
            {
                mKeyPointsList.Add(mKeyPointsArray[i]);
            }
            VectorOfKeyPoint top500KeyPoints = new VectorOfKeyPoint(mKeyPointsList.ToArray());

            Keypoints = top500KeyPoints;
            Descriptors = descriptors;

            //Image<Bgr, byte> outputImage = new Image<Bgr, byte>(image.Size);
            //Features2DToolbox.DrawKeypoints(image, top500KeyPoints, outputImage, new Bgr(Color.Red), Features2DToolbox.KeypointDrawType.Default);

            //// Save the output image to a file
            //CvInvoke.Imwrite(filePath + "_result.png", outputImage.Mat);
        }
    }
}
