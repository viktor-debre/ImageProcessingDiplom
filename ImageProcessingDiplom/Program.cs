using ImageProcessingDiplom.OpenCvServices;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

var serviceProvider = new ServiceCollection()
    .BuildServiceProvider();
var imagePathes = new List<string>
{
    "D:\\repos\\diplom\\image1.png",
    "D:\\repos\\diplom\\image2.png",
    "D:\\repos\\diplom\\image3.png"
};

BriskDetector detector = new BriskDetector(imagePathes[0]);

Console.WriteLine("Number of keypoints: " + detector.Keypoints.Size);
Console.WriteLine("Descriptors shape: " + detector.Descriptors.Rows + " x " + detector.Descriptors.Cols);

var hamming = new HammingProvider();

Stopwatch stopwatch = Stopwatch.StartNew();

var distances = hamming.FindHammingDistance(detector.Descriptors, detector.Descriptors);


stopwatch.Stop();


Console.WriteLine("distance array: " + distances.Length + " time elapsed: " + stopwatch.ElapsedMilliseconds);