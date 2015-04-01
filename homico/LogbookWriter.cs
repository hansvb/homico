using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homico
{
    class LogbookWriter
    {
        IRepository repo;
        TheCommitterConfiguration config = new TheCommitterConfiguration();

        public LogbookWriter(IRepository repo)
        {
            this.repo = repo;
        }

        public void CodingStarted()
        {
            var msg = String.Format("Coding started [{0}].", DateTime.UtcNow.ToString("s", System.Globalization.CultureInfo.InvariantCulture));

            WriteLogAndCommit(msg);
        }

        public void CodingEnded()
        {
            var msg = String.Format("Coding stopped [{0}].", DateTime.UtcNow.ToString("s", System.Globalization.CultureInfo.InvariantCulture));

            WriteLogAndCommit(msg);
        }

        private void WriteLogAndCommit(string commitMessage)
        {
            var f = new StreamWriter("README.md", true);
            f.WriteLine(commitMessage);
            f.Close();

            Signature author = config.Signature;
            Signature committer = author;
            repo.Index.Add("README.md");
            repo.Commit(commitMessage, author, committer);
        }
    }
}
