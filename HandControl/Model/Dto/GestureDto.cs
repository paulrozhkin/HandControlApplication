using System;
using System.Collections.Generic;

namespace HandControl.Model.Dto
{
    public class GestureDto: IBinarySerialize
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime LastTimeSync { get; set; }
        public bool Iterable { get; set; }
        public int Repetitions { get; set; }
        public IEnumerable<GestureActionDto> Actions { get; set; }
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
