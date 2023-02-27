using Emgu.CV;
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
   projectPath + "\\Images\\image1",
   projectPath + "\\Images\\image2",
   projectPath + "\\Images\\image3",
   projectPath + "\\Images\\image4"
};

BriskDetector detector1 = new BriskDetector(imagePathes[0]);
BriskDetector detector2 = new BriskDetector(imagePathes[1]);
BriskDetector detector3 = new BriskDetector(imagePathes[2]);
BriskDetector detector4 = new BriskDetector(imagePathes[3]);

var descriptors1 = detector1.Descriptors;
var descriptors2 = detector2.Descriptors;
var descriptors3 = detector3.Descriptors;
var descriptors4 = detector4.Descriptors;


//Vote etalones
Stopwatch stopwatch = Stopwatch.StartNew();
List<Mat> etalons = new List<Mat>() { detector1.Descriptors, detector2.Descriptors, detector3.Descriptors };
VoteResult voteResult1 = _hamming.VoteEtalon(etalons, detector4.Descriptors);


int index = 1;
foreach (var item in voteResult1.results)
{
    Console.WriteLine($"Mathes 4 image with {index} etalon: {item}");
    index++;
}

//VoteResult voteResult2 = _hamming.VoteEtalon(etalons, detector1.Descriptors);
//index = 1;

//foreach (var item in voteResult2.results)
//{
//    Console.WriteLine($"Mathes 1 image with {index} etalon: {item}");
//    index++;
//}

//var mathes11 = _manhattan.CountThresholdMathes(distances11);
//results += "Mathes 1 with 1: " + mathes11 + '\n';
//Console.WriteLine("Mathes 1 with 1: " + mathes11);

//var mathes12 = _manhattan.CountThresholdMathes(distances12);
//results += "Mathes 1 with 2: " + mathes12 + '\n';
//Console.WriteLine("Mathes 1 with 2: " + mathes12);

//var mathes13 = _manhattan.CountThresholdMathes(distances13);
//results += "Mathes 1 with 3: " + mathes13 + '\n';
//Console.WriteLine("Mathes 1 with 3: " + mathes13);

//var mathes22 = _manhattan.CountThresholdMathes(distances22);
//results += "Mathes 2 with 2: " + mathes22 + '\n';
//Console.WriteLine("Mathes 2 with 2: " + mathes22);

//var mathes23 = _manhattan.CountThresholdMathes(distances23);
//results += "Mathes 2 with 3: " + mathes23 + '\n';
//Console.WriteLine("Mathes 2 with 3: " + mathes23);

//var mathes33 = _manhattan.CountThresholdMathes(distances33);
//results += "Mathes 3 with 3: " + mathes33 + '\n';
//Console.WriteLine("Mathes 3 with 3: " + mathes33);

stopwatch.Stop();
results += "Voting method time elapsed: " + stopwatch.ElapsedMilliseconds + '\n';
Console.WriteLine("Voting method time elapsed: " + stopwatch.ElapsedMilliseconds);


Stopwatch stopwatch3 = Stopwatch.StartNew();
// distances
var distances11 = _hamming.FindHammingDistance(detector1.Descriptors, detector1.Descriptors);

//var distances12 = _hamming.FindHammingDistance(detector1.Descriptors, detector2.Descriptors);
//var distances13 = _hamming.FindHammingDistance(detector1.Descriptors, detector3.Descriptors);
var distances22 = _hamming.FindHammingDistance(detector2.Descriptors, detector2.Descriptors);
//var distances23 = _hamming.FindHammingDistance(detector2.Descriptors, detector3.Descriptors);
var distances33 = _hamming.FindHammingDistance(detector3.Descriptors, detector3.Descriptors);
stopwatch3.Stop();
Console.WriteLine("Matrix etalon time elapsed: " + stopwatch3.ElapsedMilliseconds);
/////////////////////////////////
//Medoid search
Stopwatch stopwatch2 = Stopwatch.StartNew();

var medoidIndex1 = _medoid.FindMedoidIndex(distances11);
var medoidDescriptor1 = descriptors1.Row(medoidIndex1).GetRawData();
results += "Medoid 1 is descriptor number " + (medoidIndex1 + 1) + '\n';
Console.WriteLine("Medoid is descriptor number " + (medoidIndex1 + 1));

var medoidIndex2 = _medoid.FindMedoidIndex(distances22);
var medoidDescriptor2 = descriptors2.Row(medoidIndex2).GetRawData();
results += "Medoid 2 is descriptor number " + (medoidIndex2 + 1) + '\n';
Console.WriteLine("Medoid is descriptor number " + (medoidIndex2 + 1));

var medoidIndex3 = _medoid.FindMedoidIndex(distances33);
var medoidDescriptor3 = descriptors3.Row(medoidIndex3).GetRawData();
results += "Medoid 3 is descriptor number " + (medoidIndex3 + 1) + '\n';
Console.WriteLine("Medoid is descriptor number " + (medoidIndex3 + 1));

List<byte[]> medoids = new List<byte[]>() { medoidDescriptor1, medoidDescriptor2, medoidDescriptor3 };

Console.WriteLine("min method");
int minMedoid1 = _medoid.FindMinDistanceForMedoid(distances11, medoidIndex1);
int minMedoid2 = _medoid.FindMinDistanceForMedoid(distances22, medoidIndex2);
int minMedoid3 = _medoid.FindMinDistanceForMedoid(distances33, medoidIndex3);
//int minMedoid1 = _medoid.FindMinDistanceForMedoid(descriptors1.GetRawData(medoidIndex1);
//int minMedoid2 = _medoid.FindMinDistanceForMedoid(descriptors2.GetRawData(medoidIndex2));
//int minMedoid3 = _medoid.FindMinDistanceForMedoid(descriptors3.GetRawData(medoidIndex3));

List<int> minMedoidElements = new List<int>() { minMedoid1, minMedoid2, minMedoid3 };

VoteResult voteResult2 = _medoid.TriangleMethodMin(descriptors4,medoids,minMedoidElements);

int index2 = 1;
foreach (var item in voteResult2.results)
{
    //var persent = FindPrecision(voteResult1.results[index2 - 1], item);
    Console.WriteLine($"Mathes 4 image with {index2} etalon: {item} with precision: %");
    index2++;
}

Console.WriteLine("max method");
int maxMedoid1 = _medoid.FindMaxDistanceForMedoid(distances11, medoidIndex1);
int maxMedoid2 = _medoid.FindMaxDistanceForMedoid(distances22, medoidIndex2);
int maxMedoid3 = _medoid.FindMaxDistanceForMedoid(distances33, medoidIndex3);

List<int> maxMedoidElements = new List<int>() { maxMedoid1, maxMedoid2, maxMedoid3 };

VoteResult voteResult3 = _medoid.TriangleMethodMax(descriptors4, medoids, maxMedoidElements);

int index3 = 1;

foreach (var item in voteResult3.results)
{
    //var persent = FindPrecision(voteResult1.results[index3 - 1], item);
    Console.WriteLine($"Mathes 4 image with {index3} etalon: {item} with precision: %");
    index3++;
}
//var minDistanceIndex = _medoid.FindIndexOfMinDistance(distances11, medoidIndex);
//var maxDstanceIndex = _medoid.FindIndexOfMaxDistance(distances11, medoidIndex);

//var closestDescriptorToMedoid = descriptors.Row(minDistanceIndex).GetRawData();
//var farthestDescriptorToMedoid = descriptors.Row(maxDstanceIndex).GetRawData();
//results += "\n\nClosest to medoid is descriptor number " + (minDistanceIndex + 1) + " with value: " + '\n';
//Console.WriteLine("\n\nClosest to medoid is descriptor number " + (minDistanceIndex + 1) + " with value: ");
//foreach (var item in closestDescriptorToMedoid)
//{
//    results += item + " ";
//    Console.Write(item + " ");
//}

//results += "\n\nFarthest to medoid is descriptor number " + (maxDstanceIndex + 1) + " with value: " + '\n';
//Console.WriteLine("\n\nFarthest to medoid is descriptor number " + (maxDstanceIndex + 1) + " with value: " + '\n');
//foreach (var item in farthestDescriptorToMedoid)
//{
//    results += item + " ";
//    Console.Write(item + " ");
//}

stopwatch2.Stop();
results += "Medoid method time elapsed: " + stopwatch2.ElapsedMilliseconds + '\n';
Console.WriteLine("Medoid method time elapsed: " + stopwatch2.ElapsedMilliseconds);

File.WriteAllText(projectPath + "\\results.txt", results);