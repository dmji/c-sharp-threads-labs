using System;
using System.Threading;

namespace lab_sorting
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
            int arrSize;
            int threadCount=2;
            Console.WriteLine("Enter array size: ");
            arrSize = Convert.ToInt32(Console.ReadLine());
            int[] array = new int[arrSize];
            for (int i = 0; i < arrSize; i++)
                array[i] = new Random().Next(0, arrSize * 2);

            int[] arrayOneCore = array;
            time.Start();
            bubleSort test = new bubleSort(array);
            test.thrd.Join();
            time.Stop();
            long timeOneCore = time.ElapsedMilliseconds;
            //Console.WriteLine("\nAfter 1-thread sort: ");
            //foreach (int d in test.arr)
            //    Console.Write(d.ToString() + " ");
            Console.WriteLine("1-thread sorting time: " + time.ElapsedMilliseconds);

            do
            {
                //Console.WriteLine("Enter threads count: ");
                //threadCount = Convert.ToInt32(Console.ReadLine());
                //if (threadCount == 0) break;
                //Console.WriteLine("\nBefore sort: ");
                //foreach (int d in array)
                //    Console.Write(d.ToString() + " ");

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
                for (int j = 0; j < threadCount; j++)
                    thread[j].thrd.Join();
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
                //Console.WriteLine($"\nAfter {threadCount}-thread sort: ");
                //foreach (int d in resultArray)
                //    Console.Write(d.ToString() + " ");
                Console.WriteLine($"{threadCount}-thread sorting time: " + time.ElapsedMilliseconds);
                threadCount++;
            } while (time.ElapsedMilliseconds < timeOneCore || threadCount < 17);
        }
        class bubleSort
        {
            public int[] ind;
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