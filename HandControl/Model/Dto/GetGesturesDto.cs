using System;
using System.Collections.Generic;

namespace HandControl.Model.Dto
{
    public class GetGesturesDto
    {
        public DateTime LastTimeSync { get; set; }
        public IEnumerable<GestureDto> Gestures { get; set; }
    }
}
