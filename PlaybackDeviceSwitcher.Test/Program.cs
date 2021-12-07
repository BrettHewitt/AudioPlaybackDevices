using System;
using System.Linq;

namespace PlaybackDeviceSwitcher.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                var outputs = PlaybackDeviceSwitcherWrapper.GetPlayBackDevices();

                Console.WriteLine("Found following outputs:");
                foreach (var output in outputs)
                {
                    Console.WriteLine($"{output.Name} - {output.Id}");
                }

                Console.WriteLine();
                Console.WriteLine("Enter the ID of the playback device to switch to or press \'e\' to exit:");
                var inputId = Console.ReadLine();

                if (inputId.ToLower() == "e")
                {
                    break;
                }

                if (!int.TryParse(inputId, out int id))
                {
                    Console.WriteLine("ID must be a number");
                    continue;
                }

                if (!outputs.Any(x => x.Id == id))
                {
                    Console.WriteLine($"No matching device found with ID: {id}");
                    continue;
                }

                PlaybackDeviceSwitcherWrapper.SetDefaultPlayBackDevice(id);
                Console.WriteLine($"Switched to device {outputs.First(x => x.Id == id).Name}");
            }
        }
    }
}
