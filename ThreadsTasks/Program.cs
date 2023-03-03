/*
    1. Написать программу, которая сможет замерить время выполнения заданного 
    функционала для 100 потоков и 100 тасок
    2. Нужно добавить в коде переменную, которую будут изменять потоки и таски.
    3. 50 потоков и 50 тасок должны инкрементировать переменную на 1.
    4. 50 потоков и 50 тасок должны инкрементировать переменную на -1.
    5. Необходимо изучить конструкцию lock.
    6. Необходимо проверить как работает приложение с использованием конструкции 
    lock и без нее, а также ответить на вопрос, почему так.
 */

using System;
using System.Diagnostics;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Reflection.Metadata;

namespace TreadsTasks
{
    class Program
    {
        static List<int> changedVariables = new List<int>();

        static void StartThread(int number)
        {
            ThreadStart threadStart = new ThreadStart(() =>
            {
                UInt64 result = 1;
                for (int i = 1; i < 50; i++)
                {
                    result *= (UInt64)i;
                }

                if (number < 50)
                {
                    changedVariables.Add(number);
                }
                else
                {
                    changedVariables.RemoveAt(changedVariables.Count - 1);
                }
                Console.WriteLine($"thread #{number} finished, length of list = {changedVariables.Count}");
            });
            Thread thread = new Thread(threadStart);
            thread.Start();
        }

        static void StartTask(int number)
        {
            Task task = new Task(() =>
            {
                UInt64 result = 1;
                for (int i = 1; i < 50; i++)
                {
                    result *= (UInt64)i;
                }

                if (number < 50)
                {
                    changedVariables.Add(number);
                }
                else
                {
                    changedVariables.RemoveAt(changedVariables.Count - 1);
                }
                Console.WriteLine($"task #{number} finished, length of list = {changedVariables.Count}");
            });
            task.Start();
        }

        /*
         * Threads and tasks finish in random order, 
         * System.ArgumentOutOfRangeException can happen 
         * if decrementing threads/tasks finish before 
         * the incrementing ones
         */
        static void RunWithoutLock()
        {
            for (int i = 0; i < 100; i++)
            {
                StartThread(i);
                StartTask(i);
            }
        }

        /*
         * Threads and tasks run in orderly fashion,
         * new threads/tasks do not start until previous ones
         * finish.
         */
        static void RunWithLock()
        {
            object lockObject = new object();
            for (int i = 0; i < 100; i++)
            {
                lock (lockObject){
                    StartThread(i);
                }
                lock (lockObject)
                {
                    StartTask(i);
                }
            }
        }

        public static void Main(string[] args)
        {
            int command = 0;
            Console.WriteLine("Enter command");
            Console.WriteLine("1 - Run tasks and threads without lock");
            Console.WriteLine("2 - Run tasks and threads with lock");
            command = Convert.ToInt32(Console.ReadLine());
            if (command == 1)
            {
                RunWithoutLock();
            }
            else if(command == 2)
            {
                RunWithLock();
            }
        }
    }
}