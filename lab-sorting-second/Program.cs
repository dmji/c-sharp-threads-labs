using System;
using System.Threading;
using System.Collections.Generic;

namespace lab_sorting_second
{
    class Program
    {
        static int[] resultArray;
        static void Main(string[] args)
        {
            bool CONSOLE_LOG = false;
            System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
            Console.WriteLine("Enter array size: ");
            int arrSize = Convert.ToInt32(Console.ReadLine());
            int[] array;
            int max = 0, min = int.MaxValue;
            if (arrSize == -1)
            {
                arrSize = 10000;
                int testToArr = 0;
                array = new int[arrSize];
                string testTXT = System.IO.File.ReadAllText("test10k.txt");
                while (testTXT.Contains(','))
                {
                    array[testToArr++] = Convert.ToInt32(testTXT.Substring(0, testTXT.IndexOf(',')));
                    testTXT=testTXT.Substring(testTXT.IndexOf(',')+2);
                    if (array[testToArr - 1] > max)
                        max = array[testToArr - 1];
                    else if (array[testToArr - 1] < min)
                        min = array[testToArr - 1];
                }
                array[testToArr] = Convert.ToInt32(testTXT);
                if (array[testToArr] > max)
                    max = array[testToArr];
                else if (array[testToArr] < min)
                    min = array[testToArr];
            }
            else if (arrSize == -2)
            {
                arrSize = 100000;
                int testToArr = 0;
                array = new int[arrSize];
                string testTXT = System.IO.File.ReadAllText("test100k.txt");
                while (testTXT.Contains(','))
                {
                    array[testToArr++] = Convert.ToInt32(testTXT.Substring(0, testTXT.IndexOf(',')));
                    testTXT = testTXT.Substring(testTXT.IndexOf(',') + 2);
                    if (array[testToArr-1] > max)
                        max = array[testToArr-1];
                    else if (array[testToArr-1] < min)
                        min = array[testToArr-1];
                }
                array[testToArr] = Convert.ToInt32(testTXT);
                if (array[testToArr] > max)
                    max = array[testToArr];
                else if (array[testToArr] < min)
                    min = array[testToArr];
            }
            else if(arrSize<-2)
            {
                arrSize = -arrSize;
                array = new int[arrSize];
                for (int i = 0; i < arrSize; i++)
                {
                    if (i % 10 == 0)
                        array[i] = 1000;
                    else
                        array[i] = 1;
                }
            }
            else
            {
                array = new int[arrSize];
                for (int i = 0; i < arrSize; i++)
                {
                    array[i] = new Random().Next(arrSize);
                    if (array[i] > max)
                        max = array[i];
                    else if (array[i] < min)
                        min = array[i];
                }
            }
            Console.WriteLine("Enter max threads count: ");
            int threadCountMax = Convert.ToInt32(Math.Log2(Convert.ToDouble(Console.ReadLine()))) + 1;
            if (CONSOLE_LOG)
            {
                Console.WriteLine("\nBefore sort: ");
                foreach (int d in array)
                    Console.Write(d.ToString() + " ");
            }
            for (int THRDLOOP = 0; THRDLOOP < threadCountMax; THRDLOOP++)
            {
                resultArray = new int[arrSize];
                int threadCount = Convert.ToInt32(Math.Pow(2, THRDLOOP));
                int eps=0, stind = 0;
                time.Restart();
                bubleSort[] thread = new bubleSort[threadCount];
                List<int> arrPart = new List<int>();
                for (int i = 0; i < threadCount; i++)
                {
                    //Console.WriteLine($"#i={i} board={min + Convert.ToDouble(i * max) / threadCount}");
                    arrPart.Clear();
                    for (int j = 0; j < arrSize; j++)
                    {
                        if (i == threadCount - 1) eps = 1;
                        if (array[j] >= min + Convert.ToDouble(i * (max - min)) / threadCount && array[j] < eps+ min + Convert.ToDouble((i + 1) * (max - min)) / threadCount)
                            arrPart.Add(array[j]);
                    }
                    thread[i] = new bubleSort(arrPart,stind);
                    stind = stind + arrPart.Count;
                }
               // Console.WriteLine($"#i={threadCount} board={min + Convert.ToDouble(threadCount * (max-min)) / threadCount}");
                for (int j = 0; j < threadCount; j++) thread[j].thrd.Join();

                //int[] resultArray = new int[arrSize];
                //for (int i = 0; i < threadCount; i++)
                //    for (int j = 0; j < thread[i].arr.Length; j++)
                //        resultArray[parted++] = thread[i].arr[j];
                time.Stop();
                if (CONSOLE_LOG)
                {
                    Console.WriteLine($"\nAfter {threadCount}-thread sort: ");
                    foreach (int d in resultArray)
                        Console.Write(d.ToString() + " ");
                }
                Console.WriteLine($"{threadCount}-thread sorting time: " + time.ElapsedMilliseconds);
            }
        }
        class bubleSort
        {
            public int[] arr;
            int startind;
            public Thread thrd;
            public bubleSort(int[] src,int stind)
            {
                startind = stind;
                arr = src;
                thrd = new Thread(Run);
                thrd.Start();
            }
            public bubleSort(List<int> src, int stind)
            {
                startind = stind;
                arr = src.ToArray();
                thrd = new Thread(Run);
                thrd.Start();
            }
            public void Run()
            {
                for (int j = 0; j < arr.Length - 1; j++)
                    for (int i = arr.Length - 1; i > j; i--)
                        if (arr[i - 1] > arr[i])
                        {
                            int s = arr[i - 1];
                            arr[i - 1] = arr[i];
                            arr[i] = s;
                        }
                for (int j = 0; j < arr.Length; j++)
                    resultArray[startind++] = arr[j];
            }
        }
    }
}