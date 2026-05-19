namespace MiniProject
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ShowIntro();
            ManagementApp app = new ManagementApp();
            app.Run();
        }

        static void ShowIntro()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;

            string title = "===  MARKET APP ===";

            foreach (char c in title)
            {
                Console.Write(c);
                Thread.Sleep(40);
            }

            Console.WriteLine("\n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Initializing services...");
            Thread.Sleep(1000);
            Console.ResetColor();
        }
    }


}

