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
var _manhattan = serviceProvider.GetService<VotingProvider>();
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

List<Mat> etalons = new() { detector1.Descriptors, detector2.Descriptors, detector3.Descriptors };


//Vote etalones classic method
var resultImage4 = FindImageClassicMethodVotes(etalons, detector4.Descriptors, 4, _hamming);
results += resultImage4;

var resultImage5 = FindImageClassicMethodVotes(etalons, detector5.Descriptors, 5, _hamming);
results += resultImage5;

// distances
var distances11 = _hamming.FindHammingDistance(descriptors1, descriptors1);
var distances12 = _hamming.FindHammingDistance(descriptors1, descriptors2);
var distances13 = _hamming.FindHammingDistance(descriptors1, descriptors3);
//var distances21 = _hamming.FindHammingDistance(descriptors2, descriptors1);
var distances22 = _hamming.FindHammingDistance(descriptors2, descriptors2);
//var distances23 = _hamming.FindHammingDistance(descriptors2, descriptors3);
//var distances31 = _hamming.FindHammingDistance(descriptors3, descriptors3);
//var distances32 = _hamming.FindHammingDistance(descriptors3, descriptors3);
var distances33 = _hamming.FindHammingDistance(descriptors3, descriptors3);
var distance44 = _hamming.FindHammingDistance(descriptors4, descriptors4);
var distances55 = _hamming.FindHammingDistance(descriptors5, descriptors5);


//var stopwatch3 = Stopwatch.StartNew();
//var mathes11 = _manhattan.CountThresholdMathes(distances11);
//results += "Mathes 1 with 1: " + 297 + '\n';
//Console.WriteLine("Mathes 1 with 1: " + 297);

//var mathes12 = _manhattan.CountThresholdMathes(distances12);
//results += "Mathes 1 with 2: " + 155 + '\n';
//Console.WriteLine("Mathes 1 with 2: " + 155);

//var mathes13 = _manhattan.CountThresholdMathes(distances13);
//results += "Mathes 1 with 3: " + 48 + '\n';
//Console.WriteLine("Mathes 1 with 3: " + 48);

//stopwatch3.Stop();
//Console.WriteLine("Matrix etalon time elapsed: " + stopwatch3.ElapsedMilliseconds);
/////////////////////////////////
//Medoid search
Stopwatch stopwatch2 = Stopwatch.StartNew();


var medoidIndex1 = _medoid.FindMedoidIndex(distances11);
var medoidDescriptor1 = descriptors1.Row(medoidIndex1).GetRawData();

var medoidIndex2 = _medoid.FindMedoidIndex(distances22);
var medoidDescriptor2 = descriptors2.Row(medoidIndex2).GetRawData();

var medoidIndex3 = _medoid.FindMedoidIndex(distances33);
var medoidDescriptor3 = descriptors3.Row(medoidIndex3).GetRawData();

var resultMedoid1 = PrintMedoidInfo(1, medoidIndex1);
results += resultMedoid1;
var resultMedoid2 = PrintMedoidInfo(2, medoidIndex2);
results += resultMedoid2;
var resultMedoid3 = PrintMedoidInfo(3, medoidIndex3);
results += resultMedoid3;

List<byte[]> medoids = new List<byte[]>() { medoidDescriptor1, medoidDescriptor2, medoidDescriptor3 };

Console.WriteLine("min method");
int minMedoid1 = _medoid.FindMinDistanceForMedoid(distances11, medoidIndex1);
int minMedoid2 = _medoid.FindMinDistanceForMedoid(distances22, medoidIndex2);
int minMedoid3 = _medoid.FindMinDistanceForMedoid(distances33, medoidIndex3);

List<int> minMedoidElements = new List<int>() { minMedoid1, minMedoid2, minMedoid3 };

VoteResult voteResult4 = _medoid.TriangleMethodMin(descriptors4, medoids, minMedoidElements);
string minElement = "min element";
var resultsTriengleVote4 = PrintVoteResults(4, voteResult4, minElement);
results += resultsTriengleVote4;

VoteResult voteResult2 = _medoid.TriangleMethodMin(descriptors5, medoids, minMedoidElements);

var resultsTriengleVote = PrintVoteResults(5, voteResult2, minElement);
results += resultsTriengleVote;
////////////////////////////////////////
Console.WriteLine("max method");
int maxMedoid1 = _medoid.FindMaxDistanceForMedoid(distances11, medoidIndex1);
int maxMedoid2 = _medoid.FindMaxDistanceForMedoid(distances22, medoidIndex2);
int maxMedoid3 = _medoid.FindMaxDistanceForMedoid(distances33, medoidIndex3);

List<int> maxMedoidElements = new List<int>() { maxMedoid1, maxMedoid2, maxMedoid3 };

string maxElement = "max element";
VoteResult voteResult5 = _medoid.TriangleMethodMax(descriptors4, medoids, maxMedoidElements);

var resultsTriengleVote5 = PrintVoteResults(4, voteResult5, maxElement);
results += resultsTriengleVote5;

VoteResult voteResult3 = _medoid.TriangleMethodMax(descriptors5, medoids, maxMedoidElements);

var resultsTriengleVote2 = PrintVoteResults(5, voteResult3, maxElement);
results += resultsTriengleVote2;

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


//var stopwatch5 = new Stopwatch();
//stopwatch5.Start();

//VoteResult voteResultMedoids1 = _hamming.VoteMedoid(medoids, descriptors1);
//VoteResult voteResultMedoids2 = _hamming.VoteMedoid(medoids, descriptors2);
//VoteResult voteResultMedoids3 = _hamming.VoteMedoid(medoids, descriptors3);
//VoteResult voteResultMedoids4 = _hamming.VoteMedoid(medoids, descriptors4);
//VoteResult voteResultMedoids5 = _hamming.VoteMedoid(medoids, descriptors5);

//int index4 = 1;

//Console.WriteLine("///////////////////////////");
//var resultsTriengleVote1 = PrintVoteResults(1, voteResultMedoids1);
//results += resultsTriengleVote1;

//foreach (var item in voteResultMedoids1.results)
//{
//    Console.WriteLine($"Mathes 1 image with {index4} etalon: {item}");
//    index4++;
//}
//index4 = 1;

//foreach (var item in voteResultMedoids2.results)
//{
//    Console.WriteLine($"Mathes 2 image with {index4} etalon: {item}");
//    index4++;
//}
//index4 = 1;

//foreach (var item in voteResultMedoids3.results)
//{
//    Console.WriteLine($"Mathes 3 image with {index4} etalon: {item}");
//    index4++;
//}
//index4 = 1;

//foreach (var item in voteResultMedoids4.results)
//{
//    Console.WriteLine($"Mathes 4 image with {index4} etalon: {item}");
//    index4++;
//}
//index4 = 1;

//foreach (var item in voteResultMedoids5.results)
//{
//    Console.WriteLine($"Mathes 5 image with {index4} etalon: {item}");
//    index4++;
//}

//stopwatch5.Stop();

//results += "Medoid method time elapsed: " + stopwatch2.ElapsedMilliseconds + '\n';
//Console.WriteLine("Medoid method time elapsed: " + stopwatch2.ElapsedMilliseconds);

//Stopwatch stopwatch7 = new();

//stopwatch7.Start();
//var distances44 = _hamming.FindHammingDistance(detector4.Descriptors, detector4.Descriptors);
//var medoidIndex4 = _medoid.FindMedoidIndex(distances44);
//var medoidDescriptor4 = descriptors4.Row(medoidIndex4).GetRawData();
//var medoidComparationResult = _hamming.FindMedoidMethodVoting(medoids, medoidDescriptor4);
//stopwatch7.Stop();
//Console.WriteLine("Medoid method time elapsed: " + stopwatch7.ElapsedMilliseconds);

//index4 = 1;
//Console.WriteLine("///////////////////////////");
//foreach (var item in medoidComparationResult.results)
//{
//    Console.WriteLine($"Mathes 1 image with {index4} etalon: {item}");
//    index4++;
//}

///////////////////////
//var medoidIndex5 = _medoid.FindMedoidIndex(distances55);
//var medoidDescriptor5 = descriptors5.Row(medoidIndex5).GetRawData();
//var medoidComparationResult2 = _hamming.FindMedoidMethodVoting(medoids, medoidDescriptor5);
//index4 = 1;
//Console.WriteLine("///////////////////////////");
//foreach (var item in medoidComparationResult2.results)
//{
//    Console.WriteLine($"Mathes 1 image with {index4} etalon: {item}");
//    index4++;
//}
/////////////////////
File.WriteAllText(projectPath + "\\results.txt", results);

static string FindImageClassicMethodVotes(
    List<Mat> etalons,
    Mat descriptors,
    int imageNumber,
    HammingProvider _hamming)
{
    string results = "";
    Stopwatch stopwatch = Stopwatch.StartNew();

    VoteResult voteResult1 = _hamming.VoteEtalon(etalons, descriptors);

    results += $"Voting method with image {imageNumber}" + '\n';
    int index = 1;
    foreach (var item in voteResult1.results)
    {
        Console.WriteLine($"Mathes {imageNumber} image with {index} etalon: {item}");
        results += $"Mathes {imageNumber} image with {index} etalon: {item}" + '\n';
        index++;
    }
    stopwatch.Stop();
    results += $"Voting method time elapsed for image {imageNumber}: " + stopwatch.ElapsedMilliseconds + '\n';
    Console.WriteLine($"Voting method time elapsed for image {imageNumber}: " + stopwatch.ElapsedMilliseconds);

    return results;
}

static string PrintMedoidInfo(int medoidNumber, int medoidIndex)
{
    string results = "";
    results += $"Medoid {medoidNumber} is descriptor number " + (medoidIndex + 1) + '\n';
    Console.WriteLine("Medoid is descriptor number " + (medoidIndex + 1));

    return results;
}

static string PrintVoteResults(int imageNumber, VoteResult voteResult, string message)
{
    string results = $"Triangle {message} method vote results for image {imageNumber}:" + '\n';
    int index = 1;
    foreach (var item in voteResult.results)
    {
        Console.WriteLine($"Mathes {imageNumber} image with {index} etalon: {item}");
        results += $"Mathes {imageNumber} image with {index} etalon: {item}" + '\n';
        index++;
    }

    return results;
}