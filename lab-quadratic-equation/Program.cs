using System;
using System.Threading;

namespace lab_quadratic_equation
{
    class Program
    {
        static double a = 1, b = 5, c = 6;
        static double _b, b2, ac4, d, dsqrt, a2, x1, x2;

        static EventWaitHandle wh_B_1 = new AutoResetEvent(false);
        static EventWaitHandle wh_B_2 = new AutoResetEvent(false);
        static EventWaitHandle whB2 = new AutoResetEvent(false);
        static EventWaitHandle wh4AC = new AutoResetEvent(false);
        static EventWaitHandle wh2A_1 = new AutoResetEvent(false);
        static EventWaitHandle wh2A_2 = new AutoResetEvent(false);
        static EventWaitHandle whD = new AutoResetEvent(false);
        static EventWaitHandle whDsqrt_1 = new AutoResetEvent(false);
        static EventWaitHandle whDsqrt_2 = new AutoResetEvent(false);

        static void _B()
        {
            _b = -b;
            wh_B_1.Set();
            wh_B_2.Set();
            Console.WriteLine($"-b= {_b} done");
        }

        static void B2()
        {
            b2 = b*b;
            whB2.Set();
            Console.WriteLine($"b2= {b2} done");
        }

        static void AC4()
        {
            ac4 = a*c*4;
            wh4AC.Set();
            Console.WriteLine($"ac4= {ac4} done");
        }

        static void D()
        {
            whB2.WaitOne();
            wh4AC.WaitOne();
            d = b2 - ac4;
            whD.Set();
            Console.WriteLine($"d= {d} done");
        }
        static void Dsqrt()
        {
            whD.WaitOne();
            dsqrt = Math.Sqrt(d);
            whDsqrt_1.Set();
            whDsqrt_2.Set();
            Console.WriteLine($"dsqrt= {dsqrt} done");
        }
        static void A2()
        {
            a2 = 2 * a;
            wh2A_1.Set();
            wh2A_2.Set();
            Console.WriteLine($"a*2= {a2} done");
        }
        static void X1()
        {
            wh_B_1.WaitOne();
            whDsqrt_1.WaitOne();
            wh2A_1.WaitOne();
            x1 = (_b + dsqrt) / a2;
            Console.WriteLine($"x1= {x1} done");
        }
        static void X2()
        {
            wh_B_2.WaitOne();
            whDsqrt_2.WaitOne();
            wh2A_2.WaitOne();
            x2 = (_b - dsqrt) / a2;
            Console.WriteLine($"x2= {x2} done");
        }


        static void Main(string[] args)
        {
            Thread t1 = new Thread(_B);
            Thread t2 = new Thread(B2);
            Thread t3 = new Thread(AC4);
            Thread t4 = new Thread(D);
            Thread t5 = new Thread(Dsqrt);
            Thread t6 = new Thread(A2);
            Thread t7 = new Thread(X1);
            Thread t8 = new Thread(X2);
            t1.Start();
            t2.Start();
            t3.Start();
            t4.Start();
            t5.Start();
            t6.Start();
            t7.Start();
            t8.Start();
            t8.Join();
            t7.Join();
            Console.WriteLine("x1= "+x1.ToString()+"; x2 = " + x2.ToString());
        }
    }
}
