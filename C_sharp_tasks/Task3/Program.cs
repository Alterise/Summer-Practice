using System;
using System.Collections.Generic;
using System.Threading;

namespace Task3
{
    class Program
    {
        static void Main(string[] args)
        {
            Cache<int> cache = new Cache<int>(TimeSpan.FromSeconds(3), 3);
            
            cache.Save("3", 3);
            cache.Save("2", 2);
            cache.Save("1", 1);
            cache.Save("4", 4);

            try
            {
                cache.Get("3");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine("There is no 3 in cache. It has been rewritten.");
                Console.WriteLine();
            }
            
            try
            {
                cache.Save("4", 4);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("4 already in cache.");
            }

            Console.WriteLine(cache.Get("1"));
            Console.WriteLine(cache.Get("2"));
            Console.WriteLine(cache.Get("4"));
            Console.WriteLine();
            
            Console.WriteLine("Waiting for 3 seconds...");
            Thread.Sleep(3000);
            
            try
            {
                cache.Get("1");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine("There is no 1 in cache. It has been deleted.");
            }
            
            try
            {
                cache.Get("2");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine("There is no 2 in cache. It has been deleted.");
            }
            
            try
            {
                cache.Get("4");
            }
            catch (KeyNotFoundException ex)
            {
                Console.WriteLine("There is no 4 in cache. It has been deleted.");
            }
        }
    }
}