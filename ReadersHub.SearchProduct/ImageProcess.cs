using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Net;

namespace ReadersHub.SearchProduct
{
    public class ImageProcess
    {
        static BitmapCompare SimpleCompare;

        public static Image FixedSize(Image imgPhoto, int Width, int Height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = System.Convert.ToInt16((Width -
                              (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = System.Convert.ToInt16((Height -
                              (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height,
                              PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                             imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Red);
            grPhoto.InterpolationMode =
                    InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        public static double PixelsSmilartyRatio(Bitmap orjiImage, Bitmap anotherImage)
        {
            Bitmap img1 = orjiImage;
            Bitmap img2 = new Bitmap(FixedSize(anotherImage, img1.Width, img1.Height));

            if (img1.Size != img2.Size)
            {
                //Console.Error.WriteLine("Images are of different sizes");
                return 100;
            }

            float diff = 0;

            for (int y = 0; y < img1.Height; y++)
            {
                for (int x = 0; x < img1.Width; x++)
                {
                    diff += (float)Math.Abs(img1.GetPixel(x, y).R - img2.GetPixel(x, y).R) / 255;
                    diff += (float)Math.Abs(img1.GetPixel(x, y).G - img2.GetPixel(x, y).G) / 255;
                    diff += (float)Math.Abs(img1.GetPixel(x, y).B - img2.GetPixel(x, y).B) / 255;
                }
            }

            return 100 * diff / (img1.Width * img1.Height * 3);
        }

        public static double SmilartyRatio(string isbnImageURL, string asinImageURL)
        {
            SimpleCompare = new BitmapCompare();

            WebClient webClient = new WebClient();

            byte[] asinImgByte = webClient.DownloadData(asinImageURL);
            byte[] isbnImgByte = webClient.DownloadData(isbnImageURL);

            MemoryStream asinMemStream = new MemoryStream(asinImgByte);
            MemoryStream isbnMemStream = new MemoryStream(isbnImgByte);

            Bitmap isbnImage = new Bitmap(Image.FromStream(isbnMemStream));
            Bitmap asinImage = new Bitmap(Image.FromStream(asinMemStream));

            if (PixelsSmilartyRatio(isbnImage, asinImage) >= 25)
            {
                return 0;
            }

            return Math.Round(SimpleCompare.GetSimilarity(isbnImage, asinImage), 3);
        }

        public interface IBitmapCompare
        {
            double GetSimilarity(Bitmap a, Bitmap b);
        }

        class BitmapCompare : IBitmapCompare
        {
            public struct RGBdata
            {
                public int r;
                public int g;
                public int b;

                public int GetLargest()
                {
                    if (r > b)
                    {
                        if (r > g)
                        {
                            return 1;
                        }
                        else
                        {
                            return 2;
                        }
                    }
                    else
                    {
                        return 3;
                    }
                }
            }

            private RGBdata ProcessBitmap(Bitmap a)
            {
                BitmapData bmpData = a.LockBits(new Rectangle(0, 0, a.Width, a.Height), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
                IntPtr ptr = bmpData.Scan0;
                RGBdata data = new RGBdata();

                unsafe
                {
                    byte* p = (byte*)(void*)ptr;
                    int offset = bmpData.Stride - a.Width * 3;
                    int width = a.Width * 3;

                    for (int y = 0; y < a.Height; ++y)
                    {
                        for (int x = 0; x < width; ++x)
                        {
                            data.r += p[0];             //gets red values
                            data.g += p[1];             //gets green values
                            data.b += p[2];             //gets blue values
                            ++p;
                        }
                        p += offset;
                    }
                }
                a.UnlockBits(bmpData);
                return data;
            }

            public double GetSimilarity(Bitmap a, Bitmap b)
            {
                RGBdata dataA = ProcessBitmap(a);
                RGBdata dataB = ProcessBitmap(b);
                double result = 0;
                int averageA = 0;
                int averageB = 0;
                int maxA = 0;
                int maxB = 0;

                maxA = ((a.Width * 3) * a.Height);
                maxB = ((b.Width * 3) * b.Height);

                switch (dataA.GetLargest())            //Find dominant color to compare
                {
                    case 1:
                        {
                            averageA = Math.Abs(dataA.r / maxA);
                            averageB = Math.Abs(dataB.r / maxB);
                            result = (averageA - averageB) / 2;
                            break;
                        }
                    case 2:
                        {
                            averageA = Math.Abs(dataA.g / maxA);
                            averageB = Math.Abs(dataB.g / maxB);
                            result = (averageA - averageB) / 2;
                            break;
                        }
                    case 3:
                        {
                            averageA = Math.Abs(dataA.b / maxA);
                            averageB = Math.Abs(dataB.b / maxB);
                            result = (averageA - averageB) / 2;
                            break;
                        }
                }

                result = Math.Abs((result + 100) / 100);

                if (result > 1.0)
                {
                    result -= 1.0;
                }

                return result;
            }
        }

    }
}
