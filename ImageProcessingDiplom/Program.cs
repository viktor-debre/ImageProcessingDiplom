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

Console.WriteLine($"Count on image 1 of founded keypoints: {detector1.Keypoints.ToArray().Length}");
Console.WriteLine($"Count on image 1 of founded keypoints: {detector2.Keypoints.ToArray().Length}");
Console.WriteLine($"Count on image 1 of founded keypoints: {detector3.Keypoints.ToArray().Length}");
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
////////////////////////////////////////////////////////////////////////////
//var isSymmetric11 = _manhattan.MatrixIsSymmetric(distances11);

//string symetric = isSymmetric11 ? " " : "not ";
//results += "Matrix 1 is " + symetric + "symetric" + '\n';
//Console.WriteLine("Mathes 1 is " + symetric + "symetric");


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
results += "Mathes 2 with 1: " + mathes21 + '\n';
Console.WriteLine("Mathes 2 with 1: " + mathes21);

var mathes22 = _manhattan.CountThresholdMathes(distances22);
results += "Mathes 2 with 2: " + mathes22 + '\n';
Console.WriteLine("Mathes 2 with 2: " + mathes22);

var mathes23 = _manhattan.CountThresholdMathes(distances23);
results += "Mathes 2 with 3: " + mathes23 + '\n';
Console.WriteLine("Mathes 2 with 3: " + mathes23);

var mathes31 = _manhattan.CountThresholdMathes(distances31);
results += "Mathes 3 with 1: " + mathes31 + '\n';
Console.WriteLine("Mathes 3 with 1: " + mathes31);

var mathes32 = _manhattan.CountThresholdMathes(distances32);
results += "Mathes 3 with 2: " + mathes32 + '\n';
Console.WriteLine("Mathes 3 with 2: " + mathes32);

var mathes33 = _manhattan.CountThresholdMathes(distances33);
results += "Mathes 3 with 3: " + mathes33 + '\n';
Console.WriteLine("Mathes 3 with 3: " + mathes33);


var v1 = _manhattan.FindMinimumV(distances11, distances12, distances13);
var v2 = _manhattan.FindMinimumV(distances22 ,distances21, distances23);
var v3 = _manhattan.FindMinimumV(distances33, distances31, distances32);

Console.WriteLine();
Console.WriteLine("V1 results:");
foreach (var v in v1)
{
    if (v < -1) Console.Write(-v + " ");
    else Console.Write(v + " ");
}
Console.WriteLine();
Console.WriteLine("V2 results:");
foreach (var v in v2)
{
    if (v < -1) Console.Write(-v + " ");
    else Console.Write(v + " ");
}
Console.WriteLine();
Console.WriteLine("V3 results:");
foreach (var v in v3)
{
    if (v < -1) Console.Write(-v + " ");
    else Console.Write(v + " ");
}

Console.WriteLine();
var indexes1 = _manhattan.GetTopIndices(v1, 100);
var indexes2 = _manhattan.GetTopIndices(v2, 100);
var indexes3 = _manhattan.GetTopIndices(v3, 100);

var newDistances11 = _manhattan.TakeTopElementsFromIndexes(distances11, indexes1);
var newDistances12 = _manhattan.TakeTopElementsFrom2Indexes(distances12, indexes1,indexes2);
var newDistances13 = _manhattan.TakeTopElementsFrom2Indexes(distances13, indexes1, indexes3);

var newDistances21 = _manhattan.TakeTopElementsFrom2Indexes(distances21, indexes2, indexes1);
var newDistances22 = _manhattan.TakeTopElementsFromIndexes(distances22, indexes2);
var newDistances23 = _manhattan.TakeTopElementsFrom2Indexes(distances23, indexes2, indexes3);

var newDistances31 = _manhattan.TakeTopElementsFrom2Indexes(distances31, indexes3, indexes1);
var newDistances32 = _manhattan.TakeTopElementsFrom2Indexes(distances32, indexes3, indexes2);
var newDistances33 = _manhattan.TakeTopElementsFromIndexes(distances33, indexes3);



results += "For top 100 elements\n";
Console.WriteLine("For top 100 elements:");

var newmathes11 = _manhattan.CountThresholdMathes(newDistances11);
results += "Mathes 1 with 1: " + newmathes11 + '\n';
Console.WriteLine("Mathes 1 with 1: " + newmathes11);

var newmathes12 = _manhattan.CountThresholdMathes(newDistances12);
results += "Mathes 1 with 2: " + newmathes12 + '\n';
Console.WriteLine("Mathes 1 with 2: " + newmathes12);

var newmathes13 = _manhattan.CountThresholdMathes(newDistances13);
results += "Mathes 1 with 3: " + newmathes13 + '\n';
Console.WriteLine("Mathes 1 with 3: " + newmathes13);

var newmathes21 = _manhattan.CountThresholdMathes(newDistances21);
results += "Mathes 2 with 1: " + newmathes21 + '\n';
Console.WriteLine("Mathes 2 with 1: " + newmathes21);

var newmathes22 = _manhattan.CountThresholdMathes(newDistances22);
results += "Mathes 2 with 2: " + newmathes22 + '\n';
Console.WriteLine("Mathes 2 with 2: " + newmathes22);

var newmathes23 = _manhattan.CountThresholdMathes(newDistances23);
results += "Mathes 2 with 3: " + newmathes23 + '\n';
Console.WriteLine("Mathes 2 with 3: " + newmathes23);

var newmathes31 = _manhattan.CountThresholdMathes(newDistances31);
results += "Mathes 3 with 1: " + newmathes31 + '\n';
Console.WriteLine("Mathes 3 with 1: " + newmathes31);

var newmathes32 = _manhattan.CountThresholdMathes(newDistances32);
results += "Mathes 3 with 2: " + newmathes32 + '\n';
Console.WriteLine("Mathes 3 with 2: " + newmathes32);

var newmathes33 = _manhattan.CountThresholdMathes(newDistances33);
results += "Mathes 3 with 3: " + newmathes33 + '\n';
Console.WriteLine("Mathes 3 with 3: " + newmathes33);

File.WriteAllText(projectPath + "\\results.txt", results);