using System;
using System.IO;
using System.Collections.Generic;

public enum FileSystemState
{
    INITIAL,
    CORRECT,
    OTHER
}

public class FileSystemUtils
{
    public List<string> FileSystemEntries;

    // THIS MUST BE CHANGED BEFORE USING - HARD CODE.
    string CorrectState = 
    @"ROOT\Changelog.txt
ROOT\SYS
ROOT\SYS\DRI
ROOT\SYS\LANG
ROOT\SYS\NET
ROOT\SYS\SEC
ROOT\SYS\STARTUP
ROOT\SYS\SYSMAN
ROOT\SYS\DRI\DOORADVDISP.DRI
ROOT\SYS\DRI\DOORBASICDISP.DRI
ROOT\SYS\DRI\DOORKB.DRI
ROOT\SYS\DRI\DOORMSE.DRI
ROOT\SYS\LANG\EN.LP
ROOT\SYS\LANG\EO.LP
ROOT\SYS\LANG\IG.LP
ROOT\SYS\LANG\IT.LP
ROOT\SYS\LANG\JP.LP
ROOT\SYS\LANG\RU.LP
ROOT\SYS\LANG\SR.LP
ROOT\SYS\LANG\XH.LP
ROOT\SYS\NET\HOSTS.CONF
ROOT\SYS\NET\NETMAN.EXE
ROOT\SYS\SEC\SECMAN.EXE
ROOT\SYS\SEC\SECUI.DLL
ROOT\SYS\STARTUP\SYSMANSTARTUP.LINK
ROOT\SYS\SYSMAN\SYSMAN.EXE
";

    // THIS MUST BE CHANGED BEFORE USING - HARD CODE.
    public string InitialState = 
    @"ROOT\Changelog.txt
ROOT\SYS
ROOT\SYS\DOORADVDISP.DRI
ROOT\SYS\DRI
ROOT\SYS\EO.LP
ROOT\SYS\HOSTS.CONF
ROOT\SYS\IG.LP
ROOT\SYS\IT.LP
ROOT\SYS\JP.LP
ROOT\SYS\LANG
ROOT\SYS\NETMAN.EXE
ROOT\SYS\RU.LP
ROOT\SYS\SECMAN.EXE
ROOT\SYS\SECUI.DLL
ROOT\SYS\SR.LP
ROOT\SYS\SYSMAN.EXE
ROOT\SYS\SYSMANSTARTUP.LINK
ROOT\SYS\XH.LP
ROOT\SYS\DRI\DOORBASICDISP.DRI
ROOT\SYS\DRI\DOORKB.DRI
ROOT\SYS\DRI\DOORMSE.DRI
ROOT\SYS\LANG\EN.LP
";

    public FileSystemUtils(string root)
    {
        FileSystemEntries = new List<string>(Directory.EnumerateFileSystemEntries(root, "*", SearchOption.AllDirectories));
    }

    public FileSystemState CheckFileSystem()
    {
        //Console.WriteLine("this got called");
        bool notCorrect = false;
        bool notInitial = false;

        foreach (string entry in FileSystemEntries)
        {
            if (entry.ToUpper().Contains(".LOG")) continue;
            if (!CorrectState.Contains(entry)) notCorrect = true;
            if (!InitialState.Contains(entry)) notInitial = true;
        }

        if (notCorrect && notInitial) return FileSystemState.OTHER;
        if (notCorrect) return FileSystemState.INITIAL;
        if (notInitial) return FileSystemState.CORRECT;

        return FileSystemState.OTHER;
    }

    public static string GenerateState(string root)
    {
        List<string> fileSystemEntries = new List<string>(Directory.EnumerateFileSystemEntries(root, "*", SearchOption.AllDirectories));
        string state = "";

        foreach (string entry in fileSystemEntries)
        {
            if (entry.ToUpper().Contains(".LOG")) continue;
            state += entry + "\n";
        }

        return state;
    }
}