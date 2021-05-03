using HandControl.Model.Enums;

namespace HandControl.Model.Dto
{
    public class SetSettingsDto
    {
        public bool EnableEmg { get; set; }
        public bool EnableDisplay { get; set; }
        public bool EnableGyro { get; set; }
        public bool EnableDriver { get; set; }
        public bool PowerOff { get; set; }
    }
}
