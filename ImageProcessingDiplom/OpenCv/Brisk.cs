using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System.Drawing;

namespace ImageProcessingDiplom.OpenCvServices
{
    public class Brisk
    {
        public VectorOfKeyPoint Keypoints { get; set; }
        public Mat Descriptors { get; set; }
        private readonly Emgu.CV.Features2D.Brisk _detector;

        public Brisk(string filePath)
        {
            Mat image = CvInvoke.Imread(filePath + ".png", ImreadModes.Grayscale);

            _detector = new Emgu.CV.Features2D.Brisk();

            VectorOfKeyPoint keypoints = new VectorOfKeyPoint();
            _detector.DetectRaw(image, keypoints);

            Mat descriptors = new Mat();
            _detector.Compute(image, keypoints, descriptors);

            MKeyPoint[] mKeyPointsArray = keypoints.ToArray();
            List<MKeyPoint> mKeyPointsList = new List<MKeyPoint>();
            for (int i = 0; i < mKeyPointsArray.Length - 1; i += 2)
            {
                mKeyPointsList.Add(mKeyPointsArray[i]);
            }
            VectorOfKeyPoint top500KeyPoints = new VectorOfKeyPoint(mKeyPointsList.ToArray());

            Keypoints = top500KeyPoints;
            Descriptors = descriptors;
        }
    }
}
