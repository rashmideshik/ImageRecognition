using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.MachineLearning;
//using Accord.Imaging;
using Accord.IO;
using Accord.Statistics;
using System.Drawing;
using Accord.Neuro;
using Accord.Neuro.Networks;
using System.IO;
using System.Drawing.Imaging;
using System.Collections;
using System.Windows.Forms;


namespace ImageRecognition
{
    /// <summary>
    /// This Class takes the images from a directory 
    /// converts into binay data and trains the accord framework using deepbelief algorithm
    /// </summary>   
    public class ImageRecognition
    {
        #region member variables
        /// <summary>
        /// Member variables are always in the form m_FirstLettersAreCapital
        /// Do not declare member variables public and protected. Use Properties to get and set values
        /// of memeber variables.
        /// Exceptions:
        /// You can declare them as protected if needed for derivation.
        /// You can also declare them public for serialization.
        /// </summary>

        private static readonly String StartupPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
        private static readonly String CoreDataDirectory = "CoreData";
        private static readonly String trainingDataPath = Path.Combine(StartupPath, "res\\training");
        private static readonly String TrainingFile = "train.dat";
        private static readonly String trainingFilePath = Path.Combine(StartupPath, TrainingFile);
        private static readonly String coreDataPath = Path.Combine(StartupPath, Path.Combine("res", CoreDataDirectory));

        IOutputWriter writer = new ConsoleOutputWriter();

        DeepLearningTraining training = new DeepLearningTraining();
        #endregion

        static void Main(string[] args)
        {
            ImageRecognition imageRecognition = new ImageRecognition();
            imageRecognition.DoImageRecognition();
        }

        public void DoImageRecognition()
        {
            //check if training file already exist?
            //string trainingFilePath = Path.Combine(StartupPath, TrainingFile);
            //string coreDataPath = Path.Combine(StartupPath, Path.Combine("res", CoreDataDirectory));

            if (File.Exists(trainingFilePath))
            {
                writer.WriteOutput("Training loaded from the file");
                training.LoadNetwork(trainingFilePath);
            }

            string[] images;
            if (Directory.Exists(trainingDataPath))
            {
                //new files arrived in training folder
                string[] files = Directory.GetFiles(trainingDataPath);
                if (files.Length > 0)
                {
                    DoNewTraining();
                    //saving training data
                    training.SaveNetwork(trainingFilePath);
                    //moving files to core data folder
                    PushDatatoCoreData();

                }
            }

            //testing
            writer.WriteOutput("Positive testing scenario");

            string testingData = Path.Combine(StartupPath, "res\\positiveTesting");
            images = Directory.GetFiles(testingData, "*");
            DeepLearningTesting testing = new DeepLearningTesting();
            testing.DoTesting(training.GetNetwork(), images, writer);

            // Negative testing
            writer.WriteOutput("Negative testing scenario");
            string negativeTestingData = Path.Combine(StartupPath, "res\\negativeTesting");
            images = Directory.GetFiles(negativeTestingData, "*");
            testing.DoTesting(training.GetNetwork(), images, writer);

            //wait for return key
            Console.Read();
        }


        private void DoNewTraining()
        {
            //if file is not exist then do training first.
            //training
            writer.WriteOutput("Training..");

            string[] images = Directory.GetFiles(trainingDataPath, "*");
            int imagesToCopy = 20;

            training.DoTraining(images, imagesToCopy, writer);
        }
        private void PushDatatoCoreData()
        {
            string[] images = Directory.GetFiles(trainingDataPath, "*");

            foreach (string trainedimage in images)
            {
                FileInfo info = new FileInfo(trainedimage);
                try
                {
                    MoveFile(info.FullName, Path.Combine(coreDataPath, info.Name));
                }
                catch (IOException ex)
                {
                    if (ex.Message.Equals("Cannot create a file when that file already exists.\r\n"))
                    {
                        Random rand = new Random();
                        int val = rand.Next(99999);
                        string newName = info.Name + "_" + val + "." + info.Extension;
                        try
                        {
                            MoveFile(info.FullName, Path.Combine(coreDataPath, newName));
                        }
                        catch (IOException exinner)
                        {
                            //ignore
                        }


                    }
                }


                //
            }
        }

        private void MoveFile(string sourceFile, string destFile)
        {
            File.Move(sourceFile, destFile);
        }

        public void WriteOutput(string value)
        {

        }
    }
}
