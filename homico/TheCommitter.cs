using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homico
{
    class TheCommitter
    {
        IRepository repo;
        TheCommitterConfiguration config = new TheCommitterConfiguration();
        LogbookWriter logbookWriter;

        public TheCommitter()
        {
            CreateOrOpenRepo();

            logbookWriter = new LogbookWriter(repo);
            logbookWriter.CodingStarted();

            TheCommitterTools.PrintStats(repo);

            Start();

            Console.WriteLine("Watching....");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Press any key to stop!");
            Console.ResetColor();
            Console.ReadKey();

            logbookWriter.CodingEnded();
        }

        private void CreateOrOpenRepo()
        {
            string repoDir = TheCommitterTools.DiscoverRepoDir();
                        
            if(repoDir == null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Creating new HoMICo repo!");
                Console.ResetColor();
                repoDir = ".";
                Repository.Init(repoDir);
                repo = new Repository(repoDir);
                DoInitialCommit();
            }
            else
            {
                repo = new Repository(repoDir);

                Console.WriteLine("A repo was found in [{0}].", repoDir);
                Console.Write("Checking if it's a HoMICo repo... ");
                if (IsHomicoRepo(repoDir))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Yes!");
                    Console.ResetColor();
                    Console.WriteLine("Using [{0}]...", repoDir);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("It is not!");
                    Console.ResetColor();
                    TheCommitterTools.ExitGracefully();
                }
            }
        }

        private void DoInitialCommit()
        {
            var now = DateTime.UtcNow;
            var f = new StreamWriter("README.md");
            f.WriteLine("This HoMICo repo was created on {0}.", now.ToString());
            f.Close();

            File.WriteAllText(".gitignore", config.GitIgnoreContents);

            Signature author = config.Signature;
            Signature committer = author;

            repo.Index.Add("README.md");
            repo.Index.Add(".gitignore");
            repo.Commit(config.InitialCommitMessage, author, committer);
        }

        private bool IsHomicoRepo(string path)
        {
            repo = new Repository(path);
            if (repo.Commits.Count() > 0 && repo.Commits.Last().Message.Trim() == config.InitialCommitMessage)
            {
                return true;
            }
            return false;
        }

        void OnFileSave(Action<string> fileSaveAction)
        {
            var w = new FileSystemWatcher();

            w.Path = ".";
            w.Filter = "*";
            w.IncludeSubdirectories = true;
            w.Changed += (object sender, FileSystemEventArgs e) =>
            {
                string diskObject = e.FullPath.Substring(2); // remove "./"-prefix

                bool hasToBeIgnoredAndIsNotFile = false

                    // Ignore changes in `.git` (and other files/dirs starting with `.`)
                    || Path.GetFileName(diskObject).StartsWith(".")
                    || Path.GetDirectoryName(diskObject).StartsWith(".")

                    // When a file in a subdirectory is saved, this fires a changed-event on the directory.
                    // We don't need that event. We don't need directories.
                    || Directory.Exists(diskObject);

                if (!hasToBeIgnoredAndIsNotFile)
                {
                    if (File.Exists(diskObject)) // 90% of the time VS tmp files are already deleted by now!
                    {
                        bool hasToBeIgnored = diskObject.Contains("~");

                        if (!hasToBeIgnored)
                        {
                            fileSaveAction(diskObject);
                        }
                    }
                }
            };

            w.EnableRaisingEvents = true;
        }

        private void Start()
        {
            OnFileSave(file =>
            {
                Console.WriteLine("Save detected: [{0}].", file);

                Signature author = config.Signature;
                Signature committer = author;

                string msg = String.Format("[{0}] saved.", file);

                // gitIgnore should (git-magically) ignore the rest of the files
                // when there is no difference between the file (slipped through the nets) then this is a no-op
                repo.Index.Add(file);

                if (repo.Diff.Compare<TreeChanges>(repo.Head.Tip.Tree, DiffTargets.Index).Count() > 0) // otherwise commit throws exception
                {
                    repo.Commit(msg, author, committer);
                    Console.WriteLine(msg);
                }

            });
        }

    }
}