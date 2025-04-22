using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Labb_2___Threads___Async
{
    public class Race
    {
        //private static List<Ships> ships = new List<Ships>();

        public static void StartRace(List<Ships> ships)
        {
            Console.WriteLine("Välkommen till Star Trek Podracing!" +
                "\nRacet börjar om...");
            Thread.Sleep(2000);
            Console.WriteLine("3...");
            Thread.Sleep(2000);
            Console.WriteLine("2...");
            Thread.Sleep(2000);
            Console.WriteLine("1...Kör");
            Console.WriteLine("Tryck ENTER för att få uppdateringar :)");

            List<Thread> threads = new List<Thread>();

            foreach (var ship in ships) 
            { 
                Thread thread = new Thread(ship.Drive);
                threads.Add(thread);
                thread.Start();
            }

            Thread statusThread = new Thread(() => ListenForStatus(ships));
            statusThread.Start();

            foreach (var thread in threads)
            {
                thread.Join();
            }
            Console.WriteLine("Alla Star-Trek skepp har gått i mål!");
        }

        private static void ListenForStatus(List<Ships> ships)
        {
            while (true)
            {
                string input = Console.ReadLine()?.ToLower();

                if (input == "")
                {
                    Console.WriteLine("\nSTATUSUPPDATERING:");
                    foreach (var ship in ships)
                    {
                        Console.WriteLine($"{ship.ShipName}: {ship.Distance:F1} m, {ship.Speed:F1} km/h");
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
