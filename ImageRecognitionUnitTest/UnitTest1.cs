using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageRecognition;
using System.IO;

namespace ImageRecognitionUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        private string StartupPath;
      

        [TestInitialize]
        public void Initialize()
        {           
            StartupPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));          
        }

        //[TestMethod]
        //public void BitArrayFileTesting()
        //{
        //   string TempFolder = Path.GetTempPath();
        //    string TrainingBinaryFilePath = Path.Combine(TempFolder, "testBinaryFile.txt");
        //    string trainingData = Path.Combine(StartupPath, "testRes\\training");

        //    string[] images = Directory.GetFiles(trainingData, "*");
        //    ImageUtil img = new ImageUtil();
        //    img.GenerateBinaryFile(TrainingBinaryFilePath, images);

        //    Assert.IsTrue(File.Exists(TrainingBinaryFilePath));

        //    File.Delete(TrainingBinaryFilePath);
        //}

        [TestMethod]
        public void PositivescenarioTesting()
        {

            string trainingData = Path.Combine(StartupPath, "testRes\\training");

            string[] images = Directory.GetFiles(trainingData, "*");                      
            Assert.AreEqual(images.Length, 40);
            DeepLearningTraining training = new DeepLearningTraining();
            training.DoTraining(images, 20, null);
          
            string testingData = Path.Combine(StartupPath, "testRes\\positiveTestData");
            images = Directory.GetFiles(testingData, "*");
            Assert.IsNotNull(training.GetNetwork());
            DeepLearningTesting testing = new DeepLearningTesting();
            int totalMatchedCount = testing.DoTesting(training.GetNetwork(), images);

            Assert.AreEqual(40, totalMatchedCount);

        }

        [TestMethod]
        public void NegativeScenarioTesting()
        {
            string trainingData = Path.Combine(StartupPath, "testRes\\training");
            string[] images = Directory.GetFiles(trainingData, "*");
           Assert.AreEqual(images.Length, 40);

            DeepLearningTraining training = new DeepLearningTraining();
            training.DoTraining(images, 20, null);

            string testingData = Path.Combine(StartupPath, "testRes\\negativeTestData");
            images = Directory.GetFiles(testingData, "*");
          
            Assert.IsNotNull(training.GetNetwork());

            DeepLearningTesting testing = new DeepLearningTesting();
            int totalMatchedCount = testing.DoTesting(training.GetNetwork(), images);

            Assert.AreEqual(22, totalMatchedCount);
        }
       

    }

}
