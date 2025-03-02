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
    string CorrectState = @"";

    // THIS MUST BE CHANGED BEFORE USING - HARD CODE.
    string InitialState = @"";

    string TestState = "";

    public FileSystemUtils(string root)
    {
        FileSystemEntries = new List<string>(Directory.EnumerateFileSystemEntries(root, "*", SearchOption.AllDirectories));
    }

    public FileSystemState CheckFileSystem()
    {
        foreach (string entry in FileSystemEntries)
        {
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
}