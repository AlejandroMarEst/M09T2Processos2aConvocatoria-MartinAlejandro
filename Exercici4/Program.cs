using System;
using System.Diagnostics;
using System.IO;

class Program
{
    static void Main()
    {
        string filePath = "../../../ProcessesAndThreads.txt";
        int totalProcesses = 0;
        int totalThreads = 0;

        using (StreamWriter writer = new StreamWriter(filePath))
        {
            Process[] processes = Process.GetProcesses();
            totalProcesses = processes.Length;

            foreach (Process process in processes)
            {
                try
                {
                    writer.WriteLine($"Process: {process.ProcessName} (PID: {process.Id})");

                    ProcessThreadCollection threads = process.Threads;
                    totalThreads += threads.Count;

                    foreach (ProcessThread thread in threads)
                    {
                        writer.WriteLine($"\tThread ID: {thread.Id}");
                    }
                }
                catch (Exception ex)
                {
                    writer.WriteLine($"\tCannot access process: {ex.Message}");
                }
            }
            writer.WriteLine();
            writer.WriteLine($"Total processes: {totalProcesses}");
            writer.WriteLine($"Total threads: {totalThreads}");
        }
        Console.WriteLine($"Information saved to file: {filePath}");
    }
}
