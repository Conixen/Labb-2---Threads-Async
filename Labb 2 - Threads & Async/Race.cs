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
            Console.WriteLine("1...");
            Thread.Sleep(1000);
            Console.WriteLine("KÖR!!!!");
            Console.WriteLine("Tryck ENTER för att få uppdateringar :)");

            List<Thread> threads = new List<Thread>();
            Stopwatch stopwatch = Stopwatch.StartNew(); // Start the stopwatch

            foreach (var ship in ships) 
            {
                ship.RaceStopwatch = stopwatch; // Assign the stopwatch to each ship
                Thread thread = new Thread(ship.Drive); // Create a new thread for each ship
                threads.Add(thread);
                thread.Start();
            }

            Thread statusThread = new Thread(() => ListenForStatus(ships));
            statusThread.Start();

            foreach (var thread in threads) // waitng for all ships to finish
            {
                thread.Join();
            }

            // Once all threads are finished, show final message
            Console.WriteLine("Alla Star-Trek skepp har gått i mål!");
            Console.WriteLine("Programmet kommer att stängs av!");
            Thread.Sleep(3000); 
            Environment.Exit(0); 
        }

        private static void ListenForStatus(List<Ships> ships)  // method to show status of ships
        {
            while (true)
            {
                string input = Console.ReadLine()?.ToLower();

                if (input == "")
                {
                    Console.WriteLine("\nSTATUS UPPDATERING:");
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
