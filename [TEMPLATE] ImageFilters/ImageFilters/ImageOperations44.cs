using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
namespace ImageFilters
{
    public class ImageOperations
    {
        /// <summary>
        /// Open an image, convert it to gray scale and load it into 2D array of size (Height x Width)
        /// </summary>
        /// <param name="ImagePath">Image file path</param>
        /// <returns>2D array of gray values</returns>
        public static byte[,] OpenImage(string ImagePath)
        {
            Bitmap original_bm = new Bitmap(ImagePath);
            int Height = original_bm.Height;
            int Width = original_bm.Width;
              
            byte[,] Buffer = new byte[Height, Width];  

            unsafe
            {
                BitmapData bmd = original_bm.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, original_bm.PixelFormat);
                int x, y;
                int nWidth = 0;
                bool Format32 = false;
                bool Format24 = false;
                bool Format8 = false;

                if (original_bm.PixelFormat == PixelFormat.Format24bppRgb)
                {
                    Format24 = true;
                    nWidth = Width * 3;
                }
                else if (original_bm.PixelFormat == PixelFormat.Format32bppArgb || original_bm.PixelFormat == PixelFormat.Format32bppRgb || original_bm.PixelFormat == PixelFormat.Format32bppPArgb)
                {
                    Format32 = true;
                    nWidth = Width * 4;
                }
                else if (original_bm.PixelFormat == PixelFormat.Format8bppIndexed)
                {
                    Format8 = true;
                    nWidth = Width;
                }
                int nOffset = bmd.Stride - nWidth;
                byte* p = (byte*)bmd.Scan0;
                for (y = 0; y < Height; y++)
                {
                    for (x = 0; x < Width; x++)
                    {
                        if (Format8)
                        {
                            Buffer[y, x] = p[0];
                            p++;
                        }
                        else
                        {
                            Buffer[y, x] = (byte)((int)(p[0] + p[1] + p[2]) / 3);
                            if (Format24) p += 3;
                            else if (Format32) p += 4;
                        }
                    }
                    p += nOffset;
                }
                original_bm.UnlockBits(bmd);
            }

            return Buffer;
        }

        /// <summary>
        /// Get the height of the image 
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <returns>Image Height</returns>
        public static int GetHeight(byte[,] ImageMatrix)
        {
            return ImageMatrix.GetLength(0);
        }

        /// <summary>
        /// Get the width of the image 
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <returns>Image Width</returns>
        public static int GetWidth(byte[,] ImageMatrix)
        {
            return ImageMatrix.GetLength(1);
        }

        /// <summary>
        /// Display the given image on the given PictureBox object
        /// </summary>
        /// <param name="ImageMatrix">2D array that contains the image</param>
        /// <param name="PicBox">PictureBox object to display the image on it</param>
        public static void DisplayImage(byte[,] ImageMatrix, PictureBox PicBox)
        {
            // Create Image:
            //==============
            //length(0) bygeb element el fel row
            //length(1) bygeb elment el fel column
            int Height = ImageMatrix.GetLength(0);
            int Width = ImageMatrix.GetLength(1);
            //Bitmap it is bitarray to store pixels
            Bitmap ImageBMP = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
            //unsafe is declaration of a type or member or to specify a block code ( el code haydrb mn gherha)
            unsafe
            {
                //LockBits locks access to the bitmap's bits
                BitmapData bmd = ImageBMP.LockBits(new Rectangle(0, 0, Width, Height), ImageLockMode.ReadWrite, ImageBMP.PixelFormat);
                int nWidth = 0;
                nWidth = Width * 3;
                int nOffset = bmd.Stride - nWidth;
                byte* p = (byte*)bmd.Scan0;
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        p[0] = p[1] = p[2] = ImageMatrix[i, j];
                        p += 3;
                    }

                    p += nOffset;
                }
                ImageBMP.UnlockBits(bmd);
            }
            PicBox.Image = ImageBMP;
        }
         public static byte[] COUNTING_SORT(byte[] Array, int ArrayLength, byte Max, byte Min)//
         {
             byte[] count = new byte[Max - Min + 1];//Range in array elements 
             int z = 0;

             for (int i = 0; i < count.Length; i++)
            { count[i] = 0; }
             for (int i = 0; i < ArrayLength; i++) 
            { count[Array[i] - Min]++; } 

             for (int i = Min; i <= Max; i++)
             {
                 while (count[i - Min]-- > 0)
                 {
                     Array[z] = (byte)i;
                     z++;
                 }
             }
             return Array;
         }

        public static int Partition(byte[] Arr, int L, int r)
        {
            byte x = Arr[r];
            byte temp;
            int i = L;
            for (int j = L; j < r; j++)
            {
                if (Arr[j] <= x)
                {

                    temp = Arr[j];
                    Arr[j] = Arr[i];
                    Arr[i++] = temp;
                }
            }
            temp = Arr[i];
            Arr[i] = Arr[r];
            Arr[r] = temp;
            return i;
        }

        public static byte[] Quick_Sort(byte[] Array, int l, int r)
        {
            if (l < r)
            {
                int pivot =  Partition(Array, l, r);
                Quick_Sort(Array, l, pivot - 1);
                Quick_Sort(Array, pivot + 1, r);
            }
            return Array;
        }

        public static byte AlphaTrimFilter(byte[,] ImageMatrix, int x, int y, int Wmax, int Sort,int trim)
        {
            byte[] Array;
            int[] Dx, Dy;//height //width
            int min = 0 , max=0 , zero=0;
            if (Wmax % 2 != 0)
            {
                Array = new byte[Wmax * Wmax];
                Dx = new int[Wmax * Wmax];
                Dy = new int[Wmax * Wmax];
            }
            else
            {
                Array = new byte[(Wmax + 1) * (Wmax + 1)];
                Dx = new int[(Wmax + 1) * (Wmax + 1)];
                Dy = new int[(Wmax + 1) * (Wmax + 1)];
            }


            int Index = 0;
            for (int _y = -(Wmax / 2); _y <= (Wmax / 2); _y++)
            {
                for (int _x = -(Wmax / 2); _x <= (Wmax / 2); _x++)
                {
                    Dx[Index] = _x;
                    Dy[Index] = _y;
                    Index++;
                }
            }
            byte Max, Min, Z;
            int ArrayLength, Sum, NewY, NewX, Avg=0;
            Sum = 0 ;
            int Sum_Total = 0;
            Max = 0;
            Min = 255;
            ArrayLength = 0;
            Z = ImageMatrix[y, x];
            for (int i = 0; i < Wmax * Wmax; i++)
            {
                NewY = y + Dy[i];
                NewX = x + Dx[i];
                if (NewX >= 0 && NewX < GetWidth(ImageMatrix) && NewY >= 0 && NewY < GetHeight(ImageMatrix))
                {
                    Array[ArrayLength] = ImageMatrix[NewY, NewX];//height and width
                    if (Array[ArrayLength] > Max)
                        Max = Array[ArrayLength];
                    if (Array[ArrayLength] < Min)
                        Min = Array[ArrayLength];
                      Sum_Total += Array[ArrayLength];
                     ArrayLength++;
                }
            }
            if(Sort==1)
            { 
                Array = COUNTING_SORT(Array, ArrayLength, Max, Min);
                byte[] trimmedArray = new byte [Array.Length - (trim*2)];                                                         
                for (int i = trim; i < ArrayLength-trim; i++)
                    trimmedArray[i - trim] = Array[i];

                for (int i = 0; i < trimmedArray.Length; i++)
                    Sum += trimmedArray[i];
                 Avg = Sum / trimmedArray.Length;    
                  return (byte)Avg;
            }
            /*else 
            {
                 byte[] trimmedArray = new byte [Array.Length - (trim*2)]; 
                for(int i=0 ; i < trim ; i++)
                {
                     for(int j=0 ; j < Array.Length ; j++)
                     { 
                        if(Array[j] < Min) 
                        {   Min=Array[j];
                            min = j;
                        }
                        else if(Array[j] > Max)
                        { 
                            Max=Array[j];
                            max=j;
                        }
                     }
                     for (int j = 0; j < trimmedArray; j++)
                     { 
                        
                        if(j==min)
                        {
                            if (j==trimmedArray-1) break;
                            trimmedArray[j]=Array[j+1];
                        }
                        else if(j==max)
                        {
                             if(j==0)continue;
                            trimmedArray[j]=Array[j-1];
                        }
                        else trimmedArray[j]==Array[j];   
                    //trimmedArray[i - trim] = Array[i];
                     }
                }
               
                for (int i = 0; i < trimmedArray.Length; i++)
                    Sum += trimmedArray[i];
                 Avg = Sum / trimmedArray.Length;    
                  return (byte)Avg;
                /*
                byte[] newArr = new byte[Array.Length-(trim*2)];
                for(int i=0 ; i < trim ; i++)
                {
                    
                     for(int j=0 ; j < ArrayLength-trim; j++)
                     {
                        if(j==min)newArr[j]=Array[j+1];
                        else newArr[j]==Array[j];
                        if(j==max)newArr[j]=Array[j-1];
                        else newArr[j]==Array[j]; 
                     }
                     
                     for(int i=0 ; i< newArr.Length ; i++)
                     Sum+=newArr[i];
                     Avg = Sum /newArr.Length;    
                     return (byte)Avg;
                */
               
             else 
             {
                Sum_Total -= Max;
                Sum_Total -= Min;
                ArrayLength -= 2;
                Avg = Sum_Total / ArrayLength;
                return (byte)Avg;
             }
        }
         public static byte AdaptiveMedianFilter(byte[,] ImageMatrix, int x, int y, int window_size, int Wmax, int Sort)
         {

            byte[] Array = new byte[window_size * window_size];
            int[] _xfilter = new int[window_size * window_size];
            int[] _yfilter = new int[window_size * window_size];
            int Index = 0;
            for (int _y = -(window_size / 2); _y <= (window_size / 2); _y++)
            {
                for (int _x = -(window_size / 2); _x <= (window_size / 2); _x++)
                {
                    _xfilter[Index] = _x;
                    _yfilter[Index] = _y;
                    Index++;
                }
            }
            byte Max, Min, Med, Z;
            int A1, A2, B1, B2, Arr_Length, NewY, NewX;
            Max = 0;
            Min = 255;
            Arr_Length = 0;
            Z = ImageMatrix[y, x];
            for (int i = 0; i < window_size * window_size; i++)
            {
                NewY = y + _yfilter[i];
                NewX = x + _xfilter[i];
                if (NewX >= 0 && NewX < GetWidth(ImageMatrix) && NewY >= 0 && NewY < GetHeight(ImageMatrix))
                {
                    Array[Arr_Length] = ImageMatrix[NewY, NewX];
                    if (Array[Arr_Length] > Max)
                        Max = Array[Arr_Length];
                    if (Array[Arr_Length] < Min)
                        Min = Array[Arr_Length];
                    Arr_Length++;
                }
            }
            if ( Sort == 3) Array = Quick_Sort(Array, 0, Arr_Length - 1);
            else if (Sort == 1) Array = COUNTING_SORT(Array, Arr_Length, Max, Min);

            Min = Array[0];
            Med = Array[Arr_Length / 2];
            A1 = Med - Min;
            A2 = Max - Med;
            if (A1 > 0 && A2 > 0)//non noise filter 
            {
                B1 = Z - Min;
                B2 = Max - Z;
                if (B1 > 0 && B2 > 0)//non-noise pixel
                    return Z;
                else
                {
                    if (window_size + 2 < Wmax)
                        return AdaptiveMedianFilter(ImageMatrix, x, y, window_size + 2, Wmax, Sort);
                    else
                        return Med;
                }
            }
            else
            {
                return Med;
            }

        }
         public static byte[,] ImageFilter(byte[,] ImageMatrix, int Max_Size, int Sort, int filter,int trim)
        {
            //int trim = 3;
      
            byte[,] ImageMatrix2 = ImageMatrix;
            for (int y = 0; y < GetHeight(ImageMatrix); y++)
            {
                for (int x = 0; x < GetWidth(ImageMatrix); x++)
                {
                    if (filter == 1)
                    { 
                       
                        ImageMatrix2[y, x] = AlphaTrimFilter(ImageMatrix, x, y, Max_Size,Sort ,trim );
                    }
                    else
                        ImageMatrix2[y, x] = AdaptiveMedianFilter(ImageMatrix, x, y, 3, Max_Size, Sort);
                }
            }

            return ImageMatrix2;
         }
    }
}