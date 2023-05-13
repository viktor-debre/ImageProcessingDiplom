using Emgu.CV;
using ImageProcessingDiplom;
using ImageProcessingDiplom.OpenCvServices;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

//Dependency Injection
var services = new ServiceCollection();

var serviceProvider = services.AddServices()
    .BuildServiceProvider();

var _hamming = serviceProvider.GetService<HammingProvider>();
var _manhattan = serviceProvider.GetService<ManhattanDictanceProvider>();
var _medoid = serviceProvider.GetService<MedoidFinder>();


//Program
string results = "";
var projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
var imagePathes = new List<string>
{
   projectPath + "\\Images\\image1",
   projectPath + "\\Images\\image2",
   projectPath + "\\Images\\image3",
   projectPath + "\\Images\\image4",
   projectPath + "\\Images\\image5",
};

BriskDetector detector1 = new BriskDetector(imagePathes[0]);
BriskDetector detector2 = new BriskDetector(imagePathes[1]);
BriskDetector detector3 = new BriskDetector(imagePathes[2]);
BriskDetector detector4 = new BriskDetector(imagePathes[3]);
BriskDetector detector5 = new BriskDetector(imagePathes[4]);

var descriptors1 = detector1.Descriptors;
var descriptors2 = detector2.Descriptors;
var descriptors3 = detector3.Descriptors;
var descriptors4 = detector4.Descriptors;
var descriptors5 = detector5.Descriptors;


//Vote etalones
Stopwatch stopwatch = Stopwatch.StartNew();
List<Mat> etalons = new() { detector1.Descriptors, detector2.Descriptors, detector3.Descriptors };
VoteResult voteResult1 = _hamming.VoteEtalon(etalons, detector5.Descriptors);


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



stopwatch.Stop();
results += "Voting method time elapsed: " + stopwatch.ElapsedMilliseconds + '\n';
Console.WriteLine("Voting method time elapsed: " + stopwatch.ElapsedMilliseconds);


Stopwatch stopwatch3 = Stopwatch.StartNew();
// distances
var distances11 = _hamming.FindHammingDistance(detector1.Descriptors, detector1.Descriptors);

var distances12 = _hamming.FindHammingDistance(detector1.Descriptors, detector2.Descriptors);
var distances13 = _hamming.FindHammingDistance(detector1.Descriptors, detector3.Descriptors);
//var distances21 = _hamming.FindHammingDistance(detector2.Descriptors, detector1.Descriptors);
var distances22 = _hamming.FindHammingDistance(detector2.Descriptors, detector2.Descriptors);
//var distances23 = _hamming.FindHammingDistance(detector2.Descriptors, detector3.Descriptors);
//var distances31 = _hamming.FindHammingDistance(detector3.Descriptors, detector1.Descriptors);
//var distances32 = _hamming.FindHammingDistance(detector3.Descriptors, detector2.Descriptors);
var distances33 = _hamming.FindHammingDistance(detector3.Descriptors, detector3.Descriptors);
var distances55 = _hamming.FindHammingDistance(detector5.Descriptors, detector5.Descriptors);


var mathes11 = _manhattan.CountThresholdMathes(distances11);
results += "Mathes 1 with 1: " + mathes11 + '\n';
Console.WriteLine("Mathes 1 with 1: " + 297);

var mathes12 = _manhattan.CountThresholdMathes(distances12);
results += "Mathes 1 with 2: " + mathes12 + '\n';
Console.WriteLine("Mathes 1 with 2: " + 155);

var mathes13 = _manhattan.CountThresholdMathes(distances13);
results += "Mathes 1 with 3: " + mathes13 + '\n';
Console.WriteLine("Mathes 1 with 3: " + 48);

//var mathes21 = _manhattan.CountThresholdMathes(distances21);
//results += "Mathes 2 with 1: " + mathes21 + '\n';
//Console.WriteLine("Mathes 2 with 2: " + mathes21);

//var mathes22 = _manhattan.CountThresholdMathes(distances22);
//results += "Mathes 2 with 2: " + mathes22 + '\n';
//Console.WriteLine("Mathes 2 with 2: " + mathes22);

//var mathes23 = _manhattan.CountThresholdMathes(distances23);
//results += "Mathes 2 with 3: " + mathes23 + '\n';
//Console.WriteLine("Mathes 2 with 3: " + mathes23);

//var mathes31 = _manhattan.CountThresholdMathes(distances31);
//results += "Mathes 3 with 3: " + mathes31 + '\n';
//Console.WriteLine("Mathes 3 with 3: " + mathes31);

//var mathes32 = _manhattan.CountThresholdMathes(distances32);
//results += "Mathes 3 with 3: " + mathes32 + '\n';
//Console.WriteLine("Mathes 3 with 3: " + mathes32);

//var mathes33 = _manhattan.CountThresholdMathes(distances33);
//results += "Mathes 3 with 3: " + mathes33 + '\n';
//Console.WriteLine("Mathes 3 with 3: " + mathes33);

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

VoteResult voteResult2 = _medoid.TriangleMethodMin(descriptors5,medoids,minMedoidElements);

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

VoteResult voteResult3 = _medoid.TriangleMethodMax(descriptors5, medoids, maxMedoidElements);

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


var stopwatch5 = new Stopwatch();
stopwatch5.Start();

VoteResult voteResultMedoids1 = _hamming.VoteMedoid(medoids, descriptors1);
VoteResult voteResultMedoids2 = _hamming.VoteMedoid(medoids, descriptors2);
VoteResult voteResultMedoids3 = _hamming.VoteMedoid(medoids, descriptors3);
VoteResult voteResultMedoids4 = _hamming.VoteMedoid(medoids, descriptors4);
VoteResult voteResultMedoids5 = _hamming.VoteMedoid(medoids, descriptors5);

int index4 = 1;
Console.WriteLine("///////////////////////////");
foreach (var item in voteResultMedoids1.results)
{
    Console.WriteLine($"Mathes 1 image with {index4} etalon: {item}");
    index4++;
}
index4 = 1;

foreach (var item in voteResultMedoids2.results)
{
    Console.WriteLine($"Mathes 2 image with {index4} etalon: {item}");
    index4++;
}
index4 = 1;

foreach (var item in voteResultMedoids3.results)
{
    Console.WriteLine($"Mathes 3 image with {index4} etalon: {item}");
    index4++;
}
index4 = 1;

foreach (var item in voteResultMedoids4.results)
{
    Console.WriteLine($"Mathes 4 image with {index4} etalon: {item}");
    index4++;
}
index4 = 1;

foreach (var item in voteResultMedoids5.results)
{
    Console.WriteLine($"Mathes 5 image with {index4} etalon: {item}");
    index4++;
}

stopwatch5.Stop();

results += "Medoid method time elapsed: " + stopwatch2.ElapsedMilliseconds + '\n';
Console.WriteLine("Medoid method time elapsed: " + stopwatch2.ElapsedMilliseconds);

Stopwatch stopwatch7 = new();

stopwatch7.Start();
var distances44 = _hamming.FindHammingDistance(detector4.Descriptors, detector4.Descriptors);
var medoidIndex4 = _medoid.FindMedoidIndex(distances44);
var medoidDescriptor4 = descriptors4.Row(medoidIndex4).GetRawData();
var medoidComparationResult = _hamming.FindMedoidMethodVoting(medoids, medoidDescriptor4);
stopwatch7.Stop();
Console.WriteLine("Medoid method time elapsed: " + stopwatch7.ElapsedMilliseconds);

index4 = 1;
Console.WriteLine("///////////////////////////");
foreach (var item in medoidComparationResult.results)
{
    Console.WriteLine($"Mathes 1 image with {index4} etalon: {item}");
    index4++;
}

/////////////////////
var medoidIndex5 = _medoid.FindMedoidIndex(distances55);
var medoidDescriptor5 = descriptors5.Row(medoidIndex5).GetRawData();
var medoidComparationResult2 = _hamming.FindMedoidMethodVoting(medoids, medoidDescriptor5);
index4 = 1;
Console.WriteLine("///////////////////////////");
foreach (var item in medoidComparationResult2.results)
{
    Console.WriteLine($"Mathes 1 image with {index4} etalon: {item}");
    index4++;
}
/////////////////////
File.WriteAllText(projectPath + "\\results.txt", results);