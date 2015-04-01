using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homico
{
    class TheCommitterTools
    {
        static public string DiscoverRepoDir()
        {
            string foundRepo = Repository.Discover(".");

            if (foundRepo == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("No repo found. ");
                Console.ResetColor();
            }

            return foundRepo;
        }
        
        static public void ExitGracefully()
        {
            Console.WriteLine("Don't use HoMICo in existing GIT repos!");
            Console.WriteLine();
            Console.WriteLine("Exiting...");
            Environment.Exit(1);
        }

        static public void PrintStats(IRepository repo)
        {
            Console.WriteLine(" Some stats:");
            Console.WriteLine("     # Commits: " + repo.Commits.Count());
            if (repo.Commits.Count() > 0)
            {
                Console.WriteLine("     Earliest commit: " + repo.Commits.Last().Author.When.ToString());
                Console.WriteLine("     Latest commit: " + repo.Commits.First().Author.When.ToString());
            }
        }

    }
}
