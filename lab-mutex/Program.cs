using System;
using System.Threading;

namespace lab_mutex
{
    class Program
    {
        class SharedRes
        {
            public static int Count = 0;
            public static Mutex Mtx = new Mutex();
        }

        class IncThread
        {
            int num;
            public Thread Thrd;
            public IncThread(string name, int n)
            {
                Thrd = new Thread(this.Run);
                num = n;
                Thrd.Name = name;
                Thrd.Start();
            }

            void Run()
            {
                Console.WriteLine(Thrd.Name + " wait mutex.");
                SharedRes.Mtx.WaitOne();
                Console.WriteLine(Thrd.Name + " getting mutex.");
                do
                {
                    Thread.Sleep(100);
                    SharedRes.Count++;
                    Console.WriteLine("In stream " + Thrd.Name + ", SharedRes.Count = " + SharedRes.Count);
                    num--;
                } while (num > 0);

                Console.WriteLine(Thrd.Name + "release mutex.");
                SharedRes.Mtx.ReleaseMutex();
            }
        }

        class DecThread
        {
            int num;
            public Thread Thrd;
            public DecThread(string name, int n)
            {
                Thrd = new Thread(this.Run);
                num = n;
                Thrd.Name = name;
                Thrd.Start();
            }

            void Run()
            {
                Console.WriteLine(Thrd.Name + " wait mutex.");
                SharedRes.Mtx.WaitOne();
                Console.WriteLine(Thrd.Name + " getting mutex.");
                do
                {
                    Thread.Sleep(100);
                    SharedRes.Count--;
                    Console.WriteLine("In stream " + Thrd.Name + ", SharedRes.Count = " + SharedRes.Count);
                    num--;
                } while (num > 0);

                Console.WriteLine(Thrd.Name + "release mutex.");
                SharedRes.Mtx.ReleaseMutex();
            }
        }

        class IncThreadNM
        {
            int num;
            public Thread Thrd;
            public IncThreadNM(string name, int n)
            {
                Thrd = new Thread(this.Run);
                num = n;
                Thrd.Name = name;
                Thrd.Start();
            }

            void Run()
            {
                Console.WriteLine(Thrd.Name + " wait mutex.");
                Console.WriteLine(Thrd.Name + " getting mutex.");
                do
                {
                    Thread.Sleep(100);
                    SharedRes.Count++;
                    Console.WriteLine("In stream " + Thrd.Name + ", SharedRes.Count = " + SharedRes.Count);
                    num--;
                } while (num > 0);

                Console.WriteLine(Thrd.Name + "release mutex.");
            }
        }

        class DecThreadNM
        {
            int num;
            public Thread Thrd;
            public DecThreadNM(string name, int n)
            {
                Thrd = new Thread(this.Run);
                num = n;
                Thrd.Name = name;
                Thrd.Start();
            }

            void Run()
            {
                Console.WriteLine(Thrd.Name + " wait mutex.");
                Console.WriteLine(Thrd.Name + " getting mutex.");
                do
                {
                    Thread.Sleep(100);
                    SharedRes.Count--;
                    Console.WriteLine("In stream " + Thrd.Name + ", SharedRes.Count = " + SharedRes.Count);
                    num--;
                } while (num > 0);

                Console.WriteLine(Thrd.Name + "release mutex.");
            }
        }

        static void Main(string[] args)
        {
            IncThread mt1 = new IncThread("IncStream", 5);
            DecThread mt2 = new DecThread("DecStream", 5);
            mt1.Thrd.Join();
            mt1.Thrd.Join();
            Console.WriteLine("Threads done");

            IncThreadNM Nmt1 = new IncThreadNM("IncStreamNM", 5);
            DecThreadNM Nmt2 = new DecThreadNM("DecStreamNM", 5);
            mt1.Thrd.Join();
            mt1.Thrd.Join();
            Console.WriteLine("NMThreads done");
        }
    }
}
