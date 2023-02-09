using ImageProcessingDiplom;
using ImageProcessingDiplom.Interfaces;
using ImageProcessingDiplom.OpenCvServices;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

//Dependency Injection
var services = new ServiceCollection();

var serviceProvider = services.AddServices()
    .BuildServiceProvider();

var _hamming = serviceProvider.GetService<IHammingProvider>();
var _manhattan = serviceProvider.GetService<IManhattanDictanceProvider>();
var _medoid = serviceProvider.GetService<MedoidFinder>();


//Program
string results = "";
var projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
var imagePathes = new List<string>
{
   projectPath + "\\Images\\image1.png",
   projectPath + "\\Images\\image2.png",
   projectPath + "\\Images\\image3.png"
};

BriskDetector detector1 = new BriskDetector(imagePathes[0]);
BriskDetector detector2 = new BriskDetector(imagePathes[1]);
BriskDetector detector3 = new BriskDetector(imagePathes[2]);

results += "Number of keypoints: " + detector1.Keypoints.Size + '\n';
Console.WriteLine("Number of keypoints: " + detector1.Keypoints.Size);
results += "Descriptors shape: " + detector1.Descriptors.Rows + " x " + detector1.Descriptors.Cols + '\n';
Console.WriteLine("Descriptors shape: " + detector1.Descriptors.Rows + " x " + detector1.Descriptors.Cols);

Stopwatch stopwatch = Stopwatch.StartNew();


var distances1 = _hamming.FindHammingDistance(detector1.Descriptors, detector1.Descriptors);
var distances2 = _hamming.FindHammingDistance(detector1.Descriptors, detector2.Descriptors);
var distances3 = _hamming.FindHammingDistance(detector1.Descriptors, detector3.Descriptors);


var mathes1 = _manhattan.CountThresholdMathes(distances1);
results += "Mathes 1 with 1: " + mathes1 + '\n';
Console.WriteLine("Mathes 1 with 1: " + mathes1);

var mathes2 = _manhattan.CountThresholdMathes(distances2);
results += "Mathes 1 with 2: " + mathes2 + '\n';
Console.WriteLine("Mathes 1 with 2: " + mathes2);

var mathes3 = _manhattan.CountThresholdMathes(distances3);
results += "Mathes 1 with 3: " + mathes3 + '\n';
Console.WriteLine("Mathes 1 with 3: " + mathes3);

stopwatch.Stop();
results += "distance array: " + distances1.Length + " time elapsed: " + stopwatch.ElapsedMilliseconds + '\n';
Console.WriteLine("distance array: " + distances1.Length + " time elapsed: " + stopwatch.ElapsedMilliseconds);

//Medoid search
var medoidIndex = _medoid.FindMedoidIndex(distances2);

var descriptors = detector1.Descriptors;
var descriptor = descriptors.Row(medoidIndex).GetRawData();
results += "Medoid is descriptor number " + (medoidIndex + 1) + " with value: " + '\n';
Console.WriteLine("Medoid is descriptor number " + (medoidIndex + 1) + " with value: ");
foreach (var item in descriptor)
{
    results += item + " ";
    Console.Write(item + " ");
}

var minDistanceIndex = _medoid.FindIndexOfMinDistance(distances2, medoidIndex);
var maxDstanceIndex = _medoid.FindIndexOfMaxDistance(distances2, medoidIndex);

var closestDescriptorToMedoid = descriptors.Row(minDistanceIndex).GetRawData();
var farthestDescriptorToMedoid = descriptors.Row(maxDstanceIndex).GetRawData();
results += "\n\nClosest to medoid is descriptor number " + (minDistanceIndex + 1) + " with value: " + '\n';
Console.WriteLine("\n\nClosest to medoid is descriptor number " + (minDistanceIndex + 1) + " with value: ");
foreach (var item in closestDescriptorToMedoid)
{
    results += item + " ";
    Console.Write(item + " ");
}

results += "\n\nFarthest to medoid is descriptor number " + (maxDstanceIndex + 1) + " with value: " + '\n';
Console.WriteLine("\n\nFarthest to medoid is descriptor number " + (maxDstanceIndex + 1) + " with value: ");
foreach (var item in farthestDescriptorToMedoid)
{
    results += item + " ";
    Console.Write(item + " ");
}

File.WriteAllText(projectPath + "\\results.txt", results);