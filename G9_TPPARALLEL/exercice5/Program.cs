using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace exercice3
{
    class MainClass
    {
        const int MAX_CHARGE = 15;
        const int MAX_BOUCLE = 100;
        static int Carre(int a)
        {
            //Simule un charge
            Thread.Sleep(MAX_CHARGE);
            return a * a;
        }


        static void Run(Action action, String nomMethode)
        {
            Stopwatch sw = Stopwatch.StartNew();
            Console.WriteLine("Démarrage de la méthode {0}", nomMethode);
            sw.Start();
            //Démarrage de la méthode à tester
            action();
            sw.Stop();
            Console.WriteLine("La méthode {0} : {1} ms", nomMethode, sw.ElapsedMilliseconds);
            Console.WriteLine("==========================================");
            sw.Reset();
        }

        public static void Main(string[] args)
        {
            Run(() =>
            {
                int x = 0;
                for (int i = 0; i < MAX_BOUCLE; i++)
                {
                    x += Carre(42);
                }
                Console.WriteLine("Résultat : {0}", x);
            }, "Boucle Séquentielle");

            Run(() =>
            {
                var partition = Partitioner.Create(0, MAX_BOUCLE,(int)(MAX_BOUCLE /Environment.ProcessorCount)+1);
                int x = 0;
                Parallel.ForEach(partition, (Tuple<int, int> range) => {
                    Console.WriteLine("Range.Item1 : {0} - Range.Item2 : {1} : Tâche #: {2}", range.Item1, range.Item2, Task.CurrentId);
                    for (int i = range.Item1; i < range.Item2; i++){
                        int tempo = Carre(42); 
                        Interlocked.Add(ref x, tempo); 
                    }
                });
                Console.WriteLine("Résultat : {0}", x);
            }, "Boucle Parallèle");


        }
    }
}
