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
   projectPath + "\\Images\\karl1",
   projectPath + "\\Images\\karl2",
   projectPath + "\\Images\\karl3"//,
   //projectPath + "\\Images\\image4"
};

BriskDetector detector1 = new BriskDetector(imagePathes[0]);
BriskDetector detector2 = new BriskDetector(imagePathes[1]);
BriskDetector detector3 = new BriskDetector(imagePathes[2]);
//BriskDetector detector4 = new BriskDetector(imagePathes[3]);

var descriptors1 = detector1.Descriptors;
var descriptors2 = detector2.Descriptors;
var descriptors3 = detector3.Descriptors;
//var descriptors4 = detector4.Descriptors;


////Vote etalones
//Stopwatch stopwatch = Stopwatch.StartNew();
//List<Mat> etalons = new List<Mat>() { detector1.Descriptors, detector2.Descriptors, detector3.Descriptors };
//VoteResult voteResult1 = _hamming.VoteEtalon(etalons, detector4.Descriptors);


//int index = 1;
//foreach (var item in voteResult1.results)
//{
//    Console.WriteLine($"Mathes 4 image with {index} etalon: {item}");
//    index++;
//}


//stopwatch.Stop();
//results += "Voting method time elapsed: " + stopwatch.ElapsedMilliseconds + '\n';
//Console.WriteLine("Voting method time elapsed: " + stopwatch.ElapsedMilliseconds);


Stopwatch stopwatch3 = Stopwatch.StartNew();
// distances
var distances11 = _hamming.FindHammingDistance(detector1.Descriptors, detector1.Descriptors);
var distances12 = _hamming.FindHammingDistance(detector1.Descriptors, detector2.Descriptors);
var distances13 = _hamming.FindHammingDistance(detector1.Descriptors, detector3.Descriptors);
var distances21 = _hamming.FindHammingDistance(detector2.Descriptors, detector1.Descriptors);
var distances22 = _hamming.FindHammingDistance(detector2.Descriptors, detector2.Descriptors);
var distances23 = _hamming.FindHammingDistance(detector2.Descriptors, detector3.Descriptors);
var distances31 = _hamming.FindHammingDistance(detector3.Descriptors, detector1.Descriptors);
var distances32 = _hamming.FindHammingDistance(detector3.Descriptors, detector2.Descriptors);
var distances33 = _hamming.FindHammingDistance(detector3.Descriptors, detector3.Descriptors);
stopwatch3.Stop();
Console.WriteLine("Matrix etalon time elapsed: " + stopwatch3.ElapsedMilliseconds);

//VoteResult voteResult2 = _hamming.VoteEtalon(etalons, detector1.Descriptors);
//index = 1;

//foreach (var item in voteResult2.results)
//{
//    Console.WriteLine($"Mathes 1 image with {index} etalon: {item}");
//    index++;
//}

var mathes11 = _manhattan.CountThresholdMathes(distances11);
results += "Mathes 1 with 1: " + mathes11 + '\n';
Console.WriteLine("Mathes 1 with 1: " + mathes11);

var mathes12 = _manhattan.CountThresholdMathes(distances12);
results += "Mathes 1 with 2: " + mathes12 + '\n';
Console.WriteLine("Mathes 1 with 2: " + mathes12);

var mathes13 = _manhattan.CountThresholdMathes(distances13);
results += "Mathes 1 with 3: " + mathes13 + '\n';
Console.WriteLine("Mathes 1 with 3: " + mathes13);

var mathes21 = _manhattan.CountThresholdMathes(distances21);
results += "Mathes 1 with 3: " + mathes21 + '\n';
Console.WriteLine("Mathes 1 with 3: " + mathes21);

var mathes22 = _manhattan.CountThresholdMathes(distances22);
results += "Mathes 2 with 2: " + mathes22 + '\n';
Console.WriteLine("Mathes 2 with 2: " + mathes22);

var mathes23 = _manhattan.CountThresholdMathes(distances23);
results += "Mathes 2 with 3: " + mathes23 + '\n';
Console.WriteLine("Mathes 2 with 3: " + mathes23);

var mathes31 = _manhattan.CountThresholdMathes(distances31);
results += "Mathes 3 with 3: " + mathes31 + '\n';
Console.WriteLine("Mathes 3 with 3: " + mathes31);

var mathes32 = _manhattan.CountThresholdMathes(distances32);
results += "Mathes 3 with 3: " + mathes32 + '\n';
Console.WriteLine("Mathes 3 with 3: " + mathes32);

var mathes33 = _manhattan.CountThresholdMathes(distances33);
results += "Mathes 3 with 3: " + mathes33 + '\n';
Console.WriteLine("Mathes 3 with 3: " + mathes33);


File.WriteAllText(projectPath + "\\results.txt", results);