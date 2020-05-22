using System;
using System.Threading;

namespace lab_sorting
{
    class Program
    {
        static void Main(string[] args)
        {
            int CONSOLE_LOG = 0;
            System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
            Console.WriteLine("Enter array size: ");
            int arrSize = Convert.ToInt32(Console.ReadLine());
            int[] array;
            if (arrSize == -1)
            {
                arrSize = 10000;
                int testToArr = 0;
                array = new int[arrSize];
                string testTXT = System.IO.File.ReadAllText("test10k.txt");
                while (testTXT.Contains(','))
                {
                    array[testToArr++] = Convert.ToInt32(testTXT.Substring(0, testTXT.IndexOf(',')));
                    testTXT = testTXT.Substring(testTXT.IndexOf(',') + 2);
                }
                array[testToArr] = Convert.ToInt32(testTXT);
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
                }
                array[testToArr] = Convert.ToInt32(testTXT);
            }
            else if (arrSize < -2)
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
                    array[i] = new Random().Next(arrSize);
            }
            Console.WriteLine("Enter max threads count: ");
            int threadCountMax = Convert.ToInt32( Math.Log2( Convert.ToDouble(Console.ReadLine())))+1;

            for (int THRDLOOP = 0; THRDLOOP < threadCountMax; THRDLOOP++)
            {
                if (CONSOLE_LOG == 1)
                {
                    Console.WriteLine("\nBefore sort: ");
                    foreach (int d in array)
                        Console.Write(d.ToString() + " ");
                }
                int threadCount = Convert.ToInt32(Math.Pow(2, THRDLOOP));
                int eps = arrSize % threadCount, eps_inc = 0, parted = 0;
                if (eps > 0) eps_inc = 1;
                time.Restart();
                bubleSort[] thread = new bubleSort[threadCount];
                for (int i = 0; i < threadCount; i++)
                {
                    int[] arrPart = new int[arrSize / threadCount + eps_inc];
                    if (eps != 0)
                    {
                        eps--;
                        if (eps == 0)
                            eps_inc = 0;
                    }
                    for (int j = 0; j < arrPart.Length; j++)
                        arrPart[j] = array[parted++];
                    thread[i] = new bubleSort(arrPart);
                }
                for (int j = 0; j < threadCount; j++) thread[j].thrd.Join();

                int[] index = new int[threadCount];
                int[] resultArray = new int[arrSize];
                for (int i = 0; i < threadCount; i++)
                    index[i] = 0;
                parted = 0;
                do
                {
                    int curMin = -1;
                    for (int i = 0; i < threadCount; i++)
                    {
                        if (index[i] < thread[i].arr.Length)
                        {
                            if (curMin == -1)
                                curMin = i;
                            else if (thread[curMin].arr[index[curMin]] > thread[i].arr[index[i]])
                                curMin = i;
                        }
                    }
                    resultArray[parted++] = thread[curMin].arr[index[curMin]];
                    index[curMin]++;
                } while (parted < arrSize);
                time.Stop();
                if (CONSOLE_LOG == 1)
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
            public Thread thrd;
            public bubleSort(int[] src)
            {
                arr = src;
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
            }
        }
    }
}