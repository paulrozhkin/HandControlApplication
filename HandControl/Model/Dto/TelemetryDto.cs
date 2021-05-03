using System;
using HandControl.Model.Enums;

namespace HandControl.Model.Dto
{
    public class TelemetryDto
    {
        public ModuleStatus EmgStatus { get; set; }
        public ModuleStatus DisplayStatus { get; set; }

        public ModuleStatus GyroStatus { get; set; }

        public DriverStatus DriverStatus { get; set; }


        public DateTime LastTimeSync { get; set; }
        public int Emg { get; set; }
        public Guid? ExecutableGesture { get; set; }
        public int Power { get; set; }
        public int PointerFingerPosition { get; set; }
        public int MiddleFingerPosition { get; set; }
        public int RingFingerPosition { get; set; }
        public int LittleFingerPosition { get; set; }
        public int ThumbFingerPosition { get; set; }
    }
}
