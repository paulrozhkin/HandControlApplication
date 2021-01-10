using System;

namespace HandControl.Model.Dto
{
    public class SaveGestureDto
    {
        public DateTime TimeSync { get; set; }
        public GestureDto Gesture { get; set; }
    }
}
