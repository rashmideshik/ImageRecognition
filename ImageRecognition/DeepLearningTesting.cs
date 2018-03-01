using Accord.Neuro.Networks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageRecognition
{
    public class DeepLearningTesting
    {
        private static readonly string ApplicationPath = System.IO.Directory.GetCurrentDirectory();
        private static readonly string TrainingFileName = "BitArrayTesting.txt";

        public int DoTesting(DeepBeliefNetwork network, string[] images)
        {
           return DoTesting(network, images, null);
        }

        public int DoTesting(DeepBeliefNetwork network, string[] images, IOutputWriter writer)
        {

            int totalCount = images.Length;

            double[][] testInputs;
            double[][] testOutputs;
            string binaryFilePath = Path.Combine(ApplicationPath, TrainingFileName);

            //binary file generation
            ImageUtil util = new ImageUtil(ImageDimentionConstant.WIDTH, ImageDimentionConstant.HEIGHT);
            util.GenerateBinaryFile(binaryFilePath, images);
            
            // Load  dataset.
            testInputs = DataManager.Load(binaryFilePath, out testOutputs);

            int correct = 0;
            for (int i = 0; i < totalCount; i++)
            {
                double[] outputValues = network.Compute(testInputs[i]);
                if (DataManager.FormatOutputResult(outputValues) == DataManager.FormatOutputResult(testOutputs[i]))
                {
                    correct++;
                }
            }

            writer?.WriteOutput("Correct " + correct + "/" + totalCount + ", " + Math.Round(((double)correct / (double)totalCount * 100), 2) + "%");

            return correct;
        }    
    }
}
