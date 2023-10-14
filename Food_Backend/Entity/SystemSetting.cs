namespace Food_Backend.Entity
{
    public class SystemSetting : BaseEnntity<int>
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public DateTime ShuffleDate { get; set; }
    }
}
