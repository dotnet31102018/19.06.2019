using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _1906_
{
    class Program
    {
        static void Main(string[] args)
        {
            
            // Task.Run .NET 4.5
            // must work from ThreadPool
            Task t = Task.Run(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine("Hello from Task runner.........");
            });
            t.Wait();


            Task.Factory.StartNew(() =>
                           {
                               Thread.Sleep(1000);
                               Console.WriteLine("Hello from Task Factory.........");
                           }, TaskCreationOptions.LongRunning).Wait();
             

            Task t3 = Task.Run(() =>
            {
                Console.WriteLine("Starting runner.....");
                Thread.Sleep(1000);
                Console.WriteLine("Hello from Task runner.........");
                Thread.Sleep(1000);
                Console.WriteLine("Done!!");
            });

            while (!t3.IsCompleted)
            {
                Thread.Sleep(10);
                Console.Write(".");
            }

            Task.Run<int>(() =>
            {
                Console.WriteLine($"Starting task ....{Thread.CurrentThread.ManagedThreadId}");
                Thread.Sleep(1000);
                Console.WriteLine($"Task completed ....{Thread.CurrentThread.ManagedThreadId}");
                return 5;
                
            }).ContinueWith((Task<int> antecendent) =>
            {
                Console.WriteLine("Task child");
                Console.WriteLine($"Task completed ....{Thread.CurrentThread.ManagedThreadId}");
                //Console.WriteLine(antecendent.Result);
                return 6;
            }).ContinueWith((Task<int> antecendent) =>
            {
                Console.WriteLine("Task child");
                Console.WriteLine($"Task completed ....{Thread.CurrentThread.ManagedThreadId}");
                Console.WriteLine(antecendent.Result);
            });
            

                Task tp = Task.Run(() =>
                {
                    Console.WriteLine($"Starting task ....{Thread.CurrentThread.ManagedThreadId}");
                    Thread.Sleep(1000);
                    Console.WriteLine($"Task completed ....{Thread.CurrentThread.ManagedThreadId}");
                    int a = 5;
                    int b = 0;
                    int c = a / b;
                    //return 5;

                }).ContinueWith((Task antecendent) =>
                {
                    try
                    {
                        Console.WriteLine("Task child");
                        Console.WriteLine($"Task completed ....{Thread.CurrentThread.ManagedThreadId}");
                        //Console.WriteLine(antecendent.Result);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                   // return 6;
                }).ContinueWith((Task antecendent) =>
                {
                    //try
                    //{
                    try
                    {
                        Console.WriteLine("Task child");
                        Console.WriteLine($"Task completed ....{Thread.CurrentThread.ManagedThreadId}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex);
                    }
                    //Console.WriteLine(antecendent.Result);
                    //}
                    //catch (Exception ex)
                    //{
                    //    Console.WriteLine(ex);
                    //}
                    // return 6;
                });


            Thread t33 = new Thread(() =>
            {
                int a = 5;
                int b = 0;
                int c = a / b;

            });
            t33.IsBackground = true;
            t3.Start();




            Thread.Sleep(10 * 1000);
            Console.WriteLine("Done..............");
            Console.WriteLine(t.IsFaulted);
            Console.WriteLine(t.Status);
            
            
            Task t4 = Task.Run(() =>
            {
                int a = 5;
                int b = 0;
                int c = a / b;
            });
            
               // t4.Wait();
            Console.WriteLine("done.........................");
            
            

            //t4.Wait();
            Thread.Sleep(1000);
            Console.WriteLine(".......................");
            if(t4.IsFaulted)
            {
                Console.WriteLine("faulted.............");
                t4.Exception.Handle(e =>
                {
                    Console.WriteLine("Handler............");
                    Console.WriteLine(e);
                    return true;
                });
            }
            
            // 19.8
            Task.Run(() =>
            {
                int a = 5;
                int b = 0;
                int c = a / b;
            }).ContinueWith((Task antecendent) =>
            {
                Console.WriteLine("is faulted?");
                Console.WriteLine(antecendent.IsFaulted);
                antecendent.Exception.Handle(e =>
                {
                    Console.WriteLine("Handler............");
                    Console.WriteLine(e);
                    return true;
                });
            }, TaskContinuationOptions.OnlyOnFaulted);
            
    
            Thread.Sleep(30 * 1000);
            

            CancellationTokenSource tokenSource = new CancellationTokenSource();

            Task t5 = Task.Run(() =>
            {
                Print(tokenSource);

            });


            
            Console.ReadLine();

            tokenSource.Cancel();

            Console.WriteLine("Done.");
            Thread.Sleep(1000);
            Console.WriteLine(t5.IsFaulted);
            t5.Wait();
            Console.ReadLine();
        }

        private static void Print(CancellationTokenSource tokenSource)
        {
            int i = 0;
            while (i < 1 && !tokenSource.IsCancellationRequested)
            {
                Thread.Sleep(1000);
                Console.WriteLine(i);
                i++;
            }

            //if(tokenSource.IsCancellationRequested)
            //{
            //    throw new OperationCanceledException();    
            //}
            tokenSource.Token.ThrowIfCancellationRequested();
        }
    }
}
