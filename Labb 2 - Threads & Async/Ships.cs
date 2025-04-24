using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Labb_2___Threads___Async
{
    public class Ships
    {
        public string ShipName { get; set; }
        public double Distance { get; set; }
        public int Speed { get; set; }
        public bool GoalReached { get; set; }
        public TimeSpan FinishTime { get; set; }
        public static Ships FirstToFinish = null;   // First ship winner winner chicken dinner
        public Stopwatch RaceStopwatch { get; set; } 

        private static readonly Random random = new Random();   
        private static readonly object consoleLock = new object();  // Lock object for console output

        public Ships(string shipName, int speed)
        {
            ShipName = shipName;
            Distance = 0;
            Speed = speed;
            GoalReached = false;
            // RaceStopwatch = new Stopwatch();
        }
        public async Task DriveAsync()
        {
            int time = 0;

            while (Distance < 10000)    // Track distance 10 km
            {
                await Task.Delay(1000);
                Distance += Speed;  // move the ship forward with current speed

                time++;
                if (time % 10 == 0) // every 10 seconds checks for random obstacles
                {
                    await RandomObstaclesAsync();
                }
            }

            GoalReached = true;
            FinishTime = RaceStopwatch.Elapsed; // save the finish time

            lock (consoleLock)  // one thread at a time
            {
                if (FirstToFinish == null) // if no ship has finnished yet, they are the first ship
                {
                    FirstToFinish = this;
                    Console.WriteLine($"\n{ShipName} vann racet! Tid: {FinishTime.TotalSeconds:F2} sekunder.");
                }
                else    // if they are second or last place
                {
                    Console.WriteLine($"\n{ShipName} har gått i mål! Tid: {FinishTime.TotalSeconds:F2} sekunder.");
                }
            }

        }
        public async Task RandomObstaclesAsync()   // method to simulate random obstacles
        {
            int chance = random.Next(1, 51);    // random number between 1 and 50

            lock (consoleLock)
            {
                if (chance == 1)    // 1/50 = 2%
                {
                    Console.WriteLine($"\n{ShipName} har slut på bränsle! Omriktar till närmaste bränslestation. Väntar i 15 sekunder.");
                }
                else if (chance >= 2 && chance <= 3)       // 2/50 = 4% 
                {
                    Console.WriteLine($"\n{ShipName} tog skada av en asteroid! Reparerar skrov i 10 sekunder.");
                }
                else if (chance >= 4 && chance <= 9)       // 5/50 = 10 %
                {
                    Console.WriteLine($"\n{ShipName} blev attackerad av rymdflugor! Spolar cockpiten i 5 sekunder.");
                }
                else if (chance <= 10 && chance <= 19)  // 10/50 = 20%
                {
                    Speed = Speed - 1; 
                    Console.WriteLine($"\n{ShipName} fick motorproblem! Hastighet sänkt till {Speed} km/h.");
                    return;
                }
            }
            if (chance == 1)        // asynce
                await Task.Delay(15000);
            else if (chance <= 3)
                await Task.Delay(10000);
            else if (chance <= 9)
                await Task.Delay(5000);
        }
    }
}
