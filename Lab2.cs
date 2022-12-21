using System;
using System.Threading; 

namespace Lab2_Vargan
{
    class number
    {
        private double _mynum;
        public double mynum
        {
            get
            {
                return _mynum;
            }
            set
            {
                _mynum = value; 
            }
        }
    }
    class ClassMain
    {
        private static Random rand = new Random();
        private static Mutex mymutex = new Mutex();

        public static double Randnum()
        {
            return rand.NextDouble()*rand.Next(1001);
        }

        public static void Add(number num1, number num2, int n)
        {
            mymutex.WaitOne();
            try
            {
                for (int i = 0; i < n; i++)
                    num1.mynum = num1.mynum + Randnum();
                for (int i = 0; i < n; i++)
                    num2.mynum = num2.mynum + Randnum();
            }
            finally
            {
                mymutex.ReleaseMutex();
            }
        }

        public static void Main()
        {
            var num1 = new number();
            var num2 = new number();

            num1.mynum = 0;
            num2.mynum = 0;

            int n = rand.Next(10, 21);
            int n1 = rand.Next(10000, 20001);
            int n2 = rand.Next(10000, 20001);

            WaitHandle[] waitHandles = new WaitHandle[n];

            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            for(int i = 0; i < n; i++)
            {
                var handle = new EventWaitHandle(false, EventResetMode.ManualReset);
                if (i < n/2)
                {
                    var thread = new Thread(() =>
                    {
                        Add(num1, num2, n1);
                        handle.Set();
                    });
                    thread.Start();
                }
                else
                {
                    var thread = new Thread(() =>
                    {
                        Add(num2, num1, n2);
                        handle.Set();
                    });
                    thread.Start();
                }
                waitHandles[i] = handle;
            }

            WaitHandle.WaitAll(waitHandles);

            watch.Stop();

            Console.WriteLine(num1.mynum);
            Console.WriteLine(num2.mynum);
            Console.WriteLine("Time to execute:" + watch.ElapsedMilliseconds + "ms");
        }
    }
}

