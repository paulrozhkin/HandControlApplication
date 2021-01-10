using HandControl.Model.Enums;

namespace HandControl.Model.Dto
{
    public class GetSettingsDto
    {
        public ModeType TypeWork { get; set; }
        public bool EnableEmg { get; set; }
        public bool EnableDisplay { get; set; }
        public bool EnableGyro { get; set; }
        public bool EnableDriver { get; set; }
    }
}
