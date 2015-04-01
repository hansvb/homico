using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homico
{
    class TheCommitterConfiguration
    {
        public Signature Signature
        {
            get
            {
                return new Signature("HowMICoding", "@HoMICo", DateTime.Now);
            }
        }
        public string InitialCommitMessage = "HowMICoding repo initial commit";
        public string GitIgnoreContents = @"
# owMICoding ignores
NativeBinaries/
LibGit2Sharp.dll
HowMICoding.exe
*.TMP
*.qld
*.*ql
*.fip
#ignore thumbnails created by windows
Thumbs.db
#Ignore files build by Visual Studio
*.obj
*.exe
*.pdb
*.user
*.aps
*.pch
*.vspscc
*_i.c
*_p.c
*.ncb
*.suo
*.tlb
*.tlh
*.bak
*.cache
*.ilk
*.log
[Bb]in
[Dd]ebug*/
*.lib
*.sbr
obj/
[Rr]elease*/
_ReSharper*/
[Tt]est[Rr]esult*
_NCrunch*
*.ncrunch*
*.lvu
*.ixd
Nuget/*.nupkg
NuGet/**/**/**/*.dll
NuGet/**/**/**/*.xml
";

    }
}
