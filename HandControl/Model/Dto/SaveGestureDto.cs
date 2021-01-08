using System;

namespace HandControl.Model.Dto
{
    public class SaveGesturesDto
    {
        public DateTime TimeSync { get; set; }
        public GestureDto Gesture { get; set; }
    }
}
