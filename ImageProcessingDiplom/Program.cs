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

Stopwatch stopwatch3 = Stopwatch.StartNew();
// distances
var listDescriptors = new List<Mat>()
{
    descriptors1,
    descriptors2,
    descriptors3
};

var uniqueNumbers = _hamming.FindUniqueMathesForAllEtalon(listDescriptors);
stopwatch3.Stop();

Console.WriteLine("Matrix etalon time elapsed: " + stopwatch3.ElapsedMilliseconds);
/////////////////////////////////
for (int k = 0; k < uniqueNumbers.Count; k++)
{
    for (int i = 0; i < listDescriptors.Count; i++)
    {
        results += $"Repeding value for etalon {k + 1} with etalon {i + 1}\n";
        for (int j = 0; j < 500; j++)
        {
            results += uniqueNumbers[k][i, j] + " ";
        }
        results += '\n';
    }
}


Console.Write(results);
Console.WriteLine();
File.WriteAllText(projectPath + "\\results.txt", results);