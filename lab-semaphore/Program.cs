using System;
using System.Threading;

namespace lab_semaphore
{
    class Program
    {
        class MyThread
        {
            public Thread Thrd;
            static Semaphore sem = new Semaphore(2, 2);

            public MyThread(string name)
            {
                Thrd = new Thread(this.Run);
                Thrd.Name = name;
                Thrd.Start();
            }

            void Run()
            {
                Console.WriteLine(Thrd.Name + "wait to accept.");
                sem.WaitOne();
                Console.WriteLine(Thrd.Name + "take acception.");
                for(char ch='A';ch<'D';ch++)
                {
                    Console.WriteLine(Thrd.Name + " : " + ch + " ");
                    Thread.Sleep(500);
                }
                Console.WriteLine(Thrd.Name + "retrun acception");
                sem.Release();
            }
        }

        class MyThreadNS
        {
            public Thread Thrd;


            public MyThreadNS(string name)
            {
                Thrd = new Thread(this.Run);
                Thrd.Name = name;
                Thrd.Start();
            }

            void Run()
            {
                Console.WriteLine(Thrd.Name + "wait to accept.");
                Console.WriteLine(Thrd.Name + "take acception.");
                for (char ch = 'A'; ch < 'D'; ch++)
                {
                    Console.WriteLine(Thrd.Name + " : " + ch + " ");
                    Thread.Sleep(500);
                }
                Console.WriteLine(Thrd.Name + "retrun acception");
            }
        }

        static void Main(string[] args)
        {
            MyThread mt1 = new MyThread("Stream1");
            MyThread mt2= new MyThread("Stream2");
            MyThread mt3= new MyThread("Stream3");
            mt1.Thrd.Join();
            mt2.Thrd.Join();
            mt3.Thrd.Join();
            Console.WriteLine("Threads done\n\n");
            MyThreadNS mtNS1 = new MyThreadNS("StreamNS1");
            MyThreadNS mtNS2 = new MyThreadNS("StreamNS2");
            MyThreadNS mtNS3 = new MyThreadNS("StreamNS3");
            mtNS1.Thrd.Join();
            mtNS2.Thrd.Join();
            mtNS3.Thrd.Join();
            Console.WriteLine("ThreadsNS done");
        }
    }
}
