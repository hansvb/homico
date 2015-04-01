# Warning

I was hoping to put this code online to get some feedback about the design and get
some code reviews. Unfortunately I there are most likely still some nasty bugs.

It **should** work in simple directories. Less so in IDE-managed directories.



# What?

**homico** = HOw aM I COding : a git-based history-tool for beginning programmers 

It watches a directory for file changes and commits every file change to a
git repository.

Mainly intended for beginning programmers who are not familiar with `git` (yet).



# How?

- It creates a git repository in the current directory (only if none exist
or recognized as HoMICo-repo)
- It watches the filesystem (using `System.IO.FileSystemWatcher`)
- It commits on (roughly) every file save

External dependencies (via NuGet):

- `LibGit2Sharp`



# Why?


## Students can examine their (and each others) history

To avoid "But it worked 5 min. ago!" 
Sometimes students by accident delete good code. 
All is lost then. But not if they can check out the history of their saves!

**Advantage**: Students learn the benefits of version control early on.


## Assessment

The repo can be used to assess students.

**Advantage**: It can be helpful to see the history and tempo in which students
solve exercises.

To improve further, a seperate assessment-viewer should be made.



# Features

- Not many
- Many bugs!
- Student has to start the .exe in the project-directory
- Tries to handle VS temp files *"gracefully"*
- Works best in normal directories, not so much for IDE projects while the IDE is running
- README.md is used as logbook



# Conclusion

`FileSystemWatcher` is a pain to use.



# Usage

Copy `homico.exe`, `LibGit2Sharp.dll` and the directory
`NativeBinaries` to a directory you want to watch. Run `homico`. Start making
changes to your files. When finished, press a key in the `homico`-console.
*Hope it doesn't crash.*



# Tips

You can use these post-build steps to copy files to a test-directory:

```
copy /Y $(TargetPath) c:\homicotest
copy /Y $(TargetDir)LibGit2Sharp.dll c:\homicotest
md c:\homicotest\NativeBinaries
xcopy /S /Y $(TargetDir)NativeBinaries c:\homicotest\NativeBinaries
```

When testing, be prepared for a lot of:

```
del /f /q /s .git
echo test > test.txt
```
