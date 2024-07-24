using DbUp;
using System.Reflection;

namespace DbUpgrader
{
    internal class Program
    {
        static int Main(string[] args)
        {

            var connectionString =
                args.FirstOrDefault()
<<<<<<< HEAD
                ?? "data source=DESKTOP-MMDKBJP\\SQLEXPRESS;initial catalog=CentricSummerPractice;trusted_connection=true;TrustServerCertificate=True;";
=======
                ?? "data source=DESKTOP-J4EKKM8\\SQLEXPRESS;initial catalog=CentricSummerPractice;trusted_connection=true;TrustServerCertificate=True;";
>>>>>>> main
            EnsureDatabase.For.SqlDatabase(connectionString);

            var upgrader =
                DeployChanges.To
                    .SqlDatabase(connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .LogToConsole()
                    .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
                #if DEBUG
                Console.ReadLine();
                #endif
                return -1;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
            return 0;
        }
    }
}
