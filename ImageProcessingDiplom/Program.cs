using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
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

//Program
string results = "";
var projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
var imagePathes = new List<string>
{
   projectPath + "\\Images\\nft1",
   projectPath + "\\Images\\nft2",
   projectPath + "\\Images\\nft3"//,
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

Console.WriteLine($"Count on image 1 of founded keypoints: {detector1.Keypoints.ToArray().Length}");
Console.WriteLine($"Count on image 1 of founded keypoints: {detector2.Keypoints.ToArray().Length}");
Console.WriteLine($"Count on image 1 of founded keypoints: {detector3.Keypoints.ToArray().Length}");

Stopwatch stopwatch2 = Stopwatch.StartNew();
// distances
var listDescriptors = new List<Mat>()
{
    descriptors1,
    descriptors2,
    descriptors3
};

var uniqueNumbers = _hamming.FindUniqueMathesForAllEtalon(listDescriptors);
stopwatch2.Stop();

Console.WriteLine("Matrix etalon time elapsed: " + stopwatch2.ElapsedMilliseconds);
////////////////////
//for (int k = 0; k < uniqueNumbers.Count; k++)
//{
//    for (int i = 0; i < listDescriptors.Count; i++)
//    {
//        results += $"Repeding value for etalon {k + 1} with etalon {i + 1}\n";
//        for (int j = 0; j < 500; j++)
//        {
//            results += uniqueNumbers[k][i, j] + " ";
//        }
//        results += '\n';
//    }
//}

List<int[,]> b = new List<int[,]>();

int arrayRows = 3;
int arrayColumns = 500;

int numberOf2DArrays = 3;

for (int k = 0; k < numberOf2DArrays; k++)
{
    int[,] newArray = new int[arrayRows, arrayColumns];

    for (int i = 0; i < arrayRows; i++)
    {
        for (int j = 0; j < arrayColumns; j++)
        {
            newArray[i, j] = 0;
        }
    }

    b.Add(newArray);
}

for (int k = 0; k < uniqueNumbers.Count; k++)
{
    for (int i = 0; i < listDescriptors.Count; i++)
    {
        for (int j = 0; j < 500; j++)
        {
            for (int m = 0; m < listDescriptors.Count; m++)
            {
                if (m != i)
                {
                    b[k][i, j] += uniqueNumbers[k][m, j];
                }
            }
        }
    }
}


//for (int k = 0; k < uniqueNumbers.Count; k++)
//{
//    for (int i = 0; i < listDescriptors.Count; i++)
//    {
//        results += $"Repeding value for etalon {k + 1} with etalon {i + 1}\n";
//        for (int j = 0; j < 500; j++)
//        {
//            results += b[k][i, j] / 1000.0 + " ";
//        }
//        results += '\n';
//    }
//}

List<int[,]> s = new List<int[,]>();


for (int k = 0; k < numberOf2DArrays; k++)
{
    int[,] newArray = new int[arrayRows, arrayColumns];

    for (int i = 0; i < arrayRows; i++)
    {
        for (int j = 0; j < arrayColumns; j++)
        {
            newArray[i, j] = 0;
        }
    }

    s.Add(newArray);
}

for (int k = 0; k < uniqueNumbers.Count; k++)
{
    for (int i = 0; i < listDescriptors.Count; i++)
    {
        for (int j = 0; j < 500; j++)
        {
            s[k][i, j] = (uniqueNumbers[k][i, j] + b[k][i, j]) / 1500;
        }
    }
}

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


int[] indexes1 = _hamming.GetIndexesOfTop100LeastElements(_hamming.TakeOnlyRow(0, uniqueNumbers[0]));
int[] indexes2 = _hamming.GetIndexesOfTop100LeastElements(_hamming.TakeOnlyRow(1, uniqueNumbers[1]));
int[] indexes3 = _hamming.GetIndexesOfTop100LeastElements(_hamming.TakeOnlyRow(2, uniqueNumbers[2]));


Stopwatch stopwatch4 = Stopwatch.StartNew();
// distances
var distances11short = _hamming.FindHammingDistanceWithIndexes(detector1.Descriptors, detector1.Descriptors, indexes1, indexes1);
var distances12short = _hamming.FindHammingDistanceWithIndexes(detector1.Descriptors, detector2.Descriptors, indexes1, indexes2);
var distances13short = _hamming.FindHammingDistanceWithIndexes(detector1.Descriptors, detector3.Descriptors, indexes1, indexes3);
var distances21short = _hamming.FindHammingDistanceWithIndexes(detector2.Descriptors, detector1.Descriptors, indexes2, indexes1);
var distances22short = _hamming.FindHammingDistanceWithIndexes(detector2.Descriptors, detector2.Descriptors, indexes2, indexes2);
var distances23short = _hamming.FindHammingDistanceWithIndexes(detector2.Descriptors, detector3.Descriptors, indexes2, indexes3);
var distances31short = _hamming.FindHammingDistanceWithIndexes(detector3.Descriptors, detector1.Descriptors, indexes3, indexes1);
var distances32short = _hamming.FindHammingDistanceWithIndexes(detector3.Descriptors, detector2.Descriptors, indexes3, indexes2);
var distances33short = _hamming.FindHammingDistanceWithIndexes(detector3.Descriptors, detector3.Descriptors, indexes3, indexes3);
stopwatch4.Stop();
Console.WriteLine("Matrix etalon time elapsed: " + stopwatch4.ElapsedMilliseconds);

Console.WriteLine("/////////////////");

var mathes11short = _manhattan.CountThresholdMathes(distances11short);
results += "Mathes 1 with 1: " + mathes11short + '\n';
Console.WriteLine("Mathes 1 with 1: " + mathes11short);

var mathes12short = _manhattan.CountThresholdMathes(distances12short);
results += "Mathes 1 with 2: " + mathes12short + '\n';
Console.WriteLine("Mathes 1 with 2: " + mathes12short);

var mathes13short = _manhattan.CountThresholdMathes(distances13short);
results += "Mathes 1 with 3: " + mathes13short + '\n';
Console.WriteLine("Mathes 1 with 3: " + mathes13short);

var mathes21short = _manhattan.CountThresholdMathes(distances21short);
results += "Mathes 2 with 1: " + mathes21short + '\n';
Console.WriteLine("Mathes 2 with 1: " + mathes21short);

var mathes22short = _manhattan.CountThresholdMathes(distances22short);
results += "Mathes 2 with 2: " + mathes22short + '\n';
Console.WriteLine("Mathes 2 with 2: " + mathes22short);

var mathes23short = _manhattan.CountThresholdMathes(distances23short);
results += "Mathes 2 with 3: " + mathes23short + '\n';
Console.WriteLine("Mathes 2 with 3: " + mathes23short);

var mathes31short = _manhattan.CountThresholdMathes(distances31short);
results += "Mathes 3 with 1: " + mathes31short + '\n';
Console.WriteLine("Mathes 3 with 1: " + mathes31short);

var mathes32short = _manhattan.CountThresholdMathes(distances32short);
results += "Mathes 3 with 2: " + mathes32short + '\n';
Console.WriteLine("Mathes 3 with 2: " + mathes32short);

var mathes33short = _manhattan.CountThresholdMathes(distances33short);
results += "Mathes 3 with 3: " + mathes33short + '\n';
Console.WriteLine("Mathes 3 with 3: " + mathes33short);



//Console.Write(results);
Console.WriteLine();
File.WriteAllText(projectPath + "\\results.txt", results);