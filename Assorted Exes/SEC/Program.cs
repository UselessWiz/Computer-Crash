using System;
using System.IO;
using System.Collections.Generic;

string cwdString = System.IO.Directory.GetCurrentDirectory();

if (new DirectoryInfo(cwdString).Name != "SEC") {
    Console.WriteLine("Critical Component SEC\\SECUI.DLL missing. Exiting.");
    System.Threading.Thread.Sleep(1000);
    Environment.Exit(0);
}
else
{
    List<string> files = new List<string>(Directory.EnumerateFileSystemEntries(cwdString));
    foreach (string file in files) 
    {
        if (file.Contains("SECUI.DLL"))
        {
            Console.WriteLine("Monitoring the status of this device\nThreats found: None\n\nNote: This program is designed to be run by the system, not by regular users.");
            break;
        }
        else if (file == files.Last())
        {
            Console.WriteLine("Critical Component SEC\\SECUI.DLL missing. Exiting.");
            System.Threading.Thread.Sleep(1000);
            Environment.Exit(0);
        }
    }
    
    if (!Directory.Exists($"{cwdString}/../NET")) 
    {
        System.Threading.Thread.Sleep(200);
        Console.WriteLine("\n[CRITICAL] Cannot monitor Network Status");
    }

    Console.ReadLine();
}
