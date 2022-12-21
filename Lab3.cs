using System;
using System.Threading;

namespace Lab3_Vargan
{
    internal class SeqNumber
    {
        private int _Fvalue;

        private int _n;

        private int _nowValue;

        private ManualResetEvent _mre;

        public int Value
        {
            get
            {
                return _Fvalue;
            }
            set
            {
                _Fvalue = value;
            }
        }

        public int N
        {
            get
            {
                return _n;
            }
            set
            {
                _n = value;
            }
        }

        public int nowValue
        {
            get
            {
                return _nowValue;
            }
            set
            {
                _nowValue = value;
            }
        }

        public ManualResetEvent mre
        {
            get
            {
                return _mre;
            }
            set
            {
                _mre = value;
            }
        }

        public void Next()
        {
            if (_nowValue % 2 == 0)
            {
                _nowValue /= 2;
            }
            else
            {
                _nowValue = 3*_nowValue + 1;
            }
            _n++;
        }
        public void TreadPoolCallBack(Object state)
        {
            while (_nowValue != 1)
            {
                Next();
            }
            _mre.Set();
        }

    }
    class Program
    {
        private static int numbers = 192;

        public static void CreateEl(SeqNumber num, int a)
        {
            num.Value = a;
            num.N = 0;
            num.nowValue = a;
        }
        public static int NumFragments(int n)
        {
            if (n < 64)
            {
                return 1;
            }
            else if (n % 64 != 0)
            {
                return n / 64 + 1;
            }
            else
            {
                return n / 64;
            }
        }

        public static void Main()
        {
            int qsize = 0;

            var numQueue = new Queue<SeqNumber>();

            for (int i = 0; i<numbers; i++)
            {
                SeqNumber e = new SeqNumber();
                CreateEl(e, i+1);
                numQueue.Enqueue(e);
            }

            int pointer = 0;
            bool switcher = true;
            int counter = 1;

            while (pointer != numQueue.Peek().Value)
            {
                if (switcher)
                {
                    pointer = 1;
                    switcher = false;
                }

                if (counter != NumFragments(numbers))
                {
                    qsize = 64;
                }
                else if(counter == 1)
                {
                    qsize = numbers;
                }
                else
                {
                    qsize = numbers %((counter - 1) * 64);
                }

                var done = new ManualResetEvent[qsize];

                for(int i = 0; i < qsize; i++)
                {
                    done[i] = new ManualResetEvent(false);
                    SeqNumber e = numQueue.Dequeue();
                    e.mre = done[i];
                    ThreadPool.QueueUserWorkItem(e.TreadPoolCallBack);
                    numQueue.Enqueue(e);
                }
                WaitHandle.WaitAll(done);
                counter++;
            }

            foreach(var e in numQueue)
            {
                Console.WriteLine("The first value: " + e.Value);
                Console.WriteLine("The number of steps: " +  e.N + "\n");
            }
        }
    }
}
