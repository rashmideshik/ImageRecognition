using Accord.Neuro;
using Accord.Neuro.Networks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageRecognition
{
    class DeepLearningNetworkBuilder
    {
        private static readonly int hiddenNeurons = 10;

        private static readonly double stdDev = 0.2;

        public static DeepBeliefNetwork CreateAndGetNetwork(int inputsCount)
        {
            DeepBeliefNetwork network = new DeepBeliefNetwork(inputsCount, hiddenNeurons, hiddenNeurons);
            new GaussianWeights(network, stdDev).Randomize();
            network.UpdateVisibleWeights();

            return network;
        }


    }
}
