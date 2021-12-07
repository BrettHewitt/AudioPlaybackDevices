using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace PlaybackDeviceSwitcher
{
    public static class PlaybackDeviceSwitcherWrapper
    {
        [DllImport(@"Dlls\PlaybackDeviceSwitcherNative.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Unicode)]
        static extern IntPtr GetPlaybackDeviceNames(ref int deviceCount);

        [DllImport(@"Dlls\PlaybackDeviceSwitcherNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int SetPlaybackDevice(int deviceNumber);

        [DllImport(@"Dlls\PlaybackDeviceSwitcherNative.dll", CallingConvention = CallingConvention.Cdecl)]
        static extern int ReleaseArrayMemory(IntPtr ptr);

        public static PlayBackDevice[] GetPlayBackDevices()
        {
            string[] strings = null;
            var thread = new Thread(() =>
            {
                var numberOfDevices = 0;
                var data = GetPlaybackDeviceNames(ref numberOfDevices);
                var stringPointers = new IntPtr[numberOfDevices];
                Marshal.Copy(data, stringPointers, 0, numberOfDevices);
                //convert the pointer array into a string array
                strings = new string[numberOfDevices];
                for (int i = 0; i < numberOfDevices; i++)
                {
                    strings[i] = Marshal.PtrToStringUni(stringPointers[i]);
                }

                ReleaseArrayMemory(data);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            var devices = new PlayBackDevice[strings.Length];
            for (var i = 0; i < strings.Length; i++)
            {
                devices[i] = new PlayBackDevice()
                {
                    Id = i,
                    Name = strings[i],
                };
            }
            return devices;
        }

        public static int SetDefaultPlayBackDevice(int deviceId)
        {
            var hResult = -1;
            var thread = new Thread(() =>
            {
                hResult = SetPlaybackDevice(deviceId);
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();

            return hResult;
        }
    }
}
