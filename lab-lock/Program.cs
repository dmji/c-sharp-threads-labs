using System;
using System.Threading;

namespace lab_lock
{
    class Program
    {
        class SumArray
        {
            int sum;
            object lockOn = new object();
            
            public int Sum(int[] nums)
            {
                lock(lockOn)
                {
                    sum = 0;
                    for(int i=0;i<nums.Length;i++)
                    {
                        sum += nums[i];
                        Console.WriteLine("Current sum for thread " + Thread.CurrentThread.Name + " = " + sum);
                        Thread.Sleep(10);
                    }
                    return sum;
                }
            }
        }

        class SumArrayNL
        {
            int sum;
            object lockOn = new object();

            public int Sum(int[] nums)
            {
                sum = 0;
                for (int i = 0; i < nums.Length; i++)
                {
                    sum += nums[i];
                    Console.WriteLine("Current sum for thread " + Thread.CurrentThread.Name + " = " + sum);
                    Thread.Sleep(10);
                }
                return sum;
            }
        }

        class SumThread
        {
            public Thread Thrd;
            int[] a;
            int answer;
            static SumArray sa = new SumArray();
            static SumArrayNL saNL = new SumArrayNL();
            bool LC = true;

            public SumThread (string name, int[] nums,bool Lock=true)
            {
                a = nums;
                Thrd = new Thread(this.Run);
                Thrd.Name = name;
                Thrd.Start();
                this.LC = Lock;
            }

            void Run()
            {
                Console.WriteLine(Thrd.Name + " begun.");
                if (LC == true)
                    answer = sa.Sum(a);
                else
                    answer = saNL.Sum(a);
                Console.WriteLine("Summary for thread " + Thrd.Name + " = " + answer);
                Console.WriteLine(Thrd.Name + " done");
            }
        }

        static void Main(string[] args)
        {
            int[] a = { 1, 2, 3, 4, 5 };
            SumThread st1 = new SumThread("Stream 1", a);
            SumThread st2 = new SumThread("Stream 2", a);
            st1.Thrd.Join();
            st2.Thrd.Join();
            SumThread st3 = new SumThread("StreamNL 1", a,false);
            SumThread st4 = new SumThread("StreamNL 2", a,false);
        }
    }
}
