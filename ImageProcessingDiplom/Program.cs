using ImageProcessingDiplom;
using ImageProcessingDiplom.Interfaces;
using ImageProcessingDiplom.OpenCvServices;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

var services = new ServiceCollection();

var serviceProvider = services.AddServices()
    .BuildServiceProvider();

var _hamming = serviceProvider.GetService<IHammingProvider>();
var _manhattan = serviceProvider.GetService<IManhattanDictanceProvider>();
var _medoid = serviceProvider.GetService<MedoidFinder>();

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


var distances1 = _hamming.FindHammingDistance(detector1.Descriptors, detector1.Descriptors);
var distances2 = _hamming.FindHammingDistance(detector1.Descriptors, detector2.Descriptors);
var distances3 = _hamming.FindHammingDistance(detector1.Descriptors, detector3.Descriptors);

Stopwatch stopwatch = Stopwatch.StartNew();

var mathes1 = _manhattan.CountThresholdMathes(distances1);
Console.WriteLine("Mathes 1 with 1: " + mathes1);

var mathes2 = _manhattan.CountThresholdMathes(distances2);
Console.WriteLine("Mathes 1 with 2: " + mathes2);

var mathes3 = _manhattan.CountThresholdMathes(distances3);
Console.WriteLine("Mathes 1 with 3: " + mathes3);

stopwatch.Stop();

Console.WriteLine("distance array: " + distances1.Length + " time elapsed: " + stopwatch.ElapsedMilliseconds);

//Medoid search
var medoidIndex = _medoid.FindMedoidIndex(distances2);

var descriptors = detector1.Descriptors;
var descriptor = descriptors.Row(medoidIndex).GetRawData();
Console.WriteLine("Medoid is descriptor number " + medoidIndex + " with value: ");
foreach (var item in descriptor)
{
    Console.Write(item + " ");
}

var minDistanceIndex = _medoid.FindIndexOfMinDistance(distances2, medoidIndex);
var maxDstanceIndex = _medoid.FindIndexOfMaxDistance(distances2, medoidIndex);

var closestDescriptorToMedoid = descriptors.Row(minDistanceIndex).GetRawData();
var farthestDescriptorToMedoid = descriptors.Row(maxDstanceIndex).GetRawData();

Console.WriteLine("\n\nClosest to medoid is descriptor number " + minDistanceIndex + " with value: ");
foreach (var item in closestDescriptorToMedoid)
{
    Console.Write(item + " ");
}

Console.WriteLine("\n\nFarthest to medoid is descriptor number " + maxDstanceIndex + " with value: ");
foreach (var item in farthestDescriptorToMedoid)
{
    Console.Write(item + " ");
}