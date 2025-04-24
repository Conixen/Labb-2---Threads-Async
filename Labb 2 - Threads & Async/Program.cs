namespace Labb_2___Threads___Async
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //t1.Name = "X-Wing";
            //t2.Name = "Millenium Falcon";
            //t3.Name = "TIE-Fighter";

            // the contenders for my space race with name and thier start speed
            Ships t1 = new Ships("X-Wing", 120);
            Ships t2 = new Ships("Millenium-Falcon", 120);
            Ships t3 = new Ships("TIE-Fighter", 120);

            List<Ships> ships = new List<Ships>
            {
                t1,
                t2,
                t3
            };

            await Race.StartRaceAsync(ships);
        }
    }
}
