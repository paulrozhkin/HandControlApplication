using System;
using System.Collections.Generic;

namespace HandControl.Model.Dto
{
    public class GetGesturesDto: IBinarySerialize
    {
        public DateTime LastTimeSync { get; set; }
        public IEnumerable<GestureDto> Gestures { get; set; }

        public byte[] BinarySerialize()
        {
            throw new NotImplementedException();
        }

        public void BinaryDeserialize(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
