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
        public static async Task StartRaceAsync(List<Ships> ships)
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

            List<Task> shipTasks = new List<Task>();
            Stopwatch stopwatch = Stopwatch.StartNew(); // Start the stopwatch

            foreach (var ship in ships)
            {
                ship.RaceStopwatch = stopwatch;
                shipTasks.Add(Task.Run(() => ship.DriveAsync()));  // start the driveAsynce method for each ship
            }
            Task statusTask = Task.Run(() => ListenForStatus(ships));

            await Task.WhenAll(shipTasks);  // waitng for all ships to finish

            // Once all threads are finished, show final message
            Console.WriteLine("\nAlla Star-Trek skepp har gått i mål!");
            Console.WriteLine("Programmet kommer att stängs av!");
            Thread.Sleep(4000); 
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
