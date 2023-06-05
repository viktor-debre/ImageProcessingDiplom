using ImageProcessingDiplom.OpenCvServices;
using System.Diagnostics;

var _voting = new Voting();
var _minDistanceFinder = new MinDistanceFinder();

string results = "";
var projectPath = Directory.GetCurrentDirectory();
var imagePathes = new List<string>
{
   projectPath + "\\Images\\karl1",
   projectPath + "\\Images\\karl2",
   projectPath + "\\Images\\karl3"
};

Brisk detector1 = new Brisk(imagePathes[0]);
Brisk detector2 = new Brisk(imagePathes[1]);
Brisk detector3 = new Brisk(imagePathes[2]);

Console.WriteLine($"Count on image 1 of founded keypoints: {detector1.Keypoints.ToArray().Length}");
Console.WriteLine($"Count on image 1 of founded keypoints: {detector2.Keypoints.ToArray().Length}");
Console.WriteLine($"Count on image 1 of founded keypoints: {detector3.Keypoints.ToArray().Length}");


Stopwatch statisticsTimer1 = Stopwatch.StartNew();
var distances11 = _voting.FindHammingDistance(detector1.Descriptors, detector1.Descriptors);
var distances12 = _voting.FindHammingDistance(detector1.Descriptors, detector2.Descriptors);
var distances13 = _voting.FindHammingDistance(detector1.Descriptors, detector3.Descriptors);
var distances21 = _voting.FindHammingDistance(detector2.Descriptors, detector1.Descriptors);
var distances22 = _voting.FindHammingDistance(detector2.Descriptors, detector2.Descriptors);
var distances23 = _voting.FindHammingDistance(detector2.Descriptors, detector3.Descriptors);
var distances31 = _voting.FindHammingDistance(detector3.Descriptors, detector1.Descriptors);
var distances32 = _voting.FindHammingDistance(detector3.Descriptors, detector2.Descriptors);
var distances33 = _voting.FindHammingDistance(detector3.Descriptors, detector3.Descriptors);
statisticsTimer1.Stop();
Console.WriteLine("Matrix etalon time elapsed: " + statisticsTimer1.ElapsedMilliseconds);

var mathes11 = _minDistanceFinder.CountThresholdMathes(distances11);
results += "Mathes 1 with 1: " + mathes11 + '\n';
Console.WriteLine("Mathes 1 with 1: " + mathes11);

var mathes12 = _minDistanceFinder.CountThresholdMathes(distances12);
results += "Mathes 1 with 2: " + mathes12 + '\n';
Console.WriteLine("Mathes 1 with 2: " + mathes12);

var mathes13 = _minDistanceFinder.CountThresholdMathes(distances13);
results += "Mathes 1 with 3: " + mathes13 + '\n';
Console.WriteLine("Mathes 1 with 3: " + mathes13);

var mathes21 = _minDistanceFinder.CountThresholdMathes(distances21);
results += "Mathes 2 with 1: " + mathes21 + '\n';
Console.WriteLine("Mathes 2 with 1: " + mathes21);

var mathes22 = _minDistanceFinder.CountThresholdMathes(distances22);
results += "Mathes 2 with 2: " + mathes22 + '\n';
Console.WriteLine("Mathes 2 with 2: " + mathes22);

var mathes23 = _minDistanceFinder.CountThresholdMathes(distances23);
results += "Mathes 2 with 3: " + mathes23 + '\n';
Console.WriteLine("Mathes 2 with 3: " + mathes23);

var mathes31 = _minDistanceFinder.CountThresholdMathes(distances31);
results += "Mathes 3 with 1: " + mathes31 + '\n';
Console.WriteLine("Mathes 3 with 1: " + mathes31);

var mathes32 = _minDistanceFinder.CountThresholdMathes(distances32);
results += "Mathes 3 with 2: " + mathes32 + '\n';
Console.WriteLine("Mathes 3 with 2: " + mathes32);

var mathes33 = _minDistanceFinder.CountThresholdMathes(distances33);
results += "Mathes 3 with 3: " + mathes33 + '\n';
Console.WriteLine("Mathes 3 with 3: " + mathes33);

Stopwatch statisticsTimer2 = Stopwatch.StartNew();

var v1 = _minDistanceFinder.FindMinimumV(distances11, distances12, distances13);
var v2 = _minDistanceFinder.FindMinimumV(distances22 ,distances21, distances23);
var v3 = _minDistanceFinder.FindMinimumV(distances33, distances31, distances32);

Console.WriteLine();
Console.WriteLine("V1 results:");
foreach (var v in v1)
{
    Console.Write(v + " ");
}
Console.WriteLine();
Console.WriteLine("V2 results:");
foreach (var v in v2)
{
    Console.Write(v + " ");
}
Console.WriteLine();
Console.WriteLine("V3 results:");
foreach (var v in v3)
{
    Console.Write(v + " ");
}

Console.WriteLine();
var indexes1 = _minDistanceFinder.GetTopIndices(v1, 100);
var indexes2 = _minDistanceFinder.GetTopIndices(v2, 100);
var indexes3 = _minDistanceFinder.GetTopIndices(v3, 100);

var newDistances11 = _minDistanceFinder.TakeTopElementsFromIndexes(distances11, indexes1);
var newDistances12 = _minDistanceFinder.TakeTopElementsFrom2Indexes(distances12, indexes1,indexes2);
var newDistances13 = _minDistanceFinder.TakeTopElementsFrom2Indexes(distances13, indexes1, indexes3);

var newDistances21 = _minDistanceFinder.TakeTopElementsFrom2Indexes(distances21, indexes2, indexes1);
var newDistances22 = _minDistanceFinder.TakeTopElementsFromIndexes(distances22, indexes2);
var newDistances23 = _minDistanceFinder.TakeTopElementsFrom2Indexes(distances23, indexes2, indexes3);

var newDistances31 = _minDistanceFinder.TakeTopElementsFrom2Indexes(distances31, indexes3, indexes1);
var newDistances32 = _minDistanceFinder.TakeTopElementsFrom2Indexes(distances32, indexes3, indexes2);
var newDistances33 = _minDistanceFinder.TakeTopElementsFromIndexes(distances33, indexes3);


results += "For top 100 elements\n";
Console.WriteLine("For top 100 elements:");

var newmathes11 = _minDistanceFinder.CountThresholdMathes(newDistances11);
results += "Mathes 1 with 1: " + newmathes11 + '\n';
Console.WriteLine("Mathes 1 with 1: " + newmathes11);

var newmathes12 = _minDistanceFinder.CountThresholdMathes(newDistances12);
results += "Mathes 1 with 2: " + newmathes12 + '\n';
Console.WriteLine("Mathes 1 with 2: " + newmathes12);

var newmathes13 = _minDistanceFinder.CountThresholdMathes(newDistances13);
results += "Mathes 1 with 3: " + newmathes13 + '\n';
Console.WriteLine("Mathes 1 with 3: " + newmathes13);

var newmathes21 = _minDistanceFinder.CountThresholdMathes(newDistances21);
results += "Mathes 2 with 1: " + newmathes21 + '\n';
Console.WriteLine("Mathes 2 with 1: " + newmathes21);

var newmathes22 = _minDistanceFinder.CountThresholdMathes(newDistances22);
results += "Mathes 2 with 2: " + newmathes22 + '\n';
Console.WriteLine("Mathes 2 with 2: " + newmathes22);

var newmathes23 = _minDistanceFinder.CountThresholdMathes(newDistances23);
results += "Mathes 2 with 3: " + newmathes23 + '\n';
Console.WriteLine("Mathes 2 with 3: " + newmathes23);

var newmathes31 = _minDistanceFinder.CountThresholdMathes(newDistances31);
results += "Mathes 3 with 1: " + newmathes31 + '\n';
Console.WriteLine("Mathes 3 with 1: " + newmathes31);

var newmathes32 = _minDistanceFinder.CountThresholdMathes(newDistances32);
results += "Mathes 3 with 2: " + newmathes32 + '\n';
Console.WriteLine("Mathes 3 with 2: " + newmathes32);

var newmathes33 = _minDistanceFinder.CountThresholdMathes(newDistances33);
results += "Mathes 3 with 3: " + newmathes33 + '\n';
Console.WriteLine("Mathes 3 with 3: " + newmathes33);
statisticsTimer2.Stop();
Console.WriteLine("Matrix etalon time elapsed: " + (int)(statisticsTimer1.ElapsedMilliseconds / 3.5));

File.WriteAllText(projectPath + "\\statistics.txt", results);