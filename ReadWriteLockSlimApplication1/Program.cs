using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReadWriteLockSlimApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var locker = new ReaderWriterLockSlim();

            var message = new List<String>();

            // writer
            Parallel.For(0, 10, i=>
                Task.Factory.StartNew(() => {

                    var s = Convert.ToString(i);

                    while (true)
                    {
                        locker.EnterWriteLock();
                        message.Add(s);
                        Thread.Sleep(50);   // writer block check
                        locker.ExitWriteLock();
                        //Thread.Sleep(50);   // multi read check
                    }
            
                })
            );

            // Reader
            Task.Factory.StartNew(() => {

                while (true)
                {
                    locker.EnterReadLock();
                    Console.WriteLine(message.Count);
                    Thread.Sleep(10);
                    locker.ExitReadLock();
                }
            });


            while (true)
            {
                locker.EnterReadLock();
                Console.WriteLine(message.Count);
                Thread.Sleep(10);
                locker.ExitReadLock();

            }

        }
    }
}
