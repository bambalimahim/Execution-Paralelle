using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace exercice6
{
    class MainClass
    {

        const int MAX_CHARGE = 15;

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
            List<Client> listeClients = null; 
            int maxclients = 500;
            Run(() => { 
                listeClients = Client.CreerClients(maxclients);
                Console.WriteLine("Nombre de clients crées : {0} : ", listeClients.Count);
            }, "CreerClients"); 
            Run(() => {
                List<Client> sousListe = new List<Client>(); 
                foreach (Client c in listeClients)
                {
                    if (c.Age >= 65 && c.Age <= 70)
                    { 
                        //Simule une charge
                        Thread.Sleep(MAX_CHARGE);
                        sousListe.Add(c);
                    } 
                }
                Console.WriteLine("{0} clients répondent aux critères de recherches", sousListe.Count );
            }, "Boucle Séquentielle"); 

            Run(() => {
                BlockingCollection<Client> sousListe = new BlockingCollection<Client>();
                Parallel.ForEach (listeClients, (Client c)=> { 
                    if (c.Age >= 65 && c.Age <= 70) {
                        //Simule une charge
                        Thread.Sleep(MAX_CHARGE);
                        sousListe.Add(c);
                    }
                });
                Console.WriteLine("{0} clients répondent aux critères de recherches", sousListe.Count);
            }, "Boucle Parallèle");
            Console.ReadKey();
        }

    }
}
