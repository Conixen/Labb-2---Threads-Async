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
        public TimeSpan FinishTime { get; private set; }
        public static Ships FirstToFinish = null;
        public Stopwatch RaceStopwatch { get; set; }

        private static readonly Random random = new Random();
        private static readonly object consoleLock = new object();

        public Ships(string shipName, int speed)
        {
            ShipName = shipName;
            Distance = 0;
            Speed = speed;
            GoalReached = false;
            RaceStopwatch = new Stopwatch();
        }
        public void Drive()
        {
            int time = 0;

            while (Distance < 10000)
            {
                Thread.Sleep(1000);
                Distance += Speed;

                time++;
                if (time % 10 == 0)
                {
                    RandomObstacles();
                }
            }

            GoalReached = true;
            FinishTime = RaceStopwatch.Elapsed;

            lock (consoleLock)
            {
                if (FirstToFinish == null)
                {
                    FirstToFinish = this;
                    Console.WriteLine($"{ShipName} vann racet! Tid: {FinishTime.TotalSeconds} sekunder.");
                }
                else
                {
                    Console.WriteLine($"{ShipName} har gått i mål! Tid: {FinishTime.TotalSeconds} sekunder.");
                }
            }
        }
        public void RandomObstacles()
        {
            int chance = random.Next(1, 51);

            lock (consoleLock)
            {
                if (chance == 1)
                {
                    Console.WriteLine($"{ShipName} har slut på bränsle! Omriktar till närmaste bränslestation. Väntar i 15 sekunder.");
                    Thread.Sleep(15000);
                }
                else if (chance <= 3)
                {
                    Console.WriteLine($"{ShipName} tog skada av en asteroid! Reparerar skrov i 10 sekunder.");
                    Thread.Sleep(10000);
                }
                else if (chance <= 8)
                {
                    Console.WriteLine($"{ShipName} blev attackerad av rymdflugor! Spolar cockpit i 5 sekunder.");
                    Thread.Sleep(5000);
                }
                else if (chance <= 18)
                {
                    Speed = Speed - 1; 
                    Console.WriteLine($"{ShipName} fick motorproblem! Hastighet sänkt till {Speed} km/h.");
                }
            }
        }
    }
}
