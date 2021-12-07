namespace PlaybackDeviceSwitcher
{
    public class PlayBackDevice
    {
        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public void SetAsDefault()
        {
            PlaybackDeviceSwitcherWrapper.SetDefaultPlayBackDevice(Id);
        }
    }
}
