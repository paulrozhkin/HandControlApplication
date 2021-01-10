using HandControl.Model.Enums;

namespace HandControl.Model.Dto
{
    public class SetSettingsDto
    {
        public ModeType TypeWork { get; set; }
        public int TelemetryFrequency { get; set; }
        public bool EnableEmg { get; set; }
        public bool EnableDisplay { get; set; }
        public bool EnableGyro { get; set; }
        public bool EnableDriver { get; set; }
        public bool PowerOff { get; set; }
    }
}
