using System;
using System.Threading;
using System.Timers;

namespace lab_sorting
{
    class Program
    {
        private static System.Timers.Timer aTimer;

        static void Main(string[] args)
        {
            System.Diagnostics.Stopwatch time = new System.Diagnostics.Stopwatch();
            int arrSize;
            int threadCount;

            Console.WriteLine("Enter array size: ");
            arrSize = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter threads count: ");
            threadCount = Convert.ToInt32(Console.ReadLine());

            int[] array = new int[arrSize];
            for (int i = 0; i < arrSize; i++)
                array[i] = new Random().Next(0, arrSize * 5);

            Console.WriteLine("\nBefore sort: ");
            foreach (int d in array)
                Console.Write(d.ToString() + " ");

            int eps = arrSize % threadCount, eps_inc = 0, parted = 0;
            if (eps > 0) eps_inc = 1;
            time.Start();
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
            /////////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////
            /////////////////////////////////////////////////////////////////
            int[] index = new int[threadCount];
            int[] lastInt = new int[threadCount];
            for(int i=0;i<threadCount;i++)
            {
                index[i] = i;
                lastInt[i] = thread[i].arr[thread[i].arr.Length - 1];
            }
            bubleSort concatSort = new bubleSort(lastInt, index);
            concatSort.thrd.Join();
            int[] resultArray = new int[arrSize];
            parted--;
            for (int i = 0; i < threadCount; i++)
                for (int j = 0; j < thread[i].arr.Length; j++)
                    resultArray[parted--] = thread[i].arr[thread[i].arr.Length - 1 - j];
            time.Stop();
           
            Console.WriteLine($"\nAfter {threadCount}-thread sort: ");
            foreach (int d in resultArray)
                Console.Write(d.ToString() + " ");
            Console.WriteLine($"\n{threadCount}-thread sorting time: " + time.Elapsed + "\n");
            /////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////
            ////////////////////////////////////////////////////////////////////
            time.Restart();
            bubleSort test = new bubleSort(array);
            test.thrd.Join();
            time.Stop();
            
            Console.WriteLine("\nAfter 1-thread sort: ");
            foreach (int d in test.arr)
                Console.Write(d.ToString() + " ");
            Console.WriteLine("\n1-thread sorting time: " + time.Elapsed + "\n");
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
            public bubleSort(int[] src,int[] srcInd)
            {
                arr = src;
                ind = srcInd;
                thrd = new Thread(RunAdv);
                thrd.Start();
            }

            public void Run()
            {
                int s = 0;
                for (int j = 0; j < arr.Length - 1; j++)
                    for (int i = arr.Length - 1; i > j; i--)
                        if (arr[i - 1] > arr[i])
                        {
                            s = arr[i - 1];
                            arr[i - 1] = arr[i];
                            arr[i] = s;
                        }
            }

            public void RunAdv()
            {
                int s = 0;
                for (int j = 0; j < arr.Length - 1; j++)
                    for (int i = arr.Length - 1; i > j; i--)
                        if (arr[i - 1] > arr[i])
                        {
                            s = arr[i - 1];
                            arr[i - 1] = arr[i];
                            arr[i] = s;

                            s = ind[i - 1];
                            ind[i - 1] = ind[i];
                            ind[i] = s;
                        }
            }
        }

        
    }
}