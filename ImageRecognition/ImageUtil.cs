using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageRecognition
{
    public class ImageUtil
    {
        public int Width { get; set; }
        public int Height { get; set; }

        //Parameterized Constructor
        public ImageUtil(int x, int y)
        {
            Width = x;
            Height = y;
        }

        public ImageUtil() { }

        public void GenerateBinaryFile(String binaryFilePath, string[] images)
        {

            //delete existing output file
            if (File.Exists(binaryFilePath))
            {
                File.Delete(binaryFilePath);
            }

            foreach (var image in images)
            {
                // appending gray scale bit iamge in single file 
                AppendImageBinary(image, binaryFilePath);
            }

        }

        private void AppendImageBinary(string image, string binaryFilePath)
        {
            //saving images in Bitmap format
            var names = image.Split('\\');
            string filename = string.Empty;
            foreach (var item in names)
            {
                // if (item.Contains("."))
                filename = " " + item.Substring(0, 1);
            }
            //int x = 64;
            //int y = 64;
            //Loading Bmp images        
            Bitmap Imgbmp = resizeImage((Bitmap)System.Drawing.Image.FromFile(image), new Size(Width,Height));
            //convert each image in to grayscale formant
            Accord.Imaging.Filters.Grayscale filter = new Accord.Imaging.Filters.Grayscale(0.1125, 0.2154, 0.3121);
            Bitmap grayImage = filter.Apply(Imgbmp);
            //  grayImage.Save("d:\\rash.bmp");
            using (StreamWriter writer = File.AppendText(binaryFilePath))
            {
                Bitmap img = new Bitmap(grayImage);
                StringBuilder t = new StringBuilder();
                int hg = img.Height;
                int wg = img.Width;
                for (int i = 0; i < hg; i++)
                {
                    for (int j = 0; j < wg; j++)
                    {
                        // t = 0 .299R + 0 .587G + 0 .144B
                        t.Append((img.GetPixel(j, i).R > 100 && img.GetPixel(j, i).G > 100 &&
                           img.GetPixel(j, i).B > 100) ? 1 : 0);
                        //  t.Append(img.GetPixel(i,j).R);
                    }
                    t.AppendLine();
                }
                string text = t.ToString();
                writer.Write(text);
                writer.WriteLine(filename);
            }

        }

        private static Bitmap resizeImage(Bitmap imgToResize, Size newSize)
        {
            return new Bitmap(imgToResize, newSize);
        }
        private static byte[] ToByteArray(Bitmap bitmap, ImageFormat format)
        {
            Image img = (Image)bitmap;
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, format);
                return ms.ToArray();
            }
        }


    }
}
