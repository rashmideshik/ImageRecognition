using Accord.Neuro;
using Accord.Neuro.ActivationFunctions;
using Accord.Neuro.Learning;
using Accord.Neuro.Networks;
using Accord.Math;
using System;
using System.Linq;
using System.IO;

namespace ImageRecognition
{
    public class DeepLearningTraining
    {
        private DeepBeliefNetwork network = null;

        private static readonly string ApplicationPath = System.IO.Directory.GetCurrentDirectory();
        private static readonly string TrainingFileName = "BitArrayTraining.txt";

        public DeepBeliefNetwork GetNetwork()
        {
            return network;
        }


        public void SaveNetwork(String path)
        {
            network.Save(path);
        }

        public void LoadNetwork(String path)
        {
            network = DeepBeliefNetwork.Load(path);
        }

        public void DoTraining(string[] images)
        {
            DoTraining(images, -1, null);
        }

        public void DoTraining(string[] images, int imagesToProceedSimultaneously, IOutputWriter writer)
        {
            string binaryFilePath = Path.Combine(ApplicationPath, TrainingFileName);
            int imagesToCopy = imagesToProceedSimultaneously;
            // for loop for user defined imagesToProceedSimultaneously
            for (int i = 0; i < images.Length; i += imagesToCopy)
            {
                if (i + imagesToCopy > images.Length)
                    imagesToCopy = images.Length - i;

                string[] pick = new string[imagesToCopy];

                Array.Copy(images, i, pick, 0, imagesToCopy);

                writer?.WriteOutput("Image: " + i + "-" + (i + imagesToCopy));


                //binary file generation
                ImageUtil util = new ImageUtil(ImageDimentionConstant.WIDTH, ImageDimentionConstant.HEIGHT);                
                //util.X = 64;
                //util.Y = 64;
                util.GenerateBinaryFile(binaryFilePath, pick);
                
                //do training
                DoTraining(binaryFilePath, writer);
                
            }            

        }

        private void DoTraining(string binaryFilePath, IOutputWriter writer)
        {
            double[][] inputs;
            double[][] outputs;
            // Load  dataset.
            inputs = DataManager.Load(binaryFilePath, out outputs);

            // Setup the deep belief n  etwork and initialize with random weights.
            if (network == null)
                network = DeepLearningNetworkBuilder.CreateAndGetNetwork(inputs.First().Length);


            // Setup the learning algorithm.
            DeepBeliefNetworkLearning teacher = new DeepBeliefNetworkLearning(network)
            {
                Algorithm = (h, v, i) => new ContrastiveDivergenceLearning(h, v)
                {
                    LearningRate = 0.1,
                    Momentum = 0.5,
                    Decay = 0.001,
                }
            };


            // Setup batches of input for learning.
            // int batchnumber = (int)totalCount / 10;
            int batchCount = Math.Max(1, inputs.Length / 5);

            // Create mini-batches to speed learning.
            int[] groups = Accord.Statistics.Classes.Random(inputs.Length, batchCount);
            double[][][] batches = inputs.Subgroups(groups);
            // Learning data for the specified layer.
            double[][][] layerData;

            // Unsupervised learning on each hidden layer, except for the output layer.
            for (int layerIndex = 0; layerIndex < network.Machines.Count - 1; layerIndex++)
            {
                teacher.LayerIndex = layerIndex;
                layerData = teacher.GetLayerInput(batches);

                for (int i = 0; i < (inputs.Length / 2); i++)
                {
                    double error = teacher.RunEpoch(layerData) / inputs.Length;

                    writer?.WriteOutput(i + ", Error = " + error);

                }
            }

            var teacher2 = new BackPropagationLearning(network)
            {
                LearningRate = 0.1,
                Momentum = 0.5
            };

            // Run supervised learning.
            for (int i = 0; i < inputs.Length; i++)
            {
                double error = teacher2.RunEpoch(inputs, outputs) / inputs.Length;
                writer?.WriteOutput(i + ", Error = " + error);
            }

        }

    }
}
