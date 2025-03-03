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
ROOT\SYS\DRI\DOORADVDISP.DRI
ROOT\SYS\DRI\DOORBASICDISP.DRI
ROOT\SYS\DRI\DOORKB.DRI
ROOT\SYS\DRI\DOORMSE.DRI
ROOT\SYS\LANG\en.lp
ROOT\SYS\LANG\ig.lp
ROOT\SYS\LANG\it.lp
ROOT\SYS\LANG\jp.lp
ROOT\SYS\NET\HOSTS.CONF
ROOT\SYS\NET\NETMAN.EXE
ROOT\SYS\SEC\SECMAN.EXE
ROOT\SYS\SEC\SECUI.DLL";

    // THIS MUST BE CHANGED BEFORE USING - HARD CODE.
    string InitialState = 
    @"ROOT\Changelog.txt
ROOT\SYS
ROOT\SYS\DOORADVDISP.DRI
ROOT\SYS\DOORBASICDISP.DRI
ROOT\SYS\DOORKB.DRI
ROOT\SYS\DOORMSE.DRI
ROOT\SYS\en.lp
ROOT\SYS\HOSTS.CONF
ROOT\SYS\ig.lp
ROOT\SYS\it.lp
ROOT\SYS\jp.lp
ROOT\SYS\NETMAN.EXE
ROOT\SYS\SECMAN.EXE
ROOT\SYS\SECUI.DLL";

    string TestState = "";

    public FileSystemUtils(string root)
    {
        FileSystemEntries = new List<string>(Directory.EnumerateFileSystemEntries(root, "*", SearchOption.AllDirectories));
    }

    public FileSystemState CheckFileSystem()
    {
        foreach (string entry in FileSystemEntries)
        {
            if (entry.ToUpper().Contains(".LOG")) continue;
            TestState += entry + "\n";
        }

        if (TestState == CorrectState)
        {
            return FileSystemState.CORRECT;
        }
        else if (TestState == InitialState) 
        {
            return FileSystemState.INITIAL;
        }
        else
        {
            return FileSystemState.OTHER;
        }
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