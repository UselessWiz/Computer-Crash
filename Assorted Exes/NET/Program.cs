using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Collections.Generic;

string cwdString = System.IO.Directory.GetCurrentDirectory();

Console.WriteLine("Doors OS Network Manager\n");
if (new DirectoryInfo(cwdString).Name == "NET") {
    // https://gist.github.com/BrunoCaimar/8399190 - Thanks Bruno.
    NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
    foreach (NetworkInterface adapter in adapters.Where(a => a.OperationalStatus == OperationalStatus.Up))
    {
        System.Threading.Thread.Sleep(200);
        Console.WriteLine("\nDescription: {0} \nId: {1} \nIsReceiveOnly: {2} \nName: {3} \nNetworkInterfaceType: {4} " +
        "\nOperationalStatus: {5} " +
        "\nSpeed (bits per second): {6} " +
        "\nSpeed (kilobits per second): {7} " +
        "\nSpeed (megabits per second): {8} " +
        "\nSpeed (gigabits per second): {9} " +
        "\nSupportsMulticast: {10}",
        adapter.Description,
        adapter.Id,
        adapter.IsReceiveOnly,
        adapter.Name,
        adapter.NetworkInterfaceType,
        adapter.OperationalStatus,
        adapter.Speed,
        adapter.Speed / 1000,
        adapter.Speed / 1000 / 1000,
        adapter.Speed / 1000 / 1000 / 1000,
        adapter.SupportsMulticast);

        var ipv4Info = adapter.GetIPv4Statistics();
        Console.WriteLine("OutputQueueLength: {0}", ipv4Info.OutputQueueLength);
        Console.WriteLine("BytesReceived: {0}", ipv4Info.BytesReceived);
        Console.WriteLine("BytesSent: {0}", ipv4Info.BytesSent);

        if (adapter.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
        adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
        {
            Console.WriteLine("*** Ethernet or WiFi Network - Speed (bits per seconde): {0}", adapter.Speed);
        }
    }
    Console.WriteLine();
    Console.WriteLine("\nNote: This program is designed to be run by the system, not by regular users.");
    Console.ReadKey();
}
else
{
    Console.WriteLine($"An unexpected error has occured while running NETMAN.EXE. Check log file at {cwdString}\\NETMAN.LOG. Exiting.");
    File.WriteAllText($"{cwdString}\\NETMAN.LOG.txt", 
@"[DEBUG]    0.000   NETMAN started in user mode.
[DEBUG]    0.022   Preparing ROOT\\SYS\\NET directory.
[CRITICAL] 0.025   NETMAN ran from illegal location.
[ALERT]    0.030   NETMAN exception handler started.
[DEBUG]    0.058   Output logs to this log file.
[ALERT]    1.031   NETMAN shutting down.");
    
    //File.Move($"{cwdString}\\NETMAN.LOG.txt",$"{cwdString}\\NET\\NETMAN.LOG"); 
    //File.Delete($"{cwdString}\\NETMAN.LOG.txt"); 
    
    System.Threading.Thread.Sleep(10000);
    Environment.Exit(0);
}