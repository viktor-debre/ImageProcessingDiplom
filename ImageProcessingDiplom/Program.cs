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

BriskDetector detector1 = new BriskDetector(imagePathes[0]);
BriskDetector detector2 = new BriskDetector(imagePathes[1]);
BriskDetector detector3 = new BriskDetector(imagePathes[2]);

Console.WriteLine("Number of keypoints: " + detector1.Keypoints.Size);
Console.WriteLine("Descriptors shape: " + detector1.Descriptors.Rows + " x " + detector1.Descriptors.Cols);

var hamming = new HammingProvider();

Stopwatch stopwatch = Stopwatch.StartNew();

var distances1 = hamming.FindHammingDistance(detector1.Descriptors, detector1.Descriptors);
var distances2 = hamming.FindHammingDistance(detector1.Descriptors, detector2.Descriptors);
var distances3 = hamming.FindHammingDistance(detector1.Descriptors, detector3.Descriptors);

stopwatch.Stop();

Console.WriteLine("distance array: " + distances1.Length + " time elapsed: " + stopwatch.ElapsedMilliseconds);

var mathes1 = hamming.CountThresholdMathes(distances1);
Console.WriteLine("Mathes1: " + mathes1);

var mathes2 = hamming.CountThresholdMathes(distances2);
Console.WriteLine("Mathes2: " + mathes2);

var mathes3 = hamming.CountThresholdMathes(distances3);
Console.WriteLine("Mathes3: " + mathes3);